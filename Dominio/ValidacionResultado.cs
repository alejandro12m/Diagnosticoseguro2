using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoMedico.Dominio
{
    public class ValidacionResultado
    {
        [Key]
        public int ValidacionResultadoId { get; set; }
        public string ValidacionResultadoCodigo { get; set; }
        public int ResultadoId { get; set; }
        public string MedicoCodigo { get; set; }
        public DateOnly FechaValidacion { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; } = "Activo";
        [ForeignKey("ResultadoId")]
        [JsonIgnore]
        public Resultado Resultado { get; set; }

    }
}
