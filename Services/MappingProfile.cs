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
            CreateMap<ProductoUpdateRequest, Productos>();

            CreateMap<AtributoCreateRequest, Atributos>();

            CreateMap<Atributos, AtributoResponse>();

            CreateMap<Productos, ProductoResponse>();
            CreateMap<Productos, ProductoListResponse>();
            CreateMap<Productos, ProductoMiniResponse>();
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