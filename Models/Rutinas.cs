using SQLite;

public class Rutinas
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string TipoRutina { get; set; }
}
