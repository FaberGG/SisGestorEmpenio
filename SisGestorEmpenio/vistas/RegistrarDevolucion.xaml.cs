using Oracle.ManagedDataAccess.Client;
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
    /// Lógica de interacción para RegistrarDevolucion.xaml
    /// </summary>
    public partial class RegistrarDevolucion : UserControl
    {
        private Cliente cliente;
        private Articulo articulo;

        public RegistrarDevolucion()
        {
            InitializeComponent();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Capturar valores del formulario
            string strclienteId = txtIdCliente.Text.Trim();
            string strarticuloId = txtIdArticulo.Text.Trim();
            string strMontoTotal = txtMontoTotal.Text.Trim();

            // Validación básica de campos
            if (string.IsNullOrWhiteSpace(strclienteId) ||
                string.IsNullOrWhiteSpace(strarticuloId) ||
                string.IsNullOrWhiteSpace(strMontoTotal))
            {
                MostrarError("Todos los campos son obligatorios.");
                return;
            }

            // Convertir ID a entero
            if (!int.TryParse(strclienteId, out int idCliente))
            {
                MostrarError("El campo Identificación del cliente debe ser un número válido.");
                return;
            }

            if (!int.TryParse(strarticuloId, out int idArticulo))
            {
                MostrarError("El campo Identificador del artículo debe ser un número válido.");
                return;
            }

            // Convertir tasa de interés a double
            if (!double.TryParse(strMontoTotal, out double MontoTotal))
            {
                MostrarError("El campo de El Monto Total debe ser un número decimal válido.");
                return;
            }


            // Crear el préstamo y registrarlo
            cliente = new Cliente("", idCliente, "", "", "", "");
            articulo = new Articulo(idArticulo, "", 0.0, "");
            Prestamo prestamo = new Prestamo(cliente, articulo, DateTime.Now, 0.0);

            var devolucion = new Devolucion(MontoTotal, prestamo);


            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarDevolución(devolucion);
                // Mostrar mensaje de éxito
                if (completado)
                {
                    MostrarExito("Devolucion registrada exitosamente.");
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
                TextoBotonIzquierdo = "Entendido",
            };
            ventanaExito.ShowDialog();
        }

    }
}
