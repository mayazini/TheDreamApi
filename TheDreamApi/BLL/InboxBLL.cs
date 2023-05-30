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
    public class InboxBLL
    {
        
        public static string SendMessage(JsonElement json)
        {
            return InboxServiceDAL.SendMessage(json);

        }

        public static DataTable GetSentMessagesByName(string userName)
        {
            return InboxServiceDAL.SentMessagesByName(userName);

        }

        public static DataTable GetUserMessages(string name)
        {
            return InboxServiceDAL.GetUserMessages(name);
        }
        public static DataTable GetMessageById(int messageId)
        {
            return InboxServiceDAL.GetMessageById(messageId);
        }

        public static bool DeleteMessage(int messageId)
        {
            return InboxServiceDAL.DeleteMessage(messageId);
        }

        public static DataTable GetTrashByName(string userName)
        {
            return InboxServiceDAL.GetTrashByName(userName);
        }
        
        public static bool MoveToTrash(int messageId)
        {
            return InboxServiceDAL.UpdateMessageIsTrash(messageId,true);
        }
    }
}
