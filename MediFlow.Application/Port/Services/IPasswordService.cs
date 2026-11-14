namespace MediFlow.Application.Port.Services;

public interface IPasswordService
{
    string HashPassword(string password);

    bool VerifyPassword(string passwordHash, string password);
}