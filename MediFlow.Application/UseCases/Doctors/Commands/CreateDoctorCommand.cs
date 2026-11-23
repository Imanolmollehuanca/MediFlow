using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record CreateDoctorCommand(CreateDoctorDto Dto) : IRequest<int>;

internal sealed class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDoctorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateDoctorCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName) ||
            string.IsNullOrWhiteSpace(dto.LicenseNumber) ||
            string.IsNullOrWhiteSpace(dto.Phone) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Todos los datos del médico son obligatorios.");
        }

        if (dto.SpecialtyId <= 0)
        {
            throw new ArgumentException("El identificador de la especialidad debe ser mayor que cero.");
        }

        var specialty = await _unitOfWork.Specialties.GetByIdAsync(dto.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
        {
            throw new ArgumentException("La especialidad indicada no existe o está inactiva.");
        }

        if (await _unitOfWork.Doctors.ExistsByLicenseNumberAsync(
                dto.LicenseNumber, cancellationToken: cancellationToken))
        {
            throw new ArgumentException("Ya existe un médico con ese número de licencia.");
        }

        if (await _unitOfWork.Doctors.ExistsByEmailAsync(
                dto.Email, cancellationToken: cancellationToken))
        {
            throw new ArgumentException("Ya existe un médico con ese correo electrónico.");
        }

        var doctor = dto.ToEntity();

        await _unitOfWork.Doctors.AddAsync(doctor);
        await _unitOfWork.SaveChangesAsync();

        return doctor.DoctorId;
    }
}
