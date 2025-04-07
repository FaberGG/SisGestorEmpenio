using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    internal class Persona
    {
        //atributos 
        private int id { get; set; }
;
        private string nombre { get; set; }


        //constructor 
         public Persona(string nombre, int edad)
    {
        Nombre = nombre;
        Edad = edad;
    }

    }
}
