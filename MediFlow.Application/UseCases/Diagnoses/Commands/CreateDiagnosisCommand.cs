using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Diagnoses.Commands;

public record CreateDiagnosisCommand(CreateDiagnosisDto Dto) : IRequest<int>;

internal sealed class CreateDiagnosisCommandHandler(
    IGenericRepository<Diagnosis> diagnoses,
    IGenericRepository<MedicalRecord> medicalRecords,
    IGenericRepository<Appointment> appointments,
    IGenericRepository<Doctor> doctors
) : IRequestHandler<CreateDiagnosisCommand, int>
{
    public async Task<int> Handle(
        CreateDiagnosisCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException("La descripción del diagnóstico es obligatoria.");

        var medicalRecord = await medicalRecords.GetByIdAsync(dto.MedicalRecordId);
        if (medicalRecord is null)
            throw new ArgumentException("La historia clínica no existe.");

        var appointment = await appointments.GetByIdAsync(dto.AppointmentId);
        if (appointment is null)
            throw new ArgumentException("La cita médica no existe.");

        var doctor = await doctors.GetByIdAsync(dto.DoctorId);
        if (doctor is null)
            throw new ArgumentException("El médico no existe.");

        var diagnosis = dto.ToEntity();
        await diagnoses.AddAsync(diagnosis);
        return diagnosis.DiagnosisId;
    }
}