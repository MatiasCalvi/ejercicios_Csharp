using Daos;
using Ninject;

namespace UsuariosConexion
{
    class Program
    {
        static void Main()
        {
            //IRepositorioUsuarios repositorio = new RepositorioUsuarios();

            //repositorio.CrearUsuario("Ximena", 28); //-----> Create
            //repositorio.MostrarInformacionDeUsuario(3); //----> Mostrar usuario por ID
            //repositorio.ActualizarUsuario(10, nombre: "Martin", edad: 23); //-----> Update
            //repositorio.EliminarUsuario(9); //-----> Delete
            //repositorio.MostrarLista(); //-----> Mostrar lista de Usuarios

            //UsuariosManager usuariosManger = new(repositorio);
            //Console.WriteLine(usuariosManger.ObtenerListadoDeUsuarios());

           
            IKernel kernel = NinjectConfig.CreateKernel();

            
            IRepositorioUsuarios repositorio = kernel.Get<IRepositorioUsuarios>();

           
            UsuariosManager usuariosManager = new UsuariosManager(repositorio);

           
            Console.WriteLine(usuariosManager.ObtenerListadoDeUsuarios());
        }
    }
}

















