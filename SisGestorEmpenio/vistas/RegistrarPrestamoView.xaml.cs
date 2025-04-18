﻿using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para RegistrarPrestamo.xaml
    /// </summary>
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
        }
        public RegistrarPrestamo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            this.cliente = null;
            txtArticuloId.Text = articulo.GetIdArticulo().ToString();
            txtArticuloId.IsEnabled = false;
        }

        public RegistrarPrestamo()
        {
            InitializeComponent();
            this.cliente = null;
            this.articulo = null;

        }

        private void txtSoloNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsNumero(e.Text);
        }

        private bool EsNumero(string texto)
        {
            return int.TryParse(texto, out _);
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Capturar valores del formulario
            
            string clienteId = txtClienteId.Text.Trim();
            string articuloId = txtArticuloId.Text.Trim();
            DateTime? fechaFinNullable = FechaFinDatePicker.SelectedDate;
            DateTime fechaFin = fechaFinNullable.Value;
            string tasaInteresStr = txtTasaInteres.Text.Trim();
            double tasaInteres;

            

            // Validación básica de campos
            if (string.IsNullOrWhiteSpace(clienteId) ||
                string.IsNullOrWhiteSpace(articuloId) ||
                string.IsNullOrWhiteSpace(tasaInteresStr) ||
                fechaFinNullable.HasValue == false
                )
                
            {
                MostrarError("Todos los campos son obligatorios.");
                return;
            }
            // Convertir ID a entero
            if (!int.TryParse(clienteId, out int idCliente))
            {
                MostrarError("El campo Identificacion del cliente debe ser un número válido.");
                return;
            }
            if (!int.TryParse(articuloId, out int idArticulo))
            {
                MostrarError("El campo identificador del articulo debe ser un número válido.");
                return;
            }
            //convertir a double
            if (!double.TryParse(tasaInteresStr, out tasaInteres))
            {
                MostrarError("El campo tasa de interes debe ser un número decimal valido.");
                return;
            }
            





            // Mostrar datos capturados (prueba)
            /*
            MessageBox.Show(
                $"Nombre: {nombre}\nApellido: {apellido}\nCorreo: {correo}\nID: {id}\nTeléfono: {telefono}\nTipo de Identidad: {tipoIdentidad}",
                "Datos capturados", MessageBoxButton.OK, MessageBoxImage.Information);
            */


            // Crear el cliente y el artículo
            if (cliente == null)
            {
                // Si el cliente es nulo, se crea un nuevo objeto cliente temp
                cliente = new Cliente("", idCliente, "", "", "", "");
            }

            //PASAR LOS DATOS A ADMINISTRADOR PARA EJECUTAR LA CONSULTA

            var prestamo = new Prestamo(cliente, articulo, fechaFin, tasaInteres);
            
            try
            {
                bool completado = Sesion.Sesion.GetAdministradorActivo().registrarPrestamo(prestamo);
                // Mostrar mensaje de éxito
                if (completado)
                {
                    MostrarExito("Prestamo registrado exitosamente.");
                }
                else
                {
                    MostrarError("No se pudo registrar el prestamo.");
                }
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
