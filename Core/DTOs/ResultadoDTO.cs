namespace DiagnosticoMedico.Core.DTOs
{
    public class ResultadoDTO
    {
        public string ResultadoCodigo { get; set; }
        public string MuestraCodigo { get; set; }
        public string ParametroExamenCodigo { get; set; }
        public decimal Valor { get; set; }
        public string EquipoCodigo { get; set; }
        public DateOnly FechaResultado { get; set; }
    }
}