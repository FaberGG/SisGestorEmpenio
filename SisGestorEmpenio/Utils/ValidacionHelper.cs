using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SisGestorEmpenio.Utils
{
    public static class ValidacionHelper
    {
        public static bool ValidarCampo(TextBox textBox, TextBlock label, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                label.Text = $"{nombreCampo} *";
                label.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                label.Text = nombreCampo;
                label.Foreground = Brushes.Black;
                return true;
            }
        }

        public static bool ValidarCampo(ComboBox comboBox, TextBlock label, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(comboBox.Text))
            {
                label.Text = $"{nombreCampo} *";
                label.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                label.Text = nombreCampo;
                label.Foreground = Brushes.Black;
                return true;
            }
        }
        public static bool ValidarCampo(DatePicker datePicker, TextBlock label, string nombreCampo)
        {
            if (!datePicker.SelectedDate.HasValue)
            {
                label.Text = $"{nombreCampo} *";
                label.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                label.Text = nombreCampo;
                label.Foreground = Brushes.Black;
                return true;
            }
        }

    }
}
