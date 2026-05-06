using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DiagnosticoMedico.Dominio
{
    public class ParametroExamen
    {
        [Key]
        public int ParametroExamenId { get; set; }
        public string ParametroExamenCodigo { get; set; }
        public int ExamenId { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public decimal ValorMin { get; set; }
        public decimal ValorMax { get;set; }
        public string Estado { get; set; } = "Activo";
        public List<Resultado> Resultados { get; set; }

        [ForeignKey("ExamenId")]
        [JsonIgnore]
        public Examen Examen { get; set; }

    }
}
