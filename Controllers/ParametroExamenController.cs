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
    public class ParametroExamenController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public ParametroExamenController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        // GET: api/ParametroExamen
        [HttpGet]
        public async Task<IActionResult> GetParametroExamen()
        {
            var Parametro= await (from a in _context.ParametroExamen
                             .Include(e => e.Examen)
                              where a.Estado != "Inactivo"
                              select a).ToListAsync();
            var ParametroDTO = Parametro.Select(p => p.MapearParametro()).ToList();
            return Ok(new {mensaje = "Lista completa de parámetros de examen", parametros = ParametroDTO});
        }

        // GET: api/ParametroExamen/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetParametroExamen(string code)
        {
            var parametroExamen = await (from a in _context.ParametroExamen
                                        .Include(e => e.Examen)
                                         where a.ParametroExamenCodigo == code && a.Estado != "Inactivo"
                                         select a).FirstOrDefaultAsync();

            if (parametroExamen == null)
            {
                return NotFound();
            }   
            var parametroExamenDTO = parametroExamen.MapearParametro();
            return Ok(new {mensaje = $"Se encontró el parámetro de examen con el código: {code}", parametro = parametroExamenDTO });
        }
        //Join de parametros con el nombre del examen que se le realizo 
        [HttpGet("parametros-examen")]
        public async Task<IActionResult> GetParametrosConExamen()
        {
            var data = await (
                from p in _context.ParametroExamen
                join e in _context.Examen
                on p.ExamenId equals e.ExamenId
                where p.Estado != "Inactivo" && e.Estado != "Inactivo"
                select new
                {
                    CodigoParametro = p.ParametroExamenCodigo,
                    Parametro = p.Nombre,
                    Unidad = p.Unidad,
                    Examen = e.Nombre
                }
            ).ToListAsync();

            return Ok(data);
        }

        // PUT: api/ParametroExamen/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<IActionResult> PutParametroExamen(string code, ParametroExamen parametroExamen)
        {
            if (code != parametroExamen.ParametroExamenCodigo)
            {
                return BadRequest();
            }

            _context.Entry(parametroExamen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParametroExamenExists(code))
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

        // POST: api/ParametroExamen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParametroExamen>> PostParametroExamen(string code,string codeExamen, string nombre,string unidad, decimal valuemin, decimal valuemax)
        {
            ParametroExamen parametrobuscado = await(from a in _context.ParametroExamen
                                                     where a.ParametroExamenCodigo == code
                                                     select a).FirstOrDefaultAsync();
            if (parametrobuscado != null) { 
                return BadRequest("El ya existe el parametro");
            }
            Examen examen = await (from a in _context.Examen
                             where a.ExamenCodigo == codeExamen
                             select a).FirstOrDefaultAsync();
            if (examen == null) 
            {
                return BadRequest("El examen no encontrado");
            }
            var parametro = new ParametroExamen
            {
                ParametroExamenCodigo = code,
                ExamenId = examen.ExamenId,
                Nombre = nombre,
                Unidad = unidad,
                ValorMin = valuemin,
                ValorMax = valuemax,
                Estado = "Activo"
            };

            _context.ParametroExamen.Add(parametro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParametroExamen", new { mensaje = "Creacion con exito" });
        }

        // DELETE: api/ParametroExamen/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteParametroExamen(string code)
        {
            ParametroExamen parametroExamen = await (from a in _context.ParametroExamen
                                         where a.ParametroExamenCodigo == code && a.Estado != "Inactivo"
                                         select a).FirstOrDefaultAsync();
            if (parametroExamen == null)
            {
                return NotFound();
            }
            parametroExamen.Estado = "Inactivo";
            _context.ParametroExamen.Update(parametroExamen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParametroExamenExists(string code)
        {
            return _context.ParametroExamen.Any(e => e.ParametroExamenCodigo == code);
        }
    }
}
