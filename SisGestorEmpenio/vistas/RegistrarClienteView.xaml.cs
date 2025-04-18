using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using System;
using System.Windows;
using System.Windows.Controls;
using SisGestorEmpenio.Utils;
using System.Windows.Input;
using System.Windows.Media;

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


            //validacion de campos
            bool nombreValido = ValidacionHelper.ValidarCampo(txtNombre, lblNombre, "Nombre");
            bool apellidoValido = ValidacionHelper.ValidarCampo(txtApellido, lblApellido, "Apellido");
            bool correoValido = ValidacionHelper.ValidarCampo(txtCorreo, lblCorreo, "Correo");
            bool telefonoValido = ValidacionHelper.ValidarCampo(txtTelefono, lblTelefono, "Teléfono");
            bool tipoIdentidadValido = ValidacionHelper.ValidarCampo(cbTipoIdentidad, lblTipoIdentidad, "Tipo Identidad");
            bool identidadValida = ValidacionHelper.ValidarCampo(txtIdentidad, lblIdentidad, "Identidad");

            if (!nombreValido || !apellidoValido || !correoValido || !telefonoValido || !tipoIdentidadValido || !identidadValida)
            {
                MostrarError("Todos los campos son obligatorios.");
                return;
            }

            // Convertir ID a entero
            if (!int.TryParse(idTexto, out int id))
            {
                MostrarError("El campo ID debe ser un número válido.");
                return;
            }

            
            
            /*
            // Mostrar datos capturados (prueba)
            
            MessageBox.Show(
                $"Nombre: {nombre}\nApellido: {apellido}\nCorreo: {correo}\nID: {id}\nTeléfono: {telefono}\nTipo de Identidad: {tipoIdentidad}",
                "Datos capturados", MessageBoxButton.OK, MessageBoxImage.Information);
            
            */

            //PASAR LOS DATOS A ADMINISTRADOR PARA EJECUTAR LA CONSULTA

            var cliente = new Cliente(nombre, id, tipoIdentidad, apellido, telefono, correo);
            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarCliente(cliente);
                // Mostrar mensaje de éxito
                if (completado)
                {
                    MostrarExito("Cliente registrado exitosamente.");
                }
                else
                {
                    MostrarError("No se pudo registrar el cliente.");
                }
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
            var ventanaError = new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Error",
                TextoBotonIzquierdo = "Entendido",
            };

            ventanaError.ShowDialog();
        }
        private void MostrarExito(string mensaje)
        {
            var ventanaExito = new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Éxito",
                TextoBotonIzquierdo = "Entendido",
            };
            ventanaExito.ShowDialog();
        }

    }
}
