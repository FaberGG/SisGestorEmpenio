using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarArticulo.xaml
    /// </summary>
    public partial class RegistrarArticulo : UserControl
    {
        public event EventHandler<Articulo> RegistroArticuloCompletado;

        public RegistrarArticulo()
        {
            InitializeComponent();

            // Establecer longitudes máximas de entrada
            txtID.MaxLength = 10;
            txtDescripcion.MaxLength = 200;
            txtValor.MaxLength = 15;

            // Validaciones automáticas con LostFocus
            txtID.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtID, lblID, "ID");
            txtDescripcion.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtDescripcion, lblDescripcion, "Descripción", 5, 200);
            cbEstado.LostFocus += (s, e) => ValidacionHelper.ValidarCampo(cbEstado, lblEstado, "Estado");
            txtValor.LostFocus += (s, e) => ValidacionHelper.ValidarDecimal(txtValor, lblValor, "Valor");

            // Limitar la cantidad de caracteres en tiempo real (opcional extra de seguridad)
            txtID.TextChanged += (s, e) => {
                if (txtID.Text.Length > 10)
                    txtID.Text = txtID.Text.Substring(0, 10);
            };

            txtDescripcion.TextChanged += (s, e) => {
                if (txtDescripcion.Text.Length > 200)
                    txtDescripcion.Text = txtDescripcion.Text.Substring(0, 200);
            };

            txtValor.TextChanged += (s, e) => {
                if (txtValor.Text.Length > 15)
                    txtValor.Text = txtValor.Text.Substring(0, 15);
            };
        }

        // Método para permitir solo números o decimales (si decides activarlo)
        private void SoloNumero_Preview(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as TextBox;
            e.Handled = !ValidacionHelper.EsDecimal(e.Text, tb.Text);
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            bool ok =
                ValidacionHelper.ValidarEntero(txtID, lblID, "ID") &
                ValidacionHelper.ValidarLongitud(txtDescripcion, lblDescripcion, "Descripción", 5, 200) &
                ValidacionHelper.ValidarCampo(cbEstado, lblEstado, "Estado") &
                ValidacionHelper.ValidarDecimal(txtValor, lblValor, "Valor");

            if (!ok)

            {
                MostrarMensaje("Corrige los campos resaltados.", "Advertencia");
                return;
            }

            var art = new Articulo(
                int.Parse(txtID.Text),
                txtDescripcion.Text.Trim(),
                double.Parse(txtValor.Text),
                cbEstado.Text
            );

            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarArticulo(art);
                if (completado)
                {
                    MostrarMensaje("Artículo registrado exitosamente.", "Éxito");
                    RegistroArticuloCompletado?.Invoke(this, art);
                }
                else
                {
                    MostrarMensaje("No se pudo registrar el artículo.", "Error");
                }
            }
            catch (OracleException ex)
            {
                MostrarMensaje($"Error de base de datos:\n{ex.Message}", "Error");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Ocurrió un error inesperado:\n{ex.Message}", "Error");
            }
        }

        private void MostrarMensaje(string mensaje, string titulo)
        {
            new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = titulo,
                TextoBotonIzquierdo = "Entendido"
            }.ShowDialog();
        }
    }
}