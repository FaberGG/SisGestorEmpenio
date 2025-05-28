using SisGestorEmpenio.Modelos;
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
    /// Lógica de interacción para DetallesDevolucion.xaml
    /// </summary>
    public partial class DetallesDevolucion : Window
    {
        public DetallesDevolucion()
        {
            InitializeComponent();
        }

        public void CargarDetalles(Devolucion devolucion)
        {
            ucDetalles.CargarDetalles(devolucion);
        }
    }
}
