using SisGestorEmpenio.repository;
using System;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repository;

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

        // Métodos originales...
 
        public double CalcularMontoTotal() { /* ... */ throw new NotImplementedException(); }
        public bool ActualizarEstadoPrestamo(string nuevoEstado) { /* ... */ throw new NotImplementedException(); }
        public DateTime GetFechaFin() => fechaFin;
        public string MostrarDetalle() { /* ... */ throw new NotImplementedException(); }
        public DateTime GetFechaInicio() => fechaInicio;
        public void SetFechaInicio(DateTime value) => fechaInicio = value;
        public double GetTasaInteres() => tasaInteres;
        public double GetMontoTotal() => montoTotal;
        public void SetMontoTotal(double value) => montoTotal = value;
        public void SetTasaInteres(double value) => tasaInteres = value;
        public string GetEstado() => estado;
        public void SetEstado(string value) => estado = value;
        public Cliente GetCliente() => cliente;
        public void SetCliente(Cliente value) => cliente = value;
        public Articulo GetArticulo() => articulo;
        public void SetArticulo(Articulo value) => articulo = value;
        public Devolucion GetDevolucion() => devolucion;
        public void SetDevolucion(Devolucion value) => devolucion = value;

        // === PROPIEDADES PÚBLICAS PARA BINDING EN WPF ===

        /// <summary>
        /// Identificador que usa el DataGrid para acciones (aquí usamos IdArticulo)
        /// </summary>
        public int IdPrestamo => ArticuloId;

        /// <summary>
        /// ID del cliente
        /// </summary>
        public int ClienteId => cliente.GetId();

        /// <summary>
        /// Nombre completo del cliente
        /// </summary>
        public string ClienteNombre => $"{cliente.GetNombre()} {cliente.GetApellido()}";

        /// <summary>
        /// ID del artículo prestado
        /// </summary>
        public int ArticuloId => articulo.GetIdArticulo();

        /// <summary>
        /// Descripción del artículo
        /// </summary>
        public string ArticuloNombre => articulo.GetDescripcion();

        /// <summary>
        /// Fecha de inicio del préstamo (formateada)
        /// </summary>
        public string FechaInicio => fechaInicio.ToString("dd/MM/yyyy");

        /// <summary>
        /// Fecha de vencimiento del préstamo (formateada)
        /// </summary>
        public string FechaFin => fechaFin.ToString("dd/MM/yyyy");

        /// <summary>
        /// Estado del préstamo (Activo/Inactivo)
        /// </summary>
        public string Estado => estado;

        /// <summary>
        /// Monto total calculado (puedes enlazar si lo requieres)
        /// </summary>
        public double MontoTotal => montoTotal;
    }
}
