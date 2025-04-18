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
    /// <summary>
    /// Lógica de interacción para RegistrarDevolucion.xaml
    /// </summary>
    public partial class RegistrarDevolucion : UserControl
    {
        private Cliente cliente;
        private Articulo articulo;

        public RegistrarDevolucion()
        {
            InitializeComponent();

            // Establecer máximos de caracteres permitidos
            txtIdCliente.MaxLength = 16;
            txtIdArticulo.MaxLength = 10;
            txtMontoTotal.MaxLength = 10;

            // Validación automática con LostFocus
            txtIdCliente.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "Cliente ID");
            txtIdArticulo.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Artículo ID");
            txtMontoTotal.LostFocus += (s, e) => ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "Monto Total");
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            bool valido =
                ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "Cliente ID") &
                ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Artículo ID") &
                ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "Monto Total");

            if (!valido)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            // Convertir valores a sus tipos
            int idCliente = int.Parse(txtIdCliente.Text.Trim());
            int idArticulo = int.Parse(txtIdArticulo.Text.Trim());
            double montoTotal = double.Parse(txtMontoTotal.Text.Trim());

            // Crear el préstamo y la devolución
            cliente = new Cliente("", idCliente, "", "", "", "");
            articulo = new Articulo(idArticulo, "", 0.0, "");
            Prestamo prestamo = new Prestamo(cliente, articulo, DateTime.Now, 0.0);
            var devolucion = new Devolucion(montoTotal, prestamo);

            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarDevolución(devolucion);
                if (completado)
                {
                    MostrarExito("Devolución registrada exitosamente.");
                }
                else
                {
                    MostrarError("No se pudo registrar.");
                }
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
            var ventanaError = new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Error",
                TextoBotonIzquierdo = "Entendido",
            };

            ventanaError.ShowDialog();
        }

        private void MostrarExito(string mensaje)
        {
            var ventanaExito = new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Éxito",
                TextoBotonIzquierdo = "Entendido.",
            };

            ventanaExito.ShowDialog();
        }
    }
}

