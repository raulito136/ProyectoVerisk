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
    public class RegionRepository(ReferenceDataDbContext db) : IRegionRepository
    {
        public async Task<List<Region>> GetAllAsync(bool? isActive, CancellationToken ct) =>
            await db.Regions    
                .Where(x => !isActive.HasValue || x.IsActive == isActive)
                .OrderBy(x => x.Code)
                .ToListAsync(ct);

        public async Task<Region?> GetByIdAsync(int id, CancellationToken ct) =>
            await db.Regions.FindAsync([id], ct);

        public async Task<Region?> GetByCodeAsync(string code, CancellationToken ct) =>
            await db.Regions.FirstOrDefaultAsync(x => x.Code == code, ct);
        public async Task<bool> ExistsAsync(string code, CancellationToken ct) =>
            await db.Regions.AnyAsync(x => x.Code == code, ct);

        public async Task<Region> CreateAsync(Region entity, CancellationToken ct)
        {
            db.Regions.Add(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<Region> UpdateAsync(Region entity, CancellationToken ct)
        {
            db.Regions.Update(entity);
            await db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(Region entity, CancellationToken ct)
        {
            db.Regions.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
    }
}
