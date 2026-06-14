using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class AjusteProfissional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Profissionais");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Profissionais");

            migrationBuilder.RenameColumn(
                name: "Rua",
                table: "Profissionais",
                newName: "Especialidade");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Profissionais",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Profissionais",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profissionais_EmpresaId",
                table: "Profissionais",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profissionais_Empresas_EmpresaId",
                table: "Profissionais",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profissionais_Empresas_EmpresaId",
                table: "Profissionais");

            migrationBuilder.DropIndex(
                name: "IX_Profissionais_EmpresaId",
                table: "Profissionais");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Profissionais");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Profissionais");

            migrationBuilder.RenameColumn(
                name: "Especialidade",
                table: "Profissionais",
                newName: "Rua");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Profissionais",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Profissionais",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
