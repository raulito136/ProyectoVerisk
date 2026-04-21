using ReferenceData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Application.Interfaces
{
    public interface IClaimStatusRepository
    {
        Task<List<ClaimStatus>> GetAllAsync(bool? isActive, CancellationToken ct);
        Task<ClaimStatus?> GetByIdAsync(int id, CancellationToken ct);
        Task<ClaimStatus?> GetByCodeAsync(string code, CancellationToken ct);
        Task<ClaimStatus> CreateAsync(ClaimStatus entity, CancellationToken ct);
        Task<ClaimStatus> UpdateAsync(ClaimStatus entity, CancellationToken ct);
        Task DeleteAsync(ClaimStatus entity, CancellationToken ct);
        Task<bool> ExistsAsync(string code, CancellationToken ct);
    }
}
