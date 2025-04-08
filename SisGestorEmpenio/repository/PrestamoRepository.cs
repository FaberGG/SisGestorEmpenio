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
            int filasAfectadasA = 0;
            string consulta = $"INSERT INTO Prestamo VALUES ({prestamo.GetCliente().GetTipoIdentidad()}, {prestamo.GetArticulo().GetIdArticulo()}, {articulo.GetValorEstimado()}, '{prestamo.GetEstado()}',{prestamo.GetFechaInicio()}, {prestamo.CalcularFechaVencimiento()}, {prestamo.tasaInteres()},{prestamo.CalcularMontoTotal()})";

            //VER CÓMO EN LA CONSULTA SE PONE LOS DATOS TIPO TIME 

            filasAfectadasA = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
