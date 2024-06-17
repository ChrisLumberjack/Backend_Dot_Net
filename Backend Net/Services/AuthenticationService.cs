using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Backend_Net.Interfaces;
namespace Backend_Net.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GetGoogleClientIdAsync()
        {
            return Task.FromResult(_configuration["Google:ClientId"]);
        }

        public Task<string> GetGoogleClientSecretAsync()
        {
            return Task.FromResult(_configuration["Google:ClientSecret"]);
        }
    }
}
