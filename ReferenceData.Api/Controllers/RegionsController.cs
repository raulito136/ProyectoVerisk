using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferenceData.Application.DTOs;
using ReferenceData.Application.Services;
using ReferenceData.Application.Common;

namespace ReferenceData.Api.Controllers
{
    [ApiController]
    [Route("api/v1/regions")]
    public class RegionsController(RegionService service) : ControllerBase
    {
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] bool includeInactive = false,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    CancellationToken ct = default)
        {
            var result = await service.GetAllAsync(includeInactive, page, pageSize, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await service.GetByIdAsync(id, ct);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<RegionDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return Ok(ApiResponse<RegionDto>.Success(result.Value!));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRegionRequest request, CancellationToken ct)
        {
            var result = await service.CreateAsync(request, ct);
            if (!result.IsSuccess)
                return Conflict(ApiResponse<RegionDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },
                ApiResponse<RegionDto>.Success(result.Value));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRegionRequest request, CancellationToken ct)
        {
            var result = await service.UpdateAsync(id, request, ct);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<RegionDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return Ok(ApiResponse<RegionDto>.Success(result.Value!));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await service.DeleteAsync(id, ct);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<bool>.Fail(result.ErrorField!, result.ErrorMessage!));
            return NoContent();
        }
    }
}
