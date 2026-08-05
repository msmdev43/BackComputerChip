using AutoMapper;
using computerChip.DTOs;
using computerChip.Models;

namespace computerChip.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================
            // USUARIO
            // ============================================
            CreateMap<Usuarios, UsuarioDto>()
                .ForMember(dest => dest.IsGoogleUser, 
                    opt => opt.MapFrom(src => src.loginGoogle != null && src.loginGoogle.Any()));

            CreateMap<UsuarioCreateDto, Usuarios>()
                .ForMember(dest => dest.password, opt => opt.Ignore())
                .ForMember(dest => dest.emailVerify, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UsuarioUpdateDto, Usuarios>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // PRODUCTO
            // ============================================
            CreateMap<Productos, ProductoDto>()
                .ForMember(dest => dest.Categorias, 
                    opt => opt.MapFrom(src => src.categoriasProductos.Select(cp => cp.categoria.nombre)))
                .ForMember(dest => dest.Marcas, 
                    opt => opt.MapFrom(src => src.marcasProductos.Select(mp => mp.marca.nombre)))
                .ForMember(dest => dest.Imagenes, 
                    opt => opt.MapFrom(src => src.productosImagenes.Select(pi => pi.imagen.url)));

            CreateMap<ProductoCreateDto, Productos>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<ProductoUpdateDto, Productos>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // CATEGORIA
            // ============================================
            CreateMap<Categorias, CategoriaDto>()
                .ForMember(dest => dest.ProductosCount, 
                    opt => opt.MapFrom(src => src.categoriasProductos.Count));

            CreateMap<CategoriaCreateDto, Categorias>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CategoriaUpdateDto, Categorias>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // MARCA
            // ============================================
            CreateMap<Marcas, MarcaDto>()
                .ForMember(dest => dest.ProductosCount, 
                    opt => opt.MapFrom(src => src.marcasProductos.Count));

            CreateMap<MarcaCreateDto, Marcas>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MarcaUpdateDto, Marcas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // CARRITO
            // ============================================
            CreateMap<CarritoProductos, CarritoItemDto>()
                .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.productoId))
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.producto.nombre))
                .ForMember(dest => dest.ProductoImagen, 
                    opt => opt.MapFrom(src => src.producto.productosImagenes.FirstOrDefault().imagen.url));

            CreateMap<Carrito, CarritoDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.carritoProductos))
                .ForMember(dest => dest.Total, 
                    opt => opt.MapFrom(src => src.carritoProductos.Sum(cp => cp.cantidad * cp.precioUnitario)))
                .ForMember(dest => dest.ItemsCount, 
                    opt => opt.MapFrom(src => src.carritoProductos.Sum(cp => cp.cantidad)));

            // ============================================
            // PEDIDO
            // ============================================
            CreateMap<ItemPedido, PedidoItemDto>()
                .ForMember(dest => dest.ProductoId, 
                    opt => opt.MapFrom(src => src.itemPedidoProductos.FirstOrDefault().productoId))
                .ForMember(dest => dest.ProductoNombre, 
                    opt => opt.MapFrom(src => src.itemPedidoProductos.FirstOrDefault().producto.nombre))
                .ForMember(dest => dest.PrecioUnitario, 
                    opt => opt.MapFrom(src => src.subtotal / src.cantidad));

            CreateMap<Pedidos, PedidoDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.usuario.nombreCompleto))
                .ForMember(dest => dest.UsuarioEmail, opt => opt.MapFrom(src => src.usuario.email))
                .ForMember(dest => dest.MetodoPago, opt => opt.MapFrom(src => src.metodoPago.tipo))
                .ForMember(dest => dest.ZonaEnvio, opt => opt.MapFrom(src => src.zonaEnvio.ciudad))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.items))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado.ToString()));

            // ============================================
            // OFERTA
            // ============================================
            CreateMap<Ofertas, OfertaDto>()
                .ForMember(dest => dest.ProductosIds, 
                    opt => opt.MapFrom(src => src.productosOfertas.Select(po => po.productoId)));

            CreateMap<OfertaCreateDto, Ofertas>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<OfertaUpdateDto, Ofertas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ============================================
            // ADMIN
            // ============================================
            CreateMap<Admin, AdminDto>();

            CreateMap<AdminCreateDto, Admin>()
                .ForMember(dest => dest.password, opt => opt.Ignore())
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            // ============================================
            // LOGIN GOOGLE
            // ============================================
            CreateMap<LoginGoogle, UsuarioDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.usuarioId))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.EmailVerify, opt => opt.MapFrom(src => src.emailVerificado))
                .ForMember(dest => dest.IsGoogleUser, opt => opt.MapFrom(src => true));
        }
    }
}