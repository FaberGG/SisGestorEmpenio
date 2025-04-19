using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;

namespace SisGestorEmpenio.vistas
{
    public partial class RegistrarArticulo : UserControl
    {
        // Valores válidos para estadoArticulo en la BD
        private readonly string[] estadosValidos = { "defectuoso", "optimo", "funcionable" };

        public event EventHandler<Articulo> RegistroArticuloCompletado;

        public RegistrarArticulo()
        {
            InitializeComponent();

            // Máximas longitudes según tabla
            txtID.MaxLength = 10;   // INT
            txtDescripcion.MaxLength = 100;  // VARCHAR2(100)
            txtValor.MaxLength = 18;   // DECIMAL(18,2) -> suficiente para "123456789012345.67"

            // Prevención de caracteres inválidos
            txtID.PreviewTextInput += SoloNumeros_Preview;
            txtValor.PreviewTextInput += SoloDecimal_Preview;

            // Validaciones LostFocus
            txtID.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtID, lblID, "Identificador único");
            txtDescripcion.LostFocus += (s, e) => ValidacionHelper.ValidarLongitud(txtDescripcion, lblDescripcion, "Descripción", 5, 100);
            cbEstado.LostFocus += (s, e) => ValidarEstado();
            txtValor.LostFocus += (s, e) => ValidacionHelper.ValidarDecimal(txtValor, lblValor, "Valor Estimado");
        }

        // Sólo dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        // Dígitos y un solo punto decimal
        private void SoloDecimal_Preview(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;
            else if (e.Text == "." && tb.Text.Contains("."))
                e.Handled = true;
        }

        // Valida que el estado esté entre los permitidos
        private bool ValidarEstado()
        {
            var val = cbEstado.Text.Trim().ToLower();
            if (!estadosValidos.Contains(val))
            {
                lblEstado.Text = "Estado inválido";
                lblEstado.Foreground = Brushes.Red;
                return false;
            }
            
            lblEstado.Text = "Estado";
            lblEstado.Foreground = Brushes.Black;
            return true;
            
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            bool ok = true;

            // Validaciones inline
            ok &= ValidacionHelper.ValidarEntero(txtID, lblID, "Identificador único");
            ok &= ValidacionHelper.ValidarLongitud(txtDescripcion, lblDescripcion, "Descripción", 5, 100);
            ok &= ValidarEstado();
            ok &= ValidacionHelper.ValidarDecimal(txtValor, lblValor, "Valor Estimado");

            if (!ok)

            {
                MostrarMensaje("Corrige los campos resaltados.", "Advertencia");
                return;
            }

            // Crear y registrar
            var art = new Articulo(
                int.Parse(txtID.Text.Trim()),
                txtDescripcion.Text.Trim(),
                double.Parse(txtValor.Text.Trim()),
                cbEstado.Text.Trim().ToLower()
            );

            try
            {
                //validar que el articulo no exista
                if (Sesion.Sesion.GetAdministradorActivo().ExisteArticulo(art))
                {
                    MostrarMensaje("EL ARTICULO YA EXISTE: \n Un articulo con este ID ya esta registrado", "Error");
                    return;
                }

            }
            catch (OracleException ex)
            {
                MostrarMensaje($"Error al validar en base de datos:\n{ex.Message}", "Error");
            }


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