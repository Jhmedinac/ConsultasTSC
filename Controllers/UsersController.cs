using ConsultasTSC.Data;
using ConsultasTSC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace ConsultasTSC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        // este metodo es sincrónico y bloqueará el hilo hasta que se complete la operación
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            
            //string constring = _context.   GetConnectionString("DefaultConnection");
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("GetUserById")]
        // este metodo es asincrónico y más adecuado para operaciones largas o intensivas que podrían bloquear el hilo de ejecución
        public async Task<ActionResult<User>> GetUserById(int Id)
        {
            
            User users = await _context.Users.Select(
                    s => new User
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Username = s.Username,
                        Password = s.Password,
                        EnrollmentDate = s.EnrollmentDate
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return users;
            }
        }
    }
}
