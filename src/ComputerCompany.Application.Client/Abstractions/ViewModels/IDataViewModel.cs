using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Abstractions.ViewModels;

public interface IDataViewModel<T> where T : BaseModel
{
    public T Data { get; }

    public void SetData(T data);
}