namespace DiagnosticoMedico.Core.DTOs.ConsultasDTOs
{
    public class VerEstadosDeOrdenMasExamenesDTO
    {
        public string OrdenLaboratorioCodigo { get; set; }
        public string NombreExamen { get; set; }
        public string TipoMuestra { get; set; } 
        public string EstadoOrdenExamen { get; set; }
    }
}
