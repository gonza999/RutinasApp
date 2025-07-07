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
        lista.Insert(0, new Rutinas { Nombre = "---Seleccionar rutina---" });
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
        var rutinaSeleccionada = (Rutinas)RutinaPicker.SelectedItem;
        var ejercicios = (List<Ejercicios>)EjerciciosView.ItemsSource;


        if (RutinaPicker.SelectedItem == null || RutinaPicker.SelectedIndex==0)
        {
            await DisplayAlert("Error", "Seleccioná una rutina antes de guardar.", "OK");
            return;
        }

        foreach (var ejercicio in ejercicios)
        {
            foreach (var serie in ejercicio.Series)
            {
                var registro = new DiasEjercidos
                {
                    Fecha = DateTime.Today,
                    RutinaId = rutinaSeleccionada.RutinaId,
                    EjercicioId = ejercicio.EjercicioId,
                    Serie = serie.Numero,
                    Repeticiones = serie.Repeticiones,
                    Peso = serie.Peso
                };

                await App.Database.GuardarDiaEjercido(registro);
            }
        }

        await DisplayAlert("Guardado", "Rutina del día guardada correctamente.", "OK");


        // Volver a la pantalla anterior
        await Navigation.PopAsync();
    }
}