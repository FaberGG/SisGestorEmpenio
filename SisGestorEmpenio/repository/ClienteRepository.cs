﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repository;

namespace SisGestorEmpenio.repository
{
    internal class ClienteRepository
    {
        ConexionDB dt = new ConexionDB();

        public bool Guardar(Cliente cliente)
        {
            int filasAfectadas = 0;
            string consulta = $"INSERT INTO Cliente VALUES ({cliente.GetId()}, '{cliente.GetNombre()}', '{cliente.GetApellido()}', '{cliente.GetTelefono()}', '{cliente.GetCorreo()}', '{cliente.GetTipoIdentidad()}', {cliente.GetAdministrador().GetId()})";
            
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
        public bool EstaGuardado(int id)
        {
            string consulta = $"SELECT * FROM cliente WHERE numeroIdentidad = {id}";
            var resultado = dt.ejecutarSelect(consulta);

            bool isSaved = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return isSaved;
        }

        public bool PoseeArticulo(int idCliente, int idArticulo)
        {
            string consulta = $"SELECT * FROM posee WHERE idCliente = {idCliente} AND idArticulo = {idArticulo}";
            var resultado = dt.ejecutarSelect(consulta);
            bool posee = (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0);
            return posee;
        }

        public Cliente Buscar(int id)
        {
            string consulta = $"SELECT * FROM cliente WHERE numeroIdentidad = {id}";
            var resultado = dt.ejecutarSelect(consulta);
            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                var row = resultado.Tables[0].Rows[0];
                return new Cliente(
                    row["nombre"].ToString(),
                    Convert.ToInt32(row["numeroIdentidad"]),
                    row["tipoIdentidad"].ToString(),
                    row["apellido"].ToString(),
                    row["telefono"].ToString(),
                    row["correo"].ToString(),
                    new Administrador(
                        "",
                        Convert.ToInt32(row["numeroIdentidadAdministrador"]),
                        "",
                        0,
                        0,
                        "",
                        ""
                    )
                );
            }
            return null;
        }

        public bool Actualizar(Cliente cliente)
        {
            int filasAfectadas = 0;
            string consulta = $"UPDATE cliente SET nombre = '{cliente.GetNombre()}', apellido = '{cliente.GetApellido()}', telefono = '{cliente.GetTelefono()}', correo = '{cliente.GetCorreo()}' WHERE numeroIdentidad = {cliente.GetId()}";
            filasAfectadas = dt.ejecutarDML(consulta);
            return filasAfectadas > 0;
        }
    }
}
