using Functions.Accounts.Core.Domain;
using System.Threading.Tasks;

namespace Functions.Accounts.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);

        Task<string> InsertUserAsync(User user);
    }
}
