using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;
namespace SisGestorEmpenio.repository
{
    internal class PrestamoRepository
    {
        ConexionDB dt = new ConexionDB();

        public bool Guardar(Prestamo prestamo)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Prestamo VALUES ({prestamo.GetCliente().GetId()}, {prestamo.GetArticulo().GetIdArticulo()}, '{prestamo.GetEstado().ToLower()}', DATE '{prestamo.GetFechaInicio().ToString("yyyy-MM-dd")}', DATE '{prestamo.GetFechaFin().ToString("yyyy-MM-dd")}', {prestamo.GetTasaInteres().ToString(System.Globalization.CultureInfo.InvariantCulture)} , {prestamo.CalcularMontoTotal().ToString(System.Globalization.CultureInfo.InvariantCulture)})";

            
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }


        public bool ActualizarEstado(Prestamo prestamo)
        {
            int filasAfectadas = 0;
            string consulta = $"UPDATE Prestamo SET estadoPrestamo = '{prestamo.GetEstado().ToLower()}' WHERE numeroIdentidadCliente = {prestamo.GetCliente().GetId()} AND idArticulo = {prestamo.GetArticulo().GetIdArticulo()}";
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }

        public bool EstaGuardado(string clienteId, string articuloId)
        {
            string consulta = $"SELECT * FROM prestamo WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }

        public Prestamo Buscar(string clienteId, string articuloId)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            ArticuloRepository articuloRepository = new ArticuloRepository();
            string consulta = $"SELECT * FROM prestamo WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                var row = resultado.Tables[0].Rows[0];
                return new Prestamo(
                    clienteRepository.Buscar(row["numeroIdentidadCliente"].ToString()),
                    articuloRepository.Buscar(row["idArticulo"].ToString()),
                    row["estadoPrestamo"].ToString(),
                    DateTime.Parse(row["fechaInicio"].ToString()),
                    DateTime.Parse(row["fechaFin"].ToString()),
                    Convert.ToDouble(row["tasaInteres"]),
                    Convert.ToDouble(row["montoTotal"])
                );
            }
            return null;
        }

        public bool Actualizar(Prestamo prestamo)
        {
            int filasAfectadas = 0;
            string consulta = $"UPDATE prestamo SET estadoPrestamo = '{prestamo.GetEstado().ToLower()}', fechaInicio = DATE '{prestamo.GetFechaInicio().ToString("yyyy-MM-dd")}', fechaFin = DATE '{prestamo.GetFechaFin().ToString("yyyy-MM-dd")}', tasaInteres = {prestamo.GetTasaInteres().ToString(System.Globalization.CultureInfo.InvariantCulture)}, montoTotal = {prestamo.CalcularMontoTotal().ToString(System.Globalization.CultureInfo.InvariantCulture)} WHERE numeroIdentidadCliente = {prestamo.GetCliente().GetId()} AND idArticulo = {prestamo.GetArticulo().GetIdArticulo()}";
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
        // SisGestorEmpenio/repository/PrestamoRepository.cs
        public List<Prestamo> ObtenerTodos()
        {
            var list = new List<Prestamo>();
            string sql = "SELECT * FROM prestamo";
            var ds = dt.ejecutarSelect(sql);
            if (ds.Tables.Count == 0) return list;

            var clienteRepo = new ClienteRepository();
            var articuloRepo = new ArticuloRepository();

            foreach (System.Data.DataRow row in ds.Tables[0].Rows)
            {
                var cli = clienteRepo.Buscar(row["numeroIdentidadCliente"].ToString());
                var art = articuloRepo.Buscar(row["idArticulo"].ToString());
                var estado = row["estadoPrestamo"].ToString();
                var fi = DateTime.Parse(row["fechaInicio"].ToString());
                var ff = DateTime.Parse(row["fechaFin"].ToString());
                var tasa = Convert.ToDouble(row["tasaInteres"]);
                var monto = Convert.ToDouble(row["montoTotal"]);

                list.Add(new Prestamo(cli, art, estado, fi, ff, tasa, monto));
            }

            return list;
        }

        public List<Prestamo> BuscarPrestamosCoincidentes(int cantidadMaxPrestamos, string clienteId, string estado, int rangoDias)
        {
            var prestamos = new List<Prestamo>();
            var condiciones = new List<string>();
            var orderBy = "ORDER BY fechaInicio DESC";

            bool filtrarPorCliente = !string.IsNullOrEmpty(clienteId);

            if (filtrarPorCliente)
            {
                // Se da prioridad a coincidencias exactas con CASE
                condiciones.Add($"CAST(numeroIdentidadCliente AS VARCHAR2(20)) LIKE '{clienteId}%'");
                orderBy = $@"
            ORDER BY 
                CASE 
                    WHEN CAST(numeroIdentidadCliente AS VARCHAR2(20)) = '{clienteId}' THEN 0 
                    ELSE 1 
                END,
                fechaInicio DESC";
            }

            if (estado.ToLower().Contains("activo") || estado.ToLower().Contains("inactivo"))
            {
                condiciones.Add($"LOWER(estadoPrestamo) = '{estado.ToLower()}'");
            }

            if (rangoDias > -1)
            {
                condiciones.Add($"fechaInicio >= SYSDATE - {rangoDias}");
            }

            string whereClause = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : "";

            string consulta = $@"
        SELECT * FROM (
            SELECT * FROM prestamo
            {whereClause}
            {orderBy}
        )
        WHERE ROWNUM <= {cantidadMaxPrestamos}";

            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count == 0 || resultado.Tables[0].Rows.Count == 0)
                return prestamos;

            var clienteRepo = new ClienteRepository();
            var articuloRepo = new ArticuloRepository();

            foreach (System.Data.DataRow row in resultado.Tables[0].Rows)
            {
                var cliente = clienteRepo.Buscar(row["numeroIdentidadCliente"].ToString());
                var articulo = articuloRepo.Buscar(row["idArticulo"].ToString());
                var estadoPrestamo = row["estadoPrestamo"].ToString();
                var fechaInicio = DateTime.Parse(row["fechaInicio"].ToString());
                var fechaFin = DateTime.Parse(row["fechaFin"].ToString());
                var tasaInteres = Convert.ToDouble(row["tasaInteres"]);
                var montoTotal = Convert.ToDouble(row["montoTotal"]);

                prestamos.Add(new Prestamo(cliente, articulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal));
            }

            return prestamos;
        }


    }
}
