using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MetalAvaloniaReactive.ViewModels;

namespace MetalAvaloniaReactive.Views;

public partial class AddUserView : UserControl
{
    public AddUserView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}