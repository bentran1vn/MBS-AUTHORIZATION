using MBS_AUTHORIZATION.Domain.Abstractions.Entities;

namespace MBS_AUTHORIZATION.Domain.Entities;

public class User : Entity<Guid>, IAuditableEntity
{
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string Password { get; set; }
    //  public bool Gender { get; set; }
    //  public string Phonenumber { get; set; }
    public int Role { get; set; }
    public int Points { get; set; }
    public int Status { get; set; }
    public Guid? MentorId { get; set; }
    public bool IsFirstLogin { get; set; } = true;

    public virtual User? Mentor { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}