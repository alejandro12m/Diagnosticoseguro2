using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagnosticoMedico.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstadoOrden",
                table: "OrdenLaboratorio",
                newName: "EstadoOrdenLaboratorio");

            migrationBuilder.AddColumn<string>(
                name: "EstadoOrdenExamen",
                table: "OrdenExamen",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoOrdenExamen",
                table: "OrdenExamen");

            migrationBuilder.RenameColumn(
                name: "EstadoOrdenLaboratorio",
                table: "OrdenLaboratorio",
                newName: "EstadoOrden");
        }
    }
}
