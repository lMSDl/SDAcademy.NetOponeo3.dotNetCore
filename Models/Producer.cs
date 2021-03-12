using System.Collections.Generic;

namespace Models
{
    public class Producer : Entity
    {
        public string Name {get; set;}
        public IEnumerable<Tire> Tires{get; set;}
    }
}