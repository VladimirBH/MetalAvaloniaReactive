using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using NodaTime.Extensions;
using ReactiveUI;
using WebServer.Classes;
using WebServer.DataAccess.Implementations.Entities;

namespace MetalAvaloniaReactive.ViewModels;

public class MainAdminViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private int _selectedTabItem;
    private string _search;
    private ObservableCollection<User> _users;
    private ObservableCollection<Role> _roles;
    private ObservableCollection<CalculationHistory> _calculationHistories;
    private ObservableCollection<Furnace> _furnaces;
    private bool _isCalculationHistory;
    private Furnace _selectedFurnace;
    private decimal _ag;
    private decimal _al;
    private decimal _au;
    private decimal _ca;
    private decimal _cr;
    private decimal _cu;
    private decimal _fe;
    private decimal _ni;
    private decimal _pb;
    private decimal _si;
    private decimal _sn;
    private decimal _zn;
    public MainAdminViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        ExitFromApplication = ReactiveCommand.CreateFromTask(async () =>
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Предупреждение",
                "Вы уверены, что хотите выйти?", 
                ButtonEnum.YesNo, 
                Icon.Info);
            var messageBoxResult = await messageBox.Show();
            if (messageBoxResult == ButtonResult.Yes)
            {
                PreparedLocalStorage.ClearLocalStorage();
                PreparedLocalStorage.SaveLocalStorage();
                _mainWindowViewModel.Content = new AuthorizationViewModel(_mainWindowViewModel);
            }
        });
        try
        {
            this.WhenAnyValue(x => x.Search)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(Searching);
            this.WhenAnyValue(x => x.SelectedTabItem)
                .Throttle(TimeSpan.FromMilliseconds(10))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(IsVisibleAddButton);
            
            _users = Users = new ObservableCollection<User>(UserImplementation.GetAllUsers().Result.OrderBy(u => u.Id));
            
            _roles = Roles = new ObservableCollection<Role>(RoleImplementation.GetAllRoles().Result.OrderBy(r => r.Id));
            
            _furnaces = Furnaces = new ObservableCollection<Furnace>(FurnaceImplementation.GetAllFurnaces().Result);
            
            _calculationHistories = CalculationHistories = new ObservableCollection<CalculationHistory>(
                CalculationHistoryImplementation.GetAllHistoryRecords().
                Result.OrderBy(x => x.CreationDate));
            
            CaclButtonClick = ReactiveCommand.CreateFromTask(() =>
            {
                CompositionCalculation();
                return Task.CompletedTask;
            });
            SearchButtonClick = ReactiveCommand.Create(SearchingItems);
            _selectedTabItem = 0;
            IsCalculationHistory = _selectedTabItem != 2;
            AddRecordClick = ReactiveCommand.Create<int>(OpenOneRecordView);
            DeleteRecordClick = ReactiveCommand.CreateFromTask<int>(async (id) => { DeleteRecord(id); });
            UpdateRecordClick = ReactiveCommand.Create<int>(OpenOneRecordView);
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Ошибка", 
                    "ex.Message", 
                    ButtonEnum.Ok, 
                    Icon.Error);
            messageBox.Show();
        }


    }

    void IsVisibleAddButton(int selectedTabItem)
    {
        _isCalculationHistory = IsCalculationHistory = selectedTabItem != 2 && selectedTabItem != 4;
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
    public ObservableCollection<CalculationHistory> CalculationHistories
    {
        get => _calculationHistories;
        set => this.RaiseAndSetIfChanged(ref _calculationHistories, value);
    }
    public ObservableCollection<Furnace> Furnaces
    {
        get => _furnaces;
        set => this.RaiseAndSetIfChanged(ref _furnaces, value);
    }

    public int SelectedTabItem
    {
        get => _selectedTabItem;
        set
        {
            IsCalculationHistory = _selectedTabItem != 2 && _selectedTabItem != 4;
            this.RaiseAndSetIfChanged(ref _selectedTabItem, value);
        }
    }

    public ReactiveCommand<int, Unit> UpdateRecordClick { get; }
    public ReactiveCommand<int, Unit> DeleteRecordClick { get; }
    
    public ReactiveCommand<Unit, Unit> SearchButtonClick { get; }

    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }

    public ReactiveCommand<int, Unit> AddRecordClick { get; }

    public ReactiveCommand<Unit, Unit> CaclButtonClick { get; }

    public Furnace SelectedFurnace
    {
        get => _selectedFurnace;
        set => this.RaiseAndSetIfChanged(ref _selectedFurnace, value);
    }

    public decimal Ag
    {
        get => _ag;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Al
    {
        get => _al;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Au
    {
        get => _au;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Ca
    {
        get => _ca;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Cr
    {
        get => _cr;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Cu
    {
        get => _cu;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Fe
    {
        get => _fe;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Ni
    {
        get => _ni;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Pb
    {
        get => _pb;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Si
    {
        get => _si;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Sn
    {
        get => _sn;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }
        
    public decimal Zn
    {
        get => _zn;
        set => this.RaiseAndSetIfChanged(ref _ag, value);
    }


    public bool IsCalculationHistory
    {
        get => _isCalculationHistory; 
        set => this.RaiseAndSetIfChanged(ref _isCalculationHistory, value);
    }

    async void DeleteRecord(int id)
    {
        var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
            "Предупреждение",
            "Вы уверены, что хотите удалить запись?", 
            ButtonEnum.YesNo, 
            Icon.Warning);
        var messageBoxResult = await messageBox.Show();
        
        if (messageBoxResult == ButtonResult.No) return;
        try
        {
            switch (SelectedTabItem)
            {
                case 0:
                    if (UserImplementation.GetCurrentUserInfo().Result.Id != id)
                    {
                        await UserImplementation.DeleteUser(id);
                    }
                    else
                    {
                        var messageBoxErrorId =
                        MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                            "Ошибка", 
                            "Вы не можете удалить самого себя \t",
                            ButtonEnum.Ok, 
                            Icon.Error);
                        messageBoxErrorId.Show();
                    }
                    break;
                case 1:
                    await RoleImplementation.DeleteRole(id);
                    break;
                case 3:
                    await FurnaceImplementation.DeleteFurnace(id);
                    break;
            }

            var messageBoxInfo =
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Инфо", 
                    "Запись успешно удалена \t",
                    ButtonEnum.Ok, 
                    Icon.Info);
            messageBoxInfo.Show();
            _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel);
        }
        catch (Exception ex)
        {
            var messageBoxError = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Ошибка",
                "Не удалось повторить действие, возможно у элемента есть зависимые записи\t", 
                ButtonEnum.Ok, 
                Icon.Error);
            messageBoxError.Show();
        }
    }
    void OpenOneRecordView(int id)
    {
        _mainWindowViewModel.Content = SelectedTabItem switch
        {
            0 => new AddUserViewModel(_mainWindowViewModel, id),
            1 => new AddRoleViewModel(_mainWindowViewModel, id),
            3 => new AddFurnaceViewModel(_mainWindowViewModel, id)
        };
    }

    void Searching(string s)
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
            case 0: _users = Users = new ObservableCollection<User>(UserImplementation.GetAllUsers().Result
                    .Where(u => u.Login.Trim().ToLower().Contains(s.Trim().ToLower()) || 
                                u.Surname.Trim().ToLower().Contains(s.Trim().ToLower()) || 
                                u.Name.Trim().ToLower().Contains(s.Trim().ToLower()) ||
                                u.Patronymic.Trim().ToLower().Contains(s.Trim().ToLower()) ||
                                u.Id.ToString() == s));
                break;
            case 1:
                _roles = Roles = new ObservableCollection<Role>(
                    RoleImplementation.
                        GetAllRoles().
                        Result.
                        Where(r => r.RoleName.Trim().ToLower().Contains(s.Trim().ToLower())));
                break;
            case 3:
                _furnaces = Furnaces = new ObservableCollection<Furnace>(
                    FurnaceImplementation.
                        GetAllFurnaces().
                        Result.
                        Where(r => r.FurnaceName.Trim().ToLower().Contains(s.Trim().ToLower())));
                break;
        }


    }
    void SearchingItems()
    {
        _users = new ObservableCollection<User>(Users.Where(u => u.Login.ToLower().Contains(_search)));
        this.RaisePropertyChanged();
    }

    async void CompositionCalculation()
    {
        var user = UserImplementation.GetCurrentUserInfo();
        try
        {
            await user;
            CalculationHistory calculationHistory = new CalculationHistory
            {
                Ag = Ag,
                Al = Al,
                Au = Au,
                Ca = Ca,
                Cr = Cr,
                Cu = Cu,
                Fe = Fe,
                Ni = Ni,
                Pb = Pb,
                Si = Si,
                Sn = Sn,
                Zn = Zn,
                FurnaceId = SelectedFurnace.Id,
                UserId = user.Result.Id,
                CreationDate = DateTime.Now.ToUniversalTime().ToInstant().ToDateTimeOffset()
            };
            await CalculationHistoryImplementation.AddCalculationHistory(calculationHistory);
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Успех", 
                "Скоро здесь будет результат подсчета\t", 
                ButtonEnum.Ok, 
                Icon.Info);
            messageBox.Show();
        }
        catch (AggregateException ex)
        {
            var messageBoxError = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "Ошибка",
                "Отсутствует подключение к серверу\t", 
                ButtonEnum.Ok, 
                Icon.Error);
            messageBoxError.Show();
        }
    }
}