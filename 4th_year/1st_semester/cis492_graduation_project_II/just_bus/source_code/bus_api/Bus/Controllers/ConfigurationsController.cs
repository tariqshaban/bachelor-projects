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
    public class ConfigurationsController : ControllerBase
    {
        private readonly BusContext _context;

        public ConfigurationsController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Configurations
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetConfigurations()
        {
            return Ok(_context.Configurations.ToList());
        }

        // PUT: api/Configurations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> PutConfiguration(Configuration configuration)
        {
            Configuration source = _context.Configurations.Single(x => x.Id == true);

            _context.Entry(source).State = EntityState.Modified;

            if (!string.IsNullOrEmpty(configuration.AppVersion))
            {
                source.AppVersion = configuration.AppVersion;
            }
            if (configuration.IsPeak != null)
            {
                source.IsPeak = configuration.IsPeak;
            }
            if (configuration.Weather != null)
            {
                source.Weather = configuration.Weather;
            }
            if (configuration.SpeedContributionFactor != null)
            {
                source.SpeedContributionFactor = configuration.SpeedContributionFactor;
            }
            if (!string.IsNullOrEmpty(configuration.ImageDrawerDirectory))
            {
                source.ImageDrawerDirectory = configuration.ImageDrawerDirectory;
            }
            if (configuration.Timeout != null)
            {
                source.Timeout = configuration.Timeout;
            }
            if (configuration.DriverTimeout != null)
            {
                source.DriverTimeout = configuration.DriverTimeout;
            }
            if (configuration.DriverLocationGetterInterval != null)
            {
                source.DriverLocationGetterInterval = configuration.DriverLocationGetterInterval;
            }
            if (configuration.DriverLocationSetterInterval != null)
            {
                source.DriverLocationSetterInterval = configuration.DriverLocationSetterInterval;
            }

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            configuration.ModificationDate = DateTime.UtcNow;
            configuration.Modifier = accessorId;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
    }
}
