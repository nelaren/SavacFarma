using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavacFarma_Shared;

namespace SavacFarma.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicationsController : ControllerBase
    {
        private readonly PruebaModel _context;

        public UbicationsController(PruebaModel context)
        {
            _context = context;
        }

        // GET: api/Ubications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubication>>> GetUbications()
        {
            return await _context.Ubications.ToListAsync();
        }

        // GET: api/Ubications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubication>> GetUbication(int id)
        {
            var ubication = await _context.Ubications.FindAsync(id);

            if (ubication == null)
            {
                return NotFound();
            }

            return ubication;
        }

        // GET: api/Ubications/?idGroup=1
        [HttpGet("{idGroup}")]
        public async Task<ActionResult<IEnumerable<Ubication>>> GetUbications(int idGroup)
        {
            var result = await _context.Ubications.Where(k=>k.IdUbicationGroup == idGroup).ToListAsync();

            return result;
        }

        // PUT: api/Ubications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbication(int id, Ubication ubication)
        {
            if (id != ubication.Id)
            {
                return BadRequest();
            }

            _context.Entry(ubication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicationExists(id))
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

        // POST: api/Ubications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Ubication>> PostUbication(Ubication ubication)
        {
            _context.Ubications.Add(ubication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUbication", new { id = ubication.Id }, ubication);
        }

        // DELETE: api/Ubications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ubication>> DeleteUbication(int id)
        {
            var ubication = await _context.Ubications.FindAsync(id);
            if (ubication == null)
            {
                return NotFound();
            }

            _context.Ubications.Remove(ubication);
            await _context.SaveChangesAsync();

            return ubication;
        }

        private bool UbicationExists(int id)
        {
            return _context.Ubications.Any(e => e.Id == id);
        }
    }
}
