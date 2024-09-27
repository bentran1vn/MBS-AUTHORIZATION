namespace MBS_AUTHORIZATION.Application.Abstractions;

public interface IPasswordHasherService
{
    public string HashPassword(string password);

    public bool VerifyPassword(string password, string hashedPassword);
}