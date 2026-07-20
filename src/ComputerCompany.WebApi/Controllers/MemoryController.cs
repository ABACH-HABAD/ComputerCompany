using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/memory")]
[ApiController]
public class MemoryController : BaseDataController<MemoryModel, IMemoryService>;