using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            SelectTab(1);
        }
        private void Tab1_Click(object sender, RoutedEventArgs e) => SelectTab(1);
        private void Tab2_Click(object sender, RoutedEventArgs e) => SelectTab(2);
        private void Tab3_Click(object sender, RoutedEventArgs e) => SelectTab(3);

        private void SelectTab(int tabNumber)
        {
            // Restablecer todos
            ResetTabStyles();

            switch (tabNumber)
            {
                case 1:
                    Tab1.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#095858");
                    Label1.FontWeight = FontWeights.Bold;
                    ContentArea.Content = new ClienteView(prestamo.GetCliente());
                    break;
                case 2:
                    Tab2.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#095858");
                    Label2.FontWeight = FontWeights.Bold;
                    ContentArea.Content = new ArticuloView(prestamo.GetArticulo());
                    break;
                case 3:
                    Tab3.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#095858");
                    Label3.FontWeight = FontWeights.Bold;
                    ContentArea.Content = new PrestamoView(prestamo);
                    break;
            }
        }


        private void ResetTabStyles()
        {
            Tab1.BorderBrush = Brushes.Transparent;
            Tab2.BorderBrush = Brushes.Transparent;
            Tab3.BorderBrush = Brushes.Transparent;

            Label1.FontWeight = FontWeights.Normal;
            Label2.FontWeight = FontWeights.Normal;
            Label3.FontWeight = FontWeights.Normal;
        }




        public ModificarPrestamoView(Prestamo prestamo)
        {
            InitializeComponent();
            this.prestamo = prestamo;
            DataContext = prestamo;
        }
    }
}
