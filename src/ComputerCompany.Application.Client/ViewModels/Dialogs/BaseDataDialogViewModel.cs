using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public abstract class BaseDataDialogViewModel<T> : BaseDialogViewModel, IDataViewModel<T> where T : BaseModel
{
    protected T _data = null!;
    public override object? ResultData
    {
        get => Data;
        protected set
        {
            if (value is T typeValue) Data = typeValue;
        }
    }

    public virtual T Data
    {
        get => _data;
        protected set => ChangeProperty(ref _data, value);
    }

    public virtual void SetData(T data)
    {
        _data = data;
    }
}