using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bus.Models;
using Bus.Enums;
using WebApi.Authorization;
using System;

namespace Bus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly BusContext _context;

        public ImagesController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Images
        [Authorize(Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/Images/5
        [Authorize(Roles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image image)
        {
            _context.Entry(image).State = EntityState.Modified;

            if (string.IsNullOrEmpty(image.Name))
            {
                _context.Entry(image).Property(x => x.Name).IsModified = false;
            }
            if (string.IsNullOrEmpty(image.Directory))
            {
                _context.Entry(image).Property(x => x.Directory).IsModified = false;
            }
            if (image.Is360 == null)
            {
                _context.Entry(image).Property(x => x.Is360).IsModified = false;
            }

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            image.ModificationDate = DateTime.UtcNow;
            image.Modifier = accessorId;
            _context.Entry(image).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(image).Property(x => x.Creator).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage(Image image)
        {
            _context.Images.Add(image);

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            image.CreationDate = DateTime.UtcNow;
            image.Creator = accessorId;
            image.ModificationDate = null;
            image.Modifier = null;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImage", new { id = image.ImageId }, image);
        }

        // DELETE: api/Images/5
        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
