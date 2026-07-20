using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Services.Api.Requests;

public record DataRequest<T>(T Data) : BaseRequest() where T : BaseModel;