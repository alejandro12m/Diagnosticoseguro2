using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class MuestraMapeador
    {
        public static MuestraDTO MapearMuestra(this Muestra muestra)
        {
            return new MuestraDTO
            {
                MuestraCodigo = muestra.MuestraCodigo,
                TipoMuestra = muestra.TipoMuestra,
                FechaRecoleccion = muestra.FechaRecoleccion,
                Volumen = muestra.Volumen,
                Condicion = muestra.Condicion
            };
        }
    }
}
