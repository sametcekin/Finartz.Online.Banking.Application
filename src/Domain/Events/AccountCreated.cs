namespace Domain.Events;

public class AccountCreated
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
}
