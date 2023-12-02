namespace Datos.Schemas
{
    public class RefreshTokenRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public string UserRole {  get; set; }
    }
}
