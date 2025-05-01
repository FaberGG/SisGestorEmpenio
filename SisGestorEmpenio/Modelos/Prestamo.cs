using SisGestorEmpenio.repository;
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

        public Prestamo(Cliente cliente, Articulo articulo, DateTime fechaFin, double tasaInteres)
        {
            this.cliente = cliente;
            this.articulo = articulo;
            this.estado = "activo";
            this.fechaInicio = DateTime.Now;
            this.fechaFin = fechaFin;
            this.tasaInteres = tasaInteres;
        }

        public Prestamo(Cliente cliente, Articulo articulo, string estado, DateTime fechaInicio, DateTime fechaFin, double tasaInteres, double montoTotal)
        {
            this.cliente = cliente;
            this.articulo = articulo;
            this.estado = estado;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.tasaInteres = tasaInteres;
            this.montoTotal = montoTotal;
        }
        public double CalcularInteres()
        {
            double valorArticulo = articulo.GetValorEstimado();
            DateTime fechaActual = DateTime.Now;
            //calcular la diferencia en meses entre la fecha de inicio y la fecha actual
            int cantidadMeses = ((fechaActual.Year - fechaInicio.Year) * 12) + fechaActual.Month - fechaInicio.Month;
            if (fechaActual.Day < fechaInicio.Day)
            {
                //resto 1 mes si el día actual es menor que el día de inicio
                cantidadMeses--;
            }
            if (cantidadMeses == 0)
            {
                cantidadMeses = 1; // Si no ha pasado un mes, se cobra intereses de un mes
            }
            return valorArticulo * (tasaInteres/100) * cantidadMeses;
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
                PrestamoRepository prestamoRepository = new PrestamoRepository();
                estado = nuevoEstado;
                return prestamoRepository.ActualizarEstado(this);
            }
            return false;
        }

        

        public DateTime GetFechaFin()
        {
           return fechaFin;

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
