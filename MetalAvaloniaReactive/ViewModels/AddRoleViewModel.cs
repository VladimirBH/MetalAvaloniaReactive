namespace MetalAvaloniaReactive.ViewModels;

public class AddRoleViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private int _id;
    public AddRoleViewModel(MainWindowViewModel mainWindowViewModel, int id)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _id = id;
    }
}