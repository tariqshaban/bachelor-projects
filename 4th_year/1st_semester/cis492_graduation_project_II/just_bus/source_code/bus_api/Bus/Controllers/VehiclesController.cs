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
    public class VehiclesController : ControllerBase
    {
        private readonly BusContext _context;

        public VehiclesController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [Authorize(Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // GET: api/Vehicles/5
        [Authorize(Roles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Modified;

            if (vehicle.PlateNumber == null)
            {
                _context.Entry(vehicle).Property(x => x.PlateNumber).IsModified = false;
            }
            if (vehicle.Manufacturer == null)
            {
                _context.Entry(vehicle).Property(x => x.Manufacturer).IsModified = false;
            }
            if (vehicle.ManufacturerAr == null)
            {
                _context.Entry(vehicle).Property(x => x.ManufacturerAr).IsModified = false;
            }
            if (vehicle.Model == null)
            {
                _context.Entry(vehicle).Property(x => x.Model).IsModified = false;
            }
            if (vehicle.ModelAr == null)
            {
                _context.Entry(vehicle).Property(x => x.ModelAr).IsModified = false;
            }
            if (vehicle.Color == null)
            {
                _context.Entry(vehicle).Property(x => x.Color).IsModified = false;
            }
            if (vehicle.SecondaryColor == null)
            {
                _context.Entry(vehicle).Property(x => x.SecondaryColor).IsModified = false;
            }
            if (vehicle.Capacity == null)
            {
                _context.Entry(vehicle).Property(x => x.Capacity).IsModified = false;
            }
            if (vehicle.Image == null)
            {
                _context.Entry(vehicle).Property(x => x.Image).IsModified = false;
            }

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            vehicle.ModificationDate = DateTime.UtcNow;
            vehicle.Modifier = accessorId;
            _context.Entry(vehicle).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(vehicle).Property(x => x.Creator).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            vehicle.CreationDate = DateTime.UtcNow;
            vehicle.Creator = accessorId;
            vehicle.ModificationDate = null;
            vehicle.Modifier = null;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
