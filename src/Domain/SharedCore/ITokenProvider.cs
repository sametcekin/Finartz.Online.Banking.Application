using Domain.Entities;

namespace Domain.SharedCore;

public interface ITokenProvider
{
    string GenerateToken(User user);
}
