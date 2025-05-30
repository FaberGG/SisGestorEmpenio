using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Lógica de interacción para DetallesDevolucionWindow.xaml
    /// </summary>
    public partial class DetallesDevolucionWindow : UserControl
    {
        public DetallesDevolucionWindow()
        {
            InitializeComponent();
        }


        public void CargarDetalles(Devolucion devolucion)
        {
            // Aquí carga los datos recibidos en tus TextBlocks
            txtCedulaCliente.Text = devolucion.GetPrestamo().GetCliente()?.GetId() ?? "";
            txtNombreCliente.Text = devolucion.GetPrestamo().GetCliente()?.GetNombre() ?? "";
            txtApellidoCliente.Text = devolucion.GetPrestamo().GetCliente()?.GetApellido() ?? "";
            txtTelefonoCliente.Text = devolucion.GetPrestamo().GetCliente()?.GetTelefono() ?? "";
            txtCorreoCliente.Text = devolucion.GetPrestamo().GetCliente()?.GetCorreo() ?? "";

            txtIdArticulo.Text = devolucion.GetPrestamo().GetArticulo()?.GetIdArticulo() ?? "";
            txtDescripcionArticulo.Text = devolucion.GetPrestamo().GetArticulo()?.GetDescripcion() ?? "";
            txtValorArticulo.Text = devolucion.GetPrestamo().GetArticulo()?.GetValorEstimado().ToString() ?? "";
            txtEstadoArticulo.Text = devolucion.GetPrestamo().GetArticulo()?.GetEstadoArticulo() ?? "";

            txtFechaDevolucion.Text = devolucion.GetFechaDevolucion().ToShortDateString();
            txtNumConvenio.Text = devolucion.GetNumeroConvenio().ToString();
            txtTasaInteres.Text = devolucion.GetPrestamo()?.GetTasaInteres().ToString() ?? "";
            txtMonto.Text = devolucion.GetMontoPagado().ToString();
        }


    }
}
