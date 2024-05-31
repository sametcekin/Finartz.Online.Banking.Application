using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.GuardClauses;
using MediatR;

namespace Application.Features.Accounts.Commands;

public class WithdrawMoneyCommand : IRequest<Result<bool>>
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }

    public class WithdrawMoneyCommandHandler : IRequestHandler<WithdrawMoneyCommand, Result<bool>>
    {
        private readonly IRepository<Account> _repository;
        public WithdrawMoneyCommandHandler(IRepository<Account> repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(WithdrawMoneyCommand request, CancellationToken cancellationToken)
        {
            var account = Guard.Against.ModelNull<Account, BusinessRuleException>(await _repository.GetFirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken: cancellationToken), "Account not found");

            account.DecreaseBalance(request.Amount);

            return Result<bool>.Success(true);
        }
    }
}
