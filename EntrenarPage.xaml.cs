namespace Rutinas;

public class Serie
{
    public int Numero { get; set; }
    public int? Repeticiones { get; set; }
    public double? Peso { get; set; }
}

public class Ejercicio
{
    public string Nombre { get; set; }
    public List<Serie> Series { get; set; }
}

public class RutinaDia
{
    public DateTime Fecha { get; set; }
    public string TipoRutina { get; set; }
    public List<Ejercicio> Ejercicios { get; set; }
}


public partial class EntrenarPage : ContentPage
{
    public EntrenarPage()
    {
        InitializeComponent();
        FechaLabel.Text = CapitalizarPrimeraLetra(DateTime.Now.ToString("dddd dd/MM/yyyy"));
        FechaLabel.TextColor = Colors.Black;

        RutinaPicker.ItemsSource = DatosRutinas.Rutinas;
        RutinaPicker.SelectedIndex = -1; // para que no quede seleccionado nada por defecto

    }

    private string CapitalizarPrimeraLetra(string texto)
    {
        if (string.IsNullOrEmpty(texto)) return texto;
        return char.ToUpper(texto[0]) + texto.Substring(1);
    }

    private void OnRutinaSeleccionada(object sender, EventArgs e)
    {
        if (RutinaPicker.SelectedItem == null)
        {
            EjerciciosView.ItemsSource = null;
            return;
        }

        string rutinaSeleccionada = RutinaPicker.SelectedItem.ToString();

        if (DatosRutinas.EjerciciosPorRutina.TryGetValue(rutinaSeleccionada, out var listaEjercicios))
        {
            // Construimos la lista con las series por ejercicio (4 series)
            var listaEjerciciosCompleta = listaEjercicios.Select(ejercicio => new Ejercicio
            {
                Nombre = ejercicio,
                Series = Enumerable.Range(1, 4).Select(i => new Serie { Numero = i }).ToList()
            }).ToList();

            EjerciciosView.ItemsSource = listaEjerciciosCompleta;
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

        var tipoRutina = RutinaPicker.SelectedItem.ToString();
        var fecha = DateTime.Today;

        // Crear y guardar Rutina
        var rutina = new Rutina
        {
            Fecha = fecha,
            TipoRutina = tipoRutina
        };

        await App.Database.GuardarRutinaAsync(rutina);

        // Obtener ejercicios desde el CollectionView
        var ejercicios = EjerciciosView.ItemsSource?.Cast<Ejercicio>().ToList();

        if (ejercicios != null)
        {
            foreach (var ejercicio in ejercicios)
            {
                foreach (var serie in ejercicio.Series)
                {
                    var ejercicioRealizado = new EjercicioRealizado
                    {
                        RutinaId = rutina.Id, // importante: se completa tras insertar Rutina
                        NombreEjercicio = ejercicio.Nombre,
                        Serie = serie.Numero,
                        Repeticiones = serie.Repeticiones,
                        Peso = serie.Peso
                    };

                    await App.Database.GuardarEjercicioAsync(ejercicioRealizado);
                }
            }
        }

        await DisplayAlert("Guardado", "Rutina del día guardada correctamente.", "OK");


        var rutinas = await App.Database.ObtenerRutinasAsync();

        // Volver a la pantalla anterior
        await Navigation.PopAsync();
    }
}