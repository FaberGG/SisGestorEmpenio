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

namespace SisGestorEmpenio
{
    /// <summary>
    /// Lógica de interacción para LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void OnLoginSuccess(object sender, EventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close(); // Cierra la ventana de login
        }
    }
}
