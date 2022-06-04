using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class MainAdminViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private int _selectedTabItem;
    private string _search;
    private ObservableCollection<User> _users;
    private ObservableCollection<Role> _roles;
    public event PropertyChangedEventHandler PropertyChanged;
    public MainAdminViewModel(MainWindowViewModel mainWindowViewModel)
    {
        try
        {
            this.WhenAnyValue(x => x.Search)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(Searching);
            
            _users = Users = new ObservableCollection<User>(UserImplementation.GetAllUsers().Result);
            _roles = Roles = new ObservableCollection<Role>(RoleImplementation.GetAllRoles().Result);
            SearchButtonClick = ReactiveCommand.Create(SearchingItems);
            _mainWindowViewModel = mainWindowViewModel;
            _selectedTabItem = 0;
            ExitFromApplication = ReactiveCommand.CreateFromTask(async () =>
            {
                var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Предупреждение",
                    "Вы уверены, что хотите выйти?", ButtonEnum.YesNo, Icon.Info);
                var messageBoxResult = await messageBox.Show();
                if (messageBoxResult == ButtonResult.Yes)
                {
                    PreparedLocalStorage.ClearLocalStorage();
                    PreparedLocalStorage.SaveLocalStorage();
                    _mainWindowViewModel.Content = new AuthorizationViewModel(_mainWindowViewModel);
                }
            });
            AddRecordClick = ReactiveCommand.Create<int>(OpenOneRecordView);
            DeleteRecordClick = ReactiveCommand.CreateFromTask<int>(async (id) => { DeleteRecord(id); });
            UpdateRecordClick = ReactiveCommand.Create<int>(OpenOneRecordView);
        }
        catch (Exception ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow
                ("Ошибка", ex.Message, ButtonEnum.Ok, Icon.Error);
            messageBox.Show();
        }
    }

    public string Search
    {
        get => _search;
        set => this.RaiseAndSetIfChanged(ref _search, value);

    }

    public ObservableCollection<User> Users
    {
        get => _users;
        set => this.RaiseAndSetIfChanged(ref _users, value);
    }
    public ObservableCollection<Role> Roles
    {
        get => _roles;
        set => this.RaiseAndSetIfChanged(ref _roles, value);
    }

    public int SelectedTabItem
    {
        get => _selectedTabItem;
        set => this.RaiseAndSetIfChanged(ref _selectedTabItem, value);
    }
    public ReactiveCommand<int, Unit> UpdateRecordClick { get; }
    public ReactiveCommand<int, Unit> DeleteRecordClick { get; }
    
    public ReactiveCommand<Unit, Unit> SearchButtonClick { get; }

    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }

    public ReactiveCommand<int, Unit> AddRecordClick { get; } 
    
    async void DeleteRecord(int id)
    {
        var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Предупреждение",
            "Вы уверены, что хотите удалить запись?", ButtonEnum.YesNo, Icon.Warning);
        var messageBoxResult = await messageBox.Show();
        if (messageBoxResult == ButtonResult.No) return;
        try
        {
            switch (SelectedTabItem)
            {
                case 0:
                    await UserImplementation.DeleteUser(id);
                    break;
                case 1:
                    await RoleImplementation.DeleteRole(id);
                    break;
            }

            var messageBoxInfo =
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Инфо", "Запись успешно удалена \t",
                    ButtonEnum.Ok, Icon.Info);
            messageBoxInfo.Show();
            _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel);
        }
        catch (Exception ex)
        {
            var messageBoxError = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "Не удалось повторить действие, возможно у элемента есть зависимые записи\t", ButtonEnum.Ok, Icon.Error);
            messageBoxError.Show();
        }
    }
    void OpenOneRecordView(int id)
    {
        _mainWindowViewModel.Content = SelectedTabItem switch
        {
            0 => new AddUserViewModel(_mainWindowViewModel, id),
            1 => new AddRoleViewModel(_mainWindowViewModel, id)
        };
    }

    async void Searching(string s)
    {
        _users.Clear();
        _roles.Clear();
        if (string.IsNullOrWhiteSpace(s))
        {
            _users = Users = new ObservableCollection<User>(UserImplementation.GetAllUsers().Result);
            _roles = Roles = new ObservableCollection<Role>(RoleImplementation.GetAllRoles().Result); 
            return;
        }
        switch (SelectedTabItem)
        {
            case 0: _users = Users = new ObservableCollection<User>(UserImplementation.GetAllUsers().Result.Where(u => u.Login.Trim().ToLower().Contains(s)));
                break;
            case 1:
                _roles = Roles = new ObservableCollection<Role>(RoleImplementation.GetAllRoles().Result.Where(r => r.RoleName.Trim().ToLower().Contains(s)));
                break;
        }


    }
    void SearchingItems()
    {
        _users = new ObservableCollection<User>(Users.Where(u => u.Login.ToLower().Contains(_search)));
        this.RaisePropertyChanged();
    }
}