using DiagnosticoMedico.Core.DTOs;
using DiagnosticoMedico.Core.DTOs.ConsultasDTOs;
using DiagnosticoMedico.Core.Mapeadores;
using DiagnosticoMedico.Data;
using DiagnosticoMedico.Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoesController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public ResultadoesController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        // GET: api/Resultadoes
        [HttpGet]
        public async Task<IActionResult> GetResultado()
        {
            var resultados = await (from a in _context.Resultado
                             .Include(r => r.ParametroExamen)
                             .Include(r => r.Muestra)
                             where a.Estado != "Inactivo"
                             select a).ToListAsync();
            var resultadosDTO = resultados.Select(r => r.toResultadoDTO()).ToList();
            return Ok(new { mensaje = $"Se encontraron {resultados.Count} resultados activos.", resultados = resultadosDTO });
        }

        // GET: api/Resultadoes/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetResultado(string code)
        {
            var resultado = await (from a in _context.Resultado
                                   .Include(r => r.ParametroExamen)
                                   .Include(r => r.Muestra)
                                   where a.ResultadoCodigo == code
                                   select a).FirstOrDefaultAsync();

            if (resultado == null)
            {
                return NotFound();
            }
            var resultadoDTO = resultado.toResultadoDTO();
            return Ok(new { mensaje = $"Se encontró el resultado con el código: {code}", resultado = resultadoDTO });
        }




        //Resutlado para una Orden especifica
        // GET: api/Laboratorio/ConsultaMedico/ORD-2026-001
        [HttpGet("ConsultaMedico/{codigoOrden}")]
        public async Task<IActionResult> GetResultadoPorCodigo(string codigoOrden)
        {
            try
            {
                var resultados = await (from res in _context.Resultado
                                        join param in _context.ParametroExamen on res.ParametroExamenId equals param.ParametroExamenId
                                        join exam in _context.Examen on param.ExamenId equals exam.ExamenId
                                        join mue in _context.Muestra on res.MuestraId equals mue.MuestraId
                                        join ordEx in _context.OrdenExamen on mue.MuestraId equals ordEx.MuestraId
                                        join ordLab in _context.OrdenLaboratorio on ordEx.OrdenId equals ordLab.OrdenLaboratorioId
                                        where ordLab.OrdenLaboratorioCodigo == codigoOrden && res.Estado != "Inactivo" && ordEx.EstadoOrdenExamen != "Pendiente"
                                        select new ResultadoPacienteDoctorDTO
                                        {
                                            ResultadoCodigo = res.ResultadoCodigo,
                                            CodigoPaciente = ordLab.PacienteCodigo,
                                            OrdenCodigo = ordLab.OrdenLaboratorioCodigo,
                                            TipoAtencion = ordLab.TipoAtencion,
                                            ExamenNombre = exam.Nombre,
                                            ParametroNombre = param.Nombre,
                                            Valor = res.Valor,
                                            Unidad = param.Unidad,
                                            Referencia = $"{param.ValorMin} - {param.ValorMax}",
                                            Fecha = res.FechaResultado
                                        }).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    return NotFound(new { mensaje = $"No se encontraron resultados para la orden: {codigoOrden}" });
                }

                return Ok(new
                {
                    mensaje = "Resultados encontrados con éxito.",
                    totalParametros = resultados.Count,
                    data = resultados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error en la consulta", detalle = ex.Message });
            }
        }




        // Resultado para documentacion
        // GET: api/Resultado/ConPaciente
        [HttpGet("doctor/consultar-resultados")]
        public async Task<IActionResult> GetResultadosParaDoctor()
        {
            try
            {
                var resultados = await (from res in _context.Resultado
                                        join param in _context.ParametroExamen on res.ParametroExamenId equals param.ParametroExamenId
                                        join exam in _context.Examen on param.ExamenId equals exam.ExamenId
                                        join mue in _context.Muestra on res.MuestraId equals mue.MuestraId
                                        join ordEx in _context.OrdenExamen on mue.MuestraId equals ordEx.MuestraId
                                        join ordLab in _context.OrdenLaboratorio on ordEx.OrdenId equals ordLab.OrdenLaboratorioId
                                        where res.Estado != "Inactivo"
                                        select new ResultadoDetalladoDTO
                                        {
                                            ResultadoCodigo = res.ResultadoCodigo,
                                            PacienteCodigo = ordLab.PacienteCodigo,
                                            OrdenCodigo = ordLab.OrdenLaboratorioCodigo,
                                            TipoAtencion = ordLab.TipoAtencion,
                                            ExamenNombre = exam.Nombre,
                                            ParametroNombre = param.Nombre,
                                            Valor = res.Valor,
                                            Unidad = param.Unidad,
                                            RangoReferencia = $"{param.ValorMin} - {param.ValorMax} {param.Unidad}",
                                            FechaResultado = res.FechaResultado
                                        }).ToListAsync();

                return Ok(new
                {
                    conteo = resultados.Count,
                    data = resultados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar reporte: {ex.Message}");
            }
        }






        //Resultados con todos los datos 
        [HttpGet("completo")]
        public async Task<IActionResult> GetResultadosCompletos()
        {
            var data = await (
                from r in _context.Resultado
                join p in _context.ParametroExamen
                    on r.ParametroExamenId equals p.ParametroExamenId
                join e in _context.Examen
                    on p.ExamenId equals e.ExamenId
                where r.Estado != "Inactivo"
                select new
                {
                    CodigoResultado = r.ResultadoCodigo,
                    Examen = e.Nombre,
                    Parametro = p.Nombre,
                    Valor = r.Valor,
                    Fecha = r.FechaResultado
                }
            ).ToListAsync();

            return Ok(data);
        }



        // PUT: api/Resultadoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<IActionResult> PutResultado(string code, Resultado resultado)
        {
            if (code != resultado.ResultadoCodigo)
            {
                return BadRequest();
            }

            _context.Entry(resultado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultadoExists(code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Resultadoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostResultado(string code, string parametroexamencodigo,string muestracodigo,decimal valor, string equipoCodigo, DateOnly fecharesultado)
        {
            Resultado resultadoBusqueda = await(from a in _context.Resultado
                                        where a.ResultadoCodigo == code
                                        select a).FirstOrDefaultAsync();
            if (resultadoBusqueda != null) { 
                return BadRequest(new { mensaje = "El resultado ya existe." });
            }
            var parametroExamen = await (from p in _context.ParametroExamen
                                        where p.ParametroExamenCodigo == parametroexamencodigo
                                         select p).FirstOrDefaultAsync();
            var muestra = await (from m in _context.Muestra
                               where m.MuestraCodigo == muestracodigo
                               select m).FirstOrDefaultAsync();
            if (parametroExamen == null || muestra == null)
                return BadRequest(new { mensaje = "El parametro de examen o la muestra no existen." });
            Resultado resultado = new Resultado()
            {
                ResultadoCodigo = code,
                ParametroExamenId = parametroExamen.ParametroExamenId,
                MuestraId = muestra.MuestraId,
                Valor = valor,
                EquipoCodigo = equipoCodigo,
                FechaResultado = fecharesultado,
            };
            _context.Resultado.Add(resultado);
            await _context.SaveChangesAsync();

            return Ok(new {mensaje = $"Se creo el Resultado exitosamente con el codigo: {code}"});
        }

        // DELETE: api/Resultadoes/5 
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteResultado(string code)
        {
            var resultado = await(from a in _context.Resultado
                                  where a.ResultadoCodigo == code
                                  select a).FirstOrDefaultAsync();
            if (resultado == null)
            {
                return NotFound();
            }
            resultado.Estado = "Inactivo";
            _context.Resultado.Update(resultado);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool ResultadoExists(string code)
        {
            return _context.Resultado.Any(e => e.ResultadoCodigo == code);
        }
    }
}
