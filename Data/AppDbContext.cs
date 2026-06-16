using computerChip.Models;
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

        // DbSets
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
        public DbSet<ProductoAtributos> ProductoAtributos { get; set; }
        public DbSet<Soporte> Soporte { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<PushToken> PushTokens { get; set; }
        public DbSet<SantanderToken> SantanderTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar charset y collation para MySQL
            modelBuilder.UseCollation("utf8mb4_unicode_ci");

            // ============================================================
            // CONFIGURACIONES DE ENTIDADES
            // ============================================================

            // ---------- USUARIO ----------
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.NombreCompleto)
                    .HasColumnType("varchar(105)")
                    .HasMaxLength(105)
                    .IsRequired(false);

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(105)")
                    .HasMaxLength(105)
                    .IsRequired(false);

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuarios_Email");

                entity.Property(e => e.Password)
                    .HasColumnType("text")
                    .IsRequired(false);

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.Provincia)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.Calle)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.Numero)
                    .HasColumnType("varchar(45)")
                    .HasMaxLength(45)
                    .IsRequired(false);

                entity.Property(e => e.Celular)
                    .HasColumnType("varchar(25)")
                    .HasMaxLength(25)
                    .IsRequired(false);

                entity.Property(e => e.EmailVerify)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(0);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            // ---------- LOGIN GOOGLE ----------
            modelBuilder.Entity<LoginGoogle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.GoogleSub)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.HasIndex(e => e.GoogleSub)
                    .IsUnique()
                    .HasDatabaseName("IX_LoginGoogle_GoogleSub");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(100)")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.EmailVerificado)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(1);

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(150)")
                    .HasMaxLength(150)
                    .IsRequired(false);

                entity.Property(e => e.AvatarUrl)
                    .HasColumnType("text")
                    .IsRequired(false);

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("text")
                    .IsRequired(false);

                entity.Property(e => e.UltimoLogin)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.LoginGoogle)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PRODUCTO ----------
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.PrecioOferta)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired(false);

                entity.Property(e => e.Garantia)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.Stock)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(1);

                entity.Property(e => e.EnvioGratis)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(1);

                entity.Property(e => e.CodigoSerie)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                // Relaciones muchos a muchos
                entity.HasMany(e => e.Categorias)
                    .WithMany(e => e.Productos)
                    .UsingEntity<Dictionary<string, object>>(
                        "CategoriasProductos",
                        j => j.HasOne<Categoria>().WithMany().HasForeignKey("CategoriaId"),
                        j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId")
                    );

                entity.HasMany(e => e.Marcas)
                    .WithMany(e => e.Productos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductosMarcas",
                        j => j.HasOne<Marca>().WithMany().HasForeignKey("MarcaId"),
                        j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId")
                    );

                entity.HasMany(e => e.Imagenes)
                    .WithMany(e => e.Productos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductosImagenes",
                        j => j.HasOne<Imagen>().WithMany().HasForeignKey("ImagenId"),
                        j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId")
                    );

                entity.HasMany(e => e.Ofertas)
                    .WithMany(e => e.Productos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductosOfertas",
                        j => j.HasOne<Oferta>().WithMany().HasForeignKey("OfertaId"),
                        j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId")
                    );

                entity.HasMany(e => e.Preguntas)
                    .WithMany(e => e.Productos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductosPreguntas",
                        j => j.HasOne<Pregunta>().WithMany().HasForeignKey("PreguntaId"),
                        j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId")
                    );

                entity.HasMany(e => e.Especificaciones)
                    .WithMany(e => e.Productos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductosEspecificaciones",
                        j => j.HasOne<Especificacion>().WithMany().HasForeignKey("EspecificacionId"),
                        j => j.HasOne<Producto>().WithMany().HasForeignKey("ProductoId")
                    );
            });

            // ---------- CATEGORIA ----------
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            // ---------- MARCA ----------
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            // ---------- CARRITO ----------
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Estado)
                    .HasColumnType("enum('activo','abandonado','convertido')")
                    .HasDefaultValue("activo");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Carritos)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- CARRITO PRODUCTO ----------
            modelBuilder.Entity<CarritoProducto>(entity =>
            {
                entity.HasKey(e => new { e.CarritoId, e.ProductoId });

                entity.Property(e => e.Cantidad)
                    .HasColumnType("int")
                    .HasDefaultValue(1)
                    .IsRequired();

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                // Relaciones
                entity.HasOne(e => e.Carrito)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(e => e.CarritoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Producto)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(e => e.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PEDIDO ----------
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Estado)
                    .HasColumnType("enum('pendiente','confirmado','enviado','entregado','cancelado')")
                    .HasDefaultValue("pendiente");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                // Relaciones
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Pedidos)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.MetodoPago)
                    .WithMany(m => m.Pedidos)
                    .HasForeignKey(e => e.MetodoPagoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ZonaEnvio)
                    .WithMany(z => z.Pedidos)
                    .HasForeignKey(e => e.ZonaEnvioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Oferta)
                    .WithMany(o => o.Pedidos)
                    .HasForeignKey(e => e.OfertaId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });

            // ---------- ITEM PEDIDO ----------
            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Cantidad)
                    .HasColumnType("int")
                    .IsRequired();

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                // Relaciones
                entity.HasOne(e => e.Pedido)
                    .WithMany(p => p.Items)
                    .HasForeignKey(e => e.PedidoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Producto)
                    .WithMany(p => p.ItemsPedido)
                    .HasForeignKey(e => e.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- METODO PAGO ----------
            modelBuilder.Entity<MetodoPago>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Tipo)
                    .HasColumnType("varchar(45)")
                    .HasMaxLength(45)
                    .IsRequired();

                entity.Property(e => e.Descuento)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.TieneDesc)
                    .HasColumnType("tinyint")
                    .HasDefaultValue(0);
            });

            // ---------- ZONA ENVIO ----------
            modelBuilder.Entity<ZonaEnvio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Ciudad)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.Provincia)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.Pais)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.Costo)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.CodigoPostal)
                    .HasColumnType("varchar(45)")
                    .HasMaxLength(45)
                    .IsRequired();
            });

            // ---------- OFERTA ----------
            modelBuilder.Entity<Oferta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Titulo)
                    .HasColumnType("varchar(255)")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Subtitulo)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.TipoOferta)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.TipoDescuento)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();

                entity.Property(e => e.Descuento)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.PrecioOriginal)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.PrecioOferta)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            // ---------- IMAGEN ----------
            modelBuilder.Entity<Imagen>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.Url)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            // ---------- ESPECIFICACION ----------
            modelBuilder.Entity<Especificacion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Titulo)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .IsRequired();
            });

            // ---------- PREGUNTA ----------
            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.TextoPregunta)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.TextoRespuesta)
                    .HasColumnType("text")
                    .IsRequired();
            });

            // ---------- ATRIBUTO ----------
            modelBuilder.Entity<Atributo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(85)")
                    .HasMaxLength(85)
                    .IsRequired();
            });

            // ---------- PRODUCTO ATRIBUTO ----------
            modelBuilder.Entity<ProductoAtributo>(entity =>
            {
                entity.HasKey(e => new { e.ProductoId, e.AtributoId });

                entity.Property(e => e.Valor)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.HasOne(e => e.Producto)
                    .WithMany()
                    .HasForeignKey(e => e.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Atributo)
                    .WithMany()
                    .HasForeignKey(e => e.AtributoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- SOPORTE ----------
            modelBuilder.Entity<Soporte>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.NombreCompleto)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.Telefono)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired(false);

                entity.Property(e => e.Mensaje)
                    .HasColumnType("text")
                    .IsRequired();
            });

            // ---------- ADMIN ----------
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Usuario)
                    .HasColumnType("varchar(65)")
                    .HasMaxLength(65)
                    .IsRequired();

                entity.HasIndex(e => e.Usuario)
                    .IsUnique()
                    .HasDatabaseName("IX_Admin_Usuario");

                entity.Property(e => e.Password)
                    .HasColumnType("text")
                    .IsRequired();
            });

            // ---------- PUSH TOKEN ----------
            modelBuilder.Entity<PushToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Token)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.Dispositivo)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50)
                    .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.Admin)
                    .WithMany(a => a.PushTokens)
                    .HasForeignKey(e => e.AdminId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.PushTokens)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);
            });

            // ---------- SANTANDER TOKEN ----------
            modelBuilder.Entity<SantanderToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.AccessToken)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("text")
                    .IsRequired();

                entity.Property(e => e.ExpiresIn)
                    .HasColumnType("int")
                    .IsRequired();

                entity.Property(e => e.TokenType)
                    .HasColumnType("varchar(50)")
                    .HasMaxLength(50)
                    .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.SantanderTokens)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
