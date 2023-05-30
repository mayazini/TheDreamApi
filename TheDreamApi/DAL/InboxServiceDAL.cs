using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Xml.Linq;
using TheDreamApi.Models;

namespace TheDreamApi.DAL
{
    public class InboxServiceDAL
    {
        public static string SendMessage(JsonElement json)
        {
            try
            {
                dynamic obj = JsonNode.Parse(json.GetRawText());
                string message = (string)obj["message"];//might not work
                string subject = (string)obj["subject"];
                string senderName = (string)obj["senderName"];
                string recieverName = (string)obj["recieverName"];

                int affected = 0;
                string checkQuery = $"select username from Users where username=N'{recieverName}'";
                DataTable check = SQLHelper.SelectData(checkQuery);
                if (check.Rows.Count != 0)
                {

                    string query = $"INSERT INTO Inbox (Message,SenderName, RecieverName,Time,Subject,IsTrash) VALUES ('{message}','{senderName}','{recieverName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',' {subject}','false')";
                    affected = SQLHelper.DoQuery(query);

                }
                else
                {
                    return ("user doesn't exist");
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

        public static DataTable GetUserMessages(string name)
        {
            string query = $"select * from Inbox where recieverName=N'{name}'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static DataTable GetMessageById(int messageId)
        {
            string query = $"select * from Inbox where id=N'{messageId}'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static DataTable SentMessagesByName(string userName)
        {
            string query = $"select * from Inbox where SenderName=N'{userName}' And IsTrash='false'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static DataTable GetTrashByName(string userName)
        {
            string query = $"select * from Inbox where SenderName=N'{userName}' And IsTrash='true'";
            DataTable result = SQLHelper.SelectData(query);
            return result;
        }

        public static bool DeleteMessage(int messageId)
        {
            // Retrieve the message from the database based on the messageId
            Inbox message = GetInboxMessageById(messageId);

            if (message != null)
            {
                if (message.IsTrash)
                {
                    // If the message is marked as trash, delete it from the database
                    return DeleteMessageFromDatabase(messageId);
                }
                else
                {
                    // If the message is not marked as trash, update the IsTrash field to true
                    return UpdateMessageIsTrash(messageId, true);
                }
            }

            return false; // Message not found
        }

        //helper function
        private static Inbox GetInboxMessageById(int messageId)
        {
            try
            {
                // Retrieve the message from the database based on the messageId
                string query = $"select * from Inbox where id=N'{messageId}' And IsTrash='false'";
                DataTable dt = SQLHelper.SelectData(query);
                if (dt != null)
                {
                    Inbox inboxMsg = new Inbox();
                    inboxMsg.Subject = dt.Rows[0]["Subject"].ToString();
                    inboxMsg.SenderName = dt.Rows[0]["SenderName"].ToString();
                    inboxMsg.RecieverName = dt.Rows[0]["RecieverName"].ToString();
                    inboxMsg.Time = Convert.ToDateTime(dt.Rows[0]["Time"]);
                    inboxMsg.IsTrash = Convert.ToBoolean(dt.Rows[0]["IsTrash"]);

                    return inboxMsg;
                }
                else
                {
                    return null; // Message not found
                }
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions if necessary
                return null; // Return null to indicate an error occurred
            }
        }

        //helper function
        private static bool DeleteMessageFromDatabase(int messageId)
        {
            // Delete the message from the database based on the messageId
            string query = $"Delete *from Inbox where id= '{messageId}'";
            int response = SQLHelper.DoQuery(query);
            return response > 0;// Return true if the deletion is successful, false otherwise
        }

        //helper function
        private static bool UpdateMessageIsTrash(int messageId, bool isTrash)
        {
            // Update the IsTrash field of the message in the database based on the messageId
            string query = $" UPDATE Inbox SET IsTrash = '{isTrash}' WHERE Id = '{messageId}'";
            int response = SQLHelper.DoQuery(query);
            return response > 0;// Return true if the update is successful, false otherwise

        }

    }
}
