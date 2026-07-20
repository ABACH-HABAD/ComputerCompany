using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class StorageService(IValidator<StorageModel> validator, IStorageRepository repository) :
BaseDataService<StorageModel, IStorageRepository>(validator, repository, "накопитель"),
IStorageService;