using System.Threading.Tasks;
using lab2.Entities;

namespace lab2.Services
{
    public interface IVkApiService
    {
        public Task<User?> Auth(string code);

        public string GetAuthorizeUrl();
    }
}
