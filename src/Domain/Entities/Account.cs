using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public decimal Balance { get; private set; }
    public Guid CustomerId { get; set; }
    public virtual User Customer { get; set; }

    private readonly object _lock = new();

    public void IncreaseBalance(decimal amount)
    {
        lock (_lock)
        {
            if (amount < 0)
                throw new BusinessRuleException("Amount cannot be negative.");
            Balance += amount;
        }
    }

    public void DecreaseBalance(decimal amount)
    {
        lock (_lock)
        {
            if (amount > Balance)
                throw new BusinessRuleException("Insufficient balance.");
            Balance -= amount;
        }
    }
}
