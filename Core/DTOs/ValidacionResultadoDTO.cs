namespace DiagnosticoMedico.Core.DTOs
{
    public class ValidacionResultadoDTO
    {
        public string ValidacionResultadoCodigo {  get; set; }
        public string ResultadoCodigo { get; set; }
        public string MedicoCodigo { get; set; }
        public DateOnly FechaValidacion { get; set; }
        public string Observaciones { get; set; }
        
    }
}
