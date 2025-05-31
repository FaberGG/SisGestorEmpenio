using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repository;

namespace SisGestorEmpenio.vistas
{
    public partial class ConsultarDevolucionView : UserControl
    {
        private Administrador admin;
        private List<Devolucion> devolucionesOriginales;
        private List<Devolucion> devolucionesFiltradas;
        private bool isInitialized = false;

        private int paginaActual = 1;
        private int registrosPorPagina = 10;
        private int totalPaginas = 0;
        private CancellationTokenSource ctsFiltrado = new CancellationTokenSource();

        public event EventHandler<string> DevolucionSeleccionada;

        public ConsultarDevolucionView()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                if (!isInitialized)
                {
                    admin = Sesion.Sesion.GetAdministradorActivo();
                    CargarDevoluciones();
                    isInitialized = true;
                }
            };
        }

        public void CargarDevoluciones()
        {
            try
            {
                devolucionesOriginales = admin?.ConsultarDevoluciones() ?? new List<Devolucion>();
                devolucionesFiltradas = new List<Devolucion>(devolucionesOriginales);

                paginaActual = 1;
                ActualizarVisualizacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar devoluciones: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarVisualizacion()
        {
            if (devolucionesFiltradas == null) return;

            int totalRegistros = devolucionesFiltradas.Count;
            totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            paginaActual = Math.Min(paginaActual, totalPaginas == 0 ? 1 : totalPaginas);

            var registrosPagina = devolucionesFiltradas
                .Skip((paginaActual - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .Select(d => new
                {
                    Cedula = d.GetPrestamo().GetCliente()?.GetId() ?? "nulo",
                    IdArticulo = d.GetPrestamo().GetArticulo().GetIdArticulo() ?? "nulo",
                    IdConvenio = d.GetNumeroConvenio().ToString(),
                    FechaDevolucion = d.GetFechaDevolucion().ToShortDateString(),
                    Devolucion = d
                })
                .ToList();

            dgDevoluciones.ItemsSource = registrosPagina;

            ActualizarControlesNavegacion();
            ActualizarInfoRegistros(totalRegistros);
        }

        private void ActualizarControlesNavegacion()
        {
            btnAnterior.IsEnabled = paginaActual > 1;
            btnSiguiente.IsEnabled = paginaActual < totalPaginas;
            pnlPaginas.Children.Clear();

            int inicio = Math.Max(1, paginaActual - 2);
            int fin = Math.Min(totalPaginas, paginaActual + 2);

            if (inicio > 1) AgregarBotonPagina(1, "...");
            for (int i = inicio; i <= fin; i++) AgregarBotonPagina(i);
            if (fin < totalPaginas) AgregarBotonPagina(totalPaginas, "...");
        }

        private void AgregarBotonPagina(int numero, string label = null)
        {
            if (label != null)
            {
                pnlPaginas.Children.Add(new TextBlock
                {
                    Text = label,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5, 0, 5, 0),
                    Foreground = new SolidColorBrush(Color.FromRgb(80, 90, 100))
                });
            }

            var btn = new Button
            {
                Content = numero.ToString(),
                Tag = numero,
                Style = numero == paginaActual
                    ? (Style)FindResource("PaginationButtonActiveStyle")
                    : (Style)FindResource("PaginationButtonStyle")
            };

            btn.Click += (s, e) =>
            {
                paginaActual = numero;
                ActualizarVisualizacion();
            };

            pnlPaginas.Children.Add(btn);
        }

        private void ActualizarInfoRegistros(int totalRegistros)
        {
            if (totalRegistros == 0)
            {
                txtInfoRegistros.Text = "No se encontraron registros";
                return;
            }

            int inicio = (paginaActual - 1) * registrosPorPagina + 1;
            int fin = Math.Min(paginaActual * registrosPorPagina, totalRegistros);
            txtInfoRegistros.Text = $"Mostrando {inicio}-{fin} de {totalRegistros} registros";
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--;
                ActualizarVisualizacion();
            }
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (paginaActual < totalPaginas)
            {
                paginaActual++;
                ActualizarVisualizacion();
            }
        }

        // EVENTOS DE FILTRADO - Actualizados para el nuevo diseño
        private void TxtIdArticulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Solo filtrar si hay texto real (no vacío)
            if (!string.IsNullOrEmpty(txtIdArticulo.Text))
                FiltrarDevoluciones();
            else if (isInitialized)
                FiltrarDevoluciones(); // Actualizar también cuando se borra el texto
        }

        private void TxtCedula_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Solo filtrar si hay texto real (no vacío)
            if (!string.IsNullOrEmpty(txtCedula.Text))
                FiltrarDevoluciones();
            else if (isInitialized)
                FiltrarDevoluciones(); // Actualizar también cuando se borra el texto
        }

        private void CmbRangoFechas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarDevoluciones();
        }
        private void FullAreaButton_Click(object sender, RoutedEventArgs e)
        {
            cmbRangoFechas.IsDropDownOpen = !cmbRangoFechas.IsDropDownOpen;
        }

        // MÉTODO DE FILTRADO ACTUALIZADO
        private async void FiltrarDevoluciones()
        {
            if (devolucionesOriginales == null || !isInitialized)
                return;

            ctsFiltrado.Cancel();
            ctsFiltrado = new CancellationTokenSource();
            var token = ctsFiltrado.Token;

            try
            {
                await Task.Delay(300, token);

                // Filtrar localmente desde devolucionesOriginales
                //var filtradas = devolucionesOriginales.AsEnumerable();

                // Filtro por cédula del cliente
                string textoCedula = txtCedula.Text.Trim();


                // Filtro por ID del artículo
                string textoArticulo = txtIdArticulo.Text.Trim();


                //obtengo el valor de rango de fechas seleccionado
                // Filtro por rango de fechas
                var comboItem = cmbRangoFechas?.SelectedItem as ComboBoxItem;
                string rango = comboItem?.Content?.ToString() ?? string.Empty;
                int rangoDias = rango switch
                {
                    "Hoy" => 0,
                    "Últimos 5 días" => 5,
                    "Últimos 10 días" => 10,
                    "Últimos 30 días" => 30,
                    _ => -1 // No filtrar por fecha
                };


                devolucionesFiltradas = admin.ConsultarDevoluciones(
                    cantidadMaxDevoluciones: 100, 
                    clienteId: string.IsNullOrWhiteSpace(textoCedula) || textoCedula == "Identificación..." ? string.Empty : textoCedula,
                    articuloId: string.IsNullOrWhiteSpace(textoArticulo) || textoArticulo == "Id Articulo..." ? string.Empty : textoArticulo,
                    rangoDias: rangoDias
                );
                paginaActual = 1;
                ActualizarVisualizacion();
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar devoluciones: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // EVENTOS DE FOCUS PARA PLACEHOLDERS
        private void TxtIdArticulo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtIdArticulo.Text == "Id Articulo...")
            {
                txtIdArticulo.Text = string.Empty;
                txtIdArticulo.Foreground = Brushes.Black;
            }
        }

        private void TxtIdArticulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdArticulo.Text))
            {
                txtIdArticulo.Text = "Id Articulo...";
                txtIdArticulo.Foreground = Brushes.Gray;
            }
        }

        private void TxtCedula_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtCedula.Text == "Identificación...")
            {
                txtCedula.Text = string.Empty;
                txtCedula.Foreground = Brushes.Black;
            }
        }

        private void TxtCedula_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                txtCedula.Text = "Identificación...";
                txtCedula.Foreground = Brushes.Gray;
            }
        }

        private void MenuVerDetallesDevolucion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Devolucion devolucion)
            {
                MostrarDetalleDevolucion(devolucion);
            }
        }

        private void MostrarDetalleDevolucion(Devolucion devolucion)
        {
            try
            {
                var ventana = new DetallesDevolucion();
                ventana.CargarDetalles(devolucion);
                ventana.Owner = Window.GetWindow(this);
                ventana.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar detalles: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RecargarDevoluciones()
        {
            CargarDevoluciones();
        }

        public void CambiarRegistrosPorPagina(int cantidad)
        {
            registrosPorPagina = cantidad;
            paginaActual = 1;
            ActualizarVisualizacion();
        }
    }
}