using ConsultasTSC.Data;
using ConsultasTSC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ConsultasTSC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganigramasController : ControllerBase
    {
        private readonly OrganigramaContext _context;

        public OrganigramasController(OrganigramaContext context)
        {
            _context = context;
        }

        // GET: api/Organigrama
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organigrama>>> GetOrganigramas()
        {
            return await _context.Organigramas.ToListAsync();
        }

        // GET: api/Organigrama/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organigrama>> GetOrganigrama(int id)
        {
            var organigrama = await _context.Organigramas.FindAsync(id);

            if (organigrama == null)
            {
                return NotFound();
            }

            return organigrama;
        }

        // PUT: api/Organigrama/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganigrama(int id, Organigrama organigrama)
        {
            if (id != organigrama.CodigoOrganigrama)
            {
                return BadRequest();
            }

            _context.Entry(organigrama).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganigramaExists(id))
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

        // POST: api/Organigrama
        [HttpPost]
        public async Task<ActionResult<Organigrama>> PostOrganigrama(Organigrama organigrama)
        {
            _context.Organigramas.Add(organigrama);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrganigrama), new { id = organigrama.CodigoOrganigrama }, organigrama);
        }

        // DELETE: api/Organigrama/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganigrama(int id)
        {
            var organigrama = await _context.Organigramas.FindAsync(id);
            if (organigrama == null)
            {
                return NotFound();
            }

            _context.Organigramas.Remove(organigrama);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool OrganigramaExists(int id)
        {
            return _context.Organigramas.Any(e => e.CodigoOrganigrama == id);
        }
    }
}