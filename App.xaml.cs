namespace Rutinas
{
    public partial class App : Application
    {
        // Propiedad estática para acceder a la base desde toda la app
        public static RutinaDatabase Database { get; private set; }

        public App()
        {
            InitializeComponent();

            // Ruta de la base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "rutinas.db3");
            Database = new RutinaDatabase(dbPath);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
