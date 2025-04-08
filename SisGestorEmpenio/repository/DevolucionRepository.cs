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
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Devolucion VALUES ({devolucion.GetPrestamo().GetCliente().GetTipoIdentidad()}, {devolucion.GetPrestamo().GetArticulo().GetIdArticulo()}, {devolucion.GetFechaDevolucion()}, {devolucion.GetMontoPagado()})";

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
