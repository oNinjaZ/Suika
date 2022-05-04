using FluentValidation;
using Suika.Api.Models;

namespace Suika.Api.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username).NotEmpty().WithMessage("Must not be empty.");
        RuleFor(user => user.Username).Length(3, 20).WithMessage("Must be between 3 and 20 characters long.");
        RuleFor(user => user.Username).Matches("^[a-zA-Z0-9]*$").WithMessage("Must contain only letters and numbers.");

        RuleFor(user => user.Email).NotEmpty();
        // todo - email regex

        RuleFor(user => user.RegistrationDate).NotEmpty();
    }
}