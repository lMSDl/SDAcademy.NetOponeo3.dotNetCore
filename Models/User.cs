using System.ComponentModel;

namespace Models
{
    public class User : Entity
    {
        [DisplayName("Username")]
        public string Login {get; set;} 
        public string Password {get; set;} 
    }
}