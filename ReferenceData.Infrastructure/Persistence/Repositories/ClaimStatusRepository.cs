using Microsoft.EntityFrameworkCore;
using ReferenceData.Application.Interfaces;
using ReferenceData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Infrastructure.Persistence.Repositories
{
    public class ClaimStatusRepository(ReferenceDataDbContext db) : IClaimStatusRepository
    {
        public async Task<List<ClaimStatus>> GetAllAsync(bool? isActive, CancellationToken ct) =>
            await db.ClaimStatuses  
                .Where(x => !isActive.HasValue || x.IsActive == isActive)
                .OrderBy(x => x.Code)
                .ToListAsync(ct);

        public async Task<ClaimStatus?> GetByIdAsync(int id, CancellationToken ct) =>
            await db.ClaimStatuses.FindAsync([id], ct);

        public async Task<ClaimStatus?> GetByCodeAsync(string code, CancellationToken ct) =>
            await db.ClaimStatuses.FirstOrDefaultAsync(x => x.Code == code, ct);
        public async Task<bool> ExistsAsync(string code, CancellationToken ct) =>
            await db.ClaimStatuses.AnyAsync(x => x.Code == code, ct);

        public async Task<ClaimStatus> CreateAsync(ClaimStatus entity, CancellationToken ct)
        {
            db.ClaimStatuses.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<ClaimStatus> UpdateAsync(ClaimStatus entity, CancellationToken ct)
        {
            db.ClaimStatuses.Update(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(ClaimStatus entity, CancellationToken ct)
        {
            db.ClaimStatuses.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
    }
}
