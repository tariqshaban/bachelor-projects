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
    public class RoutesController : ControllerBase
    {
        private readonly BusContext _context;

        public RoutesController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Routes
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Route>>> GetRoutes()
        {
            return await _context.Routes.ToListAsync();
        }

        // GET: api/Routes/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Route>> GetRoute(int id)
        {
            var route = _context.Routes
                .Include(x => x.Paths).ThenInclude(y => y.Stops.OrderBy(y => y.Sequence)).ThenInclude(y => y.Image)
                .Include(x => x.Paths).ThenInclude(y => y.Image)
                .FirstOrDefault(x => x.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            _context.Entry(route).State = EntityState.Modified;
            route.Views++;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return route;
        }

        // PUT: api/Routes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoute(int id, Route route)
        {
            _context.Entry(route).State = EntityState.Modified;

            if (string.IsNullOrEmpty(route.Name))
            {
                _context.Entry(route).Property(x => x.Name).IsModified = false;
            }
            if (route.Views == null)
            {
                _context.Entry(route).Property(x => x.Views).IsModified = false;
            }
            _context.Entry(route).Property(x => x.Views).IsModified = false;

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            route.ModificationDate = DateTime.UtcNow;
            route.Modifier = accessorId;
            _context.Entry(route).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(route).Property(x => x.Creator).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteExists(id))
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

        // POST: api/Routes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Route>> PostRoute(Route route)
        {
            _context.Routes.Add(route);

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            route.CreationDate = DateTime.UtcNow;
            route.Creator = accessorId;
            route.ModificationDate = null;
            route.Modifier = null;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoute", new { id = route.Id }, route);
        }

        // DELETE: api/Routes/5
        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Routes/utils/5
        [Authorize(Roles.Admin, Roles.Driver)]
        [HttpGet("utils/{id}")]
        public async Task<ActionResult<Route>> GetRouteByDriverId(int id)
        {
            var driver = await _context.Drivers.FirstAsync(x => x.PersonId == id);

            var route = _context.Routes
                .Include(x => x.Paths).ThenInclude(y => y.Stops.OrderBy(y => y.Sequence)).ThenInclude(y => y.Image)
                .Include(x => x.Paths).ThenInclude(y => y.Image)
                .First(x => x.Id == driver.RouteId);

            if (route == null)
            {
                return NotFound();
            }

            return route;
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.Id == id);
        }
    }
}
