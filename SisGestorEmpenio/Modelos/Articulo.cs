using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Modelos
{
    internal class Articulo
    {
        // Propiedades del artículo
        public int idArticulo { get; set; }
        public string descripcion { get; set; }
        public double valorEstimado { get; set; }
        public string estado { get; set; }

        // Constructor vacío
        public Articulo()
        {
        }

        // Constructor para registrar un artículo
        public void RegistrarArticulo(int id, string descripcion, double valorEstimado, string estado)
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
    }
}
