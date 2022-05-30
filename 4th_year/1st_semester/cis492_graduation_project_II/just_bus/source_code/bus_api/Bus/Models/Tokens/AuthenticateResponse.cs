using Bus.Models;
using Newtonsoft.Json;
namespace WebApi.Models.Users
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(Person person, string jwtToken, string refreshToken)
        {
            Id = person.Id;
            Name = person.Name;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}