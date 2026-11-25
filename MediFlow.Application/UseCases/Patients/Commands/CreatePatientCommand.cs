using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Patients.Commands;

public record CreatePatientCommand(CreatePatientDto Dto) : IRequest<int>;

internal sealed class CreatePatientCommandHandler
    : IRequestHandler<CreatePatientCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePatientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var patient = new Patient
        {
            DocumentType = dto.DocumentType,
            DocumentNumber = dto.DocumentNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return patient.PatientId;
    }
}