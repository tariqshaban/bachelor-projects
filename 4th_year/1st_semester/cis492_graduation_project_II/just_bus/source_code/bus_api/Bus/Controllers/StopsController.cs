using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bus.Models;
using WebApi.Authorization;
using Bus.Enums;
using System;

namespace Bus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StopsController : ControllerBase
    {
        private readonly BusContext _context;

        public StopsController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Stops
        [HttpGet]
        [Authorize(Roles.Admin)]
        public async Task<ActionResult<IEnumerable<Stop>>> GetStops()
        {
            return await _context.Stops.ToListAsync();
        }

        // GET: api/Stops/5
        [HttpGet("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<ActionResult<Stop>> GetStop(int id, byte type, byte sequence)
        {
            var stop = await _context.Stops.FindAsync(id, type, sequence);

            if (stop == null)
            {
                return NotFound();
            }

            return stop;
        }

        // PUT: api/Stops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> PutStop(int id, byte type, byte sequence, Stop stop)
        {
            _context.Entry(stop).State = EntityState.Modified;

            if (string.IsNullOrEmpty(stop.Name))
            {
                _context.Entry(stop).Property(x => x.Name).IsModified = false;
            }
            if (string.IsNullOrEmpty(stop.NameAr))
            {
                _context.Entry(stop).Property(x => x.NameAr).IsModified = false;
            }
            if (stop.Latitude == null)
            {
                _context.Entry(stop).Property(x => x.Latitude).IsModified = false;
            }
            if (stop.Longitude == null)
            {
                _context.Entry(stop).Property(x => x.Longitude).IsModified = false;
            }
            if (stop.Image == null)
            {
                _context.Entry(stop).Property(x => x.Image).IsModified = false;
            }
            _context.Entry(stop).Property(x => x.Path).IsModified = false;

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            stop.ModificationDate = DateTime.UtcNow;
            stop.Modifier = accessorId;
            _context.Entry(stop).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(stop).Property(x => x.Creator).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StopExists(id, type, sequence))
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

        // POST: api/Stops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles.Admin)]
        public async Task<ActionResult<Stop>> PostStop(Stop stop)
        {
            _context.Stops.Add(stop);

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            stop.CreationDate = DateTime.UtcNow;
            stop.Creator = accessorId;
            stop.ModificationDate = null;
            stop.Modifier = null;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStop", new { id = stop.RouteId }, stop);
        }

        // DELETE: api/Stops/5
        [HttpDelete("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> DeleteStop(int id, byte type, byte sequence)
        {
            var stop = await _context.Stops.FindAsync(id, type, sequence);
            if (stop == null)
            {
                return NotFound();
            }

            _context.Stops.Remove(stop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StopExists(int id, byte type, byte sequence)
        {
            return _context.Stops.Any(e => e.RouteId == id && e.PathType == type && e.Sequence == sequence);
        }
    }
}
