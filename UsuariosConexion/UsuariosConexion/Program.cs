using System.Data;
using Daos;
class Program
{
    static void Main(string[] args)
    {
        RepositorioUsuarios repositorio = new RepositorioUsuarios();

        
        var usuarios = repositorio.ListarTodosLosUsuarios();
        foreach (var usuario in usuarios) //---------> Listar los usuarios  
        {
            Console.WriteLine($"ID: {usuario.Usuario_Id}, Nombre: {usuario.Usuario_Nombre}, Edad: {usuario.Usuario_Edad}");
        }

        var usuarioInfo = repositorio.ObtenerInformacionDeUnUsuario(3); //--------> Listar usuario por Id
        Console.WriteLine($"ID: {usuarioInfo.Usuario_Id}, Nombre: {usuarioInfo.Usuario_Nombre}, Edad: {usuarioInfo.Usuario_Edad}");
    }
}

















