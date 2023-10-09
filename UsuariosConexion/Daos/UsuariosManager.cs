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

        public void MostrarLista()
        {
            var usuarios = repositorioUsuarios.ListarTodosLosUsuarios();
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"ID: {usuario.Usuario_Id}, Nombre: {usuario.Usuario_Nombre}, Edad: {usuario.Usuario_Edad}");
            }
        }

    }
}
