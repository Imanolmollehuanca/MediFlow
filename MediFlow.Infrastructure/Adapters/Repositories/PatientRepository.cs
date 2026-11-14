using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class PatientRepository(MediFlowContext context)
    : GenericRepository<Patient>(context), IPatientRepository
{
}
