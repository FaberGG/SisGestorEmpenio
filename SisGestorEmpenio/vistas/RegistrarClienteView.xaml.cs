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
    public partial class RegistrarCliente : UserControl
    {
        public event EventHandler<Cliente> RegistroClienteCompletado;
        private readonly int adminId;

        public RegistrarCliente()
        {
            InitializeComponent();

            // Suponemos que el administrador activo aporta su ID
            adminId = Sesion.Sesion.GetAdministradorActivo().GetId();

            // Máximos de caracteres
            txtNombre.MaxLength = 30;
            txtApellido.MaxLength = 30;
            txtCorreo.MaxLength = 40;
            txtTelefono.MaxLength = 18;
            txtIdentidad.MaxLength = 16;

            // PreviewTextInput: sólo dígitos donde corresponda
            txtTelefono.PreviewTextInput += SoloNumeros_Preview;
            txtIdentidad.PreviewTextInput += SoloNumeros_Preview;

            // LostFocus: validación inline
            txtNombre.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtNombre, lblNombre, "Nombre", 2, 30);
            txtApellido.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtApellido, lblApellido, "Apellido", 2, 30);
            txtCorreo.LostFocus += (s, e) => ValidarCorreo();
            txtTelefono.LostFocus += (s, e) => ValidarTelefono();
            txtIdentidad.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdentidad, lblIdentidad, "Identidad");
            cbTipoIdentidad.LostFocus += (s, e) => ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo de Identidad");
        }

        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        private void ValidarCorreo()
        {
            var txt = txtCorreo.Text.Trim();
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrWhiteSpace(txt))
            {
                lblCorreo.Text = "Correo es obligatorio.";
                lblCorreo.Foreground = Brushes.Red;
            }
            else if (!Regex.IsMatch(txt, pattern))
            {
                lblCorreo.Text = "Formato de correo inválido.";
                lblCorreo.Foreground = Brushes.Red;
            }
            else
            {
                lblCorreo.Text = "";
            }
        }

        private void ValidarTelefono()
        {
            var txt = txtTelefono.Text.Trim();
            // 10 a 18 dígitos, no empieza con 0
            if (string.IsNullOrWhiteSpace(txt) || !Regex.IsMatch(txt, @"^[1-9]\d{9,17}$"))
            {
                lblTelefono.Text = "Teléfono inválido (10–18 dígitos, sin ceros).";
                lblTelefono.Foreground = Brushes.Red;
            }
            else
            {
                lblTelefono.Text = "";
            }
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;

            valido &= ValidacionHelper.ValidarLongitud(txtNombre, lblNombre, "Nombre", 2, 30);
            valido &= ValidacionHelper.ValidarLongitud(txtApellido, lblApellido, "Apellido", 2, 30);
            ValidarCorreo(); valido &= string.IsNullOrEmpty(lblCorreo.Text);
            ValidarTelefono(); valido &= string.IsNullOrEmpty(lblTelefono.Text);
            valido &= ValidacionHelper.ValidarEntero(txtIdentidad, lblIdentidad, "Identidad");
            valido &= ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo de Identidad");
            if (!valido)
            {
                MostrarMensaje("Corrige los campos resaltados.", "Advertencia");
                return;
            }

            // Crear cliente con foreign key a admin
            var admin = Sesion.Sesion.GetAdministradorActivo();
            var cliente = new Cliente(
                txtNombre.Text.Trim(),
                int.Parse(txtIdentidad.Text.Trim()),
                cbTipoIdentidad.Text.Trim(),
                txtApellido.Text.Trim(),
                txtTelefono.Text.Trim(),
                txtCorreo.Text.Trim(),
                admin
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