using Application.Abstractions;
using Application.Helpers;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.GuardClauses;
using MediatR;

namespace Application.Features.Users.Commands;

public class RegisterCommand : IRequest<Result<Guid>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public class RegisterCommandHandler(IRepository<User> repository) : IRequestHandler<RegisterCommand, Result<Guid>>
    {
        private readonly IRepository<User> _repository = repository;

        public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var checkIfExists = Guard.Against.ModelNotNull<User, BusinessRuleException>(await _repository.GetFirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken: cancellationToken), "User with the same username already exists.");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                PasswordHash = PasswordHasher.HashPasword(request.Password, out var salt),
                PasswordSalt = salt
            };

            await _repository.AddAsync(user, cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
    }
}
