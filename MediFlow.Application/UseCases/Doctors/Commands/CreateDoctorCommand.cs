
using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record CreateDoctorCommand(int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email ) : IRequest<int>;

internal sealed class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, int>
{
    private readonly IGenericRepository<Doctor> _doctorRepository;
    private readonly IGenericRepository<Specialty> _specialtyRepository;

    public CreateDoctorCommandHandler(
        IGenericRepository<Doctor> doctorRepository,
        IGenericRepository<Specialty> specialtyRepository)
    {
        _doctorRepository = doctorRepository;
        _specialtyRepository = specialtyRepository;
    }

    public async Task<int> Handle(
        CreateDoctorCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Todos los datos del médico son obligatorios.");
        }
        var specialty = await _specialtyRepository.GetByIdAsync(request.SpecialtyId);
        if (specialty is null || !specialty.IsActive)
        {
            throw new ArgumentException("La especialidad indicada no existe o está inactiva.");
        }

        var doctor = new Doctor
        {
            SpecialtyId = request.SpecialtyId,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            LicenseNumber = request.LicenseNumber.Trim(),
            Phone = request.Phone.Trim(),
            Email = request.Email.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _doctorRepository.AddAsync(doctor);
        return doctor.DoctorId;
    }
}
