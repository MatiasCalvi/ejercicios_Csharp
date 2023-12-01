namespace Datos.Interfaces
{
    public interface IValidateMethodes
    {
        public int GetUserIdFromToken();
        public bool VerifyPassword(string pUserInput, string pHashedPassword);
        public string HashPassword(string pPassword);
    }
}
