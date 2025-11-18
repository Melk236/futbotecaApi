using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutbotecaApi.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_seguimientos_Usuarios_UsuarioId",
                table: "seguimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_seguimientos_Usuarios_UsuarioId1",
                table: "seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_seguimientos_UsuarioId",
                table: "seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_seguimientos_UsuarioId1",
                table: "seguimientos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "seguimientos");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "seguimientos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "seguimientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId1",
                table: "seguimientos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_seguimientos_UsuarioId",
                table: "seguimientos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_seguimientos_UsuarioId1",
                table: "seguimientos",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_seguimientos_Usuarios_UsuarioId",
                table: "seguimientos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_seguimientos_Usuarios_UsuarioId1",
                table: "seguimientos",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
