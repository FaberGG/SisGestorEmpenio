using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SisGestorEmpenio.Utils;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
namespace SisGestorEmpenio.vistas
{
    public partial class RegistrarPrestamo : UserControl
    {
        private Cliente cliente;
        private Articulo articulo;

        public RegistrarPrestamo(Cliente cliente, Articulo articulo)
        {
            InitializeComponent();
            this.cliente = cliente;
            this.articulo = articulo;

            txtClienteId.Text = cliente.GetId().ToString();
            txtArticuloId.Text = articulo.GetIdArticulo().ToString();
            txtClienteId.IsEnabled = false;
            txtArticuloId.IsEnabled = false;

            ConfigurarValidaciones();
        }

        public RegistrarPrestamo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            txtArticuloId.Text = articulo.GetIdArticulo().ToString();
            txtArticuloId.IsEnabled = false;

            ConfigurarValidaciones();
        }

        public RegistrarPrestamo()
        {
            InitializeComponent();
            ConfigurarValidaciones();
        }

        private void ConfigurarValidaciones()
        {
            // Máximos de caracteres
            txtClienteId.MaxLength = 10;
            txtArticuloId.MaxLength = 10;
            txtTasaInteres.MaxLength = 10;

            

            // Validaciones LostFocus
            txtClienteId.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtClienteId, lblClienteId, "Identificacion del Cliente");
            txtArticuloId.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtArticuloId, lblArticuloId, "Identificador del Articulo");
            txtTasaInteres.LostFocus += (s, e) => ValidacionHelper.ValidarDecimal(txtTasaInteres, lblTasaInteres, "Tasa de Interés");
            FechaFinDatePicker.LostFocus += (s, e) =>  ValidarFechaFin();
        }

        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        private void SoloDecimal_Preview(object sender, TextCompositionEventArgs e)
        {
            // Permite dígitos y un punto, un solo punto máximo
            var tb = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }
            else if (e.Text == "." && tb.Text.Contains("."))
            {
                e.Handled = true;
            }
        }


        private bool ValidarFechaFin()
        {
            var sel = FechaFinDatePicker.SelectedDate;
            if (!sel.HasValue || sel.Value.Date <= DateTime.Today)
            {
                lblFechaFin.Text = "Fecha debe ser posterior a hoy";
                lblFechaFin.Foreground = Brushes.Red;
                return false;
            }
            lblFechaFin.Text = "Fecha Fin";
            lblFechaFin.Foreground = Brushes.Black;
            return true;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;

            // Validar campos obligatorios y formatos
            valido &= ValidacionHelper.ValidarEntero(txtClienteId, lblClienteId, "Identificacion del Cliente");
            valido &= ValidacionHelper.ValidarEntero(txtArticuloId, lblArticuloId, "Identificador Artículo");
            valido &= ValidacionHelper.ValidarDecimal(txtTasaInteres, lblTasaInteres, "Tasa de Interés");
            valido &= ValidarFechaFin();


            if (!valido)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            int idCliente = int.Parse(txtClienteId.Text.Trim());
            int idArticulo = int.Parse(txtArticuloId.Text.Trim());
            double tasa = double.Parse(txtTasaInteres.Text.Trim());
            DateTime fecha = FechaFinDatePicker.SelectedDate.Value;

            var cliente = this.cliente;
            var articulo = this.articulo;
            if (cliente == null)
                cliente = new Cliente("", idCliente, "", "", "", "");

            var prestamo = new Prestamo(cliente, articulo, fecha, tasa);


            try
            {

                // Validar que el cliente y el artículo existan
                if (!Sesion.Sesion.GetAdministradorActivo().ExisteCliente(cliente))
                {
                    MostrarError("EL CLIENTE NO REGISTRADO:\n El cliente con este numero de identificacion no ha sido registrado aún");
                    return;
                }
                if (!Sesion.Sesion.GetAdministradorActivo().ExisteArticulo(articulo))
                {
                    MostrarError("El artículo no existe.");
                    return;
                }

                //validar que prestamo no exista
                if (Sesion.Sesion.GetAdministradorActivo().ExistePrestamo(prestamo))
                {
                    MostrarError("EL PRESTAMO YA EXISTE: \n Un prestamo con este cliente y articulo ya esta registrado");
                    return;
                }

            }
            catch (OracleException ex)
            {
                MostrarError($"Error al validar en base de datos:\n{ex.Message}");
            }

            try
            {
                bool exito = Sesion.Sesion.GetAdministradorActivo().registrarPrestamo(prestamo);
                if (exito)
                    MostrarExito("Préstamo registrado exitosamente.");
                else
                    MostrarError("No se pudo registrar el préstamo.");
            }
            catch (OracleException ex)
            {
                MostrarError("Error de base de datos:\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarError("Error inesperado:\n" + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Error",
                TextoBotonIzquierdo = "Entendido"
            }.ShowDialog();
        }

        private void MostrarExito(string mensaje)
        {
            new MensajeErrorOk
            {
                Mensaje = mensaje,
                Titulo = "Éxito",
                TextoBotonIzquierdo = "Entendido"
            }.ShowDialog();
        }
    }
}
