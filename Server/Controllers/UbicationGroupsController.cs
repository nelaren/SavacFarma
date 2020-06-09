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
    public class UbicationGroupsController : ControllerBase
    {
        private readonly PruebaModel _context;

        public UbicationGroupsController(PruebaModel context)
        {
            _context = context;
        }

        // GET: api/UbicationGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UbicationGroup>>> GetUbicationGroups()
        {
            return await _context.UbicationGroups.ToListAsync();
        }

        // GET: api/UbicationGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UbicationGroup>> GetUbicationGroup(int id)
        {
            var ubicationGroup = await _context.UbicationGroups.FindAsync(id);

            if (ubicationGroup == null)
            {
                return NotFound();
            }

            return ubicationGroup;
        }

        // PUT: api/UbicationGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicationGroup(int id, UbicationGroup ubicationGroup)
        {
            if (id != ubicationGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(ubicationGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicationGroupExists(id))
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

        // POST: api/UbicationGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UbicationGroup>> PostUbicationGroup(UbicationGroup ubicationGroup)
        {
            _context.UbicationGroups.Add(ubicationGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUbicationGroup", new { id = ubicationGroup.Id }, ubicationGroup);
        }

        // DELETE: api/UbicationGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UbicationGroup>> DeleteUbicationGroup(int id)
        {
            var ubicationGroup = await _context.UbicationGroups.FindAsync(id);
            if (ubicationGroup == null)
            {
                return NotFound();
            }

            _context.UbicationGroups.Remove(ubicationGroup);
            await _context.SaveChangesAsync();

            return ubicationGroup;
        }

        private bool UbicationGroupExists(int id)
        {
            return _context.UbicationGroups.Any(e => e.Id == id);
        }
    }
}
