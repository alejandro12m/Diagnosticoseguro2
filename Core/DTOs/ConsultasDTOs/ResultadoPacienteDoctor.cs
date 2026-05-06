namespace DiagnosticoMedico.Core.DTOs.ConsultasDTOs
{
    public class ResultadoPacienteDoctorDTO
    {
        public string ResultadoCodigo { get; set; }
        public string CodigoPaciente { get; set; }
        public string OrdenCodigo { get; set; }
        public string TipoAtencion { get; set; }
        public string ExamenNombre { get; set; }
        public string ParametroNombre { get; set; }
        public decimal Valor { get; set; }
        public string Unidad { get; set; }
        public string Referencia { get; set; }
        public DateOnly Fecha { get; set; }
    }
}
