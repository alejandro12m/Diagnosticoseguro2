using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class ParametroExamenMapeador
    {
            public static ParametroExamenDTO MapearParametro(this ParametroExamen parametro)
            {
                return new ParametroExamenDTO
                {
                    ParametroCodigo = parametro.ParametroExamenCodigo,
                    ExamenCodigo = parametro.Examen.ExamenCodigo,
                    Nombre = parametro.Nombre,
                    Unidad = parametro.Unidad,
                    ValorMin = parametro.ValorMin,
                    ValorMax = parametro.ValorMax
                };
            }
    }
}
