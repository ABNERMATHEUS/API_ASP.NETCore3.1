using API.NETCore3._1.Data;
using API.NETCore3._1.Models;
using API.NETCore3._1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.NETCore3._1.Controllers
{   
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous] //diz que qualquer um pode acessar este metodo e e se não colocar nada fica padrão assim também 
        public async Task<ActionResult<User>> Post([FromServices] DataContext dataContext,[FromBody]User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                model.Role = "employee";
                dataContext.User.Add(model);
                await dataContext.SaveChangesAsync();
                model.Password = "";

                return Ok(model);
            }
            catch (Exception)
            {

                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }

            
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices]DataContext context,
                                                              [FromBody]User model)
        {
            var user = await context.User.AsNoTracking().Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválido" });
            }
            var token = TokenServices
                .GenerateToken(user);
            return new
            {
                user=user.Username,
                token = token
            };
        }

        [HttpGet]
        //[Authorize]//Somente quem for autenticado
        [Authorize(Roles ="manager")]//somente quem for autenticado e com perfil de autorização manager
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext dataContext)
        {
            return await dataContext.User.AsNoTracking().ToListAsync();
        }

        [HttpGet]
        [Route("start")]
        public async Task<ActionResult<dynamic>>GetStart([FromServices]DataContext dataContext)
        {

            try
            {
                var employee = new User { Id = 1, Username = "abner_math", Password = "123", Role = "employee" };
                var manager = new User { Id = 2, Username = "dev", Password = "321", Role = "321" };
                var category = new Category { id = 1, Title = "Informática" };
                var product = new Product { Id = 1, Category = category, Title = "Mouse", Price = 299, Description = "Mouse Gamer" };
                
                dataContext.User.Add(employee);
                dataContext.User.Add(manager);
                dataContext.Category.Add(category);
                dataContext.Products.Add(product);

                await dataContext.SaveChangesAsync();
                return Ok(new { message = "Dados configurados com sucesso" });
            }
            catch (Exception)
            {

                return NotFound(new { message = "Dados já configurados" });
            }
           
        }

        
    }
}
