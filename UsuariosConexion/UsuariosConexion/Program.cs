using Daos;

namespace UsuariosConexion
{
    class Program
    {
        static void Main()
        {
            RepositorioUsuarios repositorio = new();

            //repositorio.CrearUsuario("Ximena", 28); //-----> Create
            repositorio.MostrarInformacionDeUsuario(3); //----> Mostrar usuario por ID
            repositorio.ActualizarUsuario(10, nombre: "Martin", edad: 23); //-----> Update
            repositorio.EliminarUsuario(9); //-----> Delete
            repositorio.MostrarLista(); //-----> Mostrar lista de Usuarios 
        }
    }
}

















