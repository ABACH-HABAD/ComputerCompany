using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Core.Models;
using ComputerCompany.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/session")]
[ApiController]
public class SessionContoller : BaseDataController<SessionModel, ISessionService>
{
    [HttpPut("refresh")]
    public async Task<IActionResult> RefreshTokensAsync([FromBody] TokenRefreshRequest request, [FromServices] ISessionService service)
    {
        IPAddress? ip = HttpContext.Connection.RemoteIpAddress;

        LoginResult result = await service.RefreshTokensAsync(request.RefreshToken, ip?.ToString());

        if (result.IsSuccess) return Ok(result);
        else return Unauthorized(result);
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> GetByAccountAsync([FromQuery] Guid accountId, [FromServices] ISessionService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (!isAdmin && currentUserId != accountId) return Forbid();

        IPAddress? ip = HttpContext.Connection.RemoteIpAddress;

        DataResult<SessionModel> result = await service.GetByAccountAsync(accountId, ip?.ToString());

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize]
    [HttpDelete("logout/{accountId}")]
    public async Task<IActionResult> LogoutAsync(Guid accountId, [FromServices] ISessionService service)
    {
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool isAdmin = User.IsInRole("Admin");

        if (!Guid.TryParse(currentUserIdString, out Guid currentUserId)) return Unauthorized();
        if (accountId == Guid.Empty) accountId = currentUserId;
        if (!isAdmin && currentUserId != accountId) return Forbid();

        IPAddress? ip = HttpContext.Connection.RemoteIpAddress;

        Result result = await service.LogoutAsync(accountId, ip?.ToString());

        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }
}