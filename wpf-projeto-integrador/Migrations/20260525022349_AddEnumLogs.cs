using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsSistema_Usuarios_UsuarioId",
                table: "LogsSistema");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "LogsSistema",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Tela",
                table: "LogsSistema",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Erro",
                table: "LogsSistema",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sucesso",
                table: "LogsSistema",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TipoAcao",
                table: "LogsSistema",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LogsSistema_Usuarios_UsuarioId",
                table: "LogsSistema",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsSistema_Usuarios_UsuarioId",
                table: "LogsSistema");

            migrationBuilder.DropColumn(
                name: "Erro",
                table: "LogsSistema");

            migrationBuilder.DropColumn(
                name: "Sucesso",
                table: "LogsSistema");

            migrationBuilder.DropColumn(
                name: "TipoAcao",
                table: "LogsSistema");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "LogsSistema",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tela",
                table: "LogsSistema",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LogsSistema_Usuarios_UsuarioId",
                table: "LogsSistema",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
