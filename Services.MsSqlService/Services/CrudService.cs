using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.MsSqlService.Services
{
    public class CrudService<T> : ICrudServiceAsync<T> where T : Entity
    {
        protected DbContextOptions<Context> Options {get;}

        public CrudService(DbContextOptions<Context> options)
        {
            Options = options;
        }

        public async Task<int> CreateAsync(T entity)
        {
            using(var context = new Context(Options)) {
                var entry = await context.AddAsync(entity);
                await context.SaveChangesAsync();
                return entry.Entity.Id;
            }
        }

        public async Task DeleteAsync(int id)
        {
            using(var context = new Context(Options)) {
                var entity = await context.Set<T>().FindAsync(id);
                context.Set<T>().Remove(entity);
                //Context.Set<T>().Remove(new User {Id = id});
                await context.SaveChangesAsync();
            }
        }

        public virtual async Task<T> ReadAsync(int id)
        {
            using(var context = new Context(Options)) {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<IEnumerable<T>> ReadAsync()
        {
            using(var context = new Context(Options)) {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        public async Task UpdateAsync(int id, T entity)
        {
            using(var context = new Context(Options)) {
                entity.Id = id;
                context.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                //Context.Entry(entity).Property(x => x.Id).IsModified = false;
                await context.SaveChangesAsync();
            }
        }
    }
}