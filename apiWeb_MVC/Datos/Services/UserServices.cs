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
        public async Task<List<UserOutput>> GetAllUsersAsync()
        {
            return await _daoBD.GetAllUsersAsync();
        }

        public async Task<UserOutput> GetInformationFromUserAsync(int pId)
        {
            try
            {
                return await _daoBD.GetUserByIDAsync(pId);
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public async Task<UserInputUpdate> GetInformationFromUserUAsync(int pId)
        {
            try
            {
                return await _daoBD.GetUserByIDUAsync(pId);
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public async Task<UserInputUpdate> GetUserByEmailAsync(string pEmail)
        {
            UserInputUpdate user = await _daoBD.GetUserByEmailAsync(pEmail);
            return user;
        }

        public async Task<List<UserOutput>> GetUsersByIdsAsync(List<int> pUserIds)
        {
            List<UserOutput> users = new List<UserOutput>();

            foreach (int userId in pUserIds)
            {
                UserOutput user = await _daoBD.GetUserByIDAsync(userId);

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

        public async Task<UserOutputCreate> CreateNewUserAsync(UserInput pUserInput)
        {
            try
            {
                string hashedPassword = _validateMethodes.HashPassword(pUserInput.User_Password);
                pUserInput.User_Password = hashedPassword;
                pUserInput.User_CreationDate = DateTime.Now;
                UserOutputCreate userOutput = null;
                try
                {
                    userOutput = await _daoBD.CreateNewUserAsync(pUserInput);
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

        public async Task<UserOutput> UpdateUserAsync(int pId, UserInputUpdate pUserUpdate)
        {
            try
            {
                UserInputUpdate currentUser = await GetInformationFromUserUAsync(pId);
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

                bool updated = await _daoBD.UpdateUserAsync(pId, currentUser);

                if (updated)
                {
                    UserOutput user = await _daoBD.GetUserByIDAsync(pId);
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

        public async Task<UserOutput> VerifyUserAsync(string pEmail, string pPassword)
        {
            UserInputUpdate user = await _daoBD.GetUserByEmailAsync(pEmail);
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

        public async Task<bool> DisableUserAsync(int pId)
        {
            bool result = await _daoBD.DisableUserAsync(pId);

            if (!result)
            {
                throw new DeletionFailedException($"Failed to disable the user with ID {pId}.");
            }

            return result;
        }

        public async Task DeletedUserAsync(int pId)
        {
            try
            {
                await _daoBD.DeletedUserAsync(pId);
            }
            catch (Exception ex)
            {
                throw new DeletionFailedException($"Error occurred while deleting the user with ID {pId}.", ex);
            }
        }
    }
}

