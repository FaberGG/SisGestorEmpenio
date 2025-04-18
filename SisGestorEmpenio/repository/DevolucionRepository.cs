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
            string consulta = $"INSERT INTO Devolucion VALUES ({devolucion.GetPrestamo().GetCliente().GetId()}, {devolucion.GetPrestamo().GetArticulo().GetIdArticulo()}, DATE '{devolucion.GetFechaDevolucion().ToString("yyyy-MM-dd HH:mm:ss")}', {devolucion.GetMontoPagado().ToString(System.Globalization.CultureInfo.InvariantCulture)})";

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
