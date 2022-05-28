using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MetalAvaloniaReactive.ViewModels;
using AvaloniaClientMetal.Models;
using AvaloniaClientMVVM.Models;
using MetalAvaloniaReactive.Models;
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
                var users = UserImplementation.GetAllUsers().Result;
                Content = AdminView = new MainAdminViewModel(users, RoleImplementation.GetAllRoles().Result);
                
               
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