using ComputerCompany.Core.Models;

namespace ComputerCompany.WebApi.Requests;

public record DataRequest<T>(T Data) : BaseRequest() where T : BaseModel;