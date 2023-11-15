using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using System.Text;
using System.Security.Cryptography;


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
            try
            {
                UserOutput user = daoBD.GetUserByID(pId);
                return user;
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public UserInputUpdate GetInformationFromUserU(int pId)
        {
            try
            {
                UserInputUpdate user = daoBD.GetUserByIDU(pId);
                return user;
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public List<UserOutput> GetAllUsers()
        {
            return daoBD.GetAllUsers();
        }

        public List<UserOutput> GetUsersByIds(List<int> pUserIds)
        {
            List<UserOutput> users = new();
            foreach (int userId in pUserIds)
            {
                UserOutput user = daoBD.GetUserByID(userId);

                if (user != null)
                {
                    users.Add(user);
                }
                else
                {
                    throw new UserNotFoundException($"User with ID {userId} was not found in the database.");
                }
            }
            return users;
        }

        internal string HashPassword(string pPassword)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pPassword.Normalize(NormalizationForm.FormKD));
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return BCrypt.Net.BCrypt.HashPassword(hashedPassword, 4); 
        }

        public bool VerifyPassword(string pUserInput, string pHashedPassword)
        {

            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pUserInput.Normalize(NormalizationForm.FormKD));
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return BCrypt.Net.BCrypt.Verify(hashedPassword, pHashedPassword);
        }

        public UserOutputCreate CreateNewUser(UserInput userInput)
        {
            try
            {
                string hashedPassword = HashPassword(userInput.User_Password);
                userInput.User_Password = hashedPassword;
                userInput.User_CreationDate = DateTime.Now;
                UserOutputCreate userOutput = daoBD.CreateNewUser(userInput);

                if (userOutput == null)
                {
                    throw new UserCreationFailedException("Failed to create a new user.");
                }

                return userOutput;
            }
            catch (Exception ex)
            {
                throw new UserCreationFailedException("Error occurred while creating a new user.", ex);
            }
        }

        public UserOutput UpdateUser(int pId, UserInputUpdate pUserUpdate)
        {
            try
            {
                UserInputUpdate currentUser = GetInformationFromUserU(pId);

                if (currentUser == null)
                {
                    throw new UserNotFoundException($"The user with ID {pId} was not found in the database.");
                }

                bool passwordChanged = !VerifyPassword(pUserUpdate.User_Password, currentUser.User_Password);

                currentUser.User_Name = pUserUpdate.User_Name ?? currentUser.User_Name;
                currentUser.User_LastName = pUserUpdate.User_LastName ?? currentUser.User_LastName;
                currentUser.User_Email = pUserUpdate.User_Email ?? currentUser.User_Email;

                if (passwordChanged)
                {
                    string hashedPassword = HashPassword(pUserUpdate.User_Password);
                    currentUser.User_Password = hashedPassword;
                }

                bool updated = daoBD.UpdateUser(pId, currentUser);

                if (updated)
                {
                    UserOutput user = daoBD.GetUserByID(pId);
                    return user;
                }
                else
                {
                    throw new UserUpdateFailedException($"Failed to update the user with ID {pId}.");
                }
            }
            catch (Exception ex)
            {
                throw new UserUpdateFailedException($"Error occurred while updating the user with ID {pId}.", ex);
            }
        }

        public bool DisableUser(int pId)
        {
            bool result = daoBD.DisableUser(pId);

            if (!result)
            {
                throw new UserDeletionFailedException($"Failed to disable the user with ID {pId}.");
            }

            return result;
        }

        public void DeletedUser(int pId)
        {
            try
            {
                daoBD.DeletedUser(pId);
            }
            catch (Exception ex)
            {
                throw new UserDeletionFailedException($"Error occurred while deleting the user with ID {pId}.", ex);
            }
        }
    }
}
