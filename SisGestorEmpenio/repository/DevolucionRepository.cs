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
        public bool EstaGuardado(int clienteId, int articuloId)
        {
            string consulta = $"SELECT * FROM devolucion WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }

        public Devolucion Buscar(int clienteId, int articuloId)
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
                    prestamoRepository.Buscar(Convert.ToInt32(row["numeroIdentidadCliente"]), Convert.ToInt32(row["idArticulo"]))
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

        public List<Devolucion> BuscarDevolucionesCoincidentes(int cantidadMaxDevoluciones, int clienteId, int articuloId, int rangoDias)
        {
            var devoluciones = new List<Devolucion>();
            var condiciones = new List<string>();

            if (clienteId != -1)
            {
                condiciones.Add($"CAST(numeroIdentidadCliente AS VARCHAR2(20)) LIKE '{clienteId}%'");
            }

            if (articuloId != -1)
            {
                condiciones.Add($"CAST(idArticulo AS VARCHAR2(20)) LIKE '{articuloId}%'");
            }

            if (rangoDias > -1)
            {
                condiciones.Add($"fechaDevolucion >= SYSDATE - {rangoDias}");
            }

            string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

            string consulta = $@"
        SELECT * FROM (
            SELECT * FROM devolucion
            {whereClause}
            ORDER BY fechaDevolucion DESC
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
                int idCliente = Convert.ToInt32(row["numeroIdentidadCliente"]);
                int idArticulo = Convert.ToInt32(row["idArticulo"]);

                // Obtener el préstamo relacionado
                var prestamo = prestamoRepo.Buscar(idCliente, idArticulo);

                // Crear objeto Devolucion
                var devolucion = new Devolucion(numConvenio, fechaDevolucion, montoPagado, prestamo);

                devoluciones.Add(devolucion);
            }

            return devoluciones;
        }
        public List<Devolucion> ConsultarDevoluciones(int? clienteId = null, int? articuloId = null, int? rangoDias = null)
        {
            var devoluciones = new List<Devolucion>();
            var condiciones = new List<string>();

            // Filtrar por cliente si se proporciona
            if (clienteId.HasValue)
            {
                condiciones.Add($"numeroIdentidadCliente = {clienteId.Value}");
            }

            // Filtrar por articulo si se proporciona
            if (articuloId.HasValue)
            {
                condiciones.Add($"idArticulo = {articuloId.Value}");
            }

            // Filtrar por rango de dias si se proporciona
            if (rangoDias.HasValue)
            {
                condiciones.Add($"fechaDevolucion >= SYSDATE - {rangoDias.Value}");
            }

            // Combinar condiciones en el WHERE
            string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

            string consulta = $@"
        SELECT * FROM (
            SELECT * FROM devolucion
            {whereClause}
            ORDER BY fechaDevolucion DESC
        )
        WHERE ROWNUM <= 100"; // Puedes ajustar el límite si quieres

            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count == 0 || resultado.Tables[0].Rows.Count == 0)
                return devoluciones;

            var prestamoRepo = new PrestamoRepository();

            foreach (System.Data.DataRow row in resultado.Tables[0].Rows)
            {
                int numConvenio = Convert.ToInt32(row["numConvenio"]);
                DateTime fechaDevolucion = DateTime.Parse(row["fechaDevolucion"].ToString());
                double montoPagado = Convert.ToDouble(row["montoPagado"]);
                int idCliente = Convert.ToInt32(row["numeroIdentidadCliente"]);
                int idArticulo = Convert.ToInt32(row["idArticulo"]);

                var prestamo = prestamoRepo.Buscar(idCliente, idArticulo);

                var devolucion = new Devolucion(numConvenio, fechaDevolucion, montoPagado, prestamo);
                devoluciones.Add(devolucion);
            }

            return devoluciones;
        }


    }
}
