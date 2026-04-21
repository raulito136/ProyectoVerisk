using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferenceData.Domain;

namespace ReferenceData.Application.Interfaces
{
    public interface IPolicyTypeRepository
    {
        Task<List<PolicyType>> GetAllAsync(bool? isActive, CancellationToken ct);
        Task<PolicyType?> GetByIdAsync(int id, CancellationToken ct);
        Task<PolicyType?> GetByCodeAsync(string code, CancellationToken ct);
        Task<PolicyType> CreateAsync(PolicyType entity, CancellationToken ct);
        Task<PolicyType> UpdateAsync(PolicyType entity, CancellationToken ct);
        Task DeleteAsync(PolicyType entity, CancellationToken ct);
        Task<bool> ExistsAsync(string code, CancellationToken ct);
    }
}
