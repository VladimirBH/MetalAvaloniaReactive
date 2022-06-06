namespace MetalAvaloniaReactive.ViewModels;

public class AddFurnaceViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private int _id;
    public AddFurnaceViewModel(MainWindowViewModel mainWindowViewModel, int id)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _id = id;
    }
}