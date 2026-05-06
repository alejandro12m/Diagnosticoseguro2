using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagnosticoMedico.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Informe_OrdenLaboratorio_OrdenId",
                table: "Informe");

            migrationBuilder.DropIndex(
                name: "IX_Informe_OrdenId",
                table: "Informe");

            migrationBuilder.DropColumn(
                name: "OrdenId",
                table: "Informe");

            migrationBuilder.CreateIndex(
                name: "IX_Informe_OrdenLaboratorioId",
                table: "Informe",
                column: "OrdenLaboratorioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Informe_OrdenLaboratorio_OrdenLaboratorioId",
                table: "Informe",
                column: "OrdenLaboratorioId",
                principalTable: "OrdenLaboratorio",
                principalColumn: "OrdenLaboratorioId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Informe_OrdenLaboratorio_OrdenLaboratorioId",
                table: "Informe");

            migrationBuilder.DropIndex(
                name: "IX_Informe_OrdenLaboratorioId",
                table: "Informe");

            migrationBuilder.AddColumn<int>(
                name: "OrdenId",
                table: "Informe",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Informe_OrdenId",
                table: "Informe",
                column: "OrdenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Informe_OrdenLaboratorio_OrdenId",
                table: "Informe",
                column: "OrdenId",
                principalTable: "OrdenLaboratorio",
                principalColumn: "OrdenLaboratorioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
