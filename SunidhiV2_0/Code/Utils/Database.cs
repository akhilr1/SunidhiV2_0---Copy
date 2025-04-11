using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using MySql.Data.MySqlClient;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace SunidhiV2_0.Code.Utils
{
    public class Database
    {
        public static MySql.Data.MySqlClient.MySqlConnection GetDBConnection()
        {
            String strConnection = Startup.ConnectionString;
            return new MySql.Data.MySqlClient.MySqlConnection(strConnection);
        }
    }
}