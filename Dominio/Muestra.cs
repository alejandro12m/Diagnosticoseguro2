using System.ComponentModel.DataAnnotations;

namespace DiagnosticoMedico.Dominio
{
    public class Muestra
    {
        [Key]
        public int MuestraId { get; set; }
        public string MuestraCodigo { get; set; }
        public string TipoMuestra { get; set; }
        public DateOnly FechaRecoleccion { get; set; }
        public double Volumen { get; set; }
        public string Condicion { get; set; }
        public List<OrdenExamen> OrdenExamen { get; set; }
        public List<Resultado> Resultados { get; set; } 

    }   
}
