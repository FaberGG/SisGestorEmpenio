using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    public class Persona
    {
        //atributos 
        protected string id;
        protected string tipoIdentidad;
        protected string nombre;


        //constructor 
         public Persona(string nombre, string id, string tipoIdentidad)
    {
            this.nombre = nombre;
            this.id = id;
            this.tipoIdentidad = tipoIdentidad;
         }


        // Métodos para 'id'
    public string GetId()
    {
        return id;
    }

    public void SetId(string nuevoId)
    {
        id = nuevoId;
    }

    // Métodos para 'nombre'
    public string GetNombre()
    {
        return nombre;
    }

    public void SetNombre(string nuevoNombre)
    {
        nombre = nuevoNombre;
    }

        public string GetTipoIdentidad()
    {
        return this.tipoIdentidad;
    }

    public void SetTipoIdentidad(string nuevoTipoIdentidad)
    {
        tipoIdentidad = nuevoTipoIdentidad;
    }
    }
}
