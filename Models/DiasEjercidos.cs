using SQLite;

public class DiasEjercidos
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int RutinaId { get; set; }

    public int EjercicioId { get; set; }

    public int Serie { get; set; }

    public int? Repeticiones { get; set; }

    public double? Peso { get; set; }
}
