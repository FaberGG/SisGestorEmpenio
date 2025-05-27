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

        public Articulo Buscar(int id)
        {
            string consulta = $"SELECT * FROM ARTICULO WHERE idArticulo = {id}";
            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                var row = resultado.Tables[0].Rows[0];
                return new Articulo(
                    Convert.ToInt32(row["idArticulo"]),
                    row["descripcion"].ToString(),
                    Convert.ToDouble(row["valorEstimado"]),
                    row["estadoArticulo"].ToString(),
                    Convert.ToBoolean(row["propiedadCasa"]),
                    row["estadoDevolucion"].ToString()
                );
            }
            return null;
        }

        public bool Actualizar(Articulo articulo)
        {
            int filasAfectadas = 0;
            int propiedadCasaInt = articulo.GetPropiedadCasa() ? 1 : 0;
            string consulta = $"UPDATE ARTICULO SET descripcion = '{articulo.GetDescripcion()}', valorEstimado = {articulo.GetValorEstimado().ToString(System.Globalization.CultureInfo.InvariantCulture)}, estadoArticulo = '{articulo.GetEstadoArticulo().ToLower()}', propiedadCasa = {propiedadCasaInt}, estadoDevolucion = '{articulo.GetEstadoDevolucion().ToLower()}' WHERE idArticulo = {articulo.GetIdArticulo()}";
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }

        public List<Articulo> BuscarArticulosCoincidentes(int cantidadMaxArticulos, int id, string descripcion, int propiedadCasa, string estado, string devolucion)
        {
            var articulos = new List<Articulo>();
            var condiciones = new List<string>();
            var condicionesIdDescripcion = new List<string>();

            if (id != -1)
                condicionesIdDescripcion.Add($"CAST(idArticulo AS VARCHAR2(20)) LIKE '{id}%'");

            if (!string.IsNullOrWhiteSpace(descripcion))
                condicionesIdDescripcion.Add($"LOWER(descripcion) LIKE '%{descripcion.ToLower()}%'");

            if (condicionesIdDescripcion.Count > 0)
                condiciones.Add("(" + string.Join(" OR ", condicionesIdDescripcion) + ")");

            if (propiedadCasa != -1)
                condiciones.Add($"propiedadCasa = {propiedadCasa}");

            if (!string.IsNullOrWhiteSpace(estado))
                condiciones.Add($"LOWER(estadoArticulo) = '{estado.ToLower()}'");

            if (!string.IsNullOrWhiteSpace(devolucion))
                condiciones.Add($"LOWER(estadoDevolucion) = '{devolucion.ToLower()}'");

            string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

            string idStr = id != -1 ? id.ToString() : "";
            string descripcionStr = !string.IsNullOrWhiteSpace(descripcion) ? descripcion.ToLower() : "";

            string consulta = $@"
SELECT * FROM (
    SELECT a.*,
        CASE
            WHEN a.idArticulo = {id} THEN 1
            WHEN CAST(a.idArticulo AS VARCHAR2(20)) LIKE '{idStr}%' THEN 2
            WHEN LOWER(a.descripcion) LIKE '{descripcionStr}%' THEN 3
            WHEN LOWER(a.descripcion) LIKE '%{descripcionStr}%' THEN 4
            ELSE 5
        END AS prioridad
    FROM articulo a
    {whereClause}
    ORDER BY prioridad, a.idArticulo
)
WHERE ROWNUM <= {cantidadMaxArticulos}";

            var resultado = dt.ejecutarSelect(consulta);

            if (resultado.Tables.Count == 0 || resultado.Tables[0].Rows.Count == 0)
                return articulos;

            foreach (System.Data.DataRow row in resultado.Tables[0].Rows)
            {
                var articulo = new Articulo(
                    Convert.ToInt32(row["idArticulo"]),
                    row["descripcion"].ToString(),
                    Convert.ToDouble(row["valorEstimado"]),
                    row["estadoArticulo"].ToString(),
                    Convert.ToBoolean(row["propiedadCasa"]),
                    row["estadoDevolucion"].ToString()
                );
                articulos.Add(articulo);
            }

            return articulos;
        }


    }
}
