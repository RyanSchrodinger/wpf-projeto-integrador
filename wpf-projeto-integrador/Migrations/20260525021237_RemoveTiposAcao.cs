using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTiposAcao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsSistema_TiposAcao_TipoAcaoId",
                table: "LogsSistema");

            migrationBuilder.DropTable(
                name: "TiposAcao");

            migrationBuilder.DropIndex(
                name: "IX_LogsSistema_TipoAcaoId",
                table: "LogsSistema");

            migrationBuilder.DropColumn(
                name: "TipoAcaoId",
                table: "LogsSistema");

            migrationBuilder.RenameColumn(
                name: "Entidade",
                table: "LogsSistema",
                newName: "EntidadeAfetada");

            migrationBuilder.AddColumn<string>(
                name: "Tela",
                table: "LogsSistema",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tela",
                table: "LogsSistema");

            migrationBuilder.RenameColumn(
                name: "EntidadeAfetada",
                table: "LogsSistema",
                newName: "Entidade");

            migrationBuilder.AddColumn<int>(
                name: "TipoAcaoId",
                table: "LogsSistema",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TiposAcao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAcao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsSistema_TipoAcaoId",
                table: "LogsSistema",
                column: "TipoAcaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposAcao_Nome",
                table: "TiposAcao",
                column: "Nome",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LogsSistema_TiposAcao_TipoAcaoId",
                table: "LogsSistema",
                column: "TipoAcaoId",
                principalTable: "TiposAcao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
