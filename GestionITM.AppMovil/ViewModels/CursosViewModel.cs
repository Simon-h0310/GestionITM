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
                }
            }
            else
            {
                _hasMoreData = false;
            }

            IsBusy = false;
        }

        [RelayCommand]
        private async Task RefrescarCursosAsync()
        {
            if (IsBusy) return;

            IsRefreshing = true;

            // Reiniciar estado
            _currentPage = 1;
            _hasMoreData = true;
            Cursos.Clear();

            await CargarCursosAsync();

            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task MatricularAsync(CursoDto curso)
        {
            if (curso == null) return;

            bool isConfirmed = await Application.Current!.MainPage!.DisplayAlert(
                "Confirmar", 
                $"¿Deseas matricularte en {curso.Nombre}?", 
                "Sí", "No");

            if (!isConfirmed) return;

            IsBusy = true;
            // Periodo harcodeado por ahora
            var (isSuccess, message) = await _matriculaService.CrearMatriculaAsync(curso.Id, "2026-1");
            IsBusy = false;

            if (isSuccess)
            {
                await Application.Current!.MainPage!.DisplayAlert("Éxito", message, "OK");
                // Podríamos actualizar cupos locales, o recargar
                curso.CuposDisponibles--;
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", message, "OK");
            }
        }
    }
}