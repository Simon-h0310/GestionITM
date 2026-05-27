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
                await Application.Current!.MainPage!.DisplayAlert("Error", "Por favor ingresa tu correo y contrasena.", "OK");
                return;
            }

            // Validar formato de email
            if (!Email.Contains("@") || !Email.Contains("."))
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Por favor ingresa un correo valido.", "OK");
                return;
            }

            IsBusy = true;
            
            try
            {
                var token = await _apiService.LoginAsync(Email, Password);

                if (token != null)
                {
                    await SecureStorage.SetAsync("jwt_token", token);
                    await Shell.Current.GoToAsync("//CursosPage");
                }
                else
                {
                    await Application.Current!.MainPage!.DisplayAlert("Error", "Credenciales invalidas. Verifica tu correo y contrasena.", "OK");
                }
            }
            catch (HttpRequestException)
            {
                await Application.Current!.MainPage!.DisplayAlert("Sin Conexion", "No se pudo conectar al servidor. Verifica tu conexion a internet.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Ocurrio un error inesperado: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
