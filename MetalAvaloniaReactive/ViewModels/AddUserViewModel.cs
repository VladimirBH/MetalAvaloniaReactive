using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class AddUserViewModel
{
    private MainWindowViewModel _mainWindowViewModel;
    public AddUserViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

    }

}