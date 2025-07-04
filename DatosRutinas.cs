public static class DatosRutinas
{
    public static List<string> Rutinas = new List<string>
    {
        "FullBody",
        "Torso",
        "Pierna"
    };

    public static Dictionary<string, List<string>> EjerciciosPorRutina = new Dictionary<string, List<string>>
{
    { "FullBody", new List<string>
        {
            "Press Banca",
            "Dominadas",
            "Press Militar",
            "Elevaciones Laterales",
            "Extenciones de Codo",
            "Curl con Mancuerna",
            "Sentadilla",
            "Peso Muerto Rumano",
            "Hip Thrust",
            "Elevacion de Talon de Pie (Gemelos)"
        }
    },

    { "Torso", new List<string>
        {
            "Press Banca",
            "Dominadas",
            "Press Militar",
            "Fondos",
            "Remo con Barra",
            "Elevaciones Laterales",
            "Extension de Codo",
            "Curl con Mancuerna"
        }
    },

    { "Pierna", new List<string>
        {
            "Sentadilla",
            "Peso Muerto Rumano",
            "Hip Thrust",
            "Extensiones en Maquina",
            "Curl en Maquina",
            "Elevacion de Talon de Pie (Gemelos)",
            "Elevacion de Talon Sentado"
        }
    }
};

}
