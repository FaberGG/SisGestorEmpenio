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
    public partial class ArticuloView : UserControl
    {
        // Valores válidos para estadoArticulo en la BD
        private readonly string[] estadosValidos = { "defectuoso", "optimo", "funcionable" };

        public event EventHandler<Articulo> RegistroArticuloCompletado;
        private Articulo articulo;
        private bool isAdding = true;

        public ArticuloView()
        {
            InitializeComponent();
            configurarValidaciones();
        }

        //constructor para editar un articulo existente
        public ArticuloView(Articulo articulo) : this()
        {
            isAdding = false;
            this.articulo = articulo;
            // Cargar datos del artículo en los campos
            txtID.Text = articulo.GetIdArticulo().ToString();
            txtID.IsEnabled = false; // Deshabilitar el campo de ID
            txtDescripcion.Text = articulo.GetDescripcion();
            txtValor.Text = articulo.GetValorEstimado().ToString();
            foreach (ComboBoxItem item in cbEstado.Items)
            {
                if (string.Equals(item.Content.ToString(), articulo.GetEstadoArticulo(), StringComparison.OrdinalIgnoreCase))
                {
                    cbEstado.SelectedItem = item;
                    break;
                }
            }
        }

        //cargar validaciones
        private void configurarValidaciones()
        {
            // Máximas longitudes según tabla
            txtID.MaxLength = 10;   // INT
            txtDescripcion.MaxLength = 100;  // VARCHAR2(100)
            txtValor.MaxLength = 18;   // DECIMAL(18,2) -> suficiente para "123456789012345.67"

            // Prevención de caracteres inválidos
            txtID.PreviewTextInput += SoloNumeros_Preview;

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

            // Crear
            var art = new Articulo(
                int.Parse(txtID.Text.Trim()),
                txtDescripcion.Text.Trim(),
                double.Parse(txtValor.Text.Trim()),
                cbEstado.Text.Trim().ToLower()
            );

            try
            {
                // Si no es un nuevo artículo, actualizar el artículo existente
                if (!isAdding)
                {
                    // Actualizar los valores del artículo original
                    this.articulo.SetDescripcion(txtDescripcion.Text.Trim());
                    this.articulo.SetValorEstimado(double.Parse(txtValor.Text.Trim()));
                    this.articulo.SetEstadoArticulo(cbEstado.Text.Trim().ToLower());

                    bool actualizado = Sesion.Sesion.GetAdministradorActivo().ActualizarArticulo(this.articulo);

                    if (actualizado)
                    {
                        MostrarMensaje("Artículo actualizado exitosamente.", "Éxito");
                        RegistroArticuloCompletado?.Invoke(this, art);
                    }
                    else
                    {
                        MostrarMensaje("No se pudo actualizar el artículo.", "Error");
                    }
                    return;
                }

                //validar que el articulo no exista
                if (Sesion.Sesion.GetAdministradorActivo().ExisteArticulo(art))
                {
                    MostrarMensaje("EL ARTICULO YA EXISTE: \n Un articulo con este ID ya esta registrado", "Error");
                    return;
                }
                bool completado = Sesion.Sesion.GetAdministradorActivo().RegistrarArticulo(art);
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
            catch (OracleException ex) when (ex.Number == 1017)
            {
                MostrarMensaje("No se pudo conectar a la base de datos.\nVerifique su conexión o comuníquese con soporte técnico.", "Error");
                return;
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
        public int? ArticuloId
        {
            get
            {
                if (int.TryParse(txtID.Text, out var id))
                    return id;
                return null;
            }
        }
    }
}