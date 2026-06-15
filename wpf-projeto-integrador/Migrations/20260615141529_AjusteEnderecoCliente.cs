using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wpf_projeto_integrador.Migrations
{
    /// <inheritdoc />
    public partial class AjusteEnderecoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Clientes",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Clientes",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Clientes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
