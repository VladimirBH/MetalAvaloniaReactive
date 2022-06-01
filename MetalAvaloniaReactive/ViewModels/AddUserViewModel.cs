using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class AddUserViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    public AddUserViewModel(MainWindowViewModel mainWindowViewModel, int idUser)
    {
        _mainWindowViewModel = mainWindowViewModel;

    }

}