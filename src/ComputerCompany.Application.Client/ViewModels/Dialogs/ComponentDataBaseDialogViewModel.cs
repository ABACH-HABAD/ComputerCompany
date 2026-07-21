using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Abstractions.ViewModels;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class ComponentDataBaseDialogViewModel<T> : BaseDataDialogViewModel<T> where T : BaseComponentModel
{
    private readonly IScopeFactory _scopeFactory;

    private string _mode = string.Empty;

    private Guid _id = default;

    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _model = string.Empty;
    private double _price;
    private int _count;

    private string _coreModel = string.Empty;
    private string _socket = string.Empty;
    private string _chipset = string.Empty;
    private string _formFactor = string.Empty;
    private string _type = string.Empty;
    private string _certification = string.Empty;
    private int _size;
    private int _power;
    private int _frequency;
    private int _videoMemoty;

    public string Mode
    {
        get => _mode;
        private set
        {
            ChangeProperty(ref _mode, value);
        }
    }

    public string Name { get => _name; set => ChangeProperty(ref _name, value); }
    public string Description { get => _description; set => ChangeProperty(ref _description, value); }
    public string Model { get => _model; set => ChangeProperty(ref _model, value); }
    public double Price { get => _price; set => ChangeProperty(ref _price, value); }
    public int Count { get => _count; set => ChangeProperty(ref _count, value); }

    public string ModelCore { get => _coreModel; set => ChangeProperty(ref _coreModel, value); }
    public string Socket { get => _socket; set => ChangeProperty(ref _socket, value); }
    public string Chipset { get => _chipset; set => ChangeProperty(ref _chipset, value); }
    public string FormFactor { get => _formFactor; set => ChangeProperty(ref _formFactor, value); }
    public string Type { get => _type; set => ChangeProperty(ref _type, value); }
    public string Certification { get => _certification; set => ChangeProperty(ref _certification, value); }
    public int Size { get => _size; set => ChangeProperty(ref _size, value); }
    public int Power { get => _power; set => ChangeProperty(ref _power, value); }
    public int Frequency { get => _frequency; set => ChangeProperty(ref _frequency, value); }
    public int VideoMemory { get => _videoMemoty; set => ChangeProperty(ref _videoMemoty, value); }

    public override T Data
    {
        get
        {
            if (typeof(T) == typeof(CpuModel))
            {
                CpuModel component = new(_id, Name, Description, Price, Count, Model, Socket);
                _data = (component as T)!;
            }
            else if (typeof(T) == typeof(GpuModel))
            {
                GpuModel component = new(_id, Name, Description, Price, Count, Model, ModelCore, VideoMemory);
                _data = (component as T)!;
            }
            else if (typeof(T) == typeof(MotherboardModel))
            {
                MotherboardModel component = new(_id, Name, Description, Price, Count, Model, Chipset, Socket);
                _data = (component as T)!;
            }
            else if (typeof(T) == typeof(MemoryModel))
            {
                MemoryModel component = new(_id, Name, Description, Price, Count, Model, Type, Size, Frequency);
                _data = (component as T)!;
            }
            else if (typeof(T) == typeof(StorageModel))
            {
                StorageModel component = new(_id, Name, Description, Price, Count, Model, FormFactor, Type, Size);
                _data = (component as T)!;
            }
            else if (typeof(T) == typeof(PowerUnitModel))
            {
                PowerUnitModel component = new(_id, Name, Description, Price, Count, Model, Certification, FormFactor, Power);
                _data = (component as T)!;
            }
            else if (typeof(T) == typeof(FrameModel))
            {
                FrameModel component = new(_id, Name, Description, Price, Count, Model, FormFactor);
                _data = (component as T)!;
            }
            else throw new Exception($"Несуществующий тип компонента: {typeof(T)}");
            return _data;
        }
        protected set => _data = value;
    }

    public ICommand DeleteCommand { get; }

    public ComponentDataBaseDialogViewModel(IScopeFactory scopeFactory)
    {
        Mode = typeof(T).ToString();

        _scopeFactory = scopeFactory;

        AcceptCommand = new RelayCommand(Accept);
        DenyCommand = new RelayCommand(Cancel);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
    }

    public override void SetData(T data)
    {
        Data = data;

        _id = data.Id;

        Name = data.Name;
        Description = data.Description;
        Price = data.Price;
        Count = data.Count;
        Model = data.Model;

        if (_data is CpuModel cpu)
        {
            Socket = cpu.Socket;
        }
        else if (_data is GpuModel gpu)
        {
            ModelCore = gpu.ModelCore;
            VideoMemory = gpu.VideoMemory;
        }
        else if (_data is MotherboardModel mb)
        {
            Socket = mb.Socket;
            Chipset = mb.Chipset;
        }
        else if (_data is MemoryModel memory)
        {
            Frequency = memory.Frequency;
            Size = memory.Size;
            Type = memory.Type;
        }
        else if (_data is StorageModel storage)
        {
            FormFactor = storage.FormFactor;
            Size = storage.Size;
            Type = storage.Type;
        }
        else if (_data is PowerUnitModel powerunit)
        {
            FormFactor = powerunit.FormFactor;
            Certification = powerunit.Certification;
            Power = powerunit.Power;
        }
        else if (_data is FrameModel frame)
        {
            FormFactor = frame.FormFactor;
        }
    }

    private static bool CheckField(IMessageService messageService, in string field, in string fieldName)
    {
        if (field == null || field == string.Empty)
        {
            messageService.ShowErrorMessage("Введите " + fieldName);
            return false;
        }
        else return true;
    }

    private static bool CheckField(IMessageService messageService, in int field, in string fieldName)
    {
        if (field <= 0)
        {
            messageService.ShowErrorMessage("Введите " + fieldName);
            return false;
        }
        else return true;
    }

    private static bool CheckField(IMessageService messageService, in double field, in string fieldName)
    {
        if (field <= 0)
        {
            messageService.ShowErrorMessage("Введите " + fieldName);
            return false;
        }
        else return true;
    }

    private void Accept()
    {
        using IServiceScope serviceScope = _scopeFactory.CreateScope();
        IMessageService messageService = serviceScope.ServiceProvider.GetRequiredService<IMessageService>();

        if (!CheckField(messageService, Name, "название")) return;
        if (!CheckField(messageService, Description, "описание")) return;
        if (!CheckField(messageService, Model, "модель")) return;
        if (!CheckField(messageService, Price, "цену")) return;
        if (!CheckField(messageService, Count, "количество")) return;

        if (_data is CpuModel)
        {
            if (!CheckField(messageService, Socket, "сокет")) return;
        }
        else if (_data is GpuModel)
        {
            if (!CheckField(messageService, ModelCore, "видеоядро")) return;
            if (!CheckField(messageService, VideoMemory, "видеопамять")) return;
        }
        else if (_data is MotherboardModel)
        {
            if (!CheckField(messageService, Socket, "сокет")) return;
            if (!CheckField(messageService, Chipset, "чипсет")) return;
        }
        else if (_data is MemoryModel)
        {
            if (!CheckField(messageService, Frequency, "частота")) return;
            if (!CheckField(messageService, Size, "объём")) return;
            if (!CheckField(messageService, Type, "тип")) return;
        }
        else if (_data is StorageModel)
        {
            if (!CheckField(messageService, Size, "объём")) return;
            if (!CheckField(messageService, Type, "тип")) return;
            if (!CheckField(messageService, FormFactor, "формфактор")) return; ;
        }
        else if (_data is PowerUnitModel)
        {
            if (!CheckField(messageService, Power, "мощность")) return;
            if (!CheckField(messageService, Certification, "сертификат")) return;
            if (!CheckField(messageService, FormFactor, "формфактор")) return;
        }
        else if (_data is FrameModel)
        {
            if (!CheckField(messageService, FormFactor, "форм фактор")) return;
        }

        DialogResult = true;
    }

    private void Cancel()
    {
        DialogResult = false;
    }

    private async Task DeleteAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        Result dialogResult = await dialogService.ShowDeleteDialogAsync($"Удаление {Name}", $"Вы уверены что хотите удалить {Name} {Model}?");
        if (dialogResult.IsSuccess)
        {
            IDataService<T> service;
            if (_data is CpuModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<ICpuService>();
            else if (_data is GpuModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<IGpuService>();
            else if (_data is MotherboardModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<IMotherboardService>();
            else if (_data is MemoryModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<IMemoryService>();
            else if (_data is StorageModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<IStorageService>();
            else if (_data is PowerUnitModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<IPowerUnitService>();
            else if (_data is FrameModel) service = (IDataService<T>)scope.ServiceProvider.GetRequiredService<IFrameService>();
            else throw new Exception("Не найден подходящий сервис для удаления");

            Result serviceResult = await service.DeleteAsync(_id);
            if (!serviceResult.IsSuccess)
            {
                IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                messageService.ShowErrorMessage(serviceResult.Message);
                return;
            }

            //Возвращаем false так как мы удалили компонент, а этот диалог был запросом на изменение
            //Если вернуть true то произойдёт попытка изменения удалённого компонента
            //А значит сервер вернёт ошибку
            DialogResult = false;
        }
    }

    private bool CanDelete() => _id != default;
}