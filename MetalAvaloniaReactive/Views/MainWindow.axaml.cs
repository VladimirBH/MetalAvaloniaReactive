using Avalonia.ReactiveUI;
using MetalAvaloniaReactive.ViewModels;

namespace MetalAvaloniaReactive.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}