using SQLite;

public class RutinaDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public RutinaDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Rutinas>().Wait();
        _database.CreateTableAsync<EjerciciosRealizados>().Wait();
    }

    public Task<int> GuardarRutinaAsync(Rutinas rutina)
    {
        return _database.InsertAsync(rutina);
    }

    public Task<int> GuardarEjercicioAsync(EjerciciosRealizados ejercicio)
    {
        return _database.InsertAsync(ejercicio);
    }

    public Task<List<Rutinas>> ObtenerRutinasAsync()
    {
        return _database.Table<Rutinas>().ToListAsync();
    }

    public Task<List<Rutinas>> ObtenerRutinasPorFechaAsync()
    {
        DateTime hoy = DateTime.Now.Date;
        DateTime mañana = hoy.AddDays(1);

        return _database.Table<Rutinas>()
            .Where(r => r.Fecha >= hoy && r.Fecha < mañana)
            .ToListAsync();
    }

    public Task<List<EjerciciosRealizados>> ObtenerEjerciciosPorRutinaAsync(int rutinaId)
    {
        return _database.Table<EjerciciosRealizados>()
                        .Where(e => e.RutinaId == rutinaId)
                        .ToListAsync();
    }

    public async Task ExecuteAsync(string query)
    {
        await _database.ExecuteAsync(query);
    }
}
