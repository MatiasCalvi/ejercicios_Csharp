using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daos
{
    public interface IRepositorioUsuarios
    {
        public List<Usuario> ListarTodosLosUsuarios();
        public void MostrarLista();
        public void MostrarInformacionDeUsuario(int id);
        public void CrearUsuario(string nombre, int edad);
        public void ActualizarUsuario(int id, string? nombre = null, int? edad = null);
        public void EliminarUsuario(int id);
    }
}
