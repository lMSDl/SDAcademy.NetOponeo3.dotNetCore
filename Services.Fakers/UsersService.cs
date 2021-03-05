using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Models;
using Services.Interfaces;

namespace Services.Fakers
{
    public class UsersService : CrudService<User>, IUsersServiceAsync
    {
        public UsersService(Faker<User> fakers, int count) : base(fakers, count)
        {
        }

        public Task<IEnumerable<User>> FindAsync(string search, Roles? role)
        {
            if(string.IsNullOrEmpty(search) && !role.HasValue)
                return ReadAsync();

            IEnumerable<User> users = Entities.ToList();    
            if(!string.IsNullOrEmpty(search)) {
                users = users.Where(x => x.Login.Contains(search, System.StringComparison.InvariantCultureIgnoreCase));
            }
            if(role.HasValue)
                users = users.Where(x => x.Role.HasFlag(role.Value));

            return Task.FromResult(users);
        }

        public Task<User> ReadByLoginAsync(string login)
        {
            return Task.FromResult(Entities.SingleOrDefault(x => x.Login == login));
        }
    }
}