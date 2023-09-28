namespace Daos
{
    public class Usuario
    {
        public int Usuario_Id { get; set; }
        public string Usuario_Nombre { get; set; } = string.Empty; // ---> se estable como valor predeterminado una cadena vacia
        public int Usuario_Edad { get; set; }
    }
}
