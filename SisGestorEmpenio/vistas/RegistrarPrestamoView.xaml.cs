using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SisGestorEmpenio.Modelos;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarPrestamoView.xaml
    /// </summary>
    public partial class RegistrarPrestamoView : UserControl
    {
        Cliente clienteRegistrado = null;
        Articulo articuloRegistrado = null;

        // Evento para notificar cuando el proceso se ha completado
        public event EventHandler ProcesoCompletado;

        // Evento para notificar cuando el proceso se ha cancelado
        public event EventHandler ProcesoCancelado;

        // Enum para las etapas del proceso
        public enum EtapaProceso
        {
            DatosCliente = 1,
            DatosArticulo = 2,
            DetallesPrestamo = 3
        }

        public RegistrarPrestamoView()
        {
            InitializeComponent();
            // Inicializar en la primera etapa
            ActualizarBarraProgreso(EtapaProceso.DatosCliente);
            MostrarVistaCliente();

        }

        public RegistrarPrestamoView(bool clienteYaRegistrado, bool articuloYaRegistrado)
        {
            InitializeComponent();


            if (clienteYaRegistrado)
            {
                if (articuloYaRegistrado)
                {
                    // Si ambos están registrados, ir directamente a detalles del préstamo
                    ActualizarBarraProgreso(EtapaProceso.DetallesPrestamo);
                    vistaFormulario.Content = new PrestamoView();
                }
                else
                {
                    // Cliente registrado, mostrar vista de artículo
                    ActualizarBarraProgreso(EtapaProceso.DatosArticulo);
                    MostrarVistaArticulo();
                }
            }
            else
            {
                // Iniciar desde el cliente
                ActualizarBarraProgreso(EtapaProceso.DatosCliente);
                MostrarVistaCliente();
            }
        }

        private void MostrarVistaCliente()
        {
            var vistaCliente = new ClienteView();
            vistaCliente.RegistroClienteCompletado += (s, cliente) =>
            {
                clienteRegistrado = cliente;
                // Pasar a la siguiente etapa
                ActualizarBarraProgreso(EtapaProceso.DatosArticulo);
                MostrarVistaArticulo();
            };
            vistaFormulario.Content = vistaCliente;
        }

        private void MostrarVistaArticulo()
        {
            var vistaArticulo = new ArticuloView();
            vistaArticulo.RegistroArticuloCompletado += (s, articulo) =>
            {
                articuloRegistrado = articulo;
                // Pasar a la etapa final
                ActualizarBarraProgreso(EtapaProceso.DetallesPrestamo);
                MostrarVistaPrestamo();
            };
            vistaFormulario.Content = vistaArticulo;
        }

        private void MostrarVistaPrestamo()
        {
            PrestamoView prestamoView;

            if (clienteRegistrado != null && articuloRegistrado != null)
            {
                prestamoView = new PrestamoView(clienteRegistrado, articuloRegistrado);
            }
            else if (clienteRegistrado != null)
            {
                prestamoView = new PrestamoView(clienteRegistrado);
            }
            else if (articuloRegistrado != null)
            {
                prestamoView = new PrestamoView(articuloRegistrado);
            }
            else
            {
                prestamoView = new PrestamoView();
            }

            // Suscribirse al evento de préstamo completado
            prestamoView.PrestamoRegistradoCompletado += (s, e) =>
            {
                // Notificar que el proceso completo ha terminado
                ProcesoCompletado?.Invoke(this, EventArgs.Empty);
            };

            vistaFormulario.Content = prestamoView;
        }

        private void ActualizarBarraProgreso(EtapaProceso etapaActual)
        {
            // Colores para los diferentes estados
            var colorActivo = (SolidColorBrush)new BrushConverter().ConvertFrom("#148484");
            var colorInactivo = (SolidColorBrush)new BrushConverter().ConvertFrom("#DFDFDF");
            var colorTextoActivo = Brushes.White;
            var colorTextoInactivo = (SolidColorBrush)new BrushConverter().ConvertFrom("#8D8D8D");
            var colorTextoLabel = Brushes.Black;
            var colorTextoLabelInactivo = (SolidColorBrush)new BrushConverter().ConvertFrom("#8D8D8D");

            // Tamaños de fuente
            var tamanoFuenteActivo = 14.0;
            var tamanoFuenteInactivo = 12.0; // Asumiendo tamaño por defecto

            // Resetear todos los elementos a estado inactivo
            ResetearElementosBarraProgreso(colorInactivo, colorTextoInactivo, colorTextoLabelInactivo, tamanoFuenteInactivo);

            // Activar elementos según la etapa actual
            switch (etapaActual)
            {
                case EtapaProceso.DatosCliente:
                    // Solo etapa 1 activa
                    CirculoEtapa1.Background = colorActivo;
                    NumeroEtapa1.Foreground = colorTextoActivo;
                    TextoEtapa1.Foreground = colorTextoLabel;
                    TextoEtapa1.FontSize = tamanoFuenteActivo;
                    TxtSubtitulo.Text = "Datos del cliente";
                    break;

                case EtapaProceso.DatosArticulo:
                    // Etapa 1 completada, etapa 2 activa
                    CirculoEtapa1.Background = colorActivo;
                    NumeroEtapa1.Foreground = colorTextoActivo;
                    TextoEtapa1.Foreground = colorTextoLabel;

                    CirculoEtapa2.Background = colorActivo;
                    NumeroEtapa2.Foreground = colorTextoActivo;
                    TextoEtapa2.Foreground = colorTextoLabel;
                    TextoEtapa2.FontSize = tamanoFuenteActivo; // Solo la etapa actual tiene fuente grande
                    TxtSubtitulo.Text = "Datos del artículo";
                    break;

                case EtapaProceso.DetallesPrestamo:
                    // Etapas 1 y 2 completadas, etapa 3 activa
                    CirculoEtapa1.Background = colorActivo;
                    NumeroEtapa1.Foreground = colorTextoActivo;
                    TextoEtapa1.Foreground = colorTextoLabel;

                    CirculoEtapa2.Background = colorActivo;
                    NumeroEtapa2.Foreground = colorTextoActivo;
                    TextoEtapa2.Foreground = colorTextoLabel;

                    CirculoEtapa3.Background = colorActivo;
                    NumeroEtapa3.Foreground = colorTextoActivo;
                    TextoEtapa3.Foreground = colorTextoLabel;
                    TextoEtapa3.FontSize = tamanoFuenteActivo; // Solo la etapa actual tiene fuente grande
                    TxtSubtitulo.Text = "Detalles del préstamo";
                    break;
            }
        }

        private void ResetearElementosBarraProgreso(SolidColorBrush colorInactivo, SolidColorBrush colorTextoInactivo, SolidColorBrush colorTextoLabelInactivo, double tamanoFuenteInactivo)
        {
            // Resetear etapa 1
            CirculoEtapa1.Background = colorInactivo;
            NumeroEtapa1.Foreground = colorTextoInactivo;
            TextoEtapa1.Foreground = colorTextoLabelInactivo;
            TextoEtapa1.FontSize = tamanoFuenteInactivo;

            // Resetear etapa 2
            CirculoEtapa2.Background = colorInactivo;
            NumeroEtapa2.Foreground = colorTextoInactivo;
            TextoEtapa2.Foreground = colorTextoLabelInactivo;
            TextoEtapa2.FontSize = tamanoFuenteInactivo;

            // Resetear etapa 3
            CirculoEtapa3.Background = colorInactivo;
            NumeroEtapa3.Foreground = colorTextoInactivo;
            TextoEtapa3.Foreground = colorTextoLabelInactivo;
            TextoEtapa3.FontSize = tamanoFuenteInactivo;
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            var confirmacionCancelar = new ConfirmacionWindow
            {
                Mensaje = "¿Está seguro que quiere cancelar el registro?",
                Titulo = "Cancelar Registro",
                TextoBotonIzquierdo = "No",
                TextoBotonDerecho = "Sí",
                MostrarBotonDerecho = true
            };

            bool? resultadoCancelar = confirmacionCancelar.ShowDialog();

            if (resultadoCancelar == true && confirmacionCancelar.Confirmado)
            {
                //elimina los datos del cliente y artículo registrados
                var administrador = Sesion.Sesion.GetAdministradorActivo();
                try
                {
                    if (clienteRegistrado != null)
                    {
                        administrador.EliminarCliente(clienteRegistrado);
                    }
                    if (articuloRegistrado != null)
                    {
                        administrador.EliminarArticulo(articuloRegistrado);
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción si ocurre un error al eliminar
                    MessageBox.Show($"Error al cancelar el registro por completo (puede que algunos registros persistan): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // El usuario confirmó que quiere cancelar
                ProcesoCancelado?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CerrarVista()
        {
            // Buscar el ContentControl padre para cerrar la vista
            DependencyObject padre = this.Parent;
            while (padre != null)
            {
                if (padre is ContentControl contentControl)
                {
                    // Limpiar el contenido del ContentControl
                    contentControl.Content = null;
                    break;
                }
                padre = LogicalTreeHelper.GetParent(padre);
            }
        }

        // Método público para permitir que la ventana padre cierre la vista
        public void CerrarVistaDesdeVentanaPadre()
        {
            CerrarVista();
        }
    }
}