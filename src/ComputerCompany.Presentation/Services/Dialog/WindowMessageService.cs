using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using System.Windows;

namespace ComputerCompany.Presentation.Services.Dialog;

public class WindowMessageService : IMessageService
{
    public void ShowErrorMessage(string message, string header = "Ошибка")
    {
        MessageBox.Show
        (
            message,
            header,
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }

    public void ShowInformationMessage(string message, string header = "Внимание")
    {
        MessageBox.Show
        (
            message,
            header,
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    public bool ShowQuestionMessage(string message, string header = "?")
    {
        MessageBoxResult result = MessageBox.Show
        (
            message,
            header,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );
        
        return (result == MessageBoxResult.Yes && result != MessageBoxResult.No);
    }
}