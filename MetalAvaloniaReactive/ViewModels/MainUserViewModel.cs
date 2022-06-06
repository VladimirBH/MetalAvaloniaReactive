using System.Reactive;
using AvaloniaClientMetal.Models;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class MainUserViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    
    public MainUserViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
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
    
    public ReactiveCommand<Unit, Unit> ExitFromApplication { get; }
}