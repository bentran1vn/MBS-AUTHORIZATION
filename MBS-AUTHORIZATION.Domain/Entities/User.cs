using MBS_AUTHORIZATION.Domain.Abstractions.Entities;

namespace MBS_AUTHORIZATION.Domain.Entities;

public class User : Entity<Guid>, IAuditableEntity
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Phonenumber { get; set; }
    public int Role { get; set; }
    public Guid? VendorId { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}