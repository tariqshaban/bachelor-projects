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
    public class DriversController : ControllerBase
    {
        private readonly BusContext _context;

        public DriversController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [Authorize(Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        // GET: api/Drivers/5
        [Authorize(Roles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        // PUT: api/Drivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            _context.Entry(driver).State = EntityState.Modified;

            if (driver.RouteId == null)
            {
                _context.Entry(driver).Property(x => x.RouteId).IsModified = false;
            }
            if (driver.LastLatitude == null)
            {
                _context.Entry(driver).Property(x => x.LastLatitude).IsModified = false;
            }
            if (driver.LastLongitude == null)
            {
                _context.Entry(driver).Property(x => x.LastLongitude).IsModified = false;
            }
            if (driver.LastLocationUpdate == null)
            {
                _context.Entry(driver).Property(x => x.LastLocationUpdate).IsModified = false;
            }
            if (driver.Image == null)
            {
                _context.Entry(driver).Property(x => x.Image).IsModified = false;
            }

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            driver.ModificationDate = DateTime.UtcNow;
            driver.Modifier = accessorId;
            _context.Entry(driver).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(driver).Property(x => x.Creator).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        // POST: api/Drivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            var person = await _context.People.FindAsync(driver.PersonId);

            if (person == null || person.Role != (int)Roles.Driver)
            {
                return BadRequest();
            }

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            driver.CreationDate = DateTime.UtcNow;
            driver.Creator = accessorId;
            driver.ModificationDate = null;
            driver.Modifier = null;

            _context.Drivers.Add(driver);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DriverExists(driver.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDriver", new { id = driver.PersonId }, driver);
        }

        // DELETE: api/Drivers/5
        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.PersonId == id);
        }

        // GET: api/Drivers/utils/5
        [AllowAnonymous]
        [HttpGet]
        [Route("utils/{routeId}")]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDriversInRoute(int routeId)
        {
            DateTime fetchDiscontinuityThreshold = DateTime.UtcNow.AddSeconds(-20);
            return await _context.Drivers
                .Where(e => e.RouteId == routeId && e.LastLatitude != null && e.LastLongitude != null && e.LastLocationUpdate != null /*&& fetchDiscontinuityThreshold <= e.LastLocationUpdate.Value*/)
                .Include(y => y.Image)
                .Include(x => x.Vehicle)
                .ThenInclude(y => y.Image)
                .Include(x => x.Person)
                .Select(x => x.ShowOnlyPersonName())
                .ToListAsync();
        }

        // GET: api/Drivers/utils/5
        [Authorize(Roles.Admin, Roles.Driver)]
        [HttpPut]
        [Route("utils/{id}")]
        public async Task<IActionResult> UpdateDriverCoordinates(int id, decimal latitude, decimal longitude, decimal? estimatedSpeed, PathType? pathType)
        {
            Driver driver = _context.Drivers.Single(x => x.PersonId == id);

            _context.Entry(driver).State = EntityState.Modified;

            driver.LastLatitude = latitude;
            driver.LastLongitude = longitude;
            driver.LastLocationUpdate = DateTime.UtcNow;

            if (estimatedSpeed != null && estimatedSpeed > 0)
            {
                byte speedContributionFactor = (byte)_context.Configurations.ToList().First().SpeedContributionFactor;

                if (speedContributionFactor != 0 && estimatedSpeed > 0)
                {
                    Route route = _context.Routes.Single(x => x.Id == driver.RouteId);

                    Path path = _context.Paths.Single(x => x.RouteId == route.Id && x.Type == (int)pathType);

                    _context.Entry(path).State = EntityState.Modified;

                    if (speedContributionFactor == 100)
                    {
                        path.AverageSpeed = estimatedSpeed;
                    }
                    else
                    {
                        path.AverageSpeed = (path.AverageSpeed / speedContributionFactor) +
                            ((estimatedSpeed - path.AverageSpeed) / (100 - speedContributionFactor));
                    }
                }

            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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
    }
}
