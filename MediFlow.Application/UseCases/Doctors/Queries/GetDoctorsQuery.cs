using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Queries;
public record GetDoctorsQuery() : IRequest<IEnumerable<DoctorDto>>;

internal sealed class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, IEnumerable<DoctorDto>>
{
    private readonly IGenericRepository<Doctor> _repository;
    public GetDoctorsQueryHandler(IGenericRepository<Doctor> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DoctorDto>> Handle(
        GetDoctorsQuery request,
        CancellationToken cancellationToken)
    {
        var doctors = await _repository.GetAllAsync();

        return doctors.Select(d => new DoctorDto(d.DoctorId, d.SpecialtyId, d.FirstName, d.LastName, d.LicenseNumber, d.Phone, d.Email, d.IsActive, d.CreatedAt
        )).ToList();
    }
}