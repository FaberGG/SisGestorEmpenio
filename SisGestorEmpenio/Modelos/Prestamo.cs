using System;

namespace SisGestorEmpenio.Modelos
{
    internal class Prestamo
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double TasaInteres { get; set; }
        public double MontoTotal { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }
        public Articulo Articulo { get; set; }
        public Devolucion Devolucion { get; set; }

        public Prestamo(Cliente cliente, Articulo articulo, double tasaInteres, DateTime fechaInicio)
        {
            Cliente = cliente;
            Articulo = articulo;
            TasaInteres = tasaInteres;
            FechaInicio = fechaInicio;
            Estado = "Activo";
            CalcularFechaVencimiento();
            CalcularMontoTotal();
        }

        public double CalcularInteres()
        {
            // Ejemplo simple: interés = valor estimado * tasa
            return Articulo.ValorEstimado * TasaInteres;
        }

        public void CalcularMontoTotal()
        {
            MontoTotal = Articulo.ValorEstimado + CalcularInteres();
        }

        public bool ActualizarEstadoPrestamo(string nuevoEstado)
        {
            if (!string.IsNullOrWhiteSpace(nuevoEstado))
            {
                Estado = nuevoEstado;
                return true;
            }
            return false;
        }

        public void MarcarComoDevuelto()
        {
            Estado = "Devuelto";
            Devolucion = new Devolucion(DateTime.Now); // Ejemplo: devuelve hoy
        }

        public void CalcularFechaVencimiento()
        {
            FechaFin = FechaInicio.AddDays(30); // 30 días plazo por defecto
        }

        public string MostrarDetalle()
        {
            return $"Cliente: {Cliente.Apellido}\nArtículo: {Articulo.Descripcion}\nMonto Total: {MontoTotal:F2}\nEstado: {Estado}\nFecha Vencimiento: {FechaFin.ToShortDateString()}";
        }
    }
}
