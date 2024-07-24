using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using oracleDataAcess.Models;
using Microsoft.EntityFrameworkCore;

namespace ultima_prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clientController : ControllerBase
    {
        private ModelContext _context;
        public clientController(ModelContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<List<Cliente>> Listar()
        {
            return await _context.Clientes.ToListAsync();
        }


        [HttpGet("Buscar Cliente Por Id")]
        public async Task<ActionResult<Cliente>> SearchByIdAsync(decimal Id_Cliente)
        {
            var retorno = await _context.Clientes.FirstOrDefaultAsync(x => x.IdClient == Id_Cliente);

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
        public async Task<ActionResult<Cliente>> SaveAsync(Cliente c)
        {
            try
            {
                await _context.Clientes.AddAsync(c);
                await _context.SaveChangesAsync();
                c.IdClient = await _context.Clientes.MaxAsync(u => u.IdClient);

                return c;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Cliente>> UpdateAsync(Cliente c)
        {
            if (c == null || c.IdClient == 0)
                return BadRequest("Faltan datos");

            Cliente cat = await _context.Clientes.FirstOrDefaultAsync(x => x.IdClient == c.IdClient);

            if (cat == null)
                return NotFound();

            try
            {
                cat.Nombre = c.Nombre;
                _context.Clientes.Update(cat);
                await _context.SaveChangesAsync();

                return cat;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(decimal IdClient)
        {
            Cliente cat = await _context.Clientes.FirstOrDefaultAsync(x => x.IdClient ==IdClient );

            if (cat == null)
                return NotFound();

            try
            {
                _context.Clientes.Remove(cat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }


    }
}