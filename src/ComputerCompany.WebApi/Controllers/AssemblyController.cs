using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/assembly")]
[ApiController]
public class AssemblyController : BaseDataController<AssemblyModel, IAssemblyService>
{
    [Authorize]
    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(Guid id, [FromServices] IAssemblyService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        DataResult<AssemblyModel> result = await service.GetByIdAsync(id);

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && result.IsSuccess && result.Data.Account.Id != currentUserId) return Forbid();

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet("all")]
    public override async Task<IActionResult> GetAsync([FromServices] IAssemblyService service)
    {
        return await base.GetAsync(service);
    }

    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> PostAsync([FromBody] DataRequest<AssemblyModel> request, [FromServices] IAssemblyService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (request.Data.Account.Id == Guid.Empty) request = request with { Data = request.Data with { Account = request.Data.Account with { Id = currentUserId } } };
        if (!isAdmin && currentUserId != request.Data.Account.Id) return Forbid();


        return await base.PostAsync(request, service);
    }

    [Authorize]
    [HttpPut]
    public override async Task<IActionResult> PutAsync([FromBody] DataRequest<AssemblyModel> request, [FromServices] IAssemblyService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Account.Id) return Forbid();

        return await base.PutAsync(request, service);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public override async Task<IActionResult> DeleteAsync(Guid id, [FromServices] IAssemblyService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        DataResult<AssemblyModel> result = await service.GetByIdAsync(id);

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && result.IsSuccess && result.Data.Account.Id != currentUserId) return Forbid();

        return await base.DeleteAsync(id, service);
    }
}