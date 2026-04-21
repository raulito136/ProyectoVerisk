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
    public class CoverageTypeService(ICoverageTypeRepository repo)
    {
        public async Task<PagedResponse<CoverageTypeDto>> GetAllAsync(bool includeInactive, int page, int pageSize, CancellationToken ct)
        {
            bool? isActiveFilter = includeInactive ? null : true;

            var all = await repo.GetAllAsync(isActiveFilter, ct);
            var total = all.Count;
            var items = all.Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .Select(ToDto)
                           .ToList();

            return new PagedResponse<CoverageTypeDto> { Data = items, Page = page, PageSize = pageSize, Total = total };
        }

        public async Task<ServiceResult<CoverageTypeDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            return entity is null
                ? ServiceResult<CoverageTypeDto>.Fail("Id", $"CoverageType with id {id} not found")
                : ServiceResult<CoverageTypeDto>.Ok(ToDto(entity));
        }

        public async Task<ServiceResult<CoverageTypeDto>> GetByCodeAsync(string code, CancellationToken ct)
        {
            var entity = await repo.GetByCodeAsync(code.ToUpper(), ct);
            return entity is null
                ? ServiceResult<CoverageTypeDto>.Fail("Code", $"CoverageType '{code}' not found")
                : ServiceResult<CoverageTypeDto>.Ok(ToDto(entity));
        }

        public async Task<ServiceResult<CoverageTypeDto>> CreateAsync(CreateCoverageTypeRequest request, CancellationToken ct)
        {
            var code = request.Code.Trim().ToUpper();
            if (await repo.ExistsAsync(code, ct))
                return ServiceResult<CoverageTypeDto>.Fail("Code", $"Code '{code}' already exists");

            var entity = new CoverageType { Code = code, Name = request.Name.Trim(), Description = request.Description ?? string.Empty, IsActive = true};
            var created = await repo.CreateAsync(entity, ct);
            return ServiceResult<CoverageTypeDto>.Ok(ToDto(created));
        }

        public async Task<ServiceResult<CoverageTypeDto>> UpdateAsync(int id, UpdateCoverageTypeRequest request, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null)
                return ServiceResult<CoverageTypeDto>.Fail("Id", $"CoverageType with id {id} not found");

            entity.Name = request.Name.Trim();
            entity.Description = request.Description ?? string.Empty;
            entity.IsActive = request.IsActive;

            return ServiceResult<CoverageTypeDto>.Ok(ToDto(await repo.UpdateAsync(entity, ct)));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return ServiceResult<bool>.Fail("Id", "No encontrado");

            entity.IsActive = false; // Soft Delete
            await repo.UpdateAsync(entity, ct);
            return ServiceResult<bool>.Ok(true);
        }

        private static CoverageTypeDto ToDto(CoverageType e) => new CoverageTypeDto
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            Description = e.Description,
            IsActive = e.IsActive
        };
    }
}
