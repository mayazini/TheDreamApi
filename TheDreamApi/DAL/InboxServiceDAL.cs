using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json;

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

                    string query = $"INSERT INTO Inbox (Message,SenderName, RecieverName,Time,Subject) VALUES ('{message}','{senderName}','{recieverName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',' {subject}')";
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
    }
}
