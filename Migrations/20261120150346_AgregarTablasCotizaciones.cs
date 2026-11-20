using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace computerChip.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasCotizaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cotizaciones",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_unicode_ci"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    comprador_nombre = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_unicode_ci"),
                    comprador_email = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: false, collation: "utf8mb4_unicode_ci"),
                    comprador_telefono = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false, collation: "utf8mb4_unicode_ci"),
                    entrega_tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_unicode_ci"),
                    entrega_provincia = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_unicode_ci"),
                    entrega_ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_unicode_ci"),
                    entrega_codigo_postal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_unicode_ci"),
                    entrega_direccion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_unicode_ci"),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_unicode_ci"),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    envio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    puede_confirmar = table.Column<sbyte>(type: "tinyint", nullable: false),
                    advertencias = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime", nullable: false),
                    utilizada = table.Column<sbyte>(type: "tinyint", nullable: false, defaultValue: (sbyte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotizaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_cotizaciones_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "cotizaciones_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cotizacion_id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_unicode_ci"),
                    producto_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci"),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotizaciones_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_cotizaciones_items_cotizaciones_cotizacion_id",
                        column: x => x.cotizacion_id,
                        principalTable: "cotizaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_UsuarioId",
                table: "cotizaciones",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_Utilizada_Vencimiento",
                table: "cotizaciones",
                columns: new[] { "utilizada", "fecha_vencimiento" });

            migrationBuilder.CreateIndex(
                name: "IX_CotizacionesItems_CotizacionId",
                table: "cotizaciones_items",
                column: "cotizacion_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cotizaciones_items");

            migrationBuilder.DropTable(
                name: "cotizaciones");
        }
    }
}
