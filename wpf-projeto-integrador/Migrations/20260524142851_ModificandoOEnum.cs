using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class ModificandoOEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Administrador_NivelAcesso",
                table: "Administradores");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Administrador_NivelAcesso",
                table: "Administradores",
                sql: "NivelAcesso IN ('AdministradorGeral','Atendente','Suporte','Financeiro','Moderador')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Administrador_NivelAcesso",
                table: "Administradores");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Administrador_NivelAcesso",
                table: "Administradores",
                sql: "NivelAcesso IN ('Baixo','Medio','Alto')");
        }
    }
}
