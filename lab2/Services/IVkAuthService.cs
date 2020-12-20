using System.Threading.Tasks;
using lab2.Models;

namespace lab2.Services
{
    public interface IVkAuthService
    {
        public string AuthorizeUrl { get; }
        public Task<AccessTokenData> Auth(string code);
    }
}
