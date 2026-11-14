using MediFlow.Domain.Entities;

namespace MediFlow.Application.Port.Services;

public interface ITokenService
{
    TokenResult GenerateToken(User user);
}