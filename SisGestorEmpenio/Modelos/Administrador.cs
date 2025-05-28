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

        //METODOS DE REGISTRO
        public bool RegistrarCliente(Cliente cliente)
        { 
            ClienteRepository clienteRepository = new ClienteRepository();
            cliente.SetAdministrador(this); // Asignar el administrador al cliente
            return clienteRepository.Guardar(cliente);
        }

        public bool RegistrarArticulo(Articulo articulo)
        {
            ArticuloRepository articuloRepository=new ArticuloRepository();
            articulo.SetAdministrador(this); // Asignar el administrador al artículo
            return articuloRepository.Guardar(articulo);
        }

        public bool RegistrarPrestamo(Prestamo prestamo)
        {
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            return prestamoRepository.Guardar(prestamo);
        }

        public bool RegistrarDevolución(Devolucion devolucion)
        {
            DevolucionRepository devolucionRepository=new DevolucionRepository();
            bool guardarDevExitoso = devolucionRepository.Guardar(devolucion);
            if (guardarDevExitoso)
            {
                devolucion.GetPrestamo().ActualizarEstadoPrestamo("Inactivo");
                devolucion.GetPrestamo().GetArticulo().marcarComoDevuelto();
            }
            return guardarDevExitoso;
        }

        //METODOS DE BUSQUEDA

        public Cliente BuscarCliente(int id)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            return clienteRepository.Buscar(id);
        }

        public Articulo BuscarArticulo(int id)
        {
            ArticuloRepository articuloRepository = new ArticuloRepository();
            return articuloRepository.Buscar(id);
        }

        //buscar articulos coincidentes 
        public List<Articulo> BuscarArticulosCoincidentes(int cantidadMaxArticulos = 100, int id = -1, string descripcion = "", int propiedadCasa = -1, string estado = "", string devolucion = "")
        {
            ArticuloRepository articuloRepository = new ArticuloRepository();
            return articuloRepository.BuscarArticulosCoincidentes(cantidadMaxArticulos, id, descripcion, propiedadCasa, estado, devolucion );
        }

        public Prestamo BuscarPrestamo(int clienteId, int articuloId)
        {
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            return prestamoRepository.Buscar(clienteId, articuloId);
        }

        public Devolucion BuscarDevolucion(int clienteId, int articuloId)
        {
            DevolucionRepository devolucionRepository = new DevolucionRepository();
            return devolucionRepository.Buscar(clienteId, articuloId);
        }

        //METODOS DE ACTUALIZACION
        public bool ActualizarCliente(Cliente cliente)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            return clienteRepository.Actualizar(cliente);
        }

        public bool ActualizarArticulo(Articulo articulo)
        {
            ArticuloRepository articuloRepository = new ArticuloRepository();
            return articuloRepository.Actualizar(articulo);
        }

        public bool ActualizarPrestamo(Prestamo prestamo)
        {
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            return prestamoRepository.Actualizar(prestamo);
        }

        public bool ActualizarDevolucion(Devolucion devolucion)
        {
            DevolucionRepository devolucionRepository = new DevolucionRepository();
            return devolucionRepository.Actualizar(devolucion);
        }


        //METODOS DE VALIDACION
        public bool ExisteCliente(Cliente cliente)
        {
            int id = cliente.GetId();
            ClienteRepository clienteRepository = new ClienteRepository();
            return clienteRepository.EstaGuardado(id);
        }

        public bool ExisteArticulo(Articulo articulo)
        {
            int id = articulo.GetIdArticulo();
            ArticuloRepository articuloRepository = new ArticuloRepository();
            return articuloRepository.EstaGuardado(id);
        }

        public bool ExistePrestamo(Prestamo prestamo)
        {
            int clienteId = prestamo.GetCliente().GetId();
            int articuloId = prestamo.GetArticulo().GetIdArticulo();
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            return prestamoRepository.EstaGuardado(clienteId, articuloId);
        }

        public bool ExisteDevolucion(Devolucion devolucion)
        {
            int clienteId = devolucion.GetPrestamo().GetCliente().GetId();
            int articuloId = devolucion.GetPrestamo().GetArticulo().GetIdArticulo();
            DevolucionRepository devolucionRepository = new DevolucionRepository();
            return devolucionRepository.EstaGuardado(clienteId, articuloId);
        }

        public bool ClientePoseeArticulo(Cliente cliente, Articulo articulo)
        {
            int clienteId = cliente.GetId();
            int articuloId = articulo.GetIdArticulo();
            ClienteRepository clienteRepository = new ClienteRepository();
            return clienteRepository.PoseeArticulo(clienteId, articuloId);
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


