namespace DiagnosticoMedico.Core.DTOs.ConsultasDTOs
{
    public class ResultadoEspecificoDTO
    {
        public string PacienteCodigo { get; set; }
        public string OrdenCodigo { get; set; }
        public string PacienteNombre { get; set; } 
        public string Examen { get; set; }
        public string Parametro { get; set; }
        public decimal Valor { get; set; }
        public string Unidad { get; set; }
        public string Referencia { get; set; }
        public DateOnly Fecha { get; set; }

    }
}
