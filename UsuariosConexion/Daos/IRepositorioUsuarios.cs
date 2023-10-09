namespace Daos
{
    public interface IRepositorioUsuarios
    {
        public List<Usuario> ListarTodosLosUsuarios();
        public void MostrarLista();
        public void MostrarInformacionDeUsuario(int id);

        public Usuario ObtenerInformacionDeUnUsuario(int id);
        public void CrearUsuarioEnBD(Usuario usuario);
        public void ActualizarUsuarioEnBD(Usuario usuario);
        public void EliminarUsuario(int id);
    }
}
