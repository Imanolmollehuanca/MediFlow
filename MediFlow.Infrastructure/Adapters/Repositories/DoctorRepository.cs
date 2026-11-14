using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class DoctorRepository(MediFlowContext context)
    : GenericRepository<Doctor>(context), IDoctorRepository
{
}
