using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace SisGestorEmpenio.vistas
{
    public partial class ModificarDevolucion : UserControl
    {
        private Devolucion _devolucionOriginal;

        public ModificarDevolucion(Devolucion devolucion)
        {
            InitializeComponent();

            _devolucionOriginal = devolucion ?? throw new ArgumentNullException(nameof(devolucion));

            // 1) Máximos de caracteres
            txtIdCliente.MaxLength = 16;
            txtIdArticulo.MaxLength = 10;
            txtMontoTotal.MaxLength = 15;

            // 2) Validaciones automáticas con LostFocus
            txtIdCliente.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "identificación del cliente");
            txtIdArticulo.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "identificador del artículo");
            txtMontoTotal.LostFocus += (s, e) => ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "monto total");

            // 3) Prevención de caracteres inválidos mientras digita
            txtIdCliente.PreviewTextInput += SoloNumeros_Preview;
            txtIdArticulo.PreviewTextInput += SoloNumeros_Preview;

            // 4) Poblar campos con datos existentes
            txtIdCliente.Text = _devolucionOriginal.GetPrestamo().GetCliente().GetId().ToString();
            txtIdArticulo.Text = _devolucionOriginal.GetPrestamo().GetArticulo().GetIdArticulo().ToString();
            txtMontoTotal.Text = _devolucionOriginal.GetMontoPagado().ToString("F2");

            // Deshabilitar cambios en ID (PK)
            txtIdCliente.IsEnabled = false;
            txtIdArticulo.IsEnabled = false;
        }

        // Permite sólo dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void Modificar_Click(object sender, MouseButtonEventArgs e)
        {
            // Validar sólo el monto
            bool ok = ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "Monto Total");
            if (!ok)
            {
                MostrarError("Corrige el monto antes de continuar.");
                return;
            }

            // Parseo seguro del nuevo monto
            double nuevoMonto = double.Parse(txtMontoTotal.Text.Trim());
            _devolucionOriginal.SetMontoPagado(nuevoMonto);

            var admin = SisGestorEmpenio.Sesion.Sesion.GetAdministradorActivo();
            try
            {
                // Llamada a la capa de datos para actualizar
                bool completado = admin.ActualizarDevolucion(_devolucionOriginal);

                if (completado)
                    MostrarExito("Devolución modificada exitosamente.");
                else
                    MostrarError("No se pudo modificar la devolución.");
            }
            catch (OracleException ex) when (ex.Number == 1017)
            {
                MostrarError("No se pudo conectar a la base de datos.\nVerifique su conexión o comuníquese con soporte técnico.");
            }
            catch (OracleException ex)
            {
                MostrarError($"Error en base de datos:\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error inesperado:\n" + ex.Message);
            }
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

        private void MostrarExito(string mensaje)
        {
            new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Éxito",
                TextoBotonIzquierdo = "Entendido"
            }.ShowDialog();
        }
    }
}