using Application.Abstractions;
using Application.Helpers;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.GuardClauses;
using Domain.SharedCore;
using MediatR;

namespace Application.Features.Users.Commands;

public class LoginCommand : IRequest<Result<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }

    public class LoginCommandHandler(IRepository<User> repository, ITokenProvider tokenProvider) : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IRepository<User> _repository = repository;
        private readonly ITokenProvider _tokenProvider;
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = Guard.Against.ModelNull<User, BusinessRuleException>(await _repository.GetFirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken: cancellationToken), "User not found.");

            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                return Result<string>.Failure("Invalid password.");

            var token = tokenProvider.GenerateToken(user);


            return Result<string>.Success(token);
        }
    }
}
