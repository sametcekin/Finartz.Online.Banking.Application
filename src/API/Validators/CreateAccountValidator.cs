using API.Models.Requests.Accounts;
using FluentValidation;

namespace API.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.AccountNumber)
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(x => x.AccountHolderName)
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
