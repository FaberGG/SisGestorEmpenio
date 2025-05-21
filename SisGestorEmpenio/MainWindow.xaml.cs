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
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBlock textoSeleccionadoActual = null;
        private Administrador admin;

        public MainWindow()
        {
            InitializeComponent();

            // Obtener administrador activo
            admin = Sesion.Sesion.GetAdministradorActivo();

            // Cargar vista de inicio
            MainContent.Content = new vistas.HomeView();
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
        

        private void GoToHome(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new vistas.HomeView();
        }

       

        // ARTICULO
        private void GoToRegistrarArticulo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarArticulo);
            ActualizarEncabezado("Registrar Artículo");
            MainContent.Content = new ArticuloView();
        }



        // PRESTAMO
        private void GoToRegistrarPrestamo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarPrestamo);
            ActualizarEncabezado("Registrar Préstamo");

            Cliente clienteRegistrado = null;
            Articulo articuloRegistrado = null;

            var confirmacionCliente = new ConfirmacionWindow
            {
                Mensaje = "¿El cliente ya está registrado?",
                Titulo = "Cliente registrado?",
                TextoBotonIzquierdo = "Registrar",
                TextoBotonDerecho = "Si",
                MostrarBotonDerecho = true
            };

            bool? resultadoCliente = confirmacionCliente.ShowDialog();

            if (resultadoCliente == true && confirmacionCliente.Confirmado)
            {
                //pregunto si el articulo ya esta registrado
                var confirmacionArticulo = new ConfirmacionWindow
                {
                    Mensaje = "¿El articulo ya está registrado?",
                    Titulo = "Articulo registrado?",
                    TextoBotonIzquierdo = "Registrar",
                    TextoBotonDerecho = "Si",
                    MostrarBotonDerecho = true
                };
                bool? resultadoArticulo = confirmacionArticulo.ShowDialog();

                if (resultadoArticulo == true && confirmacionArticulo.Confirmado)
                {
                    ActualizarEncabezado("Registrar Préstamo");
                    MainContent.Content = new PrestamoView();
                }
                else
                {
                    ActualizarEncabezado("Registrar Préstamo", "Artículo");
                    var vistaArticulo = new ArticuloView();
                    vistaArticulo.RegistroArticuloCompletado += (s, articulo) =>
                    {
                        articuloRegistrado = articulo;
                        ActualizarEncabezado("Registrar Préstamo");
                        MainContent.Content = new PrestamoView(articuloRegistrado);
                    };
                    MainContent.Content = vistaArticulo;
                }
            }
            else
            {
                ActualizarEncabezado("Registrar Préstamo", "Cliente");
                var vistaCliente = new ClienteView();
                vistaCliente.RegistroClienteCompletado += (s1, cliente) =>
                {
                    clienteRegistrado = cliente;
                    ActualizarEncabezado("Registrar Préstamo", "Artículo");
                    var vistaArticulo = new ArticuloView();
                    vistaArticulo.RegistroArticuloCompletado += (s2, articulo) =>
                    {
                        articuloRegistrado = articulo;
                        ActualizarEncabezado("Registrar Préstamo");

                        MainContent.Content = new PrestamoView(cliente, articulo);
                    };
                    MainContent.Content = vistaArticulo;
                };
                MainContent.Content = vistaCliente;
            }
        }
        // Modificar Prestamo
        private void GoToModificarPrestamo(object sender, MouseButtonEventArgs e)
        {
            

            BuscarPrestamoPorIdWindow buscarPrestamo = new BuscarPrestamoPorIdWindow();
            bool? resultado = buscarPrestamo.ShowDialog();

            if(resultado == true)
            {
                SeleccionarOpcion(TxtModificarPrestamo);
                ActualizarEncabezado("Modificar Prestamo");
                Prestamo prestamo = buscarPrestamo.Prestamo;
                if (prestamo != null)
                {
                    ActualizarEncabezado("Modificar Prestamo");
                    MainContent.Content = new ModificarPrestamoView(prestamo);
                }
            }
            
        }

        private void GoToRegistrarDevolucion(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarDevolucion);
            ActualizarEncabezado("Registrar Devolución");
            // Antes: new RegistrarDevolucion()
            MainContent.Content = new DevolucionView();    // ← Aquí instanciamos el nuevo control en modo “crear”
        }

        private void GoToModificarDevolucion(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtModificarDevolucion);

            var buscarWin = new BuscarDevolucionPorIdWindow();
            bool? resultado = buscarWin.ShowDialog();
            if (resultado != true || buscarWin.DevolucionSeleccionada == null)
                return;

            ActualizarEncabezado("Modificar Devolución");
            // Antes: new ModificarDevolucion(buscarWin.DevolucionSeleccionada)
            MainContent.Content = new DevolucionView(buscarWin.DevolucionSeleccionada);  // ← Aquí en modo “editar”
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

        private void GoToModificarArticulo(object sender, MouseButtonEventArgs e)
        {
            var buscarArticulo = new BuscarArticuloPorId();
            bool? resultado = buscarArticulo.ShowDialog();

            if (resultado == true)
            {
                SeleccionarOpcion(TxtModificarArticulo);
                ActualizarEncabezado("Modificar Artículo");
                var articulo = buscarArticulo.Articulo;
                if (articulo != null)
                {
                    MainContent.Content = new ArticuloView(articulo);
                }
            }
        }


        private void GoToConsultarPrestamo(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SeleccionarOpcion(TxtConsultarPrestamo);
                ActualizarEncabezado("Consultar Préstamo");

                // Crear y suscribir evento
                var consulta = new ConsultarPrestamos();
                consulta.PrestamoSeleccionado += OnPrestamoSeleccionado;
                MainContent.Content = consulta;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la vista de consulta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Manejador del evento lanzado por ConsultarPrestamos
        private void OnPrestamoSeleccionado(object sender, int idArticulo)
        {
            // Buscar el préstamo completo
            var prestamo = admin.ObtenerTodosPrestamos()
                                 .FirstOrDefault(p => p.GetArticulo().GetIdArticulo() == idArticulo);
            if (prestamo == null) return;

            // Crear vista de detalles y cargar datos
            var detalles = new DetallesPrestamo();
            detalles.CargarDatos(prestamo);

            // Navegar a detalles
            MainContent.Content = detalles;
        }



    }
}