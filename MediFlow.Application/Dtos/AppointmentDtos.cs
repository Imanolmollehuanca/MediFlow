using System.ComponentModel.DataAnnotations;

namespace MediFlow.Application.Dtos;

public class AppointmentRequestDto
{
    [Range(1, int.MaxValue)] public int PatientId { get; init; }
    [Range(1, int.MaxValue)] public int DoctorId { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    [StringLength(30)] public string Status { get; init; } = "Pending";
    [StringLength(1000)] public string? Reason { get; init; }
}

public class AppointmentDto
{
    public int AppointmentId { get; init; }
    public int PatientId { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public int DoctorId { get; init; }
    public string DoctorName { get; init; } = string.Empty;
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Reason { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
