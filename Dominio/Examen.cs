using System.ComponentModel.DataAnnotations;

namespace DiagnosticoMedico.Dominio
{
    public class Examen
    {
        [Key]
        public int ExamenId { get; set; }
        public string ExamenCodigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TiempoProcesamiento { get; set; }
        public bool RequiereAyuno { get; set; }
        public string Estado { get; set; } = "Activo";
        public List<ParametroExamen> ParametroExamenes { get; set; }
        public List<OrdenExamen> OrdenExamenes { get; set; }

    }
}
