using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MetalAvaloniaReactive.Views;

public partial class AddFurnaceView : UserControl
{
    public AddFurnaceView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}