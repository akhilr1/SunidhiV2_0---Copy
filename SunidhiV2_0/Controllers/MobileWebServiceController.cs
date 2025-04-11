using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
using SunidhiV2_0.Models;
using SunidhiV2_0.Code.DAL;
using SunidhiV2_0.Code.Common;

namespace SunidhiV2_0.Controllers
{
    public class MobileWebServiceController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public MobileWebServiceController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost]
        public IActionResult SyncFromServer(int UserID, int SyncServerID, int WorkspaceID, string LastSyncDatetime = "")
        {
            ListSyncItem objListSyncItem = new ListSyncItem();
            MobileWebServiceDAL objMobService = new MobileWebServiceDAL();
            MyTaskTestDAL objMyTaskTestDAL = new MyTaskTestDAL();
            List<MyTask> lstMyTask = new List<MyTask>();
            List<CompletedTask> lstMyCompletedTask = new List<CompletedTask>();
            List<TaskAuditLog> lstMyTaskAuditLog = new List<TaskAuditLog>();
            List<UpdatedTaskList> lstUpdatedTaskList = new List<UpdatedTaskList>();
            List<SurveyDataCollection> lstSurveyDataCollection = new List<SurveyDataCollection>();
            var LastUpdatedDatetime = string.Empty;

            if (string.IsNullOrEmpty(LastSyncDatetime))
            {
                LastSyncDatetime = "";
            }

            // lstMyTask = objMobService.SyncFromServer(UserID, SyncServerID, WorkspaceID).ToList();

            objListSyncItem = objMyTaskTestDAL.GetProjectAccountsByID(UserID, SyncServerID, WorkspaceID, LastSyncDatetime);

            lstMyCompletedTask = null;//objMobService.GetMyCompletedTaskList(UserID, WorkspaceID, SyncServerID, LastSyncDatetime).ToList();
            lstMyTaskAuditLog = null; //objMobService.GetTaskAuditLog(UserID, LastSyncDatetime).ToList();
            lstUpdatedTaskList = null;// objMobService.GetUpdatedTaskList(UserID, SyncServerID, LastSyncDatetime, WorkspaceID).ToList();

            if(SyncServerID >0)
            {
                lstSurveyDataCollection = objMyTaskTestDAL.GetResubmittedSurvey(SyncServerID, UserID);
            }

            if (objListSyncItem != null && objListSyncItem.lstMyTaskList != null && objListSyncItem.lstMyTaskList.Count > 0)
            {
                LastUpdatedDatetime = objListSyncItem.lstMyTaskList[0].TaskSubmittedDate.ToString("yyyy-MM-dd hh:mm:ss");
            }

            return Json(new { status = "success", data = new { MyTaskList = objListSyncItem, MyCompletedTask = lstMyCompletedTask, MyTaskAuditLog = lstMyTaskAuditLog, UpdatedTaskList = lstUpdatedTaskList, LastSyncDatetime = LastUpdatedDatetime, SurveyRejectedDataCollection  = lstSurveyDataCollection } }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult SyncTaskToServer(List<MyTaskSyncToServerData> lstMyTaskSyncToServerData, int loggedUserID)//, List<Document> lstBenRegistration, List<Document> lstSurveyResponse)
        {
            var retStatus = "";
            int id = 1;
            int loggedUserId = loggedUserID;
            var claimlist = User.Claims.ToList();
            List<MyTaskSyncToServerStatus> lstMyTaskSyncToServerStatus = new List<MyTaskSyncToServerStatus>();
            var Files = Request.Form.Files;
            int Filecount = 0;

            int syncDBID = 0;
            if (claimlist.Count > 0)
            {
                loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            }
            //int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);

            if (lstMyTaskSyncToServerData != null && lstMyTaskSyncToServerData.Count() > 0)
            {
                foreach (var item in lstMyTaskSyncToServerData)
                {
                    MobileWebServiceDAL objMobService = new MobileWebServiceDAL();
                    syncDBID = objMobService.GetSyncDBIDByClientGuid(item.ClientGuid);

                    if (syncDBID <= 0)
                    {
                        if (item.Type == "Survey")
                        {
                            id = objMobService.SyncToServer_Survey(item.WFLevel, item.IsBeneficiaryLinked, item.ClientData, item.Periodicity, loggedUserId, item.ClientGuid);

                            if (Files != null && Files.Count > 0)
                            {
                                if (item.lstBenRegistration != null && item.lstBenRegistration.Count() > 0)
                                {
                                    for (int benitem = 0; benitem < item.lstBenRegistration.Count; benitem++)
                                    {
                                        var itemFiles = Files[Filecount];

                                        Console.WriteLine(Path.DirectorySeparatorChar);

                                        if (itemFiles != null && itemFiles.Name.Contains("lstBenRegistration") && !string.IsNullOrEmpty(item.lstBenRegistration[benitem].DocumentName))
                                        {
                                            string uploadPath = _hostingEnvironment.WebRootPath + "/Upload/BeneficiaryRegistration/" + item.lstBenRegistration[benitem].ProjectId.ToString() + "/";
                                            if (!Directory.Exists(uploadPath))
                                                Directory.CreateDirectory(uploadPath);

                                            using (var fileStream = new FileStream((uploadPath + "/" + item.lstBenRegistration[benitem].DocumentName), FileMode.Create))
                                            {
                                                itemFiles.CopyTo(fileStream);
                                                fileStream.Close();
                                            }
                                        }

                                        Filecount++;
                                    }
                                }


                                if (item.lstSurveyResponse != null && item.lstSurveyResponse.Count() > 0)
                                {
                                    for (int srvitem = 0; srvitem < item.lstSurveyResponse.Count; srvitem++)
                                    {
                                        var itemfileSurvey = Files[Filecount];

                                        if (itemfileSurvey != null && itemfileSurvey.Name.Contains("lstSurveyResponse") && !string.IsNullOrEmpty(item.lstSurveyResponse[srvitem].DocumentName))
                                        {
                                            string uploadPath = _hostingEnvironment.WebRootPath + "/Upload/SureyQuestionnaire/" + item.lstSurveyResponse[srvitem].ProjectId.ToString() + "/";
                                            if (!Directory.Exists(uploadPath))
                                                Directory.CreateDirectory(uploadPath);

                                            using (var fileStream = new FileStream((uploadPath + "/" + item.lstSurveyResponse[srvitem].DocumentName), FileMode.Create))
                                            {
                                                itemfileSurvey.CopyTo(fileStream);
                                                fileStream.Close();
                                            }
                                        }

                                        Filecount++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            id = objMobService.SyncToServer(item.Type, item.WFLevel, item.IsBeneficiaryLinked, item.ClientData, item.Periodicity, loggedUserId);
                        }

                        retStatus = id > 0 ? "success" : "Failed";
                    }
                    else
                    {
                        id = syncDBID;
                        retStatus = "success";
                    }

                    MyTaskSyncToServerStatus objMyTaskSyncToServerStatus = new MyTaskSyncToServerStatus();
                    objMyTaskSyncToServerStatus.ClientGuid = item.ClientGuid;
                    objMyTaskSyncToServerStatus.TaskSyncID = id;
                    objMyTaskSyncToServerStatus.Status = retStatus;
                    lstMyTaskSyncToServerStatus.Add(objMyTaskSyncToServerStatus);
                }
            }

            //try
            //{


            //    if (Files != null && Files.Count > 0)
            //    {
            //        if (lstBenRegistration != null && lstBenRegistration.Count() > 0)
            //        {
            //            for (int benitem = 0; benitem < lstBenRegistration.Count; benitem++)
            //            {
            //                var item = Files[Filecount];

            //                if (item != null && item.Name.Contains("lstBenRegistration") && !string.IsNullOrEmpty(lstBenRegistration[benitem].DocumentName))
            //                {
            //                    string uploadPath = _hostingEnvironment.WebRootPath + "\\Upload\\BeneficiaryRegistration\\" + lstBenRegistration[benitem].ProjectId.ToString() + "\\";
            //                    if (!Directory.Exists(uploadPath))
            //                        Directory.CreateDirectory(uploadPath);

            //                    using (var fileStream = new FileStream((uploadPath + "\\" + lstBenRegistration[benitem].DocumentName), FileMode.Create))
            //                    {
            //                        item.CopyTo(fileStream);
            //                        fileStream.Close();
            //                    }
            //                }

            //                Filecount++;
            //            }

            //            //foreach (var benitem in lstBenRegistration)
            //            //{
            //            //    foreach (var item in Files)
            //            //    {
            //            //        if (benitem.OriginalFileName.ToLower() == item.FileName.ToLower() && !string.IsNullOrEmpty(benitem.DocumentName))
            //            //        {
            //            //            string uploadPath = _hostingEnvironment.WebRootPath + "\\Upload\\BeneficiaryRegistration\\" + benitem.ProjectId.ToString() + "\\";
            //            //            if (!Directory.Exists(uploadPath))
            //            //                Directory.CreateDirectory(uploadPath);

            //            //            using (var fileStream = new FileStream((uploadPath + "\\" + benitem.DocumentName), FileMode.Create))
            //            //            {
            //            //                item.CopyTo(fileStream);
            //            //                fileStream.Close();
            //            //            }
            //            //        }
            //            //    }
            //            //}
            //        }

            //        if (lstSurveyResponse != null && lstSurveyResponse.Count() > 0)
            //        {
            //            for (int srvitem = 0; srvitem < lstSurveyResponse.Count; srvitem++)
            //            {
            //                var item = Files[Filecount];

            //                if (item != null && item.Name.Contains("lstSurveyResponse") && !string.IsNullOrEmpty(lstSurveyResponse[srvitem].DocumentName))
            //                {
            //                    string uploadPath = _hostingEnvironment.WebRootPath + "\\Upload\\SureyQuestionnaire\\" + lstSurveyResponse[srvitem].ProjectId.ToString() + "\\";
            //                    if (!Directory.Exists(uploadPath))
            //                        Directory.CreateDirectory(uploadPath);

            //                    using (var fileStream = new FileStream((uploadPath + "\\" + lstSurveyResponse[srvitem].DocumentName), FileMode.Create))
            //                    {
            //                        item.CopyTo(fileStream);
            //                        fileStream.Close();
            //                    }
            //                }

            //                Filecount++;
            //            }

            //            //foreach (var srvitem in lstSurveyResponse)
            //            //{
            //            //    foreach (var item in Files)
            //            //    {
            //            //        if (srvitem.OriginalFileName.ToLower() == item.FileName.ToLower() && !string.IsNullOrEmpty(srvitem.DocumentName))
            //            //        {
            //            //            string uploadPath = _hostingEnvironment.WebRootPath + "\\Upload\\SureyQuestionnaire\\" + srvitem.ProjectId.ToString() + "\\";
            //            //            if (!Directory.Exists(uploadPath))
            //            //                Directory.CreateDirectory(uploadPath);

            //            //            using (var fileStream = new FileStream((uploadPath + "\\" + srvitem.DocumentName), FileMode.Create))
            //            //            {
            //            //                item.CopyTo(fileStream);
            //            //                fileStream.Close();
            //            //            }
            //            //        }
            //            //    }
            //            //}
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            return Json(new { status = "success", Data = lstMyTaskSyncToServerStatus }, new JsonSerializerSettings());
        }

        public ActionResult sendNotification(int userid)
        {
            Notification objNotification = new Notification();
            objNotification.UserId = userid; // assign user id here
            objNotification.Title = "Notification Title Here";
            objNotification.Subject = "Notification Body Here";
            new NotificationDAL().SendUserNotification(objNotification);
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        public ActionResult FunctionToCheckNetwork()
        {
            return Json(new { status = "success" }, new JsonSerializerSettings());
        }
    }

}