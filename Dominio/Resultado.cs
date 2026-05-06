using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoMedico.Dominio
{
    public class Resultado
    {
        [Key]
        public int ResultadoId { get; set; }
        public string ResultadoCodigo { get; set; }
        public int MuestraId { get; set; }
        public int ParametroExamenId { get; set; }
        public decimal Valor { get; set; }
        public string EquipoCodigo { get; set; }
        public DateOnly FechaResultado { get; set; }
        public string Estado { get; set; } = "Activo";

        [ForeignKey("ParametroExamenId")]
        public ParametroExamen ParametroExamen { get; set; }

        [ForeignKey("MuestraId")]
        public Muestra Muestra { get; set; }
        public List<ValidacionResultado> Validaciones { get; set; }

    }
}
