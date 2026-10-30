namespace MediFlow.Domain.Entities;

public class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Relación con los usuarios
    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}