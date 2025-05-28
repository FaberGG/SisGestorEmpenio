using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para ConsultarArticulosView.xaml
    /// </summary>
    public partial class ConsultarArticulosView : UserControl
    {
        private Administrador admin;
        private List<Articulo> articulos;
        private bool isInitialized = false;


        // Variable de cancelación para el filtrado
        private CancellationTokenSource ctsFiltrado = new CancellationTokenSource();

        // Variables para la paginación
        private int paginaActual = 1;
        private int registrosPorPagina = 10;
        private int totalPaginas = 0;

        // Evento para comunicarse con la ventana contenedora
        public event EventHandler<int> ArticuloSeleccionado;



        public ConsultarArticulosView()
        {
            InitializeComponent();
            // Usamos el evento Loaded para garantizar que todos los controles estén inicializados
            this.Loaded += (s, e) =>
            {
                if (!isInitialized)
                {
                    admin = Sesion.Sesion.GetAdministradorActivo();
                    CargarArticulos();
                    isInitialized = true;
                }
            };
        }


        //carga todos los articulos desde administrador
        public void CargarArticulos()
        {
            try
            {
                articulos = admin?.BuscarArticulosCoincidentes() ?? new List<Articulo>();
                paginaActual = 1;
                ActualizarVisualizacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar articulos: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Actualiza la visualización del DataGrid y la paginación
        /// </summary>
        private void ActualizarVisualizacion()
        {
            if (articulos == null) return;

            // Calcular paginación
            int totalRegistros = articulos.Count;
            totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            if (totalPaginas == 0)
            {
                totalPaginas = 1;
                paginaActual = 1;
            }
            else if (paginaActual > totalPaginas)
            {
                paginaActual = totalPaginas;
            }


            // Obtener registros de la página actual
            var registrosPagina = articulos
                .Skip((paginaActual - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                // Proyectar los datos necesarios para el DataGrid
                .Select(p => new
                {
                    idArticulo = p.GetIdArticulo(),
                    descripcion = p.GetDescripcion(),
                    valor = p.GetValorEstimado(),
                    EstadoArticulo = p.GetEstadoArticulo() == "" ? "Sin estado" : p.GetEstadoArticulo().ToLower(),
                    EstadoDevolucion = p.GetEstadoDevolucion() == "" ? "Sin estado" : p.GetEstadoDevolucion().ToLower(),
                })
                .ToList();

            // Actualizar DataGrid
            MainDataGrid.ItemsSource = registrosPagina;

            // Actualizar controles de navegación
            ActualizarControlesNavegacion();
        }


        /// <summary>
        /// Actualiza los controles de navegación (botones y números de página)
        /// </summary>
        private void ActualizarControlesNavegacion()
        {
            // Habilitar/deshabilitar botones
            btnAnterior.IsEnabled = paginaActual > 1;
            btnSiguiente.IsEnabled = paginaActual < totalPaginas;

            // Limpiar panel de páginas
            pnlPaginas.Children.Clear();

            // Agregar números de página
            int inicioRango = Math.Max(1, paginaActual - 2);
            int finRango = Math.Min(totalPaginas, paginaActual + 2);

            // Botón primera página si está fuera del rango
            if (inicioRango > 1)
            {
                var btnPrimera = CrearBotonPagina(1);
                pnlPaginas.Children.Add(btnPrimera);

                if (inicioRango > 2)
                {
                    var lblPuntos = new TextBlock
                    {
                        Text = "...",
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5, 0, 5, 0),
                        Foreground = new SolidColorBrush(Color.FromRgb(80, 90, 100))
                    };
                    pnlPaginas.Children.Add(lblPuntos);
                }
            }

            // Botones del rango visible
            for (int i = inicioRango; i <= finRango; i++)
            {
                var btnPagina = CrearBotonPagina(i);
                pnlPaginas.Children.Add(btnPagina);
            }

            // Botón última página si está fuera del rango
            if (finRango < totalPaginas)
            {
                if (finRango < totalPaginas - 1)
                {
                    var lblPuntos = new TextBlock
                    {
                        Text = "...",
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(5, 0, 5, 0),
                        Foreground = new SolidColorBrush(Color.FromRgb(80, 90, 100))
                    };
                    pnlPaginas.Children.Add(lblPuntos);
                }

                var btnUltima = CrearBotonPagina(totalPaginas);
                pnlPaginas.Children.Add(btnUltima);
            }
        }


        /// <summary>
        /// Crea un botón para navegar a una página específica
        /// </summary>
        private System.Windows.Controls.Button CrearBotonPagina(int numeroPagina)
        {
            var btn = new System.Windows.Controls.Button
            {
                Content = numeroPagina.ToString(),
                Tag = numeroPagina,
                Style = numeroPagina == paginaActual
                    ? (Style)FindResource("PaginationButtonActiveStyle")
                    : (Style)FindResource("PaginationButtonStyle")
            };

            btn.Click += (s, e) =>
            {
                if (int.TryParse(btn.Tag?.ToString(), out int pagina))
                {
                    paginaActual = pagina;
                    ActualizarVisualizacion();
                }
            };

            return btn;
        }


        /// <summary>
        /// Maneja el clic en el botón Anterior
        /// </summary>
        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--;
                ActualizarVisualizacion();
            }
        }

        /// <summary>
        /// Maneja el clic en el botón Siguiente
        /// </summary>
        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual < totalPaginas)
            {
                paginaActual++;
                ActualizarVisualizacion();
            }
        }

        /// <summary>
        /// Maneja el cambio de selección en los filtros
        /// </summary>
        private void Filtro_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => FiltrarArticulos();

        /// <summary>
        /// Maneja el cambio de texto en el campo de identificación
        /// </summary>
        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
                FiltrarArticulos();
        }

        /// <summary>
        /// Aplica todos los filtros a la lista de articulos
        /// </summary>
        private async void FiltrarArticulos()
        {
            if (articulos == null || !isInitialized)
                return;

            // Cancelar solicitud anterior
            ctsFiltrado.Cancel();
            ctsFiltrado = new CancellationTokenSource();
            var token = ctsFiltrado.Token;

            try
            {
                // Delay antes de aplicar el filtro (debounce de 500 ms)
                await Task.Delay(300, token);

                // Obtener filtros
                System.Windows.Controls.ComboBox cmbEstado = FindChild<System.Windows.Controls.ComboBox>(MainDataGrid, "cmbEstadoArticulo");
                System.Windows.Controls.ComboBox cmbDevolucion = FindChild<System.Windows.Controls.ComboBox>(MainDataGrid, "cmbEstadoDevolucion");
                string estadoArticulo = (cmbEstado?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                string estadoDevolucion = (cmbDevolucion?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                string propiedadCasa = (cmbPropiedadCasa.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                string txtIdDescripcion = txtBusqueda.Text.Trim();

                int idArticulo = int.TryParse(  txtIdDescripcion, out var tempId) ? tempId : -1;
                // Filtrar artículos
                
                var listaArticulosFiltrados = admin.BuscarArticulosCoincidentes(
                    cantidadMaxArticulos: 100,
                    id: idArticulo,
                    descripcion: txtIdDescripcion,
                    propiedadCasa: (propiedadCasa == "Todos" || propiedadCasa == "Propiedad") ? -1 : (propiedadCasa == "Casa" ? 1 : 0),
                    estado: estadoArticulo == "Estado (Todos)" ? "" : estadoArticulo,
                    devolucion: estadoDevolucion == "Devolución (Todos)" ? "" : estadoDevolucion
                );
                
                articulos = listaArticulosFiltrados;
                paginaActual = 1;
                ActualizarVisualizacion();
            }
            catch (TaskCanceledException)
            {
                // Se canceló la tarea anterior, no hacer nada
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar préstamos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        /// <summary>
        /// Maneja el clic en el botón de tres puntos
        /// </summary>
        private void BtnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }








        //METODO PARA ENCONTRAR EL COMPONENTE HIJO EN EL VISUAL TREE
        //para encontrar los combobox dentro del datagrid
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T childType)
                {
                    var fe = child as FrameworkElement;
                    if (fe != null && fe.Name == childName)
                        return childType;
                }

                var result = FindChild<T>(child, childName);
                if (result != null)
                    return result;
            }

            return null;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            if (button?.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.Placement = PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }

        private void EditarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menuItem = sender as MenuItem;

                if (menuItem?.DataContext != null)
                {
                    // Obtener datos del objeto anónimo
                    dynamic datosGrid = menuItem.DataContext;

                    // Disparamos el evento para que la ventana padre maneje la navegación
                    ArticuloSeleccionado?.Invoke(this, datosGrid.idArticulo);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
