using Plugin.Maui.Calendar.Controls;
using Plugin.Maui.Calendar.Enums;
using Plugin.Maui.Calendar.Models;

namespace Rutinas;

public partial class HistorialPage : ContentPage
{
    public EventCollection EventCollection { get; set; } = new EventCollection();
    public List<DateTime> listaDates { get; set; } = new List<DateTime>();
    public HistorialPage()
    {
        InitializeComponent();
        EventCollection = new EventCollection();
        listaDates = new List<DateTime>();
        IniciarEventos();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        EventCollection = new EventCollection();
        listaDates = new List<DateTime>();
        IniciarEventos();
    }

    private async void IniciarEventos()
    {
        bool canContinue = true;
        EventCollection = new EventCollection();
        listaDates = new List<DateTime>();
        var rutinas = await App.Database.ObtenerRutinasAsync();

        foreach (var r in rutinas)
        {
            var date = r.Fecha;
            if (listaDates.Contains(date.Date))
            {
                canContinue = false;
            }
            else
            {
                listaDates.Add(date.Date);
                canContinue = true;
            }
            if (canContinue)
            {
                var eventModel = new EventModel();
                eventModel.TipoRutina = r.TipoRutina;
                eventModel.RutinaId = r.Id;
                eventModel.GetEjercicios();
                EventCollection.Add(date, new List<EventModel>
                {
                   eventModel
                });
            }
        }

        BindingContext = this;
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        // Volver a la pantalla anterior
        await Navigation.PopAsync();
    }
}

public class EventModel
{
    public string TipoRutina { get; set; }

    public int RutinaId { get; set; }

    public List<EjercicioRealizado> Ejercicios { get; set; }

    public void GetEjercicios()
    {
        Ejercicios = new List<EjercicioRealizado>();
        var ejercicios = App.Database.ObtenerEjerciciosPorRutinaAsync(RutinaId).Result;
        foreach (var e in ejercicios)
        {
            Ejercicios.Add(e);
        }
    }
}

