using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.repository;

namespace SisGestorEmpenio.Modelos
{
    public class Articulo
    {
        // Propiedades del artículo
        private string idArticulo;
        private string descripcion;
        private double valorEstimado;
        private string estadoArticulo;
        private bool propiedadCasa;
        private string estadoDevolucion;
        private Administrador administrador;
        // Constructor vacío
        public Articulo()
        {
        }

        // Constructor para registrar un artículo
        public Articulo(string id, string descripcion, double valorEstimado, string estadoArticulo)
        {
            this.idArticulo = id;
            this.descripcion = descripcion;
            this.valorEstimado = valorEstimado;
            this.estadoArticulo = estadoArticulo;
            this.propiedadCasa = false; // Por defecto, no es propiedad de la casa
            this.estadoDevolucion = "libre"; // Por defecto, no ha sido devuelto
        }

        // Constructor para buscar un artículo
        public Articulo(string id, string descripcion, double valorEstimado, string estadoArticulo, bool propiedadCasa, string estadoDevolucion)
        {
            this.idArticulo = id;
            this.descripcion = descripcion;
            this.valorEstimado = valorEstimado;
            this.estadoArticulo = estadoArticulo;
            this.propiedadCasa = propiedadCasa;
            this.estadoDevolucion = estadoDevolucion;
        }

        // Método para mostrar los detalles del artículo
        public string MostrarDetalleArticulo()
        {
            return $"ID: {idArticulo}\nDescripción: {descripcion}\nValor Estimado: ${valorEstimado:F2}\nEstado: {estadoArticulo}";
        }
        public bool marcarComoDevuelto()
        {
            estadoDevolucion = "devuelto";
            ArticuloRepository articuloRepository = new ArticuloRepository();
            return articuloRepository.MarcarComoDevuelto(idArticulo);
        }
        public void marcarComoLibre()
        {
            estadoDevolucion = "libre";
        }
        //Set y get de las propiedades del artículo 
        public Administrador GetAdministrador()
        {
            return administrador;
        }
        public void SetAdministrador(Administrador value)
        {
            administrador = value;
        }
        public string GetIdArticulo()
        {
            return idArticulo;
        }

        public void SetIdArticulo(string value)
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

        public string GetEstadoArticulo()
        {
            return estadoArticulo;
        }

        public void SetEstadoArticulo(string value)
        {
            estadoArticulo = value;
        }

        public bool GetPropiedadCasa()
        {
            return propiedadCasa;
        }
        public void SetPropiedadCasa(bool value)
        {
            propiedadCasa = value;
        }

        public string GetEstadoDevolucion()
        {
            return estadoDevolucion;
        }
        public void SetEstadoDevolucion(string value)
        {
            estadoDevolucion = value;
        }
    }
}
