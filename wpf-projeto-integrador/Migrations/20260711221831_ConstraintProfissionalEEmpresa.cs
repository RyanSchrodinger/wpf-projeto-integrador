using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class ConstraintProfissionalEEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Empresas_EmpresaId",
                table: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_EmpresaId_Nome",
                table: "Servicos");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "Servicos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProfissionalId",
                table: "Servicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_EmpresaId",
                table: "Servicos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_ProfissionalId",
                table: "Servicos",
                column: "ProfissionalId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Servico_Prestador",
                table: "Servicos",
                sql: "(\r\n                [EmpresaId] IS NOT NULL\r\n                AND [ProfissionalId] IS NULL\r\n              )\r\n              OR\r\n              (\r\n                [EmpresaId] IS NULL\r\n                AND [ProfissionalId] IS NOT NULL\r\n              )");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Empresas_EmpresaId",
                table: "Servicos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Profissionais_ProfissionalId",
                table: "Servicos",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Empresas_EmpresaId",
                table: "Servicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Profissionais_ProfissionalId",
                table: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_EmpresaId",
                table: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_ProfissionalId",
                table: "Servicos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Servico_Prestador",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "ProfissionalId",
                table: "Servicos");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "Servicos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_EmpresaId_Nome",
                table: "Servicos",
                columns: new[] { "EmpresaId", "Nome" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Empresas_EmpresaId",
                table: "Servicos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
