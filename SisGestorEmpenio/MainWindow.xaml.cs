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
using System.Numerics;

namespace SisGestorEmpenio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBlock textoSeleccionadoActual = null;
        private Administrador admin;
        // Color para hover
        private readonly Color hoverColor = Color.FromRgb(0x44, 0x86, 0x86);
        private readonly Color activeColor = Color.FromRgb(0x44, 0x86, 0x86); // #448686

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


        // Método para mostrar vista con encabezado
        public void ShowViewWithHeader(UserControl content, string titulo = "", string subtitulo = "")
        {
            // Configurar el encabezado
            if (!string.IsNullOrEmpty(titulo))
                TxtTitulo.Text = titulo;
            if (!string.IsNullOrEmpty(subtitulo))
                TxtSubtitulo.Text = subtitulo;

            // Mostrar el encabezado
            HeaderPanel.Visibility = Visibility.Visible;
            HeaderRow.Height = new GridLength(107);

            // Configurar el contenido con margen normal
            MainContent.Margin = new Thickness(0, 10, 0, 0);
            MainContent.Content = content;
        }

        // Método para mostrar vista sin encabezado (pantalla completa)
        public void ShowFullScreenView(UserControl content)
        {
            // Ocultar el encabezado
            HeaderPanel.Visibility = Visibility.Collapsed;
            HeaderRow.Height = new GridLength(0);

            // Configurar el contenido para ocupar toda la pantalla
            MainContent.Margin = new Thickness(0);
            MainContent.Content = content;
        }

        // Método para alternar entre modos
        public void ToggleHeaderVisibility(bool showHeader)
        {
            if (showHeader)
            {
                HeaderPanel.Visibility = Visibility.Visible;
                HeaderRow.Height = new GridLength(107);
                MainContent.Margin = new Thickness(0, 10, 0, 0);
            }
            else
            {
                HeaderPanel.Visibility = Visibility.Collapsed;
                HeaderRow.Height = new GridLength(0);
                MainContent.Margin = new Thickness(0);
            }
        }



        // Método para cambiar la vista en el ContentControl


        private void GoToHome(object sender, EventArgs e)
        {

            var homeView = new vistas.HomeView();
            ShowViewWithHeader(homeView, "Bienvenido administrador a J-LHYS", "Sistema gestor de la casa de empeños");
        }



        // ARTICULO
        private void GoToRegistrarArticulo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarArticulo);
            var articuloView = new ArticuloView();
            ShowViewWithHeader(articuloView, "Registrar Artículo");
        }



        // PRESTAMO
        private void GoToRegistrarPrestamo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarPrestamo);

            var confirmacionCliente = new ConfirmacionWindow
            {
                Mensaje = "¿El cliente ya está registrado?",
                Titulo = "Cliente registrado?",
                TextoBotonIzquierdo = "Registrar",
                TextoBotonDerecho = "Si",
                MostrarBotonDerecho = true
            };
            bool? resultadoCliente = confirmacionCliente.ShowDialog();
            bool clienteYaRegistrado = resultadoCliente == true && confirmacionCliente.Confirmado;
            bool articuloYaRegistrado = false;

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
            articuloYaRegistrado = resultadoArticulo == true && confirmacionArticulo.Confirmado;

            //muestro la vista de registrar prestamo
            var prestamoView = new RegistrarPrestamoView(clienteYaRegistrado, articuloYaRegistrado);
            //suscribo eventos para completar o cancelar el proceso
            prestamoView.ProcesoCompletado += GoToHome;
            prestamoView.ProcesoCancelado += ProcesoCancelado;
            ShowFullScreenView(prestamoView);
        }

        private void ProcesoCancelado(object sender, EventArgs e)
        {
            // El usuario canceló el proceso
            // Cambiar a HomeView
            GoToHome(sender, e);

            new MensajeErrorOk
            {
                Mensaje = "Proceso cancelado con éxito... \n Volviendo a la pantalla principal",
                Titulo = "Proceso cancelado",
                TextoBotonIzquierdo = "Entendido"
            }.ShowDialog();
        }


        // Modificar Prestamo
        private void GoToModificarPrestamo(object sender, MouseButtonEventArgs e)
        {


            BuscarPrestamoPorIdWindow buscarPrestamo = new BuscarPrestamoPorIdWindow();
            bool? resultado = buscarPrestamo.ShowDialog();

            if (resultado == true)
            {
                SeleccionarOpcion(TxtModificarPrestamo);
                Prestamo prestamo = buscarPrestamo.Prestamo;
                if (prestamo != null)
                {
                    var prestamoView = new ModificarPrestamoView(prestamo);
                    ShowViewWithHeader(prestamoView, "Modificar Préstamo");
                }
            }

        }

        private void GoToRegistrarDevolucion(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtRegistrarDevolucion);
            // Antes: new RegistrarDevolucion()
            var devolucionView = new DevolucionView();    // ← Aquí instanciamos el nuevo control en modo “crear”
            ShowViewWithHeader(devolucionView, "Registrar Devolución");
        }

        private void GoToModificarDevolucion(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtModificarDevolucion);

            var buscarWin = new BuscarDevolucionPorIdWindow();
            bool? resultado = buscarWin.ShowDialog();
            if (resultado != true || buscarWin.DevolucionSeleccionada == null)
                return;

            // Antes: new ModificarDevolucion(buscarWin.DevolucionSeleccionada)
            var devolucionView = new DevolucionView(buscarWin.DevolucionSeleccionada);  // ← Aquí en modo “editar”
            ShowViewWithHeader(devolucionView, "Modificar Devolución");
        }


        // EVENTOS HOVER PARA PRÉSTAMOS
        private void PrestamoBotonPrincipal_MouseEnter(object sender, MouseEventArgs e)
        {
            // Solo aplica hover si está colapsado (fondo transparente)
            bool estaColapsado = OpcionesPrestamos.Visibility == Visibility.Collapsed;
            if (estaColapsado)
            {
                PrestamoBotonPrincipal.Background = new SolidColorBrush(hoverColor);
            }
        }

        private void PrestamoBotonPrincipal_MouseLeave(object sender, MouseEventArgs e)
        {
            // Solo quita hover si está colapsado
            bool estaColapsado = OpcionesPrestamos.Visibility == Visibility.Collapsed;
            if (estaColapsado)
            {
                PrestamoBotonPrincipal.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        // EVENTOS HOVER PARA DEVOLUCIONES
        private void DevolucionBotonPrincipal_MouseEnter(object sender, MouseEventArgs e)
        {
            bool estaColapsado = OpcionesDevoluciones.Visibility == Visibility.Collapsed;
            if (estaColapsado)
            {
                DevolucionBotonPrincipal.Background = new SolidColorBrush(hoverColor);
            }
        }

        private void DevolucionBotonPrincipal_MouseLeave(object sender, MouseEventArgs e)
        {
            bool estaColapsado = OpcionesDevoluciones.Visibility == Visibility.Collapsed;
            if (estaColapsado)
            {
                DevolucionBotonPrincipal.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        // EVENTOS HOVER PARA INVENTARIO
        private void ArticuloBotonPrincipal_MouseEnter(object sender, MouseEventArgs e)
        {
            bool estaColapsado = OpcionesArticulos.Visibility == Visibility.Collapsed;
            if (estaColapsado)
            {
                ArticuloBotonPrincipal.Background = new SolidColorBrush(hoverColor);
            }
        }

        private void ArticuloBotonPrincipal_MouseLeave(object sender, MouseEventArgs e)
        {
            bool estaColapsado = OpcionesArticulos.Visibility == Visibility.Collapsed;
            if (estaColapsado)
            {
                ArticuloBotonPrincipal.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        private void ToggleGestPrestamo_Click(object sender, MouseButtonEventArgs e)
        {
            bool estaVisible = OpcionesPrestamos.Visibility == Visibility.Visible;
            OpcionesPrestamos.Visibility = estaVisible ? Visibility.Collapsed : Visibility.Visible;
            PrestamoBotonPrincipal.Background = new SolidColorBrush(
                estaVisible ? Colors.Transparent : activeColor
            );
            //Rota la flecha
            PrestamoFlechaTransform.Angle = estaVisible ? 90 : -90;
        }

        private void ToggleGestDevolucion_Click(object sender, MouseButtonEventArgs e)
        {
            bool estaVisible = OpcionesDevoluciones.Visibility == Visibility.Visible;
            OpcionesDevoluciones.Visibility = estaVisible ? Visibility.Collapsed : Visibility.Visible;
            DevolucionBotonPrincipal.Background = new SolidColorBrush(
                estaVisible ? Colors.Transparent : activeColor
            );
            //Rota la flecha
            DevolucionFlechaTransform.Angle = estaVisible ? 90 : -90;
        }

        private void ToggleGestInventario_Click(object sender, MouseButtonEventArgs e)
        {
            bool estaVisible = OpcionesArticulos.Visibility == Visibility.Visible;
            OpcionesArticulos.Visibility = estaVisible ? Visibility.Collapsed : Visibility.Visible;
            ArticuloBotonPrincipal.Background = new SolidColorBrush(
                estaVisible ? Colors.Transparent : activeColor
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
                var articulo = buscarArticulo.Articulo;
                if (articulo != null)
                {
                    var articuloView = new ArticuloView(articulo);
                    ShowViewWithHeader(articuloView, "Modificar Artículo");
                }
            }
        }



        private void GoToConsultarPrestamo(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SeleccionarOpcion(TxtConsultarPrestamo);

                // Crear y suscribir evento
                var consulta = new ConsultarPrestamosView();
                consulta.PrestamoSeleccionado += OnPrestamoSeleccionado;
                ShowFullScreenView(consulta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la vista de consulta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToConsultarArticulo(object sender, MouseButtonEventArgs e)
        {
            SeleccionarOpcion(TxtConsultarArticulo);

            try
            {
                // Crear y suscribir evento
                var consulta = new ConsultarArticulosView();
                consulta.ArticuloSeleccionado += OnArticuloSeleccionado;
                ShowFullScreenView(consulta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la vista de consulta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Manejador del evento lanzado por ConsultarPrestamos
        private void OnPrestamoSeleccionado(object sender, string idPrestamo)
        {
            // Separar los IDs
            var partes = idPrestamo.Split('_');
            if (partes.Length != 2) return;

            if (!BigInteger.TryParse(partes[0], out BigInteger clienteId)) return;
            if (!BigInteger.TryParse(partes[1], out BigInteger articuloId)) return;

            // Buscar el préstamo con la función específica
            var prestamo = admin.BuscarPrestamo(clienteId.ToString(), articuloId.ToString());
            if (prestamo == null) return;

            // Crear vista de detalles y cargar datos
            var detalles = new DetallesPrestamoView();
            detalles.CargarDatos(prestamo);

            // Navegar a detalles
            MainContent.Content = detalles;
        }

        //Cuando te lnazan a consultar una devolucion
        private void GoToConsultarDevolucion(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SeleccionarOpcion(TxtConsultarDevolucion);

                // Crear instancia de la vista
                var consulta = new ConsultarDevolucionView();

                // Cargar la vista en el contenedor principal
                ShowFullScreenView(consulta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la vista de consulta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void OnArticuloSeleccionado(object sender, string idArticulo)
        {
            //se muestra la vista de editar artículo con el id seleccionado
            try
            {
                if(!BigInteger.TryParse(idArticulo, out BigInteger articuloId))
                {
                    MessageBox.Show("ID de artículo inválido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var articulo = Sesion.Sesion.GetAdministradorActivo().BuscarArticulo(idArticulo);
                if (articulo != null)
                {

                    var articuloView = new ArticuloView(articulo);
                    ShowViewWithHeader(articuloView, "Consultar Artículo", "Actualizar articulo consultado");
                }
                else
                {
                    MessageBox.Show("Artículo no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar el artículo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}