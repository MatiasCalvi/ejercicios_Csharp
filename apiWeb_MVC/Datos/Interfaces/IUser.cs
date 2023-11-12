namespace Datos.Interfaces
{
    public interface IUser
    {   
        public int User_ID { get; set; }
        public string User_Name { get; set;}
        public string User_LastName { get; set;}
        public string User_Email { get; set;}
    }
    public interface IUserWithCreationDate : IUser
    {
        DateTime User_CreationDate { get; set; }
    }
}
