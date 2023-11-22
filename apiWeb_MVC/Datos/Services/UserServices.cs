using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;


namespace apiWeb_MVC.Services
{
    public class UserServices : IUserServices
    {
        private IDaoBD _daoBD;

        public UserServices(IDaoBD daoBD)
        {
            _daoBD = daoBD;
        }

        public UserOutput GetInformationFromUser(int pId)
        {
            try
            {
                UserOutput user = _daoBD.GetUserByID(pId);
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
                UserInputUpdate user = _daoBD.GetUserByIDU(pId);
                return user;
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public UserInputUpdate GetUserByEmail(string email)
        {
            UserInputUpdate user = _daoBD.GetUserByEmail(email);
            return user;
        }

        public List<UserOutput> GetAllUsers()
        {
            return _daoBD.GetAllUsers();
        }

        public List<UserOutput> GetUsersByIds(List<int> pUserIds)
        {
            List<UserOutput> users = new();
            foreach (int userId in pUserIds)
            {
                UserOutput user = _daoBD.GetUserByID(userId);

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
                UserOutputCreate userOutput = null;
                try
                {
                    userOutput = _daoBD.CreateNewUser(userInput);
                }
                catch (MySqlException ex)
                {
                    return null;
                }

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
                DateTime UpdateDate = DateTime.Now;

                if (currentUser == null)
                {
                    throw new UserNotFoundException($"The user with ID {pId} was not found in the database.");
                }

                bool passwordChanged = !VerifyPassword(pUserUpdate.User_Password, currentUser.User_Password);

                currentUser.User_Name = pUserUpdate.User_Name ?? currentUser.User_Name;
                currentUser.User_LastName = pUserUpdate.User_LastName ?? currentUser.User_LastName;
                currentUser.User_Email = pUserUpdate.User_Email ?? currentUser.User_Email;
                currentUser.User_UpdateDate = UpdateDate;

                if (passwordChanged)
                {
                    string hashedPassword = HashPassword(pUserUpdate.User_Password);
                    currentUser.User_Password = hashedPassword;
                }

                bool updated = _daoBD.UpdateUser(pId, currentUser);

                if (updated)
                {
                    UserOutput user = _daoBD.GetUserByID(pId);
                    UserUpdateDate userWithDate = new UserUpdateDate
                    {
                        User_ID = user.User_ID,
                        User_Name = user.User_Name,
                        User_LastName = user.User_LastName,
                        User_Email = user.User_Email,
                        User_UpdateDate = UpdateDate
                    };

                    return userWithDate;
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

        public UserOutput VerifyUser(string email, string password)
        {
            UserInputUpdate user = _daoBD.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool passwordMatch = VerifyPassword(password, user.User_Password);
            if (passwordMatch)
            {
                UserOutput userOutput = new UserOutput
                {
                    User_ID = user.User_ID,
                    User_Name = user.User_Name,
                    User_LastName = user.User_LastName,
                    User_Email = user.User_Email
                };
                return userOutput;
            }
            else
            {
                return null;
            }
        }

        public bool DisableUser(int pId)
        {
            bool result = _daoBD.DisableUser(pId);

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
                _daoBD.DeletedUser(pId);
            }
            catch (Exception ex)
            {
                throw new UserDeletionFailedException($"Error occurred while deleting the user with ID {pId}.", ex);
            }
        }
    }
}

