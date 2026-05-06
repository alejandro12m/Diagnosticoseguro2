using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiagnosticoMedico.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔴 Eliminar FK solo si existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_Informe_OrdenLaboratorio_OrdenId'
    ) THEN
        ALTER TABLE ""Informe"" DROP CONSTRAINT ""FK_Informe_OrdenLaboratorio_OrdenId"";
    END IF;
END $$;");

            // 🔴 Eliminar índice solo si existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM pg_indexes 
        WHERE indexname = 'IX_Informe_OrdenId'
    ) THEN
        DROP INDEX ""IX_Informe_OrdenId"";
    END IF;
END $$;");

            // 🔴 Eliminar columna solo si existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name='Informe' AND column_name='OrdenId'
    ) THEN
        ALTER TABLE ""Informe"" DROP COLUMN ""OrdenId"";
    END IF;
END $$;");

            // ✅ Crear índice nuevo (si no existe)
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes 
        WHERE indexname = 'IX_Informe_OrdenLaboratorioId'
    ) THEN
        CREATE INDEX ""IX_Informe_OrdenLaboratorioId"" 
        ON ""Informe"" (""OrdenLaboratorioId"");
    END IF;
END $$;");

            // ✅ Crear nueva FK solo si no existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_Informe_OrdenLaboratorio_OrdenLaboratorioId'
    ) THEN
        ALTER TABLE ""Informe""
        ADD CONSTRAINT ""FK_Informe_OrdenLaboratorio_OrdenLaboratorioId""
        FOREIGN KEY (""OrdenLaboratorioId"")
        REFERENCES ""OrdenLaboratorio""(""OrdenLaboratorioId"")
        ON DELETE CASCADE;
    END IF;
END $$;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 🔴 Eliminar nueva FK si existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_Informe_OrdenLaboratorio_OrdenLaboratorioId'
    ) THEN
        ALTER TABLE ""Informe"" DROP CONSTRAINT ""FK_Informe_OrdenLaboratorio_OrdenLaboratorioId"";
    END IF;
END $$;");

            // 🔴 Eliminar índice nuevo si existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM pg_indexes 
        WHERE indexname = 'IX_Informe_OrdenLaboratorioId'
    ) THEN
        DROP INDEX ""IX_Informe_OrdenLaboratorioId"";
    END IF;
END $$;");

            // 🔴 Agregar columna solo si no existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name='Informe' AND column_name='OrdenId'
    ) THEN
        ALTER TABLE ""Informe"" ADD COLUMN ""OrdenId"" integer NOT NULL DEFAULT 0;
    END IF;
END $$;");

            // 🔴 Crear índice viejo si no existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes 
        WHERE indexname = 'IX_Informe_OrdenId'
    ) THEN
        CREATE INDEX ""IX_Informe_OrdenId"" 
        ON ""Informe"" (""OrdenId"");
    END IF;
END $$;");

            // 🔴 Crear FK vieja si no existe
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_Informe_OrdenLaboratorio_OrdenId'
    ) THEN
        ALTER TABLE ""Informe""
        ADD CONSTRAINT ""FK_Informe_OrdenLaboratorio_OrdenId""
        FOREIGN KEY (""OrdenId"")
        REFERENCES ""OrdenLaboratorio""(""OrdenLaboratorioId"")
        ON DELETE CASCADE;
    END IF;
END $$;");
        }
    }
}