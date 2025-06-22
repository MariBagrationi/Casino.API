using Casino.Application.ModelsDTO;
using FluentValidation;

namespace Casino.API.Infrastructure.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 20).WithMessage("First name must be between 2 and 20 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 20).WithMessage("Last name must be between 2 and 20 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 20).WithMessage("Username must be between 3 and 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 8).WithMessage("Password must be exactly 8 characters long.")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Password must contain only alphanumeric characters.");

        }
    }
}
