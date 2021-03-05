using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Interfaces;

namespace Services.MsSqlService.Services
{
    public class CrudService<T> : ICrudServiceAsync<T> where T : Entity
    {
        protected DbContext Context {get;}

        public CrudService(DbContext context)
        {
            Context = context;
        }

        public async Task<int> CreateAsync(T entity)
        {
            var entry = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await Context.Set<T>().FindAsync(id);
            Context.Set<T>().Remove(entity);
            //Context.Set<T>().Remove(new User {Id = id});
            await Context.SaveChangesAsync();
        }

        public async Task<T> ReadAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public Task UpdateAsync(int id, T entity)
        {
            entity.Id = id;
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            //Context.Entry(entity).Property(x => x.Id).IsModified = false;
            return Context.SaveChangesAsync();
        }
    }
}