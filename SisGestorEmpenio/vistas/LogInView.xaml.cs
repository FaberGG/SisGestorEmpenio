using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.Servicios;
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
    /// Lógica de interacción para LogInView.xaml
    /// </summary>
    public partial class LogInView : UserControl
    {

        public event EventHandler LoginSuccess;

        public LogInView()
        {
            InitializeComponent();
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Password;


            //uso del metodo estatico de validacion de credenciales
            try
            {

                //esto solo en caso de no poderse conectar a la base de datos
                //para pruebas unitarias
                
                bool credencialesValidas = true;
                if (!(usuario == "" && contrasena == ""))
                {
                    credencialesValidas =    AutenticacionService.validarCredenciales(usuario, contrasena);

                }
                
                //bool credencialesValidas = AutenticacionService.validarCredenciales(usuario, contrasena);



                if (credencialesValidas)
                {
                    LoginSuccess?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (OracleException ex)
            {

                MostrarError("Error de base de datos:\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error inesperado:\n" + ex.Message);
            }

           
        }
        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
