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

        foreach (var g in diasEjercidos.GroupBy(d => d.Fecha.Date))
        {
            var date = g.Key;
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
                eventModel.GetRutina(g.FirstOrDefault().RutinaId);
                eventModel.GetEjercicios(diasEjercidos);
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

    public async void GetEjercicios(List<DiasEjercidos> diasEjercidos)
    {
        Ejercicios = new List<Ejercicios>();
        var ejercicios = await App.Database.ObtenerEjerciciosPorRutinaAsync(Rutina.RutinaId);
        foreach (var e in ejercicios)
        {
            foreach (var s in e.Series)
            {
                s.Peso = diasEjercidos
                    .Where(d => d.EjercicioId == e.EjercicioId && d.Serie == s.Numero)
                    .Select(d => d.Peso)
                    .FirstOrDefault();
                s.Repeticiones = diasEjercidos
                    .Where(d => d.EjercicioId == e.EjercicioId && d.Serie == s.Numero)
                    .Select(d => d.Repeticiones)
                    .FirstOrDefault();
            }
            Ejercicios.Add(e);
        }
    }

    public void GetRutina(int rutinaId)
    {
        Rutina = App.Database.ObtenerRutinaPorIdAsync(rutinaId).Result;
    }
}

