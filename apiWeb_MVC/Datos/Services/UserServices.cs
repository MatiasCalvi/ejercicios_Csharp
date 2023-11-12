using System.Security.Cryptography;
using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using System.Text;

namespace apiWeb_MVC.Services
{
    public class UserServices : IUserServices
    {
        private readonly IDaoBD daoBD;

        public UserServices(IDaoBD _daoBD)
        {
            daoBD = _daoBD;
        }

        public UserOutput GetInformationFromUser(int pId)
        {
            UserOutput user = daoBD.GetUserByID(pId);
            return user;
        }

        internal UserInputUpdate GetInformationFromUserU(int pId)
        {
            UserInputUpdate user = daoBD.GetUserByIDU(pId);
            return user;
        }

        public List<UserOutput> GetAllUsers()
        {
            return daoBD.GetAllUsers();
        }

        public List<UserOutput> GetUsersByIds(List<int> userIds)
        {
            List<UserOutput> users = new List<UserOutput>();
            foreach (int userId in userIds)
            {
                UserOutput user = daoBD.GetUserByID(userId);
                
                if (user != null) users.Add(user);
               
                //else throw new NotFoundException($"User with ID {userId} was not found in the database.");
            }
            return users;
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password); //---> convierte la contraseña en un array de bytes

                byte[] hashBytes = sha256.ComputeHash(passwordBytes); //---> sirve para calcular el hash de la contraseña.

                StringBuilder hashBuilder = new StringBuilder(); //---> convierte la cadena a hexadecimal
                
                foreach (byte b in hashBytes)
                {
                    hashBuilder.Append(b.ToString("x2"));
                }

                return hashBuilder.ToString();
            }
        }

        public bool VerifyPassword(string userInput, string hashedPassword)
        {
            string salt = hashedPassword.Substring(hashedPassword.Length - 44);
            string hash = hashedPassword.Substring(0, hashedPassword.Length - 44); 
            string hashedInput = HashPassword(userInput + salt); 
            return hash == hashedInput;
        }

        public UserOutputCreate CreateNewUser(UserInput userInput)
        {
            var salt = new byte[32];
            RandomNumberGenerator.Fill(salt); // ---> llena el array con bytes aleatorios
            string saltString = Convert.ToBase64String(salt);
            string hashedPassword = HashPassword(userInput.User_Password + saltString); 
            userInput.User_Password = hashedPassword + saltString;
            userInput.User_CreationDate = DateTime.Now;
            UserOutputCreate userOutput = daoBD.CreateNewUser(userInput); 

            return userOutput;
        }

        public UserOutput UpdateUser(int id, UserInputUpdate userUpdate)
        {
            UserInputUpdate currentUser = GetInformationFromUserU(id);

            if (currentUser == null) throw new NotFoundException($"The user with ID {id} was not found in the database.");

            bool passwordChanged = !VerifyPassword(userUpdate.User_Password, currentUser.User_Password);

            currentUser.User_Name = userUpdate.User_Name ?? currentUser.User_Name;
            currentUser.User_LastName = userUpdate.User_LastName ?? currentUser.User_LastName;
            currentUser.User_Email = userUpdate.User_Email ?? currentUser.User_Email;

            if (passwordChanged)
            {
                var salt = new byte[32];
                RandomNumberGenerator.Fill(salt);
                string saltString = Convert.ToBase64String(salt);
                string hashedPassword = HashPassword(userUpdate.User_Password + saltString);
                currentUser.User_Password = hashedPassword + saltString;
            }

            bool updated = daoBD.UpdateUser(id, currentUser);

            if (updated)
            {
                UserOutput user = daoBD.GetUserByID(id);
                return user;
            }
            else
            {
                return null;
            }
        }
        public bool DisableUser(int pId) 
        {  
            return daoBD.DisableUser(pId);
        }
        public void DeletedUser(int pId)
        {
            daoBD.DeletedUser(pId);
        }

    }
}
