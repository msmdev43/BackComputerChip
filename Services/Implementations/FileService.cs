using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using Microsoft.AspNetCore.WebUtilities;

namespace computerChip.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            _environment = environment;
            _logger = logger;
            AsegurarWebRoot();
        }

        private void AsegurarWebRoot()
        {
            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot"
                );
            }

            if (!Directory.Exists(_environment.WebRootPath))
            {
                Directory.CreateDirectory(_environment.WebRootPath);
                _logger.LogInformation("📁 wwwroot creado en {Path}", _environment.WebRootPath);
            }
        }

        // ============================================
        // MÉTODOS PARA PRODUCTOS
        // ============================================

        public async Task<string> GuardarImagenProductoAsync(IFormFile archivo, int productoId, string nombreProducto)
        {
            AsegurarWebRoot();

            _logger.LogInformation("Guardando imagen para producto ID: {ProductoId}, Nombre: {Nombre}",
                productoId, nombreProducto);

            var carpetaProductos = ObtenerCarpetaProductos();
            var rutaCompleta = Path.Combine(_environment.WebRootPath, carpetaProductos);

            CrearDirectorioSiNoExiste(rutaCompleta);

            var nombreArchivo = $"{LimpiarNombreArchivo(nombreProducto)}_{productoId}.webp";
            var rutaArchivo = Path.Combine(rutaCompleta, nombreArchivo);

            await GuardarImagenOptimizadaAsync(archivo, rutaArchivo);

            var rutaRelativa = Path.Combine(carpetaProductos, nombreArchivo)
                .Replace("\\", "/");

            _logger.LogInformation("✅ Imagen de producto guardada: {Ruta}", rutaRelativa);
            return rutaRelativa;
        }

        public async Task<string> GuardarImagenProductoConTimestampAsync(IFormFile archivo, int productoId, string nombreProducto)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            var nombreLimpio = LimpiarNombreArchivo(nombreProducto);
            var nombreConTimestamp = $"{nombreLimpio}_{productoId}_{timestamp}";

            _logger.LogInformation("Guardando imagen con timestamp para producto ID: {ProductoId}", productoId);

            return await GuardarImagenProductoAsync(archivo, productoId, nombreConTimestamp);
        }

        // ============================================
        // MÉTODOS PARA CATEGORÍAS
        // ============================================

        public async Task<string> GuardarImagenCategoriaAsync(IFormFile archivo, int categoriaId, string nombreCategoria)
        {
            AsegurarWebRoot();

            _logger.LogInformation("Guardando imagen para categoría ID: {CategoriaId}, Nombre: {Nombre}",
                categoriaId, nombreCategoria);

            var carpetaCategorias = ObtenerCarpetaCategorias();
            var rutaCompleta = Path.Combine(_environment.WebRootPath, carpetaCategorias);

            CrearDirectorioSiNoExiste(rutaCompleta);

            var nombreArchivo = $"{LimpiarNombreArchivo(nombreCategoria)}_{categoriaId}.webp";
            var rutaArchivo = Path.Combine(rutaCompleta, nombreArchivo);

            await GuardarImagenOptimizadaAsync(archivo, rutaArchivo);

            var rutaRelativa = Path.Combine(carpetaCategorias, nombreArchivo)
                .Replace("\\", "/");

            _logger.LogInformation("✅ Imagen de categoría guardada: {Ruta}", rutaRelativa);
            return rutaRelativa;
        }

        // ============================================
        // MÉTODOS PARA MARCAS
        // ============================================

        public async Task<string> GuardarImagenMarcaAsync(IFormFile archivo, int marcaId, string nombreMarca)
        {
            AsegurarWebRoot();

            _logger.LogInformation("Guardando imagen para marca ID: {MarcaId}, Nombre: {Nombre}",
                marcaId, nombreMarca);

            var carpetaMarcas = ObtenerCarpetaMarcas();
            var rutaCompleta = Path.Combine(_environment.WebRootPath, carpetaMarcas);

            CrearDirectorioSiNoExiste(rutaCompleta);

            var nombreArchivo = $"{LimpiarNombreArchivo(nombreMarca)}_{marcaId}.webp";
            var rutaArchivo = Path.Combine(rutaCompleta, nombreArchivo);

            await GuardarImagenOptimizadaAsync(archivo, rutaArchivo);

            var rutaRelativa = Path.Combine(carpetaMarcas, nombreArchivo)
                .Replace("\\", "/");

            _logger.LogInformation("✅ Imagen de marca guardada: {Ruta}", rutaRelativa);
            return rutaRelativa;
        }

        // ============================================
        // MÉTODOS GENERALES
        // ============================================

        public async Task<bool> EliminarArchivoAsync(string rutaRelativa)
        {
            try
            {
                AsegurarWebRoot();
                if (string.IsNullOrEmpty(rutaRelativa))
                    return true;

                _logger.LogInformation("🗑️ Eliminando archivo: {Ruta}", rutaRelativa);

                var rutaCompleta = Path.Combine(_environment.WebRootPath, rutaRelativa);

                if (File.Exists(rutaCompleta))
                {
                    await Task.Run(() => File.Delete(rutaCompleta));
                    _logger.LogInformation("✅ Archivo eliminado: {Ruta}", rutaRelativa);
                    return true;
                }

                _logger.LogWarning("⚠️ Archivo no encontrado: {Ruta}", rutaRelativa);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error eliminando archivo: {Ruta}", rutaRelativa);
                return false;
            }
        }

        public string ObtenerRutaBase()
        {
            return Path.Combine(_environment.WebRootPath, "imagenes");
        }

        public string ObtenerUrlCompleta(string rutaRelativa)
        {
            if (string.IsNullOrEmpty(rutaRelativa))
                return string.Empty;

            rutaRelativa = rutaRelativa.TrimStart('/');
            return $"/{rutaRelativa}";
        }

        // ============================================
        // MÉTODOS DE VERIFICACIÓN
        // ============================================

        public bool ExisteArchivo(string rutaRelativa)
        {
            if (string.IsNullOrEmpty(rutaRelativa))
                return false;

            var rutaCompleta = Path.Combine(_environment.WebRootPath, rutaRelativa);
            return File.Exists(rutaCompleta);
        }

        public string ObtenerRutaAbsoluta(string rutaRelativa)
        {
            if (string.IsNullOrEmpty(rutaRelativa))
                return string.Empty;

            return Path.Combine(_environment.WebRootPath, rutaRelativa);
        }

        // ============================================
        // MÉTODOS DE DIAGNÓSTICO
        // ============================================

        public string ObtenerEstructuraCarpetas()
        {
            return $"📁 Estructura de carpetas:\n" +
                   $"   - Productos: {ObtenerCarpetaProductos()}/\n" +
                   $"   - Categorías: {ObtenerCarpetaCategorias()}/\n" +
                   $"   - Marcas: {ObtenerCarpetaMarcas()}/\n" +
                   $"   - Base: {ObtenerRutaBase()}/";
        }

        // ============================================
        // MÉTODOS PRIVADOS AUXILIARES
        // ============================================

        private string ObtenerCarpetaProductos()
        {
            return Path.Combine("imagenes", "productos");
        }

        private string ObtenerCarpetaCategorias()
        {
            return Path.Combine("imagenes", "categorias");
        }

        private string ObtenerCarpetaMarcas()
        {
            return Path.Combine("imagenes", "marcas");
        }

        private void CrearDirectorioSiNoExiste(string rutaCompleta)
        {
            if (!Directory.Exists(rutaCompleta))
            {
                Directory.CreateDirectory(rutaCompleta);
                _logger.LogInformation("📁 Directorio creado: {Ruta}", rutaCompleta);
            }
        }

        private async Task GuardarImagenOptimizadaAsync(IFormFile archivo, string rutaArchivo)
        {
            using var inputStream = archivo.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            // Redimensionar inteligente (mantiene proporción)
            image.Mutate(x =>
                x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(1200, 1200)
                })
            );

            // Guardar como WebP con alta calidad
            var encoder = new WebpEncoder
            {
                Quality = 80
            };

            await image.SaveAsync(rutaArchivo, encoder);

            _logger.LogInformation("✅ Imagen optimizada guardada: {Ruta}", rutaArchivo);
        }

        private string LimpiarNombreArchivo(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                return "imagen";

            var invalidos = Path.GetInvalidFileNameChars();
            var limpio = new string(nombre
                .Where(c => !invalidos.Contains(c))
                .ToArray());

            // Reemplazar caracteres especiales y espacios
            limpio = limpio.Replace(" ", "_")
                          .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                          .Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n")
                          .Replace("Á", "A").Replace("É", "E").Replace("Í", "I")
                          .Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N")
                          .ToLower()
                          .Trim();

            // Remover extensión .webp si ya está incluida
            limpio = limpio.Replace(".webp", "");

            return limpio.Length > 100 ? limpio.Substring(0, 100) : limpio;
        }
    }
}