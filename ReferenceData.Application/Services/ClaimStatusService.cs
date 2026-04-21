using ReferenceData.Application.Common;
using ReferenceData.Application.DTOs;
using ReferenceData.Application.Interfaces;
using ReferenceData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Application.Services
{
    public class ClaimStatusService(IClaimStatusRepository repo)
    {
        public async Task<PagedResponse<ClaimStatusDto>> GetAllAsync(bool includeInactive, int page, int pageSize, CancellationToken ct)
        {
            bool? isActiveFilter = includeInactive ? null : true;

            var all = await repo.GetAllAsync(isActiveFilter, ct);
            var total = all.Count;
            var items = all.Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .Select(ToDto)
                           .ToList();

            return new PagedResponse<ClaimStatusDto> { Data = items, Page = page, PageSize = pageSize, Total = total };
        }

        public async Task<ServiceResult<ClaimStatusDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity is null
                ? ServiceResult<ClaimStatusDto>.Fail("Id", $"ClaimStatus with id {id} not found")
                : ServiceResult<ClaimStatusDto>.Ok(ToDto(entity));
        }

        public async Task<ServiceResult<ClaimStatusDto>> GetByCodeAsync(string code, CancellationToken ct)
        {
            var entity = await repo.GetByCodeAsync(code.ToUpper(), ct);
            return entity is null
                ? ServiceResult<ClaimStatusDto>.Fail("Code", $"ClaimStatus '{code}' not found")
                : ServiceResult<ClaimStatusDto>.Ok(ToDto(entity));
        }

        public async Task<ServiceResult<ClaimStatusDto>> CreateAsync(CreateClaimStatusRequest request, CancellationToken ct)
        {
            var code = request.Code.Trim().ToUpper();
            if (await repo.ExistsAsync(code, ct))
                return ServiceResult<ClaimStatusDto>.Fail("Code", $"Code '{code}' already exists");

            var entity = new ClaimStatus
            {
                Code = code,
                Name = request.Name.Trim(),
                Description = request.Description ?? string.Empty,
                IsActive = true
            };

            return ServiceResult<ClaimStatusDto>.Ok(ToDto(await repo.CreateAsync(entity, ct)));
        }

        public async Task<ServiceResult<ClaimStatusDto>> UpdateAsync(int id, UpdateClaimStatusRequest request, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return ServiceResult<ClaimStatusDto>.Fail("Id", "Not found");

            entity.Name = request.Name.Trim();
            entity.Description = request.Description ?? string.Empty;
            entity.IsActive = request.IsActive;

            return ServiceResult<ClaimStatusDto>.Ok(ToDto(await repo.UpdateAsync(entity, ct)));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return ServiceResult<bool>.Fail("Id", "Not found");

            entity.IsActive = false;
            await repo.UpdateAsync(entity, ct);
            return ServiceResult<bool>.Ok(true);
        }

        private static ClaimStatusDto ToDto(ClaimStatus e) => new ClaimStatusDto
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            Description = e.Description,
            IsActive = e.IsActive
        };
    }
}
