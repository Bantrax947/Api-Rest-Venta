﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using oracleDataAcess.Models;
using Microsoft.EntityFrameworkCore;
using ultima_prueba.Models;
using Microsoft.AspNetCore.Authorization;

namespace ultima_prueba.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class productoController : ControllerBase
    {
        private ModelContext _context;
        public productoController(ModelContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<List<Producto>> Listar()
        {
            return await _context.Productos.ToListAsync();
        }

        [HttpGet("BuscarProductoPorId")]
        public async Task<ActionResult<Producto>> SearchByIdAsync(decimal Idproducto)
        {
            var retorno = await _context.Productos.FirstOrDefaultAsync(x => x.Idproducto == Idproducto);

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
        public async Task<ActionResult<Producto>> SaveAsync(ProductoCreateCommand c)
        {
            Producto producto = new Producto();

            producto.Nombreprod = c.NombreProd;
            producto.Cantidad = c.Cantidad;
            producto.Preciounit = c.PrecioUnit;

            try
            {
                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();
                producto.Idproducto = await _context.Productos.MaxAsync(u => u.Idproducto);

                return producto;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }
        [HttpPut]
        public async Task<ActionResult<Producto>> UpdateAsync(Producto c)
        {
            if (c == null || c.Idproducto == 0)
                return BadRequest("Faltan datos");

            Producto cat = await _context.Productos.FirstOrDefaultAsync(x => x.Idproducto == c.Idproducto);

            if (cat == null)
                return NotFound();

            try
            {
                cat.Nombreprod = c.Nombreprod;
                cat.Cantidad = c.Cantidad;


                _context.Productos.Update(cat);
                await _context.SaveChangesAsync();

                return cat;
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Se encontró un error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(decimal Idproducto)
        {
            Producto cat = await _context.Productos.FirstOrDefaultAsync(x => x.Idproducto == Idproducto);

            if (cat == null)
                return NotFound();

            try
            {
                _context.Productos.Remove(cat);
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
