using SisGestorEmpenio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisGestorEmpenio.Sesion
{
    internal static class Sesion
    {
        public static Administrador AdministradorActivo { get; private set; }

        public static void IniciarSesion(Administrador admin)
        {
            AdministradorActivo = admin;
        }

        public static void CerrarSesion()
        {
            AdministradorActivo = null;
        }

        public static bool SesionIniciada => AdministradorActivo != null;
    }
}
