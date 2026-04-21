using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferenceData.Application.DTOs;
using ReferenceData.Application.Services;
using ReferenceData.Application.Common;

namespace ReferenceData.Api.Controllers
{
    [ApiController]
    [Route("api/v1/claim-statuses")]
    public class ClaimStatusesController(ClaimStatusService service) : ControllerBase
    {
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
                return NotFound(ApiResponse<ClaimStatusDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return Ok(ApiResponse<ClaimStatusDto>.Success(result.Value!));
        }

        [HttpGet("by-code/{code}")]
        public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
        {
            var result = await service.GetByCodeAsync(code, ct);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<ClaimStatusDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return Ok(ApiResponse<ClaimStatusDto>.Success(result.Value!));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClaimStatusRequest request, CancellationToken ct)
        {
            var result = await service.CreateAsync(request, ct);
            if (!result.IsSuccess)
                return Conflict(ApiResponse<ClaimStatusDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },
                ApiResponse<ClaimStatusDto>.Success(result.Value));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClaimStatusRequest request, CancellationToken ct)
        {
            var result = await service.UpdateAsync(id, request, ct);
            if (!result.IsSuccess)
                return NotFound(ApiResponse<ClaimStatusDto>.Fail(result.ErrorField!, result.ErrorMessage!));
            return Ok(ApiResponse<ClaimStatusDto>.Success(result.Value!));
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

