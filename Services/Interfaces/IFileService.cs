using Microsoft.AspNetCore.Http;

namespace computerChip.Services.Interfaces
{
    public interface IFileService
    {
        // ========== MÉTODOS PARA PRODUCTOS ==========
        Task<string> GuardarImagenProductoAsync(IFormFile archivo, int productoId, string nombreProducto);
        Task<string> GuardarImagenProductoConTimestampAsync(IFormFile archivo, int productoId, string nombreProducto);

        // ========== MÉTODOS PARA CATEGORÍAS Y MARCAS ==========
        Task<string> GuardarImagenCategoriaAsync(IFormFile archivo, int categoriaId, string nombreCategoria);
        Task<string> GuardarImagenMarcaAsync(IFormFile archivo, int marcaId, string nombreMarca);

        // ========== MÉTODOS GENERALES ==========
        Task<bool> EliminarArchivoAsync(string rutaRelativa);
        string ObtenerRutaBase();
        string ObtenerUrlCompleta(string rutaRelativa);

        // ========== MÉTODOS DE VERIFICACIÓN ==========
        bool ExisteArchivo(string rutaRelativa);
        string ObtenerRutaAbsoluta(string rutaRelativa);

        // ========== MÉTODOS DE DIAGNÓSTICO ==========
        string ObtenerEstructuraCarpetas();
    }
}