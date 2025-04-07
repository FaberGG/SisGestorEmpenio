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
        private int numeroConvenio { get; set; }
        private DateTime fechaDevolucion { get; set; }
        private double montoPagado { get; set; }


        //constructor
        public Devolucion(int numeroConvenio, DateTime fechaDevolucion, double montoPagado)
        {
            this.numeroConvenio = numeroConvenio;
            this.fechaDevolucion = fechaDevolucion;
            this.montoPagado = montoPagado;
        }
    }
}
