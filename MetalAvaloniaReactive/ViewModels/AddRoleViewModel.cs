using System;
using System.Reactive;
using System.Security.Authentication;
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
            ActionForSubmitButton = ReactiveCommand.CreateFromTask(async () =>
            {
                UpdateRole();
            });
            ContentForSubmitButton = "Изменить";
            TitleContent = "Изменение данных";
        }
        else
        {
            ActionForSubmitButton = ReactiveCommand.CreateFromTask( async () =>
            {
                AddRole();
            }, addEnabled);
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
        _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel, true);
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
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Успех", "Роль успешно создана\t", ButtonEnum.Ok, Icon.Info);
            messageBox.Show();
            CancellationOperation();
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
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
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Успех", "Данные успешно обновлены\t", ButtonEnum.Ok, Icon.Info);
            messageBox.Show();
            CancellationOperation();
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
            messageBox.Show();
        }
    }
}