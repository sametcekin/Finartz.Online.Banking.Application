using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.GuardClauses;
using MediatR;

namespace Application.Features.Accounts.Commands;

public class DepositMoneyCommand : IRequest<Result<bool>>
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }

    public class DepositMoneyCommandHandler : IRequestHandler<DepositMoneyCommand, Result<bool>>
    {
        private readonly IRepository<Account> _repository;
        public DepositMoneyCommandHandler(IRepository<Account> repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(DepositMoneyCommand request, CancellationToken cancellationToken)
        {
            var account = Guard.Against.ModelNull<Account, BusinessRuleException>(await _repository.GetFirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken: cancellationToken), $"Account not found");

            account.IncreaseBalance(request.Amount);

            return Result<bool>.Success(true);
        }
    }
}
