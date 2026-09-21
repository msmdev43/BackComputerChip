using AutoMapper;
using computerChip.DTOs.Requests.Admin;
using computerChip.DTOs.Requests.Atributo;
using computerChip.DTOs.Requests.Carrito;
using computerChip.DTOs.Requests.Categoria;
using computerChip.DTOs.Requests.Especificacion;
using computerChip.DTOs.Requests.Marcas;
using computerChip.DTOs.Requests.MetodoPago;
using computerChip.DTOs.Requests.Oferta;
using computerChip.DTOs.Requests.Pedido;
using computerChip.DTOs.Requests.Pregunta;
using computerChip.DTOs.Requests.Productos;
using computerChip.DTOs.Requests.PushToken;
using computerChip.DTOs.Requests.Soporte;
using computerChip.DTOs.Requests.Usuario;
using computerChip.DTOs.Requests.ZonaEnvio;
using computerChip.DTOs.Responses.AdminDashboard;
using computerChip.DTOs.Responses.Atributo;
using computerChip.DTOs.Responses.Auth;
using computerChip.DTOs.Responses.Carrito;
using computerChip.DTOs.Responses.Categoria;
using computerChip.DTOs.Responses.Especificacion;
using computerChip.DTOs.Responses.Marcas;
using computerChip.DTOs.Responses.MetodoPago;
using computerChip.DTOs.Responses.Oferta;
using computerChip.DTOs.Responses.Pedido;
using computerChip.DTOs.Responses.Productos;
using computerChip.DTOs.Responses.PushToken;
using computerChip.DTOs.Responses.Soporte;
using computerChip.DTOs.Responses.Usuario;
using computerChip.DTOs.Responses.ZonaEnvio;
using computerChip.Models;
using computerChip.Models.Enum;
using computerChip.Models.TablasIntermedias;

namespace computerChip.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================
            // ADMIN DASHBOARD y SOPORTE
            // ============================================
            CreateMap<AdminLoginRequest, Admin>();
            CreateMap<Admin, computerChip.DTOs.Responses.AdminDashboard.AdminResponse>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
    .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.usuario))
    .ForMember(dest => dest.Email, opt => opt.Ignore())
    .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.Token, opt => opt.Ignore())
    .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<SoporteCreateRequest, Soporte>();

            CreateMap<Admin, DashboardStatsResponse>();

            CreateMap<Soporte, SoporteResponse>();

            // ============================================
            // USUARIOS y ZONA DE ENVIO
            // ============================================
            CreateMap<UsuarioRegisterRequest, Usuarios>();
            CreateMap<UsuarioUpdateRequest, Usuarios>();

            CreateMap<ZonaEnvioCreateRequest, ZonaEnvio>();
            CreateMap<ZonaEnvioUpdateRequest, ZonaEnvio>();
            CreateMap<ZonaEnvioCostoRequest, ZonaEnvio>();

            CreateMap<Usuarios, UsuarioResponse>();
            CreateMap<Usuarios, UsuarioDetailResponse>();
            CreateMap<Usuarios, UsuarioResumenResponse>();
            CreateMap<Usuarios, UsuarioAdminResponse>();
            CreateMap<Usuarios, UsuarioLoginResponse>();

            CreateMap<ZonaEnvio, ZonaEnvioResponse>();
            CreateMap<ZonaEnvio, ZonaEnvioCostoResponse>();

            // ============================================
            // PRODUCTOS y ATRIBUTOS
            // ============================================
            CreateMap<ProductoCreateRequest, Productos>();
            CreateMap<ProductoSearchRequest, Productos>();
            CreateMap<ProductoStockRequest, Productos>();
            CreateMap<ProductoUpdateRequest, Productos>();

            CreateMap<AtributoCreateRequest, Atributos>();
            CreateMap<AtributoUpdateRequest, Atributos>();

            CreateMap<Atributos, AtributoResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.nombre));

            CreateMap<Atributos, AtributoDetailResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.nombre))
                .ForMember(d => d.Productos,
                    o => o.MapFrom(s => s.ProductosAtributos.Select(pa => pa.Productos)));

            // ---------- MAPEO PRINCIPAL DE PRODUCTO ----------
            CreateMap<Productos, ProductoResponse>()
                // Categorías → List<ProductoCategoriaResponse>
                .ForMember(d => d.Categorias,
                    o => o.MapFrom(s => s.CategoriasProductos.Select(cp => cp.Categorias)))
                // Marcas → List<ProductoMarcaResponse>
                .ForMember(d => d.Marcas,
                    o => o.MapFrom(s => s.ProductosMarcas.Select(pm => pm.Marcas)))
                // Imágenes → List<string> (solo URLs, ordenadas)
                .ForMember(d => d.Imagenes,
                    o => o.MapFrom(s => s.ProductosImagenes
                        .OrderBy(pi => pi.orden)
                        .Select(pi => pi.Imagenes.url)
                        .ToList()))
                // Especificaciones → List<ProductoEspecificacionResponse>
                .ForMember(d => d.Especificaciones,
                    o => o.MapFrom(s => s.ProductosEspecificaciones.Select(pe => pe.Especificaciones)))
                // Atributos → List<ProductoAtributoResponse>
                .ForMember(d => d.Atributos,
                    o => o.MapFrom(s => s.ProductoAtributos))
                // EnvioGratis: bool
                .ForMember(d => d.EnvioGratis,
                    o => o.MapFrom(s => s.envioGratis));

            CreateMap<ProductosAtributos, AsignacionAtributoResponse>()
                .ForMember(d => d.ProductoId, o => o.MapFrom(s => s.productoId))
                .ForMember(d => d.ProductoNombre, o => o.MapFrom(s => s.Productos.nombre))
                .ForMember(d => d.AtributoId, o => o.MapFrom(s => s.atributoId))
                .ForMember(d => d.AtributoNombre, o => o.MapFrom(s => s.Atributos.nombre))
                .ForMember(d => d.Valor, o => o.MapFrom(s => s.valor));

            // ---------- MAPEOS DE TIPOS ANIDADOS ----------

            // Categoría → ProductoCategoriaResponse
            CreateMap<Categorias, ProductoCategoriaResponse>()
                .ForMember(d => d.nombre, o => o.MapFrom(s => s.nombre));

            // Marca → ProductoMarcaResponse
            CreateMap<Marcas, ProductoMarcaResponse>()
                .ForMember(d => d.nombre, o => o.MapFrom(s => s.nombre));

            // Imagen → ProductoImagenResponse (por si lo usás en otro DTO)
            CreateMap<Imagenes, ProductoImagenResponse>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.nombre, o => o.MapFrom(s => s.nombre))
                .ForMember(d => d.url, o => o.MapFrom(s => s.url));

            // Especificación → ProductoEspecificacionResponse
            CreateMap<Especificaciones, ProductoEspecificacionResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.Titulo, o => o.MapFrom(s => s.titulo))
                .ForMember(d => d.Descripcion, o => o.MapFrom(s => s.descripcion));

            // Relación ProductosAtributos → ProductoAtributoResponse
            CreateMap<ProductosAtributos, ProductoAtributoResponse>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.atributoId))
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.Atributos.nombre))
                .ForMember(d => d.Valor, o => o.MapFrom(s => s.valor));

            // ---------- OTROS RESPONSES ----------
            CreateMap<Productos, ProductoListResponse>();

            CreateMap<Productos, ProductoMiniResponse>()
                .ForMember(d => d.ImagenPrincipal,
                    o => o.MapFrom(s => s.ProductosImagenes
                        .OrderBy(pi => pi.orden)
                        .Select(pi => pi.Imagenes.url)
                        .FirstOrDefault()));

            CreateMap<Productos, ProductoMasVendidoResponse>();

            // ============================================
            // CATEGORIAS
            // ============================================
            CreateMap<CategoriaCreateRequest, Categorias>();
            CreateMap<CategoriaUpdateRequest, Categorias>();

            CreateMap<Categorias, CategoriaResponse>();
            CreateMap<Categorias, CategoriaDetailResponse>();

            // ============================================
            // MARCAS
            // ============================================
            CreateMap<MarcaCreateRequest, Marcas>();
            CreateMap<MarcaUpdateRequest, Marcas>();

            CreateMap<Marcas, MarcaResponse>();
            CreateMap<Marcas, MarcaDetailResponse>();

            // ============================================
            // ESPECIFICACIONES
            // ============================================
            CreateMap<EspecificacionCreateRequest, Especificaciones>();
            CreateMap<EspecificacionUpdateRequest, Especificaciones>();

            CreateMap<Especificaciones, EspecificacionResponse>();

            // ============================================
            // PREGUNTAS
            // ============================================
            CreateMap<PreguntaCreateRequest, Preguntas>();
            CreateMap<PreguntaUpdateRequest, Preguntas>();
            CreateMap<PreguntaResponderRequest, Preguntas>();

            // ============================================
            // PEDIDOS, CARRITO y METODO PAGO
            // ============================================
            CreateMap<PedidoCreateRequest, Pedidos>();
            CreateMap<PedidoFilterRequest, Pedidos>();
            CreateMap<PedidoUpdateEstadoRequest, Pedidos>();

            CreateMap<CarritoAddItemRequest, Carrito>();
            CreateMap<CarritoUpdateItemRequest, Carrito>();
            CreateMap<CarritoRemoveItemRequest, Carrito>();

            CreateMap<MetodoPagoCreateRequest, MetodoPago>();
            CreateMap<MetodoPagoUpdateRequest, MetodoPago>();

            CreateMap<Pedidos, PedidoResponse>();
            CreateMap<Pedidos, PedidoListResponse>();
            CreateMap<Pedidos, PedidoStatsResponse>();
            CreateMap<Pedidos, PedidoItemResponse>();

            CreateMap<Carrito, CarritoResponse>();

            CreateMap<MetodoPago, MetodoPagoResponse>();

            // ============================================
            // OFERTAS
            // ============================================
            CreateMap<OfertaCreateRequest, Ofertas>();
            CreateMap<OfertaUpdateRequest, Ofertas>();
            CreateMap<OfertaApplyRequest, Ofertas>();

            CreateMap<Ofertas, OfertaResponse>();
            CreateMap<Ofertas, OfertaListResponse>();
            CreateMap<Ofertas, OfertaProductoResponse>();
            CreateMap<Ofertas, OfertaResumenResponse>();

            // ============================================
            // PUSH TOKENS
            // ============================================
            CreateMap<PushTokenRegisterRequest, PushToken>();
            CreateMap<PushTokenSendRequest, PushToken>();

            CreateMap<PushToken, PushTokenResponse>();
            CreateMap<PushToken, PushTokenSendResponse>();
        }
    }
}