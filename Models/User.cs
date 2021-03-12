using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User : Entity
    {
        [Required]
        [MinLength(5)]
        public string Login {get; set;} 
        public string Password {get; set;} 
        public Roles Role {get; set;}

        //public IEnumerable<Order> Orders {get; set;}
    }
}