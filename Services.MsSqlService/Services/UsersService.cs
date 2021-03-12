using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.MsSqlService.Services
{
    public class UsersService : CrudService<User>, IUsersServiceAsync
    {
        public UsersService(DbContextOptions<Context> options) : base(options)
        {
        }

        public async Task<IEnumerable<User>> FindAsync(string search, Roles? role)
        {
            if(string.IsNullOrEmpty(search) && !role.HasValue)
                return await ReadAsync();

            using(var context = new Context(Options)) {
                var query = context.Set<User>().AsNoTracking();    
                if(!string.IsNullOrEmpty(search)) {
                    query = query.Where(x => x.Login.Contains(search, System.StringComparison.InvariantCultureIgnoreCase));
                }
                if(role.HasValue)
                    query = query.Where(x => x.Role.HasFlag(role.Value));

                return await query.ToListAsync();
            }
        }

        public async Task<User> ReadByLoginAsync(string login)
        {
            using(var context = new Context(Options)) {
                return await context.Set<User>().AsNoTracking().SingleOrDefaultAsync(x => x.Login == login);
            }
        }
    }
}