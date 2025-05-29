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
    public partial class ClienteView : UserControl
    {
        public event EventHandler<Cliente> RegistroClienteCompletado;
        private bool _isAdding = true;
        public ClienteView()
        {
            InitializeComponent();

            cargarValidaciones();
        }

        //constructor para ver y editar un cliente existente
        public ClienteView(Cliente cliente) : this()
        {
            InitializeComponent();
            _isAdding = false; // Cambiar a modo edición
            btnLabel.Text = "Guardar Cambios";

            // Cargar datos del cliente en los campos
            txtIdentidad.IsEnabled = false; // Deshabilitar el campo de ID
            cbTipoIdentidad.IsEnabled = false; // Deshabilitar el campo de tipo de identidad
            txtNombre.Text = cliente.GetNombre();
            txtApellido.Text = cliente.GetApellido();
            txtIdentidad.Text = cliente.GetId().ToString();
            cbTipoIdentidad.Text = cliente.GetTipoIdentidad();
            txtTelefono.Text = cliente.GetTelefono();
            txtCorreo.Text = cliente.GetCorreo();

            cargarValidaciones();

        }

        //cargar validaciones

        private void cargarValidaciones()
        {
            // Máximos de caracteres
            txtNombre.MaxLength = 30;
            txtApellido.MaxLength = 30;
            txtCorreo.MaxLength = 40;
            txtTelefono.MaxLength = 18;
            txtIdentidad.MaxLength = 10;

            // PreviewTextInput: sólo dígitos donde corresponda
            txtTelefono.PreviewTextInput += SoloNumeros_Preview;
            txtIdentidad.PreviewTextInput += SoloNumeros_Preview;

            // LostFocus: validación inline
            txtNombre.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtNombre, lblNombre, "Nombre*", 2, 30);
            txtApellido.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtApellido, lblApellido, "Apellido*", 2, 30);
            txtCorreo.LostFocus += (s, e) => ValidarCorreo();
            txtTelefono.LostFocus += (s, e) => ValidarTelefono();
            txtIdentidad.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdentidad, lblIdentidad, "Número de identidad*");
            cbTipoIdentidad.LostFocus += (s, e) => ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo de Identidad*");
        }

        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        private bool ValidarCorreo()
        {
            var txt = txtCorreo.Text.Trim();
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrWhiteSpace(txt))
            {
                lblCorreo.Text = "Correo es obligatorio.";
                lblCorreo.Foreground = Brushes.Red;
                return false;
            }
            else if (!Regex.IsMatch(txt, pattern))
            {
                lblCorreo.Text = "Formato de correo inválido.";
                lblCorreo.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                lblCorreo.Text = "Correo*";
                lblCorreo.Foreground = Brushes.Black;
                return true;
            }
        }

        private bool ValidarTelefono()
        {
            var txt = txtTelefono.Text.Trim();
            // 10 a 18 dígitos, no empieza con 0
            if (string.IsNullOrWhiteSpace(txt) || !Regex.IsMatch(txt, @"^[1-9]\d{9,17}$"))
            {
                lblTelefono.Text = "Teléfono inválido (10–18 dígitos, sin ceros).";
                lblTelefono.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                lblTelefono.Text = "Telefono*";
                lblTelefono.Foreground = Brushes.Black;
                return true;
            }
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;

            valido &= ValidacionHelper.ValidarLongitud(txtNombre, lblNombre, "Nombre*", 2, 30);
            valido &= ValidacionHelper.ValidarLongitud(txtApellido, lblApellido, "Apellido*", 2, 30);
            valido &= ValidarCorreo();
            valido &= ValidarTelefono();
            valido &= ValidacionHelper.ValidarEntero(txtIdentidad, lblIdentidad, "Número de identidad*");
            valido &= ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo de Identidad*");
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
                if(!_isAdding)
                {
                    // Si no es un nuevo cliente, actualizar el cliente existente
                    bool actualizado = Sesion.Sesion.GetAdministradorActivo().ActualizarCliente(cliente);
                    if (actualizado)
                    {
                        MostrarMensaje("Cliente actualizado exitosamente.", "Éxito");
                        RegistroClienteCompletado?.Invoke(this, cliente);
                    }
                    else
                    {
                        MostrarMensaje("No se pudo actualizar el cliente.", "Error");
                    }
                    return;
                }
                // Validar que el cliente no exista en caso de estar en registro
                if (Sesion.Sesion.GetAdministradorActivo().ExisteCliente(cliente))
                {
                    MostrarMensaje("EL CLIENTE YA EXISTE: \n Un cliente con esta identificacion ya esta registrado", "Error");
                    return;
                }

                bool completado = Sesion.Sesion.GetAdministradorActivo().RegistrarCliente(cliente);
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
            catch (OracleException ex) when (ex.Number == 1017)
            {
                MostrarMensaje("No se pudo conectar a la base de datos.\nVerifique su conexión o comuníquese con soporte técnico.", "Error");
                return;
            }
            catch (OracleException ex)
            {
                MostrarMensaje($"Error en la base de datos:\n{ex.Message}", "Error");
                return;
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