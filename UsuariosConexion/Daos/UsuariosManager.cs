using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daos
{
    public class UsuariosManager
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public UsuariosManager(IRepositorioUsuarios _repositorioUsuarios)
        {
            repositorioUsuarios = _repositorioUsuarios;
        }

        public string ObtenerListadoDeUsuarios()
        {
            List<Usuario>listaUsuarios = repositorioUsuarios.ListarTodosLosUsuarios();
            List<Usuario> usuariosOrdenadosXEdad = listaUsuarios.OrderBy(user => user.Usuario_Edad).ToList();
            string usuariosSeparadosPorComa = string.Join(", ", usuariosOrdenadosXEdad.Select(user=>user.Usuario_Nombre)); 
            return usuariosSeparadosPorComa;
        }
    }
}
