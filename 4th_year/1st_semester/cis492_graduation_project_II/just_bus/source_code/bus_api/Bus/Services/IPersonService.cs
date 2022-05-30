using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Helpers;
using WebApi.Authorization;
using Bus.Models;
using WebApi.Models.Users;
using Bus.Helpers;

namespace WebApi.Services
{
    public interface IPersonService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        IEnumerable<Person> GetAll();
        Person GetById(int id);
    }

    public class PersonService : IPersonService
    {
        private readonly BusContext _context;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public PersonService(
            BusContext context,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var person = _context.People.SingleOrDefault(x => x.Number == model.Number);

            // validate
            if (person == null)
                throw new AppException("User Doesnt Exist");

            bool isAutherized = Argon2.VerifyHash(model.Password, person.Password, person.Salt);

            // validate
            if (!isAutherized)
                throw new AppException("Person name or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(person);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            person.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from person
            RemoveOldRefreshTokens(person);

            // save changes to db
            _context.Update(person);
            _context.SaveChanges();

            return new AuthenticateResponse(person, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var person = GetPersonByRefreshToken(token);
            var refreshToken = _context.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, person, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _context.Update(person);
                _context.SaveChanges();
            }

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
            person.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from person
            RemoveOldRefreshTokens(person);

            // save changes to db
            _context.Update(person);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(person);
            return new AuthenticateResponse(person, jwtToken, newRefreshToken.Token);
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var person = GetPersonByRefreshToken(token);
            var refreshToken = _context.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _context.Update(person);
            _context.SaveChanges();
        }

        public IEnumerable<Person> GetAll()
        {
            return _context.People;
        }

        public Person GetById(int id)
        {
            var person = _context.People.Find(id);
            if (person == null) throw new KeyNotFoundException("Person not found");
            return person;
        }

        // helper methods

        private Person GetPersonByRefreshToken(string token)
        {
            var person = _context.People.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (person == null)
                throw new AppException("Invalid token");

            return person;
        }

        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens(Person person)
        {
            // remove old inactive refresh tokens from person based on TTL in app settings
            foreach (var item in person.RefreshTokens)
            {
                if (!item.IsActive && item.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow)
                {
                    person.RefreshTokens.Remove(item);
                }
            }

            //person.RefreshTokens.RemoveAll(x =>
            //    !x.IsActive &&
            //    x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, Person person, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = _context.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    RevokeRefreshToken(childToken, ipAddress, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, person, ipAddress, reason);
            }
        }

        private static void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
    }
}