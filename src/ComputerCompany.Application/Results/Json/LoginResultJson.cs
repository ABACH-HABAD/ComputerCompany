namespace ComputerCompany.Application.Results.Json;

internal record LoginResultJson(bool IsSuccess, string Message, string Accept, string Refresh) : ResultJson(IsSuccess, Message);