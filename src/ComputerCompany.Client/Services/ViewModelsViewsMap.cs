using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Dialogs;
using ComputerCompany.Application.Client.ViewModels.UserControls;
using ComputerCompany.Presentation.UserControls;
using ComputerCompany.Presentation.UserControls.WelcomeUserControls;
using ComputerCompany.Presentation.Windows;
using ComputerCompany.Presentation.Windows.Dialogs;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;

namespace ComputerCompany.Presentation.Services;

public class ViewModelsViewsMap : IViewModelsViewsMap
{
    private readonly Dictionary<Type, Type> _map = new()
    {
        //Windows
        {typeof(WelcomeViewModel), typeof(WelcomeWindow)},
        {typeof(MainViewModel), typeof(MainWindow)},

        //Dialogs
        {typeof(EditAccountViewModel), typeof(AccountEditDialogWindow)},
        {typeof(CardPayViewModel), typeof(CardPay)},
        {typeof(ChangeNameViewModel), typeof(ChangeDisplayName)},
        {typeof(ChangeLoginViewModel), typeof(ChangeDisplayName)},
        {typeof(CreateAccountViewModel), typeof(CreateAccountWindow)},
        {typeof(ComponentDataBaseDialogViewModel<CpuModel>), typeof(DataBaseAddDataWindow)},
        {typeof(ComponentDataBaseDialogViewModel<GpuModel>), typeof(DataBaseAddDataWindow)},
        {typeof(ComponentDataBaseDialogViewModel<MotherboardModel>), typeof(DataBaseAddDataWindow)},
        {typeof(ComponentDataBaseDialogViewModel<MemoryModel>), typeof(DataBaseAddDataWindow)},
        {typeof(ComponentDataBaseDialogViewModel<StorageModel>), typeof(DataBaseAddDataWindow)},
        {typeof(ComponentDataBaseDialogViewModel<PowerUnitModel>), typeof(DataBaseAddDataWindow)},
        {typeof(ComponentDataBaseDialogViewModel<FrameModel>), typeof(DataBaseAddDataWindow)},
        {typeof(DeleteAccountViewModel), typeof(DeleteAccount)},
        {typeof(InformationViewModel), typeof(InformationWindow)},
        {typeof(PasswordResetViewModel), typeof(PasswordResetDialog)},
        {typeof(QrViewModel), typeof(QRPay)},

        //UserControls
        {typeof(AuthorizationViewModel), typeof(AuthorizationUserControl)},
        {typeof(RegistrationViewModel), typeof(RegistrationUserControl)},
        {typeof(AdminDataBaseViewModel), typeof(AdminDataBaseUserControl)},
        {typeof(BuildViewModel), typeof(ClientBuildUserControl)},
        {typeof(ComponentsViewModel), typeof(ClientComponentUserControl)},
        {typeof(HomeViewModel), typeof(ClientMainUserControl)},
        {typeof(ShoppingCartViewModel), typeof(ClientShoppingCartUserControl)},
        {typeof(UpgradeViewModel), typeof(ClientUpgradeUserControl)},
        {typeof(ManagerDataBaseViewModel), typeof(ManagerDataBaseUserControl)},
        {typeof(ProfileViewModel), typeof(Profile)},
    };

    public Type GetViewType(Type viewModelType)
    {
        Type foundType;
        try
        {
            foundType = _map[viewModelType];
        }
        catch (Exception ex)
        {
            throw new Exception("Подходящий тип не найден", innerException: ex);
        }

        if (foundType == null) throw new Exception("Подходящий тип не найден");
        return foundType;
    }

    public Type GetViewType<T>() where T : IViewModel
    {
        return GetViewType(typeof(T));
    }
}