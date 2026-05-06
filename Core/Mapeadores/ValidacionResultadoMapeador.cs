using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class ValidacionResultadoMapeador
    {
        public static ValidacionResultadoDTO MapearValidacion(this ValidacionResultado validacion)
        {
            return new ValidacionResultadoDTO
            {
                ValidacionResultadoCodigo = validacion.ValidacionResultadoCodigo,
                ResultadoCodigo = validacion.Resultado.ResultadoCodigo,
                MedicoCodigo = validacion.MedicoCodigo,
                FechaValidacion = validacion.FechaValidacion,
                Observaciones = validacion.Observaciones
            };
         
        }
    }
}
