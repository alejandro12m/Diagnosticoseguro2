
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DiagnosticoMedico.Dominio;
using DiagnosticoMedico.Core.DTOs;

namespace DiagnosticoMedico.Core.Mapeadores
{
    public static class OrdenLaboratorioMapeador
    {
        public static OrdenLaboratorioDTO toOrdenLaboratorioDTO(this OrdenLaboratorio ordenLaboratorio) {

            if (ordenLaboratorio == null) return null;
            return new OrdenLaboratorioDTO
            {
                OrdenLaboratorioCodigo = ordenLaboratorio.OrdenLaboratorioCodigo,
                PacienteCodigo = ordenLaboratorio.PacienteCodigo,
                MedicoCodigo = ordenLaboratorio.MedicoCodigo,
                FechaOrden = ordenLaboratorio.FechaOrden,
                TipoAtencion = ordenLaboratorio.TipoAtencion,
                Observaciones = ordenLaboratorio.Observaciones,
                EstadoOrden = ordenLaboratorio.EstadoOrdenLaboratorio
            };



        }

    }
}
