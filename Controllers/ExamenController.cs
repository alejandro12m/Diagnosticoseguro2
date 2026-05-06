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
    public class ExamenController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public ExamenController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        //// GET: api/Examen
        [HttpGet]
        public async Task<IActionResult> GetExamen()
        {
            var Emanenes = await (from a in _context.Examen
                             where a.Estado != "Inactivo"
                             select a).ToListAsync();
            var examenesDTO = Emanenes.Select(e => e.MapearExamen()).ToList();
            return Ok(new { mensaje = $"Se encontraron {Emanenes.Count} examenes activos.", examenes = examenesDTO });

        }

         

        // GET: api/Examen/
        [HttpGet("{code}")]
        public async Task<IActionResult> GetExamen(string code)
        {
         var Examen = await( from a in _context.Examen
                             where a.ExamenCodigo == code && a.Estado != "Inactivo"
                             select a).FirstOrDefaultAsync();
            if (Examen == null)
            {
                return NotFound();
            }
            var examenDTO = Examen.MapearExamen();
            return Ok(new { mensaje = $"Se encontro el examen con el codigo -> {code}", examen=examenDTO });
        }

        //2. Agrupación con conteo (GROUP BY + COUNT)

        [HttpGet("cantidad-por-examen")]
        public async Task<IActionResult> GetCantidadExamenes()
        {
            var data = await (
                from oe in _context.OrdenExamen
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                group e by e.Nombre into g
                select new
                {
                    Examen = g.Key,
                    Cantidad = g.Count()
                }
            ).ToListAsync();

            return Ok(data);
        }



        //Exámenes más solicitados
        [HttpGet("examenes-mas-solicitados")]
        public async Task<IActionResult> GetMasSolicitados()
        {
            var data = await (
                from oe in _context.OrdenExamen
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                group e by e.Nombre into g
                select new
                {
                    Examen = g.Key,
                    Total = g.Count()
                }
            ).OrderByDescending(x => x.Total).ToListAsync();

            return Ok(data);
        }


        // PUT: api/Examen/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<IActionResult> PutExamen(string code, Examen examen)
        {
            if (code != examen.ExamenCodigo)
            {
                return BadRequest();
            }

            _context.Entry(examen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamenExists(examen.ExamenCodigo))
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

        // POST: api/Examen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostExamen(string code,string nombre, string descripcion, int tiempoProcesamiento, bool ayuno)
        {
            var examen1 = await (from a in _context.Examen
                                  where a.ExamenCodigo == code
                                  select a).FirstOrDefaultAsync();
            if (examen1 != null) { 
                return BadRequest("El código del examen ya existe en la base de datos");
            }
            Examen examen2 = new Examen()
            {
                ExamenCodigo = code,
                Nombre = nombre,
                Descripcion = descripcion,
                TiempoProcesamiento = tiempoProcesamiento,
                RequiereAyuno = ayuno
            };
            _context.Examen.Add(examen2);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = $"Se creo el examen de manera correcta con el codigo de -> {code}" });
            }

        // DELETE: api/Examen/
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteExamen(string code)
        {
            var examen = await (from a in _context.Examen
                                where a.ExamenCodigo == code
                                select a).FirstOrDefaultAsync();
            if (examen == null) {
                return NotFound();
            }

            examen.Estado = "Inactivo";
            _context.Examen.Update(examen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamenExists(string code)
        {
            return _context.Examen.Any(e => e.ExamenCodigo == code);
        }
    }
}
