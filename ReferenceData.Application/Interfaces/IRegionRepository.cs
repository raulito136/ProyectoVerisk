using ReferenceData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Application.Interfaces
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync(bool? isActive, CancellationToken ct);
        Task<Region?> GetByIdAsync(int id, CancellationToken ct);
        Task<Region?> GetByCodeAsync(string code, CancellationToken ct);
        Task<Region> CreateAsync(Region entity, CancellationToken ct);
        Task<Region> UpdateAsync(Region entity, CancellationToken ct);
        Task DeleteAsync(Region entity, CancellationToken ct);
        Task<bool> ExistsAsync(string code, CancellationToken ct);
    }
}
