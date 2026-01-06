using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class UnidadeKPIPIGE_MultiServidores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnidadesKPIPIGE_Servidores_ServidorId",
                table: "UnidadesKPIPIGE");

            migrationBuilder.DropIndex(
                name: "IX_UnidadesKPIPIGE_ServidorId",
                table: "UnidadesKPIPIGE");

            migrationBuilder.DropColumn(
                name: "ServidorId",
                table: "UnidadesKPIPIGE");

            migrationBuilder.CreateTable(
                name: "ServidorUnidadeKPIPIGE",
                columns: table => new
                {
                    ServidoresId = table.Column<int>(type: "int", nullable: false),
                    UnidadeKPIPIGEId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServidorUnidadeKPIPIGE", x => new { x.ServidoresId, x.UnidadeKPIPIGEId });
                    table.ForeignKey(
                        name: "FK_ServidorUnidadeKPIPIGE_Servidores_ServidoresId",
                        column: x => x.ServidoresId,
                        principalTable: "Servidores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServidorUnidadeKPIPIGE_UnidadesKPIPIGE_UnidadeKPIPIGEId",
                        column: x => x.UnidadeKPIPIGEId,
                        principalTable: "UnidadesKPIPIGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ServidorUnidadeKPIPIGE_UnidadeKPIPIGEId",
                table: "ServidorUnidadeKPIPIGE",
                column: "UnidadeKPIPIGEId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServidorUnidadeKPIPIGE");

            migrationBuilder.AddColumn<int>(
                name: "ServidorId",
                table: "UnidadesKPIPIGE",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesKPIPIGE_ServidorId",
                table: "UnidadesKPIPIGE",
                column: "ServidorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnidadesKPIPIGE_Servidores_ServidorId",
                table: "UnidadesKPIPIGE",
                column: "ServidorId",
                principalTable: "Servidores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
