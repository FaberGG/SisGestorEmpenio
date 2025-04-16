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
    /// Lógica de interacción para ConfirmacionWindow.xaml
    /// </summary>
    public partial class ConfirmacionWindow : Window, INotifyPropertyChanged
    {
        public string Mensaje { get; set; } = "¿Estás seguro?";
        public string Titulo { get; set; } = "Confirmación";
        public string TextoBotonIzquierdo { get; set; } = "Aceptar";
        public string TextoBotonDerecho { get; set; } = "Cancelar";
        public Brush ColorBotonIzquierdo { get; set; } = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3A7575"));
        public Brush ColorBotonDerecho { get; set; } = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3A7575"));
        public bool MostrarBotonDerecho { get; set; } = true;

        public bool Confirmado { get; private set; } = false;

        public ConfirmacionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Confirmado = true;
            DialogResult = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Confirmado = false;
            DialogResult = false;
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
