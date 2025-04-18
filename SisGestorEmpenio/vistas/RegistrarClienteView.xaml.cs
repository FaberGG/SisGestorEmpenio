using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using System;
using System.Windows;
using System.Windows.Controls;
using SisGestorEmpenio.Utils;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarCliente.xaml
    /// </summary>
    public partial class RegistrarCliente : UserControl
    {
        public event EventHandler<Cliente> RegistroClienteCompletado;

        public RegistrarCliente()
        {
            InitializeComponent();

            // Longitudes máximas de entrada
            txtNombre.MaxLength = 30;
            txtApellido.MaxLength = 30;
            txtCorreo.MaxLength = 40;
            txtTelefono.MaxLength = 18;
            txtIdentidad.MaxLength = 16;

            // Validaciones con LostFocus
            txtNombre.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtNombre, lblNombre, "Nombre", 2, 30);
            txtApellido.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtApellido, lblApellido, "Apellido", 2, 30);
            txtCorreo.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtCorreo, lblCorreo, "Correo", 5, 40);
            txtTelefono.LostFocus += ValidarTelefono;
            txtIdentidad.LostFocus += ValidarIdentidad;
            cbTipoIdentidad.LostFocus += (s, e) => ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo de Identidad");
        }

        private void ValidarTelefono(object sender, RoutedEventArgs e)
        {
            string telefono = txtTelefono.Text.Trim();
            if (string.IsNullOrWhiteSpace(telefono) || !Regex.IsMatch(telefono, @"^[1-9]\d{9,17}$"))
            {
                lblTelefono.Text = "El número de teléfono no es válido.";
                lblTelefono.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                lblTelefono.Text = "";
            }
        }

        private void ValidarIdentidad(object sender, RoutedEventArgs e)
        {
            string identidad = txtIdentidad.Text.Trim();
            if (string.IsNullOrWhiteSpace(identidad) || !Regex.IsMatch(identidad, @"^[1-9]\d{0,12}$"))
            {
                lblIdentidad.Text = "La identidad debe contener solo números y no empezar con 0.";
                lblIdentidad.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                lblIdentidad.Text = "";
            }
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;

            valido &= ValidacionHelper.ValidarLongitud(txtNombre, lblNombre, "Nombre", 2, 30);
            valido &= ValidacionHelper.ValidarLongitud(txtApellido, lblApellido, "Apellido", 2, 30);
            valido &= ValidacionHelper.ValidarLongitud(txtCorreo, lblCorreo, "Correo", 5, 40);
            valido &= ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo de Identidad");

            // Validación personalizada con Regex
            if (!Regex.IsMatch(txtTelefono.Text.Trim(), @"^[1-9]\d{9,17}$"))
            {
                lblTelefono.Text = "El número de teléfono no es válido.";
                lblTelefono.Foreground = new SolidColorBrush(Colors.Red);
                valido = false;
            }
            else
            {
                lblTelefono.Text = "";
            }

            if (!Regex.IsMatch(txtIdentidad.Text.Trim(), @"^[1-9]\d{0,12}$"))
            {
                lblIdentidad.Text = "La identidad debe contener solo números y no empezar con 0.";
                lblIdentidad.Foreground = new SolidColorBrush(Colors.Red);
                valido = false;
            }
            else
            {
                lblIdentidad.Text = "";
            }

            if (!valido)
            {
                MostrarMensaje("Corrige los campos resaltados.", "Advertencia");
                return;
            }

            var cliente = new Cliente(
                txtNombre.Text.Trim(),
                int.Parse(txtIdentidad.Text.Trim()),
                cbTipoIdentidad.Text.Trim(),
                txtApellido.Text.Trim(),
                txtTelefono.Text.Trim(),
                txtCorreo.Text.Trim()
            );

            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarCliente(cliente);
                if (completado)
                {
                    MostrarMensaje("Cliente registrado exitosamente.", "Éxito");
                    RegistroClienteCompletado?.Invoke(this, cliente);
                }
                else
                {
                    MostrarMensaje("No se pudo registrar el cliente.", "Error");
                }
            }
            catch (OracleException ex)
            {
                MostrarMensaje("Error de base de datos:\n" + ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error inesperado:\n" + ex.Message, "Error");
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