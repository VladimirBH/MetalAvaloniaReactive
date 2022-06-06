using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetalAvaloniaReactive.Views;

public partial class ConnectionErrorView : UserControl
{
    public ConnectionErrorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}