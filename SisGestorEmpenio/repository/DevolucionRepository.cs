using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;
namespace SisGestorEmpenio.repository
{
    internal class DevolucionRepository
    {


        ConexionDB dt = new ConexionDB();

        public bool guardar(Devolucion devolucion)
        {
            int filasAfectadasA = 0;
            string consulta = $"INSERT INTO Devolucion VALUES ({devolucion.GetCliente().GetTipoIdentidad()}, {devolucion.GetArticulo().GetIdArticulo()}, {devolucion.GetNumeroConvenio()}, {devolucion.GetFechaDevolucion()}, {devolucion.GetMontoPagado()})";

            filasAfectadasA = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
