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
using Microsoft.Extensions.FileProviders;


namespace SunidhiV2_0.Controllers
{
    public class DocumentController : Controller
    {

        private IHostingEnvironment _hostingEnvironment;

        public DocumentController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost]
        public ActionResult UplaodFiles(Document objDoc = null)
        {

            string retStatus = "success";
            string docOrginalName = string.Empty;
            string docName = string.Empty;
            var Files = Request.Form.Files;

            if (Files != null && Files.Count > 0)
            {
                foreach (var item in Files)
                {
                    string uploadPath = _hostingEnvironment.WebRootPath + "/Upload/Documents/";
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    docOrginalName += docOrginalName != "" ? "," + item.FileName : item.FileName;
                    docName += docName != "" ? "," + $@"{Guid.NewGuid()}" + "." + item.FileName.Split('.')[1] : $@"{Guid.NewGuid()}" + "." + item.FileName.Split('.')[1];

                    using (var fileStream = new FileStream((uploadPath + "/" + docName), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        fileStream.Close();

                    }
                }
            }

            //========= Save to TbDocuments ======================
            if (objDoc != null)
            {
                int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                ProjectDAL objPrjDAL = new ProjectDAL();
                retStatus = objPrjDAL.SaveDocumentDetails(docName, docOrginalName, objDoc.DocumentSize, objDoc.DocumentTypeIcon,
                                    objDoc.DocumentType, objDoc.DocumentLastModifiedDate, objDoc.ProjectId, objDoc.UploadedFrom, objDoc.LinkedTo, objDoc.LinkedToID, objDoc.TaskSyncID, loggedUserId);

            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult DeleteFile(string docList= null)
        {
            string retStatus = "";
            string filePath = _hostingEnvironment.WebRootPath + "/Upload/Documents/";
            //string delPath = _hostingEnvironment.WebRootPath + "\\Upload\\TobeDeleted\\";
            var Files = Request.Form.Files;

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();

            if (docList != null)
            {
                List<Document> objList = JsonConvert.DeserializeObject<List<Document>>(docList);
                foreach (Document docItem in objList)
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filePath + docItem.UniqueDocumentName);
                    if (System.IO.File.Exists(filePath + docItem.UniqueDocumentName))
                    {
                        try
                        {
                            file.Delete();
                            //System.IO.File.Move(filePath + FileName, delPath + FileName);
                        }
                        catch (System.IO.IOException e)
                        {
                            retStatus = "Failed in deleting File..";
                            throw e;
                        }
                    }
                    // ======= Update to TbDocuments ======================
                    retStatus = objPrjDAL.DeleteDocument(docItem.DocumentID, loggedUserId);                    
                }
                   
            }
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            else
                return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RenameFile(int docID, string oldFileName,string newFileName)
        {

            string retStatus = "success";
            string filePath = _hostingEnvironment.WebRootPath + "/Upload/Documents/";
            newFileName += "." + oldFileName.Split('.')[1];

            var Files = Request.Form.Files;
            System.IO.FileInfo file = new System.IO.FileInfo(filePath + oldFileName);
           
            if (System.IO.File.Exists(filePath + oldFileName))
            {
                try
                {
                    //file.Delete();
                    System.IO.File.Move(filePath + oldFileName, filePath + newFileName);

                }
                catch (System.IO.IOException e)
                {
                    retStatus = "Failed in renaming File..";
                    throw e;
                }
            }
            // ======= Update to TbDocuments ======================

            int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
            ProjectDAL objPrjDAL = new ProjectDAL();
            retStatus = objPrjDAL.RenameDocument(docID, newFileName, loggedUserId);
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult UplaodQuestionnaireAnsFiles(int ProjectID, string FileName, Document objDoc = null)
        {

            string retStatus = "success";            
            var Files = Request.Form.Files;           
            if (Files != null && Files.Count > 0)
            {
                foreach (var item in Files)
                {
                    string uploadPath = _hostingEnvironment.WebRootPath + "/Upload/BeneficiaryRegistration/"+ProjectID.ToString()+"/";
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);                   
                    
                    using (var fileStream = new FileStream((uploadPath + "/" + FileName), FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                        fileStream.Close();

                    }
                }
            }
            if (objDoc != null)
            {
                int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                ProjectDAL objPrjDAL = new ProjectDAL();
                retStatus = objPrjDAL.SaveResponseDocumentDetails(FileName, objDoc.DocumentTypeIcon, ProjectID, objDoc.ResponseFrom, objDoc.UploadedFrom,
                            objDoc.LinkedTo, objDoc.LinkedToID, objDoc.TaskSyncID, loggedUserId);

            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult RemoveQuestionnaireFiles(int ProjectID,string docList = null)
        {
            string retStatus = "";
            string filePath = _hostingEnvironment.WebRootPath + "/Upload/BeneficiaryRegistration/"+ ProjectID.ToString()+"/";           
            var Files = Request.Form.Files;
           

            if (docList != null)
            {
                List<BeneficiaryRegistration> lstFiles = JsonConvert.DeserializeObject<List<BeneficiaryRegistration>>(docList);
               
                foreach (BeneficiaryRegistration ansItem in lstFiles)
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filePath + ansItem.Answer);
                    if (System.IO.File.Exists(filePath + ansItem.Answer))
                    {
                        try
                        {
                            file.Delete();

                        }
                        catch (System.IO.IOException e)
                        {
                            retStatus = "Failed in deleting File..";
                            throw e;
                        }
                    }
                }

            }
            if (retStatus != "")
                return Json(new { status = retStatus }, new JsonSerializerSettings());
            else
                return Json(new { status = "success" }, new JsonSerializerSettings());
        }

        [HttpPost]
        public ActionResult UploadSureyQuestionnaireFiles(int ProjectID, string FileName, Document objDoc = null)
        {

            string retStatus = "success";
            var Files = Request.Form.Files;
            if (Files != null && Files.Count > 0)
            {
                if (FileName != null && FileName!="")
                {

                    foreach (var item in Files)
                    {
                        string uploadPath = _hostingEnvironment.WebRootPath + "/Upload/SureyQuestionnaire/" + ProjectID.ToString() + "/";
                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);

                        using (var fileStream = new FileStream((uploadPath + "/" + FileName), FileMode.Create))
                        {
                            item.CopyTo(fileStream);
                            fileStream.Close();

                        }
                    }
                }
            }
            if (objDoc != null)
            {
                int loggedUserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID")?.Value);
                ProjectDAL objPrjDAL = new ProjectDAL();
                retStatus = objPrjDAL.SaveResponseDocumentDetails(FileName, objDoc.DocumentTypeIcon, ProjectID, objDoc.ResponseFrom,objDoc.UploadedFrom,
                            objDoc.LinkedTo, objDoc.LinkedToID, objDoc.TaskSyncID, loggedUserId);

            }
            return Json(new { status = retStatus }, new JsonSerializerSettings());
        }
    }
}