using System.Threading.Tasks;
using lab2.Entities;

namespace lab2.Services
{
    public interface IVkApiService
    {
        public Task Auth(string code);

        public void Logout();

        public string GetAuthorizeUrl();
        
        public User? CurrentUser { get; }
    }
}
