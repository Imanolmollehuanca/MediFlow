using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Queries;

public record GetDoctorByIdQuery(int DoctorId)
    : IRequest<DoctorDto?>;

internal sealed class GetDoctorByIdQueryHandler
    : IRequestHandler<GetDoctorByIdQuery, DoctorDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDoctorByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DoctorDto?> Handle(
        GetDoctorByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DoctorId <= 0)
            return null;

        var doctor = await _unitOfWork.Doctors.GetByIdAsync(
            request.DoctorId);

        return doctor is null
            ? null
            : DoctorDto.FromEntity(doctor);
    }
}
