namespace DiagnosticoMedico.Core.DTOs
{
    public class ParametroExamenDTO
    {
        public string ParametroCodigo { get; set; }
        public string ExamenCodigo { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public decimal ValorMin { get; set; }
        public decimal ValorMax { get; set; }
    }
}
