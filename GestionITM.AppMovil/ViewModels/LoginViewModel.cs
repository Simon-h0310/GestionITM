using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionITM.AppMovil.Services;

namespace GestionITM.AppMovil.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Campos vacíos", "OK");
                return;
            }

            IsBusy = true;
            var token = await _apiService.LoginAsync(Email, Password);
            IsBusy = false;

            if (token != null)
            {
                await SecureStorage.SetAsync("jwt_token", token);
                await Shell.Current.GoToAsync("//CursosPage"); // Asumiendo CursosPage como root en appshell
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Credenciales inválidas", "OK");
            }
        }
    }
}