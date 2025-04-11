using SisGestorEmpenio.vistas;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SisGestorEmpenio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Por ejemplo, cargar la vista de inicio
            MainContent.Content = new vistas.HomeView(); // Asegúrate de que esta clase exista
        }

        // Método para cambiar la vista en el ContentControl
        public void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Home":
                    MainContent.Content = new vistas.HomeView();
                    break;
                case "RegistrarCliente":
                    MainContent.Content = new vistas.RegistrarCliente();
                    break;
                case "RegistrarArticulo":
                    MainContent.Content = new vistas.RegistrarArticulo();
                    break;
                case "RegistrarPrestamo":
                    MainContent.Content = new vistas.RegistrarPrestamo();
                    break;
                case "RegistrarDevolucion":
                    MainContent.Content = new vistas.RegistrarDevolucion();
                    break;

            }
        }

        private void GoToHome(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new vistas.HomeView();
        }

        private void GoToRegistrarCliente(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new vistas.RegistrarCliente();
        }

        private void GoToRegistrarArticulo(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new vistas.RegistrarArticulo();
        }
        private void GoToRegistrarPrestamo(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "¿El cliente ya está registrado?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                // Cliente registrado: primero registrar artículo, luego préstamo
                MainContent.Content = new RegistrarArticulo();
                // Suscribimos un evento para continuar con préstamo luego
                ((RegistrarArticulo)MainContent.Content).RegistroArticuloCompletado += (s, args) =>
                {
                    MainContent.Content = new RegistrarPrestamo();
                };
            }
            else
            {
                // Cliente no registrado: primero registrar cliente
                MainContent.Content = new RegistrarCliente();
                ((RegistrarCliente)MainContent.Content).RegistroClienteCompletado += (s1, args1) =>
                {
                    MainContent.Content = new RegistrarArticulo();
                    ((RegistrarArticulo)MainContent.Content).RegistroArticuloCompletado += (s2, args2) =>
                    {
                        MainContent.Content = new RegistrarPrestamo();
                    };
                };
            }
        }

        private void GoToRegistrarDevolucion(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new vistas.RegistrarDevolucion();
        }

    }
}