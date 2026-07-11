using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class GestaoServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "Pagamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Pendente")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    IdServico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.IdServico);
                    table.ForeignKey(
                        name: "FK_Servicos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicosPedidos",
                columns: table => new
                {
                    IdItem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ServicoId = table.Column<int>(type: "int", nullable: false),
                    ProfissionalId = table.Column<int>(type: "int", nullable: false),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "-"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Pendente")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicosPedidos", x => x.IdItem);
                    table.ForeignKey(
                        name: "FK_ServicosPedidos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicosPedidos_Profissionais_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicosPedidos_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "IdServico",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicoPedidoId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.CheckConstraint("CK_Avaliacao_Nota", "[Nota] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_ServicosPedidos_ServicoPedidoId",
                        column: x => x.ServicoPedidoId,
                        principalTable: "ServicosPedidos",
                        principalColumn: "IdItem",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_ClienteId",
                table: "Avaliacoes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_ServicoPedidoId",
                table: "Avaliacoes",
                column: "ServicoPedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_EmpresaId_Nome",
                table: "Servicos",
                columns: new[] { "EmpresaId", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPedidos_PedidoId",
                table: "ServicosPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPedidos_ProfissionalId",
                table: "ServicosPedidos",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPedidos_ServicoId",
                table: "ServicosPedidos",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Pedidos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "IdPedido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Pedidos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "ServicosPedidos");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Pagamentos");
        }
    }
}
