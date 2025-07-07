namespace AppRutinas;

public partial class EntrenarPage : ContentPage
{
    public EntrenarPage()
    {
        InitializeComponent();
        FechaLabel.Text = CapitalizarPrimeraLetra(DateTime.Now.ToString("dddd dd/MM/yyyy"));
        FechaLabel.TextColor = Colors.Black;

        CargarPicker();
        RutinaPicker.SelectedIndex = 0; // para que no quede seleccionado nada por defecto

    }

    private async void CargarPicker()
    {
        var lista = await App.Database.ObtenerRutinasAsync();
        lista.Insert(0, new Rutinas { Nombre = "Seleccionar rutina" });
        RutinaPicker.ItemsSource = lista;
    }

    private string CapitalizarPrimeraLetra(string texto)
    {
        if (string.IsNullOrEmpty(texto)) return texto;
        return char.ToUpper(texto[0]) + texto.Substring(1);
    }

    private async void OnRutinaSeleccionada(object sender, EventArgs e)
    {
        var rutinaSeleccionada = (Rutinas)RutinaPicker.SelectedItem;

        if (rutinaSeleccionada == null || rutinaSeleccionada.Nombre == "Seleccionar rutina")
        {
            EjerciciosView.ItemsSource = null;
            return;
        }

        var ejercicios = await App.Database.ObtenerEjerciciosPorRutinaAsync(rutinaSeleccionada.RutinaId);

        if (ejercicios != null && ejercicios.Any())
        {
            EjerciciosView.ItemsSource = ejercicios;
        }
        else
        {
            EjerciciosView.ItemsSource = null;
        }
    }

    private async void OnGuardarDiaClicked(object sender, EventArgs e)
    {
        if (RutinaPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Seleccioná una rutina antes de guardar.", "OK");
            return;
        }

        //var tipoRutina = RutinaPicker.SelectedItem.ToString();
        //var fecha = DateTime.Today;

        //// Crear y guardar Rutina
        //var rutina = new Rutinas
        //{
        //    Fecha = fecha,
        //    Nombre = tipoRutina
        //};

        //await App.Database.GuardarRutinaAsync(rutina);

        //// Obtener ejercicios desde el CollectionView
        //var ejercicios = EjerciciosView.ItemsSource?.Cast<Ejercicio>().ToList();

        //if (ejercicios != null)
        //{
        //    foreach (var ejercicio in ejercicios)
        //    {
        //        foreach (var serie in ejercicio.Series)
        //        {
        //            var ejercicioRealizado = new EjerciciosRutinas
        //            {
        //                RutinaId = rutina.Id, // importante: se completa tras insertar Rutina
        //                EjercicioId = ejercicio.Nombre,
        //                Serie = serie.Numero,
        //                Repeticiones = serie.Repeticiones,
        //                Peso = serie.Peso
        //            };

        //            await App.Database.GuardarEjercicioAsync(ejercicioRealizado);
        //        }
        //    }
        //}

        await DisplayAlert("Guardado", "Rutina del día guardada correctamente.", "OK");


        // Volver a la pantalla anterior
        await Navigation.PopAsync();
    }
}