using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvaloniaClientMVVM.Models;

namespace MetalAvaloniaReactive.ViewModels;

public class MainAdminViewModel : ViewModelBase
{
    public MainAdminViewModel(IEnumerable<User> users, IEnumerable<Role> roles)
    {
        Users = new ObservableCollection<User>(users);
        Roles = new ObservableCollection<Role>(roles);
    }

    public ObservableCollection<User> Users { get; }
    public ObservableCollection<Role> Roles { get; }
}