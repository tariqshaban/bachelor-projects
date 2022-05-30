using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bus.Models;
using Bus.Enums;
using WebApi.Models.Users;
using WebApi.Services;
using Microsoft.AspNetCore.Http;
using System;
using Bus.Helpers;
using WebApi.Authorization;

namespace Bus.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly BusContext _context;
        private readonly IPersonService _personService;

        public PeopleController(BusContext context, IPersonService personService)
        {
            _context = context;
            _personService = personService;
        }

        // GET: api/People
        [Authorize(Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            ActionResult<IEnumerable<Person>> people =  await _context.People.ToListAsync();

            foreach (var person in people.Value)
            {
                person.Password = null;
                person.Salt = null;
            }

            return people;
        }

        // GET: api/People/5
        [Authorize(Roles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            person.Password = null;
            person.Salt = null;

            return person;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            _context.Entry(person).State = EntityState.Modified;

            if (string.IsNullOrEmpty(person.Name))
            {
                _context.Entry(person).Property(x => x.Name).IsModified = false;
            }
            if (string.IsNullOrEmpty(person.NameAr))
            {
                _context.Entry(person).Property(x => x.NameAr).IsModified = false;
            }
            if (string.IsNullOrEmpty(person.Number))
            {
                _context.Entry(person).Property(x => x.Number).IsModified = false;
            }
            if (string.IsNullOrEmpty(person.Password))
            {
                _context.Entry(person).Property(x => x.Password).IsModified = false;
            }
            else
            {
                HashedPassword hashedPassword;

                hashedPassword = Argon2.HashPassword(person.Password);

                person.Password = hashedPassword.Password;
                person.Salt = hashedPassword.Salt;
            }
            if (person.Role == null)
            {
                _context.Entry(person).Property(x => x.Role).IsModified = false;
            }
            _context.Entry(person).Property(x => x.Salt).IsModified = false;

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            person.ModificationDate = DateTime.UtcNow;
            person.Modifier = accessorId;
            _context.Entry(person).Property(x => x.CreationDate).IsModified = false;
            _context.Entry(person).Property(x => x.Creator).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);

            _context.Entry(person).Property(x => x.Salt).IsModified = false;
            if (person.Role != (int)Roles.Driver)
            {
                _context.Entry(person).Reference(x => x.DriverPerson).IsModified = false;
            }

            HashedPassword hashedPassword = Argon2.HashPassword(person.Password);

            person.Password = hashedPassword.Password;
            person.Salt = hashedPassword.Salt;

            int accessorId = ((Person)HttpContext.Items["Person"]).Id;
            person.CreationDate = DateTime.UtcNow;
            person.Creator = accessorId;
            person.ModificationDate = null;
            person.Modifier = null;


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            if (person.Role == (int)Roles.Admin)
            {
                return Unauthorized();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        [AllowAnonymous]
        [HttpPost("authentication/authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse response;

            try
            {
                response = _personService.Authenticate(model, IpAddress());
            }
            catch
            {
                return Unauthorized();
            }

            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("authentication/refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            AuthenticateResponse response;

            try
            {
                response = _personService.RefreshToken(refreshToken, IpAddress());
            }
            catch
            {
                return Unauthorized();
            }

            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize(Roles.Admin)]
        [HttpPost("authentication/revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            try
            {
                _personService.RevokeToken(token, IpAddress());
            }
            catch
            {
                return Unauthorized();
            }

            return Ok(new { message = "Token revoked" });
        }

        [Authorize(Roles.Admin)]
        [HttpGet("authentication/{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            Person person;

            try
            {
                person = _personService.GetById(id);
            }
            catch
            {
                return NotFound();
            }


            return Ok(person.RefreshTokens);
        }

        // helper methods

        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
