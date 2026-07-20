namespace ComputerCompany.Application.Results.Json;

internal record DataResultJson<T>(bool IsSuccess, string Message, T Data) : ResultJson(IsSuccess, Message);