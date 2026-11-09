using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Queries;

public record GetDoctorByIdQuery(int DoctorId)
    : IRequest<DoctorDto?>;

internal sealed class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto?>
{
    private readonly IGenericRepository<Doctor> _repository;

    public GetDoctorByIdQueryHandler(
        IGenericRepository<Doctor> repository)
    {
        _repository = repository;
    }

    public async Task<DoctorDto?> Handle(
        GetDoctorByIdQuery request,
        CancellationToken cancellationToken)
    {
        var doctor = await _repository.GetByIdAsync(request.DoctorId);

        if (doctor is null)
            return null;

        return new DoctorDto(doctor);
    }
}