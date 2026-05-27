using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil.Views
{
    public partial class CursosPage : ContentPage
    {
        public CursosPage(CursosViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is CursosViewModel vm && vm.Cursos.Count == 0)
            {
                vm.CargarCursosCommand.Execute(null);
            }
        }
    }
}