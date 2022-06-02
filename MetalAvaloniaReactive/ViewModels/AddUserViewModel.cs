using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Authentication;
using System.Windows.Input;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels;

public class AddUserViewModel : ViewModelBase
{
    private MainWindowViewModel _mainWindowViewModel;
    private string _surname;
    private string _name;
    private string _patronymic;
    private string _phoneNumber;
    private string _login;
    private string _password;
    private DateTime _dateBirth;
    private int _roleId;


    private List<string> _roleNames;

    public AddUserViewModel(MainWindowViewModel mainWindowViewModel, int idUser)
    {
        var addEnabled = this.WhenAnyValue(
            surname => surname.Surname,
            name => name.Name,
            login => login.Login,
            password => password.Password,
            phoneNumber => phoneNumber.PhoneNumber,
            dateBirth => dateBirth.DateBirth,
            (surname, name, login,password, phoneNumber,  dateBirth) => !string.IsNullOrWhiteSpace(surname) &&
                                                                        !string.IsNullOrWhiteSpace(name) &&
                                                                        !string.IsNullOrWhiteSpace(login) &&
                                                                        !string.IsNullOrWhiteSpace(password) &&
                                                                        !string.IsNullOrWhiteSpace(phoneNumber) &&
                                                                        dateBirth < DateTime.Now.Date.AddYears(-18) &&
                                                                        dateBirth > DateTime.Now.Date.AddYears(-100));
        _mainWindowViewModel = mainWindowViewModel;
        CancelButtonClick = ReactiveCommand.Create(CancellationOperation);
        _roleNames = new List<string>();
        foreach (Role role in RoleImplementation.GetAllRoles().Result)
        {
            _roleNames.Add(role.RoleName);
        }

        if (idUser != -1)
        {
            User user = UserImplementation.GetUserById(idUser).Result;
            _surname = user.Surname;
            _name = user.Name;
            _patronymic = user.Patronymic;
            _login = user.Login;
            _password = user.Password;
            _dateBirth = user.DateBirth;
            _phoneNumber = user.PhoneNumber;
            _roleId = user.RoleId;
            ActionForSubmitButton = ReactiveCommand.Create(UpdateUser);
            ContentForSubmitButton = "Изменить";
            TitleContent = "Изменение данных";
        }
        else
        {
            ActionForSubmitButton = ReactiveCommand.CreateFromTask( async () =>
            {
                User user = new User
                {
                    Id = 2,
                    Surname = Surname,
                    Name = Name,
                    Patronymic = Patronymic,
                    DateBirth = DateBirth,
                    PhoneNumber = PhoneNumber,
                    RoleId = 1,
                    Login = Login,
                    Password = Password
                };
                var task = UserImplementation.AddUser(user);
                try
                {
                    await task;
                }
                catch (AuthenticationException ex)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
                    messageBox.Show();
                }
            }, addEnabled);
            ContentForSubmitButton = "Добавить";
            TitleContent = "Добавление пользователя";
        }
    }

    public ReactiveCommand<Unit, Unit> CancelButtonClick { get; }
    public ReactiveCommand<Unit, Unit> ActionForSubmitButton { get; }
    public string ContentForSubmitButton { get; }
    public string TitleContent { get; }

    public string Surname
    {
        get => _surname;
        set => this.RaiseAndSetIfChanged(ref _surname, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    public string Patronymic
    {
        get => _patronymic;
        set => this.RaiseAndSetIfChanged(ref _patronymic, value);
    }
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => this.RaiseAndSetIfChanged(ref _phoneNumber, value);
    }
    
    public int RoleId
    {
        get => _roleId;
        set => this.RaiseAndSetIfChanged(ref _roleId, value);
    }
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    public DateTime DateBirth
    {
        get => _dateBirth;
        set => this.RaiseAndSetIfChanged(ref _dateBirth, value);
    }
    

    public List<string> RoleNames
    {
        get => _roleNames;
        set => this.RaiseAndSetIfChanged(ref _roleNames, value);
    }
    void CancellationOperation()
    {
        _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel);
    }

    async void AddUser()
    {



    }

    void UpdateUser()
    {
        
    }
}