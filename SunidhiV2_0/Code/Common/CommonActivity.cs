using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Dapper;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace SunidhiV2_0.Code.Common
{
    public class CommonActivity
    {
        static protected IDbConnection _connection;

        #region GetDateTimeValue
        /// <summary>
        /// GetDateTimeValue
        /// </summary>
        /// <param name="strDateOfDelivery"></param>
        /// <returns></returns>
        public static DateTime? GetDateTimeValue(string strDateOfDelivery)
        {
            string[] ds;
            if (strDateOfDelivery.Contains('-'))
            {
                ds = strDateOfDelivery.Split(' ')[0].Split('-');
            }
            else
            {
                ds = strDateOfDelivery.Split(' ')[0].Split('/');

            }

            int Month = Convert.ToInt32(ds[1]);
            int Year = Convert.ToInt32(ds[2]);
            int Day = Convert.ToInt32(ds[0]);
            DateTime value = new DateTime(Year, Month, Day);
            return value;
        }
        #endregion GetDateTimeValue

        #region GenerateMonthDate
        /// <summary>
        /// GenerateMonthDate
        /// </summary>
        public static List<KeyValuePair<int, string>> GenerateMonthDate()
        {
            List<KeyValuePair<int, string>> listDate = new List<KeyValuePair<int, string>>();

            for (int i = 1; i <= 28; i++)
            {
                listDate.Add(new KeyValuePair<int, string>(i, i.ToString()));
            }

            return listDate;
        }
        #endregion GenerateMonthDate

        #region GenerateMonthList
        /// <summary>
        /// GenerateMonthList
        /// </summary>
        public static List<KeyValuePair<int, string>> GenerateMonthList()
        {
            List<KeyValuePair<int, string>> listDate = new List<KeyValuePair<int, string>>();
            int i = 1;

            foreach (string name in Enum.GetNames(typeof(Months)))
            {
                listDate.Add(new KeyValuePair<int, string>(i, name));
                i++;
            }

            return listDate;
        }
        #endregion GenerateMonthList

        #region GenerateWeekList
        /// <summary>
        /// GenerateWeekList
        /// </summary>
        public static List<KeyValuePair<int, string>> GenerateWeekList()
        {
            List<KeyValuePair<int, string>> listDate = new List<KeyValuePair<int, string>>();
            int i = 0;

            foreach (string name in Enum.GetNames(typeof(Weeks)))
            {
                listDate.Add(new KeyValuePair<int, string>(i, name));
                i++;
            }

            return listDate;
        }
        #endregion GenerateWeekList

        #region GetReportingFrequencyList
        /// <summary>
        /// GetReportingFrequencyList
        /// </summary>
        /// <returns>Frequency list</returns>
        public static List<KeyValuePair<int, string>> GetReportingFrequencyList()
        {
            List<KeyValuePair<int, string>> lstRptFrequency = new List<KeyValuePair<int, string>>();
            int i = 1;
            foreach (string name in Enum.GetNames(typeof(ReportingFrequency)))
            {
                lstRptFrequency.Add(new KeyValuePair<int, string>(i, name));
                i++;
            }

            return lstRptFrequency;
        }
        #endregion GetReportingFrequencyList

        #region GetReportingFrequencyDateList
        /// <summary>
        /// GetReportingFrequencyList
        /// </summary>
        /// <returns>Frequency list</returns>
        public static List<string> GetReportingFrequencyDateList(DateTime? StartDate, DateTime? endDate, int reportingFrequency, int day, int month, string onceDate = "")
        {
            int dtDifference = (int)(endDate.Value - StartDate.Value).TotalDays;
            List<string> lstDates = new List<string>();
            DateTime? freqValue = null;
            int count = 0;

            switch (reportingFrequency)
            {
                case (int)ReportingFrequency.Once:
                    DateTime dtOnce = GetDateTimeValue(onceDate).Value;
                    lstDates.Add(dtOnce.ToString("yyyy-MM-dd"));
                    break;
                case (int)ReportingFrequency.Daily:
                    for (int i = 0; i <= dtDifference; i++)
                    {
                        lstDates.Add(StartDate.Value.AddDays(i).ToString("yyyy-MM-dd"));
                    }
                    break;
                case (int)ReportingFrequency.Weekly:

                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if ((int)StartDate.Value.AddDays(i).DayOfWeek == day)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }

                    while (freqValue.Value.AddDays(count) <= endDate)
                    {
                        lstDates.Add(freqValue.Value.AddDays(count).ToString("yyyy-MM-dd"));
                        count = count + 7;
                    }
                    break;
                case (int)ReportingFrequency.Monthly:

                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Day == day)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }

                    while (freqValue.Value.AddMonths(count) <= endDate)
                    {
                        lstDates.Add(freqValue.Value.AddMonths(count).ToString("yyyy-MM-dd"));
                        count = count + 1;
                    }
                    break;
                case (int)ReportingFrequency.Quarterly:

                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Month == month
                            && StartDate.Value.AddDays(i).Day == day)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }

                    while (freqValue.Value.AddMonths(count) <= endDate)
                    {
                        lstDates.Add(freqValue.Value.AddMonths(count).ToString("yyyy-MM-dd"));
                        count = count + 3;
                    }
                    break;
                case (int)ReportingFrequency.Half_Yearly:
                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Month == month
                            && StartDate.Value.AddDays(i).Day == day)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }

                    while (freqValue.Value.AddMonths(count) <= endDate)
                    {
                        lstDates.Add(freqValue.Value.AddMonths(count).ToString("yyyy-MM-dd"));
                        count = count + 6;
                    }
                    break;
                case (int)ReportingFrequency.Yearly:
                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Month == month
                            && StartDate.Value.AddDays(i).Day == day)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }

                    while (freqValue.Value.AddMonths(count) <= endDate)
                    {
                        lstDates.Add(freqValue.Value.AddMonths(count).ToString("yyyy-MM-dd"));
                        count = count + 12;
                    }
                    break;
            }

            return lstDates;
        }
        #endregion GetReportingFrequencyList

        #region GetQuestionType
        /// <summary>
        /// GetQuestionType
        /// </summary>
        /// <returns> KeyValuePair </returns>
        public static List<KeyValuePair<int, string>> GetQuestionType()
        {
            List<KeyValuePair<int, string>> lstQuestionType = new List<KeyValuePair<int, string>>();
            dynamic obj;

            using (_connection = Utils.Database.GetDBConnection())
            {
                string query = @"SELECT ID,Type FROM TbQuestionType ";
                obj = _connection.Query<object>(query).ToList();

                foreach (dynamic item in obj)
                {
                    lstQuestionType.Add(new KeyValuePair<int, string>(item.ID, item.Type));
                }
            }

            return lstQuestionType;
        }
        #endregion GetQuestionType

        #region IsReportingFrequencyAvailable
        /// <summary>
        /// IsReportingFrequencyAvailable
        /// </summary>
        /// <returns>Frequency list</returns>
        public static bool IsReportingFrequencyAvailable(DateTime? StartDate, DateTime? endDate, int reportingFrequency, int ReportingMonth, int ReportingMonthDay, int ReportingWeek, int ReportingMonthDateDay)
        {
            int dtDifference = (int)(endDate.Value - StartDate.Value).TotalDays;
            DateTime? freqValue = null;
            bool status = false;

            switch (reportingFrequency)
            {
                case (int)ReportingFrequency.Weekly:

                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if ((int)StartDate.Value.AddDays(i).DayOfWeek == ReportingWeek)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }
                    break;
                case (int)ReportingFrequency.Monthly:

                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Day == ReportingMonthDay)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }
                    break;
                case (int)ReportingFrequency.Quarterly:

                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Month == ReportingMonth
                            && StartDate.Value.AddDays(i).Day == ReportingMonthDateDay)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }
                    break;
                case (int)ReportingFrequency.Half_Yearly:
                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Month == ReportingMonth
                            && StartDate.Value.AddDays(i).Day == ReportingMonthDateDay)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }

                    break;
                case (int)ReportingFrequency.Yearly:
                    for (int i = 0; i <= dtDifference; i++)
                    {
                        if (StartDate.Value.AddDays(i).Month == ReportingMonth
                            && StartDate.Value.AddDays(i).Day == ReportingMonthDateDay)
                        {
                            freqValue = StartDate.Value.AddDays(i);
                            break;
                        }
                    }
                    break;
            }

            status = (freqValue != null ? true : false);

            return status;
        }
        #endregion IsReportingFrequencyAvailable

        #region GetCommonCalcFunctionList

        public static List<KeyValuePair<int, string>> GetCommonCalcFunctionList()
        {
            List<KeyValuePair<int, string>> lstCommonFn = new List<KeyValuePair<int, string>>();
            int i = 1;
            foreach (string name in Enum.GetNames(typeof(CommonCalcFunction)))
            {
                lstCommonFn.Add(new KeyValuePair<int, string>(i, name));
                i++;
            }

            return lstCommonFn;
        }
        #endregion GetCommonCalcFunctionList

        #region GetTaskApprovedOrNot
        // check the survey task is approved or not
        public int GetTaskApprovedOrNot(string Type , int TypeID, int WorkspaceID)
        {
            dynamic obj;
            using (_connection = Utils.Database.GetDBConnection())
            {
                string query = @"SELECT MyTask.WFLevel from TaskSyncDB SyncDB inner join TbMyTask MyTask on MyTask.SyncTaskID = SyncDB.ID
                                where MyTask.TypeID = @TypeID and MyTask.Type =@Type and SyncDB.WorkspaceID = @WorkspaceID and SyncDB.IsActive = 1 order by MyTask.ID desc limit 1 ";
                obj = _connection.Query<int>(query, new { TypeID = TypeID, Type = Type, WorkspaceID = WorkspaceID }).FirstOrDefault();

            }
            return obj;
        }
        #endregion

        public static string ReverseGeoCoding(string latlong)
        {
            string apiKey = Startup.GoogleAPIKey;
            string baseUrlRGC = "https://maps.googleapis.com/maps/api/geocode/json?latlng="; // part1 of the URL for Reverse GeoCoding
            string plusUrl = "&key=" + apiKey + "&sensor=false"; // part2 of the URL
            //var json = new WebClient().DownloadString(baseUrlRGC + latitude.Replace(" ", "") + "," + longitude.Replace(" ", "") + plusUrl);//concatenate URL with the input lat/lng and downloads the requested resource
            var json = new WebClient().DownloadString(baseUrlRGC + latlong.Replace(" ", "") + plusUrl);//concatenate URL with the input lat/lng and downloads the requested resource
            GoogleGeoCodeResponse jsonResult = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(json); //deserializing the result to GoogleGeoCodeResponse

            string status = jsonResult.status; // get status

            string geoLocation = String.Empty;
            int counter = 0;

            if (status == "OK") //check if status is OK
            {
                for (int i = 0; i < jsonResult.results.Length; i++) //loop throught the result for addresses
                {
                    // geoLocation += "Address: " + jsonResult.results[i].formatted_address + Environment.NewLine; //append the result addresses to every new line

                    if (jsonResult.results[i].address_components.Length > 0)
                    {
                        for (int j = 0; j < jsonResult.results[i].address_components.Length; j++)
                        {
                            if (jsonResult.results[i].address_components[j].types.Length > 0)
                            {
                                for (int k = 0; k < jsonResult.results[i].address_components[j].types.Length; k++)
                                {
                                    if (jsonResult.results[i].address_components[j].types[k] == "administrative_area_level_1")
                                    {
                                        geoLocation += "state: " + jsonResult.results[i].address_components[j].long_name + Environment.NewLine; //state
                                        counter += 1;
                                    }
                                    if (jsonResult.results[i].address_components[j].types[k] == "administrative_area_level_2" ||
                                        jsonResult.results[i].address_components[j].types[k] == "administrative_area_level_3" ||
                                        jsonResult.results[i].address_components[j].types[k] == "administrative_area_level_4")
                                    {
                                        if (!geoLocation.ToString().Contains("district"))
                                        {
                                            geoLocation += "district: " + jsonResult.results[i].address_components[j].long_name + Environment.NewLine; // district
                                        }

                                        counter += 1;
                                    }
                                    if (counter > 1)
                                    {
                                        break;
                                    }
                                }
                                
                            }
                            if (counter > 1)
                            {
                                break;
                            }

                        }
                        if (counter > 1)
                        {
                            break;
                        }
                    }
                }
                
            }
            return geoLocation; //return result

        }

        public static  bool IsValidlatitudeAndlongitude(string latlong)
        {
            string pattern = @"^(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)$";
            return System.Text.RegularExpressions.Regex.IsMatch(latlong, pattern);
        }

        public enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum Weeks
        {
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }

        public enum ReportingFrequency
        {
            Never = 1,
            Once = 2,
            Daily = 3,
            Weekly = 4,
            Monthly = 5,
            Quarterly = 6,
            Half_Yearly = 7,
            Yearly = 8,
            Anytime = 9
        }
        public enum CommonCalcFunction
        {
            Average = 0,
            Sum = 1,
            Subtraction = 3,
            Multiplication = 4,
            RepeatVal_Aggregate = 2,
            RepeatVal_Aggregate_Average = 6,
            RepeatVal_Aggregate_Multiplication = 7,
            RepeatVal_Aggregate_Subtraction = 8,
            Generic_Formula = 9,
            IDCG_Calculation = 10
            //Percentage = 2
        }

        public static string GenerateOTP()
        {
            string OTPLength = "4";
            string OTP = string.Empty;

            string Chars = string.Empty;
            Chars = "1,2,3,4,5,6,7,8,9,0";

            char[] seplitChar = { ',' };
            string[] arr = Chars.Split(seplitChar);
            string NewOTP = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                NewOTP += temp;
                OTP = NewOTP;
            }
            return OTP;
        }
        public enum QuestionType
        {
            Date = 13,
            MultiChoice = 2,
            BenMultiChoice = 4,
            SingleChoice = 1,
            Dropdown = 7,
            CascadingDropdown = 8,
            GPS_Capture = 9,
            Note = 14
        }

        public enum LogicCondition
        {
            AND = 1,
            OR = 2
        }
    }

    public class GoogleGeoCodeResponse
    {

        public string status { get; set; }
        public results[] results { get; set; }

    }
    public class results
    {
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string[] types { get; set; }
        public address_component[] address_components { get; set; }
    }
    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
    public class geometry
    {
        public string location_type { get; set; }
        public location location { get; set; }
    }

    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    
}
