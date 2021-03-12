using System.Collections.Generic;

namespace Models
{
    public class Order : Entity
    {
        public int UserId {get; set;}
        public User User {get; set;}

        public IEnumerable<Tire> Tires {get; set;} 
    }
}