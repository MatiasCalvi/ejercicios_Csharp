using Daos;
using Ninject;

namespace UsuariosConexion
{
    class Program
    {
        static void Main()
        {
         
            Ruter ruter = new Ruter();

            IKernel kernel = NinjectConfig.CreateKernel();
            
            IRepositorioUsuarios repositorio = kernel.Get<IRepositorioUsuarios>();
           
            UsuariosManager usuariosManager = new(repositorio);
           
            Console.WriteLine(usuariosManager.ObtenerListadoDeUsuarios());
            
            //Usuario usuario = new Usuario(0,"Lautaro",52);
            //repositorio.CrearUsuarioEnBD(usuario);        

            ruter.ActualizarUsuario(1,"",25);
           
            //repositorio.MostrarInformacionDeUsuario(3); //----> Mostrar usuario por ID
            //repositorio.ActualizarUsuario(10, nombre: "Martin"); //-----> Update

            //repositorio.EliminarUsuario(9); //-----> Delete
            //repositorio.MostrarLista(); //-----> Mostrar lista de Usuarios

            //UsuariosManager usuariosManger = new(repositorio);
            //Console.WriteLine(usuariosManger.ObtenerListadoDeUsuarios());
        }
    }
}

















