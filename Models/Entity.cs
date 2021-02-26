using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Entity
    {
        [DisplayName("Identifier")]
        //[Display(Name = "Identifier", ResourceType = typeof(Program))]
        public int Id {get; set;}
    }
}