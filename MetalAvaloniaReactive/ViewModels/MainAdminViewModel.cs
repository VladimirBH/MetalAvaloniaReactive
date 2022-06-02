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
            DeleteUser(id);
        });
        UpdateUserClick = ReactiveCommand.Create<int>(OpenOneUserView);
        AddUserClick = ReactiveCommand.Create<int>(OpenOneUserView);
    }
    
    public ObservableCollection<User> Users { get; }
    public ObservableCollection<Role> Roles { get; }

    
    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }
    public ReactiveCommand<int, Unit> UpdateUserClick { get; }
    
    public ReactiveCommand<int, Unit> AddUserClick { get; }
    public ReactiveCommand<int, Unit> DeleteUserClick { get; }

    void OpenOneUserView(int id)
    {
        _mainWindowViewModel.Content = new AddUserViewModel(_mainWindowViewModel, id);
    }

    async void DeleteUser(int id)
    {
        var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Предупреждение", "Вы уверены, что хотите удалить запись?", ButtonEnum.YesNo, Icon.Warning);
        var messageBoxResult = await messageBox.Show();
        if (messageBoxResult == ButtonResult.No)
        {
            Console.WriteLine("Отменил");
        }
        else
        {
            Console.WriteLine("Не отменил");
        }
    }
}