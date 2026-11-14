using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class MedicationRepository(MediFlowContext context)
    : GenericRepository<Medication>(context), IMedicationRepository
{
}
