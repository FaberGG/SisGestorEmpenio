using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarCliente.xaml
    /// </summary>
    public partial class RegistrarCliente : UserControl
    {
        // Evento público que se disparará al finalizar el registro
        public event EventHandler<Cliente> RegistroClienteCompletado;

        public RegistrarCliente()
        {
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
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string tipoIdentidad = cbTipoIdentidad.Text.Trim();
            string idTexto = txtIdentidad.Text.Trim();

            // Validación básica de campos
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(tipoIdentidad) ||
                string.IsNullOrWhiteSpace(idTexto))
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

            
            

            // Mostrar datos capturados (prueba)
            /*
            MessageBox.Show(
                $"Nombre: {nombre}\nApellido: {apellido}\nCorreo: {correo}\nID: {id}\nTeléfono: {telefono}\nTipo de Identidad: {tipoIdentidad}",
                "Datos capturados", MessageBoxButton.OK, MessageBoxImage.Information);
            */


            //PASAR LOS DATOS A ADMINISTRADOR PARA EJECUTAR LA CONSULTA

            var cliente = new Cliente(nombre, id, tipoIdentidad, apellido, telefono, correo);
            try
            {
                Sesion.Sesion.GetAdministradorActivo().registrarCliente(cliente);
                RegistroClienteCompletado?.Invoke(this, cliente);
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
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
