using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class InformeMapeador
    {
        public static InformeDTO MapearADTO(this Informe informe)
        {
            return new InformeDTO
            {
                InformeCodigo = informe.InformeCodigo,
                OrdenCodigo = informe.Orden.OrdenLaboratorioCodigo,
                FechaEmision = informe.FechaEmision,
                Observaciones = informe.ObservacionesGenerales
            };
        }
    }
}
