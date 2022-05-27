using System;
using System.Reactive;
using System.Security.Authentication;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using MessageBox.Avalonia.Enums;
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
        AuthorizationButtonClick = ReactiveCommand.Create(
            () =>
            {
                try
                {
                    Task<TokenPair> tokenPair = UserImplementation.UserAuthorization(new DataAuth { Login = Login, Password = Password });
                    PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair.Result);
                }
                catch (AuthenticationException ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
                    messageBox.Show();
                }
                catch (Exception ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка",
                        "Произошла ошибка\t", ButtonEnum.Ok, Icon.Error);
                    messageBox.Show();
                }
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