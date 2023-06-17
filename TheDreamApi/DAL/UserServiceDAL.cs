using Newtonsoft.Json.Linq;
using TheDreamApi.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using TheDreamApi.Models;
using Microsoft.AspNetCore.Http;


namespace TheDreamApi.DAL
{
    public class UserServiceDAL
    {
        //helper function to input as user object fron a Users row
        public static User BuildUser(DataRow row)
        {
            User user = new User();
            user.Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0;
            user.FirstName = row["FirstName"] != DBNull.Value ? row["FirstName"].ToString() : null;
            user.LastName = row["LastName"] != DBNull.Value ? row["LastName"].ToString() : null;
            user.Age = row["Age"] != DBNull.Value ? Convert.ToInt32(row["Age"]) : 0;
            user.LinkedInLink = row["LinkedInLink"] != DBNull.Value ? row["LinkedInLink"].ToString() : null;
            user.IsAdmin = row["IsAdmin"] != DBNull.Value ? Convert.ToBoolean(row["IsAdmin"]) : false;
            user.UserName = row["UserName"] != DBNull.Value ? row["UserName"].ToString() : null;
            user.Password = row["Password"] != DBNull.Value ? row["Password"].ToString() : null;
            user.Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : null;
            user.HashSalt = row["HashSalt"] != DBNull.Value ? row["HashSalt"].ToString() : null;
            return user;
        }

        public static User GetUserDataByNameDAL(string username)
        {
            try
            {
                string query = "select * from Users Where UserName = N'" + username + "'";
                DataTable result = SQLHelper.SelectData(query);
                User user = new User();
                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    return BuildUser(row);
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public static string Register(User user)
        {
            try
            {
                int affected = 0;
                string checkQuery = $"select username from Users where Username='{user.UserName}'";
                DataTable check = SQLHelper.SelectData(checkQuery);
                if (check.Rows.Count == 0)
                {

                    string query = $"exec spInsertUserData @UserName='{user.UserName}',@Password='{user.Password}',@Email='{user.Email}',@Age='{user.Age}',@IsAdmin='{user.IsAdmin}',@LinkedInLink='{user.LinkedInLink}',@FirstName='{user.FirstName}',@LastName='{user.LastName}',@HashSalt='{user.HashSalt}'";
                    affected = SQLHelper.DoQuery(query);

                    if (affected > 0)
                    {
                        // Insert the event into the Events table
                        string eventQuery = $"INSERT INTO Events (EventType, Time, username) VALUES ('{EventTypes.createNewUser}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{user.UserName}')";
                        affected = SQLHelper.DoQuery(eventQuery);

                        return affected > 0 ? "ok" : "error in the query";
                    }
                    else
                    {
                        return "error in the query";
                    }

                }
                else
                {
                    return ("username already taken");
                }
            }
            catch (Exception ex)
            {
                return ("unkown error");
            }
        }
        public static List<User> GetAllUsers()
        {
            try
            {
                string query = "select * from Users";
                DataTable results = SQLHelper.SelectData(query);
                List<User> userList = new List<User>();

                foreach (DataRow row in results.Rows)
                {
                    User user = BuildUser(row);
                    userList.Add(user);
                }

                if (userList != null && userList.Count > 0)
                {
                    return userList;
                }
                else
                {
                    throw new Exception("No users found");
                }
            }
            catch(Exception ex) { throw new Exception("Unknown error"); }

        }

        public static bool DeleteUser(string userName)
        {
            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", userName);
                bool deletedFolder = false;

                if (Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                        deletedFolder = !Directory.Exists(folderPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred while deleting the folder: {ex.Message}");
                    }
                }
                else
                {
                    deletedFolder = true; // Assume success if the folder doesn't exist
                }

                if (deletedFolder)
                {
                    string query = "exec spDeleteUser " + userName;
                    int response = SQLHelper.DoQuery(query);

                    if (response > 0)
                    {
                        return true; // User deleted successfully
                    }
                }

                throw new Exception("Failed to delete user from the database.User file was deleted");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unknown error: {ex.Message}");
            }
        }

    }
}