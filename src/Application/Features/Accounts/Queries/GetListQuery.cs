using Application.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Accounts.Queries
{
    public class GetListQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetListQuery, List<Account>>
        {
            private readonly IRepository<Account> _repository;
            public Handler(IRepository<Account> repository)
            {
                _repository = repository;
            }

            public async Task<List<Account>> Handle(GetListQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetListAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
