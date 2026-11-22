using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.DoctorRooms.Commands;

internal static class DoctorRoomValidation
{
    public static async Task ValidateAsync(
        IUnitOfWork unitOfWork, int doctorId, int roomId, bool isActive,
        int? excludedAssignmentId, CancellationToken cancellationToken)
    {
        if (doctorId <= 0 || roomId <= 0)
            throw new ArgumentException("Los IDs del médico y de la sala deben ser positivos.");

        var doctor = await unitOfWork.Doctors.GetByIdAsync(doctorId)
            ?? throw new KeyNotFoundException("El médico no existe.");
        var room = await unitOfWork.Rooms.GetByIdAsync(roomId)
            ?? throw new KeyNotFoundException("La sala no existe.");

        if (!isActive)
            return;

        if (!doctor.IsActive || !room.IsActive)
            throw new InvalidOperationException("La asignación activa requiere un médico y una sala activos.");

        if (await unitOfWork.DoctorRooms.HasActiveAssignmentAsync(
                doctorId, roomId, excludedAssignmentId, cancellationToken))
            throw new InvalidOperationException("El médico ya tiene una asignación activa en esta sala.");

        if (await unitOfWork.DoctorRooms.HasScheduleConflictAsync(
                doctorId, roomId, excludedAssignmentId, cancellationToken))
            throw new InvalidOperationException("La sala ya está ocupada por otro médico en un horario del médico asignado.");
    }
}
