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


namespace TheDreamApi.DAL
{
    public class UserServiceDAL
    {
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

                    string query = $"INSERT INTO Users (Email, Username, Password,IsAdmin,HashSalt) VALUES ('{user.Email}', '{user.UserName}', '{user.Password}','false','{user.HashSalt}')";
                    affected = SQLHelper.DoQuery(query);

                    if (affected > 0)
                    {
                        // Insert the event into the Events table
                        string eventQuery = $"INSERT INTO Events (EventType, Time, username) VALUES ('{EventTypes.createNewUser}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{user.UserName}')";
                        affected = SQLHelper.DoQuery(eventQuery);

                        return affected>0 ?"ok" : "error in the query";
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
        public static DataTable GetAllUsers()
        {
            string query = "select * from Users";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }
        
    }
}