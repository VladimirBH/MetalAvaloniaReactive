using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MetalAvaloniaReactive.ViewModels;
using ReactiveUI;

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