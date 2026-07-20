using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/motherboard")]
[ApiController]
public class MotherboardController : BaseDataController<MotherboardModel, IMotherboardService>;