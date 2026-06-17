using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class CriandoPagamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormasPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusPagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPagamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormaPagamentoId = table.Column<int>(type: "int", nullable: false),
                    StatusPagamentoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaPagamentoId = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: true),
                    ProfissionalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_CategoriasPagamento_CategoriaPagamentoId",
                        column: x => x.CategoriaPagamentoId,
                        principalTable: "CategoriasPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pagamentos_FormasPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormasPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Profissionais_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pagamentos_StatusPagamentos_StatusPagamentoId",
                        column: x => x.StatusPagamentoId,
                        principalTable: "StatusPagamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CategoriasPagamento",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Serviço" },
                    { 2, "Locação" },
                    { 3, "Venda" },
                    { 4, "Multa" },
                    { 5, "Outro" }
                });

            migrationBuilder.InsertData(
                table: "FormasPagamento",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Pix" },
                    { 2, "Cartão de Crédito" },
                    { 3, "Cartão de Débito" },
                    { 4, "Dinheiro" },
                    { 5, "Boleto" },
                    { 6, "Transferência Bancária" }
                });

            migrationBuilder.InsertData(
                table: "StatusPagamentos",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Pago" },
                    { 3, "Vencido" },
                    { 4, "Cancelado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasPagamento_Nome",
                table: "CategoriasPagamento",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagamento_Nome",
                table: "FormasPagamento",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_CategoriaPagamentoId",
                table: "Pagamentos",
                column: "CategoriaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_ClienteId",
                table: "Pagamentos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_EmpresaId",
                table: "Pagamentos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_FormaPagamentoId",
                table: "Pagamentos",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_ProfissionalId",
                table: "Pagamentos",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_StatusPagamentoId",
                table: "Pagamentos",
                column: "StatusPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPagamentos_Nome",
                table: "StatusPagamentos",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "CategoriasPagamento");

            migrationBuilder.DropTable(
                name: "FormasPagamento");

            migrationBuilder.DropTable(
                name: "StatusPagamentos");
        }
    }
}
