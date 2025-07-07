using Microsoft.Maui.Controls;
using Microsoft.Win32;
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

    public Task<int> GuardarDiaEjercido(DiasEjercidos registro)
    {
        return _database.InsertAsync(registro);
    }

    public Task<List<DiasEjercidos>> ObtenerDiasEjercidosPorFechaAsync()
    {
        DateTime hoy = DateTime.Now.Date;
        DateTime mañana = hoy.AddDays(1);

        return _database.Table<DiasEjercidos>()
            .Where(d => d.Fecha >= hoy && d.Fecha < mañana)
            .ToListAsync();
    }

    public Task<List<DiasEjercidos>> ObtenerDiasEjercidosAsync()
    {
        return _database.Table<DiasEjercidos>().ToListAsync();
    }

    public Task<Rutinas> ObtenerRutinaPorIdAsync(int rutinaId)
    {
        return _database.Table<Rutinas>()
                           .Where(r => r.RutinaId == rutinaId)
                           .FirstOrDefaultAsync();
    }

    public Task<Ejercicios> ObtenerEjercicioPorIdAsync(int ejercicioId)
    {
        return _database.Table<Ejercicios>()
                           .Where(e => e.EjercicioId == ejercicioId)
                           .FirstOrDefaultAsync();
    }

    public Task<List<Ejercicios>> ObtenerEjerciciosAsync()
    {
        return _database.Table<Ejercicios>().ToListAsync();
    }

    public Task<int> GuardarRutina(Rutinas rutina)
    {
        return _database.InsertAsync(rutina);
    }

    public Task<int> GuardarEjercicioRutina(EjerciciosRutinas ejerciciosRutinas)
    {
        return _database.InsertAsync(ejerciciosRutinas);
    }

    public Task<int> GuardarEjercicio(Ejercicios nuevoEjercicio)
    {
        return _database.InsertAsync(nuevoEjercicio);
    }

    public Task<Ejercicios> ObtenerEjercicioPorNombreAsync(string nuevoNombre)
    {
        return _database.Table<Ejercicios>()
                   .Where(e => e.Nombre.ToLower() == nuevoNombre.ToLower())
                   .FirstOrDefaultAsync();
    }
}
