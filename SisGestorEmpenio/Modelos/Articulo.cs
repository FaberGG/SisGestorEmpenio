﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    internal class Articulo
    {
        // Propiedades del artículo
        private int idArticulo;
        private string descripcion;
        private double valorEstimado;
        private string estado;
        // Constructor vacío
        public Articulo()
        {
        }

        // Constructor para registrar un artículo
        public Articulo(int id, string descripcion, double valorEstimado, string estado)
        {
            idArticulo = id;
            this.descripcion = descripcion;
            this.valorEstimado = valorEstimado;
            this.estado = estado;
        }

        // Método para mostrar los detalles del artículo
        public string MostrarDetalleArticulo()
        {
            return $"ID: {idArticulo}\nDescripción: {descripcion}\nValor Estimado: ${valorEstimado:F2}\nEstado: {estado}";
        }

        //Set y get de las propiedades del artículo 
        public int GetIdArticulo()
        {
            return idArticulo;
        }

        public void SetIdArticulo(int value)
        {
            idArticulo = value;
        }

        public string GetDescripcion()
        {
            return descripcion;
        }

        public void SetDescripcion(string value)
        {
            descripcion = value;
        }

        public double GetValorEstimado()
        {
            return valorEstimado;
        }

        public void SetValorEstimado(double value)
        {
            valorEstimado = value;
        }

        public string GetEstado()
        {
            return estado;
        }

        public void SetEstado(string value)
        {
            estado = value;
        }
    }
}
