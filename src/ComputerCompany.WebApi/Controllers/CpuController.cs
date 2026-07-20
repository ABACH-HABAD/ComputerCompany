using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/cpu")]
[ApiController]
public class CpuController : BaseDataController<CpuModel, ICpuService>;