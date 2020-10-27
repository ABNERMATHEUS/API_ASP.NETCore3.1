using API.NETCore3._1.Data;
using API.NETCore3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.NETCore3._1.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase

    {

        [HttpGet]
        [Authorize]
        [ResponseCache(VaryByHeader = "User-Agent",Location =ResponseCacheLocation.Any, Duration =30)]
        public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext dataContext)
        {
            var categories = await dataContext.Category.AsNoTracking().ToArrayAsync();
            return Ok(categories);

        }

        [Route("{id:int}")] //Permitindo apenas parametros do tipo int (isso é chamado de restrição de rota) 
        [HttpGet]
        public async Task<ActionResult<Category>> GetById(int id,[FromServices]DataContext dataContext)
        {
            var category = await dataContext.Category.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            if(category == null)
            {
                return  BadRequest(new { message = "Categoria não encontrada" });
            }
            else
            {
                return Ok(category);
            }
            
        }


        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] Category category, [FromServices] DataContext dataContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                dataContext.Category.Add(category);
                await dataContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }


            return Ok(category);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<ActionResult<Category>> Put(int id, [FromBody] Category category, [FromServices] DataContext dataContext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (id != category.id)
            {
                return NotFound(new { message = "Categoria não encontrada" });
            }

            try
            {
                dataContext.Entry<Category>(category).State = EntityState.Modified;
                await dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }


            return Ok(category);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<ActionResult<string>> Delete(int id, [FromServices] DataContext datacontext)
        {
            var category = await datacontext.Category.FirstOrDefaultAsync(x => x.id == id);
            if(category == null)
            {
                return NotFound(new { message = "Categoria não encontrada" });
            }

            try
            {
                datacontext.Category.Remove(category);
                await datacontext.SaveChangesAsync();
                return Ok(new { message="Categoria removida com sucesso"});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria" });
            }
        }




    }
}
