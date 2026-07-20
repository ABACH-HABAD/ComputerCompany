using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/frame")]
[ApiController]
public class FrameController : BaseDataController<FrameModel, IFrameService>;