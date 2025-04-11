using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using SisGestorEmpenio.Modelos;
namespace SisGestorEmpenio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // Este método se ejecuta cuando se inicia la aplicación
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Puedes hacer inicializaciones aquí, como:
            // - Inyectar dependencias
            // - Cambiar temas
            // - Mostrar una ventana diferente a MainWindow
            // - Manejar archivos de configuración

            // Ejemplo: abrir una ventana distinta
            // var login = new LoginWindow();
            // login.Show();
        }

        // También puedes manejar errores no controlados
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Mostrar mensaje de error
            MessageBox.Show("Ocurrió un error inesperado: " + e.Exception.Message);
            e.Handled = true;
        }
    }

}
