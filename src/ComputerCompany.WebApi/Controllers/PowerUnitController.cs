using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/powerunit")]
[ApiController]
public class PowerUnitController : BaseDataController<PowerUnitModel, IPowerUnitService>;