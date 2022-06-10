using System;
using System.Linq;
using System.Reactive;
using System.Security.Authentication;
using System.Threading.Tasks;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using NodaTime.Extensions;
using ReactiveUI;
namespace MetalAvaloniaReactive.ViewModels;

public class AddRoleViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private int _idRole;
    private string _roleName;
    private Role _role;
    private string _forRoleName;
    public AddRoleViewModel(MainWindowViewModel mainWindowViewModel, int idRole)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _idRole = idRole;
        var addEnabled = this.WhenAnyValue(
            x => x.RoleName,
            (x) => !string.IsNullOrWhiteSpace(x));
        _mainWindowViewModel = mainWindowViewModel;
        CancelButtonClick = ReactiveCommand.Create(CancellationOperation);
        
        if (_idRole != -1)
        {
            Role = RoleImplementation.GetRoleById(_idRole).Result;
            RoleName = Role.RoleName;
            _forRoleName = RoleName;
            ActionForSubmitButton = ReactiveCommand.CreateFromTask( () =>
            {
                if (ValidatingData())
                {
                    UpdateRole();
                }
                return Task.CompletedTask;
            });
            ContentForSubmitButton = "Изменить";
            TitleContent = "Изменение данных";
        }
        else
        {
            ActionForSubmitButton = ReactiveCommand.CreateFromTask(() =>
            {
                if (ValidatingData())
                {
                    AddRole();
                }

                return Task.CompletedTask;
            });
            ContentForSubmitButton = "Добавить";
            TitleContent = "Добавление роли";
        }
    }

    public ReactiveCommand<Unit, Unit> CancelButtonClick { get; }
    public ReactiveCommand<Unit, Unit> ActionForSubmitButton { get; }
    public string ContentForSubmitButton { get; }
    public string TitleContent { get; }

    public Role Role
    {
        get => _role;
        set => this.RaiseAndSetIfChanged(ref _role, value);
    }

    public string RoleName
    {
        get => _roleName;
        set => this.RaiseAndSetIfChanged(ref _roleName, value);
    }
    
    void CancellationOperation()
    {
        _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel);
    }
    
    async void AddRole()
    {
        Role role = new Role
        {
            Id = 2,
            RoleName = RoleName,
            CreationDate = DateTime.Now.ToUniversalTime().ToInstant().ToDateTimeOffset()
        };
        var task = RoleImplementation.AddRole(role);
        try
        {
            await task;
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Успех", 
                "Роль успешно создана\t", 
                ButtonEnum.Ok, 
                Icon.Info);
            messageBox.Show();
            CancellationOperation();
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Ошибка", 
                ex.Message + "\t", 
                ButtonEnum.Ok, 
                Icon.Error);
            messageBox.Show();
        }
    }
    
    
    async void UpdateRole()
    {
        Role role = new Role
        {
            Id = _idRole,
            RoleName = RoleName,
            CreationDate = Role.CreationDate,
            UpdatedDate = DateTime.Now.ToUniversalTime().ToInstant().ToDateTimeOffset()
        };
        var task = RoleImplementation.UpdateRole(role);
        try
        {
            await task;
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Успех", 
                "Данные успешно обновлены\t", 
                ButtonEnum.Ok, 
                Icon.Info);
            messageBox.Show();
            CancellationOperation();
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Ошибка", 
                ex.Message + "\t", 
                ButtonEnum.Ok, 
                Icon.Error);
            messageBox.Show();
        }
    }

    bool TryGetByRoleName(string roleName)
    {
        return string.Equals(roleName.Trim(), _forRoleName.Trim(), StringComparison.CurrentCultureIgnoreCase) || 
               RoleImplementation.GetAllRoles().Result.Any(r => 
                   string.Equals(r.RoleName.Trim(), roleName.Trim(), StringComparison.CurrentCultureIgnoreCase));
    }

    bool ValidatingData()
    {
        string errorText = "";
        if (!string.IsNullOrWhiteSpace(RoleName))
        {
            if (TryGetByRoleName(RoleName))
            {
                errorText = "Такая роль уже существует";
            }
            return true;
        }

        {
            errorText = "Заполните все поля";
        }
        var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
            "Ошибка", 
            errorText, 
            ButtonEnum.Ok, 
            Icon.Error);
        messageBox.Show();
        return false;
    }
}