using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Doctors.Queries;

public record GetDoctorsQuery()
    : IRequest<IEnumerable<DoctorDto>>;

internal sealed class GetDoctorsQueryHandler
    : IRequestHandler<GetDoctorsQuery, IEnumerable<DoctorDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDoctorsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DoctorDto>> Handle(
        GetDoctorsQuery request,
        CancellationToken cancellationToken)
    {
        var doctors = await _unitOfWork.Doctors.GetAllAsync();

        return doctors.Select(DoctorDto.FromEntity).ToList();
    }
}
