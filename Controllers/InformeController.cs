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
    public class InformeController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public InformeController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        // GET: api/Informes
        [HttpGet]
        public async Task<IActionResult> GetInforme()
        {
            var informes = await(from i in _context.Informe.Include(i=> i.Orden)
                                 where i.Estado != "Inactivo"
                                 select i).ToListAsync();
            var informeDTOs = informes.Select(i => InformeMapeador.MapearADTO(i)).ToList();

            return Ok(new { mensaje = $"Esta es la lista completa de los informes sobre las ordenes ",  informeDTOs });
            
        }

        // GET: api/Informes/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetInforme(string code)
        {
            var informe = await (from i in _context.Informe.Include(i => i.Orden)
                                 where i.InformeCodigo == code && i.Estado != "Inactivo"
                                 select i).FirstOrDefaultAsync();

            if (informe == null)
            {
                return NotFound();
            }
            var informeDTO = InformeMapeador.MapearADTO(informe);
            return Ok(new {mensaje = $"Informe encontrado con código {code}", informe = informeDTO});
        }

        // PUT: api/Informes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("actualizar/{code}/{ordenCode}/{fechaEmision}/{observacionesGenerales}")]
        public async Task<IActionResult> PutInforme(string code, DateOnly fechaEmision, string observacionesGenerales)
        {
            var informe = await (from i in _context.Informe
                             where i.InformeCodigo == code 
                             select i).FirstOrDefaultAsync();
            if (informe == null)
            {
                return NotFound();
            }
            informe.FechaEmision = fechaEmision;
            informe.ObservacionesGenerales = observacionesGenerales;
            await _context.SaveChangesAsync();
            return Ok("Actualizado");

        }

        // POST: api/Informes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CrearInforme")]
        public async Task<IActionResult> PostInforme(string code, string ordenCode , DateOnly fechaEmision, string observacionesGenerales)
        {
            var OrdenLaboratorio = await(from o in _context.OrdenLaboratorio
                                    where o.OrdenLaboratorioCodigo == ordenCode
                                    select o ).FirstOrDefaultAsync();
            if (OrdenLaboratorio == null) { 
                return BadRequest("Orden de laboratorio no encontrada.");
            }
                
            var informe = new Informe
            {
                InformeCodigo = code,
                OrdenLaboratorioId = OrdenLaboratorio.OrdenLaboratorioId,
                FechaEmision = fechaEmision,
                ObservacionesGenerales = observacionesGenerales,
            };
            _context.Informe.Add(informe);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = $"Informe creado correctamente con el código: {informe.InformeCodigo}" });

        }

        // DELETE: api/Informes/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteInforme(string code)
        {
            var informe = await (from i in _context.Informe
                                 where i.InformeCodigo == code
                                 select i).FirstOrDefaultAsync();
            if (informe == null)
            {
                return NotFound();
            }
            
            informe.Estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Informe con código {code} se ha eliminado." });
        }

        private bool InformeExists(int id)
        {
            return _context.Informe.Any(e => e.InformeId == id);
        }
    }
}
