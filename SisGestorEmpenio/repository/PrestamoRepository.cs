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

        public bool guardar(Prestamo prestamo)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Prestamo VALUES ({prestamo.GetCliente().GetId()}, {prestamo.GetArticulo().GetIdArticulo()}, '{prestamo.GetEstado().ToLower()}', DATE '{prestamo.GetFechaInicio().ToString("yyyy-MM-dd HH:mm:ss")}', DATE '{prestamo.CalcularFechaVencimiento().ToString("yyyy-MM-dd HH:mm:ss")}', {prestamo.GetTasaInteres().ToString(System.Globalization.CultureInfo.InvariantCulture)} , {prestamo.CalcularMontoTotal().ToString(System.Globalization.CultureInfo.InvariantCulture)})";

            //VER CÓMO EN LA CONSULTA SE PONE LOS DATOS TIPO TIME 

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
