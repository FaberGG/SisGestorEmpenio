using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SisGestorEmpenio.repository
{
    internal class AdministradorRepository
    {
        ConexionDB dt = new ConexionDB();

        public bool guardar(Administrador administrador)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Administrador VALUES ({administrador.GetId()}, '{administrador.GetNombre()}', '{administrador.GetTipoIdentidad()}', {administrador.GetSalario().ToString(System.Globalization.CultureInfo.InvariantCulture)}, {administrador.GetAniosExp()}, '{administrador.GetUsuario()}', '{administrador.GetContrasenia()}')";

            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }

        public Administrador buscarPorCredenciales(string usuario, string contrasenia)
        {
            string consulta = $"SELECT * FROM Administrador WHERE usuario = '{usuario}' AND contrasenia = '{contrasenia}'";
            var resultado = dt.ejecutarSelect(consulta);

            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                var fila = resultado.Tables[0].Rows[0];

                Administrador admin = new Administrador(
                    fila["nombre"].ToString(),
                    fila["numeroIdentidad"].ToString(),
                    fila["tipoIdentidad"].ToString(),
                    Convert.ToDouble(fila["salario"]),
                    Convert.ToInt32(fila["aniosExperiencia"]),
                    fila["usuario"].ToString(),
                    fila["contrasenia"].ToString()
                );

                return admin;
            }

            return null;
        }
    }
}
