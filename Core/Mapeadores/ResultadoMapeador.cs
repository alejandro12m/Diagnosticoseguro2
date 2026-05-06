using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class ResultadoMapeador
    {
        public static ResultadoDTO toResultadoDTO(this Resultado resultado)
        {
            if (resultado == null) return null;
            return new ResultadoDTO
            {
                ResultadoCodigo = resultado.ResultadoCodigo,
                MuestraCodigo = resultado.Muestra.MuestraCodigo,
                ParametroExamenCodigo = resultado.ParametroExamen.ParametroExamenCodigo,
                Valor = resultado.Valor,
                EquipoCodigo = resultado.EquipoCodigo,
                FechaResultado = resultado.FechaResultado
            };
        }

    }
}
