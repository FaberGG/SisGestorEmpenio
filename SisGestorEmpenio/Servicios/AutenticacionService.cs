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
    public class AutenticacionService
    {
        public AutenticacionService()
        {
            // Constructor vacío
        }
        public static bool validarCredenciales(string usuario, string contrasenia)
        {

            //SOLO EN CASO DE NO PODER ACCEDER A LA BASE DE DATOS   
            //comentar o descomentar
            /*
            if (usuario== "" && contrasenia == "")
            {
                Sesion.Sesion.IniciarSesion(new Administrador("admin", 1, "N/A", 0, 0, "admin", "admin"));
                return true;
            }
            
            //fin seccion comentable
            */
            
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
