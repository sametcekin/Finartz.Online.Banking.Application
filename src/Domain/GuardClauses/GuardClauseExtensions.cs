using Domain.Exceptions;
using Domain.GuardClauses.Abstract;

namespace Domain.GuardClauses;

public static class GuardClauseExtensions
{
    public static T ModelNull<T, TException>(this IGuardClause guardClause, [ValidatedNotNull] T input, string? message = null) where TException : BusinessRuleException, new()
    {
        if (input is null)
        {
            var exception = (TException)Activator.CreateInstance(typeof(TException), message) ?? throw new InvalidOperationException("Unable to create exception instance.");
            throw exception;
        }

        return input;
    }

    public static T ModelNotNull<T, TException>(this IGuardClause guardClause, [ValidatedNotNull] T input, string? message = null) where TException : BusinessRuleException, new()
    {
        if (input is not null)
        {
            var exception = (TException)Activator.CreateInstance(typeof(TException), message) ?? throw new InvalidOperationException("Unable to create exception instance.");
            throw exception;
        }

        return input;
    }


}
