using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Afastamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroProcesso = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    ServidorNome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServidorMatricula = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServidorCargo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeclaracaoPdfId = table.Column<int>(type: "int", nullable: false),
                    ManifestacaoPdfId = table.Column<int>(type: "int", nullable: false),
                    PossuiInconsistencia = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CriadoPor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afastamentos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ementarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AfastamentoId = table.Column<int>(type: "int", nullable: false),
                    EmentaResumo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fundamentacao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CriadoEm = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ementarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ementarios_Afastamentos_AfastamentoId",
                        column: x => x.AfastamentoId,
                        principalTable: "Afastamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NumeroSei",
                table: "Ementarios",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UltimaAlteracaoPor",
                table: "ResultadosKPIPIGE",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPIPIGE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UltimaAlteracaoPor",
                table: "ResultadosKPI",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
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

            migrationBuilder.UpdateData(
                table: "Documentos",
                keyColumn: "ContentType",
                keyValue: null,
                column: "ContentType",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Documentos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DivisaoId",
                table: "Documentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
               name: "IX_Documentos_AfastamentoId",
               table: "Documentos",
               column: "AfastamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_DivisaoId",
                table: "Documentos",
                column: "DivisaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ementarios_AfastamentoId",
                table: "Ementarios",
                column: "AfastamentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Afastamentos_AfastamentoId",
                table: "Documentos",
                column: "AfastamentoId",
                principalTable: "Afastamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Divisoes_DivisaoId",
                table: "Documentos",
                column: "DivisaoId",
                principalTable: "Divisoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
               name: "FK_Documentos_Afastamentos_AfastamentoId",
               table: "Documentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Divisoes_DivisaoId",
                table: "Documentos");

            migrationBuilder.DropTable(
                name: "Ementarios");

            migrationBuilder.DropTable(
                name: "Afastamentos");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_AfastamentoId",
                table: "Documentos");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_DivisaoId",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "DivisaoId",
                table: "Documentos");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Documentos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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