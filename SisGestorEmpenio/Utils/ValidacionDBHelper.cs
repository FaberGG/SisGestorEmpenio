using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using SisGestorEmpenio.repository;

namespace SisGestorEmpenio.Utils
{
    public static class ValidacionDBHelper
    {

        public static bool EstaClienteRegistrado(int id)
        {
            try
            {
                ClienteRepository clienteRepository = new ClienteRepository();
                return clienteRepository.EstaGuardado(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EstaArticuloRegistrado(int id)
        {
            try
            {
                ArticuloRepository articuloRepository = new ArticuloRepository();
                return articuloRepository.EstaGuardado(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EstaPrestamoRegistrado(int clienteId, int articuloId)
        {
            try
            {
                PrestamoRepository prestamoRepository = new PrestamoRepository();
                return prestamoRepository.EstaGuardado(clienteId, articuloId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EstaDevolucionRegistrada(int clienteId, int articuloId)
        {
            try
            {
                DevolucionRepository devolucionRepository = new DevolucionRepository();
                return devolucionRepository.EstaGuardado(clienteId, articuloId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
