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
using DiagnosticoMedico.Core.Mapeadores.MapeadoresConsultas;

namespace DiagnosticoMedico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenExamenController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public OrdenExamenController(DiagnosticoMedicoContext context)
        {
            _context = context;
        }

        // GET: api/OrdenExamen
        [HttpGet]
        public async Task<IActionResult> GetOrdenExamen()
        {
            var ordenExamenes = await (from o in _context.OrdenExamen
                                      .Include(o => o.OrdenLaboratorio)
                                      .Include(o => o.Examen)
                                      .Include(o => o.Muestra)
                                       where o.Estado != "Inactivo"
                                       select o).ToListAsync();
            var ordenExamenDTOs = ordenExamenes.Select(o => o.toOrdenExamenDTO()).ToList();
            return Ok(new { mensaje = $"Se encontraron {ordenExamenes.Count} órdenes de examen activas.", data = ordenExamenDTOs });
        }





        //para los doctores 

        [HttpGet("Mostrar-Datos-A-Doctores/{code}")]
        public async Task<IActionResult> GetOrdenExamenPorMedico(string code)
        {
            var datos = await _context.OrdenExamen
                .Include(o => o.OrdenLaboratorio)
                .Include(o => o.Examen)
                .Include(o => o.Muestra)
                .Where(o => o.OrdenLaboratorio.OrdenLaboratorioCodigo == code && o.Estado != "Inactivo")
                .ToListAsync();

            var ordenLaboratorio = await (from o  in _context.OrdenLaboratorio
                                          where o.OrdenLaboratorioCodigo == code
                                          select o).FirstOrDefaultAsync();

            if (datos == null || !datos.Any())
            {
                return NotFound(new { mensaje = $"No se encontraron órdenes para el código: {code}" });
            }

            var ordenExamenDTOs = datos.Select(o => o.MapearDoctores()).ToList();

            return Ok(new
            {
                mensaje = $"Se encontraron {datos.Count} exámenes para la fecha {ordenLaboratorio.FechaOrden} y con el tipo de atención {ordenLaboratorio.TipoAtencion}.",
                data = ordenExamenDTOs
            });
        }





        // Ver examenes pendientes
        [HttpGet("Pendientes")]
        public async Task<IActionResult> GetOrdenExamenPendientes()
        {
            var ordenExamenes = await(from o in _context.OrdenExamen
                                      join e in _context.Examen on o.ExamenId equals e.ExamenId
                                      join l in _context.OrdenLaboratorio on o.OrdenId equals l.OrdenLaboratorioId
                                      where o.Estado == "Pendiente"
                                      select new 
                                      {
                                          l.OrdenLaboratorioCodigo,
                                          ExamenNombre = e.Nombre,
                                          o.AreaLaboratorio,
                                          e.TiempoProcesamiento,
                                          o.Estado,
                                       }
                                      ).ToListAsync();
           
            
            return Ok(new { mensaje = $"Se encontraron {ordenExamenes.Count} órdenes de examen pendientes.",ordenExamenes});
        }



        //Exámenes pendientes por área
        [HttpGet("pendientes-por-area")]
        public async Task<IActionResult> GetPendientesPorArea()
        {
            var data = await (
                from oe in _context.OrdenExamen
                where oe.Estado == "Pendiente"
                group oe by oe.AreaLaboratorio into g
                select new
                {
                    Area = g.Key,
                    Pendientes = g.Count()
                }
            ).ToListAsync();

            return Ok(data);
        }



        //Tiempo total por área
        [HttpGet("tiempo-por-area")]
        public async Task<IActionResult> GetTiempoPorArea()
        {
            var data = await (
                from oe in _context.OrdenExamen
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                group e by oe.AreaLaboratorio into g
                select new
                {
                    Area = g.Key,
                    TiempoTotal = g.Sum(x => x.TiempoProcesamiento)
                }
            ).ToListAsync();

            return Ok(data);
        }


        //Órdenes con muestra
        [HttpGet("ordenes-con-muestra")]
        public async Task<IActionResult> GetOrdenesConMuestra()
        {
            var data = await (
                from oe in _context.OrdenExamen
                join m in _context.Muestra on oe.MuestraId equals m.MuestraId
                select new
                {
                    oe.OrdenId,
                    m.MuestraCodigo,
                    m.TipoMuestra
                }
            ).ToListAsync();

            return Ok(data);
        }


        //Exámenes sin muestra
        [HttpGet("examenes-sin-muestra")]
        public async Task<IActionResult> GetSinMuestra()
        {
            var data = await (
                from oe in _context.OrdenExamen
                where oe.MuestraId == null
                select new
                {
                    oe.OrdenId,
                    oe.ExamenId,
                    oe.AreaLaboratorio
                }
            ).ToListAsync();

            return Ok(data);
        }


        // PUT: api/OrdenExamen/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdenExamen(int id, OrdenExamen ordenExamen)
        {
            if (id != ordenExamen.OrdenExamenId)
            {
                return BadRequest();
            }

            _context.Entry(ordenExamen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdenExamenExists(id))
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

        // POST: api/OrdenExamen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostOrdenExamen(string OrdenCodigo,string ExamenCodigo, string MuestraCodigo, string AreaLaboratorio)
        {
            var ordenLaboratorio = await (from o in _context.OrdenLaboratorio
                                          where o.OrdenLaboratorioCodigo == OrdenCodigo
                                          select o).FirstOrDefaultAsync();
            
            var examen = await (from e in _context.Examen
                             where e.ExamenCodigo == ExamenCodigo
                             select e).FirstOrDefaultAsync();
            
            var muestra = await (from m in _context.Muestra
                                 where m.MuestraCodigo == MuestraCodigo
                                 select m).FirstOrDefaultAsync();

            if (ordenLaboratorio == null || examen == null || muestra == null)
                return BadRequest("No existe");

            var existe = await (from oe in _context.OrdenExamen
                          where oe.OrdenId == ordenLaboratorio.OrdenLaboratorioId &&
                                oe.ExamenId == examen.ExamenId &&
                                oe.MuestraId == muestra.MuestraId
                          select oe).AnyAsync();

             if(existe)
               return BadRequest("Ya existe la orden de examen");
                
             var nuevaOrden = new OrdenExamen
             {
                 OrdenId = ordenLaboratorio.OrdenLaboratorioId,
                 ExamenId = examen.ExamenId,
                 MuestraId = muestra.MuestraId,
                 AreaLaboratorio = AreaLaboratorio
             };
            _context.OrdenExamen.Add(nuevaOrden);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Orden de examen creada exitosamente." });

        }

        // DELETE: api/OrdenExamen/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdenExamen(int id)
        {
            var ordenExamen = await _context.OrdenExamen.FindAsync(id);
            if (ordenExamen == null)
            {
                return NotFound();
            }

            _context.OrdenExamen.Remove(ordenExamen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdenExamenExists(int id)
        {
            return _context.OrdenExamen.Any(e => e.OrdenExamenId == id);
        }
    }
}
