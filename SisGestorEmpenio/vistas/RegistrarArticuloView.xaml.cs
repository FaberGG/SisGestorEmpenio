using System;
using System.Windows;
using System.Windows.Controls;

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

            // Limpiamos las cajitas de texto
            txtNombre.GotFocus += RemovePlaceholder;
            txtPrecio.GotFocus += RemovePlaceholder;
            txtTipo.GotFocus += RemovePlaceholder;
            txtID.GotFocus += RemovePlaceholder;
            txtInteres.GotFocus += RemovePlaceholder;
        }

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null && (txt.Text == "Name" || txt.Text == "Price" || txt.Text == "Type" || txt.Text == "ID" || txt.Text == "Interest percent"))
            {
                txt.Text = "";
                txt.Foreground = System.Windows.Media.Brushes.Black;
                txt.FontStyle = FontStyles.Normal;
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || txtNombre.Text == "Name" ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) || txtPrecio.Text == "Price" ||
                string.IsNullOrWhiteSpace(txtTipo.Text) || txtTipo.Text == "Type" ||
                string.IsNullOrWhiteSpace(txtID.Text) || txtID.Text == "ID" ||
                string.IsNullOrWhiteSpace(txtInteres.Text) || txtInteres.Text == "Interest percent")
            {
                MessageBox.Show("Por favor, completa todos los campos obligatorios.", "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Artículo registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            // Aquí ya podrías guardar los datos en la BD

            // Disparar el evento para continuar con el flujo
            RegistroArticuloCompletado?.Invoke(this, EventArgs.Empty);
        }
    }
}
