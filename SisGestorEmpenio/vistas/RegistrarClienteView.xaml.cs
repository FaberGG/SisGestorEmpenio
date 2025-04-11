using SisGestorEmpenio.Modelos;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarCliente.xaml
    /// </summary>
    public partial class RegistrarCliente : UserControl
    {
        // Evento público que se disparará al finalizar el registro
        public event EventHandler RegistroClienteCompletado;

        public RegistrarCliente()
        {
            InitializeComponent();
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            // Capturar valores del formulario
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string tipoIdentidad = txtTipoIdentidad.Text.Trim();
            string idTexto = txtID.Text.Trim();

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

            // Disparar el evento para continuar con el flujo
            RegistroClienteCompletado?.Invoke(this, EventArgs.Empty);
        }
    }
}
