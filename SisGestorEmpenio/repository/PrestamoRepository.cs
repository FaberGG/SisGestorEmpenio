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

        public bool EstaGuardado(int clienteId, int articuloId)
        {
            string consulta = $"SELECT * FROM prestamo WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }

        public Prestamo Buscar(int clienteId, int articuloId)
        {
            ClienteRepository clienteRepository = new ClienteRepository();
            ArticuloRepository articuloRepository = new ArticuloRepository();
            string consulta = $"SELECT * FROM prestamo WHERE numeroIdentidadCliente = {clienteId} AND idArticulo = {articuloId}";
            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                var row = resultado.Tables[0].Rows[0];
                return new Prestamo(
                    clienteRepository.Buscar(Convert.ToInt32(row["numeroIdentidadCliente"])),
                    articuloRepository.Buscar(Convert.ToInt32(row["idArticulo"])),
                    row["estadoPrestamo"].ToString(),
                    DateTime.Parse(row["fechaInicio"].ToString()),
                    DateTime.Parse(row["fechaFin"].ToString()),
                    Convert.ToDouble(row["tasaInteres"]),
                    Convert.ToDouble(row["montoTotal"])
                );
            }
            return null;
        }
    }
}
