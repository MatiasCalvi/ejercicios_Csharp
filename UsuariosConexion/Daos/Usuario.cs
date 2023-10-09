using System.Security.Cryptography.X509Certificates;

namespace Daos
{
    public class Usuario
    {
        public int Usuario_Id { get; set; }
        public string Usuario_Nombre { get; set; } = string.Empty; // ---> se estable como valor predeterminado una cadena vacia
        public int Usuario_Edad { get; set; }

        public Usuario(){
            
        }

        public Usuario(int pId, string pNombre,int pEdad)
        {
            this.Usuario_Id = pId;
            this.Usuario_Nombre = pNombre;
            this.Usuario_Edad = pEdad;
        }
    }
}
