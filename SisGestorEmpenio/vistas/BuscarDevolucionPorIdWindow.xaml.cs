using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;
using SisGestorEmpenio.Sesion;
using System;
using System.Windows;
using System.Windows.Input;

namespace SisGestorEmpenio.vistas
{
    public partial class BuscarDevolucionPorIdWindow : Window
    {
        // Propiedad para exponer la devolución seleccionada al exterior
        public Devolucion DevolucionSeleccionada { get; private set; }

        public BuscarDevolucionPorIdWindow()
        {
            InitializeComponent();

            // Máximo de caracteres para los TextBox
            txtClienteId.MaxLength = 10;
            txtArticuloId.MaxLength = 10;

            // Validaciones automáticas al perder foco
            txtClienteId.LostFocus += (s, e) =>
                ValidacionHelper.ValidarIdentificador(txtClienteId, lblIdCliente, "identificación del cliente*");
            txtArticuloId.LostFocus += (s, e) =>
                ValidacionHelper.ValidarIdentificador(txtArticuloId, lblIdArticulo, "identificador del artículo*");

            // Prevenir caracteres no numéricos
            txtClienteId.PreviewTextInput += SoloNumeros_Preview;
            txtArticuloId.PreviewTextInput += SoloNumeros_Preview;
        }

        // Solo permite dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        // Evento click del botón Buscar
        private void btnBuscar_Click(object sender, MouseButtonEventArgs e)
        {
            bool valido =
                ValidacionHelper.ValidarIdentificador(txtClienteId, lblIdCliente, "identificación del cliente*") &
                ValidacionHelper.ValidarIdentificador(txtArticuloId, lblIdArticulo, "identificador del artículo*");

            if (!valido)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }
            //obtengo los valores de los campos
            string clienteId = txtClienteId.Text.Trim();
            string articuloId = txtArticuloId.Text.Trim();

            var admin = Sesion.Sesion.GetAdministradorActivo();
            try
            {
                var dev = admin.BuscarDevolucion(clienteId, articuloId);
                if (dev == null)
                {
                    MostrarError("No existe una devolución asociada a este cliente y artículo.");
                    return;
                }

                DevolucionSeleccionada = dev;
                DialogResult = true;
            }
            catch (OracleException ex)
            {
                MostrarError("Error al buscar la devolución: " + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarError("Error inesperado: " + ex.Message);
            }
        }

        // Muestra un cuadro de diálogo de error
        private void MostrarError(string mensaje)
        {
            new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Error",
                TextoBotonIzquierdo = "Entendido"
            }.ShowDialog();
        }
    }
}