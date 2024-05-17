using ConsultasTSC.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultasTSC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CervezasController : ControllerBase
    {
        private readonly CervezaContext _context;

        public CervezasController(CervezaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCervezas()
        {
            var cervezas = _context.Cervezas.ToList(); // Obtiene todas las cervezas de la base de datos
            return Ok(cervezas); // Devuelve un resultado HTTP 200 OK con la lista de cervezas
        }
    }
}