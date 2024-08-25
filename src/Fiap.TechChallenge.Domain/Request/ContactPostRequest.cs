using FluentValidation;


namespace Fiap.TechChallenge.Domain.Request;

public class ContactPostRequest(short ddd, string email, string phoneNumber, string name)
{
    public short Ddd { get; init; } = ddd;
    public string Email { get; init; } = email;
    public string PhoneNumber { get; init; } = phoneNumber;
    public string Name { get; init; } = name;
}

public class ContactPostRequestValidator : AbstractValidator<ContactPostRequest>
{
    public ContactPostRequestValidator()
    {
        RuleFor(x => x.Ddd).NotEmpty().WithMessage("DDD is required");
        RuleFor(x => x.Ddd).LessThan((short)100).WithMessage("invalid ddd number");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format");
        RuleFor(x => x.PhoneNumber).Length(9).WithMessage("Phone number must contain 9 digits");
        RuleFor(x => x.Name).MaximumLength(200).WithMessage("The length of 'Name' must be 200 characters or fewer.");
        RuleFor(x => x.Name).MinimumLength(3).WithMessage("The length of 'Name' must be at least 3 characters.");
    }
}