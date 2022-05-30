using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bus.Models;
using System;
using WebApi.Authorization;
using GuerrillaNtp;
using System.Net;

namespace Bus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly BusContext _context;

        public PingController(BusContext context)
        {
            _context = context;
        }

        // GET: api/Ping
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Ping()
        {
            try
            {
                DateTime apiEntryTime = GetNtpTime();

                _context.Configurations.Select(u => u.AppVersion).SingleOrDefault();

                DateTime queryRetrievalTime = GetNtpTime();

                return Ok(new { apiEntryTime, queryRetrievalTime });
            }
            catch
            {
                DateTime apiEntryTime = GetNtpTime();
                DateTime queryRetrievalTime = GetNtpTime();

                return NotFound(new { apiEntryTime, queryRetrievalTime });
            }
        }

        private static DateTime GetNtpTime()
        {
            TimeSpan offset;
            using (var ntp = new NtpClient(Dns.GetHostAddresses("time.google.com")[0]))
                offset = ntp.GetCorrectionOffset();

            return DateTime.UtcNow + offset;
        }
    }
}
