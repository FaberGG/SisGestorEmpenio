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

        public bool Guardar(Cliente cliente)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Cliente VALUES ({cliente.GetId()}, '{cliente.GetNombre()}', '{cliente.GetApellido()}', '{cliente.GetTelefono()}', '{cliente.GetCorreo()}', '{cliente.GetTipoIdentidad()}', {cliente.GetAdministrador().GetId()})";
            
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
        public bool EstaGuardado(int id)
        {
            string consulta = $"SELECT * FROM cliente WHERE numeroIdentidad = {id}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }
    }
}
