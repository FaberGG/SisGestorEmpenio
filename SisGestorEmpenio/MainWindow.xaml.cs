using SisGestorEmpenio.vistas;
using SisGestorEmpenio.Sesion;
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
        //para el menu
        private TextBlock textoSeleccionadoActual = null;

        public MainWindow()
        {
            InitializeComponent();
            // Por ejemplo, cargar la vista de inicio
            MainContent.Content = new vistas.HomeView(); // Asegúrate de que esta clase exista
        }

        private void CerrarSesion_Click(object sender, MouseButtonEventArgs e)
        {
            // Cerrar la sesión
            Sesion.Sesion.CerrarSesion();
            // Crear nueva instancia de la ventana de login
            var login = new LogInWindow(); 
            login.Show();

            // Cerrar la ventana actual
            this.Close();
        }

        private void SeleccionarOpcion(TextBlock nuevoTexto)
        {
            if (textoSeleccionadoActual != null)
                textoSeleccionadoActual.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#83C4C4"));

            nuevoTexto.Foreground = Brushes.White;
            textoSeleccionadoActual = nuevoTexto;
        }

        private void ActualizarEncabezado(string titulo, string subtitulo = "")
        {
            TxtTitulo.Text = titulo;
            TxtSubtitulo.Text = subtitulo;
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

       

        // ARTICULO
        private void GoToRegistrarArticulo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarArticulo);
            ActualizarEncabezado("Registrar Artículo");
            MainContent.Content = new RegistrarArticulo();
        }



        // PRESTAMO
        private void GoToRegistrarPrestamo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarPrestamo);
            ActualizarEncabezado("Registrar Préstamo");

            var result = MessageBox.Show(
                "¿El cliente ya está registrado?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                ActualizarEncabezado("Registrar Préstamo", "Artículo");
                var vistaArticulo = new RegistrarArticulo();
                vistaArticulo.RegistroArticuloCompletado += (s, args) =>
                {
                    ActualizarEncabezado("Registrar Préstamo");
                    MainContent.Content = new RegistrarPrestamo();
                };
                MainContent.Content = vistaArticulo;
            }
            else
            {
                ActualizarEncabezado("Registrar Préstamo", "Cliente");
                var vistaCliente = new RegistrarCliente();
                vistaCliente.RegistroClienteCompletado += (s1, args1) =>
                {
                    ActualizarEncabezado("Registrar Préstamo", "Artículo");
                    var vistaArticulo = new RegistrarArticulo();
                    vistaArticulo.RegistroArticuloCompletado += (s2, args2) =>
                    {
                        ActualizarEncabezado("Registrar Préstamo");
                        MainContent.Content = new RegistrarPrestamo();
                    };
                    MainContent.Content = vistaArticulo;
                };
                MainContent.Content = vistaCliente;
            }
        }

        private void GoToRegistrarDevolucion(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarDevolucion);
            ActualizarEncabezado("Registrar Devolución");
            MainContent.Content = new RegistrarDevolucion();
        }

        private void ToggleGestPrestamo_Click(object sender, MouseButtonEventArgs e)
        {
            bool estaVisible = OpcionesPrestamos.Visibility == Visibility.Visible;

            OpcionesPrestamos.Visibility = estaVisible ? Visibility.Collapsed : Visibility.Visible;
            PrestamoBotonPrincipal.Background = new SolidColorBrush(
                estaVisible ? Colors.Transparent : Color.FromRgb(0x59, 0x91, 0x91)
            );

            //Rota la flecha
            PrestamoFlechaTransform.Angle = estaVisible ? 90 : -90;
        }

        private void ToggleGestDevolucion_Click(object sender, MouseButtonEventArgs e)
        {
            bool estaVisible = OpcionesDevoluciones.Visibility == Visibility.Visible;

            OpcionesDevoluciones.Visibility = estaVisible ? Visibility.Collapsed : Visibility.Visible;
            DevolucionBotonPrincipal.Background = new SolidColorBrush(
                estaVisible ? Colors.Transparent : Color.FromRgb(0x59, 0x91, 0x91)
            );

            //Rota la flecha
            DevolucionFlechaTransform.Angle = estaVisible ? 90 : -90;
        }
        private void ToggleGestInventario_Click(object sender, MouseButtonEventArgs e)
        {
            bool estaVisible = OpcionesArticulos.Visibility == Visibility.Visible;

            OpcionesArticulos.Visibility = estaVisible ? Visibility.Collapsed : Visibility.Visible;
            ArticuloBotonPrincipal.Background = new SolidColorBrush(
                estaVisible ? Colors.Transparent : Color.FromRgb(0x59, 0x91, 0x91)
            );

            //Rota la flecha
            ArticuloFlechaTransform.Angle = estaVisible ? 90 : -90;
        }
    }
}