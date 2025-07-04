using SQLite;

public class EjercicioRealizado
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int RutinaId { get; set; }

    public string NombreEjercicio { get; set; }

    public int Serie { get; set; }

    public int? Repeticiones { get; set; }

    public double? Peso { get; set; }
}
