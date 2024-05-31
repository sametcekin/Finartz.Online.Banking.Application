using Domain.GuardClauses.Abstract;

namespace Domain.GuardClauses;

public class Guard : IGuardClause
{
    public static IGuardClause Against { get; } = new Guard();

    private Guard()
    {
    }
}
