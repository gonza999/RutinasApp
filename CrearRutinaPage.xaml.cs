using Microsoft.Maui.Controls;

namespace AppRutinas;

public partial class CrearRutinaPage : ContentPage
{
    private List<Ejercicios> ejerciciosDisponibles;

    public CrearRutinaPage()
    {
        InitializeComponent();
        LoadEjercicios();
    }

    private async void LoadEjercicios()
    {
        ejerciciosDisponibles = await App.Database.ObtenerEjerciciosAsync();
        EjerciciosCollection.ItemsSource = ejerciciosDisponibles;
    }

    private async void OnGuardarRutinaClicked(object sender, EventArgs e)
    {
        string nombreRutina = NombreRutinaEntry.Text?.Trim();

        if (string.IsNullOrEmpty(nombreRutina))
        {
            await DisplayAlert("Error", "Ingresá un nombre para la rutina.", "OK");
            return;
        }

        // 1. Insertar la rutina
        var rutina = new Rutinas { Nombre = nombreRutina };
        await App.Database.GuardarRutina(rutina);

        // 2. Obtener la selección
        var ejerciciosSeleccionados = EjerciciosCollection.SelectedItems.Cast<Ejercicios>().ToList();

        // 3. Guardar en tabla intermedia
        foreach (var ejercicio in ejerciciosSeleccionados)
        {
            await App.Database.GuardarEjercicioRutina(new EjerciciosRutinas
            {
                RutinaId = rutina.RutinaId,
                EjercicioId = ejercicio.EjercicioId
            });
        }

        await DisplayAlert("Éxito", "Rutina guardada correctamente.", "OK");
        await Navigation.PopAsync();
    }

    private async void OnAgregarEjercicioClicked(object sender, EventArgs e)
    {
        string nuevoNombre = await DisplayPromptAsync("Nuevo Ejercicio", "Ingresá el nombre del nuevo ejercicio:");

        if (string.IsNullOrWhiteSpace(nuevoNombre))
            return;

        nuevoNombre = nuevoNombre.Trim().ToLowerInvariant();

        // Verificamos si ya existe (ignorando mayúsculas/minúsculas)
        var existente = await App.Database.ObtenerEjercicioPorNombreAsync(nuevoNombre);
        if (existente != null)
        {
            await DisplayAlert("Duplicado", "Ya existe un ejercicio con ese nombre.", "OK");
            return;
        }

        // Crear nuevo ejercicio
        var nuevoEjercicio = new Ejercicios
        {
            Nombre = nuevoNombre.Trim(),
            Imagen = "" // Podés extender esto para agregar imagen en el futuro
        };

        await App.Database.GuardarEjercicio(nuevoEjercicio);

        // Volver a cargar lista de ejercicios
        LoadEjercicios();
    }

}
