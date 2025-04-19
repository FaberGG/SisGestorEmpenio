using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;
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
    public partial class RegistrarDevolucion : UserControl
    {
        public RegistrarDevolucion()
        {
            InitializeComponent();

            // 1) Máximos de caracteres
            txtIdCliente.MaxLength = 16;
            txtIdArticulo.MaxLength = 10;
            txtMontoTotal.MaxLength = 15;

            // 2) Validaciones automáticas con LostFocus
            txtIdCliente.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "identificacion del cliente");
            txtIdArticulo.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Identificador del artículo");
            txtMontoTotal.LostFocus += (s, e) => ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "Monto Total");

            // 3) Prevención de caracteres inválidos mientras digita
            txtIdCliente.PreviewTextInput += SoloNumeros_Preview;
            txtIdArticulo.PreviewTextInput += SoloNumeros_Preview;
            txtMontoTotal.PreviewTextInput += SoloDecimal_Preview;
        }

        // Permite sólo dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        // Permite dígitos y un sólo punto
        private void SoloDecimal_Preview(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;
            else if (e.Text == "." && tb.Text.Contains("."))
                e.Handled = true;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            bool ok =
                ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "Identifiacion del cliente") &
                ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Identificador del artículo") &
                ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "Monto Total");

            if (!ok)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            // Parseo seguro
            int idCliente = int.Parse(txtIdCliente.Text.Trim());
            int idArticulo = int.Parse(txtIdArticulo.Text.Trim());
            double monto = double.Parse(txtMontoTotal.Text.Trim());

            var admin = Sesion.Sesion.GetAdministradorActivo();

            


            // Construcción de objetos
            var cliente = new Cliente("", idCliente, "", "", "", "");
            var articulo = new Articulo(idArticulo, "", 0.0, "");
            var prestamo = new Prestamo(cliente, articulo, DateTime.Now, 0.0);
            var devolucion = new Devolucion(monto, prestamo);

            try
            {
                bool completado = admin.registrarDevolución(devolucion);
                if (completado)
                    MostrarExito("Devolución registrada exitosamente.");
                else
                    MostrarError("No se pudo registrar la devolución.");
            }
            catch (OracleException ex)
            {
                MostrarError("Error de base de datos:\n" + ex.Message);
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
