using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class PatientRepository(MediFlowContext context)
    : GenericRepository<Patient>(context), IPatientRepository
{
    public async Task<bool> ExistsByDocumentAsync(string documentType, string documentNumber, int? excludePatientId = null)
    {
        var normalizedType = documentType.Trim().ToUpper();
        var normalizedNumber = documentNumber.Trim();

        return await DbSet
            .AsNoTracking()
            .AnyAsync(p =>
                p.DocumentType.ToUpper() == normalizedType &&
                p.DocumentNumber == normalizedNumber &&
                (!excludePatientId.HasValue || p.PatientId != excludePatientId.Value));
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string? documentNumber, string? name, bool? isActive)
    {
        var query = DbSet.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(documentNumber))
        {
            var doc = documentNumber.Trim();
            query = query.Where(p => p.DocumentNumber.Contains(doc));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var term = name.Trim().ToLower();
            query = query.Where(p =>
                p.FirstName.ToLower().Contains(term) ||
                p.LastName.ToLower().Contains(term) ||
                (p.FirstName + " " + p.LastName).ToLower().Contains(term));
        }

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}