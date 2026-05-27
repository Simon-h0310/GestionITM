namespace GestionITM.AppMovil
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("CursosPage", typeof(Views.CursosPage));
        }
    }
}
