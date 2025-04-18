using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para MensajeErrorOk.xaml
    /// </summary>
    public partial class MensajeErrorOk : Window, INotifyPropertyChanged
    {

        public string Mensaje { get; set; } = "¿Estás seguro?";
        public string Titulo { get; set; } = "Error";
        public string TextoBotonIzquierdo { get; set; } = "Aceptar";
        public Brush ColorBotonIzquierdo { get; set; } = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3A7575"));

        public bool Confirmado { get; private set; } = false;
        public MensajeErrorOk()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Confirmado = true;
            DialogResult = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
