using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;

namespace SisGestorEmpenio.repository
{
    internal class ArticuloRepository
    {
        ConexionDB dt = new ConexionDB();

        public bool guardar(Articulo articulo)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Articulo VALUES ({articulo.GetIdArticulo()}, '{articulo.GetDescripcion()}', {articulo.GetValorEstimado()}, '{articulo.GetEstado().ToLower()}')";

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
