using System;
using System.Reactive;
using System.Security.Authentication;
using AvaloniaClientMetal.Models;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class AuthorizationViewModel : ViewModelBase
{
    private string login;
    private string password;
    private MainWindowViewModel _mainWindowViewModel;

    public AuthorizationViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        var okEnabled = this.WhenAnyValue(
            x => x.Login, 
            y => y.Password,
            (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y));
        AuthorizationButtonClick = ReactiveCommand.CreateFromTask(async () =>
            {
                var tokenPair = UserImplementation.UserAuthorization(new DataAuth 
                    { 
                        Login = Login, 
                        Password = Password 
                    });
                try
                {
                    await tokenPair;
                    PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair.Result);
                    KeepRoleId.RoleId = tokenPair.Result.IdRole;
                    if (KeepRoleId.RoleId == 1)
                    {
                        _mainWindowViewModel.Content = new MainAdminViewModel(mainWindowViewModel);
                    }
                    else
                    {
                        _mainWindowViewModel.Content = new MainUserViewModel(mainWindowViewModel);
                    }
                }
                catch (AuthenticationException ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        "Ошибка",
                        ex.Message + "\t", 
                        ButtonEnum.Ok, 
                        Icon.Error);
                    messageBox.Show();
                }
                catch (Exception ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        "Ошибка",
                        "Произошла ошибка\t", 
                        ButtonEnum.Ok, 
                        Icon.Error);
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