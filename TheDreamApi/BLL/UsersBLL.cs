using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using TheDreamApi.DAL;
using TheDreamApi.Models;

namespace TheDreamApi.BLL
{
    public class UsersBLL
    {

        public static DataTable GetUserDataBLL(string username, string password)
        {
            DataTable data = UserServiceDAL.GetUserDataDAL(username, password);

            if (data == null || data.Rows.Count == 0)// check if login incorrect
            {
                return null;
            }
            return data;
        }

        public static string Register(JsonElement json)
        {
            return UserServiceDAL.Register(json);

        }

    }
}