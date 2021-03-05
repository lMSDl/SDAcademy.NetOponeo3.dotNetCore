using Models;

namespace Services.Fakers.Models
{
    public class TireFaker : EntityFaker<Tire>
    {
        public TireFaker()
        {
            RuleFor(x => x.Producer, x => x.Company.CompanyName());
            RuleFor(x => x.Season, x => x.PickRandom<TireSeason>());
            RuleFor(x => x.Width, x => x.Random.Int(6, 355));
            RuleFor(x => x.Profile, x => x.Random.Int(35, 80));
            RuleFor(x => x.Diameter, x => x.Random.Int(15, 19));
        }
    }
}