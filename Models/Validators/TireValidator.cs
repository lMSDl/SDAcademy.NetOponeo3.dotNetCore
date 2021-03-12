using System;
using FluentValidation;

namespace Models.Validators
{
    public class TireValidator : AbstractValidator<Tire>
    {
        public TireValidator()
        {
            RuleFor(x => x.Producer)
                .NotNull()
                .When(x => x.Id == 0);

            RuleFor(x => x.Season)
                .IsInEnum();

            RuleFor(x => x.Diameter)
                .NotNull()
                .NotEmpty()
                .LessThanOrEqualTo(21)
                .GreaterThanOrEqualTo(13);
        }
    }
}