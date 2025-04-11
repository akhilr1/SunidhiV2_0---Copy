using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SunidhiV2_0.Models;

namespace SunidhiV2_0.Code.DAL
{
    public class NotificationDAL
    {
        protected IDbConnection _connection;

        #region SendUserNotification
        /// <summary>
        /// SendUserNotification
        /// </summary>
        /// <param name="objNotification"></param>
        public async void SendUserNotification(Notification objNotification)
        {
            string filename = Startup.Basepath + Startup.TokenFile;
            objNotification.FileName = filename;

            List<string> lstTokens = new List<string>();
            using (_connection = Utils.Database.GetDBConnection())
            {
                string query = @"SELECT IFNULL(Token,'') Token FROM TbNotificationToken WHERE UserId=@UserId AND IFNULL(Token,'') != '' AND CreatedDate > DATE_SUB(NOW(), INTERVAL 24 HOUR) ORDER BY ID DESC;";
                lstTokens = _connection.Query<string>(query, new { UserId = objNotification.UserId }).ToList();
            }

            if (lstTokens != null && lstTokens.Count > 0)
            {
                foreach (var itm in lstTokens)
                {
                    objNotification.Token = itm;

                    if(objNotification.Token != null && objNotification.Token != "null")
                    {
                        await SendPushNotification(objNotification);
                    }
                }
            }
        }
        #endregion SendUserNotification

        #region SendPushNotification
        /// <summary>
        /// SendPushNotification
        /// </summary>
        /// <param name="objNotification"></param>
        /// <param name="_hostingEnvironment"></param>
        /// <returns></returns>
        public async Task SendPushNotification(Notification objNotification)
        {
            if (!System.IO.File.Exists(objNotification.FileName))
            {
                return;
            }

            var credential = GoogleCredential.FromFile(objNotification.FileName)
            .CreateScoped(new List<string>() { "https://www.googleapis.com/auth/firebase.messaging" });
            var accessToken = credential.UnderlyingCredential.GetAccessTokenForRequestAsync().Result;
            var responseStr = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                // client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; UTF-8");
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { message = new { notification = new { title = objNotification.Title, body = objNotification.Subject }, token = objNotification.Token } }, Formatting.Indented);

                var response = client.PostAsync("https://fcm.googleapis.com/v1/projects/test-9cafc/messages:send", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                var result = response.Result;
                var stream = await result.Content.ReadAsStreamAsync();
                responseStr = new StreamReader(stream).ReadToEnd();
            }
        }
        #endregion SendPushNotification
    }
}
