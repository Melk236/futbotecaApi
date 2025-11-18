using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;


#nullable disable

namespace FutbotecaApi.Migrations
{
    /// <inheritdoc />
    public partial class nuevo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Asegurar que 'Id' es AUTO_INCREMENT
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Seguimientos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // Índice único para evitar duplicados
            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_SeguidorId_SeguidoId",
                table: "Seguimientos",
                columns: new[] { "SeguidorId", "SeguidoId" },
                unique: true);

            // Clave foránea
            migrationBuilder.AddForeignKey(
                name: "FK_Seguimientos_Usuarios_SeguidoId",
                table: "Seguimientos",
                column: "SeguidoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seguimientos_Usuarios_SeguidoId",
                table: "Seguimientos");

            migrationBuilder.DropIndex(
                name: "IX_Seguimientos_SeguidorId_SeguidoId",
                table: "Seguimientos");

            // Revertir AUTO_INCREMENT si se desea
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Seguimientos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}