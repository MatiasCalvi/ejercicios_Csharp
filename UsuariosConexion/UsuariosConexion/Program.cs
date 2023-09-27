using System.Data;
using Daos;

namespace UsuariosConexion
{
    class Program
    {
        public static Usuario CrearInstanciaUsuario(string nombre, int edad)
        {
            return new Usuario
            {   
                Usuario_Nombre = nombre,
                Usuario_Edad = edad,
            };
        }


        static void Main()
        {
            RepositorioUsuarios repositorio = new RepositorioUsuarios();

            Usuario usuarioNuevo = CrearInstanciaUsuario("Xelena",22);      
            // repositorio.CrearUsuario(usuarioNuevo);//-----> Create


            var usuarios = repositorio.ListarTodosLosUsuarios();
            foreach (var usuario in usuarios) //---------> Listar los usuarios  
            {
                Console.WriteLine($"ID: {usuario.Usuario_Id}, Nombre: {usuario.Usuario_Nombre}, Edad: {usuario.Usuario_Edad}");
            }


            var usuarioInfo = repositorio.ObtenerInformacionDeUnUsuario(3); //--------> Listar usuario por Id
            if (usuarioInfo != null) // ---> Verificando nulidad
            {
                Console.WriteLine($"ID: {usuarioInfo.Usuario_Id}, Nombre: {usuarioInfo.Usuario_Nombre}, Edad: {usuarioInfo.Usuario_Edad}");
            }
            else
            {
                Console.WriteLine("No se encontró el usuario."); 
            }


            var usuarioActualizado = repositorio.ObtenerInformacionDeUnUsuario(2);
            if (usuarioActualizado != null)
            {
                usuarioActualizado.Usuario_Nombre = "Lorena";
                repositorio.ActualizarUsuario(usuarioActualizado); //-----> Update
            }
            else
            {
                Console.WriteLine("No se encontró el usuario.");
            }

            var usuarioEliminado = repositorio.ObtenerInformacionDeUnUsuario(5);
            if (usuarioEliminado != null)
            {
                repositorio.EliminarUsuario(usuarioEliminado.Usuario_Id); //-----> Delete
            }
            else
            {
                Console.WriteLine("No se encontró el usuario.");
            }
        }
    }
}

















