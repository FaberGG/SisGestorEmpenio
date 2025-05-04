using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;
using SisGestorEmpenio.Sesion;
using System;
using System.Windows;
using System.Windows.Input;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para BuscarDevolucionPorIdWindow.xaml
    /// </summary>
    public partial class BuscarDevolucionPorIdWindow : Window
    {
        // Propiedad para almacenar la devolución encontrada
        public Devolucion DevolucionSeleccionada { get; private set; }

        public BuscarDevolucionPorIdWindow()
        {
            InitializeComponent();

            // Máximos de caracteres
            txtClienteId.MaxLength = 10;
            txtArticuloId.MaxLength = 10;

            // Validaciones automáticas con LostFocus
            txtClienteId.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtClienteId, lblIdCliente, "identificación del cliente");
            txtArticuloId.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtArticuloId, lblIdArticulo, "identificador del artículo");

            // Prevención de caracteres inválidos mientras digita
            txtClienteId.PreviewTextInput += SoloNumeros_Preview;
            txtArticuloId.PreviewTextInput += SoloNumeros_Preview;
        }

        // Sólo dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void btnBuscar_Click(object sender, MouseButtonEventArgs e)
        {
            // Validar ambos campos
            bool ok =
                ValidacionHelper.ValidarEntero(txtClienteId, lblIdCliente, "Identificación del cliente") &
                ValidacionHelper.ValidarEntero(txtArticuloId, lblIdArticulo, "Identificador del artículo");

            if (!ok)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            // Parseo seguro
            int clienteId = int.Parse(txtClienteId.Text.Trim());
            int articuloId = int.Parse(txtArticuloId.Text.Trim());

            var admin = Sesion.Sesion.GetAdministradorActivo();
            try
            {
                // Buscar la devolución
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

        private void btnCancelar_Click(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

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
