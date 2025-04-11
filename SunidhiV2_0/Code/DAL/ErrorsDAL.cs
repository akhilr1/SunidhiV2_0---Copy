using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunidhiV2_0.Models;
using SunidhiV2_0.Controllers;
using Dapper;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Data;
using SunidhiV2_0.Code.Utils;

namespace SunidhiV2_0.Code.DAL
{
    public class ErrorsDAL
    {
        protected IDbConnection _connection;

        public void SaveErrorLog(ErrorLog ObjErrorLog)
        {
            using (_connection = Utils.Database.GetDBConnection())
            {
                string query = string.Empty;
                query = "INSERT INTO TbErrorLog(UserName,MachineName,Message,StackTrace,RequestURL,Source,InnerException,Date)";
                query += " VALUES(@UserName,@MachineName,@Message,@StackTrace,@RequestURL,@Source,@InnerException,@Date);";
                _connection.Execute(query, new
                {
                    UserName = ObjErrorLog.UserName,
                    MachineName = ObjErrorLog.MachineName,
                    Message = ObjErrorLog.Message,
                    StackTrace = ObjErrorLog.StackTrace,
                    RequestURL = ObjErrorLog.RequestURL,
                    Source = ObjErrorLog.Source,
                    InnerException = ObjErrorLog.InnerException,
                    Date = ObjErrorLog.Date
                });
            }
        }
    }
}