using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repositor;

namespace SisGestorEmpenio.repository
{
    internal class ClienteRepository
    {
        ConexionDB conexion = new ConexionDB();

        public bool guardar(Cliente cliente)
        {
            string consulta = $"INSERT INTO Cliente VALUES ({cliente.GetId()}, {cliente.GetNombre()}', '{cliente.GetApellido()}', '{cliente.GetTelefono}', '{cliente.GetCorreo()}', '{cliente.GetTipoIdentidad()}', '{cliente.Administrador.TipoIdentidad}')";
            return dt.ejecutarDML(consulta);

        }

    }
}
