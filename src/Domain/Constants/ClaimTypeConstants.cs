using System.Security.Claims;

namespace Domain.Constants;

internal class ClaimTypeConstants
{
    public const string Sid = "_id";

    public const string Name = "firstname";

    public const string Surname = "lastname";

    public const string Email = ClaimTypes.Email;
}
