using Microsoft.AspNetCore.Mvc;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.WebApi.Controllers;

[Route("api/storage")]
[ApiController]
public class StorageController : BaseDataController<StorageModel, IStorageService>;