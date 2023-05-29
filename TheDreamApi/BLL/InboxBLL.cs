﻿using System;
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
    public class InboxBLL
    {
        public static string SendMessage(JsonElement json)
        {
            return InboxServiceDAL.SendMessage(json);

        }

        public static DataTable GetUserMessages(string name)
        {
            return InboxServiceDAL.GetUserMessages(name);
        }
    }
}
