namespace DiagnosticoMedico.Core.DTOs
{
    public class OrdenLaboratorioDTO
    {
        public string OrdenLaboratorioCodigo { get; set; }
        public string PacienteCodigo { get; set; }
        public string MedicoCodigo { get; set; }
        public DateOnly FechaOrden { get; set; }
        public string TipoAtencion { get; set; }
        public string Observaciones { get; set; }
        public string EstadoOrden { get; set; } = "Pendiente";

    }
}
