namespace MediFlow.Application.Dtos;

public record CreateUserDto(
    string Username, string Email, string Password
);