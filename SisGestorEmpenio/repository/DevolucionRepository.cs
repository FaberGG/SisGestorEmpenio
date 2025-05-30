﻿using System;
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
    }
}
