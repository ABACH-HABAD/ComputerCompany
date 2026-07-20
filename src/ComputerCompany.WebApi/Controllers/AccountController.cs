using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : BaseDataController<AccountModel, IAccountService>
{
    [Authorize]
    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(Guid id, [FromServices] IAccountService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (id == Guid.Empty) id = currentUserId;
        if (!isAdmin && currentUserId != id) return Forbid();

        return await base.GetAsync(id, service);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public override async Task<IActionResult> GetAsync([FromServices] IAccountService service)
    {
        return await base.GetAsync(service);
    }

    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> PostAsync([FromBody] DataRequest<AccountModel> request, [FromServices] IAccountService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Id) return Forbid();

        return await base.PostAsync(request, service);
    }

    [AllowAnonymous]
    [HttpPatch]
    public async Task<IActionResult> PatchAsync([FromBody] DataRequest<AccountModel> request, [FromServices] IAccountService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Id) return Forbid();

        return await base.PostAsync(request, service);
    }

    [AllowAnonymous]
    [HttpPut]
    public override async Task<IActionResult> PutAsync([FromBody] DataRequest<AccountModel> request, [FromServices] IAccountService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != request.Data.Id) return Forbid();

        return await base.PutAsync(request, service);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public override async Task<IActionResult> DeleteAsync(Guid id, [FromServices] IAccountService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (id == Guid.Empty) id = currentUserId;
        if (!isAdmin && currentUserId != id) return Forbid();

        return await base.DeleteAsync(id, service);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync
    (
        [FromBody] LoginRequest request,
        [FromServices] IAccountService service
    )
    {
        IPAddress? ip = HttpContext.Connection.RemoteIpAddress;
        LoginResult result = await service.LoginAsync(request.Login, request.HashedPassword, ip?.ToString());

        if (result.IsSuccess) return Ok(result);
        else return Unauthorized(result);
    }


    [HttpPost("registrate")]
    public async Task<IActionResult> RegistrateAsync
    (
        [FromBody] RegistrationRequest request,
        [FromServices] IAccountService service
    )
    {
        IPAddress? ip = HttpContext.Connection.RemoteIpAddress;
        LoginResult result = await service.RegistrateAsync(request.Login, request.HashedPassword, request.RepeatHasedPassword, ip?.ToString(), request.SaveLoginData);

        if (result.IsSuccess) return Ok(result);
        else return Unauthorized(result);
    }

    [Authorize]
    [HttpPatch("resetPassword")]
    public async Task<IActionResult> ResetPasswordAsync
    (
        [FromBody] ResetPasswordRequest request,
        [FromServices] IAccountService service
    )
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (request.Id == Guid.Empty) request = request with { Id = currentUserId };
        if (!isAdmin && currentUserId != request.Id) return Forbid();

        Result result = await service.ResetPasswordAsync(currentUserId, request.OldHashedPassword, request.NewHashedPassword, forceChange: isAdmin && request.ForceMode);

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }
}