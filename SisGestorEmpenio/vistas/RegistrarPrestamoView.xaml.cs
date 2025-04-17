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
            this.articulo = articulo;
            this.cliente = null;
            txtArticuloId.Text = articulo.GetIdArticulo().ToString();
            txtArticuloId.IsEnabled = false;
            InitializeComponent();
        }

        public RegistrarPrestamo()
        {
            this.cliente = null;
            this.articulo = null;

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
                string.IsNullOrWhiteSpace(tasaInteres) ||
                fechaFinNullable.HasValue == false
                )
                
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Convertir ID a entero
            if (!int.TryParse(clienteId, out int idCliente))
            {
                MessageBox.Show("El campo Identificacion del cliente debe ser un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(articuloId, out int idArticulo))
            {
                MessageBox.Show("El campo identificador del articulo debe ser un número válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //convertir a double
            if (!double.TryParse(tasaInteresStr, out tasaInteres))
            {
                MessageBox.Show("El campo tasa de interes debe ser un número decimal valido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                Sesion.Sesion.GetAdministradorActivo().registrarPrestamo(prestamo);
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
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
