using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using NodaTime.Extensions;
using ReactiveUI;
using WebServer.Classes;
using WebServer.DataAccess.Implementations.Entities;

namespace MetalAvaloniaReactive.ViewModels;

public class MainUserViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private ObservableCollection<Furnace> _furnaces;
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
    public MainUserViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _furnaces = Furnaces = new ObservableCollection<Furnace>(FurnaceImplementation.GetAllFurnaces().Result);
        CaclButtonClick = ReactiveCommand.CreateFromTask(() =>
        {
            CompositionCalculation();
            return Task.CompletedTask;
        });
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
    }
    public ObservableCollection<Furnace> Furnaces
    {
        get => _furnaces;
        set => this.RaiseAndSetIfChanged(ref _furnaces, value);
    }
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
    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }

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
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Успех",
                $"Скоро здесь будет результат подсчета\t", ButtonEnum.Ok, Icon.Info);
            messageBox.Show();
        }
        catch (AggregateException ex)
        {
            var messageBoxError = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                "Отсутствует подключение к серверу\t", ButtonEnum.Ok, Icon.Error);
            messageBoxError.Show();
        }
    }

}