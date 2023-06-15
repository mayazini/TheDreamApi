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
        public static DataTable GetUserDataByNameDAL(string username, string password)
        {
            string query = "select * from Users Where userName = N'"+ username + "' AND password=N'"+ password + "'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static string Register(JsonElement json)
        {
            try
            {
                dynamic obj = JsonNode.Parse(json.GetRawText());
                string username = (string)obj["username"];
                string password = (string)obj["password"];
                string email = (string)obj["email"];

                int affected = 0;
                string checkQuery = $"select username from Users where username='{username}'";
                DataTable check = SQLHelper.SelectData(checkQuery);
                if (check.Rows.Count == 0)
                {

                    string query = $"INSERT INTO Users (email, username, password,IsAdmin) VALUES ('{email}', '{username}', '{password}','false')";
                    affected = SQLHelper.DoQuery(query);

                    if (affected > 0)
                    {
                        // Insert the event into the Events table
                        string eventQuery = $"INSERT INTO Events (EventType, Time, username) VALUES ('{EventTypes.createNewUser}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{username}')";
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