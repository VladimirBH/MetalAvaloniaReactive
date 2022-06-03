using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class MainAdminViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    public MainAdminViewModel(MainWindowViewModel mainWindowViewModel)
    {
        try
        {
            Users = new ObservableCollection<User>(UserImplementation.GetAllUsers().Result);
            Roles = new ObservableCollection<Role>(RoleImplementation.GetAllRoles().Result);
            _mainWindowViewModel = mainWindowViewModel;
            ExitFromApplication = ReactiveCommand.CreateFromTask(async () =>
            {
                var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Предупреждение", "Вы уверены, что хотите выйти?", ButtonEnum.YesNo, Icon.Info);
                var messageBoxResult = await messageBox.Show();
                if (messageBoxResult == ButtonResult.Yes)
                {
                    PreparedLocalStorage.ClearLocalStorage();
                    PreparedLocalStorage.SaveLocalStorage();
                    _mainWindowViewModel.Content = new AuthorizationViewModel(_mainWindowViewModel);
                }
            });
            DeleteUserClick = ReactiveCommand.CreateFromTask<int>(async (id) =>
            {
                DeleteRecord(id, "users");
            });
            UpdateUserClick = AddUserClick= ReactiveCommand.Create<int>(OpenOneUserView);

            UpdateRoleClick = AddRoleClick = ReactiveCommand.Create<int>(OpenOneRoleView);
            DeleteRoleClick = ReactiveCommand.CreateFromTask<int>(async (id) =>
            {
                DeleteRecord(id, "roles");
            });
        }
        catch (Exception ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow
                ("Ошибка", ex.Message, ButtonEnum.Ok, Icon.Error);
            messageBox.Show();
        }
    }
    
    public ObservableCollection<User> Users { get; }
    public ObservableCollection<Role> Roles { get; }

    
    
    public ReactiveCommand<int, Unit> UpdateUserClick { get; }
    public ReactiveCommand<int, Unit> AddUserClick { get; }
    public ReactiveCommand<int, Unit> DeleteUserClick { get; }
    
    public ReactiveCommand<int, Unit> DeleteRoleClick { get; }
    public ReactiveCommand<int, Unit> UpdateRoleClick { get; }
    public ReactiveCommand<int, Unit> AddRoleClick { get; }

    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }
    
    
    void OpenOneUserView(int id)
    {
        _mainWindowViewModel.Content = new AddUserViewModel(_mainWindowViewModel, id);
    }
    
    async void DeleteRecord(int id, string table)
    {
        var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Предупреждение", "Вы уверены, что хотите удалить запись?", ButtonEnum.YesNo, Icon.Warning);
        var messageBoxResult = await messageBox.Show();
        if (messageBoxResult == ButtonResult.No) return;
        try
        {
            switch (table)
            {
                case "users":
                    await UserImplementation.DeleteUser(id);
                    break;
                case "roles":
                    await RoleImplementation.DeleteRole(id);
                    break;
            }
            var messageBoxInfo = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Инфо", "Запись успешно удалена \t", ButtonEnum.Ok, Icon.Info);
            messageBoxInfo.Show();
            _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel);
        }
        catch (Exception ex)
        {
            var messageBoxError = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Попробуйте повторить действие позже \t", ButtonEnum.Ok, Icon.Error);
            messageBoxError.Show();
        }
    }
    
    void OpenOneRoleView(int id)
    {
        _mainWindowViewModel.Content = new AddRoleViewModel(_mainWindowViewModel, id);
    }
}