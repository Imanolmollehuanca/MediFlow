using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Commands;

public record CreateDoctorCommand(CreateDoctorDto Dto) : IRequest<int>;

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
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName) ||
            string.IsNullOrWhiteSpace(dto.LicenseNumber) ||
            string.IsNullOrWhiteSpace(dto.Phone) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new ArgumentException("Todos los datos del médico son obligatorios.");
        }

        var specialty = await _specialtyRepository.GetByIdAsync(dto.SpecialtyId);

        if (specialty is null || !specialty.IsActive)
        {
            throw new ArgumentException("La especialidad indicada no existe o está inactiva.");
        }

        var doctor = dto.ToEntity();

        await _doctorRepository.AddAsync(doctor);

        return doctor.DoctorId;
        
    }
}
