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

        public bool Guardar(Devolucion devolucion)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Devolucion VALUES ({devolucion.GetPrestamo().GetCliente().GetId()}, {devolucion.GetPrestamo().GetArticulo().GetIdArticulo()}, NULL,  DATE '{devolucion.GetFechaDevolucion().ToString("yyyy-MM-dd")}', {devolucion.GetMontoPagado().ToString(System.Globalization.CultureInfo.InvariantCulture)})";

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
        public bool EstaGuardado(string clienteId, string articuloId)
        {
            string consulta = $"SELECT * FROM devolucion WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }

        public Devolucion Buscar(string clienteId, string articuloId)
        {
            PrestamoRepository prestamoRepository = new PrestamoRepository();
            string consulta = $"SELECT * FROM devolucion WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                var row = resultado.Tables[0].Rows[0];
                return new Devolucion(
                    Convert.ToInt32(row["numConvenio"]),
                    //capturar fechaDevolucion
                    DateTime.Parse(row["fechaDevolucion"].ToString()),
                    Convert.ToDouble(row["montoPagado"]),
                    prestamoRepository.Buscar(row["numeroIdentidadCliente"].ToString(), row["idArticulo"].ToString())
                );
            }
            return null;
        }

        //actualizar devolucion con fechaDevolucion y montoPagado
        public bool Actualizar(Devolucion devolucion)
        {
            int filasAfectadas = 0;
            string consulta = $"UPDATE devolucion SET fechaDevolucion = DATE '{devolucion.GetFechaDevolucion().ToString("yyyy-MM-dd")}', montoPagado = {devolucion.GetMontoPagado().ToString(System.Globalization.CultureInfo.InvariantCulture)} WHERE numeroIdentidadCliente = {devolucion.GetPrestamo().GetCliente().GetId()} AND idArticulo = {devolucion.GetPrestamo().GetArticulo().GetIdArticulo()}";
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }

        public List<Devolucion> BuscarDevolucionesCoincidentes(int cantidadMaxDevoluciones, string clienteId, string articuloId, int rangoDias)
        {
            var devoluciones = new List<Devolucion>();
            var condiciones = new List<string>();

            bool filtrarCliente = !string.IsNullOrWhiteSpace(clienteId);
            bool filtrarArticulo = !string.IsNullOrWhiteSpace(articuloId);

            if (filtrarCliente)
            {
                condiciones.Add($"CAST(numeroIdentidadCliente AS VARCHAR2(20)) LIKE '{clienteId}%'");
            }

            if (filtrarArticulo)
            {
                condiciones.Add($"CAST(idArticulo AS VARCHAR2(20)) LIKE '{articuloId}%'");
            }

            if (rangoDias > -1)
            {
                condiciones.Add($"fechaDevolucion >= SYSDATE - {rangoDias}");
            }

            string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

            // Usar coincidencia exacta y parcial para definir la prioridad
            string clienteIdStr = clienteId ?? "";
            string articuloIdStr = articuloId ?? "";

            string consulta = $@"
SELECT * FROM (
    SELECT d.*,
        CASE
            WHEN CAST(d.numeroIdentidadCliente AS VARCHAR2(20)) = '{clienteIdStr}' 
                 OR CAST(d.idArticulo AS VARCHAR2(20)) = '{articuloIdStr}' THEN 1
            WHEN CAST(d.numeroIdentidadCliente AS VARCHAR2(20)) LIKE '{clienteIdStr}%' 
                 OR CAST(d.idArticulo AS VARCHAR2(20)) LIKE '{articuloIdStr}%' THEN 2
            ELSE 3
        END AS prioridad
    FROM devolucion d
    {whereClause}
    ORDER BY prioridad ASC, fechaDevolucion DESC
)
WHERE ROWNUM <= {cantidadMaxDevoluciones}";

            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count == 0 || resultado.Tables[0].Rows.Count == 0)
                return devoluciones;

            var prestamoRepo = new PrestamoRepository();

            foreach (System.Data.DataRow row in resultado.Tables[0].Rows)
            {
                int numConvenio = Convert.ToInt32(row["numConvenio"]);
                DateTime fechaDevolucion = DateTime.Parse(row["fechaDevolucion"].ToString());
                double montoPagado = Convert.ToDouble(row["montoPagado"]);

                string idClienteStr = row["numeroIdentidadCliente"].ToString();
                string idArticuloStr = row["idArticulo"].ToString();

                Prestamo prestamo = prestamoRepo.Buscar(idClienteStr, idArticuloStr);

                var devolucion = new Devolucion(numConvenio, fechaDevolucion, montoPagado, prestamo);
                devoluciones.Add(devolucion);
            }

            return devoluciones;
        }



    }
}
