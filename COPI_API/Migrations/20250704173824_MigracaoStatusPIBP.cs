using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoStatusPIBP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusPIBP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnidadeKPIId = table.Column<int>(type: "int", nullable: false),
                    CicloId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPIBP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusPIBP_Ciclos_CicloId",
                        column: x => x.CicloId,
                        principalTable: "Ciclos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusPIBP_UnidadesKPI_UnidadeKPIId",
                        column: x => x.UnidadeKPIId,
                        principalTable: "UnidadesKPI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPIBP_CicloId",
                table: "StatusPIBP",
                column: "CicloId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPIBP_UnidadeKPIId",
                table: "StatusPIBP",
                column: "UnidadeKPIId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusPIBP");
        }
    }
}
