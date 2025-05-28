using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repository;

namespace SisGestorEmpenio.vistas
{
    public partial class ConsultarPrestamosView : UserControl
    {
        // Campo modificado para no ser readonly, permitiendo su asignación en el evento Loaded
        private Administrador admin;
        private List<Prestamo> prestamosOriginales;
        private List<Prestamo> prestamosFiltrados;
        private bool isInitialized = false;

        // Variables para la paginación
        private int paginaActual = 1;
        private int registrosPorPagina = 10;
        private int totalPaginas = 0;

        // Variable de cancelación para el filtrado
        private CancellationTokenSource ctsFiltrado = new CancellationTokenSource();

        // Evento para comunicarse con la ventana contenedora
        public event EventHandler<string> PrestamoSeleccionado;

        // Constructor sin recibir administrador por parámetro
        public ConsultarPrestamosView()
        {
            InitializeComponent();

            // Usamos el evento Loaded para garantizar que todos los controles estén inicializados
            this.Loaded += (s, e) =>
            {
                if (!isInitialized)
                {
                    admin = Sesion.Sesion.GetAdministradorActivo();
                    CargarPrestamos();
                    isInitialized = true;
                }
            };
        }

        /// <summary>
        /// Carga todos los préstamos desde el administrador
        /// </summary>
        public void CargarPrestamos()
        {
            try
            {
                prestamosOriginales = admin?.ConsultarPrestamosCoincidentes() ?? new List<Prestamo>();
                prestamosFiltrados = new List<Prestamo>(prestamosOriginales);
                paginaActual = 1;
                ActualizarVisualizacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar préstamos: {ex.Message}",
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
            if (prestamosFiltrados == null) return;

            // Calcular paginación
            int totalRegistros = prestamosFiltrados.Count;
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
            var registrosPagina = prestamosFiltrados
                .Skip((paginaActual - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .Select(p => new
                {
                    ClienteId = p.GetCliente().GetId(),
                    ClienteNombre = p.GetCliente().GetNombre(),
                    ArticuloNombre = p.GetArticulo().GetDescripcion(),
                    FechaInicio = p.GetFechaInicio().ToShortDateString(),
                    FechaFin = p.GetFechaFin().ToShortDateString(),
                    Estado = p.GetEstado(),   
                    IdPrestamo = $"{p.GetCliente().GetId()}_{p.GetArticulo().GetIdArticulo()}"
                })
                .ToList();

            // Actualizar DataGrid
            dgPrestamos.ItemsSource = registrosPagina;

            // Actualizar controles de navegación
            ActualizarControlesNavegacion();
            ActualizarInfoRegistros(totalRegistros);
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
        private Button CrearBotonPagina(int numeroPagina)
        {
            var btn = new Button
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
        /// Actualiza la información de registros mostrados
        /// </summary>
        private void ActualizarInfoRegistros(int totalRegistros)
        {
            if (totalRegistros == 0)
            {
                txtInfoRegistros.Text = "No se encontraron registros";
                return;
            }

            int registroInicio = (paginaActual - 1) * registrosPorPagina + 1;
            int registroFin = Math.Min(paginaActual * registrosPorPagina, totalRegistros);

            txtInfoRegistros.Text = $"Mostrando {registroInicio}-{registroFin} de {totalRegistros} registros";
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
            => FiltrarPrestamos();

        /// <summary>
        /// Maneja el cambio de texto en el campo de identificación
        /// </summary>
        private void TxtIdentificacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtIdentificacion.Text != "Identificación...")
                FiltrarPrestamos();
        }

        /// <summary>
        /// Aplica todos los filtros a la lista de préstamos
        /// </summary>
        private async void FiltrarPrestamos()
        {
            if (prestamosOriginales == null || !isInitialized)
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
                string estado = (cmbEstado.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                string rangoTiempo = (cmbRangoTiempo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                string identificacion = txtIdentificacion.Text.Trim().ToLower();

                int clienteId = int.TryParse(identificacion, out var tempId) ? tempId : -1;

                int rangoTiempoInt = rangoTiempo switch
                {
                    "Hoy" => 1,
                    "Últimos 5 días" => 5,
                    "Últimos 8 días" => 8,
                    "Últimos 15 días" => 15,
                    "Últimos 30 días" => 30,
                    _ => -1,
                };

                var listaPrestamosFiltrados = admin.ConsultarPrestamosCoincidentes(
                    cantidadMaxPrestamos: 1000,
                    clienteId: clienteId,
                    estado: (estado == "Todos" || estado == "Estado") ? "" : estado,
                    rangoDias: rangoTiempoInt
                );

                prestamosFiltrados = listaPrestamosFiltrados;
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
        /// Gestiona el comportamiento de placeholder cuando el control recibe el foco
        /// </summary>
        private void TxtIdentificacion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtIdentificacion.Text == "Identificación...")
            {
                txtIdentificacion.Text = string.Empty;
                txtIdentificacion.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Gestiona el comportamiento de placeholder cuando el control pierde el foco
        /// </summary>
        private void TxtIdentificacion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text))
            {
                txtIdentificacion.Text = "Identificación...";
                txtIdentificacion.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Maneja el clic en el botón de tres puntos
        /// </summary>
        private void BtnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Muestra los detalles del préstamo seleccionado
        /// </summary>
        private void MenuVerDetallesDevolucion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Devolucion devolucion)
            {
                MostrarDetalleDevolucion(devolucion);
            }
        }


        /// <summary>
        /// Muestra los detalles del préstamo
        /// </summary>
        private void MostrarDetalleDevolucion(Devolucion devolucion)
        {
            try
            {
                var ventana = new DetallesDevolucion();
                ventana.CargarDetalles(devolucion);
                ventana.Owner = Window.GetWindow(this);  // Asocia con ventana actual
                ventana.ShowDialog();  // Abre como ventana modal
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar detalles: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Método público para actualizar los filtros desde el exterior
        /// </summary>
        public void ActualizarFiltros(string estado = null, string rangoTiempo = null, string identificacion = null)
        {
            if (estado != null)
            {
                var estadoItem = cmbEstado.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == estado);
                if (estadoItem != null)
                    cmbEstado.SelectedItem = estadoItem;
            }

            if (rangoTiempo != null)
            {
                var rangoItem = cmbRangoTiempo.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == rangoTiempo);
                if (rangoItem != null)
                    cmbRangoTiempo.SelectedItem = rangoItem;
            }

            if (identificacion != null)
            {
                txtIdentificacion.Text = identificacion;
                txtIdentificacion.Foreground = Brushes.Black;
            }

            FiltrarPrestamos();
        }

        /// <summary>
        /// Método público para recargar préstamos desde el exterior
        /// </summary>
        public void RecargarPrestamos()
        {
            CargarPrestamos();
            FiltrarPrestamos();
        }

        /// <summary>
        /// Método público para cambiar el número de registros por página
        /// </summary>
        public void CambiarRegistrosPorPagina(int registros)
        {
            registrosPorPagina = registros;
            paginaActual = 1;
            ActualizarVisualizacion();
        }
    }
}