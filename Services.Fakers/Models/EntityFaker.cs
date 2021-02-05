using Bogus;
using Models;

namespace Services.Fakers.Models
{
    public class EntityFaker<T> : Faker<T> where T : Entity
    {
        public EntityFaker() : base("pl")
        {
            StrictMode(true);
            RuleFor(x => x.Id, x => x.UniqueIndex);
        }
    }
}