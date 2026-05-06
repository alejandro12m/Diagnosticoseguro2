using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiagnosticoMedico.Dominio
{
    public class Informe
    {
        [Key]
        public int InformeId { get; set; }
        public int OrdenLaboratorioId { get; set; }
        public string InformeCodigo { get; set; }
        public DateOnly FechaEmision { get; set; }
        public string ObservacionesGenerales { get; set; }
        public string Estado { get; set; } = "Activo";
        [ForeignKey("OrdenLaboratorioId")]
        [JsonIgnore]
        public OrdenLaboratorio Orden { get; set; }
    }
}
