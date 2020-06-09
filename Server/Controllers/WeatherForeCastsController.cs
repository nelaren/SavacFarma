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
    public class WeatherForeCastsController : ControllerBase
    {
        private readonly PruebaModel _context;

        public WeatherForeCastsController(PruebaModel context)
        {
            _context = context;
        }

        // GET: api/WeatherForeCasts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForeCast>>> GetWeatherForeCasts()
        {
            return await _context.WeatherForeCasts.ToListAsync();
        }

        // GET: api/WeatherForeCasts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForeCast>> GetWeatherForeCast(long id)
        {
            var weatherForeCast = await _context.WeatherForeCasts.FindAsync(id);

            if (weatherForeCast == null)
            {
                return NotFound();
            }

            return weatherForeCast;
        }

        // PUT: api/WeatherForeCasts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeatherForeCast(long id, WeatherForeCast weatherForeCast)
        {
            if (id != weatherForeCast.Id)
            {
                return BadRequest();
            }

            _context.Entry(weatherForeCast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherForeCastExists(id))
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

        // POST: api/WeatherForeCasts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WeatherForeCast>> PostWeatherForeCast(WeatherForeCast weatherForeCast)
        {
            _context.WeatherForeCasts.Add(weatherForeCast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeatherForeCast", new { id = weatherForeCast.Id }, weatherForeCast);
        }

        // DELETE: api/WeatherForeCasts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WeatherForeCast>> DeleteWeatherForeCast(long id)
        {
            var weatherForeCast = await _context.WeatherForeCasts.FindAsync(id);
            if (weatherForeCast == null)
            {
                return NotFound();
            }

            _context.WeatherForeCasts.Remove(weatherForeCast);
            await _context.SaveChangesAsync();

            return weatherForeCast;
        }

        private bool WeatherForeCastExists(long id)
        {
            return _context.WeatherForeCasts.Any(e => e.Id == id);
        }
    }
}
