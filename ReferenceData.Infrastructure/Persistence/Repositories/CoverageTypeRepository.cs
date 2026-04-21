using Microsoft.EntityFrameworkCore;
using ReferenceData.Application.Interfaces;
using ReferenceData.Domain;

namespace ReferenceData.Infrastructure.Persistence.Repositories
{
    public class CoverageTypeRepository(ReferenceDataDbContext db) : ICoverageTypeRepository
    {
        public async Task<List<CoverageType>> GetAllAsync(bool? isActive, CancellationToken ct) =>
            await db.CoverageTypes
                .Where(x => !isActive.HasValue || x.IsActive == isActive)
                .OrderBy(x => x.Code)
                .ToListAsync(ct);

        public async Task<CoverageType?> GetByIdAsync(int id, CancellationToken ct) =>
            await db.CoverageTypes.FindAsync([id], ct);

        public async Task<CoverageType?> GetByCodeAsync(string code, CancellationToken ct) =>
            await db.CoverageTypes.FirstOrDefaultAsync(x => x.Code == code, ct);
        public async Task<bool> ExistsAsync(string code, CancellationToken ct) =>
            await db.CoverageTypes.AnyAsync(x => x.Code == code, ct);

        public async Task<CoverageType> CreateAsync(CoverageType entity, CancellationToken ct)
        {
            db.CoverageTypes.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<CoverageType> UpdateAsync(CoverageType entity, CancellationToken ct)
        {
            db.CoverageTypes.Update(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(CoverageType entity, CancellationToken ct)
        {
            db.CoverageTypes.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
    }
}
