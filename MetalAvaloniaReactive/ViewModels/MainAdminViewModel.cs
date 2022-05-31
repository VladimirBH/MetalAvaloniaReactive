using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        ShowDialog = new Interaction<AddUserViewModel, MainAdminViewModel?>();
        UpdateRoleClick = ReactiveCommand.CreateFromTask<int>(OpenAddUserWindow);
    }

        
    public Interaction<AddUserViewModel, MainAdminViewModel?> ShowDialog { get; }
    public ICommand UpdateRoleClick { get; }
    public ObservableCollection<User> Users { get; }
    public ObservableCollection<Role> Roles { get; }

    async Task OpenAddUserWindow(int idUser)
    {
        var addUser = new AddUserViewModel(_mainWindowViewModel);
        var result = await ShowDialog.Handle(addUser);
        
    }
}