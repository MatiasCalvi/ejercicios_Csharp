using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using MySql.Data.MySqlClient;


namespace apiWeb_MVC.Services
{
    public class UserServices : IUserServices
    {
        private IDaoBD _daoBD;
        private IValidateMethodes _validateMethodes;

        public UserServices(IDaoBD daoBD, IValidateMethodes validateMethodes)
        {
            _daoBD = daoBD;
            _validateMethodes = validateMethodes;
        }
        public List<UserOutput> GetAllUsers()
        {
            return _daoBD.GetAllUsers();
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
                throw new NotFoundException($"User with ID {pId} was not found in the database.", ex);
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
                throw new NotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public UserInputUpdate GetUserByEmail(string pEmail)
        {
            UserInputUpdate user = _daoBD.GetUserByEmail(pEmail);
            return user;
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
                    throw new NotFoundException($"User with ID {userId} was not found in the database.");
                }
            }
            return users;
        }

        public UserOutputCreate CreateNewUser(UserInput pUserInput)
        {
            try
            {
                string hashedPassword = _validateMethodes.HashPassword(pUserInput.User_Password);
                pUserInput.User_Password = hashedPassword;
                pUserInput.User_CreationDate = DateTime.Now;
                UserOutputCreate userOutput = null;
                try
                {
                    userOutput = _daoBD.CreateNewUser(pUserInput);
                }
                catch (MySqlException ex)
                {
                    return null;
                }

                if (userOutput == null)
                {
                    throw new CreationFailedException("Failed to create a new user.");
                }

                return userOutput;
            }
            catch (Exception ex)
            {
                throw new CreationFailedException("Error occurred while creating a new user.", ex);
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
                    throw new NotFoundException($"The user with ID {pId} was not found in the database.");
                }

                bool passwordChanged = !_validateMethodes.VerifyPassword(pUserUpdate.User_Password, currentUser.User_Password);

                currentUser.User_Name = pUserUpdate.User_Name ?? currentUser.User_Name;
                currentUser.User_LastName = pUserUpdate.User_LastName ?? currentUser.User_LastName;
                currentUser.User_Email = pUserUpdate.User_Email ?? currentUser.User_Email;
                currentUser.User_UpdateDate = UpdateDate;

                if (passwordChanged)
                {
                    string hashedPassword = _validateMethodes.HashPassword(pUserUpdate.User_Password);
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
                    throw new UpdateFailedException($"Failed to update the user with ID {pId}.");
                }
            }
            catch (Exception ex)
            {
                throw new UpdateFailedException($"Error occurred while updating the user with ID {pId}.", ex);
            }
        }

        public UserOutput VerifyUser(string pEmail, string pPassword)
        {
            UserInputUpdate user = _daoBD.GetUserByEmail(pEmail);
            if (user == null)
            {
                return null;
            }

            bool passwordMatch = _validateMethodes.VerifyPassword(pPassword, user.User_Password);
            if (passwordMatch)
            {
                UserOutput userOutput = new UserOutput
                {
                    User_ID = user.User_ID,
                    User_Name = user.User_Name,
                    User_LastName = user.User_LastName,
                    User_Email = user.User_Email,
                    User_Role = user.User_Role
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
                throw new DeletionFailedException($"Failed to disable the user with ID {pId}.");
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
                throw new DeletionFailedException($"Error occurred while deleting the user with ID {pId}.", ex);
            }
        }
    }
}
