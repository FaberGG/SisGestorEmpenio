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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SisGestorEmpenio.vistas
{
    /// <summary>
    /// Lógica de interacción para ModificarPrestamoView.xaml
    /// </summary>
    public partial class ModificarPrestamoView : UserControl
    {
        public Prestamo prestamo;
        public ModificarPrestamoView()
        {
            InitializeComponent();
        }
        public ModificarPrestamoView(Prestamo prestamo)
        {
            InitializeComponent();
            this.prestamo = prestamo;
            DataContext = prestamo;
        }
    }
}
