using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoFixRelacionamentoEmentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPIPIGE");

            migrationBuilder.DropColumn(
                name: "UltimaAlteracaoPor",
                table: "ResultadosKPIPIGE");

            migrationBuilder.DropColumn(
                name: "UltimaAlteracaoEm",
                table: "ResultadosKPI");

            migrationBuilder.DropColumn(
                name: "UltimaAlteracaoPor",
                table: "ResultadosKPI");

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
        }
    }
}
