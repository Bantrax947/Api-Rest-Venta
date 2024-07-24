using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oracleDataAcess.Models;
using ultima_prueba.Models;

namespace ultima_prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DlleVtaController : ControllerBase
    {
        private ModelContext _context;
        public DlleVtaController(ModelContext context)
        {
            _context = context;

        }
  
        [HttpGet("BuscarDlleVtaPorId")]
        public async Task<ActionResult<Dlleventum>> SearchByIdAsync(decimal Iddventa)
        {
            var retorno = await _context.Dlleventa.FirstOrDefaultAsync(x => x.Iddventa == Iddventa);

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
        public async Task<ActionResult<Dlleventum>> SaveAsync(DlleVtaCommand c)
        {
            Dlleventum dllevent = new Dlleventum();

            try
            {
                dllevent.Iddventa = c.Idvta;
                dllevent.Idprod = c.Idprod;
                dllevent.Cantidad = c.Cantidad;

                var product = await _context.Productos.FirstOrDefaultAsync(x => x.Idproducto == c.Idprod);
                var subTotal = 0;
                if (product != null)
                {
                    if (product.Cantidad < c.Cantidad)
                    {
                        subTotal = Convert.ToInt32(product.Preciounit) * c.Cantidad;
                    } else
                    {
                        return BadRequest($"No hay suficiente stock disponible. Stock actual: {product.Cantidad}");
                    }

                } else
                {
                    return StatusCode(500, "El Idproducto no existe.");
                }

                await _context.Dlleventa.AddAsync(dllevent);
                await _context.SaveChangesAsync();
                dllevent.Iddventa = await _context.Dlleventa.MaxAsync(u => u.Iddventa);

                return dllevent;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }

    }
}
