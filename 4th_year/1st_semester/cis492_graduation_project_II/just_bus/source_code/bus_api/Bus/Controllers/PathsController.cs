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
    public class PathsController : ControllerBase
    {
        private readonly BusContext _context;

        public PathsController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Paths
        [Authorize(Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Path>>> GetPaths()
        {
            return await _context.Paths.ToListAsync();
        }

        // GET: api/Paths/5
        [Authorize(Roles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Path>> GetPath(int id, byte type)
        {
            var path = await _context.Paths.FindAsync(id, type);

            if (path == null)
            {
                return NotFound();
            }

            return path;
        }

        // PUT: api/Paths/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPath(int id, byte type, Path path)
        {
            _context.Entry(path).State = EntityState.Modified;

            if (string.IsNullOrEmpty(path.StartName))
            {
                _context.Entry(path).Property(x => x.StartName).IsModified = false;
            }
            if (string.IsNullOrEmpty(path.StartNameAr))
            {
                _context.Entry(path).Property(x => x.StartNameAr).IsModified = false;
            }
            if (string.IsNullOrEmpty(path.EndName))
            {
                _context.Entry(path).Property(x => x.EndName).IsModified = false;
            }
            if (string.IsNullOrEmpty(path.EndNameAr))
            {
                _context.Entry(path).Property(x => x.EndNameAr).IsModified = false;
            }
            if (string.IsNullOrEmpty(path.Path1))
            {
                _context.Entry(path).Property(x => x.Path1).IsModified = false;
            }
            if (path.IsCircular == null)
            {
                _context.Entry(path).Property(x => x.IsCircular).IsModified = false;
            }
            if (path.AverageSpeed == null)
            {
                _context.Entry(path).Property(x => x.AverageSpeed).IsModified = false;
            }
            if (path.Image == null)
            {
                _context.Entry(path).Property(x => x.Image).IsModified = false;
            }

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            path.ModificationDate = DateTime.UtcNow;
            path.Modifier = accessorId;
            _context.Entry(path).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(path).Property(x => x.Creator).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PathExists(id, type))
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

        // POST: api/Paths
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Path>> PostPath(Path path)
        {
            _context.Paths.Add(path);

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            path.CreationDate = DateTime.UtcNow;
            path.Creator = accessorId;
            path.ModificationDate = null;
            path.Modifier = null;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PathExists(path.RouteId, path.Type))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPath", new { id = path.RouteId }, path);
        }

        // DELETE: api/Paths/5
        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePath(int id, byte type)
        {
            var path = await _context.Paths.FindAsync(id, type);
            if (path == null)
            {
                return NotFound();
            }

            _context.Paths.Remove(path);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PathExists(int id, byte type)
        {
            return _context.Paths.Any(e => e.RouteId == id && e.Type == type);
        }
    }
}
