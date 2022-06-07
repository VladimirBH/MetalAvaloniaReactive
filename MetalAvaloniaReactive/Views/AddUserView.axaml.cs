using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Validation.Extensions;

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