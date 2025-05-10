using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SisGestorEmpenio.vistas
{
    public partial class DevolucionView : UserControl
    {
        private readonly bool _isEditMode;
        private readonly Devolucion _original;

        /// <summary>
        /// Constructor para modo “crear”.
        /// </summary>
        public DevolucionView() : this(null) { }

        /// <summary>
        /// Si pasas una devolución existente, entra en modo “editar”.
        /// </summary>
        public DevolucionView(Devolucion devolucion)
        {
            InitializeComponent();

            _original = devolucion;
            _isEditMode = devolucion != null;

            // Ajustes de UI según modo
            lblAction.Text = _isEditMode ? "MODIFICAR" : "GUARDAR";
            txtIdCliente.IsEnabled = !_isEditMode;
            txtIdArticulo.IsEnabled = !_isEditMode;

            // Validaciones automáticas
            txtIdCliente.LostFocus += (s, e) =>
                ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "identificación del cliente");
            txtIdArticulo.LostFocus += (s, e) =>
                ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "identificador del artículo");
            txtMontoTotal.LostFocus += (s, e) =>
                ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "monto total");

            // Sólo números al tipear
            txtIdCliente.PreviewTextInput += SoloNumeros_Preview;
            txtIdArticulo.PreviewTextInput += SoloNumeros_Preview;

            if (_isEditMode)
            {
                // Poblar datos existentes
                var prestamo = _original.GetPrestamo();
                txtIdCliente.Text = prestamo.GetCliente().GetId().ToString();
                txtIdArticulo.Text = prestamo.GetArticulo().GetIdArticulo().ToString();
                txtMontoTotal.Text = _original.GetMontoPagado().ToString("F2");
            }
        }

        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void Action_Click(object sender, MouseButtonEventArgs e)
        {
            // Validar campos
            bool ok =
                ValidacionHelper.ValidarEntero(txtIdCliente, lblIdCliente, "identificación del cliente") &
                ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "identificador del artículo") &
                ValidacionHelper.ValidarDecimal(txtMontoTotal, lblMontoTotal, "monto total");

            if (!ok)
            {
                MostrarError("Corrige los campos resaltados.");
                return;
            }

            int idCliente = int.Parse(txtIdCliente.Text.Trim());
            int idArticulo = int.Parse(txtIdArticulo.Text.Trim());
            double monto = double.Parse(txtMontoTotal.Text.Trim());

            var admin = Sesion.Sesion.GetAdministradorActivo();

            try
            {
                if (_isEditMode)
                {
                    // **Editar**
                    _original.SetMontoPagado(monto);
                    bool exito = admin.ActualizarDevolucion(_original);
                    if (exito) MostrarExito("Devolución modificada exitosamente.");
                    else MostrarError("No se pudo modificar la devolución.");
                }
                else
                {
                    // **Registrar**
                    var prestamo = admin.BuscarPrestamo(idCliente, idArticulo);
                    if (prestamo == null)
                    {
                        MostrarError("No existe un préstamo con ese cliente y artículo.");
                        return;
                    }

                    var devolucion = new Devolucion(monto, prestamo);
                    if (admin.ExisteDevolucion(devolucion))
                    {
                        MostrarError("Ya existe una devolución para este préstamo.");
                        return;
                    }

                    var conf = new ConfirmacionWindow
                    {
                        Mensaje = "¿Desea continuar con el proceso?",
                        Titulo = "Registrar Devolución?",
                        TextoBotonIzquierdo = "Cancelar",
                        TextoBotonDerecho = "Continuar",
                        MostrarBotonDerecho = true
                    };
                    if (conf.ShowDialog() == true && conf.Confirmado)
                    {
                        bool exito = admin.RegistrarDevolución(devolucion);
                        if (exito) MostrarExito("Devolución registrada exitosamente.");
                        else MostrarError("No se pudo registrar la devolución.");
                    }
                }
            }
            catch (OracleException ex) when (ex.Number == 1017)
            {
                MostrarError("No se pudo conectar a la base de datos.\nVerifique su conexión o contacte a soporte.");
            }
            catch (OracleException ex)
            {
                MostrarError($"Error en base de datos:\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error inesperado:\n" + ex.Message);
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
