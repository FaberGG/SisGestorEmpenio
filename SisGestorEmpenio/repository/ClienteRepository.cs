using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repository;

namespace SisGestorEmpenio.repository
{
    internal class ClienteRepository
    {
        ConexionDB dt = new ConexionDB();

        public bool guardar(Cliente cliente)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Cliente VALUES ({cliente.GetId()}, {cliente.GetNombre()}', '{cliente.GetApellido()}', '{cliente.GetTelefono}', '{cliente.GetCorreo()}', '{cliente.GetTipoIdentidad()}', '{cliente.GetAdministrador().GetTipoIdentidad()}')";
            
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }

    }
}
