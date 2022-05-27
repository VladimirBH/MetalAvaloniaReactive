using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetalAvaloniaReactive.Views;

public partial class MainAdminView : UserControl
{
    public MainAdminView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}