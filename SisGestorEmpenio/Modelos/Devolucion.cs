using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    public class Devolucion
    {

        //atributos
        private int numeroConvenio;
        private DateTime fechaDevolucion;
        private double montoPagado;
        private Prestamo prestamo;
        private Cliente cliente;
        private Articulo articulo;
        private Cliente clienteNombre;
        private Articulo articuloNombre;


        //constructor

        public Devolucion(double montoPagado, Prestamo prestamo)
        {
            this.montoPagado = montoPagado;
            this.prestamo = prestamo;
            this.fechaDevolucion = DateTime.Now;
        }

        public Devolucion(int numeroConvenio, DateTime fechaDevolucion, double montoPagado, Prestamo prestamo)
        {
            this.numeroConvenio = numeroConvenio;
            this.fechaDevolucion = fechaDevolucion;
            this.montoPagado = montoPagado;
            this.prestamo = prestamo;
        }

        //get y set de la clase 

        public int GetNumeroConvenio()
        {
            return numeroConvenio;
        }

        public void SetNumeroConvenio(int value)
        {
            numeroConvenio = value;
        }

        public DateTime GetFechaDevolucion()
        {
            return fechaDevolucion;
        }

        public void SetFechaDevolucion(DateTime value)
        {
            fechaDevolucion = value;
        }

        public double GetMontoPagado()
        {
            return montoPagado;
        }

        public void SetMontoPagado(double value)
        {
            montoPagado = value;
        }

        public Prestamo GetPrestamo()
        {
            return prestamo;
        }

        public void SetPrestamo(Prestamo value)
        {
            prestamo = value;
        }

        public Cliente GetCliente() => cliente;
        public void SetCliente(Cliente value) => cliente = value;
        public Articulo GetArticulo() => articulo;
        public void SetArticulo(Articulo value) => articulo = value;



    }
}
