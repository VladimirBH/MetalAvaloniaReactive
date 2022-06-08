using System;
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
                Content = AdminView = new MainAdminViewModel(this, KeepRoleId.RoleId == 1);
            }
            catch (AggregateException ex)
            {
                Content = new ConnectionErrorViewModel();
            }
            catch (Exception ex)
            {
                Content = Authorization = new AuthorizationViewModel(this);
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
            PreparedLocalStorage.LoadLocalStorage();
            TokenPair tokenPair = PreparedLocalStorage.GetTokenPairFromLocalStorage();
            var tokenPair1 = UserImplementation.RefreshTokenPair(tokenPair.RefreshToken);
            PreparedLocalStorage.PutTokenPairFromLocalStorage(tokenPair1.Result);
            KeepRoleId.RoleId = tokenPair1.Result.IdRole;
        }
    }
}