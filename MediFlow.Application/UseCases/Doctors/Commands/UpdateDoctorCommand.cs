using MediatR;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record UpdateDoctorCommand(int DoctorId, int SpecialtyId, string FirstName, string LastName, string LicenseNumber, string Phone, string Email ) : IRequest<bool>;

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
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Todos los datos del médico son obligatorios.");
        }

        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            return false;

        var specialty = await _specialtyRepository.GetByIdAsync(request.SpecialtyId);
        if (specialty is null || !specialty.IsActive)
        {
            throw new ArgumentException("La especialidad indicada no existe o está inactiva.");
        }
        
        doctor.SpecialtyId = request.SpecialtyId;
        doctor.FirstName = request.FirstName.Trim();
        doctor.LastName = request.LastName.Trim();
        doctor.LicenseNumber = request.LicenseNumber.Trim();
        doctor.Phone = request.Phone.Trim();
        doctor.Email = request.Email.Trim();
        await _doctorRepository.UpdateAsync(doctor);
        return true;
    }
}
