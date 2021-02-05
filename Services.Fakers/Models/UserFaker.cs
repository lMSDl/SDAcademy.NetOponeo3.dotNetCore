using Models;

namespace Services.Fakers.Models
{
    public class UserFaker : EntityFaker<User>
    {
        public UserFaker()
        {
            RuleFor(x => x.Login, x => x.Internet.UserName());
            RuleFor(x => x.Password, x => x.Internet.Password());
        }
    }
}