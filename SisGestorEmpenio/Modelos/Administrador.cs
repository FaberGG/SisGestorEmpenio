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
       

        public bool registrarCliente(string nombre,  int identificacion, string tipoIdentidad,  string apellido, string telefono, string correo)
        { 
            ClienteRepository clienteRepository = new ClienteRepository();
            Cliente cliente= new Cliente( nombre, identificacion, tipoIdentidad, apellido, telefono, correo, this);

            clienteRepository.guardar(cliente);
            return true;
        }

        public bool registrarArticulo(int idArticulo, string descripcion, double valorEstimado, string Estado)
        {
            ArticuloRepository articuloRepository=new ArticuloRepository();

            Articulo articulo = new Articulo(idArticulo,descripcion,valorEstimado,Estado);
            articuloRepository.guardar(articulo);
            
            return true;
        }

        public bool registrarPrestamo(Cliente cliente, Articulo articulo, double tasaInteres)
        {
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            DateTime fechaInicio = DateTime.Now;
            Prestamo prestamo = new Prestamo(cliente, articulo, fechaInicio, tasaInteres);

            prestamoRepository.guardar(prestamo);
            return true;
        }

        public bool registrarDevolución(Prestamo prestamo, double montopagado)
        {
            DevolucionRepository devolucionRepository=new DevolucionRepository();
            DateTime fechaDevolucion = DateTime.Now;
            Devolucion devolucion = new Devolucion(fechaDevolucion, montopagado, prestamo);
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


