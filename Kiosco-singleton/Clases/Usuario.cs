using Clases.Interfaces;

namespace Clases
{
    public class Usuario : IUsuario
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public Usuario(string pNombre, int pEdad)
        {
            Nombre = pNombre;
            Edad = pEdad;
        }
    }
}
