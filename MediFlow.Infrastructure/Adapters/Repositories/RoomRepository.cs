using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class RoomRepository(MediFlowContext context)
    : GenericRepository<Room>(context), IRoomRepository
{
}
