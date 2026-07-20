using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/role")]
[ApiController]
public class RoleController : BaseDataController<RoleModel, IRoleService>
{
    [HttpGet("all")]
    public override async Task<IActionResult> GetAsync([FromServices] IRoleService service)
    {
        return await base.GetAsync(service);
    }

    [Authorize]
    [HttpPost]
    public override async Task<IActionResult> PostAsync([FromBody] DataRequest<RoleModel> request, [FromServices] IRoleService service)
    {
        return Forbid();
    }

    [Authorize]
    [HttpPut]
    public override async Task<IActionResult> PutAsync([FromBody] DataRequest<RoleModel> request, [FromServices] IRoleService service)
    {
        return Forbid();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public override async Task<IActionResult> DeleteAsync(Guid id, [FromServices] IRoleService service)
    {
        return Forbid();
    }
}