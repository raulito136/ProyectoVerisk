using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferenceData.Domain;

namespace ReferenceData.Application.Interfaces
{
    public interface ICoverageTypeRepository
    {
        Task<List<CoverageType>> GetAllAsync(bool? isActive, CancellationToken ct);
        Task<CoverageType?> GetByIdAsync(int id, CancellationToken ct);
        Task<CoverageType?> GetByCodeAsync(string code, CancellationToken ct);
        Task<CoverageType> CreateAsync(CoverageType entity, CancellationToken ct);
        Task<CoverageType> UpdateAsync(CoverageType entity, CancellationToken ct);
        Task DeleteAsync(CoverageType entity, CancellationToken ct);
        Task<bool> ExistsAsync(string code, CancellationToken ct);
    }
}
