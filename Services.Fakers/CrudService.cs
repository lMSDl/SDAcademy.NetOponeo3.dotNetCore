using System;
using System.Collections.Generic;
using Bogus;
using Models;
using Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Fakers
{
    public class CrudService<T> : ICrudService<T>, ICrudServiceAsync<T> where T : Entity
    {
        public CrudService(Faker<T> fakers, int count)
        {
            Entities = fakers.Generate(count);

            _index = Entities.Max(x => x.Id);
        }

        private ICollection<T> Entities {get;}
        private int _index;

        public int Create(T entity)
        {
            entity.Id = ++_index;
            Entities.Add(entity);
            return entity.Id;
        }

        public void Delete(int id)
        {
            Entities.Remove(Read(id));
        }

        public T Read(int id)
        {
            return Entities.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> Read()
        {
            return ReadAsync().Result;
            //return Entities.ToList();
        }

        public void Update(int id, T entity)
        {
            if(Read(id) == null)
                throw new KeyNotFoundException();

            Delete(id);
            entity.Id = id;
            Entities.Add(entity);
        }

        public Task<int> CreateAsync(T entity)
        {
            //return new Task<int>(() => Create(entity)));
            return Task.FromResult(Create(entity));
            // entity.Id = ++_index;
            // Entities.Add(entity);
            // return Task.FromResult(entity.Id);
        }

        public Task<T> ReadAsync(int id)
        {
            //return new Task<T>(() => Entities.SingleOrDefault(x => x.Id == id));
            var result = Entities.SingleOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(Entities.ToList().AsEnumerable());
        }

        public async Task UpdateAsync(int id, T entity)
        {
            if(await ReadAsync(id) == null)
                throw new KeyNotFoundException();

            Delete(id);
            entity.Id = id;
            Entities.Add(entity);
        }

        public Task DeleteAsync(int id)
        {
            Entities.Remove(Read(id));
            return Task.CompletedTask;
        }
    }
}
