using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    public class Prestamo
    {
        private DateTime fechaInicio;
        private DateTime fechaFin;
        private double tasaInteres;
        private double montoTotal;
        private string estado;
        private Cliente cliente;
        private Articulo articulo;
        private Devolucion devolucion;

        public Prestamo(Cliente cliente, Articulo articulo, DateTime fechaInicio, double tasaInteres)
        {
            this.cliente = cliente;
            this.articulo = articulo;
            this.estado = "activo";
            this.fechaInicio = fechaInicio;
            this.tasaInteres = tasaInteres;
        }

        public double CalcularInteres()
        {   
            return tasaInteres * 0.10;
        }

        public double CalcularMontoTotal()
        {
            double interes = CalcularInteres();
            double valorArticulo = articulo.GetValorEstimado();
            montoTotal = valorArticulo + interes;
            return montoTotal; // Devuelve el cálculo total
        }

        public bool ActualizarEstadoPrestamo(string nuevoEstado)
        {
            if (!string.IsNullOrWhiteSpace(nuevoEstado))
            {
                estado = nuevoEstado;
                return true;
            }
            return false;
        }

        public void MarcarComoDevuelto()
        {
            //estado = "Devuelto";
            //devolucion = new Devolucion(DateTime.Now); // Ejemplo: devuelve hoy
        }

        public DateTime CalcularFechaVencimiento()
        {
           return fechaFin = fechaInicio.AddDays(30); // 30 días plazo por defecto

        }

        public string MostrarDetalle()
        {
            //return $"Cliente: {cliente.Apellido}\nArtículo: {articulo.Descripcion}\nMonto Total: {montoTotal:F2}\nEstado: {estado}\nFecha Vencimiento: {fechaFin.ToShortDateString()}";
            return "";
        }


        //Get y set del préstamo 


        public DateTime GetFechaInicio()
        {
            return fechaInicio;
        }

        public void SetFechaInicio(DateTime value)
        {
            fechaInicio = value;
        }


        public double GetTasaInteres()
        {
            return tasaInteres;
        }

        public double GetMontoTotal()
        {
            return montoTotal;
        }

        public void SetMontoTotal(double value)
        {
            montoTotal = value;
        }
        public void SetTasaInteres(double value)
        {
            tasaInteres = value;
        }

     
        public string GetEstado()
        {
            return estado;
        }

        public void SetEstado(string value)
        {
            estado = value;
        }

        public Cliente GetCliente()
        {
            return cliente;
        }

        public void SetCliente(Cliente value)
        {
            cliente = value;
        }

        public Articulo GetArticulo()
        {
            return articulo;
        }

        public void SetArticulo(Articulo value)
        {
            articulo = value;
        }

        public Devolucion GetDevolucion()
        {
            return devolucion;
        }

        public void SetDevolucion(Devolucion value)
        {
            devolucion = value;
        }
    }
}
