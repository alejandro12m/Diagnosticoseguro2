namespace DiagnosticoMedico.Core.DTOs
{
    public class InformeDTO
    {
        public string InformeCodigo { get; set; }
        public string OrdenCodigo { get; set; }

        public DateOnly FechaEmision { get; set; }
        public string Observaciones { get; set; }

    }
}
