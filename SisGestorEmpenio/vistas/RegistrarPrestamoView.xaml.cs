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
    /// Lógica de interacción para RegistrarPrestamo.xaml
    /// </summary>
    public partial class RegistrarPrestamo : UserControl
    {

        private Cliente cliente;
        private Articulo articulo;

        public RegistrarPrestamo(Cliente cliente, Articulo articulo)
        {
            InitializeComponent();
            this.cliente = cliente;
            this.articulo = articulo;

            //txtClienteId = cliente.GetId();
            //txtArticuloId = articulo.GetId();
        }
        public RegistrarPrestamo(Articulo articulo)
        {
            this.articulo = articulo;
            this.cliente = null;
            //txtArticuloId = articulo.GetId();
            InitializeComponent();
        }

        public RegistrarPrestamo()
        {
            this.cliente = null;
            this.articulo = null;

            InitializeComponent();
        }

        private void txtSoloNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsNumero(e.Text);
        }

        private bool EsNumero(string texto)
        {
            return int.TryParse(texto, out _);
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            // Capturar valores del formulario
            /*
            string clienteId = txtClienteId.Text.Trim();
            string articuloId = txtArticuloId.Text.Trim();
            string tasaInteres = txtTasaInteres.Text.Trim();
            //fecha fin


            // Validación básica de campos
            if (string.IsNullOrWhiteSpace(clienteID) ||
                string.IsNullOrWhiteSpace(articuloId) ||
                string.IsNullOrWhiteSpace(tasaInteres) ||
                // para las fechas
                )
                
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Convertir ID a entero
            if (!int.TryParse(idTexto, out int id))
            {
                MessageBox.Show("El campo ID debe ser un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            */



            // Mostrar datos capturados (prueba)
            /*
            MessageBox.Show(
                $"Nombre: {nombre}\nApellido: {apellido}\nCorreo: {correo}\nID: {id}\nTeléfono: {telefono}\nTipo de Identidad: {tipoIdentidad}",
                "Datos capturados", MessageBoxButton.OK, MessageBoxImage.Information);
            */


            //PASAR LOS DATOS A ADMINISTRADOR PARA EJECUTAR LA CONSULTA
            /*
            var prestamo = new Prestamo(cliente, articulo, fechaFin, tasaInteres);
            
            try
            {
                Sesion.Sesion.GetAdministradorActivo().registrarPrestamo(prestamo);
            }
            catch (OracleException ex)
            {

                MostrarError("Error de base de datos:\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error inesperado:\n" + ex.Message);
            }
            */

        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
