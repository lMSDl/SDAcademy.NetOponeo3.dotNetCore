using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IUsersServiceAsync : ICrudServiceAsync<User>
    {
        Task<User> ReadByLoginAsync(string login);
        Task<IEnumerable<User>> FindAsync(string search, Roles? role);
    }
}