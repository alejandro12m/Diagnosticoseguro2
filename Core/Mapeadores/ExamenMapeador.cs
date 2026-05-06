using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class ExamenMapeador
    {
        public static ExamenDTO MapearExamen(this Examen examen)
        {
            return new ExamenDTO
            {
                ExamenCodigo = examen.ExamenCodigo,
                Nombre = examen.Nombre,
                Descripcion = examen.Descripcion,
                TiempoProcesamiento = examen.TiempoProcesamiento,
                RequiereAyuno = examen.RequiereAyuno
            };
        }
    }
}
