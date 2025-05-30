using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.vistas
{
    public partial class DetallesPrestamoView : UserControl
    {
        public DetallesPrestamoView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carga todos los campos de UI con la información del préstamo.
        /// </summary>
        public void CargarDatos(Prestamo p)
        {
            // Encabezado
            txtResumenPrestamo.Text = $"Préstamo de '{p.GetArticulo().GetDescripcion()}' a {p.GetCliente().GetNombre()}";

            // Datos Cliente
            txtIdentificacion.Text = p.GetCliente().GetId();
            txtNombreCliente.Text = p.GetCliente().GetNombre();
            txtApellidoCliente.Text = p.GetCliente().GetApellido();
            txtTelefonoCliente.Text = p.GetCliente().GetTelefono();
            txtCorreoCliente.Text = p.GetCliente().GetCorreo();

            // Datos Artículo
            txtIdArticulo.Text = p.GetArticulo().GetIdArticulo(); // Nuevo campo agregado
            txtArticulo.Text = p.GetArticulo().GetDescripcion();
            txtValorArticulo.Text = p.GetArticulo().GetValorEstimado().ToString("F2");
            txtEstadoArticulo.Text = p.GetArticulo().GetEstadoArticulo();

            // Datos Préstamo
            txtFechaInicio.Text = p.GetFechaInicio().ToString("dd/MM/yyyy");
            txtFechaFin.Text = p.GetFechaFin().ToString("dd/MM/yyyy");
            txtEstadoDevolucion.Text = p.GetEstado();

            // Color según estado
            var activo = p.GetEstado().Equals("activo", StringComparison.OrdinalIgnoreCase);
            borderEstadoDevolucion.Background =
                new SolidColorBrush(activo
                    ? Color.FromRgb(0xE8, 0xF5, 0xE9)
                    : Color.FromRgb(0xFF, 0xEB, 0xEE));

            txtEstadoDevolucion.Foreground = new SolidColorBrush(activo
                ? Color.FromRgb(0x2E, 0x7D, 0x32)
                : Color.FromRgb(0xD3, 0x2F, 0x2F));

            txtTasaInteres.Text = $"{p.GetTasaInteres():F2}%";
            txtMonto.Text = p.GetMontoTotal().ToString("F2");
        }
    }
}