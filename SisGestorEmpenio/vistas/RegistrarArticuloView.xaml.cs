using System;
using System.Windows;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Input;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarArticulo.xaml
    /// </summary>
    public partial class RegistrarArticulo : UserControl
    {
        // Evento público que se disparará al completar el registro del artículo
        public event EventHandler<Articulo> RegistroArticuloCompletado;

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
            string idTexto = txtID.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            string estado = cbEstado.Text.Trim();
            string valorTexto = txtValor.Text.Trim();
            double valor;
           

            // Validación básica de campos
            if (string.IsNullOrWhiteSpace(idTexto) ||
                string.IsNullOrWhiteSpace(descripcion) ||
                string.IsNullOrWhiteSpace(estado) ||
                string.IsNullOrWhiteSpace(valorTexto))
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
            // Convertir Valor a entero
            if (!double.TryParse(valorTexto, out valor))
            {
                MostrarError("El campo Valor debe ser un número válido.");
                return;
            }


            /*
            // Mostrar datos capturados (prueba)
            MessageBox.Show(
                $"ID: {id}\nDescripcion: {descripcion}\nEstado: {estado}\nValor: {valor}",
                "Datos capturados", MessageBoxButton.OK, MessageBoxImage.Information);
            */


            //PASAR LOS DATOS A ADMINISTRADOR PARA EJECUTAR LA CONSULTA
            var articulo = new Articulo(id, descripcion, valor, estado);
            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarArticulo(articulo);
                // Mostrar mensaje de éxito
                if(completado)
                {
                    MostrarExito("Artículo registrado exitosamente.");
                }
                else
                {
                    MostrarError("No se pudo registrar el artículo.");
                }
                RegistroArticuloCompletado?.Invoke(this, articulo);
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
