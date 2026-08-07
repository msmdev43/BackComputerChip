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
using computerChip.Models;
using computerChip.Models.TablasIntermedias;

namespace computerChip.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================
            // USUARIO
            // ============================================
            CreateMap<UsuarioRegisterRequest, Usuarios>()
                .ForMember(dest => dest.password, opt => opt.Ignore())
                .ForMember(dest => dest.emailVerify, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UsuarioUpdateRequest, Usuarios>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // PRODUCTO
            // ============================================
            CreateMap<ProductoCreateRequest, Productos>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CategoriasProductos, opt => opt.Ignore())
                .ForMember(dest => dest.ProductosMarcas, opt => opt.Ignore())
                .ForMember(dest => dest.ProductoAtributos, opt => opt.Ignore())
                .ForMember(dest => dest.ProductosImagenes, opt => opt.Ignore())
                .ForMember(dest => dest.ProductosEspecificaciones, opt => opt.Ignore());

            CreateMap<ProductoUpdateRequest, Productos>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Atributos de producto (intermedio)
            CreateMap<ProductoAtributoRequest, ProductosAtributos>()
                .ForMember(dest => dest.productoId, opt => opt.Ignore());

            // ============================================
            // CATEGORIA
            // ============================================
            CreateMap<CategoriaCreateRequest, Categorias>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CategoriaUpdateRequest, Categorias>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // MARCA
            // ============================================
            CreateMap<MarcaCreateRequest, Marcas>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MarcaUpdateRequest, Marcas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // CARRITO
            // ============================================
            CreateMap<CarritoAddItemRequest, CarritoProductos>()
                .ForMember(dest => dest.carritoId, opt => opt.Ignore())
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            // ============================================
            // PEDIDO
            // ============================================
            CreateMap<PedidoCreateRequest, Pedidos>()
                .ForMember(dest => dest.estado, opt => opt.MapFrom(src => computerChip.Models.Enum.EstadoPedido.PENDIENTE))
                .ForMember(dest => dest.total, opt => opt.Ignore())
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Items, opt => opt.Ignore());

            // ============================================
            // OFERTA
            // ============================================
            CreateMap<OfertaCreateRequest, Ofertas>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ProductosOfertas, opt => opt.Ignore());

            CreateMap<OfertaUpdateRequest, Ofertas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // ESPECIFICACION
            // ============================================
            CreateMap<EspecificacionCreateRequest, Especificaciones>();
            CreateMap<EspecificacionUpdateRequest, Especificaciones>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // ATRIBUTO
            // ============================================
            CreateMap<AtributoCreateRequest, Atributos>();
            CreateMap<AtributoUpdateRequest, Atributos>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // PREGUNTA
            // ============================================
            CreateMap<PreguntaCreateRequest, Preguntas>();
            CreateMap<PreguntaUpdateRequest, Preguntas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // ZONA ENVIO
            // ============================================
            CreateMap<ZonaEnvioCreateRequest, ZonaEnvio>();
            CreateMap<ZonaEnvioUpdateRequest, ZonaEnvio>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // METODO PAGO
            // ============================================
            CreateMap<MetodoPagoCreateRequest, MetodoPago>();
            CreateMap<MetodoPagoUpdateRequest, MetodoPago>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // SOPORTE
            // ============================================
            CreateMap<SoporteCreateRequest, Soporte>()
                .ForMember(dest => dest.fecha, opt => opt.MapFrom(src => DateTime.Now));

            // ============================================
            // PUSH TOKEN
            // ============================================
            CreateMap<PushTokenRegisterRequest, PushToken>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.adminId, opt => opt.Ignore())
                .ForMember(dest => dest.usuarioId, opt => opt.Ignore());

            // ============================================
            // LOGIN GOOGLE
            // ============================================
            CreateMap<UsuarioGoogleLoginRequest, LoginGoogle>()
                .ForMember(dest => dest.emailVerificado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ultimoLogin, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.usuarioId, opt => opt.Ignore());

            // ============================================
            // TABLAS INTERMEDIAS (para creación)
            // ============================================
            CreateMap<int, CategoriasProductos>()
                .ForMember(dest => dest.categoriaId, opt => opt.MapFrom(src => src));

            CreateMap<int, ProductosMarcas>()
                .ForMember(dest => dest.marcaId, opt => opt.MapFrom(src => src));

            CreateMap<int, ProductosEspecificaciones>()
                .ForMember(dest => dest.especificacionId, opt => opt.MapFrom(src => src));

            CreateMap<ProductoAtributoRequest, ProductosAtributos>()
                .ForMember(dest => dest.productoId, opt => opt.Ignore());
        }
    }
}