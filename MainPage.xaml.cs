namespace AppRutinas
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        bool canTraining = true;
        bool canHistorial = true;
        public MainPage()
        {
            InitializeComponent();
            ValidarButtonEntrenar();
            ValidarButtonHistorial();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ValidarButtonEntrenar();
            ValidarButtonHistorial();
        }

        public async void ValidarButtonEntrenar()
        {
            var rutinas = await App.Database.ObtenerRutinasPorFechaAsync();
            if (rutinas.Count>0)
            {
                canTraining = false;
            }
            else
            {
                canTraining = true;
            }
            EntrenarButton.IsEnabled = canTraining;
        }
        public async void ValidarButtonHistorial()
        {
            var rutinas = await App.Database.ObtenerRutinasAsync();
            if (rutinas.Count > 0)
            {
                canHistorial = true;
            }
            else
            {
                canHistorial = false;
            }
            HistorialButton.IsEnabled = canHistorial;
        }

        private async void OnHistorialClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistorialPage());
        }

        private async void OnEntrenarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntrenarPage());
        }

        private async void OnBorrarDbClicked(object sender, EventArgs e)
        {
            bool confirmado = await DisplayAlert("Confirmar",
                "¿Estás seguro de que querés borrar todos los datos de la base?",
                "Sí, borrar",
                "Cancelar");

            if (confirmado)
            {
                try
                {
                    // Ejecutar los DELETE
                    await App.Database.ExecuteAsync("DELETE FROM Rutina");
                    await App.Database.ExecuteAsync("DELETE FROM sqlite_sequence");
                    await App.Database.ExecuteAsync("DELETE FROM EjercicioRealizado");

                    await DisplayAlert("Éxito", "La base de datos fue vaciada correctamente.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                }
            }
        }

    }

}
