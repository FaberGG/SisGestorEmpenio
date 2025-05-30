using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SisGestorEmpenio.Utils;
using System.Text.RegularExpressions;
using SisGestorEmpenio.Sesion;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para BuscarArticuloPorId.xaml
    /// </summary>
    public partial class BuscarArticuloPorId : Window
    {
        public Articulo Articulo { get; set; }

        public BuscarArticuloPorId()
        {
            InitializeComponent();
            // Máximas longitudes según tabla
            txtIdArticulo.MaxLength = 10;   // INT

            // Prevención de caracteres inválidos
            txtIdArticulo.PreviewTextInput += SoloNumeros_Preview;

            // Validaciones LostFocus
            txtIdArticulo.LostFocus += (s, e) => ValidacionHelper.ValidarEntero(txtIdArticulo, lblIdArticulo, "Identificador del articulo*");
        }

        // Sólo dígitos
        private void SoloNumeros_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
{       
    if (ValidacionHelper.ValidarIdentificador(txtIdArticulo, lblIdArticulo, "Identificador del articulo*"))
    {
        Articulo articulo = Sesion.Sesion.GetAdministradorActivo().BuscarArticulo(txtIdArticulo.Text.Trim());
        if (articulo != null)
        {
            this.Articulo = articulo;
            DialogResult = true;
            Close();
        }
        else
        {
            MessageBox.Show("Artículo no encontrado.");
        }
    }
    else
    {
        MessageBox.Show("Ingrese un ID válido.");
    }
}


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
