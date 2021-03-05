using FluentValidation;

namespace Models.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() {

            RuleFor(x => x.Password)
            .NotNull().NotEmpty().Length(8, 32)/*.WithMessage("Błąd")*/.When(x => x.Id == 0)
            .Length(4,16).Must(x => x?.Contains("!") ?? false).WithName("Hasło");

            RuleFor(x => x.Role).IsInEnum();
        }
    }
}