using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoProcessoSEIPIBP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SEI",
                table: "UnidadesKPI",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SEI",
                table: "UnidadesKPI");
        }
    }
}
