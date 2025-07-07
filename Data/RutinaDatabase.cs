using SQLite;

public class RutinaDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public RutinaDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Rutinas>().Wait();
        _database.CreateTableAsync<EjerciciosRutinas>().Wait();
        _database.CreateTableAsync<Ejercicios>().Wait();
        _database.CreateTableAsync<DiasEjercidos>().Wait();
    }

    public Task<int> GuardarRutinaAsync(Rutinas rutina)
    {
        return _database.InsertAsync(rutina);
    }

    public Task<List<Rutinas>> ObtenerRutinasAsync()
    {
        return _database.Table<Rutinas>().ToListAsync();
    }

    public async Task ExecuteAsync(string query)
    {
        await _database.ExecuteAsync(query);
    }

    public async Task<List<Ejercicios>> ObtenerEjerciciosPorRutinaAsync(int rutinaId)
    {
        var query = @"
        SELECT e.EjercicioId, e.Nombre, e.Imagen
        FROM Ejercicios e
        INNER JOIN EjerciciosRutinas er ON e.EjercicioId = er.EjercicioId
        WHERE er.RutinaId = ?
    ";

        return await _database.QueryAsync<Ejercicios>(query, rutinaId);
    }
}
