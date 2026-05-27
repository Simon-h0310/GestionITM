using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionITM.AppMovil.Services;

namespace GestionITM.AppMovil.ViewModels
{
    public partial class CursosViewModel : ObservableObject
    {
        private readonly CursoService _cursoService;
        private readonly MatriculaService _matriculaService;

        public ObservableCollection<CursoDto> Cursos { get; } = new();

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool noHayMasDatos;

        private int _currentPage = 1;
        private const int PageSize = 10;
        private bool _hasMoreData = true;

        public CursosViewModel(CursoService cursoService, MatriculaService matriculaService)
        {
            _cursoService = cursoService;
            _matriculaService = matriculaService;
        }

        [RelayCommand]
        private async Task CargarCursosAsync()
        {
            if (IsBusy || !_hasMoreData) return;

            IsBusy = true;

            try
            {
                var result = await _cursoService.GetCursosPaginadosAsync(_currentPage, PageSize);
                if (result != null && result.Items.Any())
                {
                    foreach (var curso in result.Items)
                    {
                        Cursos.Add(curso);
                    }
                    _currentPage++;
                    if (result.Items.Count < PageSize)
                    {
                        _hasMoreData = false;
                        NoHayMasDatos = true;
                    }
                }
                else
                {
                    _hasMoreData = false;
                    NoHayMasDatos = true;
                }
            }
            catch (HttpRequestException)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Sin Conexion", 
                    "No se pudo conectar al servidor. Verifica tu conexion a internet.", 
                    "OK");
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Error", 
                    $"Ocurrio un error al cargar los cursos: {ex.Message}", 
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RefrescarCursosAsync()
        {
            if (IsBusy) return;

            IsRefreshing = true;

            // Reiniciar estado
            _currentPage = 1;
            _hasMoreData = true;
            NoHayMasDatos = false;
            Cursos.Clear();

            await CargarCursosAsync();

            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task MatricularAsync(CursoDto curso)
        {
            if (curso == null) return;

            if (curso.CuposDisponibles <= 0)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Sin Cupos", 
                    "Este curso no tiene cupos disponibles.", 
                    "OK");
                return;
            }

            bool isConfirmed = await Application.Current!.MainPage!.DisplayAlert(
                "Confirmar Matricula", 
                $"Deseas matricularte en {curso.Nombre}?", 
                "Si", "No");

            if (!isConfirmed) return;

            IsBusy = true;
            
            try
            {
                // Periodo actual hardcodeado para el taller
                var (isSuccess, message) = await _matriculaService.CrearMatriculaAsync(curso.Id, "2026-1");

                if (isSuccess)
                {
                    await Application.Current!.MainPage!.DisplayAlert("Exito", message, "OK");
                    // Actualizar cupos localmente
                    curso.CuposDisponibles--;
                }
                else
                {
                    // Mostrar el mensaje de error del backend (ej: "No hay cupos disponibles")
                    await Application.Current!.MainPage!.DisplayAlert("Error", message, "OK");
                }
            }
            catch (HttpRequestException)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Sin Conexion", 
                    "No se pudo conectar al servidor. Verifica tu conexion a internet.", 
                    "OK");
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Error", 
                    $"Ocurrio un error inesperado: {ex.Message}", 
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CerrarSesionAsync()
        {
            SecureStorage.Remove("jwt_token");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
