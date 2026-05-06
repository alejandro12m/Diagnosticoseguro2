using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagnosticoMedico.Migrations
{
    /// <inheritdoc />
    public partial class m8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultadoCodigo",
                table: "ValidacionResultado");

            migrationBuilder.RenameColumn(
                name: "EquipoId",
                table: "Resultado",
                newName: "EquipoCodigo");

            migrationBuilder.AddColumn<string>(
                name: "EstadoOrden",
                table: "OrdenLaboratorio",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoOrden",
                table: "OrdenLaboratorio");

            migrationBuilder.RenameColumn(
                name: "EquipoCodigo",
                table: "Resultado",
                newName: "EquipoId");

            migrationBuilder.AddColumn<string>(
                name: "ResultadoCodigo",
                table: "ValidacionResultado",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
