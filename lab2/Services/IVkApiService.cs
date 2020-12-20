using System.Threading.Tasks;
using lab2.Entities;

namespace lab2.Services
{
    public interface IVkApiService
    {
        Task<User> GetUser(int id, string accessCode);
    }
}