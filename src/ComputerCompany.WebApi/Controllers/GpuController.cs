using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/gpu")]
[ApiController]
public class GpuController : BaseDataController<GpuModel, IGpuService>;