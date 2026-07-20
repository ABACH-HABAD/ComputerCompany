namespace ComputerCompany.Application.Client.Abstractions.ViewModels;

public interface IDataRequireableViewModel<T>
{
    public void LoadData(T data);
}

public interface IDataRequireableViewModel
{
    public void LoadData(params object[] data);
}