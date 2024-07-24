using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using oracleDataAcess.Models;
using ultima_prueba.Models;


namespace ultima_prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private ModelContext _context;
        public VentaController(ModelContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<List<Ventum>> Listar()
        {
            return await _context.Venta.ToListAsync();
        }


        [HttpGet("BuscarVtaPorId")]
        public async Task<ActionResult<Ventum>> SearchByIdAsync(decimal Idventa)
        {
            var retorno = await _context.Venta.FirstOrDefaultAsync(x => x.Idventa == Idventa);

            if (retorno != null)
            {
                return retorno;
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<ActionResult<Ventum>> SaveAsync(VtaCommand c)
        {
            Ventum venta = new Ventum();    
            venta.Idclient = c.Idclient;
            venta.Fchaope = c.Fchaope;
            venta.Montototal = 0;
            venta.Estado=(char)EstadoEnum.PENDIENTE;


            try
            {
                await _context.Venta.AddAsync(venta);
                await _context.SaveChangesAsync();
                venta.Idventa = await _context.Venta.MaxAsync(u => u.Idventa);

                return venta;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Ventum>> UpdateAsync(Ventum c)
        {
            if (c == null || c.Idventa == 0)
                return BadRequest("Faltan datos");

            Ventum cat = await _context.Venta.FirstOrDefaultAsync(x => x.Idventa == c.Idventa);

            if (cat == null)
                return NotFound();

            try
            {
                cat.Montototal = c.Montototal;

                _context.Venta.Update(cat);
                await _context.SaveChangesAsync();

                return cat;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }
        [HttpDelete("{Idventa}")]
        public async Task<ActionResult<bool>> DeleteAsync(decimal Idventa)
        {
            Ventum cat = await _context.Venta.FirstOrDefaultAsync(x => x.Idventa == Idventa);

            if (cat == null)
                return NotFound();

            try
            {
                _context.Venta.Remove(cat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }
        [HttpPatch]
        public async Task<IActionResult> ConfirmTransaction(decimal Idventa)
        {
            Ventum cat = await _context.Venta.FirstOrDefaultAsync(x => x.Idventa == Idventa);

            var listDllVenta = await _context.Dlleventa.Where(x => x.Idvta == Idventa).ToListAsync();

            var montoTotal = 0;

            foreach (var item in listDllVenta)
            {
                // Calcular el subtotal para el elemento actual
                int subtotal = Convert.ToInt32(item.Subtotal);  // Ejemplo: Precio y Cantidad son propiedades de Dlleventa

                // Sumar el subtotal al total acumulado
                montoTotal += subtotal;
            }
           
            if (cat != null)
            {
                cat.Montototal = montoTotal;
                cat.Estado = (char)EstadoEnum.FINALIZADO;
                _context.Venta.Update(cat);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("CancelarTransaccion")]
        public async Task<IActionResult> CancelTransaction(decimal Idventa)
        {
            Ventum cat = await _context.Venta.FirstOrDefaultAsync(x => x.Idventa == Idventa);

            if (cat != null)
            {
                var listDllVenta = await _context.Dlleventa.Where(x => x.Idvta == Idventa).ToListAsync();

             

                foreach (var item in listDllVenta)
                {
                    _context.Dlleventa.Remove(item);
                   
                }

                cat.Estado =(char) EstadoEnum.CANCELADO;
                _context.Venta.Update(cat);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
