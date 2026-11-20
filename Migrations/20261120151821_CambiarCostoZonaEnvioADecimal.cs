using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace computerChip.Migrations
{
    /// <inheritdoc />
    public partial class CambiarCostoZonaEnvioADecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "costo",
                table: "zona_envio",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(65)",
                oldMaxLength: 65)
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "costo",
                table: "zona_envio",
                type: "varchar(65)",
                maxLength: 65,
                nullable: false,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }
    }
}
