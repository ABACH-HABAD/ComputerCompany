using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerCompany.WebApi.Controllers;

[ApiController]
public abstract class BaseDataController<TModel, TService> : ControllerBase where TModel : BaseModel where TService : IDataService<TModel>
{

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(Guid id, [FromServices] TService service)
    {
        DataResult<TModel> result = await service.GetByIdAsync(id);
        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [HttpGet("all")]
    public virtual async Task<IActionResult> GetAsync([FromServices] TService service)
    {
        DataResult<List<TModel>> result = await service.GetAllAsync();
        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public virtual async Task<IActionResult> PostAsync([FromBody] DataRequest<TModel> request, [FromServices] TService service)
    {
        Result result = await service.AddAsync(request.Data);
        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPut]
    public virtual async Task<IActionResult> PutAsync([FromBody] DataRequest<TModel> request, [FromServices] TService service)
    {
        Result result = await service.UpdateAsync(request.Data);
        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id, [FromServices] TService service)
    {
        Result result = await service.DeleteAsync(id);
        if (result.IsSuccess) return Ok(result);
        else return BadRequest(result);
    }
}