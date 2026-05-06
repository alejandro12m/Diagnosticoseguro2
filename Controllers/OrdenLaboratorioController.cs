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
    public class OrdenLaboratorioController : ControllerBase
    {
        private readonly DiagnosticoMedicoContext _context;

        public OrdenLaboratorioController(DiagnosticoMedicoContext context)
        {
            _context = context;

        }

        // GET: api/OrdenLaboratorio
        [HttpGet]
        public async Task<IActionResult> GetOrdenLaboratorio()
        {
            var  ordenLaboratorio = await(from o in _context.OrdenLaboratorio
                                  where o.Estado != "Inactivo"
                                  select o).ToListAsync();
            var ordenLaboratorioDTO = ordenLaboratorio.Select(i => i.toOrdenLaboratorioDTO()).ToList(); 
            return Ok(new {mensaje = "Las ordenes que se encontraron son ",ordenLaboratorio = ordenLaboratorioDTO});
        }

        // GET: api/OrdenLaboratorio/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetOrdenLaboratorio(string code)
        {
            var ordenLaboratorio = await (from o in _context.OrdenLaboratorio
                                         where o.OrdenLaboratorioCodigo == code
                                         select o).FirstOrDefaultAsync();

            if (ordenLaboratorio == null)
            {
                return NotFound(new { mensaje = $"No se encontró la orden con el código {code}" });
            }
            var ordenLaboratorioDTO = ordenLaboratorio.toOrdenLaboratorioDTO();


            return Ok(ordenLaboratorio);
        }


        //1. Listado GENERAL CON JOIN  entre al menos 2 tablas 
        [HttpGet("detalle-ordenes")]
        public async Task<IActionResult> GetDetalleOrdenes()
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                select new
                {
                    OrdenCodigo = o.OrdenLaboratorioCodigo,
                    Fecha = o.FechaOrden,
                    TipoAtencion = o.TipoAtencion,
                    EstadoOrden = o.Estado,

                    Examen = e.Nombre,
                    TiempoProcesamiento = e.TiempoProcesamiento,
    
                    Area = oe.AreaLaboratorio,
                    EstadoExamen = oe.Estado
                }
            ).ToListAsync();

            return Ok(data);
        }

        //3. Agrupación con suma (GROUP BY + SUM)

        [HttpGet("horas-totales-por-orden")]
        public async Task<IActionResult> GetHorasTotalesPorOrden()
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                group e by o.OrdenLaboratorioCodigo into g
                select new
                {
                    OrdenCodigo = g.Key,
                    HorasTotales = g.Sum(x => x.TiempoProcesamiento)
                }
            ).ToListAsync();
            return Ok(data);
        }



        //4. Búsqueda filtrada por código o identificador


        [HttpGet("buscar-detalle-orden/{codigo}")]
        public async Task<IActionResult> BuscarDetalleOrden(string codigo)
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                where o.OrdenLaboratorioCodigo == codigo
                select new
                {
                    o.OrdenLaboratorioCodigo,
                    o.FechaOrden,
                    o.TipoAtencion,
                    EstadoOrden = o.Estado,

                    Examen = e.Nombre,
                    e.TiempoProcesamiento,

                    oe.AreaLaboratorio,
                    EstadoExamen = oe.Estado
                }
            ).ToListAsync();

            return Ok(data);
        }

        //Órdenes con más exámenes

        [HttpGet("ordenes-mas-examenes")]
        public async Task<IActionResult> GetOrdenesMasExamenes()
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                group oe by o.OrdenLaboratorioCodigo into g
                select new
                {
                    Orden = g.Key,
                    TotalExamenes = g.Count()
                }
            ).OrderByDescending(x => x.TotalExamenes).ToListAsync();

            return Ok(data);
        }




        //Muestras por estado de orden


        [HttpGet("muestras-por-estado")]
        public async Task<IActionResult> GetMuestrasPorEstado()
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                join m in _context.Muestra on oe.MuestraId equals m.MuestraId
                group m by o.Estado into g
                select new
                {
                    EstadoOrden = g.Key,
                    TotalMuestras = g.Count()
                }
            ).ToListAsync();

            return Ok(data);
        }


        //Órdenes por tipo de atención

        [HttpGet("ordenes-por-atencion")]
        public async Task<IActionResult> GetOrdenesPorAtencion()
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                group o by o.TipoAtencion into g
                select new
                {
                    TipoAtencion = g.Key,
                    Total = g.Count()
                }
            ).ToListAsync();

            return Ok(data);
        }


        //Exámenes por fecha
        [HttpGet("examenes-por-fecha")]
        public async Task<IActionResult> GetPorFecha(DateOnly fecha)
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                where o.FechaOrden == fecha
                select new
                {
                    o.OrdenLaboratorioCodigo,
                    oe.AreaLaboratorio
                }
            ).ToListAsync();

            return Ok(data);
        }


        [HttpGet("detalle-ordenes/{TipoAtencion}")]
        public async Task<IActionResult> GetDetalleOrdenesByTipoAtencion(string TipoAtencion)
        {
            var data = await (
                from o in _context.OrdenLaboratorio
                join oe in _context.OrdenExamen on o.OrdenLaboratorioId equals oe.OrdenId
                join e in _context.Examen on oe.ExamenId equals e.ExamenId
                where o.TipoAtencion == TipoAtencion
                select new
                {
                    OrdenCodigo = o.OrdenLaboratorioCodigo,
                    Fecha = o.FechaOrden,
                    TipoAtencion = o.TipoAtencion,
                    EstadoOrden = o.Estado,
                    Examen = e.Nombre,
                    TiempoProcesamiento = e.TiempoProcesamiento,
                    Area = oe.AreaLaboratorio,
                    EstadoExamen = oe.Estado
                }
            ).ToListAsync();
            return Ok(data);
        }


        // PUT: api/OrdenLaboratorios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{code}")]
        public async Task<IActionResult> PutOrdenLaboratorio(string code, string PacienteCodigo, string MedicoCodigo, DateOnly FechaOrden, string TipoAtencion, string Observaciones)
        {
            OrdenLaboratorio ordenLaboratorio = await (from o in _context.OrdenLaboratorio
                                         where o.OrdenLaboratorioCodigo == code && o.Estado != "Inactivo"
                                                       select o).FirstOrDefaultAsync();
            if(ordenLaboratorio == null)
            {
                return NotFound();
            }
            ordenLaboratorio.PacienteCodigo = PacienteCodigo;
            ordenLaboratorio.MedicoCodigo = MedicoCodigo;
            ordenLaboratorio.FechaOrden = FechaOrden;
            ordenLaboratorio.TipoAtencion = TipoAtencion;
            ordenLaboratorio.Observaciones = Observaciones;

            await _context.SaveChangesAsync();

            return Ok(ordenLaboratorio);
        }

        // POST: api/OrdenLaboratorios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("HacerOrdenLaboratorio")]
        public async Task<IActionResult> PostOrdenLaboratorio(string code, string PacienteCodigo , string MedicoCodigo, DateOnly FechaOrden, string TipoAtencion, string Observaciones)
        {
            var comprobante = await(from o in _context.OrdenLaboratorio
                                         where o.OrdenLaboratorioCodigo == code
                                         select o).FirstOrDefaultAsync();
            if (comprobante != null) 
            { 
                return BadRequest("Ya existe una orden de laboratorio con el código proporcionado.");    
            }

            OrdenLaboratorio ordenLaboratorio = new OrdenLaboratorio
            {
                OrdenLaboratorioCodigo = code,
                PacienteCodigo = PacienteCodigo,
                MedicoCodigo = MedicoCodigo,
                FechaOrden = FechaOrden,
                TipoAtencion = TipoAtencion,
                Observaciones = Observaciones,
            };

            _context.OrdenLaboratorio.Add(ordenLaboratorio);
            await _context.SaveChangesAsync();

            return Ok(new {mensaje = $"Se creo correctamente la orden de laboratorio con el codigo ->{ordenLaboratorio.OrdenLaboratorioCodigo}"});
        }

        // DELETE: api/OrdenLaboratorios/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteOrdenLaboratorio(string code)
        {
            var ordenLaboratorio = await (from o in _context.OrdenLaboratorio
                                         where o.OrdenLaboratorioCodigo == code
                                         select o).FirstOrDefaultAsync();
            if (ordenLaboratorio == null)
            {
                return NotFound();
            }

            ordenLaboratorio.Estado = "Inactivo";
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Orden de laboratorio con código {code} se ha eliminado." });
        }

        private bool OrdenLaboratorioExists(int id)
        {
            return _context.OrdenLaboratorio.Any(e => e.OrdenLaboratorioId == id);
        }
    }
}
