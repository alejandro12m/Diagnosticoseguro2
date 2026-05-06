using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class OrdenExamenMapeador
    {
        public static OrdenExamenDTO toOrdenExamenDTO(this OrdenExamen ordenExamen)
        {
            if (ordenExamen == null) return null;
            return new OrdenExamenDTO
            {
                OrdenLaboratorioCodigo = ordenExamen.OrdenLaboratorio.OrdenLaboratorioCodigo,
                ExamenCodigo = ordenExamen.Examen.ExamenCodigo,
                MuestraCodigo = ordenExamen.Muestra.MuestraCodigo,
                AreaLaboratorio = ordenExamen.AreaLaboratorio,
                EstadoOrdenExamen = ordenExamen.EstadoOrdenExamen,
            };
        }

    }
}
