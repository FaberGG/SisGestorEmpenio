using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    internal class Devolucion
    {

        //atributos
        private int numeroConvenio;
        private DateTime fechaDevolucion;
        private double montoPagado;
        Cliente cliente;
        Articulo articulo;

        //constructor
        public Devolucion(int numeroConvenio, DateTime fechaDevolucion, double montoPagado)
        {
            this.cliente = cliente;
            this.articulo = articulo;
            this.numeroConvenio = numeroConvenio;
            this.fechaDevolucion = fechaDevolucion;
            this.montoPagado = montoPagado;
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

    }
}
