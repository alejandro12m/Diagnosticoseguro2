namespace DiagnosticoMedico.Core.DTOs.ConsultasDTOs
{
    public class ResultadoDetalladoDTO
    {
        public string ResultadoCodigo { get; set; }
        public string PacienteCodigo { get; set; }
        public string OrdenCodigo { get; set; }
        public string TipoAtencion { get; set; } 

        public string ExamenNombre { get; set; }
        public string ParametroNombre { get; set; }
        public decimal Valor { get; set; }
        public string Unidad { get; set; }
        public string RangoReferencia { get; set; }
        public DateOnly FechaResultado { get; set; }
    }
}
