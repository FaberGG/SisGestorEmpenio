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
        }


      private void Buscar_Click(object sender, RoutedEventArgs e)
{
    if (int.TryParse(txtIdArticulo.Text, out int id))
    {
        var repo = new ArticuloRepository();
        var articulo = repo.Buscar(id);

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
