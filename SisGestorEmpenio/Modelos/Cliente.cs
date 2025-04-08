using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.Modelos
{
    internal class Cliente : Persona
    {
        // Declaración de variables privadas
        private string apellido;
        private string telefono;
        private string correo;
        private Administrador administrador;
        private List<Prestamo> prestamos = new List<Prestamo>();

        // Constructor
        public Cliente(string nombre, int id, string tipoIdentidad, string apellido, string telefono, string correo, Administrador administrador) : base(nombre, id, tipoIdentidad)
        {
            this.apellido = apellido;
            this.telefono = telefono;
            this.correo = correo;
            this.administrador = administrador;
            prestamos = new List<Prestamo>();

        }

        // Agregar un préstamo a la lista
        public void AgregarPrestamo(Prestamo prestamo)
        {
            prestamos.Add(prestamo);
        }

        // Mostrar información del cliente y sus préstamos
        public string MostrarDetalleCliente()
        {
            string info = $"Apellido: {apellido}\nTeléfono: {telefono}\nCorreo: {correo}\nPréstamos:\n";


            return info;
        }

            // Métodos para apellido
        public string GetApellido()
        {
            return apellido;
        }

        public void SetApellido(string nuevoApellido)
        {
            apellido = nuevoApellido;
        }

        // Métodos para telefono
        public string GetTelefono()
        {
            return telefono;
        }

        public void SetTelefono(string nuevoTelefono)
        {
            telefono = nuevoTelefono;
        }

        // Métodos para correo
        public string GetCorreo()
        {
            return correo;
        }

        public void SetCorreo(string nuevoCorreo)
        {
            correo = nuevoCorreo;
        }

        // Métodos para administrador
        public Administrador GetAdministrador()
        {
            return administrador;
        }

        public void SetAdministrador(Administrador nuevoAdministrador)
        {
            administrador = nuevoAdministrador;
        }

        // Métodos para prestamos
        public List<Prestamo> GetPrestamos()
        {
            return prestamos;
        }

        public void SetPrestamos(List<Prestamo> nuevosPrestamos)
        {
            prestamos = nuevosPrestamos;
}
    }
}
