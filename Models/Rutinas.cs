using SQLite;

public class Rutinas
{
    [PrimaryKey, AutoIncrement]
    public int RutinaId { get; set; }

    public string Nombre { get; set; }
}
