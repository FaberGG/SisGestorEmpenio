using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.repository;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.Modelos
{
    public class Administrador : Persona
    {
        //Atributos 
        private double salario;
        private int aniosExp;
        private string usuario;
        private string contrasenia;


        //Constructor
        public Administrador(string nombre, int id, string tipoIdentidad, double salario, int aniosExp, string usuario, string contrasenia) : base(nombre, id, tipoIdentidad)
        {
            this.salario = salario;
            this.aniosExp = aniosExp;
            this.contrasenia = contrasenia;
            this.usuario = usuario;
        }
       

        public bool registrarCliente(Cliente cliente)
        { 
            ClienteRepository clienteRepository = new ClienteRepository();
            cliente.SetAdministrador(this); // Asignar el administrador al cliente
            clienteRepository.guardar(cliente);
            return true;
        }

        public bool registrarArticulo(Articulo articulo)
        {
            ArticuloRepository articuloRepository=new ArticuloRepository();
            articulo.SetAdministrador(this); // Asignar el administrador al artículo
            articuloRepository.guardar(articulo);
            
            return true;
        }

        public bool registrarPrestamo(Prestamo prestamo)
        {
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            prestamoRepository.guardar(prestamo);
            return true;
        }

        public bool registrarDevolución(Devolucion devolucion)
        {
            DevolucionRepository devolucionRepository=new DevolucionRepository();
            devolucionRepository.guardar(devolucion); 
            return true;
        }



        //SETTERS Y GETTERS

        public double GetSalario()
        {
            return salario;
        }
        public void SetSalario(double nuevoSalario)
        {
            salario = nuevoSalario;
        }

        public int GetAniosExp()
        {
            return aniosExp;
        }

        public void SetAniosExp(int nuevoAniosExp)
        {
            aniosExp = nuevoAniosExp;
        }

        public string GetUsuario()
        {
            return usuario;
        }
        public void SetUsuario(string nuevoUsuario)
        {
            usuario = nuevoUsuario;
        }

        public string GetContrasenia()
        {
            return contrasenia;
        }
        public void SetContrasenia(string nuevaContrasenia)
        {
            contrasenia = nuevaContrasenia;
        }
    }
}


