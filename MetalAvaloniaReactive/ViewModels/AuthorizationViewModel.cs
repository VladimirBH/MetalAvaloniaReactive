using System;
using System.Reactive;
using System.Security.Authentication;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using MetalAvaloniaReactive.ViewModels;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class AuthorizationViewModel : ViewModelBase
{
    private string login;
    private string password;

    public AuthorizationViewModel()
    {
        var okEnabled = this.WhenAnyValue(
            x => x.Login, 
            y => y.Password,
            (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y));
        AuthorizationButtonClick = ReactiveCommand.CreateFromTask(async () =>
            {
                var tokenPair = UserImplementation.UserAuthorization(new DataAuth { Login = Login, Password = Password });
                try
                {
                    await tokenPair;
                    PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair.Result);
                    MainWindowViewModel mainWindowViewModel = new MainWindowViewModel
                    {
                        Content = new MainAdminViewModel(UserImplementation.GetAllUsers().Result, RoleImplementation.GetAllRoles().Result)
                    };
                }
                catch (AuthenticationException ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
                    messageBox.Show();
                }
                /*catch (Exception ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        "Произошла ошибка\t", ButtonEnum.Ok, Icon.Error);
                    messageBox.Show();
                }*/
            }, okEnabled
        );
    }

    public string Login
    {
        get => login;
        set => this.RaiseAndSetIfChanged(ref login, value);
    }
    
    public string Password
    {
        get => password;
        set => this.RaiseAndSetIfChanged(ref password, value);
    }
    
    public ReactiveCommand<Unit, Unit> AuthorizationButtonClick { get; }
}