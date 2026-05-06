namespace DiagnosticoMedico.Core.DTOs
{
    public class ExamenDTO
    {
        public string ExamenCodigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TiempoProcesamiento { get; set; }
        public bool RequiereAyuno { get; set; } = true;
    }
}
