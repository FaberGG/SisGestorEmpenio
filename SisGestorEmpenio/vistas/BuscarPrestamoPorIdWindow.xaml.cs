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
using System.Windows.Shapes;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para BuscarPrestamoPorIdWindow.xaml
    /// </summary>
    public partial class BuscarPrestamoPorIdWindow : Window
    {

        // Propiedades para almacenar los datos del préstamo
        public Prestamo Prestamo { get; set; }


        public BuscarPrestamoPorIdWindow()
        {
            InitializeComponent();


            // 1) Máximos de caracteres
            txtIdCliente.MaxLength = 10;
            txtIdArticulo.MaxLength = 10;


            // 2) Validaciones automáticas con LostFocus
            txtIdCliente.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "identificacion del cliente");
            txtIdArticulo.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Identificador del artículo");

            // 3) Prevención de caracteres inválidos mientras digita
            txtIdCliente.PreviewTextInput += SoloNumeros_Preview;
            txtIdArticulo.PreviewTextInput += SoloNumeros_Preview;
        }

        // Permite sólo dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

            bool ok =
                ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "Identifiacion del cliente") &
                ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Identificador del artículo");
            if (!ok)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            // Parseo seguro
            int idCliente = int.Parse(txtIdCliente.Text.Trim());
            int idArticulo = int.Parse(txtIdArticulo.Text.Trim());



            // Lógica para buscar el préstamo por ID
            var admin = Sesion.Sesion.GetAdministradorActivo();

            try
            {
                //construccion de objetos
                var prestamo = admin.buscarPrestamo(idCliente, idArticulo);

                // Validar que el prestamo exista
                if (prestamo == null)
                {
                    MostrarError("No existe un préstamo asociado a este cliente y articulo.");
                    return;
                }
                this.Prestamo = prestamo;
                this.DialogResult = true; // Indica que la búsqueda fue exitosa
            }
            catch (OracleException ex)
            {
                // Manejo de excepciones específicas de Oracle
                MostrarError("Error al buscar el préstamo: " + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                MostrarError("Error inesperado: " + ex.Message);
                return;
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
