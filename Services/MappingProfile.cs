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
        public MappingProfile(ICollection<CarritoProductos> carritoProductos)
        {
            // ============================================
            // REQUESTS → MODELOS (Creación/Actualización)
            // ============================================

            // ---------- USUARIO ----------
            CreateMap<UsuarioRegisterRequest, Usuarios>()
                .ForMember(dest => dest.password, opt => opt.Ignore())
                .ForMember(dest => dest.emailVerify, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UsuarioUpdateRequest, Usuarios>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- PRODUCTO ----------
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

            CreateMap<ProductoAtributoRequest, ProductosAtributos>()
                .ForMember(dest => dest.productoId, opt => opt.Ignore());

            // ---------- CATEGORIA ----------
            CreateMap<CategoriaCreateRequest, Categorias>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CategoriaUpdateRequest, Categorias>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- MARCA ----------
            CreateMap<MarcaCreateRequest, Marcas>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MarcaUpdateRequest, Marcas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- CARRITO ----------
            CreateMap<CarritoAddItemRequest, CarritoProductos>()
                .ForMember(dest => dest.carritoId, opt => opt.Ignore())
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now));

            // ---------- PEDIDO ----------
            CreateMap<PedidoCreateRequest, Pedidos>()
                .ForMember(dest => dest.estado, opt => opt.MapFrom(src => EstadoPedido.PENDIENTE))
                .ForMember(dest => dest.total, opt => opt.Ignore())
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Items, opt => opt.Ignore());

            // ---------- OFERTA ----------
            CreateMap<OfertaCreateRequest, Ofertas>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ProductosOfertas, opt => opt.Ignore());

            CreateMap<OfertaUpdateRequest, Ofertas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- ESPECIFICACION ----------
            CreateMap<EspecificacionCreateRequest, Especificaciones>();
            CreateMap<EspecificacionUpdateRequest, Especificaciones>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- ATRIBUTO ----------
            CreateMap<AtributoCreateRequest, Atributos>();
            CreateMap<AtributoUpdateRequest, Atributos>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- PREGUNTA ----------
            CreateMap<PreguntaCreateRequest, Preguntas>();
            CreateMap<PreguntaUpdateRequest, Preguntas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- ZONA ENVIO ----------
            CreateMap<ZonaEnvioCreateRequest, ZonaEnvio>();
            CreateMap<ZonaEnvioUpdateRequest, ZonaEnvio>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- METODO PAGO ----------
            CreateMap<MetodoPagoCreateRequest, MetodoPago>();
            CreateMap<MetodoPagoUpdateRequest, MetodoPago>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ---------- SOPORTE ----------
            CreateMap<SoporteCreateRequest, Soporte>()
                .ForMember(dest => dest.fecha, opt => opt.MapFrom(src => DateTime.Now));

            // ---------- PUSH TOKEN ----------
            CreateMap<PushTokenRegisterRequest, PushToken>()
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.adminId, opt => opt.Ignore())
                .ForMember(dest => dest.usuarioId, opt => opt.Ignore());

            // ---------- LOGIN GOOGLE ----------
            CreateMap<UsuarioGoogleLoginRequest, LoginGoogle>()
                .ForMember(dest => dest.emailVerificado, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ultimoLogin, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.usuarioId, opt => opt.Ignore());

            // ---------- TABLAS INTERMEDIAS ----------
            CreateMap<int, CategoriasProductos>()
                .ForMember(dest => dest.categoriaId, opt => opt.MapFrom(src => src));

            CreateMap<int, ProductosMarcas>()
                .ForMember(dest => dest.marcaId, opt => opt.MapFrom(src => src));

            CreateMap<int, ProductosEspecificaciones>()
                .ForMember(dest => dest.especificacionId, opt => opt.MapFrom(src => src));

            CreateMap<ProductoAtributoRequest, ProductosAtributos>()
                .ForMember(dest => dest.productoId, opt => opt.Ignore());

            // ============================================
            // MODELOS → RESPONSES
            // ============================================

            // ---------- USUARIO ----------
            //CreateMap<Usuarios, UsuarioResponse>()
            //    .ForMember(dest => dest.IsGoogleUser,
            //        opt => opt.MapFrom(src => src.LoginGoogle != null && src.LoginGoogle.Any()))
            //    .ForMember(dest => dest.PedidosCount,
            //        opt => opt.MapFrom(src => src.Pedidos != null ? src.Pedidos.Count : 0))
            //    .ForMember(dest => dest.CarritoItemsCount,
            //        opt => opt.MapFrom(src => src.Carrito != null && src.Carrito.Any()
            //            ? src.Carrito.FirstOrDefault()?.CarritoProductos?.Sum(cp => cp.cantidad) ?? 0
            //            : 0));

            CreateMap<Usuarios, UsuarioDetailResponse>()
                .ForMember(dest => dest.IsGoogleUser,
                    opt => opt.MapFrom(src => src.LoginGoogle != null && src.LoginGoogle.Any()))
                .ForMember(dest => dest.Pedidos,
                    opt => opt.MapFrom(src => src.Pedidos))
                .ForMember(dest => dest.Carrito,
                    opt => opt.MapFrom(src => src.Carrito != null ? src.Carrito.FirstOrDefault() : null))
                .ForMember(dest => dest.PushTokens,
                    opt => opt.MapFrom(src => src.PushTokens));

            CreateMap<Usuarios, UsuarioResumenResponse>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.nombreCompleto))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email));

            CreateMap<Usuarios, UsuarioAdminResponse>()
                .ForMember(dest => dest.IsGoogleUser,
                    opt => opt.MapFrom(src => src.LoginGoogle != null && src.LoginGoogle.Any()))
                .ForMember(dest => dest.PedidosCount,
                    opt => opt.MapFrom(src => src.Pedidos != null ? src.Pedidos.Count : 0))
                .ForMember(dest => dest.TotalGastado,
                    opt => opt.MapFrom(src => src.Pedidos != null ? src.Pedidos.Sum(p => p.total) : 0))
                .ForMember(dest => dest.UltimoPedido,
                    opt => opt.MapFrom(src => src.Pedidos != null && src.Pedidos.Any()
                        ? src.Pedidos.OrderByDescending(p => p.createdAt).FirstOrDefault().createdAt
                        : (DateTime?)null))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.deletedAt == null));

            // ---------- LOGIN ----------
            //CreateMap<Usuarios, UsuarioAuthResponse>()
            //    .ForMember(dest => dest.IsGoogleUser,
            //        opt => opt.MapFrom(src => src.LoginGoogle != null && src.LoginGoogle.Any()));

            // ---------- PRODUCTO ----------
            CreateMap<Productos, ProductoResponse>()
                .ForMember(dest => dest.Categorias,
                    opt => opt.MapFrom(src => src.CategoriasProductos.Select(cp => cp.Categorias.nombre)))
                .ForMember(dest => dest.Marcas,
                    opt => opt.MapFrom(src => src.ProductosMarcas.Select(pm => pm.Marcas.nombre)))
                .ForMember(dest => dest.Imagenes,
                    opt => opt.MapFrom(src => src.ProductosImagenes.Select(pi => pi.Imagenes.url)))
                .ForMember(dest => dest.Especificaciones,
                    opt => opt.MapFrom(src => src.ProductosEspecificaciones.Select(pe => pe.Especificaciones)))
                .ForMember(dest => dest.Atributos,
                    opt => opt.MapFrom(src => src.ProductoAtributos.Select(pa => new ProductoAtributoResponse
                    {
                        Id = pa.Atributos.id,
                        Nombre = pa.Atributos.nombre,
                        Valor = pa.valor
                    })))
                .ForMember(dest => dest.EnvioGratis,
                    opt => opt.MapFrom(src => src.envioGratis == 1));

            CreateMap<Productos, ProductoListResponse>()
                .ForMember(dest => dest.ImagenPrincipal,
                    opt => opt.MapFrom(src => src.ProductosImagenes.FirstOrDefault().Imagenes.url))
                .ForMember(dest => dest.CategoriaPrincipal,
                    opt => opt.MapFrom(src => src.CategoriasProductos.FirstOrDefault().Categorias.nombre));

            CreateMap<Productos, ProductoMiniResponse>()
                .ForMember(dest => dest.ImagenPrincipal,
                    opt => opt.MapFrom(src => src.ProductosImagenes.FirstOrDefault().Imagenes.url));

            CreateMap<Productos, ProductoMasVendidoResponse>()
                .ForMember(dest => dest.ImagenPrincipal,
                    opt => opt.MapFrom(src => src.ProductosImagenes.FirstOrDefault().Imagenes.url))
                .ForMember(dest => dest.CategoriaPrincipal,
                    opt => opt.MapFrom(src => src.CategoriasProductos.FirstOrDefault().Categorias.nombre))
                .ForMember(dest => dest.TotalVendido,
                    opt => opt.MapFrom(src => src.ItemsPedidoProductos.Sum(ip => ip.ItemPedido.cantidad)))
                .ForMember(dest => dest.IngresoGenerado,
                    opt => opt.MapFrom(src => src.ItemsPedidoProductos.Sum(ip => ip.ItemPedido.subtotal)));

            // ---------- CATEGORIA ----------
            CreateMap<Categorias, CategoriaResponse>()
                .ForMember(dest => dest.ProductosCount,
                    opt => opt.MapFrom(src => src.CategoriasProductos.Count))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.deletedAt == null));

            CreateMap<Categorias, CategoriaDetailResponse>()
                .ForMember(dest => dest.Productos,
                    opt => opt.MapFrom(src => src.CategoriasProductos.Select(cp => cp.Productos)));

            // ---------- MARCA ----------
            CreateMap<Marcas, MarcaResponse>()
                .ForMember(dest => dest.ProductosCount,
                    opt => opt.MapFrom(src => src.ProductosMarcas.Count))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.deletedAt == null));

            CreateMap<Marcas, MarcaDetailResponse>()
                .ForMember(dest => dest.Productos,
                    opt => opt.MapFrom(src => src.ProductosMarcas.Select(pm => pm.Productos)));

            // ---------- CARRITO ----------
            CreateMap<CarritoProductos, CarritoItemResponse>()
                .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.productoId))
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Productos.nombre))
                .ForMember(dest => dest.ProductoImagen,
                    opt => opt.MapFrom(src => src.Productos.ProductosImagenes.FirstOrDefault().Imagenes.url))
                .ForMember(dest => dest.StockDisponible,
                    opt => opt.MapFrom(src => src.Productos.stock));

            CreateMap<Carrito, CarritoResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CarritoProductos))
                .ForMember(dest => dest.ItemsCount,
                    opt => opt.MapFrom(src => src.CarritoProductos.Sum(cp => cp.cantidad)))
                .ForMember(dest => dest.ProductosDistintos,
                    opt => opt.MapFrom(src => src.CarritoProductos.Count))
                .ForMember(dest => dest.Total,
                    opt => opt.MapFrom(src => src.CarritoProductos.Sum(cp => cp.cantidad * cp.precioUnitario)));

            // ---------- PEDIDO ----------
            CreateMap<Pedidos, PedidoResponse>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado.ToString()))
                .ForMember(dest => dest.EstadoColor,
                    opt => opt.MapFrom(src => ObtenerColorEstado(src.estado)))
                .ForMember(dest => dest.MetodoPago, opt => opt.MapFrom(src => src.MetodoPago.tipo))
                .ForMember(dest => dest.ZonaEnvio,
                    opt => opt.MapFrom(src => $"{src.ZonaEnvio.ciudad}, {src.ZonaEnvio.provincia}"))
                .ForMember(dest => dest.DireccionEnvio,
                    opt => opt.MapFrom(src => $"{src.Usuarios.calle} {src.Usuarios.numero}, {src.Usuarios.ciudad}, {src.Usuarios.provincia}"))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuarios))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Oferta,
                    opt => opt.MapFrom(src => src.Ofertas != null
                        ? new OfertaResumenResponse
                        {
                            Id = src.Ofertas.id,
                            Titulo = src.Ofertas.titulo,
                            Descuento = src.Ofertas.descuento
                        }
                        : null));

            CreateMap<Pedidos, PedidoListResponse>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado.ToString()))
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuarios.nombreCompleto))
                .ForMember(dest => dest.ItemsCount,
                    opt => opt.MapFrom(src => src.Items.Sum(i => i.cantidad)));

            CreateMap<ItemPedido, PedidoItemResponse>()
                .ForMember(dest => dest.ProductoId,
                    opt => opt.MapFrom(src => src.ItemPedidoProductos.FirstOrDefault().productoId))
                .ForMember(dest => dest.ProductoNombre,
                    opt => opt.MapFrom(src => src.ItemPedidoProductos.FirstOrDefault().Productos.nombre))
                .ForMember(dest => dest.ProductoImagen,
                    opt => opt.MapFrom(src => src.ItemPedidoProductos.FirstOrDefault().Productos.ProductosImagenes.FirstOrDefault().Imagenes.url))
                .ForMember(dest => dest.PrecioUnitario,
                    opt => opt.MapFrom(src => src.subtotal / src.cantidad));

            // ---------- OFERTA ----------
            CreateMap<Ofertas, OfertaResponse>()
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.deletedAt == null))
                .ForMember(dest => dest.Productos,
                    opt => opt.MapFrom(src => src.ProductosOfertas.Select(po => po.Productos)));

            CreateMap<Ofertas, OfertaListResponse>()
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.deletedAt == null))
                .ForMember(dest => dest.ProductosCount,
                    opt => opt.MapFrom(src => src.ProductosOfertas.Count));

            CreateMap<Ofertas, OfertaResumenResponse>();

            // ---------- ESPECIFICACION ----------
            CreateMap<Especificaciones, EspecificacionResponse>()
                .ForMember(dest => dest.ProductosAsociados,
                    opt => opt.MapFrom(src => src.ProductosEspecificaciones.Count));

            // ---------- ATRIBUTO ----------
            CreateMap<Atributos, AtributoResponse>()
                .ForMember(dest => dest.ProductosAsociados,
                    opt => opt.MapFrom(src => src.ProductosAtributos.Count));

            // ---------- ZONA ENVIO ----------
            CreateMap<ZonaEnvio, ZonaEnvioResponse>()
                .ForMember(dest => dest.PedidosCount,
                    opt => opt.MapFrom(src => src.Pedidos.Count));

            // ---------- METODO PAGO ----------
            CreateMap<MetodoPago, MetodoPagoResponse>()
                .ForMember(dest => dest.PedidosCount,
                    opt => opt.MapFrom(src => src.Pedidos.Count))
                .ForMember(dest => dest.TieneDesc,
                    opt => opt.MapFrom(src => src.tieneDesc == 1));

            // ---------- SOPORTE ----------
            CreateMap<Soporte, SoporteResponse>()
                .ForMember(dest => dest.Atendido,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.mensaje)));

            // ---------- PUSH TOKEN ----------
            CreateMap<PushToken, PushTokenResponse>()
                .ForMember(dest => dest.UsuarioNombre,
                    opt => opt.MapFrom(src => src.Usuarios != null ? src.Usuarios.nombreCompleto : null))
                .ForMember(dest => dest.AdminNombre,
                    opt => opt.MapFrom(src => src.Admin != null ? src.Admin.usuario : null));

            // ---------- DASHBOARD ----------
            // DashboardStatsResponse se puede construir manualmente en el servicio o controlador
        }

        // ============================================
        // MÉTODOS AUXILIARES
        // ============================================
        private static string ObtenerColorEstado(EstadoPedido estado)
        {
            return estado switch
            {
                EstadoPedido.PENDIENTE => "#FFA500",     // Naranja
                EstadoPedido.CONFIRMADO => "#2196F3",    // Azul
                EstadoPedido.ENVIADO => "#9C27B0",       // Púrpura
                EstadoPedido.ENTREGADO => "#4CAF50",     // Verde
                EstadoPedido.CANCELADO => "#F44336",     // Rojo
                _ => "#757575"                           // Gris
            };
        }
    }
}