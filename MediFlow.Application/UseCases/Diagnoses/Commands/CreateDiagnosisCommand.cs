using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Commands;

public record CreateDiagnosisCommand(CreateDiagnosisDto Dto) : IRequest<int>;

internal sealed class CreateDiagnosisCommandHandler(
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateDiagnosisCommand, int>
{
    public async Task<int> Handle(
        CreateDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException("La descripción del diagnóstico es obligatoria.");

        if (dto.MedicalRecordId <= 0 || dto.AppointmentId <= 0 || dto.DoctorId <= 0)
            throw new ArgumentException("Los IDs de historia clínica, cita y médico deben ser válidos.");

        var medicalRecord = await unitOfWork.MedicalRecords.GetByIdAsync(dto.MedicalRecordId);

        if (medicalRecord is null)
            throw new ArgumentException("La historia clínica no existe.");

        var appointment = await unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);

        if (appointment is null)
            throw new ArgumentException("La cita médica no existe.");

        var doctor = await unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);

        if (doctor is null)
            throw new ArgumentException("El médico no existe.");

        if (appointment.PatientId != medicalRecord.PatientId)
            throw new ArgumentException("La cita no corresponde al paciente de la historia clínica.");

        if (appointment.DoctorId != dto.DoctorId)
            throw new ArgumentException("El médico no corresponde al médico asignado a la cita.");

        var diagnosis = dto.ToEntity();

        await unitOfWork.Diagnoses.AddAsync(diagnosis);
        await unitOfWork.SaveChangesAsync();

        return diagnosis.DiagnosisId;
    }
}