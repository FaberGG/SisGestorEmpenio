using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SisGestorEmpenio.Modelos
{
    internal class Cliente
    {
        public string apellido { get; set; }
        public int telefono { get; set; }
        public string correo { get; set; }

        public List<Prestamo> prestamos { get; set; }

        // Constructor
        public Cliente(string apellido, int telefono, string correo)
        {
            this.apellido = apellido;
            this.telefono = telefono;
            this.correo = correo;
            this.prestamos = new List<Prestamo>();
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
    }
}
