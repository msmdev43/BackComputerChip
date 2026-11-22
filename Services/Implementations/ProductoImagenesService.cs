using System.Text.Json;
using computerChip.Data;
using computerChip.DTOs.Requests.Productos;
using computerChip.DTOs.Responses.Productos;
using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace computerChip.Services.Implementations
{
    public sealed class ProductoImagenesService : IProductoImagenesService
    {
        private const int MaxImagenes = 8;
        private const long MaxFileSize = 5 * 1024 * 1024;
        private const string PublicFolder = "uploads/productos";

        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public ProductoImagenesService(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public async Task<IReadOnlyList<ProductoImagenResponse>?> GetByProductoAsync(int productoId)
        {
            var exists = await _db.Productos.AnyAsync(p => p.id == productoId);
            if (!exists)
                return null;

            return await _db.ProductosImagenes
                .AsNoTracking()
                .Where(pi => pi.productoId == productoId)
                .Include(pi => pi.Imagenes)
                .OrderBy(pi => pi.orden)
                .ThenBy(pi => pi.imagenId)
                .Select(pi => new ProductoImagenResponse
                {
                    Id = pi.Imagenes.id,
                    Nombre = pi.Imagenes.nombre,
                    Url = pi.Imagenes.url,
                    Orden = pi.orden,
                    Principal = pi.orden == 0
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductoImagenResponse>?> ReplaceAsync(
            int productoId,
            string manifestJson,
            IFormFileCollection files)
        {
            if (string.IsNullOrWhiteSpace(manifestJson))
                throw new ArgumentException("El campo manifest es obligatorio.");

            List<ProductoImagenManifestItem> manifest;
            try
            {
                manifest = JsonSerializer.Deserialize<List<ProductoImagenManifestItem>>(
                    manifestJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            catch (JsonException)
            {
                throw new ArgumentException("El manifest no contiene JSON válido.");
            }

            if (manifest.Count > MaxImagenes)
                throw new ArgumentException($"La galería admite un máximo de {MaxImagenes} imágenes.");

            if (manifest.Any(x => x.Orden < 0))
                throw new ArgumentException("El orden de las imágenes no puede ser negativo.");

            foreach (var item in manifest)
            {
                var tieneId = item.Id.HasValue;
                var tieneArchivo = !string.IsNullOrWhiteSpace(item.Archivo);
                if (tieneId == tieneArchivo)
                    throw new ArgumentException("Cada elemento del manifest debe indicar un id existente o una clave de archivo, pero no ambos.");
            }

            var ids = manifest.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToList();
            if (ids.Count != ids.Distinct().Count())
                throw new ArgumentException("El manifest contiene IDs de imagen duplicados.");

            var fileKeys = manifest.Where(x => !string.IsNullOrWhiteSpace(x.Archivo))
                .Select(x => x.Archivo!)
                .ToList();
            if (fileKeys.Count != fileKeys.Distinct(StringComparer.Ordinal).Count())
                throw new ArgumentException("El manifest contiene claves de archivo duplicadas.");

            var filesByKey = files.ToDictionary(f => f.Name, StringComparer.Ordinal);
            if (fileKeys.Any(key => !filesByKey.ContainsKey(key)))
                throw new ArgumentException("Falta uno o más archivos declarados en el manifest.");

            if (files.Any(f => !fileKeys.Contains(f.Name, StringComparer.Ordinal)))
                throw new ArgumentException("Se recibieron archivos que no están declarados en el manifest.");

            var productExists = await _db.Productos.AnyAsync(p => p.id == productoId);
            if (!productExists)
                return null;

            var newFiles = new List<(string Key, string FileName, string Url)>();
            var writtenPaths = new List<string>();
            var databaseCommitted = false;

            try
            {
                foreach (var key in fileKeys)
                {
                    var file = filesByKey[key];
                    var (extension, bytes) = await ReadAndValidateImageAsync(file);
                    var fileName = $"{Guid.NewGuid():N}{extension}";
                    var directory = GetUploadDirectory();
                    Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, fileName);
                    await File.WriteAllBytesAsync(path, bytes);
                    writtenPaths.Add(path);
                    newFiles.Add((key, fileName, $"/{PublicFolder}/{fileName}"));
                }

                await using var transaction = await _db.Database.BeginTransactionAsync();

                var currentRelations = await _db.ProductosImagenes
                    .Where(pi => pi.productoId == productoId)
                    .Include(pi => pi.Imagenes)
                    .ToListAsync();

                var currentByImageId = currentRelations.ToDictionary(x => x.imagenId);
                var unknownIds = ids.Where(id => !currentByImageId.ContainsKey(id)).ToList();
                if (unknownIds.Count > 0)
                    throw new ArgumentException("Uno o más IDs de imagen no pertenecen a este producto.");

                var newFileByKey = newFiles.ToDictionary(x => x.Key, StringComparer.Ordinal);
                var orderedManifest = manifest
                    .OrderBy(x => x.Orden)
                    .ThenBy(x => manifest.IndexOf(x))
                    .ToList();

                var retainedImageIds = ids.ToHashSet();
                var removedRelations = currentRelations
                    .Where(x => !retainedImageIds.Contains(x.imagenId))
                    .ToList();

                if (removedRelations.Count > 0)
                    _db.ProductosImagenes.RemoveRange(removedRelations);

                for (var index = 0; index < orderedManifest.Count; index++)
                {
                    var item = orderedManifest[index];
                    if (item.Id.HasValue)
                    {
                        currentByImageId[item.Id.Value].orden = index;
                        continue;
                    }

                    var uploaded = newFileByKey[item.Archivo!];
                    var image = new Imagenes
                    {
                        nombre = uploaded.FileName,
                        url = uploaded.Url,
                        createdAt = DateTime.UtcNow,
                        updatedAt = DateTime.UtcNow
                    };
                    _db.Imagenes.Add(image);
                    _db.ProductosImagenes.Add(new ProductosImagenes
                    {
                        productoId = productoId,
                        Imagenes = image,
                        orden = index
                    });
                }

                await _db.SaveChangesAsync();

                var removedImageIds = removedRelations.Select(x => x.imagenId).ToList();
                var orphanImages = removedImageIds.Count == 0
                    ? new List<Imagenes>()
                    : await _db.Imagenes
                        .Where(i => removedImageIds.Contains(i.id) &&
                                    !_db.ProductosImagenes.Any(pi => pi.imagenId == i.id))
                        .ToListAsync();

                if (orphanImages.Count > 0)
                    _db.Imagenes.RemoveRange(orphanImages);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                databaseCommitted = true;

                // La eliminación física se hace después del commit para no perder archivos
                // si la actualización de la base de datos falla.
                foreach (var image in orphanImages)
                    TryDeleteLocalImage(image.url);

                var result = await GetByProductoAsync(productoId);
                return result ?? Array.Empty<ProductoImagenResponse>();
            }
            catch
            {
                if (!databaseCommitted)
                {
                    foreach (var path in writtenPaths)
                    {
                        try
                        {
                            if (File.Exists(path)) File.Delete(path);
                        }
                        catch { /* No ocultar el error original por un fallo de limpieza. */ }
                    }
                }
                throw;
            }
        }

        private static async Task<(string Extension, byte[] Bytes)> ReadAndValidateImageAsync(IFormFile file)
        {
            if (file.Length <= 0)
                throw new ArgumentException("No se permiten archivos vacíos.");
            if (file.Length > MaxFileSize)
                throw new ArgumentException("Cada imagen puede pesar como máximo 5 MB.");

            await using var input = file.OpenReadStream();
            using var buffer = new MemoryStream();
            await input.CopyToAsync(buffer);
            var bytes = buffer.ToArray();

            string? extension = null;
            if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                extension = ".jpg";
            else if (bytes.Length >= 8 && bytes.AsSpan(0, 8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
                extension = ".png";
            else if (bytes.Length >= 12 &&
                     bytes.AsSpan(0, 4).SequenceEqual("RIFF"u8) &&
                     bytes.AsSpan(8, 4).SequenceEqual("WEBP"u8))
                extension = ".webp";

            if (extension == null)
                throw new ArgumentException("Formato inválido. Solo se admiten imágenes JPG, PNG o WebP.");

            return (extension, bytes);
        }

        private string GetUploadDirectory()
        {
            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
            return Path.Combine(webRoot, "uploads", "productos");
        }

        private void TryDeleteLocalImage(string url)
        {
            const string prefix = "/uploads/productos/";
            if (string.IsNullOrWhiteSpace(url) || !url.StartsWith(prefix, StringComparison.Ordinal))
                return;

            var fileName = Path.GetFileName(url);
            if (string.IsNullOrWhiteSpace(fileName) || fileName != url[prefix.Length..])
                return;

            var path = Path.Combine(GetUploadDirectory(), fileName);
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch { /* La limpieza física no debe revertir una galería ya confirmada. */ }
        }
    }
}
