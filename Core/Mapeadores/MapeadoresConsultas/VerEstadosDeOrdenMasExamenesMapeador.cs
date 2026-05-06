using DiagnosticoMedico.Core.DTOs.ConsultasDTOs;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Core.Mapeadores.MapeadoresConsultas
{
    public static class VerEstadosDeOrdenMasExamenesMapeador
    {
        public static VerEstadosDeOrdenMasExamenesDTO MapearDoctores(this OrdenExamen ordenExamen)
        {
            return new VerEstadosDeOrdenMasExamenesDTO
            {
                OrdenLaboratorioCodigo = ordenExamen.OrdenLaboratorio.OrdenLaboratorioCodigo,
                NombreExamen = ordenExamen.Examen.Nombre,
                TipoMuestra = ordenExamen.Muestra.TipoMuestra,
                EstadoOrdenExamen = ordenExamen.EstadoOrdenExamen,
            };
        }

    }
}
