using Plugin.Maui.Calendar.Controls;
using Plugin.Maui.Calendar.Enums;
using Plugin.Maui.Calendar.Models;

namespace AppRutinas;

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
        var diasEjercidos = await App.Database.ObtenerDiasEjercidosAsync();

       foreach (var d in diasEjercidos)
       {
           var date = d.Fecha;
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
               eventModel.GetRutina(d.RutinaId);
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
    public Rutinas Rutina { get; set; }
    public List<Ejercicios> Ejercicios { get; set; }

    public void GetEjercicios()
    {
        Ejercicios = new List<Ejercicios>();
        var ejercicios = App.Database.ObtenerEjerciciosPorRutinaAsync(Rutina.RutinaId).Result;
        foreach (var e in ejercicios)
        {
            Ejercicios.Add(e);
        }
    }

    public void GetRutina(int rutinaId)
    {
        Rutina= App.Database.ObtenerRutinaPorIdAsync(rutinaId).Result;
    }
}

