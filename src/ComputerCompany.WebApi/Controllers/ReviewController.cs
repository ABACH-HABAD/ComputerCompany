using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController : BaseDataController<ReviewModel, IReviewService>
{
    [HttpDelete("{id}")]
    [AllowAnonymous]
    public override async Task<IActionResult> DeleteAsync(Guid id, [FromServices] IReviewService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();

        DataResult<ReviewModel> foundedReview = await service.GetByIdAsync(id);
        if (!foundedReview.IsSuccess) return NotFound();

        if (!isAdmin && currentUserId != foundedReview.Data.Sender.Id) return Forbid();

        Result result = await service.DeleteAsync(id);
        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public override async Task<IActionResult> PostAsync([FromBody] DataRequest<ReviewModel> request, [FromServices] IReviewService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Sender.Id) return Forbid();

        return await base.PostAsync(request, service);
    }

    [HttpPut]
    [AllowAnonymous]
    public override async Task<IActionResult> PutAsync([FromBody] DataRequest<ReviewModel> request, [FromServices] IReviewService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Sender.Id) return Forbid();

        return await base.PutAsync(request, service);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetByAccountAsync([FromQuery] Guid accountId, [FromServices] IReviewService service)
    {
        DataResult<ReviewModel> result = await service.GetByAccountAsync(accountId);

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> SendReviewPostAsync([FromBody] DataRequest<ReviewModel> request, [FromServices] IReviewService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Sender.Id) return Forbid();

        Result result = await service.SendReviewAsync(request.Data);

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize]
    [HttpPut("send")]
    public async Task<IActionResult> SendReviewPutAsync([FromBody] DataRequest<ReviewModel> request, [FromServices] IReviewService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Sender.Id) return Forbid();

        Result result = await service.SendReviewAsync(request.Data);

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomAsync([FromQuery] int? range, [FromServices] IReviewService service)
    {
        if (range == null)
        {
            DataResult<ReviewModel> result = await service.GetRandomAsync();

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }
        else
        {
            DataResult<List<ReviewModel>> result = await service.GetRandomRangeAsync((int)range);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result);
        }
    }
}