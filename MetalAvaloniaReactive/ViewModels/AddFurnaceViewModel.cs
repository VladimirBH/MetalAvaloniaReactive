using System;
using System.Reactive;
using System.Security.Authentication;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using NodaTime.Extensions;
using ReactiveUI;
using WebServer.DataAccess.Implementations.Entities;

namespace MetalAvaloniaReactive.ViewModels;

public class AddFurnaceViewModel : ViewModelBase
{
   private MainWindowViewModel _mainWindowViewModel;
    private int _idRole;
    private string _furnaceName;
    private Furnace _furnace;
    public AddFurnaceViewModel(MainWindowViewModel mainWindowViewModel, int idRole)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _idRole = idRole;
        var addEnabled = this.WhenAnyValue(
            x => x.FurnaceName,
            (x) => !string.IsNullOrWhiteSpace(x));
        _mainWindowViewModel = mainWindowViewModel;
        CancelButtonClick = ReactiveCommand.Create(CancellationOperation);
        
        if (_idRole != -1)
        {
            Furnace = FurnaceImplementation.GetFurnaceById(_idRole).Result;
            FurnaceName = Furnace.FurnaceName;
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

    public Furnace Furnace
    {
        get => _furnace;
        set => this.RaiseAndSetIfChanged(ref _furnace, value);
    }

    public string FurnaceName
    {
        get => _furnaceName;
        set => this.RaiseAndSetIfChanged(ref _furnaceName, value);
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
            RoleName = FurnaceName,
            CreationDate = DateTime.Now.ToUniversalTime().ToInstant().ToDateTimeOffset()
        };
        var task = RoleImplementation.AddRole(role);
        try
        {
            await task;
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Успех", "Печь успешно внесена в базу\t", ButtonEnum.Ok, Icon.Info);
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
        Furnace furnace = new Furnace
        {
            Id = _idRole,
            FurnaceName = FurnaceName,
            CreationDate = Furnace.CreationDate,
            UpdatedDate = DateTime.Now.ToUniversalTime().ToInstant().ToDateTimeOffset()
        };
        var task = FurnaceImplementation.UpdateFurnace(furnace);
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