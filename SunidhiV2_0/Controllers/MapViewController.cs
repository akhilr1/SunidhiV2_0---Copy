using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SunidhiV2_0.Code.DAL;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.Common;
using Microsoft.AspNetCore.Hosting;

namespace SunidhiV2_0.Controllers
{
    public class MapViewController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        [HttpPost]
        public IActionResult GetprojectswithGPSinsurvey(int WorkspaceID, string VersionUniqueID)
        {
            string status = "Success";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            MapView lstMapView = new MapView();
            lstMapView = objMapViewDAL.GetProjectandSurveyListforWorkspace(WorkspaceID, VersionUniqueID);
            if (lstMapView.Project_list.Count == 0)
            {
                status = "failed";
            }
            return Json(new { data = lstMapView, status = status }, new JsonSerializerSettings());

        }
        public ActionResult SaveShareLinkVersion(SharelinkVersion inputObj)
        {
            MapViewDAL objMapViewDAL = new MapViewDAL();
            string status = "Success";
            string Message = "Share link version saved";
            int idversion = 0;
            bool isUpdated = true;
            //int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            idversion = objMapViewDAL.IsVersionExists(inputObj.VersionUniqueId);
            if (idversion > 0)
            {
                isUpdated = objMapViewDAL.UpdateShareLinkVersion(inputObj, idversion);
            }
            else
            {
                isUpdated = objMapViewDAL.SaveShareLinkVersion(inputObj);
            }

            if (!isUpdated)
            {
                status = "Failed";
                Message = "Failed because of some reason";
            }

            return Json(new { status = status, Message = Message }, new JsonSerializerSettings());
        }

        [HttpPost]
        public IActionResult GetStateWiseSurveyResponse(int SurveyID, int ProjectID, string versionUniqueId)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            List<SurveyResponseGPSLocation> gpsLocationList = new List<SurveyResponseGPSLocation>();
            gpsLocationList = new MapViewDAL().GetSurveyResponseLocation(SurveyID);
            //var statesList = objProjectDAL.GetStateList();
            //var districtsList = objProjectDAL.GetDistrictList();

            string status = "false";
            MapViewDAL objMapViewDAL = new MapViewDAL();

            List<StatewiseSurveyResponse> objSurveyResponselist = new List<StatewiseSurveyResponse>();
            objSurveyResponselist = objMapViewDAL.GetStateWiseSurveyResponse(SurveyID, ProjectID);

            List<ReducedStatewiseSurveyResponse> objReducedSurveyResponselist = new List<ReducedStatewiseSurveyResponse>();
            if (objSurveyResponselist != null)
            {
                //for filtering based on share version details - starts
                if (!string.IsNullOrEmpty(versionUniqueId))
                {
                    string cordinates = objMapViewDAL.GetShareVersionCordinates(versionUniqueId);
                    if (!string.IsNullOrEmpty(cordinates))
                    {
                        string[] strsplt = cordinates.Split('|');

                        foreach (var itm in strsplt)
                        {
                            objSurveyResponselist.RemoveAll(x => x.Answer.Trim() == itm.Trim());
                        }
                    }
                }
                //for filtering based on share version details - ends


                status = "true";

                foreach (StatewiseSurveyResponse item in objSurveyResponselist)
                {
                    item.StateID = 0;
                    item.StateName = "";

                    if (CommonActivity.IsValidlatitudeAndlongitude(item.Answer) && gpsLocationList != null && gpsLocationList.Count > 0)
                    {
                        List<SurveyResponseGPSLocation> objSurveyResponselist1 = new List<SurveyResponseGPSLocation>();
                        objSurveyResponselist1 = gpsLocationList.Where(x => x.GPS == item.Answer && x.SyncTaskID == item.SyncTaskID && x.SubmissionCount == item.SubmissionCount).ToList();

                        if (objSurveyResponselist1 != null && objSurveyResponselist1.Count > 0)
                        {
                            item.StateID = 0;
                            item.StateName = (objSurveyResponselist1[0].State != null ? objSurveyResponselist1[0].State.Trim() : "");
                        }
                        //string geoLocation = CommonActivity.ReverseGeoCoding(item.Answer);
                        //if (!string.IsNullOrEmpty(geoLocation))
                        //{
                        //    //bool b = statesList.Any(s => geoLocation.Contains(s);
                        //    //foreach (State statename in statesList)
                        //    //{
                        //    //    if (System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(geoLocation, statename.StateName, System.Globalization.CompareOptions.IgnoreCase) >= 0)
                        //    //    {
                        //    //        item.StateID = statename.StateID;
                        //    //        item.StateName = statename.StateName;
                        //    //        break;
                        //    //    }
                        //    //}

                        //    foreach (var myString in geoLocation.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                        //    {
                        //        if (myString.Length > 0)
                        //        {
                        //            if (myString.ToString().Contains("state"))
                        //            {
                        //                var indexOfColon = myString.ToString().IndexOf(":");
                        //                var statename = myString.ToString().Substring(indexOfColon + 1);
                        //                item.StateName = statename;
                        //            }

                        //        }
                        //    }
                        //}

                    }
                }

                //

                objReducedSurveyResponselist = objSurveyResponselist.Where(c => !string.IsNullOrEmpty(c.StateName)).Select(e => new ReducedStatewiseSurveyResponse { SurveyID = e.SurveyID, SurveyName = e.SurveyName, ResponseCount = e.ResponseCount, StateID = e.StateID, StateName = e.StateName, ResponseStartDate = e.ResponseStartDate, ResponseEndDate = e.ResponseEndDate, IsBeneficiarySurvey = e.IsBeneficiarySurvey, BeneficiaryName = e.BeneficiaryName }).ToList();

                foreach (ReducedStatewiseSurveyResponse item in objReducedSurveyResponselist)
                {
                    item.ResponseCount = objReducedSurveyResponselist.Where(temp => temp.StateName.Equals(item.StateName))
                    .Select(temp => temp).Count();
                    //item.ResponseCount = objReducedSurveyResponselist.Where(temp => temp.StateID.Equals(item.StateID))
                    //.Select(temp => temp).Count();
                    //item.ResponseCount = (from temp in objReducedSurveyResponselist where temp.Equals(item.StateID) select temp).Count();
                }



                //var g = reducedList.GroupBy(i => i.StateID);
                //foreach (var grp in g)
                //{
                //    Console.WriteLine("{0} {1}", grp.Key, grp.Count());
                //}

                //

            }

            var objgroupedList =
                        (from i in objReducedSurveyResponselist
                         group i by new { i.StateID, i.StateName, i.SurveyID, i.SurveyName, i.IsBeneficiarySurvey, i.ResponseCount, i.ResponseStartDate, i.ResponseEndDate } into g
                         select new { g.Key, count = g.Count(), BeneficiaryName = (g.Select(kvp => kvp.BeneficiaryName).ToString() == "" ? "" : string.Join(",", g.Select(kvp => kvp.BeneficiaryName))) }).ToList();
            //return Json(new { data = objSurveyResponselist, status = status }, new JsonSerializerSettings());
            return Json(new { data = objgroupedList, status = status }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetDistrictWiseSurveyResponse(int SurveyID, string StateName, string versionUniqueId)
        {
            string status = "false";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            List<SurveyResponseGPSLocation> gpsLocationList = new List<SurveyResponseGPSLocation>();
            gpsLocationList = new MapViewDAL().GetSurveyResponseLocation(SurveyID);
            //var districtsList = objMapViewDAL.GetDistrictList(StateName);

            List<DistrictwiseSurveyResponse> objSurveyResponselist = new List<DistrictwiseSurveyResponse>();
            objSurveyResponselist = objMapViewDAL.GetDistrictWiseSurveyResponse(SurveyID);
            List<ReducedDistrictwiseSurveyResponse> objReducedDistrictwiseResponselist = new List<ReducedDistrictwiseSurveyResponse>();
            if (objSurveyResponselist != null)
            {
                //for filtering based on share version details - starts
                if (!string.IsNullOrEmpty(versionUniqueId))
                {
                    string cordinates = objMapViewDAL.GetShareVersionCordinates(versionUniqueId);
                    if (!string.IsNullOrEmpty(cordinates))
                    {
                        string[] strsplt = cordinates.Split('|');

                        foreach (var itm in strsplt)
                        {
                            objSurveyResponselist.RemoveAll(x => x.Answer.Trim() == itm.Trim());
                        }
                    }
                }
                //for filtering based on share version details - ends


                status = "true";

                foreach (DistrictwiseSurveyResponse item in objSurveyResponselist)
                {
                    item.DistrictID = 0;
                    item.DistrictName = "";

                    if (CommonActivity.IsValidlatitudeAndlongitude(item.Answer))
                    {
                        List<SurveyResponseGPSLocation> objSurveyResponselist1 = new List<SurveyResponseGPSLocation>();
                        objSurveyResponselist1 = gpsLocationList.Where(x => x.GPS == item.Answer && x.SyncTaskID == item.SyncDBID && x.SubmissionCount == item.SubmissionCount).ToList();

                        if (objSurveyResponselist1 != null && objSurveyResponselist1.Count > 0)
                        {
                            item.DistrictID = 0;
                            item.DistrictName = (objSurveyResponselist1[0].District != null ? objSurveyResponselist1[0].District.Trim() : "");
                            item.StateName = (objSurveyResponselist1[0].State != null ? objSurveyResponselist1[0].State.Trim() : "");
                        }

                        //string geoLocation = CommonActivity.ReverseGeoCoding(item.Answer);
                        //if (!string.IsNullOrEmpty(geoLocation))
                        //{
                        //    //foreach (District districtname in districtsList)
                        //    //{
                        //    //    if (System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(geoLocation, districtname.DistrictName, System.Globalization.CompareOptions.IgnoreCase) >= 0)
                        //    //    {
                        //    //        item.DistrictID = districtname.DistrictID;
                        //    //        item.DistrictName = districtname.DistrictName;
                        //    //        break;
                        //    //    }
                        //    //}

                        //    foreach (var myString in geoLocation.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                        //    {
                        //        if (myString.Length > 0)
                        //        {
                        //            if (myString.ToString().Contains("district"))
                        //            {
                        //                var indexOfColon = myString.ToString().IndexOf(":");
                        //                var distname = myString.ToString().Substring(indexOfColon + 1);
                        //                item.DistrictName = distname;
                        //            }
                        //            if (myString.ToString().Contains("state"))
                        //            {
                        //                var indexOfColon = myString.ToString().IndexOf(":");
                        //                var statename = myString.ToString().Substring(indexOfColon + 1);
                        //                item.StateName = statename;
                        //            }

                        //        }
                        //    }
                        //}

                    }
                }

                //
                objReducedDistrictwiseResponselist = objSurveyResponselist.Where(c => !string.IsNullOrEmpty(c.DistrictName)).Where(d => d.StateName.Trim().ToLower() == StateName.Trim().ToLower()).Select(e => new ReducedDistrictwiseSurveyResponse { SurveyID = e.SurveyID, SurveyName = e.SurveyName, ResponseCount = e.ResponseCount, DistrictID = e.DistrictID, DistrictName = e.DistrictName, ResponseStartDate = e.ResponseStartDate, ResponseEndDate = e.ResponseEndDate, IsBeneficiarySurvey = e.IsBeneficiarySurvey, BeneficiaryName = e.BeneficiaryName }).ToList();

                foreach (ReducedDistrictwiseSurveyResponse item in objReducedDistrictwiseResponselist)
                {
                    item.ResponseCount = objReducedDistrictwiseResponselist.Where(temp => temp.DistrictName.Equals(item.DistrictName))
                    .Select(temp => temp).Count();
                    //item.ResponseCount = objReducedDistrictwiseResponselist.Where(temp => temp.DistrictID.Equals(item.DistrictID))
                    //.Select(temp => temp).Count();
                    //item.ResponseCount = (from temp in objReducedSurveyResponselist where temp.Equals(item.StateID) select temp).Count();
                }
                //
            }

            var objgroupedList =
                        (from i in objReducedDistrictwiseResponselist
                         group i by new { i.DistrictID, i.DistrictName, i.SurveyID, i.SurveyName, i.IsBeneficiarySurvey, i.ResponseCount, i.ResponseStartDate, i.ResponseEndDate } into g
                         select new { g.Key, count = g.Count(), BeneficiaryName = (g.Select(kvp => kvp.BeneficiaryName).ToString() == "" ? "" : string.Join(",", g.Select(kvp => kvp.BeneficiaryName))) }).ToList();
            //return Json(new { data = objSurveyResponselist, status = status }, new JsonSerializerSettings());
            return Json(new { data = objgroupedList, status = status }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetResponseGeoCoordinates(int SurveyID, string DistrictName)
        {

            string status = "false";
            MapViewDAL objMapViewDAL = new MapViewDAL();

            List<SurveyResponseGPSLocation> gpsLocationList = new List<SurveyResponseGPSLocation>();
            gpsLocationList = new MapViewDAL().GetSurveyResponseLocation(SurveyID);
            // var DistrictName = objMapViewDAL.GetDistrictName(DistrictID);

            List<DistrictwiseSurveyResponse> objSurveyResponselist = new List<DistrictwiseSurveyResponse>();
            List<Geo_coordinate_list> Geo_coordinate_list = new List<Geo_coordinate_list>();
            Geo_coordinate_list objResponseGeoCoordinatest = new Geo_coordinate_list();
            objSurveyResponselist = objMapViewDAL.GetResponseGeoCoordinate(SurveyID);
            if (objSurveyResponselist.Count > 0)
            {
                status = "true";

                foreach (DistrictwiseSurveyResponse item in objSurveyResponselist)
                {
                    if (CommonActivity.IsValidlatitudeAndlongitude(item.Answer))
                    {
                        //string geoLocation = CommonActivity.ReverseGeoCoding(item.Answer);
                        //if (!string.IsNullOrEmpty(geoLocation))
                        //{

                        //    if (geoLocation.ToUpper().Contains(DistrictName.ToUpper()))
                        //    {
                        //        objResponseGeoCoordinatest = new Geo_coordinate_list();
                        //        objResponseGeoCoordinatest.SurveyID = item.SurveyID;
                        //        objResponseGeoCoordinatest.Geo_coordinates = item.Answer;
                        //        objResponseGeoCoordinatest.SyncDBID = item.SyncDBID;
                        //        objResponseGeoCoordinatest.CreatedDate = item.CreatedDate;
                        //        objResponseGeoCoordinatest.SubmissionCount = item.SubmissionCount;
                        //        Geo_coordinate_list.Add(objResponseGeoCoordinatest);
                        //    }
                        //}

                        List<SurveyResponseGPSLocation> objSurveyResponselist1 = new List<SurveyResponseGPSLocation>();
                        objSurveyResponselist1 = gpsLocationList.Where(x => x.GPS == item.Answer.Trim() && x.SyncTaskID == item.SyncDBID && x.SubmissionCount == item.SubmissionCount).ToList();


                        if (objSurveyResponselist1 != null && objSurveyResponselist1.Count > 0)
                        {
                            if (objSurveyResponselist1[0].District.ToUpper().Contains(DistrictName.ToUpper()))
                            {
                                objResponseGeoCoordinatest = new Geo_coordinate_list();
                                objResponseGeoCoordinatest.SurveyID = item.SurveyID;
                                objResponseGeoCoordinatest.Geo_coordinates = item.Answer;
                                objResponseGeoCoordinatest.SyncDBID = item.SyncDBID;
                                objResponseGeoCoordinatest.CreatedDate = item.CreatedDate;
                                objResponseGeoCoordinatest.SubmissionCount = item.SubmissionCount;
                                Geo_coordinate_list.Add(objResponseGeoCoordinatest);
                            }
                        }
                    }
                }
            }
            return Json(new { data = Geo_coordinate_list, status = status }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetPivotPresetList(int SurveyID, string UserID)
        {
            string status = "Success";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            List<PresetList> objPresetList = new List<PresetList>();
            objPresetList = objMapViewDAL.GetPivotPresetList(SurveyID, UserID);
            if (objPresetList == null || objPresetList.Count == 0)
            {
                status = "failed";
            }
            return Json(new { data = objPresetList, status = status }, new JsonSerializerSettings());

        }
        public IActionResult GetVersionDetails(int WorkspaceID, int UserID)
        {
            string status = "Success";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            List<VersionDetails> objVersionDetails = new List<VersionDetails>();
            objVersionDetails = objMapViewDAL.GetVersionDetails(WorkspaceID, UserID);
            if (objVersionDetails == null || objVersionDetails.Count == 0)
            {
                status = "failed";
            }
            return Json(new { data = objVersionDetails, status = status }, new JsonSerializerSettings());

        }

        public IActionResult IsVersionExists(string VersionUniqueId)
        {
            string status = "Success";
            string Message = "Version Unique Id exists";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            List<VersionDetails> objVersionDetails = new List<VersionDetails>();
            int VerssionID = objMapViewDAL.IsVersionExists(VersionUniqueId);
            if (VerssionID == 0)
            {
                status = "failed";
                Message = "Version Unique Id does not exists";
            }
            return Json(new { status = status, Message = Message }, new JsonSerializerSettings());

        }

        [HttpPost]
        public IActionResult GetPresetResponseDetails(int SurveyID, int UserID, string StateName, string DistrictName, int SyncDBID)
        {
            List<SurveyResponseGPSLocation> gpsLocationList = new List<SurveyResponseGPSLocation>();
            gpsLocationList = new MapViewDAL().GetSurveyResponseLocation(SurveyID);

            string status = "true";
            dynamic responseList = "";

            //if (SurveyID > 0 && UserID > 0 && !string.IsNullOrEmpty(StateName))
            if (SurveyID > 0 && !string.IsNullOrEmpty(StateName))
            {
                string url = "getpivotdata?surveyid=" + SurveyID;

                ProjectSurveyController temp = new ProjectSurveyController(_hostingEnvironment);
                var resp = temp.GetPivotTableData(url);
                resp.Wait(); // need to change
                var result = resp.Result;

                //var responseList = JsonConvert.DeserializeObject<List<dynamic>>(result);
                responseList = JsonConvert.DeserializeObject<List<dynamic>>(result);
                List<int> removeIndexes = new List<int>();
                string distname = "", statename = "";

                for (int x = 0; x < responseList.Count; x++)
                {
                    var properties = (Newtonsoft.Json.Linq.JObject)responseList[x];
                    properties.Add(new Newtonsoft.Json.Linq.JProperty("StateName", ""));
                    properties.Add(new Newtonsoft.Json.Linq.JProperty("DistrictName", ""));

                    foreach (Newtonsoft.Json.Linq.JProperty property in responseList[x].Properties())
                    {
                        //Console.WriteLine(property.Name + " - " + property.Value);
                        string tempValue = property.Value.ToString();
                        string tempKey = property.Name.ToString();

                        if (tempKey.ToString().Contains("(Beneficiary)"))
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(tempValue) && tempValue.Trim() == "0,0")
                        {
                            removeIndexes.Add(x);
                            break;
                        }

                        if (CommonActivity.IsValidlatitudeAndlongitude(tempValue) && !string.IsNullOrEmpty(tempValue) && tempValue.Trim() != "0,0")
                        {
                            List<SurveyResponseGPSLocation> objSurveyResponselist1 = new List<SurveyResponseGPSLocation>();
                            objSurveyResponselist1 = gpsLocationList.Where(m => m.GPS == tempValue.Trim()).ToList();

                            distname = "";
                            statename = "";

                            if (objSurveyResponselist1 != null && objSurveyResponselist1.Count >0)
                            {
                                distname = (objSurveyResponselist1[0].District != null ? objSurveyResponselist1[0].District.Trim():"");
                                statename = (objSurveyResponselist1[0].State != null ? objSurveyResponselist1[0].State.Trim() : "");

                                if (!string.IsNullOrEmpty(DistrictName))
                               {
                                    if (distname.Trim().ToLower() != DistrictName.Trim().ToLower())
                                    {
                                        removeIndexes.Add(x);
                                        break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(StateName))
                                {
                                    if (statename.Trim().ToLower() != StateName.Trim().ToLower())
                                    {
                                        removeIndexes.Add(x);
                                        break;
                                    }
                                }
                            } 
                            else
                            {
                                removeIndexes.Add(x);
                                break;
                            }


                            //string geoLocation = CommonActivity.ReverseGeoCoding(tempValue);
                            //if (!string.IsNullOrEmpty(geoLocation))
                            //{

                            //    foreach (var myString in geoLocation.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                            //    {
                            //        if (myString.Length > 0)
                            //        {
                            //            if (!string.IsNullOrEmpty(DistrictName))
                            //            {
                            //                if (myString.ToString().Contains("district"))
                            //                {
                            //                    var indexOfColon = myString.ToString().IndexOf(":");
                            //                    var distname = myString.ToString().Substring(indexOfColon + 1);
                            //                    //list[x].Properties["DistrictName"] = distname;

                            //                    if (distname.Trim().ToLower() != DistrictName.Trim().ToLower())
                            //                    {
                            //                        removeIndexes.Add(x);
                            //                        break;
                            //                    }

                            //                }
                            //            }
                            //            if (!string.IsNullOrEmpty(StateName))
                            //            {
                            //                if (myString.ToString().Contains("state"))
                            //                {
                            //                    var indexOfColon = myString.ToString().IndexOf(":");
                            //                    var statename = myString.ToString().Substring(indexOfColon + 1);

                            //                    //list[x].Properties["StateName"] = statename;


                            //                    if (statename.Trim().ToLower() != StateName.Trim().ToLower())
                            //                    {
                            //                        removeIndexes.Add(x);
                            //                        break;
                            //                    }
                            //                }
                            //            }

                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    // -ve gps coordinates do not have address
                            //    removeIndexes.Add(x);
                            //    break;
                            //}
                        }

                        if (tempKey == "SyncTaskID" && SyncDBID > 0)
                        {
                            if (!string.IsNullOrEmpty(tempValue))
                            {
                                if (Convert.ToInt32(tempValue) != SyncDBID)
                                {
                                    removeIndexes.Add(x);
                                    break;
                                }
                            }
                        }

                        //if (tempKey == "SubmittedByUserID" && UserID > 0)
                        //{
                        //    if (!string.IsNullOrEmpty(tempValue))
                        //    {
                        //        if (Convert.ToInt32(tempValue) != UserID)
                        //        {
                        //            removeIndexes.Add(x);
                        //        }
                        //    }
                        //}

                        //dict.Add(property.Name, tempValue);
                    }
                }
                foreach (int indice in removeIndexes.OrderByDescending(v => v))
                {
                    if (indice < responseList.Count)
                    {
                        responseList.RemoveAt(indice);
                    }
                }
            }
            else
            {
                status = "false";
            }

            return Json(new { data = responseList, status = status }, new JsonSerializerSettings());
        }
        [HttpPost]
        public IActionResult GetGeoCoordinateDetails(int SyncTaskID)
        {
            string status = "Success";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            List<Geo_coordinate_Details> objcoordinateDetails = new List<Geo_coordinate_Details>();
            objcoordinateDetails = objMapViewDAL.GetGeoCoordinateDetails(SyncTaskID);
            if (objcoordinateDetails == null || objcoordinateDetails.Count == 0)
            {
                status = "failed";
            }
            return Json(new { data = objcoordinateDetails, status = status }, new JsonSerializerSettings());

        }
        [HttpPost]
        public IActionResult GetVersionDetailsByVersionUniqueID(string VersionUniqueID)
        {
            string status = "Success";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            VersionDataIDs objVersionData = new VersionDataIDs();
            objVersionData = objMapViewDAL.GetVersionDetailsByVersionUniqueID(VersionUniqueID);
            if (objVersionData == null)
            {
                status = "failed";
            }
            return Json(new { data = objVersionData, status = status }, new JsonSerializerSettings());

        }

        [HttpGet]
        public ShareMapSurvey GetSurveyResponseDetail(int SyncTaskID, int SurveyId, int SubmissionCount = 0)
        {
            MapViewDAL objMapViewDAL = new MapViewDAL();
            ShareMapSurvey objProjectSurvey = new ShareMapSurvey();
            objProjectSurvey = objMapViewDAL.GetIndividualResponseData(SyncTaskID, SurveyId, SubmissionCount);

            return objProjectSurvey;
        }


        public IActionResult GetShareVersionDetails(int SurveyID, int ProjectID)
        {
            List<MapStateCollection> lstMapStateCollection = new List<MapStateCollection>();
            List<MapDistrictCollection> lstMapDistrictCollection = new List<MapDistrictCollection>();
            List<MapDistrictCordinatesCollection> lstMapDistrictCordinatesCollection = new List<MapDistrictCordinatesCollection>();
            MapShareVersionList objMapShareVersionList = new MapShareVersionList();
            objMapShareVersionList = new MapViewController().GetShareVersionStateWiseSurveyResponse(SurveyID, ProjectID);

            //if (lstMapStateCollection != null && lstMapStateCollection.Count > 0)
            //{
            //    foreach (var itm in lstMapStateCollection)
            //    {
            //        List<MapDistrictCollection> lstMapDistrictCollectionChild = new List<MapDistrictCollection>();
            //        lstMapDistrictCollectionChild = new MapViewController().GetShareVersionDistrictWiseSurveyResponse(SurveyID, itm.StateName.Trim());
            //        lstMapDistrictCollection.AddRange(lstMapDistrictCollectionChild);

            //        if (lstMapDistrictCollectionChild != null && lstMapDistrictCollectionChild.Count > 0)
            //        {
            //            foreach (var itmDistrict in lstMapDistrictCollectionChild)
            //            {
            //                List<MapDistrictCordinatesCollection> lstMapDistrictCordinatesChild = new List<MapDistrictCordinatesCollection>();
            //                lstMapDistrictCordinatesChild = new MapViewController().GetShareVersionResponseGeoCoordinates(SurveyID, itmDistrict.DistrictName.Trim());
            //                lstMapDistrictCordinatesCollection.AddRange(lstMapDistrictCordinatesChild);
            //            }
            //        }
            //    }
            //}

            //MapShareVersionList objMapShareVersionList = new MapShareVersionList();
            //objMapShareVersionList.lstState = new List<MapStateCollection>();
            //objMapShareVersionList.lstState = lstMapStateCollection;

            //objMapShareVersionList.lstDistricts = new List<MapDistrictCollection>();
            //objMapShareVersionList.lstDistricts = lstMapDistrictCollection;

            //objMapShareVersionList.lstDistrictCordinates = new List<MapDistrictCordinatesCollection>();
            //objMapShareVersionList.lstDistrictCordinates = lstMapDistrictCordinatesCollection;

            return Json(new { data = objMapShareVersionList, status = "success" }, new JsonSerializerSettings());
        }

        public MapShareVersionList GetShareVersionStateWiseSurveyResponse(int SurveyID, int ProjectID)
        {
            ProjectDAL objProjectDAL = new ProjectDAL();
            string status = "false";
            MapViewDAL objMapViewDAL = new MapViewDAL();

            List<SurveyResponseGPSLocation> gpsLocationList = new List<SurveyResponseGPSLocation>();
            gpsLocationList = new MapViewDAL().GetSurveyResponseLocation(SurveyID);

            List<StatewiseSurveyResponse> objSurveyResponselist = new List<StatewiseSurveyResponse>();
            objSurveyResponselist = objMapViewDAL.GetStateWiseSurveyResponse(SurveyID, ProjectID);
            List<ReducedStatewiseSurveyResponse> objReducedSurveyResponselist = new List<ReducedStatewiseSurveyResponse>();
            List<DistrictwiseSurveyResponse> objSurveyDistrictwiseSurveyResponse = new List<DistrictwiseSurveyResponse>();
            List<ReducedDistrictwiseSharedSurveyResponse> objReducedDistrictwiseResponselist = new List<ReducedDistrictwiseSharedSurveyResponse>();

            List<MapDistrictCordinatesCollection> Geo_coordinate_list = new List<MapDistrictCordinatesCollection>();
            MapDistrictCordinatesCollection objResponseGeoCoordinatest = new MapDistrictCordinatesCollection();
            DistrictwiseSurveyResponse objDistrictwiseSurveyResponse = new DistrictwiseSurveyResponse();

            if (objSurveyResponselist != null)
            {
                status = "true";

                foreach (StatewiseSurveyResponse item in objSurveyResponselist)
                {
                    item.StateID = 0;
                    item.StateName = "";

                    objDistrictwiseSurveyResponse = new DistrictwiseSurveyResponse();

                    objDistrictwiseSurveyResponse.DistrictID = 0;
                    objDistrictwiseSurveyResponse.DistrictName = "";

                    objResponseGeoCoordinatest = new MapDistrictCordinatesCollection();
                    objResponseGeoCoordinatest.SurveyID = SurveyID;

                    if (CommonActivity.IsValidlatitudeAndlongitude(item.Answer))
                    {
                        List<SurveyResponseGPSLocation> objSurveyResponselist1 = new List<SurveyResponseGPSLocation>();
                        objSurveyResponselist1 = gpsLocationList.Where(x => x.GPS == item.Answer && x.SyncTaskID == item.SyncTaskID && x.SubmissionCount == item.SubmissionCount).ToList();

                        if (objSurveyResponselist1 != null && objSurveyResponselist1.Count > 0)
                        {
                            item.StateName = objSurveyResponselist1[0].State;
                            objDistrictwiseSurveyResponse.StateName = (objSurveyResponselist1[0].State != null ? objSurveyResponselist1[0].State.Trim() : "");
                            objDistrictwiseSurveyResponse.DistrictName = (objSurveyResponselist1[0].District != null ? objSurveyResponselist1[0].District.Trim() : "");
                            objResponseGeoCoordinatest.Geo_coordinates = item.Answer;
                            objResponseGeoCoordinatest.DistrictName = (objSurveyResponselist1[0].District != null ? objSurveyResponselist1[0].District.Trim() : "");

                            objSurveyDistrictwiseSurveyResponse.Add(objDistrictwiseSurveyResponse);
                            Geo_coordinate_list.Add(objResponseGeoCoordinatest);
                        }

                        //string geoLocation = CommonActivity.ReverseGeoCoding(item.Answer);
                        //if (!string.IsNullOrEmpty(geoLocation))
                        //{
                        //    foreach (var myString in geoLocation.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                        //    {
                        //        if (myString.Length > 0)
                        //        {
                        //            if (myString.ToString().Contains("district"))
                        //            {
                        //                var indexOfColon = myString.ToString().IndexOf(":");
                        //                var distname = myString.ToString().Substring(indexOfColon + 1);
                        //                objDistrictwiseSurveyResponse.DistrictName = distname.Trim();

                        //                objResponseGeoCoordinatest.Geo_coordinates = item.Answer;
                        //                objResponseGeoCoordinatest.DistrictName = distname.Trim();
                        //            }

                        //            if (myString.ToString().Contains("state"))
                        //            {
                        //                var indexOfColon = myString.ToString().IndexOf(":");
                        //                var statename = myString.ToString().Substring(indexOfColon + 1);
                        //                item.StateName = statename.Trim();
                        //                objDistrictwiseSurveyResponse.StateName = statename.Trim();
                        //            }

                        //        }
                        //    }

                        //    objSurveyDistrictwiseSurveyResponse.Add(objDistrictwiseSurveyResponse);
                        //    Geo_coordinate_list.Add(objResponseGeoCoordinatest);
                        //}
                    }
                }

                objReducedSurveyResponselist = objSurveyResponselist.Where(c => !string.IsNullOrEmpty(c.StateName)).Select(e => new ReducedStatewiseSurveyResponse { SurveyID = e.SurveyID, SurveyName = e.SurveyName, ResponseCount = e.ResponseCount, StateID = e.StateID, StateName = e.StateName, ResponseStartDate = e.ResponseStartDate, ResponseEndDate = e.ResponseEndDate, IsBeneficiarySurvey = e.IsBeneficiarySurvey, BeneficiaryName = e.BeneficiaryName }).ToList();

                objReducedDistrictwiseResponselist = objSurveyDistrictwiseSurveyResponse.Where(c => !string.IsNullOrEmpty(c.DistrictName)).Select(e => new ReducedDistrictwiseSharedSurveyResponse { SurveyID = e.SurveyID, SurveyName = e.SurveyName, ResponseCount = e.ResponseCount, DistrictID = e.DistrictID, DistrictName = e.DistrictName, ResponseStartDate = e.ResponseStartDate, ResponseEndDate = e.ResponseEndDate, IsBeneficiarySurvey = e.IsBeneficiarySurvey, BeneficiaryName = e.BeneficiaryName, StateName = e.StateName }).ToList();

            }

            var objgroupedList =
                        (from i in objReducedSurveyResponselist
                         group i by new { i.StateID, i.StateName, i.SurveyID, i.SurveyName, i.IsBeneficiarySurvey, i.ResponseCount, i.ResponseStartDate, i.ResponseEndDate } into g
                         select new { g.Key, count = g.Count(), BeneficiaryName = (g.Select(kvp => kvp.BeneficiaryName).ToString() == "" ? "" : string.Join(",", g.Select(kvp => kvp.BeneficiaryName))) }).ToList();

            List<MapStateCollection> lstMapStateCollection = new List<MapStateCollection>();

            if (objgroupedList != null && objgroupedList.Count > 0)
            {
                foreach (var itm in objgroupedList)
                {
                    MapStateCollection objReducedStatewiseSurveyResponse = new MapStateCollection();
                    objReducedStatewiseSurveyResponse.StateName = itm.Key.StateName;
                    objReducedStatewiseSurveyResponse.SurveyID = itm.Key.SurveyID;
                    lstMapStateCollection.Add(objReducedStatewiseSurveyResponse);
                }
            }

            var objgroupedListDT =
                        (from i in objReducedDistrictwiseResponselist
                         group i by new { i.DistrictID, i.DistrictName, i.SurveyID, i.SurveyName, i.IsBeneficiarySurvey, i.ResponseCount, i.ResponseStartDate, i.ResponseEndDate, i.StateName } into g
                         select new { g.Key, count = g.Count(), BeneficiaryName = (g.Select(kvp => kvp.BeneficiaryName).ToString() == "" ? "" : string.Join(",", g.Select(kvp => kvp.BeneficiaryName))) }).ToList();


            List<MapDistrictCollection> lstReducedDistrictwiseSurveyResponse = new List<MapDistrictCollection>();

            if (objgroupedList != null && objgroupedList.Count > 0)
            {
                foreach (var itm in objgroupedListDT)
                {
                    MapDistrictCollection objReducedStatewiseSurveyResponse = new MapDistrictCollection();
                    objReducedStatewiseSurveyResponse.DistrictName = itm.Key.DistrictName.Trim();
                    objReducedStatewiseSurveyResponse.StateName = itm.Key.StateName.Trim();
                    objReducedStatewiseSurveyResponse.SurveyID = SurveyID;
                    lstReducedDistrictwiseSurveyResponse.Add(objReducedStatewiseSurveyResponse);
                }
            }

            MapShareVersionList objMapShareVersionList = new MapShareVersionList();
            objMapShareVersionList.lstState = new List<MapStateCollection>();
            objMapShareVersionList.lstState = lstMapStateCollection;

            objMapShareVersionList.lstDistricts = new List<MapDistrictCollection>();
            objMapShareVersionList.lstDistricts = lstReducedDistrictwiseSurveyResponse;

            objMapShareVersionList.lstDistrictCordinates = new List<MapDistrictCordinatesCollection>();
            objMapShareVersionList.lstDistrictCordinates = Geo_coordinate_list;

            return objMapShareVersionList;
        }


        public List<MapDistrictCollection> GetShareVersionDistrictWiseSurveyResponse(int SurveyID, string StateName)
        {

            string status = "false";
            MapViewDAL objMapViewDAL = new MapViewDAL();
            List<DistrictwiseSurveyResponse> objSurveyResponselist = new List<DistrictwiseSurveyResponse>();
            objSurveyResponselist = objMapViewDAL.GetDistrictWiseSurveyResponse(SurveyID);
            List<ReducedDistrictwiseSurveyResponse> objReducedDistrictwiseResponselist = new List<ReducedDistrictwiseSurveyResponse>();
            if (objSurveyResponselist != null)
            {
                status = "true";

                foreach (DistrictwiseSurveyResponse item in objSurveyResponselist)
                {
                    item.DistrictID = 0;
                    item.DistrictName = "";

                    if (CommonActivity.IsValidlatitudeAndlongitude(item.Answer))
                    {
                        string geoLocation = CommonActivity.ReverseGeoCoding(item.Answer);
                        if (!string.IsNullOrEmpty(geoLocation))
                        {

                            foreach (var myString in geoLocation.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (myString.Length > 0)
                                {
                                    if (myString.ToString().Contains("district"))
                                    {
                                        var indexOfColon = myString.ToString().IndexOf(":");
                                        var distname = myString.ToString().Substring(indexOfColon + 1);
                                        item.DistrictName = distname;
                                    }
                                    if (myString.ToString().Contains("state"))
                                    {
                                        var indexOfColon = myString.ToString().IndexOf(":");
                                        var statename = myString.ToString().Substring(indexOfColon + 1);
                                        item.StateName = statename;
                                    }

                                }
                            }
                        }

                    }

                }

                objReducedDistrictwiseResponselist = objSurveyResponselist.Where(c => !string.IsNullOrEmpty(c.DistrictName)).Where(d => d.StateName.Trim().ToLower() == StateName.Trim().ToLower()).Select(e => new ReducedDistrictwiseSurveyResponse { SurveyID = e.SurveyID, SurveyName = e.SurveyName, ResponseCount = e.ResponseCount, DistrictID = e.DistrictID, DistrictName = e.DistrictName, ResponseStartDate = e.ResponseStartDate, ResponseEndDate = e.ResponseEndDate, IsBeneficiarySurvey = e.IsBeneficiarySurvey, BeneficiaryName = e.BeneficiaryName }).ToList();

                foreach (ReducedDistrictwiseSurveyResponse item in objReducedDistrictwiseResponselist)
                {
                    item.ResponseCount = objReducedDistrictwiseResponselist.Where(temp => temp.DistrictName.Equals(item.DistrictName))
                    .Select(temp => temp).Count();
                }
            }

            var objgroupedList =
                        (from i in objReducedDistrictwiseResponselist
                         group i by new { i.DistrictID, i.DistrictName, i.SurveyID, i.SurveyName, i.IsBeneficiarySurvey, i.ResponseCount, i.ResponseStartDate, i.ResponseEndDate } into g
                         select new { g.Key, count = g.Count(), BeneficiaryName = (g.Select(kvp => kvp.BeneficiaryName).ToString() == "" ? "" : string.Join(",", g.Select(kvp => kvp.BeneficiaryName))) }).ToList();


            List<MapDistrictCollection> lstReducedDistrictwiseSurveyResponse = new List<MapDistrictCollection>();

            if (objgroupedList != null && objgroupedList.Count > 0)
            {
                foreach (var itm in objgroupedList)
                {
                    MapDistrictCollection objReducedStatewiseSurveyResponse = new MapDistrictCollection();
                    objReducedStatewiseSurveyResponse.DistrictName = itm.Key.DistrictName.Trim();
                    objReducedStatewiseSurveyResponse.StateName = StateName.Trim();
                    objReducedStatewiseSurveyResponse.SurveyID = itm.Key.SurveyID;
                    lstReducedDistrictwiseSurveyResponse.Add(objReducedStatewiseSurveyResponse);
                }
            }
            return lstReducedDistrictwiseSurveyResponse;
        }

        public List<MapDistrictCordinatesCollection> GetShareVersionResponseGeoCoordinates(int SurveyID, string DistrictName)
        {
            string status = "false";
            MapViewDAL objMapViewDAL = new MapViewDAL();

            List<SurveyResponseGPSLocation> gpsLocationList = new List<SurveyResponseGPSLocation>();
            gpsLocationList = new MapViewDAL().GetSurveyResponseLocation(SurveyID);

            List<DistrictwiseSurveyResponse> objSurveyResponselist = new List<DistrictwiseSurveyResponse>();
            List<MapDistrictCordinatesCollection> Geo_coordinate_list = new List<MapDistrictCordinatesCollection>();
            MapDistrictCordinatesCollection objResponseGeoCoordinatest = new MapDistrictCordinatesCollection();
            objSurveyResponselist = objMapViewDAL.GetResponseGeoCoordinate(SurveyID);
            if (objSurveyResponselist.Count > 0)
            {
                status = "true";

                foreach (DistrictwiseSurveyResponse item in objSurveyResponselist)
                {
                    if (CommonActivity.IsValidlatitudeAndlongitude(item.Answer))
                    {
                        List<SurveyResponseGPSLocation> objSurveyResponselist1 = new List<SurveyResponseGPSLocation>();
                        objSurveyResponselist1 = gpsLocationList.Where(x => x.SurveyID == SurveyID && x.GPS.Trim() == item.Answer.Trim()).ToList();

                        if (objSurveyResponselist1 != null && objSurveyResponselist1.Count > 0 && objSurveyResponselist1[0].District != null)
                        {
                            objResponseGeoCoordinatest = new MapDistrictCordinatesCollection();
                            objResponseGeoCoordinatest.SurveyID = item.SurveyID;
                            objResponseGeoCoordinatest.Geo_coordinates = item.Answer;
                            objResponseGeoCoordinatest.DistrictName = objSurveyResponselist1[0].District.Trim();
                            Geo_coordinate_list.Add(objResponseGeoCoordinatest);
                        }

                        //string geoLocation = CommonActivity.ReverseGeoCoding(item.Answer);
                        //if (!string.IsNullOrEmpty(geoLocation))
                        //{
                        //    if (geoLocation.ToUpper().Contains(DistrictName.ToUpper()))
                        //    {
                        //        objResponseGeoCoordinatest = new MapDistrictCordinatesCollection();
                        //        objResponseGeoCoordinatest.SurveyID = item.SurveyID;
                        //        objResponseGeoCoordinatest.Geo_coordinates = item.Answer;
                        //        objResponseGeoCoordinatest.DistrictName = DistrictName.Trim();
                        //        Geo_coordinate_list.Add(objResponseGeoCoordinatest);
                        //    }
                        //}
                    }
                }
            }

            return Geo_coordinate_list;
        }

        [HttpPost]
        public IActionResult SaveSurveyGPSBYSurveyID(int SurveyID)
        {
            bool status = new MapViewDAL().SaveSurveyGPSBYSurveyID(SurveyID);
            return Json(new { data = status, status = "success" }, new JsonSerializerSettings());
        }
    }
}
