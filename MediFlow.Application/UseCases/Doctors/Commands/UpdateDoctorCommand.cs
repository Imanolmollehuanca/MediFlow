using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record UpdateDoctorCommand(int DoctorId, UpdateDoctorDto Dto) : IRequest<bool>;

internal sealed class UpdateDoctorCommandHandler
    : IRequestHandler<UpdateDoctorCommand, bool>
{
    private readonly IGenericRepository<Doctor> _doctorRepository;
    private readonly IGenericRepository<Specialty> _specialtyRepository;

    public UpdateDoctorCommandHandler(
        IGenericRepository<Doctor> doctorRepository,
        IGenericRepository<Specialty> specialtyRepository)
    {
        _doctorRepository = doctorRepository;
        _specialtyRepository = specialtyRepository;
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
            throw new ArgumentException("Todos los datos del médico son obligatorios.");
        }

        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            return false;

        var specialty = await _specialtyRepository.GetByIdAsync(dto.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
        {
            throw new ArgumentException("La especialidad indicada no existe o está inactiva.");
        }

        doctor.SpecialtyId = dto.SpecialtyId;
        doctor.FirstName = dto.FirstName.Trim();
        doctor.LastName = dto.LastName.Trim();
        doctor.LicenseNumber = dto.LicenseNumber.Trim();
        doctor.Phone = dto.Phone.Trim();
        doctor.Email = dto.Email.Trim();

        await _doctorRepository.UpdateAsync(doctor);

        return true;
    }
}
