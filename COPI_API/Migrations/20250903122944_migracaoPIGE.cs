using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COPI_API.Migrations
{
    /// <inheritdoc />
    public partial class migracaoPIGE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliacoesPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NivelGerenciadoAtingido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NivelIntegradoAtingido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NivelPadronizadoAtingido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotaIMPIGE = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NotaNG = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NotaNI = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NotaNP = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacoesPIGE", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CiclosPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Encerrado = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiclosPIGE", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EixosPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Peso = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EixosPIGE", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NiveisPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveisPIGE", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnidadesKPIPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnidadeId = table.Column<int>(type: "int", nullable: false),
                    ServidorId = table.Column<int>(type: "int", nullable: false),
                    SEI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesKPIPIGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadesKPIPIGE_Servidores_ServidorId",
                        column: x => x.ServidorId,
                        principalTable: "Servidores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnidadesKPIPIGE_Unidades_UnidadeId",
                        column: x => x.UnidadeId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KPIsPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelId = table.Column<int>(type: "int", nullable: true),
                    EixoId = table.Column<int>(type: "int", nullable: true),
                    Pontuacao = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIsPIGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KPIsPIGE_EixosPIGE_EixoId",
                        column: x => x.EixoId,
                        principalTable: "EixosPIGE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KPIsPIGE_NiveisPIGE_NivelId",
                        column: x => x.NivelId,
                        principalTable: "NiveisPIGE",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StatusPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CicloPIGEId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UnidadeKPIPIGEId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPIGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusPIGE_CiclosPIGE_CicloPIGEId",
                        column: x => x.CicloPIGEId,
                        principalTable: "CiclosPIGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusPIGE_UnidadesKPIPIGE_UnidadeKPIPIGEId",
                        column: x => x.UnidadeKPIPIGEId,
                        principalTable: "UnidadesKPIPIGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ResultadosKPIPIGE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnidadeKPIPIGEId = table.Column<int>(type: "int", nullable: false),
                    KPIPIGEId = table.Column<int>(type: "int", nullable: false),
                    CicloPIGEId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Prova = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AvaliacaoEscrita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AvaliacaoPIGEId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosKPIPIGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadosKPIPIGE_AvaliacoesPIGE_AvaliacaoPIGEId",
                        column: x => x.AvaliacaoPIGEId,
                        principalTable: "AvaliacoesPIGE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResultadosKPIPIGE_CiclosPIGE_CicloPIGEId",
                        column: x => x.CicloPIGEId,
                        principalTable: "CiclosPIGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadosKPIPIGE_KPIsPIGE_KPIPIGEId",
                        column: x => x.KPIPIGEId,
                        principalTable: "KPIsPIGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadosKPIPIGE_UnidadesKPIPIGE_UnidadeKPIPIGEId",
                        column: x => x.UnidadeKPIPIGEId,
                        principalTable: "UnidadesKPIPIGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_KPIsPIGE_EixoId",
                table: "KPIsPIGE",
                column: "EixoId");

            migrationBuilder.CreateIndex(
                name: "IX_KPIsPIGE_NivelId",
                table: "KPIsPIGE",
                column: "NivelId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosKPIPIGE_AvaliacaoPIGEId",
                table: "ResultadosKPIPIGE",
                column: "AvaliacaoPIGEId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosKPIPIGE_CicloPIGEId",
                table: "ResultadosKPIPIGE",
                column: "CicloPIGEId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosKPIPIGE_KPIPIGEId",
                table: "ResultadosKPIPIGE",
                column: "KPIPIGEId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosKPIPIGE_UnidadeKPIPIGEId",
                table: "ResultadosKPIPIGE",
                column: "UnidadeKPIPIGEId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPIGE_CicloPIGEId",
                table: "StatusPIGE",
                column: "CicloPIGEId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPIGE_UnidadeKPIPIGEId",
                table: "StatusPIGE",
                column: "UnidadeKPIPIGEId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesKPIPIGE_ServidorId",
                table: "UnidadesKPIPIGE",
                column: "ServidorId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesKPIPIGE_UnidadeId",
                table: "UnidadesKPIPIGE",
                column: "UnidadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosKPIPIGE");

            migrationBuilder.DropTable(
                name: "StatusPIGE");

            migrationBuilder.DropTable(
                name: "AvaliacoesPIGE");

            migrationBuilder.DropTable(
                name: "KPIsPIGE");

            migrationBuilder.DropTable(
                name: "CiclosPIGE");

            migrationBuilder.DropTable(
                name: "UnidadesKPIPIGE");

            migrationBuilder.DropTable(
                name: "EixosPIGE");

            migrationBuilder.DropTable(
                name: "NiveisPIGE");
        }
    }
}
