using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetalAvaloniaReactive.Views;

public partial class MainUserView : UserControl
{
    public MainUserView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}