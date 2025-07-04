using SQLite;

public class RutinaDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public RutinaDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Rutina>().Wait();
        _database.CreateTableAsync<EjercicioRealizado>().Wait();
    }

    public Task<int> GuardarRutinaAsync(Rutina rutina)
    {
        return _database.InsertAsync(rutina);
    }

    public Task<int> GuardarEjercicioAsync(EjercicioRealizado ejercicio)
    {
        return _database.InsertAsync(ejercicio);
    }

    public Task<List<Rutina>> ObtenerRutinasAsync()
    {
        return _database.Table<Rutina>().ToListAsync();
    }

    public Task<List<Rutina>> ObtenerRutinasPorFechaAsync()
    {
        DateTime hoy = DateTime.Now.Date;
        DateTime mañana = hoy.AddDays(1);

        return _database.Table<Rutina>()
            .Where(r => r.Fecha >= hoy && r.Fecha < mañana)
            .ToListAsync();
    }

    public Task<List<EjercicioRealizado>> ObtenerEjerciciosPorRutinaAsync(int rutinaId)
    {
        return _database.Table<EjercicioRealizado>()
                        .Where(e => e.RutinaId == rutinaId)
                        .ToListAsync();
    }

    public async Task ExecuteAsync(string query)
    {
        await _database.ExecuteAsync(query);
    }
}
