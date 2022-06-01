﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MetalAvaloniaReactive.ViewModels;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MetalAvaloniaReactive.Models;
using MetalAvaloniaReactive.Views;
using ReactiveUI;

namespace MetalAvaloniaReactive.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        public MainWindowViewModel()
        {

            try
            {
                /*PreparedLocalStorage.ClearLocalStorage();
                PreparedLocalStorage.SaveLocalStorage();*/
                LoadingApplication();
                if (KeepRoleId.RoleId == 1)
                {
                    Content = AdminView = new MainAdminViewModel(UserImplementation.GetAllUsers().Result, RoleImplementation.GetAllRoles().Result, this);
                }
                else
                {
                    Content = AdminView = new MainAdminViewModel(UserImplementation.GetAllUsers().Result, RoleImplementation.GetAllRoles().Result, this);
                }



            }
            catch (ApplicationException ex)
            {
                Content = Authorization = new AuthorizationViewModel(this);
                
                //Observable.Merge(Authorization.AuthorizationButtonClick.Select());
                //Content = AdminView = new MainAdminViewModel(UserImplementation.GetAllUsers().Result, RoleImplementation.GetAllRoles().Result);
            }

        }

        public ViewModelBase Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        
        public AuthorizationViewModel Authorization { get; }
        
        public MainAdminViewModel AdminView { get; }
        

        private void LoadingApplication()
        {
            try
            {
                PreparedLocalStorage.LoadLocalStorage();
                TokenPair tokenPair = PreparedLocalStorage.GetTokenPairFromLocalStorage();
                var tokenPair1 = UserImplementation.RefreshTokenPair(tokenPair.RefreshToken);
                PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair1.Result);
                KeepRoleId.RoleId = tokenPair1.Result.IdRole;
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        /*public void AuthorizationButtonClick()
        {
            UserImplementation.UserAuthorization();
            var vm = new MainAdminView();
        }*/
    }
}