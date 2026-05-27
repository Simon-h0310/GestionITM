using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionITM.AppMovil.Models;

namespace GestionITM.AppMovil.ViewModels
{
    // Regla de oro 1: La clase debe ser partial y heredadr de ObservableObject
    public partial class ProfesoresViewModel : ObservableObject
    {
        // Regla de oro 2: Usar ObservableCollection, NUNCA List.
        // Una lista normal no le avisa a la pantalla cuando se agrega un dato.
        // ObservableCollection sí le grita a la pantalla: "¡Me agregaron algo, dibújalo!"
        public ObservableCollection<ProfesorModel> ListaProfesores { get; set; } = new();

        // Regla de oro 3:  Las variables que cambian en pantalla van en MINÚSCULAS
        // y llevan el atributo [ObservableProperty]
        [ObservableProperty]
        private string tituloPantalla = "Directorio de profesores ITM";

        [ObservableProperty]
        private bool estaCargando;

        public ProfesoresViewModel()
        {
            // (En la próxima clase llamaremos a la API aquí)
        }

        // Regla de oro 4:  Los botones no llaman métodos normales , llaman "Comandos".
        // El atributo [RelayCommand] convierte este método en un botón conectable desde la vista.
        [RelayCommand]
        public async Task CargarProfesoresAsync()
        {
            if (EstaCargando) return; // Evita que el usuario hunda el botón 50 veces seguidas.

            EstaCargando = true; // Al cambiar a true, la pantalla va a mostra una ruedita de carga.

            try
            {
                // Simulamos una demora de red (Ir a buscar a la API)
                await Task.Delay(2000);

                // Agregamos datos falsos por el momento
                ListaProfesores.Clear();
                ListaProfesores.Add(new ProfesorModel { Id = 1, Nombre = "Daniel Villamizar", Especialidad = "Backend y Arquitectura Cloud" });
                ListaProfesores.Add(new ProfesorModel { Id = 2, Nombre = "Sara Quimbayo", Especialidad = "Arquitectura" });
                ListaProfesores.Add(new ProfesorModel { Id = 3, Nombre = "Thomas Reyes", Especialidad = "Seguridad" });

                TituloPantalla = $"Se cargaron {ListaProfesores.Count} profesores";
            }
            finally
            {
                EstaCargando = false; // Apagamos la ruedita de carga
            }
        }
    }
}
