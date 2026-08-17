using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace computerChip.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    password = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "atributos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_atributos", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "especificaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    titulo = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    descripcion = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_especificaciones", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "imagenes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    url = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagenes", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "marcas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marcas", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "metodos_pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tipo = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_unicode_ci"),
                    descuento = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    tieneDesc = table.Column<sbyte>(type: "tinyint", nullable: false, defaultValue: (sbyte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metodos_pago", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "ofertas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    titulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci"),
                    subtitulo = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false, collation: "utf8mb4_unicode_ci"),
                    tipoOferta = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false, collation: "utf8mb4_unicode_ci"),
                    tipoDescuento = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false, collation: "utf8mb4_unicode_ci"),
                    descuento = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    precioOriginal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    precioOferta = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ofertas", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "preguntas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    textopregunta = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci"),
                    textorespuesta = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preguntas", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci"),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    precio_oferta = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    garantia = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: false, collation: "utf8mb4_unicode_ci"),
                    stock = table.Column<sbyte>(type: "tinyint", nullable: false, defaultValue: (sbyte)1),
                    envioGratis = table.Column<sbyte>(type: "tinyint", nullable: false, defaultValue: (sbyte)1),
                    codigoSerie = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true, collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "soporte",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombreCompleto = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    email = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true, collation: "utf8mb4_unicode_ci"),
                    telefono = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true, collation: "utf8mb4_unicode_ci"),
                    mensaje = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soporte", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombreCompleto = table.Column<string>(type: "varchar(105)", maxLength: 105, nullable: true, collation: "utf8mb4_unicode_ci"),
                    email = table.Column<string>(type: "varchar(105)", maxLength: 105, nullable: true, collation: "utf8mb4_unicode_ci"),
                    password = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci"),
                    pais = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true, collation: "utf8mb4_unicode_ci"),
                    provincia = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true, collation: "utf8mb4_unicode_ci"),
                    ciudad = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true, collation: "utf8mb4_unicode_ci"),
                    calle = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true, collation: "utf8mb4_unicode_ci"),
                    numero = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_unicode_ci"),
                    celular = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true, collation: "utf8mb4_unicode_ci"),
                    email_verify = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValue: (sbyte)0),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "zona_envio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ciudad = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    provincia = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    pais = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    costo = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci"),
                    codigopostal = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zona_envio", x => x.id);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "categorias_has_productos",
                columns: table => new
                {
                    categorias_idcategorias = table.Column<int>(type: "int", nullable: false),
                    productos_idproductos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias_has_productos", x => new { x.categorias_idcategorias, x.productos_idproductos });
                    table.ForeignKey(
                        name: "FK_categorias_has_productos_categorias_categorias_idcategorias",
                        column: x => x.categorias_idcategorias,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_categorias_has_productos_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos_has_atributos",
                columns: table => new
                {
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    atributos_idatributos = table.Column<int>(type: "int", nullable: false),
                    valor = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false, collation: "utf8mb4_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_has_atributos", x => new { x.productos_idproductos, x.atributos_idatributos });
                    table.ForeignKey(
                        name: "FK_productos_has_atributos_atributos_atributos_idatributos",
                        column: x => x.atributos_idatributos,
                        principalTable: "atributos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_has_atributos_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos_has_especificaciones",
                columns: table => new
                {
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    especificaciones_idespecificaciones = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_has_especificaciones", x => new { x.productos_idproductos, x.especificaciones_idespecificaciones });
                    table.ForeignKey(
                        name: "FK_productos_has_especificaciones_especificaciones_especificaci~",
                        column: x => x.especificaciones_idespecificaciones,
                        principalTable: "especificaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_has_especificaciones_productos_productos_idproduct~",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos_has_imagenes",
                columns: table => new
                {
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    imagenes_idimagenes = table.Column<int>(type: "int", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_has_imagenes", x => new { x.productos_idproductos, x.imagenes_idimagenes });
                    table.ForeignKey(
                        name: "FK_productos_has_imagenes_imagenes_imagenes_idimagenes",
                        column: x => x.imagenes_idimagenes,
                        principalTable: "imagenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_has_imagenes_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos_has_marcas",
                columns: table => new
                {
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    marcas_idmarcas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_has_marcas", x => new { x.productos_idproductos, x.marcas_idmarcas });
                    table.ForeignKey(
                        name: "FK_productos_has_marcas_marcas_marcas_idmarcas",
                        column: x => x.marcas_idmarcas,
                        principalTable: "marcas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_has_marcas_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos_has_ofertas",
                columns: table => new
                {
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    ofertas_idofertas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_has_ofertas", x => new { x.productos_idproductos, x.ofertas_idofertas });
                    table.ForeignKey(
                        name: "FK_productos_has_ofertas_ofertas_ofertas_idofertas",
                        column: x => x.ofertas_idofertas,
                        principalTable: "ofertas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_has_ofertas_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "productos_has_preguntas",
                columns: table => new
                {
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    preguntas_idpreguntas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_has_preguntas", x => new { x.productos_idproductos, x.preguntas_idpreguntas });
                    table.ForeignKey(
                        name: "FK_productos_has_preguntas_preguntas_preguntas_idpreguntas",
                        column: x => x.preguntas_idpreguntas,
                        principalTable: "preguntas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productos_has_preguntas_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "carrito",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "enum('activo','abandonado','convertido')", nullable: false, defaultValue: "activo", collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrito", x => x.id);
                    table.ForeignKey(
                        name: "FK_carrito_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "login_google",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    google_sub = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci"),
                    email_verificado = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValue: (sbyte)1),
                    nombre = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_unicode_ci"),
                    avatar_url = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci"),
                    refresh_token = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci"),
                    ultimo_login = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    deletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_google", x => x.id);
                    table.ForeignKey(
                        name: "FK_login_google_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "push_token",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Admin_idAdmin = table.Column<int>(type: "int", nullable: true),
                    usuarios_idusuarios = table.Column<int>(type: "int", nullable: true),
                    token = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci"),
                    dispositivo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_push_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_push_token_admin_Admin_idAdmin",
                        column: x => x.Admin_idAdmin,
                        principalTable: "admin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_push_token_usuarios_usuarios_idusuarios",
                        column: x => x.usuarios_idusuarios,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "santander_token",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    access_token = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci"),
                    refresh_token = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_unicode_ci"),
                    expires_in = table.Column<int>(type: "int", nullable: false),
                    token_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_santander_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_santander_token_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    metodos_pago_idmetodos_pago = table.Column<int>(type: "int", nullable: false),
                    zona_envio_idenvio = table.Column<int>(type: "int", nullable: false),
                    ofertas_idofertas = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<string>(type: "enum('PENDIENTE','CONFIRMADO','ENVIADO','ENTREGADO','CANCELADO')", nullable: false, defaultValue: "PENDIENTE", collation: "utf8mb4_unicode_ci"),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedidos", x => x.id);
                    table.ForeignKey(
                        name: "FK_pedidos_metodos_pago_metodos_pago_idmetodos_pago",
                        column: x => x.metodos_pago_idmetodos_pago,
                        principalTable: "metodos_pago",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pedidos_ofertas_ofertas_idofertas",
                        column: x => x.ofertas_idofertas,
                        principalTable: "ofertas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_pedidos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pedidos_zona_envio_zona_envio_idenvio",
                        column: x => x.zona_envio_idenvio,
                        principalTable: "zona_envio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "carrito_has_productos",
                columns: table => new
                {
                    carrito_id = table.Column<int>(type: "int", nullable: false),
                    productos_idproductos = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrito_has_productos", x => new { x.carrito_id, x.productos_idproductos });
                    table.ForeignKey(
                        name: "FK_carrito_has_productos_carrito_carrito_id",
                        column: x => x.carrito_id,
                        principalTable: "carrito",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_carrito_has_productos_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "item_pedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cantProducto = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Pedidosid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_pedido", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_pedido_pedidos_Pedidosid",
                        column: x => x.Pedidosid,
                        principalTable: "pedidos",
                        principalColumn: "id");
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "item_pedido_has_productos",
                columns: table => new
                {
                    item_pedido_iditem_pedido = table.Column<int>(type: "int", nullable: false),
                    productos_idproductos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_pedido_has_productos", x => new { x.item_pedido_iditem_pedido, x.productos_idproductos });
                    table.ForeignKey(
                        name: "FK_item_pedido_has_productos_item_pedido_item_pedido_iditem_ped~",
                        column: x => x.item_pedido_iditem_pedido,
                        principalTable: "item_pedido",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_pedido_has_productos_productos_productos_idproductos",
                        column: x => x.productos_idproductos,
                        principalTable: "productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "pedidos_has_item_pedido",
                columns: table => new
                {
                    pedidos_idpedidos = table.Column<int>(type: "int", nullable: false),
                    item_pedido_iditem_pedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedidos_has_item_pedido", x => new { x.pedidos_idpedidos, x.item_pedido_iditem_pedido });
                    table.ForeignKey(
                        name: "FK_pedidos_has_item_pedido_item_pedido_item_pedido_iditem_pedido",
                        column: x => x.item_pedido_iditem_pedido,
                        principalTable: "item_pedido",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pedidos_has_item_pedido_pedidos_pedidos_idpedidos",
                        column: x => x.pedidos_idpedidos,
                        principalTable: "pedidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_Usuario",
                table: "admin",
                column: "usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_carrito_usuario_id",
                table: "carrito",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_carrito_has_productos_productos_idproductos",
                table: "carrito_has_productos",
                column: "productos_idproductos");

            migrationBuilder.CreateIndex(
                name: "IX_categorias_has_productos_productos_idproductos",
                table: "categorias_has_productos",
                column: "productos_idproductos");

            migrationBuilder.CreateIndex(
                name: "IX_item_pedido_Pedidosid",
                table: "item_pedido",
                column: "Pedidosid");

            migrationBuilder.CreateIndex(
                name: "IX_item_pedido_has_productos_productos_idproductos",
                table: "item_pedido_has_productos",
                column: "productos_idproductos");

            migrationBuilder.CreateIndex(
                name: "IX_login_google_usuario_id",
                table: "login_google",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_LoginGoogle_GoogleSub",
                table: "login_google",
                column: "google_sub",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_metodos_pago_idmetodos_pago",
                table: "pedidos",
                column: "metodos_pago_idmetodos_pago");

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_ofertas_idofertas",
                table: "pedidos",
                column: "ofertas_idofertas");

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_usuario_id",
                table: "pedidos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_zona_envio_idenvio",
                table: "pedidos",
                column: "zona_envio_idenvio");

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_has_item_pedido_item_pedido_iditem_pedido",
                table: "pedidos_has_item_pedido",
                column: "item_pedido_iditem_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_productos_has_atributos_atributos_idatributos",
                table: "productos_has_atributos",
                column: "atributos_idatributos");

            migrationBuilder.CreateIndex(
                name: "IX_productos_has_especificaciones_especificaciones_idespecifica~",
                table: "productos_has_especificaciones",
                column: "especificaciones_idespecificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_productos_has_imagenes_imagenes_idimagenes",
                table: "productos_has_imagenes",
                column: "imagenes_idimagenes");

            migrationBuilder.CreateIndex(
                name: "IX_productos_has_marcas_marcas_idmarcas",
                table: "productos_has_marcas",
                column: "marcas_idmarcas");

            migrationBuilder.CreateIndex(
                name: "IX_productos_has_ofertas_ofertas_idofertas",
                table: "productos_has_ofertas",
                column: "ofertas_idofertas");

            migrationBuilder.CreateIndex(
                name: "IX_productos_has_preguntas_preguntas_idpreguntas",
                table: "productos_has_preguntas",
                column: "preguntas_idpreguntas");

            migrationBuilder.CreateIndex(
                name: "IX_push_token_Admin_idAdmin",
                table: "push_token",
                column: "Admin_idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_push_token_usuarios_idusuarios",
                table: "push_token",
                column: "usuarios_idusuarios");

            migrationBuilder.CreateIndex(
                name: "IX_santander_token_usuario_id",
                table: "santander_token",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carrito_has_productos");

            migrationBuilder.DropTable(
                name: "categorias_has_productos");

            migrationBuilder.DropTable(
                name: "item_pedido_has_productos");

            migrationBuilder.DropTable(
                name: "login_google");

            migrationBuilder.DropTable(
                name: "pedidos_has_item_pedido");

            migrationBuilder.DropTable(
                name: "productos_has_atributos");

            migrationBuilder.DropTable(
                name: "productos_has_especificaciones");

            migrationBuilder.DropTable(
                name: "productos_has_imagenes");

            migrationBuilder.DropTable(
                name: "productos_has_marcas");

            migrationBuilder.DropTable(
                name: "productos_has_ofertas");

            migrationBuilder.DropTable(
                name: "productos_has_preguntas");

            migrationBuilder.DropTable(
                name: "push_token");

            migrationBuilder.DropTable(
                name: "santander_token");

            migrationBuilder.DropTable(
                name: "soporte");

            migrationBuilder.DropTable(
                name: "carrito");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "item_pedido");

            migrationBuilder.DropTable(
                name: "atributos");

            migrationBuilder.DropTable(
                name: "especificaciones");

            migrationBuilder.DropTable(
                name: "imagenes");

            migrationBuilder.DropTable(
                name: "marcas");

            migrationBuilder.DropTable(
                name: "preguntas");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "pedidos");

            migrationBuilder.DropTable(
                name: "metodos_pago");

            migrationBuilder.DropTable(
                name: "ofertas");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "zona_envio");
        }
    }
}
