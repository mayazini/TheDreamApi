using Newtonsoft.Json.Linq;
using TheDreamApi.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;


namespace TheDreamApi.DAL
{
    public class UserServiceDAL
    {
        public static DataTable GetUserDataDAL(string username, string password)
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
                //string email = (string)obj["email"];
                int affected = 0;
                string checkQuery = $"select username from Users where username='{username}'";
                DataTable check = SQLHelper.SelectData(checkQuery);
                if (check.Rows.Count == 0)
                {

                    string query = $"INSERT INTO Users (email,username, password) VALUES ('{email}','{username}',' {password}')";
                    affected = SQLHelper.DoQuery(query);

                }
                else
                {
                    return ("username already taken");
                }
                if (affected > 0)
                {
                    return ("ok");
                }
                else
                {
                    return ("error in the query");
                }
            }
            catch (Exception ex)
            {
                return ("unkown error");
            }
        }
    }
}