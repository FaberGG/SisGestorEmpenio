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
using static MaterialDesignThemes.Wpf.Theme;
namespace SisGestorEmpenio.vistas
{
    public partial class PrestamoView: UserControl
    {
        private Cliente cliente;
        private Articulo articulo;
        private Prestamo prestamo;
        private bool isAdding = true;
        private DateTime fechaInicio;
        // Evento que se dispara cuando el préstamo se registra exitosamente
        public event EventHandler PrestamoRegistradoCompletado;
        
        public PrestamoView(Cliente cliente, Articulo articulo)
        {
            InitializeComponent();
            this.cliente = cliente;
            this.articulo = articulo;

            txtClienteId.Text = cliente.GetId();
            txtArticuloId.Text = articulo.GetIdArticulo();
            txtClienteId.IsEnabled = false;
            txtArticuloId.IsEnabled = false;

            ConfigurarValidaciones();
        }

        public PrestamoView(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            txtArticuloId.Text = articulo.GetIdArticulo();
            txtArticuloId.IsEnabled = false;

            ConfigurarValidaciones();
        }

        public PrestamoView(Cliente cliente)
        {
            InitializeComponent();
            this.cliente = cliente;
            txtClienteId.Text = cliente.GetId();
            txtClienteId.IsEnabled = false;
            ConfigurarValidaciones();
        }

        //constructor para editar prestamo desde esta vista
        public PrestamoView(Prestamo prestamo)
        {
            InitializeComponent();

            isAdding = false;
            this.prestamo = prestamo;

            //desactivar campos
            txtArticuloId.IsEnabled = false;
            txtClienteId.IsEnabled = false;
            this.cliente = prestamo.GetCliente();
            this.articulo = prestamo.GetArticulo();
            txtClienteId.Text = cliente.GetId();
            txtArticuloId.Text = articulo.GetIdArticulo();
            txtTasaInteres.Text = prestamo.GetTasaInteres().ToString();
            txtClienteId.IsEnabled = false;
            txtArticuloId.IsEnabled = false;
            Loaded += MainWindow_Loaded;
            ConfigurarValidaciones();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FechaFinDatePicker.SelectedDate = prestamo.GetFechaFin();
        }

        public PrestamoView()
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

            this.fechaInicio = DateTime.Today;
            //si esta editando la fecha minima permitida es la creacion del prestamo
            if (!isAdding)
            {
                this.fechaInicio = prestamo.GetFechaInicio();
            }
            
            FechaFinDatePicker.DisplayDateStart = this.fechaInicio;
            //además mostrar en gris (no seleccionables) todas las fechas anteriores
            FechaFinDatePicker.BlackoutDates.Add(
                new CalendarDateRange(DateTime.MinValue, this.fechaInicio.AddDays(-1))
            );

            // Validaciones LostFocus
            txtClienteId.LostFocus += (s, e) => ValidacionHelper.ValidarIdentificador(txtClienteId, lblClienteId, "Identificacion del Cliente*");
            txtArticuloId.LostFocus += (s, e) => ValidacionHelper.ValidarIdentificador(txtArticuloId, lblArticuloId, "Identificador del Articulo*");
            txtTasaInteres.LostFocus += (s, e) => ValidacionHelper.ValidarPorcentaje(txtTasaInteres, lblTasaInteres, "Tasa de Interés*");
            FechaFinDatePicker.LostFocus += (s, e) =>  ValidacionHelper.ValidarFechaFin(FechaFinDatePicker, lblFechaFin, "Fecha de Finalizacion*", this.fechaInicio);
        }

        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }


        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;

            // Validar campos obligatorios y formatos
            valido &= ValidacionHelper.ValidarIdentificador(txtClienteId, lblClienteId, "Identificacion del Cliente*");
            valido &= ValidacionHelper.ValidarIdentificador(txtArticuloId, lblArticuloId, "Identificador Artículo*");
            valido &= ValidacionHelper.ValidarPorcentaje(txtTasaInteres, lblTasaInteres, "Tasa de Interés*");
            valido &= ValidacionHelper.ValidarFechaFin(FechaFinDatePicker, lblFechaFin, "Fecha de Finalizacion*", this.fechaInicio);


            if (!valido)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            // Obtener los valores de los campos
            string idCliente = txtClienteId.Text.Trim();
            string idArticulo = txtArticuloId.Text.Trim();
            double tasa = double.Parse(txtTasaInteres.Text.Trim());
            DateTime fecha = FechaFinDatePicker.SelectedDate.Value;

            //cargar cliente y articulo si no se han pasado por constructor
            var cliente = this.cliente;
            var articulo = this.articulo;
            if (cliente == null)
                cliente = new Cliente("", idCliente, "", "", "", "");
            if(articulo == null)
                articulo = new Articulo(idArticulo, "", 0.0, "");
            var prestamo = this.prestamo;
            if (isAdding) prestamo = new Prestamo(cliente, articulo, fecha, tasa);



            try
            {
                //actualizar el prestamo si se esta editando
                if (!isAdding)
                {
                    prestamo.SetTasaInteres(tasa);
                    prestamo.SetFechaFin(fecha);
                    bool actualizado = Sesion.Sesion.GetAdministradorActivo().ActualizarPrestamo(prestamo);
                    if (actualizado)
                    {
                        MostrarExito("Préstamo actualizado exitosamente.");
                       
                        return;
                    }
                    else
                    {
                        MostrarError("No se pudo actualizar el préstamo.");
                        return;
                    }
                }

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


                //validar que el cliente posea ese articulo
                //if (!Sesion.Sesion.GetAdministradorActivo().ClientePoseeArticulo(cliente, articulo))
                //{
                //    MostrarError("EL CLIENTE NO POSEE EL ARTICULO: \n El cliente no posee el articulo con este identificador");
                //    return;
                //}

                bool exito = Sesion.Sesion.GetAdministradorActivo().RegistrarPrestamo(prestamo);
                if (exito) 
                { 
                    MostrarExito("Préstamo registrado exitosamente.");
                    PrestamoRegistradoCompletado?.Invoke(this, EventArgs.Empty);
                }
                else
                    MostrarError("Error desconocido: No se pudo registrar el préstamo.");
            }
            catch (OracleException ex) when (ex.Number == 1017)
            {
                MostrarError("No se pudo conectar a la base de datos.\nVerifique su conexión o comuníquese con soporte técnico.");
                return;
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
