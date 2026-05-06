using System.ComponentModel.DataAnnotations;

namespace DiagnosticoMedico.Dominio
{
    public class OrdenLaboratorio
    {
        [Key]
        public int OrdenLaboratorioId { get; set; }
        public string OrdenLaboratorioCodigo { get; set; }
        public string PacienteCodigo { get; set; }
        public string MedicoCodigo { get; set; }
        public DateOnly FechaOrden { get; set; }
        public string TipoAtencion { get; set; }
        public string Observaciones { get; set; }
        public string EstadoOrdenLaboratorio { get; set; } = "Pendiente";
        public string Estado { get; set; } = "Activo";
        public List<Informe> Informes { get; set; }
        public List<OrdenExamen> OrdenExamenesOrdenExamenes { get; set; }
    }
}
