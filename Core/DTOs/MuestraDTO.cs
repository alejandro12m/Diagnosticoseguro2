namespace DiagnosticoMedico.Core.DTOs
{
    public class MuestraDTO
    {
        public string MuestraCodigo { get; set; }
        public string TipoMuestra { get; set; }
        public DateOnly FechaRecoleccion { get; set; }
        public double Volumen { get; set; }
        public string Condicion { get; set; }

    }
}
