using System;
using System.Windows;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Input;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarArticulo.xaml
    /// </summary>
    public partial class RegistrarArticulo : UserControl
    {
        // Evento público que se disparará al completar el registro del artículo
        public event EventHandler RegistroArticuloCompletado;

        public RegistrarArticulo()
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
            string nombre = txtID.Text.Trim();
            string apellido = txtDescripcion.Text.Trim();
            string correo = cbEstado.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string tipoIdentidad = cbTipoIdentidad.Text.Trim();
           Vis

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
            MessageBox.Show(
                $"Nombre: {nombre}\nApellido: {apellido}\nCorreo: {correo}\nID: {id}\nTeléfono: {telefono}\nTipo de Identidad: {tipoIdentidad}",
                "Datos capturados", MessageBoxButton.OK, MessageBoxImage.Information);



            //PASAR LOS DATOS A ADMINISTRADOR PARA EJECUTAR LA CONSULTA

            try
            {
                Sesion.Sesion.GetAdministradorActivo().registrarCliente(nombre, id, tipoIdentidad, apellido, telefono, correo);
                // Disparar el evento para continuar con el flujo
            }
            catch (OracleException ex)
            {

                MostrarError("Error de base de datos:\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error inesperado:\n" + ex.Message);
            }

            RegistroClienteCompletado?.Invoke(this, EventArgs.Empty);
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
