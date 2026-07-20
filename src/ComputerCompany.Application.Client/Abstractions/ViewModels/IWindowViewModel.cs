namespace ComputerCompany.Application.Client.Abstractions.ViewModels;

public interface IWindowViewModel
{
    public event EventHandler<EventArgs> Closed;

    public void Close();
}