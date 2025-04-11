#region NameSpaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.Common;
using System.Dynamic;
using Newtonsoft.Json;
#endregion #region NameSpaces

namespace SunidhiV2_0.Controllers
{
    public class CommonController : Controller
    {
        #region GetReportingFrequencyDetails
        /// <summary>
        /// GetReportingFrequencyDetails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetReportingFrequencyDetails()
        {
            var list = new List<dynamic>();

            dynamic DyObj = new ExpandoObject();

            DyObj.RptFrequencyList = CommonActivity.GetReportingFrequencyList();
            DyObj.WeekList = CommonActivity.GenerateWeekList();
            DyObj.MonthList = CommonActivity.GenerateMonthList();
            DyObj.MonthDate = CommonActivity.GenerateMonthDate();
            list.Add(DyObj);

            return Json(new { status = "success", data = DyObj }, new JsonSerializerSettings());
        }
        #endregion GetReportingFrequencyDetails

        #region GetQuestionType
        /// <summary>
        /// GetQuestionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetQuestionType()
        {
            List<KeyValuePair<int, string>> lstQuestionType = new List<KeyValuePair<int, string>>();
            lstQuestionType = CommonActivity.GetQuestionType();

            return Json(new {status = "success", data = lstQuestionType }, new JsonSerializerSettings());
        }
        #endregion GetQuestionType

    }
}