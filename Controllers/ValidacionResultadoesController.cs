using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiagnosticoMedico.Data;
using DiagnosticoMedico.Dominio;
using DiagnosticoMedico.Core.Mapeadores;

namespace DiagnosticoMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidacionResultadoesController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public ValidacionResultadoesController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        // GET: api/ValidacionResultadoes
        [HttpGet]
        public async Task<IActionResult> GetValidacionResultado()
        {
            var validaciones = await (from v in _context.ValidacionResultado
                                      .Include(v => v.Resultado)
                                      where v.Estado != "Inactivo"
                                      select v).ToListAsync();
            var validacionesDTO = validaciones.Select(v => v.MapearValidacion()).ToList();
            return Ok(new { mensaje = "Lista completa de validaciones de resultados", validaciones = validacionesDTO });
        }

        // GET: api/ValidacionResultadoes/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetValidacionResultado(string code)
        {
            var validacionResultado = await (from v in _context.ValidacionResultado
                                             .Include(v => v.Resultado)
                                             where v.ValidacionResultadoCodigo==code
                                             select v).FirstOrDefaultAsync();

            if (validacionResultado == null)
            {
                return NotFound();
            }
                var validacionResultadoDTO = validacionResultado.MapearValidacion();

            return Ok(new { mensaje = $"Se encontró la validación de resultado con el código: {code}", validacion = validacionResultadoDTO });
        }

        // PUT: api/ValidacionResultadoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValidacionResultado(int id, ValidacionResultado validacionResultado)
        {
            if (id != validacionResultado.ValidacionResultadoId)
            {
                return BadRequest();
            }

            _context.Entry(validacionResultado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidacionResultadoExists(id))
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

        // POST: api/ValidacionResultadoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostValidacionResultado(string code, string coderesultado, string medicoCodigo, DateOnly fechaValidacion, string observacion)
        {
            var validar = await(from r in _context.ValidacionResultado
                                 where r.ValidacionResultadoCodigo == code
                                 select r).FirstOrDefaultAsync();
            if (validar != null) { 
                return BadRequest("La validación de resultado con el código proporcionado ya existe.");
            }
            
            var resultado = await (from r in _context.Resultado
                                  where r.ResultadoCodigo == coderesultado
                                  select r).FirstOrDefaultAsync();
            if (resultado == null)
            {
                return BadRequest("Resultado no encontrado con el código proporcionado.");
            }
            var validacionNueva = new ValidacionResultado
            {
                ValidacionResultadoCodigo = code,
                ResultadoId = resultado.ResultadoId,
                MedicoCodigo = medicoCodigo,
                FechaValidacion = fechaValidacion,
                Observaciones = observacion
            };



            _context.ValidacionResultado.Add(validacionNueva);
            await _context.SaveChangesAsync();

            return Ok(new {mensaje = $"Validación de resultado creada correctamente con el código: {validacionNueva.ValidacionResultadoCodigo}" });
                
                }

        // DELETE: api/ValidacionResultadoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValidacionResultado(int id)
        {
            var validacionResultado = await _context.ValidacionResultado.FindAsync(id);
            if (validacionResultado == null)
            {
                return NotFound();
            }

            _context.ValidacionResultado.Remove(validacionResultado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ValidacionResultadoExists(int id)
        {
            return _context.ValidacionResultado.Any(e => e.ValidacionResultadoId == id);
        }
    }
}
