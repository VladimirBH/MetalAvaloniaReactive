using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class MainAdminViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    public MainAdminViewModel(IEnumerable<User> users, IEnumerable<Role> roles, MainWindowViewModel mainWindowViewModel)
    {
        Users = new ObservableCollection<User>(users);
        Roles = new ObservableCollection<Role>(roles);
        //AuthorizationButtonClick = ReactiveCommand.Create<int>(OpenAddUserWindow);
        _mainWindowViewModel = mainWindowViewModel;
        ExitFromApplication = ReactiveCommand.Create(() =>
        {
            PreparedLocalStorage.ClearLocalStorage();
            PreparedLocalStorage.SaveLocalStorage();
            _mainWindowViewModel.Content = new AuthorizationViewModel(_mainWindowViewModel);
        });

        UpdateUserClick = ReactiveCommand.Create<int>(OpenUpdateUserView);
    }
    
    public ObservableCollection<User> Users { get; }
    public ObservableCollection<Role> Roles { get; }

    
    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }
    
    public ReactiveCommand<int, Unit> UpdateUserClick { get; }
    void OpenUpdateUserView(int id)
    {
        _mainWindowViewModel.Content = new AddUserViewModel(_mainWindowViewModel, id);
    }
    
}