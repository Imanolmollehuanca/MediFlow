using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Prescriptions.Commands;

public record CreatePrescriptionCommand(CreatePrescriptionDto Prescription)
    : IRequest<PrescriptionDto>;

public class CreatePrescriptionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePrescriptionCommand, PrescriptionDto>
{
    public async Task<PrescriptionDto> Handle(
        CreatePrescriptionCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Prescription;

        if (dto.DiagnosisId <= 0)
            throw new ArgumentException(
                "El ID del diagnóstico debe ser válido.");

        var diagnosis = await unitOfWork.Diagnoses
            .GetByIdAsync(dto.DiagnosisId);

        if (diagnosis is null)
            throw new KeyNotFoundException(
                "El diagnóstico no existe.");

        var appointment = await unitOfWork.Appointments
            .GetByIdAsync(diagnosis.AppointmentId);

        if (appointment is null)
            throw new KeyNotFoundException(
                "La cita relacionada con el diagnóstico no existe.");

        if (appointment.Status == "Cancelled")
            throw new InvalidOperationException(
                "No se puede emitir una receta para una cita cancelada.");

        var prescription = dto.ToEntity();

        await unitOfWork.Prescriptions.AddAsync(prescription);
        await unitOfWork.SaveChangesAsync();

        return new PrescriptionDto(prescription);
    }
}