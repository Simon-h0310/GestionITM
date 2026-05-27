using GestionITM.AppMovil.ViewModels;

namespace GestionITM.AppMovil.Views;

public partial class ProfesoresPage : ContentPage
{
    // Inyectamos el ViewModel por Constructor, para que la pantalla tenga acceso a los datos y comandos.
    public ProfesoresPage(ProfesoresViewModel viewModel)
    {
     
		InitializeComponent();

        // ESTO ES EL CORDON UMBILICAL ENTRE LA PANTALLA Y EL VIEWMODEL.  Sin esto, la pantalla no sabe de dónde sacar los datos.
        // Le decimos a la página gráfica que su cerebro será este ViewModel que le inyectamos por constructor.
        BindingContext = viewModel;
    }
}