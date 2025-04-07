using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SisGestorEmpenio.Modelos
{
    internal class Prestamo
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public double tasaInteres { get; set; }
        public double montoTotal { get; set; }
        public string estado { get; set; }

        public Cliente cliente { get; set; }
        public Articulo articulo { get; set; }
        public Devolucion devolucion { get; set; }

        public Prestamo(Cliente cliente, Articulo articulo, double tasaInteres, DateTime fechaInicio)
        {
            this.cliente = cliente;
            this.articulo = articulo;
            this.tasaInteres = tasaInteres;
            this.fechaInicio = fechaInicio;
            estado = "Activo";
            CalcularFechaVencimiento();
            CalcularMontoTotal();
        }

        public double CalcularInteres()
        {
            // Ejemplo simple: interés = valor estimado * tasa
            //return articulo.valorEstimado * tasaInteres;
            return 0;
        }

        public void CalcularMontoTotal()
        {
            //montoTotal = articulo.ValorEstimado + CalcularInteres();
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

        public void CalcularFechaVencimiento()
        {
            fechaFin = fechaInicio.AddDays(30); // 30 días plazo por defecto
        }

        public string MostrarDetalle()
        {
            //return $"Cliente: {cliente.Apellido}\nArtículo: {articulo.Descripcion}\nMonto Total: {montoTotal:F2}\nEstado: {estado}\nFecha Vencimiento: {fechaFin.ToShortDateString()}";
            return "";
        }
    }
}
