using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Entity
    {
        //[DisplayName("Identifier")]
        [Display(Name = "Identifier", ResourceType = typeof(Models.Properties.Resources))]
        public int Id {get; set;}
    }
}