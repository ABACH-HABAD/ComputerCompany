using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Dialog;

public interface IResultMessageService
{
    public void ShowResultMessage(Result result);
}