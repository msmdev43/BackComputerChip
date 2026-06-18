using computerChip.Models;
using computerChip.Models.TablasIntermedias;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace computerChip.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ============================================
        // DbSets - TABLAS PRINCIPALES
        // ============================================
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<LoginGoogle> LoginGoogle { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Marcas> Marcas { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoProductos> CarritoProductos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<ZonaEnvio> ZonasEnvio { get; set; }
        public DbSet<Ofertas> Ofertas { get; set; }
        public DbSet<Imagenes> Imagenes { get; set; }
        public DbSet<Especificaciones> Especificaciones { get; set; }
        public DbSet<Preguntas> Preguntas { get; set; }
        public DbSet<Atributos> Atributos { get; set; }
        public DbSet<ProductosAtributos> ProductosAtributos { get; set; }
        public DbSet<Soporte> Soporte { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<PushToken> PushTokens { get; set; }
        public DbSet<SantanderToken> SantanderTokens { get; set; }

        // ============================================
        // DbSets - TABLAS INTERMEDIAS
        // ============================================
        public DbSet<CategoriasProductos> CategoriasProductos { get; set; }
        public DbSet<ProductosMarcas> ProductosMarcas { get; set; }
        public DbSet<ProductosImagenes> ProductosImagenes { get; set; }
        public DbSet<ProductosOfertas> ProductosOfertas { get; set; }
        public DbSet<ProductosPreguntas> ProductosPreguntas { get; set; }
        public DbSet<ProductosEspecificaciones> ProductosEspecificaciones { get; set; }
        public DbSet<PedidosItemPedido> PedidosItemPedido { get; set; }
        public DbSet<ItemPedidoProductos> ItemPedidoProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar charset y collation para MySQL
            modelBuilder.UseCollation("utf8mb4_unicode_ci");

            // ============================================================
            // CONFIGURACIONES DE ENTIDADES PRINCIPALES
            // ============================================================

            // ---------- USUARIO ----------
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombreCompleto)
                    .HasColumnType("varchar(105)")
                    .HasMaxLength(105)
                    .IsRequired(false);

                entity.Property(e => e.email)
                    .HasColumnType("varchar(105)")
                    .HasMaxLength(105)
                    .IsRequired(false);

                entity.HasIndex(e => e.email)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuarios_Email");

                entity.Property(e => e.password)
                    .HasColumnType("text")
                    .IsRequired(false);

                entity.Property(e => e.pais)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.provincia)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.ciudad)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.calle)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.numero)
                    .HasColumnType("varchar(45)")
                    .HasMaxLength(45)
                    .IsRequired(false);

                entity.Property(e => e.celular)
                    .HasColumnType("varchar(25)")
                    .HasMaxLength(25)
                    .IsRequired(false);

                entity.Property(e => e.emailVerify)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(0);

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // RELACIONES
                entity.HasMany(e => e.LoginGoogle)
                    .WithOne(e => e.Usuarios)
                    .HasForeignKey(e => e.usuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Carrito)
                    .WithOne(e => e.Usuarios)
                    .HasForeignKey(e => e.usuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Pedidos)
                    .WithOne(e => e.Usuarios)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.PushTokens)
                    .WithOne(e => e.Usuarios)
                    .HasForeignKey(e => e.usuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.SantanderTokens)
                    .WithOne(e => e.Usuarios)
                    .HasForeignKey(e => e.usuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- LOGIN GOOGLE ----------
            modelBuilder.Entity<LoginGoogle>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.googleSub)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.HasIndex(e => e.googleSub)
                    .IsUnique()
                    .HasDatabaseName("IX_LoginGoogle_GoogleSub");

                entity.Property(e => e.email)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.emailVerificado)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(1);

                entity.Property(e => e.nombre)
                    .HasColumnType("varchar(150)")
                    .HasMaxLength(150)
                    .IsRequired(false);

                entity.Property(e => e.avatarUrl)
                    .HasColumnType("text")
                    .IsRequired(false);

                entity.Property(e => e.refreshToken)
                    .HasColumnType("text")
                    .IsRequired(false);

                entity.Property(e => e.ultimoLogin)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            // ---------- PRODUCTO ----------
            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombre)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.precio)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.precioOferta)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired(false);

                entity.Property(e => e.garantia)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.stock)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(1);

                entity.Property(e => e.envioGratis)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(1);

                entity.Property(e => e.codigoSerie)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired(false);

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // RELACIONES Many-to-Many con tablas intermedias
                entity.HasMany(e => e.CategoriasProductos)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ProductosMarcas)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ProductosImagenes)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ProductosOfertas)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ProductosPreguntas)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ProductosEspecificaciones)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ProductoAtributos)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relaciones One-to-Many
                entity.HasMany(e => e.CarritoProductos)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ItemsPedidoProductos)
                    .WithOne(e => e.Productos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- CATEGORIA ----------
            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombre)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // RELACIONES
                entity.HasMany(e => e.CategoriasProductos)
                    .WithOne(e => e.Categorias)
                    .HasForeignKey(e => e.categoriaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- MARCA ----------
            modelBuilder.Entity<Marcas>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombre)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // RELACIONES
                entity.HasMany(e => e.ProductosMarcas)
                    .WithOne(e => e.Marcas)
                    .HasForeignKey(e => e.marcaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- CARRITO ----------
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.estado)
                    .HasColumnType("enum('activo','abandonado','convertido')")
                    .HasDefaultValue("activo");

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                // RELACIONES
                entity.HasMany(e => e.CarritoProductos)
                    .WithOne(e => e.Carrito)
                    .HasForeignKey(e => e.carritoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- CARRITO PRODUCTO ----------
            modelBuilder.Entity<CarritoProductos>(entity =>
            {
                entity.HasKey(e => new { e.carritoId, e.productoId });

                entity.Property(e => e.cantidad)
                    .HasColumnType("int")
                    .HasDefaultValue(1)
                    .IsRequired();

                entity.Property(e => e.precioUnitario)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // ---------- PEDIDO ----------
            modelBuilder.Entity<Pedidos>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.estado)
                    .HasColumnType("enum('pendiente','confirmado','enviado','entregado','cancelado')")
                    .HasDefaultValue("pendiente");

                entity.Property(e => e.total)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                // RELACIONES
                entity.HasMany(e => e.PedidosItemPedido)
                    .WithOne(e => e.Pedidos)
                    .HasForeignKey(e => e.pedidosId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- ITEM PEDIDO ----------
            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.cantidad)
                    .HasColumnType("int")
                    .IsRequired();

                entity.Property(e => e.subtotal)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                // RELACIONES
                entity.HasMany(e => e.PedidosItemPedido)
                    .WithOne(e => e.ItemPedido)
                    .HasForeignKey(e => e.itemPedidoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ItemPedidoProductos)
                    .WithOne(e => e.ItemPedido)
                    .HasForeignKey(e => e.itemPedidoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- METODO PAGO ----------
            modelBuilder.Entity<MetodoPago>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.tipo)
                    .HasColumnType("varchar(45)")
                    .HasMaxLength(45)
                    .IsRequired();

                entity.Property(e => e.descuento)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.tieneDesc)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(0);

                // RELACIONES
                entity.HasMany(e => e.Pedidos)
                    .WithOne(e => e.MetodoPago)
                    .HasForeignKey(e => e.MetodoPagoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- ZONA ENVIO ----------
            modelBuilder.Entity<ZonaEnvio>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.ciudad)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.provincia)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.pais)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.costo)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.codigoPostal)
                    .HasColumnType("varchar(45)")
                    .HasMaxLength(45)
                    .IsRequired();

                // RELACIONES
                entity.HasMany(e => e.Pedidos)
                    .WithOne(e => e.ZonaEnvio)
                    .HasForeignKey(e => e.ZonaEnvioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- OFERTA ----------
            modelBuilder.Entity<Ofertas>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.titulo)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.subtitulo)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.tipoOferta)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.tipoDescuento)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.descuento)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.precioOriginal)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.precioOferta)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // RELACIONES
                entity.HasMany(e => e.ProductosOfertas)
                    .WithOne(e => e.Ofertas)
                    .HasForeignKey(e => e.ofertaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Pedidos)
                    .WithOne(e => e.Ofertas)
                    .HasForeignKey(e => e.OfertaId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ---------- IMAGEN ----------
            modelBuilder.Entity<Imagenes>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombre)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.url)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.deletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // RELACIONES
                entity.HasMany(e => e.ProductosImagenes)
                    .WithOne(e => e.Imagenes)
                    .HasForeignKey(e => e.imagenId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- ESPECIFICACION ----------
            modelBuilder.Entity<Especificaciones>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.titulo)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.descripcion)
                    .HasColumnType("text")
                    .IsRequired();

                // RELACIONES
                entity.HasMany(e => e.ProductosEspecificaciones)
                    .WithOne(e => e.Especificaciones)
                    .HasForeignKey(e => e.especificacionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PREGUNTA ----------
            modelBuilder.Entity<Preguntas>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.textoPregunta)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.textoRespuesta)
                    .HasColumnType("text")
                    .IsRequired();

                // RELACIONES
                entity.HasMany(e => e.ProductosPreguntas)
                    .WithOne(e => e.Preguntas)
                    .HasForeignKey(e => e.preguntaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- ATRIBUTO ----------
            modelBuilder.Entity<Atributos>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombre)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                // RELACIONES
                entity.HasMany(e => e.ProductosAtributos)
                    .WithOne(e => e.Atributos)
                    .HasForeignKey(e => e.atributoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PRODUCTO ATRIBUTO ----------
            modelBuilder.Entity<ProductosAtributos>(entity =>
            {
                entity.HasKey(e => new { e.productoId, e.atributoId });

                entity.Property(e => e.valor)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();
            });

            // ---------- SOPORTE ----------
            modelBuilder.Entity<Soporte>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.nombreCompleto)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.email)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.telefono)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.mensaje)
                    .HasColumnType("text")
                    .IsRequired();
            });

            // ---------- ADMIN ----------
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.usuario)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.HasIndex(e => e.usuario)
                    .IsUnique()
                    .HasDatabaseName("IX_Admin_Usuario");

                entity.Property(e => e.password)
                    .HasColumnType("text")
                    .IsRequired();

                // RELACIONES
                entity.HasMany(e => e.PushTokens)
                    .WithOne(e => e.Admin)
                    .HasForeignKey(e => e.adminId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PUSH TOKEN ----------
            modelBuilder.Entity<PushToken>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.token)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.dispositivo)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50)
                    .IsRequired(false);

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // ---------- SANTANDER TOKEN ----------
            modelBuilder.Entity<SantanderToken>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.accessToken)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.refreshToken)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.expiresIn)
                    .HasColumnType("int")
                    .IsRequired();

                entity.Property(e => e.tokenType)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50)
                    .IsRequired(false);

                entity.Property(e => e.createdAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.updatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // ============================================================
            // CONFIGURACIONES DE TABLAS INTERMEDIAS (MANY-TO-MANY)
            // ============================================================

            // ---------- CATEGORIAS PRODUCTOS ----------
            modelBuilder.Entity<CategoriasProductos>(entity =>
            {
                entity.HasKey(e => new { e.categoriaId, e.productoId });

                entity.HasOne(e => e.Categorias)
                    .WithMany(e => e.CategoriasProductos)
                    .HasForeignKey(e => e.categoriaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.CategoriasProductos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- MARCAS PRODUCTOS ----------
            modelBuilder.Entity<ProductosMarcas>(entity =>
            {
                entity.HasKey(e => new { e.productoId, e.marcaId });

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.ProductosMarcas)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Marcas)
                    .WithMany(e => e.ProductosMarcas)
                    .HasForeignKey(e => e.marcaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PRODUCTOS IMAGENES ----------
            modelBuilder.Entity<ProductosImagenes>(entity =>
            {
                entity.HasKey(e => new { e.productoId, e.imagenId });

                entity.Property(e => e.orden)
                    .HasColumnType("int")
                    .IsRequired();

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.ProductosImagenes)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Imagenes)
                    .WithMany(e => e.ProductosImagenes)
                    .HasForeignKey(e => e.imagenId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PRODUCTOS OFERTAS ----------
            modelBuilder.Entity<ProductosOfertas>(entity =>
            {
                entity.HasKey(e => new { e.productoId, e.ofertaId });

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.ProductosOfertas)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Ofertas)
                    .WithMany(e => e.ProductosOfertas)
                    .HasForeignKey(e => e.ofertaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PRODUCTOS PREGUNTAS ----------
            modelBuilder.Entity<ProductosPreguntas>(entity =>
            {
                entity.HasKey(e => new { e.productoId, e.preguntaId });

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.ProductosPreguntas)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Preguntas)
                    .WithMany(e => e.ProductosPreguntas)
                    .HasForeignKey(e => e.preguntaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PRODUCTOS ESPECIFICACIONES ----------
            modelBuilder.Entity<ProductosEspecificaciones>(entity =>
            {
                entity.HasKey(e => new { e.productoId, e.especificacionId });

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.ProductosEspecificaciones)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Especificaciones)
                    .WithMany(e => e.ProductosEspecificaciones)
                    .HasForeignKey(e => e.especificacionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PEDIDOS ITEM PEDIDO ----------
            modelBuilder.Entity<PedidosItemPedido>(entity =>
            {
                entity.HasKey(e => new { e.pedidosId, e.itemPedidoId });

                entity.HasOne(e => e.Pedidos)
                    .WithMany(e => e.PedidosItemPedido)
                    .HasForeignKey(e => e.pedidosId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ItemPedido)
                    .WithMany(e => e.PedidosItemPedido)
                    .HasForeignKey(e => e.itemPedidoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- ITEM PEDIDO PRODUCTOS ----------
            modelBuilder.Entity<ItemPedidoProductos>(entity =>
            {
                entity.HasKey(e => new { e.itemPedidoId, e.productoId });

                entity.HasOne(e => e.ItemPedido)
                    .WithMany(e => e.ItemPedidoProductos)
                    .HasForeignKey(e => e.itemPedidoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Productos)
                    .WithMany(e => e.ItemsPedidoProductos)
                    .HasForeignKey(e => e.productoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}