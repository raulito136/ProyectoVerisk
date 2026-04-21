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
    public class RegionService(IRegionRepository repo)
    {
        public async Task<PagedResponse<RegionDto>> GetAllAsync(bool includeInactive, int page, int pageSize, CancellationToken ct)
        {
            bool? isActiveFilter = includeInactive ? null : true;

            var all = await repo.GetAllAsync(isActiveFilter, ct);
            var total = all.Count;

            var items = all.Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .Select(ToDto)
                           .ToList();

            return new PagedResponse<RegionDto>
            {
                Data = items,
                Page = page,
                PageSize = pageSize,
                Total = total
            };
        }

        public async Task<ServiceResult<RegionDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity is null
                ? ServiceResult<RegionDto>.Fail("Id", $"Region with id {id} not found")
                : ServiceResult<RegionDto>.Ok(ToDto(entity));
        }

        public async Task<ServiceResult<RegionDto>> GetByCodeAsync(string code, CancellationToken ct)
        {
            var entity = await repo.GetByCodeAsync(code.ToUpper(), ct);
            return entity is null
                ? ServiceResult<RegionDto>.Fail("Code", $"Region '{code}' not found")
                : ServiceResult<RegionDto>.Ok(ToDto(entity));
        }

        public async Task<ServiceResult<RegionDto>> CreateAsync(CreateRegionRequest request, CancellationToken ct)
        {
            var code = request.Code.Trim().ToUpper();
            if (await repo.ExistsAsync(code, ct))
                return ServiceResult<RegionDto>.Fail("Code", $"Code '{code}' already exists");

            var entity = new Region { Code = code, Name = request.Name.Trim(), IsActive = true };
            return ServiceResult<RegionDto>.Ok(ToDto(await repo.CreateAsync(entity, ct)));
        }

        public async Task<ServiceResult<RegionDto>> UpdateAsync(int id, UpdateRegionRequest request, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null)
                return ServiceResult<RegionDto>.Fail("Id", $"Region with id {id} not found");

            entity.Name = request.Name.Trim();
            entity.IsActive = request.IsActive;

            return ServiceResult<RegionDto>.Ok(ToDto(await repo.UpdateAsync(entity, ct)));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return ServiceResult<bool>.Fail("Id", "No encontrado");

            entity.IsActive = false;
            await repo.UpdateAsync(entity, ct);
            return ServiceResult<bool>.Ok(true);
        }

        private static RegionDto ToDto(Region e) => new RegionDto
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            IsActive = e.IsActive
        };
    }
}
