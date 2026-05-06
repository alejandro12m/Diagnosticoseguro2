using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoMedico.Dominio
{
    public class OrdenExamen
    {
        [Key]
        public int OrdenExamenId { get; set; }
        public int OrdenId { get; set; }
        public int ExamenId { get; set; }
        public int MuestraId { get; set; }
        public string AreaLaboratorio { get; set; }
        public string EstadoOrdenExamen { get; set; } = "Pendiente";
        public string Estado { get; set; } = "Activo";
        [ForeignKey("OrdenId")]
        public OrdenLaboratorio OrdenLaboratorio { get; set; }
        [ForeignKey("ExamenId")]
        public Examen Examen { get; set; }
        [ForeignKey("MuestraId")]
        public Muestra Muestra { get; set; }
    }
}
