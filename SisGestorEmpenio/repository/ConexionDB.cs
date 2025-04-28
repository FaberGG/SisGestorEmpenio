using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace SisGestorEmpenio.repository
{
    internal class ConexionDB
    {
        //paso 1: crear la cadena de conexion
        //La configuracion para la cadena de conexion se movio a App.config en la raiz del proyecto
        string cadenaConexion = ConfigurationManager.ConnectionStrings["MiConexionOracle"].ConnectionString;
        //string cadenaConexion = "Data Source=localhost;User ID=system;Password=oracle";

        /*paso 2: crear el metodo que permite ejecutar
         * una operación DML insertar, actualizar o borrar*/
        public int ejecutarDML(string consulta)
        {
            int filasAfectadas = 0;
            //paso1: crear una conexion
            OracleConnection miConexion = new OracleConnection(cadenaConexion);

            //paso 2: crear un obj de tipo comando el cual recibe
            //la instruccion sql que que se va a ejecutar
            OracleCommand miComando = new OracleCommand(consulta, miConexion);

            //paso3: abrir la conexion
            miConexion.Open();

            //paso4: ejecuto el comando.Retorna el numero de filas
            //que se afectaron en la operacion DML

            filasAfectadas = miComando.ExecuteNonQuery();

            //paso 5: cerrar la conexion
            miConexion.Close();

            //paso 6: retorno las filas afectadas por la operacion DML
            return filasAfectadas;
        }
        public DataSet ejecutarSelect(string consultaSELECT)
        {
            //paso 1: creo un data set vacio
            DataSet ds = new DataSet();
            //paso2: creo un adaptador
            OracleDataAdapter miAdaptador = new OracleDataAdapter(consultaSELECT, cadenaConexion);
            //paso 3: llenar el dataset a través del adaptador
            miAdaptador.Fill(ds, "ResultadoDatos");
            return ds;
        }

        
    }
}
