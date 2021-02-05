using System;
using System.Collections.Generic;
using Bogus;
using Models;
using Services.Interfaces;
using System.Linq;

namespace Services.Fakers
{
    public class CrudService<T> : ICrudService<T> where T : Entity
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
            return Entities.ToList();
        }

        public void Update(int id, T entity)
        {
            if(Read(id) == null)
                throw new KeyNotFoundException();

            Delete(id);
            entity.Id = id;
            Entities.Add(entity);
        }
    }
}
