using SisGestorEmpenio.Modelos;
using SisGestorEmpenio.Sesion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisGestorEmpenio.repository;
namespace SisGestorEmpenio.Servicios
{
    internal class AutenticacionService
    {
        public AutenticacionService()
        {
            // Constructor vacío
        }
        public static bool ValidarCredenciales(string usuario, string contrasenia)
        {
            AdministradorRepository adminRepo = new AdministradorRepository();
            Administrador admin = adminRepo.buscarPorCredenciales(usuario, contrasenia);
            if (admin != null)
            {
                Sesion.Sesion.IniciarSesion(admin);
                return true;
            }

            return false;
        }
    }
}
