using MediatR;
using MediFlow.Application.Dtos;
using MediFlow.Domain.Port.Repositories;

namespace MediFlow.Application.UseCases.Prescriptions.Queries;

public record GetPrescriptionsByPatientQuery(int PatientId)
    : IRequest<IEnumerable<PrescriptionDto>>;

public class GetPrescriptionsByPatientQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPrescriptionsByPatientQuery, IEnumerable<PrescriptionDto>>
{
    public async Task<IEnumerable<PrescriptionDto>> Handle(
        GetPrescriptionsByPatientQuery request,
        CancellationToken cancellationToken)
    {
        var prescriptions = await unitOfWork.Prescriptions.GetAllAsync();
        var diagnoses = await unitOfWork.Diagnoses.GetAllAsync();
        var appointments = await unitOfWork.Appointments.GetAllAsync();

        var diagnosisIds = diagnoses
            .Where(d => appointments.Any(a =>
                a.AppointmentId == d.AppointmentId &&
                a.PatientId == request.PatientId))
            .Select(d => d.DiagnosisId)
            .ToHashSet();

        return prescriptions
            .Where(p => diagnosisIds.Contains(p.DiagnosisId))
            .Select(p => new PrescriptionDto(p));
    }
}