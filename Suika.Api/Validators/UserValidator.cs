using FluentValidation;
using Suika.Api.Models;

namespace Suika.Api.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username).NotEmpty().WithMessage("Username field must not be empty.");
        RuleFor(user => user.Username).Length(3, 20).WithMessage("");

        RuleFor(user => user.Email).NotEmpty();
        // todo - email regex

        RuleFor(user => user.RegistrationDate).NotEmpty();
    }
}