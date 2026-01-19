using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoMetaModulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvaliacaoDi",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");


            migrationBuilder.AddColumn<string>(
                name: "Aprovador",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Consultado",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Entregavel",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Informado",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Objetivo",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Premissas",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Responsavel",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Restricoes",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Riscos",
                table: "Metas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aprovador",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "AvaliacaoDi",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Consultado",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Entregavel",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Informado",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Objetivo",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Premissas",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Responsavel",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Restricoes",
                table: "Metas");

            migrationBuilder.DropColumn(
                name: "Riscos",
                table: "Metas");
        }
    }
}
