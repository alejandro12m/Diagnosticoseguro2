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
    public class MuestrasController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public MuestrasController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        // GET: api/Muestras
        [HttpGet]
        public async Task<IActionResult> GetMuestra()
        {
            var muestras = await (from m in _context.Muestra
                            where m.Condicion != "Inactiva"
                            select m).ToListAsync();
            var muestrasDTO = muestras.Select(m => m.MapearMuestra()).ToList();
            return Ok(new { mensaje = $"Se encontraron {muestras.Count} muestras activas.", muestras = muestrasDTO });
        }

        // GET: api/Muestras/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetMuestraCodigo(string code)
        {
            var muestra = await (from m in _context.Muestra
                                 where m.MuestraCodigo == code && m.Condicion != "Inactiva"
                                 select m).FirstOrDefaultAsync();

            if (muestra == null)
            {
                return BadRequest($"No se encontro la muestra con el codigo -> {code}");
            }
            var muestraDTO = muestra.MapearMuestra();
            return Ok(new {mensaje = $"Se encontro la muestra con el codigo -> {code}", muestra = muestraDTO });
        }



        //Muestras por tipo
        [HttpGet("muestras-por-tipo")]
        public async Task<IActionResult> GetMuestrasPorTipo()
        {
            var data = await (
                from m in _context.Muestra
                group m by m.TipoMuestra into g
                select new
                {
                    Tipo = g.Key,
                    Total = g.Count()
                }
            ).ToListAsync();

            return Ok(data);
        }


        // PUT: api/Muestras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMuestra(int id, Muestra muestra)
        {
            if (id != muestra.MuestraId)
            {
                return BadRequest();
            }

            _context.Entry(muestra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MuestraExists(id))
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

        // POST: api/Muestras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Muestra>> PostMuestra(string code, string TipoMuestra, DateOnly FechaRecoleccion, double Volumen, string Codicion)
        {

            var muestraComprobante = await (from m in _context.Muestra
                                            where m.MuestraCodigo == code
                                            select m
                                           ).FirstOrDefaultAsync();
            if (muestraComprobante != null)
            {
                return BadRequest("El código de muestra ya existe. Por favor, ingrese un código único.");
            }

            Muestra muestra = new Muestra
            {
                MuestraCodigo = code,
                TipoMuestra = TipoMuestra,
                FechaRecoleccion = FechaRecoleccion,
                Volumen = Volumen,
                Condicion = Codicion
            };

                _context.Muestra.Add(muestra);
            await _context.SaveChangesAsync();

            return Ok(new {mensaje = $"Se creo la muestra de manera correcta con el codigo de -> {code}"});
        }

        // DELETE: api/Muestras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMuestra(int id)
        {
            var muestra = await _context.Muestra.FindAsync(id);
            if (muestra == null)
            {
                return NotFound();
            }

            _context.Muestra.Remove(muestra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MuestraExists(int id)
        {
            return _context.Muestra.Any(e => e.MuestraId == id);
        }
    }
}
