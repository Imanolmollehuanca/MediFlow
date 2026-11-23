using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record UpdateDoctorCommand(
    int DoctorId,
    UpdateDoctorDto Dto) : IRequest<bool>;

internal sealed class UpdateDoctorCommandHandler
    : IRequestHandler<UpdateDoctorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDoctorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateDoctorCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName) ||
            string.IsNullOrWhiteSpace(dto.LicenseNumber) ||
            string.IsNullOrWhiteSpace(dto.Phone) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException(
                "Todos los datos del médico son obligatorios.");
        }

        if (dto.SpecialtyId <= 0)
        {
            throw new ArgumentException(
                "El identificador de la especialidad debe ser mayor que cero.");
        }

        var doctor = await _unitOfWork.Doctors.GetByIdAsync(
            request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            return false;

        var specialty = await _unitOfWork.Specialties.GetByIdAsync(
            dto.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
        {
            throw new ArgumentException(
                "La especialidad indicada no existe o está inactiva.");
        }

        if (await _unitOfWork.Doctors.ExistsByLicenseNumberAsync(
                dto.LicenseNumber, request.DoctorId, cancellationToken))
        {
            throw new ArgumentException(
                "Ya existe otro médico con ese número de licencia.");
        }

        if (await _unitOfWork.Doctors.ExistsByEmailAsync(
                dto.Email, request.DoctorId, cancellationToken))
        {
            throw new ArgumentException(
                "Ya existe otro médico con ese correo electrónico.");
        }

        dto.ApplyTo(doctor);

        await _unitOfWork.Doctors.UpdateAsync(doctor);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
