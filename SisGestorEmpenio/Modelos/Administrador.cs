using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.Modelos
{
    internal class Administrador : Persona
    {
        //Atributos 
        private
            int salario { get; set; };
            int aniosExp { get; set; };
            string contrasenia { get; set; };
            string usuario { get; set; };

        //Constructor
        public Administrador(string nombre, int edad, int salario, int aniosExp, string contrasenia, string usurario) : base(nombre, edad)
        {
            this.salario = salario;
            this.aniosExp = aniosExp;
            this.contrasenia = contrasenia;
            this.usurario = usurario;
        }

        public bool registrarCliente(int identificacion, string nombre, string apellido, string telefono, string correo, string tipoIdentificacion)
        {
            //Lógica para registrar cliente
            return true;
        }

        public bool registrarArticulo(int idArticulo, string descripcion, double valorEstimado, string Estado)
        {
            //Lógica para registrar artículo
            return true;
        }

        public bool registrarPrestamo(Cliente Cliente, Articulo articulo, double monto, float tasaInteres)
        {
            //Lógica para registrar préstamo
            return true;
        }

        

    }
}


