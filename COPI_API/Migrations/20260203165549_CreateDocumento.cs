using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UltimaAlteracaoPor",
                table: "ResultadosKPIPIGE",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPIPIGE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UltimaAlteracaoPor",
                table: "ResultadosKPI",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPI",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AfastamentoId = table.Column<int>(type: "int", nullable: false),
                    NomeOriginal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeArmazenado = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TextoExtraido = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Anonimizado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ContentType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tamanho = table.Column<long>(type: "bigint", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CriadoPor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
              name: "UltimaAlteracaoPor",
              table: "ResultadosKPIPIGE");

            migrationBuilder.DropColumn(
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPIPIGE");

            migrationBuilder.DropColumn(
               name: "UltimaAlteracaoPor",
               table: "ResultadosKPI");

            migrationBuilder.DropColumn(
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPI");

            migrationBuilder.DropTable(
                name: "Documentos");
        }
    }
}
