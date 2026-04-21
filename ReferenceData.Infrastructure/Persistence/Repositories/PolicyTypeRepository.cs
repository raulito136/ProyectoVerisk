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
    public class PolicyTypeRepository(ReferenceDataDbContext db) : IPolicyTypeRepository
    {
        public async Task<List<PolicyType>> GetAllAsync(bool? isActive, CancellationToken ct) =>
            await db.PolicyTypes
                .Where(x => !isActive.HasValue || x.IsActive == isActive)
                .OrderBy(x => x.Code)
                .ToListAsync(ct);

        public async Task<PolicyType?> GetByIdAsync(int id, CancellationToken ct) =>
            await db.PolicyTypes.FindAsync([id], ct);

        public async Task<PolicyType?> GetByCodeAsync(string code, CancellationToken ct) =>
            await db.PolicyTypes.FirstOrDefaultAsync(x => x.Code == code, ct);

        public async Task<bool> ExistsAsync(string code, CancellationToken ct) =>
            await db.PolicyTypes.AnyAsync(x => x.Code == code, ct);

        public async Task<PolicyType> CreateAsync(PolicyType entity, CancellationToken ct)
        {
            db.PolicyTypes.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<PolicyType> UpdateAsync(PolicyType entity, CancellationToken ct)
        {
            db.PolicyTypes.Update(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(PolicyType entity, CancellationToken ct)
        {
            db.PolicyTypes.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
    }
}
