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

        public bool Guardar(Articulo articulo)
        {
            int filasAfectadas = 0;
            int propiedadCasaInt = articulo.GetPropiedadCasa() ? 1 : 0;
            string consulta = $"INSERT INTO Articulo VALUES ({articulo.GetIdArticulo()}, '{articulo.GetDescripcion()}', {articulo.GetValorEstimado().ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{articulo.GetEstadoArticulo().ToLower()}', {propiedadCasaInt}, '{articulo.GetEstadoDevolucion().ToLower()}' )";

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }

        public bool EstaGuardado(int id)
        {
            string consulta = $"SELECT * FROM ARTICULO WHERE idArticulo = {id}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }

        public bool MarcarComoDevuelto(int idArticulo)
        {
            int filasAfectadas = 0;
            string consulta = $"UPDATE ARTICULO SET estadoDevolucion = 'devuelto' WHERE idArticulo = {idArticulo}";
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
