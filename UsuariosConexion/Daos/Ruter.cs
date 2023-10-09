using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daos;

namespace Daos
{
    public class Ruter
    {
        IRepositorioUsuarios repositorio = new RepositorioUsuarios();
        public void ActualizarUsuario(int id, string? nombre = null, int? edad = null)
        {
            if (repositorio.ObtenerInformacionDeUnUsuario(id) is Usuario usuarioActualizado)
            {
                if (nombre != null || nombre != "")
                {
                    usuarioActualizado.Usuario_Nombre = nombre;
                }

                if (edad != null || edad < 0 )
                {
                    usuarioActualizado.Usuario_Edad = edad ?? 0;
                }
                repositorio.ActualizarUsuarioEnBD(usuarioActualizado); //-----> Update
            }
            else
            {
                Console.WriteLine("No se encontró el usuario.");
            }
        }
    }
}
