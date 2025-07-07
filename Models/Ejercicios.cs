using SQLite;

public class Ejercicios
{
    [PrimaryKey, AutoIncrement]
    public int EjercicioId { get; set; }

    public string Nombre { get; set; }

    public string Imagen { get; set; } // Por ejemplo: "press_banca.png"

    [Ignore] // Esta propiedad no está en la base de datos, es solo para mostrar
    public List<int> Series => new List<int> { 1, 2, 3, 4 };
}
