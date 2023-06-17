using System;
using System.Collections.Generic;
using System.Data;
using TheDreamApi.Models;
using System.Linq;
using System.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using TheDreamApi.DAL;
using ServiceReference1;
using TheDreamApi.Models;

namespace TheDreamApi.BLL
{
    public class UsersServiceBLL
    {

        public static (User, string) Login(string username, string inputPassword)
        {
            try
            {
                User user = UserServiceDAL.GetUserDataByNameDAL(username);

                string hashedInputPassword = HashingFunction.HashPasswordWithSalt(inputPassword, user.HashSalt);

                if (hashedInputPassword == user.Password)
                {
                    return (user, "successfull login");
                }
                else
                {
                    return (null, "Incorrect password");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "User not found")
                {
                    return (null, "User does not exist");
                }
                else
                {
                    // Unexpected error, rethrow the exception
                    throw;
                }
            }
        }


        public static string Register(User newUser)
        {
            var client = new ServiceReference1.ValidationSoapClient(ServiceReference1.ValidationSoapClient.EndpointConfiguration.ValidationSoap); // Instantiate the generated client proxy

            var response = client.isEmailAsync(newUser.Email).GetAwaiter().GetResult();

            bool isEmailValid = response.Body.isEmailResult;
            if (isEmailValid)
            {
                string salt = HashingFunction.GenerateSalt();
                newUser.HashSalt = salt;
                string passwordHash = HashingFunction.HashPasswordWithSalt(newUser.Password, salt);
                newUser.Password = passwordHash;
                return UserServiceDAL.Register(newUser);
            }
            return "invalid email";

        }
        public static List<User> GetAllUsers()
        {
            try
            {
                List<User> users = UserServiceDAL.GetAllUsers();
                return users;
            }
            catch(Exception ex) { throw new Exception(ex.Message); }

        }

        public static bool DeleteUser(string userName)
        {
            try
            {
                return UserServiceDAL.DeleteUser(userName);
            }
            catch(Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

    }
}