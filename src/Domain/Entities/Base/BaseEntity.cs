namespace Domain.Entities.Base;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
