using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace computerChip.Migrations
{
    /// <inheritdoc />
    public partial class CrearCheckoutPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_item_pedido_has_productos_productos_productos_idproductos",
                table: "item_pedido_has_productos");

            migrationBuilder.AlterColumn<int>(
                name: "metodos_pago_idmetodos_pago",
                table: "pedidos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "comprador_email",
                table: "pedidos",
                type: "varchar(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "comprador_nombre",
                table: "pedidos",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "comprador_telefono",
                table: "pedidos",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "cotizacion_id",
                table: "pedidos",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true,
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "entrega_ciudad",
                table: "pedidos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "entrega_codigo_postal",
                table: "pedidos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "entrega_direccion",
                table: "pedidos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "entrega_provincia",
                table: "pedidos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "entrega_tipo",
                table: "pedidos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "envio",
                table: "pedidos",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "estado_pago",
                table: "pedidos",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "NO_SOLICITADO",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "idempotency_key",
                table: "pedidos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "pedidos",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "request_hash",
                table: "pedidos",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true,
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "subtotal",
                table: "pedidos",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "nombre_producto",
                table: "item_pedido",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "precio_unitario",
                table: "item_pedido",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "zona_envio_id",
                table: "cotizaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_CotizacionId",
                table: "pedidos",
                column: "cotizacion_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdempotencyKey",
                table: "pedidos",
                column: "idempotency_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cotizaciones_zona_envio_id",
                table: "cotizaciones",
                column: "zona_envio_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cotizaciones_zona_envio_zona_envio_id",
                table: "cotizaciones",
                column: "zona_envio_id",
                principalTable: "zona_envio",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_item_pedido_has_productos_productos_productos_idproductos",
                table: "item_pedido_has_productos",
                column: "productos_idproductos",
                principalTable: "productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_cotizaciones_cotizacion_id",
                table: "pedidos",
                column: "cotizacion_id",
                principalTable: "cotizaciones",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cotizaciones_zona_envio_zona_envio_id",
                table: "cotizaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_item_pedido_has_productos_productos_productos_idproductos",
                table: "item_pedido_has_productos");

            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_cotizaciones_cotizacion_id",
                table: "pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_CotizacionId",
                table: "pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdempotencyKey",
                table: "pedidos");

            migrationBuilder.DropIndex(
                name: "IX_cotizaciones_zona_envio_id",
                table: "cotizaciones");

            migrationBuilder.DropColumn(
                name: "comprador_email",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "comprador_nombre",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "comprador_telefono",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "cotizacion_id",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "entrega_ciudad",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "entrega_codigo_postal",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "entrega_direccion",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "entrega_provincia",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "entrega_tipo",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "envio",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "estado_pago",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "idempotency_key",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "request_hash",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "subtotal",
                table: "pedidos");

            migrationBuilder.DropColumn(
                name: "nombre_producto",
                table: "item_pedido");

            migrationBuilder.DropColumn(
                name: "precio_unitario",
                table: "item_pedido");

            migrationBuilder.DropColumn(
                name: "zona_envio_id",
                table: "cotizaciones");

            migrationBuilder.AlterColumn<int>(
                name: "metodos_pago_idmetodos_pago",
                table: "pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_item_pedido_has_productos_productos_productos_idproductos",
                table: "item_pedido_has_productos",
                column: "productos_idproductos",
                principalTable: "productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
