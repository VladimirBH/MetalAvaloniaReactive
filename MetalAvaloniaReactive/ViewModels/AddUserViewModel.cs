using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Security.Authentication;
using System.Threading.Tasks;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MessageBox.Avalonia.Enums;
using MetalAvaloniaReactive.Models;
using NodaTime.Extensions;
using ReactiveUI;
//using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace MetalAvaloniaReactive.ViewModels;

public class AddUserViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private string _surname;
    private string _name;
    private string _patronymic;
    private string _phoneNumber;
    private string _login;
    private string _password;
    private DateTime _dateBirth;
    private int _roleId;
    private Role _selectedRole;
    private readonly User _user;
    private string ForPassword;

    private List<Role> _roles;

    public AddUserViewModel(MainWindowViewModel mainWindowViewModel, int idUser)
    {
        var addEnabled = this.WhenAnyValue(
            surname => surname.Surname,
            name => name.Name,
            login => login.Login,
            password => password.Password,
            phoneNumber => phoneNumber.PhoneNumber,
            dateBirth => dateBirth.DateBirth,
            selectedRole => selectedRole.SelectedRole,
            (surname, name, login,password, phoneNumber,  dateBirth, selectedRole) => !string.IsNullOrWhiteSpace(surname) &&
                                                                        !string.IsNullOrWhiteSpace(name) &&
                                                                        !string.IsNullOrWhiteSpace(login) &&
                                                                        !string.IsNullOrWhiteSpace(password) &&
                                                                        !string.IsNullOrWhiteSpace(phoneNumber) &&
                                                                        selectedRole != null &&
                                                                        login.Length > 3 &&
                                                                        password.Length > 8);
        _mainWindowViewModel = mainWindowViewModel;
        CancelButtonClick = ReactiveCommand.Create(CancellationOperation);
        _roles = RoleImplementation.GetAllRoles().Result;
        
        if (idUser != -1)
        {
             _user = UserImplementation.GetUserById(idUser).Result;
            _surname = _user.Surname;
            _name = _user.Name;
            _patronymic = _user.Patronymic;
            _login = _user.Login;
            _dateBirth = _user.DateBirth;
            _phoneNumber = _user.PhoneNumber;
            _roleId = _user.RoleId;
            ForPassword = _user.Password;
            ActionForSubmitButton = ReactiveCommand.CreateFromTask(() =>
            {
                UpdateUser();
                return Task.CompletedTask;
            }, addEnabled);
            ContentForSubmitButton = "Изменить";
            TitleContent = "Изменение данных";
            _selectedRole = _roles.FirstOrDefault(x => x.Id == _roleId);
        }
        else
        {
            _surname = string.Empty;
            _name = string.Empty;
            _patronymic = string.Empty;
            _login = string.Empty;
            _dateBirth = DateTime.Now;
            _phoneNumber = string.Empty;
            _roleId = -1;
            ForPassword = string.Empty;
            ActionForSubmitButton = ReactiveCommand.CreateFromTask(() =>
            {
                AddUser();
                return Task.CompletedTask;
            });
            ContentForSubmitButton = "Добавить";
            TitleContent = "Добавление пользователя";
            _selectedRole = null;
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
    

    public List<Role> Roles
    {
        get => _roles;
        set => this.RaiseAndSetIfChanged(ref _roles, value);
    }
    
    void CancellationOperation()
    {
        _mainWindowViewModel.Content = new MainAdminViewModel(_mainWindowViewModel, true);
    }

    public Role SelectedRole
    {
        get => _selectedRole;
        set => this.RaiseAndSetIfChanged(ref _selectedRole, value);
    }

    async void AddUser()
    {
        User user = new User
        {
            Id = 2,
            Surname = Surname,
            Name = Name,
            Patronymic = Patronymic,
            DateBirth = DateBirth, 
            PhoneNumber = PhoneNumber,
            RoleId = SelectedRole.Id,
            Login = Login,
            Password = BCrypt.Net.BCrypt.HashPassword(Password, 14),
            CreationDate = DateTime.Now.ToUniversalTime().ToInstant().ToDateTimeOffset()
        };
        var task = UserImplementation.AddUser(user);
        try
        {
            await task;
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Успех", "Пользователь успешно создан\t", ButtonEnum.Ok, Icon.Info);
            messageBox.Show();
            CancellationOperation();
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
            messageBox.Show();
        }
    }
    
    
    async void UpdateUser()
    {
        if (!string.IsNullOrWhiteSpace(Password))
        {
            ForPassword = BCrypt.Net.BCrypt.HashPassword(Password, 14);
        }

        User user = new User
        {
            Id = _user.Id,
            Surname = Surname,
            Name = Name,
            Patronymic = Patronymic,
            DateBirth = DateBirth, 
            PhoneNumber = PhoneNumber,
            RoleId = SelectedRole.Id,
            Login = Login,
            Password = ForPassword,
            CreationDate = _user.CreationDate
        };
        var task = UserImplementation.UpdateUser(user);
        try
        {
            await task;
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Успех", "Данные успешно обновлены\t", ButtonEnum.Ok, Icon.Info);
            messageBox.Show();
            CancellationOperation();
        }
        catch (AuthenticationException ex)
        {
            var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", ex.Message + "\t", ButtonEnum.Ok, Icon.Error);
            messageBox.Show();
        }
    }

    bool GetByLogin(string? login)
    {
        return UserImplementation.GetAllUsers().Result.Any(u => u.Login == login);
    }


}