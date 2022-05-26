﻿using System;
using System.Collections.Generic;
using System.Text;
using MetalAvaloniaReactive.ViewModels;
using AvaloniaClientMetal.Models;
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
                LoadingApplication();
                Content = Authorization = new AuthorizationViewModel();
               
            }
            catch (ApplicationException ex)
            {
                Content = Authorization = new AuthorizationViewModel();
            }

        }

        public ViewModelBase Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        
        public AuthorizationViewModel Authorization { get; }
        private static void LoadingApplication()
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
    }
}