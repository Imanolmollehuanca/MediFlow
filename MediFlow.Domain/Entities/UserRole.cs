namespace MediFlow.Domain.Entities;

public class UserRole
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    // Relación con el usuario
    public User User { get; set; } = null!;

    // Relación con el rol
    public Role Role { get; set; } = null!;
}