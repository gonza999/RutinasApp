using SQLite;

public class Ejercicios
{
    [PrimaryKey, AutoIncrement]
    public int EjercicioId { get; set; }

    public string Nombre { get; set; }

    public string Imagen { get; set; } // Por ejemplo: "press_banca.png"

    [Ignore]
    public List<Serie> Series { get; set; }
        = new List<Serie> {
    new Serie() { Numero = 1, Repeticiones = 0, Peso = 0 },
    new Serie() { Numero = 2, Repeticiones = 0, Peso = 0 },
    new Serie() { Numero = 3, Repeticiones = 0, Peso = 0 },
    new Serie() { Numero = 4, Repeticiones = 0, Peso = 0 }
    };

}

public class Serie
{
    public int Numero { get; set; }
    public int? Repeticiones { get; set; }
    public double? Peso { get; set; }
}
