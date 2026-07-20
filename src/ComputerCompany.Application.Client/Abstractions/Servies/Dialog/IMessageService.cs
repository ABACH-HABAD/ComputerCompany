namespace ComputerCompany.Application.Client.Abstractions.Servies.Dialog;

public interface IMessageService
{
    public void ShowInformationMessage(string message, string header = "Внимание");
    public void ShowErrorMessage(string message, string header = "Ошибка");
    public bool ShowQuestionMessage(string message, string header = "?");
}