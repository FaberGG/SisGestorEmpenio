using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.repository;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.Modelos
{
    internal class Administrador : Persona
    {
        //Atributos 
        private
            int salario { get; set; }
            int aniosExp { get; set; }
            string contrasenia { get; set; }
            string usuario { get; set; }

        //Constructor
        public Administrador(string nombre, int id, string tipoIdentidad, int salario, int aniosExp, string contrasenia, string usuario) : base(nombre, id, tipoIdentidad)
        {
            this.salario = salario;
            this.aniosExp = aniosExp;
            this.contrasenia = contrasenia;
            this.usuario = usuario;
        }

        public bool registrarCliente(string nombre,  int identificacion, string tipoIdentidad,  string apellido, string telefono, string correo)
        { 
            ClienteRepository clienteRepository = new ClienteRepository();
            Cliente cliente= new Cliente( nombre, identificacion, tipoIdentidad, apellido, telefono, correo, this);
            
            
            clienteRepository.guardar(cliente);
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

        
        public bool registrarDevolucion(Cliente cliente, Articulo articulo, double monto)
        {
            //Lógica para registrar devolución
            return true;
        }
    }
}


