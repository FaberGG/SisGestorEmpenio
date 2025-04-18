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
        // *** Preview: solo dígitos y un solo punto ***
        public static bool EsDecimal(string input, string actual)
        {
            if (!char.IsDigit(input, 0) && input != ".")
                return false;
            if (input == "." && actual.Contains("."))
                return false;
            return true;
        }

        public static bool ValidarCampo(Control campo, TextBlock etiquetaError, string nombreCampo)
        {
            string texto = campo is TextBox tb ? tb.Text :
                           campo is ComboBox cb ? cb.Text : "";
            if (string.IsNullOrWhiteSpace(texto))
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} es obligatorio.");
                return false;
            }
            LimpiarError(etiquetaError);
            return true;
        }

        public static bool ValidarEntero(TextBox textBox, TextBlock etiquetaError, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} es obligatorio.");
                return false;
            }
            if (!int.TryParse(textBox.Text, out int v))
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} debe ser un número entero.");
                return false;
            }
            if (v <= 0)
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} debe ser mayor que cero.");
                return false;
            }
            LimpiarError(etiquetaError);
            return true;
        }

        public static bool ValidarDecimal(TextBox textBox, TextBlock etiquetaError, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} es obligatorio.");
                return false;
            }
            if (!double.TryParse(textBox.Text, out double v))
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} debe ser un número válido.");
                return false;
            }
            if (v <= 0)
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} debe ser mayor que cero.");
                return false;
            }
            LimpiarError(etiquetaError);
            return true;
        }

        public static bool ValidarLongitud(TextBox textBox, TextBlock etiquetaError, string nombreCampo, int min, int max)
        {
            string texto = textBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} es obligatorio.");
                return false;
            }
            if (texto.Length < min || texto.Length > max)
            {
                MostrarError(etiquetaError, $"El campo {nombreCampo} debe tener entre {min} y {max} caracteres.");
                return false;
            }
            LimpiarError(etiquetaError);
            return true;
        }

        private static void MostrarError(TextBlock etiqueta, string mensaje)
        {
            etiqueta.Text = mensaje;
            etiqueta.Foreground = new SolidColorBrush(Colors.Red);
        }

        private static void LimpiarError(TextBlock etiqueta)
        {
            etiqueta.Text = "";
        }
    }
}

