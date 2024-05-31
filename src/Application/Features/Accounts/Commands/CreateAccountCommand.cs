using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.GuardClauses;
using MediatR;

namespace Application.Features.Accounts.Commands;

public class CreateAccountCommand : IRequest<Result<Guid>>
{
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public decimal Balance { get; set; }

    public class CreateAccountCommandHandler(IRepository<Account> repository) : IRequestHandler<CreateAccountCommand, Result<Guid>>
    {
        private readonly IRepository<Account> _repository = repository;

        public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var checkIfAccountExists = Guard.Against.ModelNotNull<Account, BusinessRuleException>(await _repository.GetFirstOrDefaultAsync(x => x.AccountNumber == request.AccountNumber, cancellationToken: cancellationToken), "Account with the same account number already exists.");

            var account = new Account
            {
                AccountNumber = request.AccountNumber,
                AccountHolderName = request.AccountHolderName
            };

            await _repository.AddAsync(account, cancellationToken);

            return Result<Guid>.Success(account.Id);
        }
    }
}
