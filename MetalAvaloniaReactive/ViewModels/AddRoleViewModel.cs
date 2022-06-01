namespace MetalAvaloniaReactive.ViewModels;

public class AddRoleViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    public AddRoleViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
}