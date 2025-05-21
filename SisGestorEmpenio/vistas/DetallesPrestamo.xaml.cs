using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.vistas
{
    public partial class DetallesPrestamo : UserControl
    {
        public DetallesPrestamo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carga todos los campos de UI con la información del préstamo.
        /// </summary>
        public void CargarDatos(Prestamo p)
        {
            // Encabezado
            txtResumenPrestamo.Text = $"Préstamo de '{p.ArticuloNombre}' a {p.ClienteNombre}";

            // Datos Cliente
            txtIdentificacion.Text = p.ClienteId.ToString();
            var nombres = p.ClienteNombre.Split(' ');
            txtNombreCliente.Text = nombres.First();
            txtApellidoCliente.Text = nombres.Last();
            txtTelefonoCliente.Text = p.GetCliente().GetTelefono();
            txtCorreoCliente.Text = p.GetCliente().GetCorreo();

            // Datos Artículo
            txtIdArticulo.Text = p.ArticuloId.ToString(); // Nuevo campo agregado
            txtArticulo.Text = p.ArticuloNombre;
            txtValorArticulo.Text = p.GetArticulo().GetValorEstimado().ToString("F2");
            txtEstadoArticulo.Text = p.GetArticulo().GetEstadoDevolucion();

            // Datos Préstamo
            txtFechaInicio.Text = p.FechaInicio;
            txtFechaFin.Text = p.FechaFin;
            txtEstadoDevolucion.Text = p.Estado;

            // Color según estado
            var activo = p.Estado.Equals("activo", StringComparison.OrdinalIgnoreCase);
            borderEstadoDevolucion.Background =
                new SolidColorBrush(activo
                    ? Color.FromRgb(0xE8, 0xF5, 0xE9)
                    : Color.FromRgb(0xFF, 0xEB, 0xEE));

            txtEstadoDevolucion.Foreground = new SolidColorBrush(activo
                ? Color.FromRgb(0x2E, 0x7D, 0x32)
                : Color.FromRgb(0xD3, 0x2F, 0x2F));

            txtTasaInteres.Text = $"{p.GetTasaInteres():F2}%";
            txtMonto.Text = p.MontoTotal.ToString("F2");
        }
    }
}