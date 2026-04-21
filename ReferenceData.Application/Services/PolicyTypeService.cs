using ReferenceData.Application.Common;
using ReferenceData.Application.DTOs;
using ReferenceData.Application.Interfaces;
using ReferenceData.Domain;
using System.Xml.Linq;

namespace ReferenceData.Application.Services;

public class PolicyTypeService(IPolicyTypeRepository repo)
{
    public async Task<PagedResponse<PolicyTypeDto>> GetAllAsync(bool includeInactive, int page, int pageSize, CancellationToken ct)
    {
        bool? isActiveFilter = includeInactive ? null : true;

        var all = await repo.GetAllAsync(isActiveFilter, ct);
        var total = all.Count;

        var items = all.Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .Select(ToDto)
                       .ToList();

        return new PagedResponse<PolicyTypeDto>
        {
            Data = items,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }

    public async Task<ServiceResult<PolicyTypeDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        return entity is null
            ? ServiceResult<PolicyTypeDto>.Fail("Id", $"PolicyType with id {id} not found")
            : ServiceResult<PolicyTypeDto>.Ok(ToDto(entity));
    }

    public async Task<ServiceResult<PolicyTypeDto>> GetByCodeAsync(string code, CancellationToken ct)
    {
        var entity = await repo.GetByCodeAsync(code.ToUpper(), ct);
        return entity is null
            ? ServiceResult<PolicyTypeDto>.Fail("Code", $"PolicyType '{code}' not found")
            : ServiceResult<PolicyTypeDto>.Ok(ToDto(entity));
    }

    public async Task<ServiceResult<PolicyTypeDto>> CreateAsync(CreatePolicyTypeRequest request, CancellationToken ct)
    {
        var code = request.Code.Trim().ToUpper();

        if (string.IsNullOrWhiteSpace(code))
            return ServiceResult<PolicyTypeDto>.Fail("Code", "Code is required");

        if (await repo.ExistsAsync(code, ct))
            return ServiceResult<PolicyTypeDto>.Fail("Code", $"Code '{code}' already exists");

        var entity = new PolicyType
        {
            Code = code,
            Name = request.Name.Trim(),
            Description = request.Description ?? string.Empty,
            IsActive = true        };

        var created = await repo.CreateAsync(entity, ct);
        return ServiceResult<PolicyTypeDto>.Ok(ToDto(created));
    }

    public async Task<ServiceResult<PolicyTypeDto>> UpdateAsync(int id, UpdatePolicyTypeRequest request, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        if (entity is null)
            return ServiceResult<PolicyTypeDto>.Fail("Id", $"PolicyType with id {id} not found");

        entity.Name = request.Name.Trim();
        entity.IsActive = request.IsActive;
        entity.Description = request.Description ?? string.Empty;


        var updated = await repo.UpdateAsync(entity, ct);
        return ServiceResult<PolicyTypeDto>.Ok(ToDto(updated));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(id, ct);
        if (entity is null)
            return ServiceResult<bool>.Fail("Id", $"PolicyType with id {id} not found");

        entity.IsActive = false; // Soft Delete
        await repo.UpdateAsync(entity, ct);
        return ServiceResult<bool>.Ok(true);
    }

    private static PolicyTypeDto ToDto(PolicyType e)=> new PolicyTypeDto
    {
        Id = e.Id,
        Code = e.Code,
        Name = e.Name,
        Description = e.Description,
        IsActive = e.IsActive
    };

}