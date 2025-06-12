using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.Security;
using System.Data;
using IIIBL;
using System.Configuration;
using System.IO;
using System.Text;
using System.Reflection;
using System.Drawing;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1.Ocsp;

//CSS Route
namespace IIIRegistrationPortal2.Controllers
{
    public class CandidatesController : Controller
    {
        #region Scorecard download
        //Ok
        [HttpGet]
        public ActionResult Scorecard()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        //OK
        [HttpPost]
        public JsonResult Scorecard(String Dummy = "")
        {
            String ImagePath = Server.MapPath("~/Images/logo_III.jpg");
            Boolean Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.HSTCPrinter objHSTCPrinter = null;

            String URN = String.Empty;
            DateTime DOB = DateTime.MinValue;
            DateTime ExamDate = DateTime.MinValue;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]).Trim();

                String _dummy = Convert.ToString(Request.Form["txtDOB"]);
                if (_dummy.Trim() == String.Empty)
                {
                    DOB = DateTime.MinValue;
                }
                else
                {
                    DOB = Convert.ToDateTime(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["txtExamDate"]);
                if (_dummy.Trim() == String.Empty)
                {
                    ExamDate = DateTime.MinValue;
                }
                else
                {
                    ExamDate = Convert.ToDateTime(_dummy);
                }
                if (DOB == DateTime.MinValue && ExamDate == DateTime.MinValue)
                {
                    throw new Exception("Invalid Date Values");
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Status)
            {
                try
                {
                    String OutputURL = String.Format("../Downloads/SC_{0}.pdf", URN);
                    String OutputPath = Server.MapPath("~/Downloads");

                    objHSTCPrinter = new HSTCPrinter();
                    String Results = objHSTCPrinter.PrintScorecard(PortalApplication.ConnectionString, URN, DOB, ExamDate, 1, ImagePath, OutputPath);
                    if (Results == "SUCCESS")
                    {
                        Status = true;
                        Message = "";
                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputURL);
                        s = HelperUtilities.ToJSON(Status, Message, null, KVPair);
                    }
                    else if (Results == "NO_DATA_FOUND")
                    {
                        Status = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Status, Message);
                    }
                    else
                    {
                        Status = false;
                        Message = Results;
                        s = HelperUtilities.ToJSON(Status, Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
                finally
                {
                    objHSTCPrinter = null;
                }
            }

            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public JsonResult Scorecard2()
        {
            String ImagePath = Server.MapPath("~/Images/logo_III.jpg");
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.HSTCPrinter objHSTCPrinter = null;
            try
            {
                String URN = Convert.ToString(Request.Form["txtURN"]).Trim();
                DateTime DOB = DateTime.MinValue; // Convert.ToDateTime(Request.Form["txtDOB"]);
                DateTime ExamDate = DateTime.MinValue;

                String OutputURL = String.Format("../Downloads/SC_{0}.pdf", URN);
                String OutputPath = Server.MapPath("~/Downloads");

                objHSTCPrinter = new HSTCPrinter();
                String Results = objHSTCPrinter.PrintScorecard(PortalApplication.ConnectionString, URN, DOB, ExamDate, 2, ImagePath, OutputPath);
                if (Results == "SUCCESS")
                {
                    Status = true;
                    Message = "";
                    KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputURL);
                    s = HelperUtilities.ToJSON(Status, Message, null, KVPair);
                }
                else if (Results == "NO_DATA_FOUND")
                {
                    Status = false;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
                else
                {
                    Status = false;
                    Message = Results;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Status = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Status, Message);
            }
            finally
            {
                objHSTCPrinter = null;
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        #endregion

        #region Hallticket download
        //OK
        [HttpGet]
        public ActionResult HallTicket()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        //OK
        [HttpPost]
        public ActionResult HallTicket(String Dummy = "")
        {
            String ImagePath = Server.MapPath("~/Images/logo_III.jpg");
            Boolean Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.HSTCPrinter objHSTCPrinter = null;

            String URN = String.Empty;
            DateTime DOB = DateTime.MinValue;
            DateTime ExamDate = DateTime.MinValue;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]).Trim();

                String _dummy = Convert.ToString(Request.Form["txtDOB"]);
                if (_dummy.Trim() == String.Empty)
                {
                    DOB = DateTime.MinValue;
                }
                else
                {
                    DOB = Convert.ToDateTime(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["txtExamDate"]);
                if (_dummy.Trim() == String.Empty)
                {
                    ExamDate = DateTime.MinValue;
                }
                else
                {
                    ExamDate = Convert.ToDateTime(_dummy);
                }
                if (DOB == DateTime.MinValue && ExamDate == DateTime.MinValue)
                {
                    throw new Exception("Invalid Date Values");
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Status)
            {
                try
                {
                    String OutputURL = String.Format("../Downloads/HT_{0}.pdf", URN);
                    //Uri uri = new Uri(OutputURL);
                    String OutputPath = Server.MapPath("~/Downloads");
                    String FooterFile1 = Server.MapPath("../App_Data/HTFooters.txt");
                    String FooterFile2 = Server.MapPath("../App_Data/HTFooters2.txt");
                    String FooterFileRP = Server.MapPath("../App_Data/HTFooters_RP.txt");

                    objHSTCPrinter = new HSTCPrinter();
                    String Results = objHSTCPrinter.PrintHallTicket(PortalApplication.ConnectionString, URN, DOB, ExamDate, 1, ImagePath, OutputPath, FooterFile1, FooterFile2, FooterFileRP);
                    if (Results == "SUCCESS")
                    {
                        Status = true;
                        Message = "";
                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputURL);
                        s = HelperUtilities.ToJSON(Status, Message, null, KVPair);
                    }
                    else if (Results == "NO_DATA_FOUND")
                    {
                        Status = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Status, Message);
                    }
                    else
                    {
                        Status = false;
                        Message = Results;
                        s = HelperUtilities.ToJSON(Status, Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
                finally
                {
                    objHSTCPrinter = null;
                }
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public ActionResult HallTicket2()
        {
            String ImagePath = Server.MapPath("~/Images/logo_III.jpg");
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.HSTCPrinter objHSTCPrinter = null;
            try
            {
                String URN = Convert.ToString(Request.Form["txtURN"]).Trim();
                DateTime DOB = DateTime.MinValue; // Convert.ToDateTime(Request.Form["txtDOB"]);
                DateTime ExamDate = DateTime.MinValue;

                String OutputURL = String.Format("../Downloads/HT_{0}.pdf", URN);
                //Uri uri = new Uri(OutputURL);
                String OutputPath = Server.MapPath("~/Downloads");

                String FooterFile1 = Server.MapPath("../App_Data/HTFooters.txt");
                String FooterFile2 = Server.MapPath("../App_Data/HTFooters2.txt");
                String FooterFileRP = Server.MapPath("../App_Data/HTFooters_RP.txt");

                objHSTCPrinter = new HSTCPrinter();
                String Results = objHSTCPrinter.PrintHallTicket(PortalApplication.ConnectionString, URN, DOB, ExamDate, 2, ImagePath, OutputPath, FooterFile1, FooterFile2, FooterFileRP);
                if (Results == "SUCCESS")
                {
                    Status = true;
                    Message = "";
                    KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputURL);
                    s = HelperUtilities.ToJSON(Status, Message, null, KVPair);
                }
                else if (Results == "NO_DATA_FOUND")
                {
                    Status = false;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
                else
                {
                    Status = false;
                    Message = Results;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Status = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Status, Message);
            }
            finally
            {
                objHSTCPrinter = null;
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        #endregion

        #region Partial URN Modification 
        [HttpGet]
        public ActionResult Modifications()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpPost]
        public JsonResult Modifications(String URN, DateTime DOB)
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            try
            {
                objURN = new URN();
                objDataSet = objURN.GetDataForPartialModification(PortalApplication.ConnectionString, URN, DOB, out Message, -1);
                if (Message.Trim() == String.Empty)
                {
                    Success = true;
                    objDataTable = objDataSet.Tables[0];
                    objDataTable.Columns.Add("Photo", typeof(String));
                    objDataTable.Columns.Add("Sign", typeof(String));
                    DataRow dataRow = objDataTable.Rows[0];
                    dataRow["Photo"] = Convert.ToBase64String((byte[])dataRow["imgPhoto"]);
                    dataRow["Sign"] = Convert.ToBase64String((byte[])dataRow["imgSign"]);
                }
                else 
                {
                    Success = false;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        
        [HttpPost]
        public JsonResult SaveModifications()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            Exception DBException = null;
            try
            {
                int OldExamCenterId = -1;
                int OldLanguageId = -1;
                int NewExamCenterId = -1;
                int NewLanguageId = -1;

                String URN = Convert.ToString(Request.Form["hdnURN"]);
                if (Request.Form["hdnExamCenterId"] != String.Empty)
                {
                    OldExamCenterId = Convert.ToInt32(Request.Form["hdnExamCenterId"]);
                }
                if (Request.Form["hdnExamCenterId"] != String.Empty)
                {
                    OldLanguageId = Convert.ToInt32(Request.Form["hdnExamLanguageId"]);
                }
                if (Request.Form["cboExamCenters"] != String.Empty)
                {
                    NewExamCenterId = Convert.ToInt32(Request.Form["cboExamCenters"]);
                }
                if (Request.Form["cboLanguage"] != String.Empty)
                {
                    NewLanguageId = Convert.ToInt32(Request.Form["cboLanguage"]);
                }
                Byte[] Photo = null;
                Byte[] Sign = null;
                /*
                MemoryStream ms = null;
                if (Request.Files["txtFilePhoto"].FileName == String.Empty && Request.Files["txtFilePhoto"].ContentLength == 0)
                {
                }
                else
                {
                    ms = new MemoryStream((Int32)Request.Files["txtFilePhoto"].InputStream.Length);
                    Request.Files["txtFilePhoto"].InputStream.CopyTo(ms);
                    Photo = ms.ToArray();
                    ms = null;
                }

                if (Request.Files["txtFileSign"].FileName == String.Empty && Request.Files["txtFileSign"].ContentLength == 0)
                {
                }
                else
                {
                    ms = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                    Request.Files["txtFileSign"].InputStream.CopyTo(ms);
                    Sign = ms.ToArray();
                    ms = null;
                }
                */
                
                String AllowWhatsappMessage = String.Empty;
                String WhatsappNumber = String.Empty;
                if (Request.Form["cboAllowWhatsappMessage"] != String.Empty)
                {
                    AllowWhatsappMessage = Request.Form["cboAllowWhatsappMessage"];
                }

                if (Request.Form["txtWhatsAppNo"] != String.Empty)
                {
                    WhatsappNumber = Request.Form["txtWhatsAppNo"];
                }

                objURN = new IIIBL.URN();
                if (PortalApplication.IntegrationMode == "CSS" )
                {
                    objURN.SaveModification(PortalApplication.ConnectionString, URN, NewLanguageId, NewExamCenterId, Photo, Sign, AllowWhatsappMessage, WhatsappNumber, out Status, out Message, -1, -1);
                }
                else // OAIMS
                {
                    String BasePathP = ConfigurationManager.AppSettings.Get("AIMSPhotoFolderBasePath");
                    String BasePathS = ConfigurationManager.AppSettings.Get("AIMSSignFolderBasePath");
                    String ShareUser = ConfigurationManager.AppSettings.Get("AIMSImageFolderUser");
                    String SharePass = ConfigurationManager.AppSettings.Get("AIMSImageFolderPassword");

                    objURN.SaveModificationOAIMS(PortalApplication.ConnectionString, PortalApplication.OAIMSConnectionString, URN, NewLanguageId, NewExamCenterId, Photo, Sign,AllowWhatsappMessage, WhatsappNumber, out Status, out Message, BasePathP, BasePathS, ShareUser, SharePass, -1, -1,out DBException);
                }

                if (DBException != null )
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, DBException, Request.Form);
                }

                Success = Status == "SUCCESS";
                if (Success)
                {
                    Message = CommonMessages.DATA_SAVE_SUCCESS;
                }
                else
                {
                    Message = CommonMessages.DATA_SAVE_FAIL + "." + Message;
                }
                s = HelperUtilities.ToJSON(Success, Message, null);

            }
            catch(Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        #endregion

        #region Partial URN Modification 
        [HttpGet]
        [AuthorizeExt]
        public ActionResult Modifications2()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult Modifications2(String URN, DateTime DOB)
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            try
            {
                objURN = new URN();
                objDataSet = objURN.GetDataForPartialModification(PortalApplication.ConnectionString, URN, DOB, out Message, PortalSession.InsurerUserID);
                if (Message.Trim() == String.Empty)
                {
                    Success = true;
                    objDataTable = objDataSet.Tables[0];
                    objDataTable.Columns.Add("Photo", typeof(String));
                    objDataTable.Columns.Add("Sign", typeof(String));
                    DataRow dataRow = objDataTable.Rows[0];
                    dataRow["Photo"] = Convert.ToBase64String((byte[])dataRow["imgPhoto"]);
                    dataRow["Sign"] = Convert.ToBase64String((byte[])dataRow["imgSign"]);
                }
                else
                {
                    Success = false;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [Authorize]
        public JsonResult SaveModifications2()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            Exception exOut = null;
            
            try
            {
                int OldExamCenterId = -1;
                int OldLanguageId = -1;
                int NewExamCenterId = -1;
                int NewLanguageId = -1;

                String URN = Convert.ToString(Request.Form["hdnURN"]);
                if (Request.Form["hdnExamCenterId"] != String.Empty)
                {
                    OldExamCenterId = Convert.ToInt32(Request.Form["hdnExamCenterId"]);
                }
                if (Request.Form["hdnExamCenterId"] != String.Empty)
                {
                    OldLanguageId = Convert.ToInt32(Request.Form["hdnExamLanguageId"]);
                }
                if (Request.Form["cboExamCenters"] != String.Empty)
                {
                    NewExamCenterId = Convert.ToInt32(Request.Form["cboExamCenters"]);
                }
                if (Request.Form["cboLanguage"] != String.Empty)
                {
                    NewLanguageId = Convert.ToInt32(Request.Form["cboLanguage"]);
                }
                Byte[] Photo = null;
                Byte[] Sign = null;
                MemoryStream ms = null;
                if (Request.Files["txtFilePhoto"].FileName == String.Empty && Request.Files["txtFilePhoto"].ContentLength == 0)
                {
                }
                else
                {
                    ms = new MemoryStream((Int32)Request.Files["txtFilePhoto"].InputStream.Length);
                    Request.Files["txtFilePhoto"].InputStream.CopyTo(ms);
                    Photo = ms.ToArray();
                    ms = null;
                }

                if (Request.Files["txtFileSign"].FileName == String.Empty && Request.Files["txtFileSign"].ContentLength == 0)
                {
                }
                else
                {
                    ms = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                    Request.Files["txtFileSign"].InputStream.CopyTo(ms);
                    Sign = ms.ToArray();
                    ms = null;
                }

                String AllowWhatsappMessage = String.Empty;
                String WhatsappNumber = String.Empty;
                if (Request.Form["cboAllowWhatsappMessage"] != String.Empty)
                {
                    AllowWhatsappMessage = Request.Form["cboAllowWhatsappMessage"];
                }

                if (Request.Form["txtWhatsAppNo"] != String.Empty)
                {
                    WhatsappNumber = Request.Form["txtWhatsAppNo"];
                }


                objURN = new IIIBL.URN();
                if (PortalApplication.IntegrationMode == "CSS" )
                {
                    objURN.SaveModification(PortalApplication.ConnectionString, URN, NewLanguageId, NewExamCenterId, Photo, Sign,AllowWhatsappMessage, WhatsappNumber, out Status, out Message, PortalSession.UserID, PortalSession.InsurerUserID);
                }
                else //OAIMS
                {
                    String BasePathP = ConfigurationManager.AppSettings.Get("AIMSPhotoFolderBasePath");
                    String BasePathS = ConfigurationManager.AppSettings.Get("AIMSSignFolderBasePath");
                    String ShareUser = ConfigurationManager.AppSettings.Get("AIMSImageFolderUser");
                    String SharePass = ConfigurationManager.AppSettings.Get("AIMSImageFolderPassword");

                    objURN.SaveModificationOAIMS(PortalApplication.ConnectionString, PortalApplication.OAIMSConnectionString, URN, NewLanguageId, NewExamCenterId, Photo, Sign, AllowWhatsappMessage, WhatsappNumber,out Status, out Message,BasePathP,BasePathS, ShareUser, SharePass, PortalSession.UserID, PortalSession.InsurerUserID, out exOut);
                }

                Success = Status == "SUCCESS";
                if (Success)
                {
                    Message = CommonMessages.DATA_SAVE_SUCCESS;
                }
                else
                {
                    Message = CommonMessages.DATA_SAVE_FAIL + "." + Message;
                    if (exOut != null)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, exOut, Request.Form);
                    }
                }
                s = HelperUtilities.ToJSON(Success, Message, null);

            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        #endregion
        
        #region PAN Lookup
        //OK
        [HttpGet]
        [AuthorizeExt]
        public ActionResult PANLookup()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        //OK
        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult PANLookup(String PAN)
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null; //To be taken from application 
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;

            try
            {
                objURN = new URN();
                objDataSet = objURN.GetURNForPAN(PortalApplication.ConnectionString, PAN);
                if (objDataSet != null && objDataSet.Tables.Count != 0)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult UnarchiveURN()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            //DataSet objDataSet = null;
            //DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = true;
            String Status = String.Empty;
            String Message = String.Empty;
            String URN = String.Empty;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Success)
            {
                try
                {
                    objURN = new URN();
                    Message = objURN.UnarchiveURN(PortalApplication.ConnectionString, URN );
                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = "URN restored successfully";
                    }
                    else
                    {
                        Success = false;
                        //Message = Message;
                    }
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    //objDataTable = null;
                    //objDataSet = null;
                    objURN = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        #endregion

        #region URNLookup
        //OK
        [HttpGet]
        [AuthorizeExt]
        public ActionResult URNLookup()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            ViewBag.IsPostback = false;
            return View();
        }

        //OK
        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult URNLookup(String dummy = "")
        {
            IIIBL.URN objURN = null; //To be taken from application 
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String URN = String.Empty;
            try
            {
                URN = Request.Form["txtURN"];
                objURN = new URN();
                objDataSet = objURN.GetURNDetails(PortalApplication.ConnectionString, URN, PortalSession.UserID);
                if (objDataSet != null && objDataSet.Tables.Count != 0)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count == 0)
                    {
                        ViewBag._STATUS_ = true;
                        ViewBag._MESSAGE_ = CommonMessages.NO_DATA_FOUND;
                        ViewBag._Data_ = null;
                    }
                    else
                    {
                        ViewBag._STATUS_ = true;
                        ViewBag._MESSAGE_ = String.Empty;
                        ViewBag._Data_ = objDataTable;
                    }
                }
                else
                {
                    ViewBag._STATUS_ = true;
                    ViewBag._MESSAGE_ = CommonMessages.NO_DATA_FOUND;
                    ViewBag._Data_ = null;
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                ViewBag._STATUS_ = false;
                ViewBag._MESSAGE_ = CommonMessages.ERROR_OCCURED;
                ViewBag._Data_ = null;
            }
            finally
            {
                objURN = null;
            }

            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            ViewBag.URN = URN;
            ViewBag.IsPostback = true;
            return View();
        }

        [HttpGet]
        [AuthorizeAJAX]
        public FileContentResult DownloadPhoto(String URN)
        {
            FileContentResult fileContentResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            try
            {
                objURN = new IIIBL.URN();
                objDataSet = objURN.GetPhotoAndSignature(PortalApplication.ConnectionString, URN);
                if (objDataSet != null && objDataSet.Tables.Count > 0)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        byte[] FileContents = (byte[])objDataTable.Rows[0]["imgApplicantPhoto"];
                        fileContentResult = new FileContentResult(FileContents, "image/jpg");
                        fileContentResult.FileDownloadName = "Photo_" + URN + ".jpg";

                       
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
            }
            finally
            {
                objURN = null;
                objDataTable = null;
                objDataSet = null;
            }
            return fileContentResult;
        }

        [HttpGet]
        [AuthorizeAJAX]
        public FileContentResult DownloadSign(String URN)
        {
            FileContentResult fileContentResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            try
            {
                objURN = new IIIBL.URN();
                objDataSet = objURN.GetPhotoAndSignature(PortalApplication.ConnectionString, URN);
                if (objDataSet != null && objDataSet.Tables.Count > 0)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        byte[] FileContents = (byte[])objDataTable.Rows[0]["imgApplicantSign"];
                        fileContentResult = new FileContentResult(FileContents, "image/jpg");
                        fileContentResult.FileDownloadName = "Sign_" + URN + ".jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
            }
            finally
            {
                objURN = null;
                objDataTable = null;
                objDataSet = null;
            }
            return fileContentResult;
        }
        #endregion

        #region URN creation
        [HttpGet]
        [AuthorizeExt]
        public ActionResult URNCreation()
        {
            if (PortalSession.RoleCode == "I")
            {
                ViewBag.Title = "Sponsorship Form";
            }
            else
            {
                ViewBag.Title = "Training  Registration Form";
            }

           

            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult URNCreation(String Dummy="")
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            DateTime dtApplicationDate = System.DateTime.Now.Date;
            String InsuranceType = String.Empty;
            String CORType = String.Empty;
            String Salutation = String.Empty;
            String ApplicantName = String.Empty;
            String FHName = String.Empty;
            String DOB = String.Empty;
            String Gender = String.Empty;
            Int32 Category = 0;
            String Area = String.Empty;
            Int32 Nationality = 0;
            String PAN = String.Empty;            
            String DrivingLicense = String.Empty;
            String Passport = String.Empty;
            String VoterId = String.Empty;
            String PhotoId = String.Empty;
            String BasicQualification = String.Empty;
            String BoardName = String.Empty;
            String RollNumber = String.Empty;
            DateTime YearOfPassing = System.DateTime.Now.Date;
            String ProfessionalQualification = String.Empty;
            String OtherQualification = String.Empty;
            String HouseNo_C = String.Empty;
            String StreetNo_C = String.Empty;
            String TownCity_C = String.Empty;
            Int32 StateId_C = 0;
            Int32 DistrictId_C = 0;
            Int32 Pincode_C = 0;

            String HouseNo_P = String.Empty;
            String StreetNo_P = String.Empty;
            String TownCity_P = String.Empty;
            Int32 StateId_P = 0;
            Int32 DistrictId_P = 0;
            Int32 Pincode_P = 0;

            String Landline = String.Empty;
            String Mobile = String.Empty;
            String WhatsAppNumber = String.Empty;
            String AllowWhatsAppMessage = String.Empty;
            Int32 TelemarketerId = -1;

            String Email = String.Empty;
            String CPEMail = String.Empty;
            String Occupation = String.Empty;
            String ExmployeeCode = String.Empty;
            Int32 Branch = -1;

            String ExamMode = String.Empty;
            String ExamBody = String.Empty;
            String ExamCenters = String.Empty;
            Int32 ExamCenter = 0;
            Int32 ExamLanguage = 0;
            Byte[] Sign = null;
            Byte[] Photo = null;
            Byte[] QualificationDoc = null;
            Byte[] BasicQualificationDoc = null;

            try
            {
                if ((Request.Form["chkDeclaration"]) == "on" || (Request.Form["chkDeclaration"]) == "true") //Declaration has been checked.
                {
                    String _dummy = Convert.ToString(Request.Form["txtApplicationDate"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        dtApplicationDate = DateTime.Now.Date;
                    }
                    else
                    {
                        dtApplicationDate = Convert.ToDateTime(_dummy);
                    }

                    //dtApplicationDate       = Convert.ToDateTime(Request.Form["txtApplicationDate"]);
                    InsuranceType = Convert.ToString(Request.Form["cboInsuranceCategory"]);
                    CORType = Convert.ToString(Request.Form["cboCORType"]);

                    Salutation = Convert.ToString(Request.Form["cboSalutation"]);
                    ApplicantName = Convert.ToString(Request.Form["txtApplicantName"]);
                    FHName = Convert.ToString(Request.Form["txtFHName"]);
                    DOB = Convert.ToString(Request.Form["txtDOB"]);
                    Gender = Convert.ToString(Request.Form["cboGender"]);
                    if (Gender == null || Gender == "")
                    {
                        switch (Salutation)
                        {
                            case "Mr.":
                                Gender = "M";
                                break;
                            case "Mrs.":
                            case "Ms.":
                                Gender = "F";
                                break;
                            //case "Dr.": // no need to handle this as gender wont be null in this case
                            case "Mx.":
                                Gender = "O";
                                break;
                            default:
                                Gender = "O";
                                break;
                        }
                    }

                    _dummy = Convert.ToString(Request.Form["cboCategory"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid Category");
                    }
                    else
                    {
                        Category = Convert.ToInt32(_dummy);
                    }

                    //Category         = Convert.ToInt32(Request.Form["cboCategory"]);
                    Area = Convert.ToString(Request.Form["cboArea"]);

                    _dummy = Convert.ToString(Request.Form["cboNationality"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid Nationality");
                    }
                    else
                    {
                        Nationality = Convert.ToInt32(_dummy);
                    }
                    //Nationality      = Convert.ToInt32(Request.Form["cboNationality"]);
                    PAN = Convert.ToString(Request.Form["txtPAN"]);                   

                    DrivingLicense = Convert.ToString(Request.Form["txtDrivingLic"]);
                    Passport = Convert.ToString(Request.Form["txtPassport"]);
                    VoterId = Convert.ToString(Request.Form["txtVoterId"]);
                    PhotoId = Convert.ToString(Request.Form["txtPhotoId"]);

                    MemoryStream ms = new MemoryStream((Int32)Request.Files["txtFilePhoto"].InputStream.Length);
                    Request.Files["txtFilePhoto"].InputStream.CopyTo(ms);
                    Photo = ms.ToArray();
                    ms = null;

                    ms = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                    Request.Files["txtFileSign"].InputStream.CopyTo(ms);
                    Sign = ms.ToArray();
                    ms = null;

                    BasicQualification = Convert.ToString(Request.Form["cboBasicQualification"]);
                    BoardName = Convert.ToString(Request.Form["txtBoardName"]);
                    RollNumber = Convert.ToString(Request.Form["txtRollnumber"]);
                    _dummy = Convert.ToString(Request.Form["txtYearofPassing"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtYearofPassing");
                    }
                    else
                    {
                        YearOfPassing = Convert.ToDateTime(_dummy);
                    }
                    //YearOfPassing               = Convert.ToDateTime(Request.Form["txtYearofPassing"]);
                    ProfessionalQualification = Convert.ToString(Request.Form["cboProfessionalQualification"]);
                    OtherQualification = Convert.ToString(Request.Form["txtOtherQualification"]);

                    /* Certificate upload for CA / WA / IMF / BR and COR type = 'PO' */
                    if (CORType == "PO")
                    {   //Basic Qualification
                        ms = new MemoryStream((Int32)Request.Files["txtBasicQualificationDoc"].InputStream.Length);
                        Request.Files["txtBasicQualificationDoc"].InputStream.CopyTo(ms);
                        BasicQualificationDoc = ms.ToArray();
                        ms = null;
                        //Professional Qualification
                        if (!ProfessionalQualification.In(new string[] { "NA" }))
                        {
                            ms = new MemoryStream((Int32)Request.Files["txtQualificationDoc"].InputStream.Length);
                            Request.Files["txtQualificationDoc"].InputStream.CopyTo(ms);
                            QualificationDoc = ms.ToArray();
                            ms = null;
                        }
                        else
                        {

                        }
                    }

                    /* End addition */


                    HouseNo_C = Convert.ToString(Request.Form["txtHouseNo_current"]);
                    StreetNo_C = Convert.ToString(Request.Form["txtStreet_current"]);
                    TownCity_C = Convert.ToString(Request.Form["txtCity_current"]);
                    _dummy = Convert.ToString(Request.Form["cboStates_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboStates_current");
                    }
                    else
                    {
                        StateId_C = Convert.ToInt32(_dummy);
                    }
                    //StateId_C             = Convert.ToInt32(Request.Form["cboStates_current"]);
                    _dummy = Convert.ToString(Request.Form["cboDistricts_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboDistricts_current");
                    }
                    else
                    {
                        DistrictId_C = Convert.ToInt32(_dummy);
                    }
                    //DistrictId_C          = Convert.ToInt32(Request.Form["cboDistricts_current"]);
                    _dummy = Convert.ToString(Request.Form["txtPincode_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtPincode_current");
                    }
                    else
                    {
                        Pincode_C = Convert.ToInt32(_dummy);
                    }
                    //Pincode_C             = Convert.ToInt32(Request.Form["txtPincode_current"]);

                    HouseNo_P = String.Empty;
                    StreetNo_P = String.Empty;
                    TownCity_P = String.Empty;
                    StateId_P = 0;
                    DistrictId_P = 0;
                    Pincode_P = 0;
                    if ((Request.Form["chkSameAsCurrent"]) == "on" || (Request.Form["chkSameAsCurrent"]) == "true")
                    {
                        HouseNo_P = HouseNo_C;
                        StreetNo_P = StreetNo_C;
                        TownCity_P = TownCity_C;
                        StateId_P = StateId_C;
                        DistrictId_P = DistrictId_C;
                        Pincode_P = Pincode_C;
                    }
                    else
                    {
                        HouseNo_P = Convert.ToString(Request.Form["txtHouseNo_perm"]);
                        StreetNo_P = Convert.ToString(Request.Form["txtStreet_perm"]);
                        TownCity_P = Convert.ToString(Request.Form["txtCity_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboStates_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboStates_perm");
                        }
                        else
                        {
                            StateId_P = Convert.ToInt32(_dummy);
                        }
                        //StateId_P = Convert.ToInt32(Request.Form["cboStates_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboDistricts_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboDistricts_perm");
                        }
                        else
                        {
                            DistrictId_P = Convert.ToInt32(_dummy);
                        }
                        //DistrictId_P = Convert.ToInt32(Request.Form["cboDistricts_perm"]);
                        _dummy = Convert.ToString(Request.Form["txtPincode_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid txtPincode_perm");
                        }
                        else
                        {
                            Pincode_P = Convert.ToInt32(_dummy);
                        }
                        //Pincode_P = Convert.ToInt32(Request.Form["txtPincode_perm"]);
                    }
                    Landline = Convert.ToString(Request.Form["txtLandlineNo"]);
                    Mobile = Convert.ToString(Request.Form["txtMobileNo"]);
                    Email = Convert.ToString(Request.Form["txtEMailId"]);
                    if (PortalSession.RoleCode == "I" && CORType == "IA")
                    {
                        CPEMail = String.Empty;
                    }
                    else{
                        CPEMail = Convert.ToString(Request.Form["txtCPEMailId"]);
                        if (CPEMail.Trim() == String.Empty)
                        {
                            throw new Exception("Contact Persons Email Id is Required");
                        }
                    }
                    
                    WhatsAppNumber = Convert.ToString(Request.Form["txtWhatsAppNo"]);
                    _dummy = Convert.ToString(Request.Form["cboAllowWhatsappMessage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid Whatsapp message status");
                    }
                    else
                    {
                        AllowWhatsAppMessage = _dummy;
                    }                   

                    Occupation = Convert.ToString(Request.Form["txtPrimaryOccupation"]);
                    ExmployeeCode = Convert.ToString(Request.Form["txtEmployeeCode"]);

                    //Int32 BranchState = -1;
                    //Int32 BranchDistrict = -1;
                    Branch = -1;
                    if (PortalSession.RoleCode == "CA")
                    {
                        _dummy = Convert.ToString(Request.Form["cboBranch"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboBranch");
                        }
                        else
                        {
                            Branch = Convert.ToInt32(_dummy);
                        }
                        //Branch = Convert.ToInt32(Request.Form["cboBranch"]);
                    }

                    ExamMode = Convert.ToString(Request.Form["txtExamMode"]);
                    ExamBody = Convert.ToString(Request.Form["cboExamBody"]);
                    ExamCenters = Convert.ToString(Request.Form["cboExamCenter"]);
                    ExamCenter = Convert.ToInt32(ExamCenters.Split('|')[0]);

                    _dummy = Convert.ToString(Request.Form["cboLanguage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboLanguage");
                    }
                    else
                    {
                        ExamLanguage = Convert.ToInt32(_dummy);
                    }

                    TelemarketerId = -1;
                    ////if (CORType == "AV")
                    ////{
                    ////    TelemarketerId = -1;
                    ////    _dummy = Convert.ToString(Request.Form["cboTelemarketer"]);
                    ////    if (_dummy.Trim() == String.Empty)
                    ////    {
                    ////        throw new Exception("Invalid cboTelemarketer");
                    ////    }
                    ////    else
                    ////    {
                    ////        TelemarketerId = Convert.ToInt32(_dummy);
                    ////    }
                    ////}

                }
                else //Declaration not checked.
                {
                    _Status = false;
                    s = HelperUtilities.ToJSON(false, "Unable to proceed as you have not provided your confirmation for Declaration");
                }
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            //Server Side Validations needs to be added... here
            //End Server Side Validations...

            if (_Status)
            {
                
                Boolean Success = false;
                IIIBL.URN objURN = null;
                DataTable objApplicant = null;
                try
                {
                    objApplicant = PortalApplication.Applicant;
                    objApplicant.Columns.Add("sntExamCenterID", typeof(Int32));
                    objApplicant.Columns.Add("chrExamMode", typeof(String));
                    objApplicant.Columns.Add("tntExamLanguageID", typeof(Int32));
                    objApplicant.Columns.Add("imgBasicQualificationDoc", typeof(System.Byte[]));
                    objApplicant.Columns.Add("imgQualificationDoc", typeof(System.Byte[]));

                    Boolean NeedsApproval = false;
                    if ( CORType=="PO" && ProfessionalQualification != "NA")
                    {
                        NeedsApproval = true;
                    }

                    DataRow dr = objApplicant.NewRow();
                    dr["bntApplicantID"] = 1;
                    dr["dtApplicationDate"] = dtApplicationDate;
                    dr["chrRollNumber"] = DBNull.Value;
                    dr["chrNameInitial"] = Salutation;
                    dr["varApplicantName"] = ApplicantName;
                    dr["varApplicantFatherName"] = FHName;
                    dr["tntCategory"] = Category;
                    dr["chrLicIndOrCorporate"] = "I";
                    dr["chrInsuranceCategory"] = InsuranceType;
                    dr["varPermHouseNo"] = HouseNo_P;
                    dr["varPermStreet"] = StreetNo_P;
                    dr["varPermTown"] = TownCity_P;
                    dr["sntPermDistrictID"] = DistrictId_P;
                    dr["intPermPINCode"] = Pincode_P;
                    dr["varCurrHouseNo"] = HouseNo_C;
                    dr["varCurrStreet"] = StreetNo_C;
                    dr["varCurrTown"] = TownCity_C;
                    dr["sntCurrDistrictID"] = DistrictId_C;
                    dr["intCurrPINCode"] = Pincode_C;
                    dr["chrUrbanRural"] = Area;
                    dr["chrQualification"] = ProfessionalQualification;
                    dr["varOtherQualification"] = OtherQualification;
                    dr["dtApplicantDOB"] = DOB;
                    dr["chrSex"] = Gender;
                    dr["varPhoneNo"] = Landline;
                    dr["varMobileNo"] = Mobile;
                    dr["allowwhatsapp_message"] = AllowWhatsAppMessage;
                    dr["whatsapp_number"] = WhatsAppNumber;
                    dr["varEmailID"] = Email;
                    dr["varPAN"] = PAN.ToUpper();
                    dr["varVoterID"] = VoterId.ToUpper();
                    dr["varPassportNo"] = Passport.ToUpper();
                    dr["varGovtIDCard"] = PhotoId.ToUpper();
                    dr["varDrivingLicenseNo"] = DrivingLicense.ToUpper();
                    dr["intAgntCounselorUserID"] = PortalSession.AgentCounselorUserID;
                    dr["intDPUserID"] = PortalSession.DPUserID;
                    dr["intInsurerUserID"] = PortalSession.InsurerUserID;
                    dr["varInsurerExtnRefNo"] = ExmployeeCode;
                    dr["imgApplicantPhoto"] = Photo;
                    dr["imgApplicantSign"] = Sign;
                    dr["btIsCompositeLicense"] = 0;
                    dr["varProfessionBusiness"] = Occupation;
                    dr["chrNationality"] = "I";// /???
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["btIsCertificate"] = 1;
                    }
                    else
                    {
                        dr["btIsCertificate"] = 0;
                    }

                    dr["varLicenseNo"] = DBNull.Value;
                    dr["tntApplicantStatusID"] = 0;
                    dr["btIsDirectApplicant"] = 0;
                    dr["intCreatedBy"] = PortalSession.UserID;
                    dr["dtCreatedOn"] = DBNull.Value;
                    dr["intLastModifiedBy"] = DBNull.Value;
                    dr["dtLastModifiedOn"] = DBNull.Value;
                    dr["chrBasicQualification"] = BasicQualification;
                    dr["varBasicQualBoardName"] = BoardName;
                    dr["varBasicQualRollNumber"] = RollNumber;
                    dr["dtBasicQualYearOfPassing"] = YearOfPassing;
                    dr["sntNationalityCountryID"] = Nationality;
                    dr["btIsRenewalApplicant"] = 0;
                    dr["varLicenceRefNo"] = DBNull.Value;
                    dr["intAgntCounselorUserID2"] = DBNull.Value;
                    dr["intDPUserID2"] = DBNull.Value;
                    dr["intInsurerUserID2"] = DBNull.Value;
                    //if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    //{
                        dr["LicenseType"] = PortalSession.RoleCode;
                        dr["CorporateType"] = CORType;
                    //}
                    dr["Record_ID"] = DBNull.Value;
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["CorporateUserID"] = PortalSession.UserID;
                    }
                    dr["PaymentDoneForIIITraining"] = "N";
                    dr["intBranchNo"] = (Branch == -1) ? (Object)DBNull.Value : Branch;
                    dr["RenewalURNNo"] = DBNull.Value;
                    dr["URNFromService"] = "N";
                    dr["varModificationRemarks"] = DBNull.Value;                    
                    dr["ContactPersonEmailId"] = CPEMail;                    

                    dr["chrExamMode"] = "O";
                    dr["tntExamLanguageID"] = ExamLanguage;
                    dr["sntExamCenterID"] = ExamCenter;
                    if (QualificationDoc == null)
                    {
                        dr["imgQualificationDoc"] = DBNull.Value;
                    }
                    else
                    {
                        dr["imgQualificationDoc"] = QualificationDoc;
                    }
                    if (BasicQualificationDoc == null)
                    {
                        dr["imgBasicQualificationDoc"] = DBNull.Value;
                    }
                    else
                    {
                        dr["imgBasicQualificationDoc"] = BasicQualificationDoc;
                    }
                    dr["tm_id"] = (TelemarketerId < 0) ? (Object)DBNull.Value : TelemarketerId;
                    objApplicant.Rows.Add(dr);

                    String RollNo = String.Empty;
                    String Status = String.Empty;
                    Message = String.Empty;

                    objURN = new URN();
                    objURN.GenerateURN(PortalApplication.ConnectionString, objApplicant, PortalSession.RoleCode, PortalSession.UserID, out RollNo, out Message);

                    if (Message.Trim() == String.Empty)
                    {
                        if (NeedsApproval)
                        {
                            Success = true;
                            Message = "The data has been saved and forwarded to 'Insurance Institute of India' for approval.";
                            s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_SUCCESS + " : " + Message, null);
                        }
                        else
                        {
                            Success = true;
                            Message = String.Empty;
                            KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                            kVPair[0] = new KeyValuePair<string, string>("URN", RollNo);
                            s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_SUCCESS, null, kVPair);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_FAIL + " :" + Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {
                    objApplicant = null;
                    objURN = null;
                }
            }

            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult UploadURN()
        {
            String[] FieldsCA = new String[] {"IRDA URN", "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN","Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name" , "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no", "UploadRemark"};
            String[] FieldsWA = new String[] { "IRDA URN", "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN","Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no", "UploadRemark" };
            String[] FieldsBR = new String[] { "IRDA URN", "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN",  "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no", "UploadRemark" };
            String[] FieldsIMF = new String[] { "IRDA URN", "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN", "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "UploadRemark" };
            String[] FieldsI = new String[] { "IRDA URN", "Sponsorship Date", "Name Initial", "Candidate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Other Qualification", "Date of Birth", "Sex", "Phone No", "Mobile No", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "PAN", "Insurer Ref No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Primary Profession", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Candidate Type", "Telemarketer TRAI reg no", "Contact Person Email ID", "UploadRemark" };
            String[] Fields = null;

            //String[] FieldsFmtCA = new String[] { String.Empty, String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "MMM-yyyy", String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };
            String[] FieldsFmtCA = new String[] { String.Empty, String.Empty,  String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

            //String[] FieldsFmtWA = new String[] { String.Empty, String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "MMM-yyyy", String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };
            String[] FieldsFmtWA = new String[] { String.Empty, String.Empty,  String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

            //String[] FieldsFmtBR = new String[] { String.Empty, String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "MMM-yyyy", String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };
            String[] FieldsFmtBR = new String[] { String.Empty, String.Empty,  String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };
            
            //String[] FieldsFmtIMF = new String[] { String.Empty, String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "MMM-yyyy", String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };
            String[] FieldsFmtIMF = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

            //String[] FieldsFmtI = new String[] { String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "MMM-yyyy", String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };
            String[] FieldsFmtI = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

            String[] FieldsFmt = null;

            IIIBL.URN objURN = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;
            try
            {
                if ((Request.Form["chkDeclarationU"]) == "on" || (Request.Form["chkDeclarationU"]) == "true") //Declaration has been checked.
                {
                    if (Path.GetExtension(Request.Files[0].FileName).EndsWith("zip"))
                    {
                        String OriginalFileName = Request.Files[0].FileName;
                        String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                        String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                        String FilePath = String.Format("../Uploads/{0}", FileName);
                        String OutputDirectoryName = String.Format("../Uploads/{0}", FileNamePart);

                        String OutputFileNamePart = PortalSession.UserLoginID + "_SponsorshipResponse_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                        String OutputFileName = String.Format("../Downloads/{0}", OutputFileNamePart + ".xlsx"); // ToString be passed back outside 
                        String OutputFileNameCSV = String.Format("../Downloads/{0}", OutputFileNamePart + ".csv"); // ToString be passed back outside 
                        String OutputFileName2 = Server.MapPath(OutputFileName); // for inner processing
                        String OutputFileNameCSV2 = Server.MapPath(OutputFileNameCSV); // for inner processing

                        OutputDirectoryName = Server.MapPath(OutputDirectoryName);
                        FilePath = Server.MapPath(FilePath);
                        Request.Files[0].SaveAs(FilePath);

                        HelperUtilities.UnzipFile(FilePath, OutputDirectoryName);

                        objURN = new IIIBL.URN();
                        objDataSet = objURN.UploadURN(PortalApplication.ConnectionString, OriginalFileName, PortalSession.UserID, PortalSession.RoleCode, PortalSession.InsurerTypeNew, OutputDirectoryName, out Message, PortalApplication.AKey, PortalApplication.AIV);
                        DataTable objDataTable = null;
                        KeyValuePair<String, String>[] KVPair = null;

                        switch (PortalSession.RoleCode)
                        {
                            case "CA":
                                Fields = FieldsCA;
                                FieldsFmt = FieldsFmtCA;
                                break;
                            case "IMF":
                                Fields = FieldsIMF;
                                FieldsFmt = FieldsFmtIMF;
                                break;
                            case "BR":
                                Fields = FieldsBR;
                                FieldsFmt = FieldsFmtBR;
                                break;
                            case "WA":
                                Fields = FieldsWA;
                                FieldsFmt = FieldsFmtWA;
                                break;
                            case "I":
                                Fields = FieldsI;
                                FieldsFmt = FieldsFmtI;
                                break;
                            default:
                                throw new Exception("Invalid Role Code");
                                break;
                        }

                        if (Message.Trim() == String.Empty)
                        {
                            objDataTable = objDataSet.Tables[0];

                            XLXporter.WriteExcel(OutputFileName2, objDataTable, Fields, Fields, FieldsFmt);
                            objDataTable.DataTable2File(OutputFileNameCSV2, "\t");

                            KVPair = new KeyValuePair<String, String>[2];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                            KVPair[1] = new KeyValuePair<string, string>("_RESPONSE_FILE2_", OutputFileNameCSV);

                            s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                        else
                        {
                            if (objDataSet != null)
                            {
                                objDataTable = objDataSet.Tables[0];
                                XLXporter.WriteExcel(OutputFileName2, objDataTable, Fields, Fields, FieldsFmt);
                                objDataTable.DataTable2File(OutputFileNameCSV2, "\t");

                                KVPair = new KeyValuePair<String, String>[2];
                                KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                                KVPair[1] = new KeyValuePair<string, string>("_RESPONSE_FILE2_", OutputFileNameCSV);
                                s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message, null, KVPair);
                            }
                            else
                            {
                                s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                            }
                        }

                        try
                        {
                            System.IO.File.Delete(FilePath);
                        }
                        catch (Exception ex)
                        {
                            Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                        }
                    }
                    else
                    {
                        s = HelperUtilities.ToJSON(false, "Invalid file type. Please refer to the template");
                        //Invalid File
                    }
                }
                else
                {
                    s = HelperUtilities.ToJSON(false, "Unable to proceed as you have not provided your confirmation for Declaration");
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                objURN = null;
            }

            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }
        #endregion

        #region URN Modification
        [HttpGet]
        [AuthorizeExt]
        public ActionResult URNModification()
        {
            if (PortalSession.RoleCode == "I")
            {
                ViewBag.Title = "Sponsorship Form Correction";
            }
            else
            {
                ViewBag.Title = "Training Registration Form Correction";
            }
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult URNModification(String URN)
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            try
            {
                objURN = new URN();
                objDataSet = objURN.GetDataForModification(PortalApplication.ConnectionString, URN, PortalSession.UserID, out Status, out Message);
                if (Status == "SUCCESS")
                {
                    Success = true;
                    objDataTable = objDataSet.Tables[0];
                    objDataTable.Columns.Add("Photo", typeof(String));
                    objDataTable.Columns.Add("Sign", typeof(String));
                    DataRow dataRow = objDataTable.Rows[0];
                    dataRow["Photo"] = Convert.ToBase64String((byte[])dataRow["imgApplicantPhoto"]);
                    dataRow["Sign"] = Convert.ToBase64String((byte[])dataRow["imgApplicantSign"]);
                    
                }
                if (Status == "FAIL")
                {
                    Success = false;
                    if (Message == "NO_DATA_FOUND")
                    {
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveURNModification()
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            DateTime dtApplicationDate = System.DateTime.Now.Date;
            String InsuranceType = String.Empty;
            String CORType = String.Empty;
            String Salutation = String.Empty;
            String ApplicantName = String.Empty;
            String FHName = String.Empty;
            String DOB = String.Empty;
            String Gender = String.Empty;
            Int32 Category = 0;
            String Area = String.Empty;
            Int32 Nationality = 0;
            String PAN = String.Empty;            
            String DrivingLicense = String.Empty;
            String Passport = String.Empty;
            String VoterId = String.Empty;
            String PhotoId = String.Empty;
            String BasicQualification = String.Empty;
            String BoardName = String.Empty;
            String RollNumber = String.Empty;
            DateTime YearOfPassing = System.DateTime.Now.Date;
            String ProfessionalQualification = String.Empty;
            String OtherQualification = String.Empty;
            String HouseNo_C = String.Empty;
            String StreetNo_C = String.Empty;
            String TownCity_C = String.Empty;
            Int32 StateId_C = 0;
            Int32 DistrictId_C = 0;
            Int32 Pincode_C = 0;

            String HouseNo_P = String.Empty;
            String StreetNo_P = String.Empty;
            String TownCity_P = String.Empty;
            Int32 StateId_P = 0;
            Int32 DistrictId_P = 0;
            Int32 Pincode_P = 0;

            String Landline = String.Empty;
            String Mobile = String.Empty;
            String WhatsAppNumber = String.Empty;
            String AllowWhatsAppMessage = String.Empty;
            Int32 TelemarketerId = -1;

            String Email = String.Empty;
            String CPEMail = String.Empty;
            String Occupation = String.Empty;
            String ExmployeeCode = String.Empty;
            Int32 Branch = -1;

            String ExamMode = String.Empty;
            String ExamBody = String.Empty;
            String ExamCenters = String.Empty;
            Int32 ExamCenter = 0;
            Int32 ExamLanguage = 0;
            Byte[] Sign = null;
            Byte[] Photo = null;
            Byte[] QualificationDoc = null;
            Byte[] BasicQualificationDoc = null;

            Int64 ApplicantId = -1; 
            String URN = string.Empty;
            
            try
            {
                if ((Request.Form["chkDeclaration"]) == "on" || (Request.Form["chkDeclaration"]) == "true") //Declaration has been checked.
                {

                    String _dummy = Convert.ToString(Request.Form["hdnApplicantId"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid hdnApplicantId");
                    }
                    else
                    {
                        ApplicantId = Convert.ToInt64(_dummy);
                    }
                    URN = Convert.ToString(Request.Form["hdnURN"]);

                    _dummy = Convert.ToString(Request.Form["txtApplicationDate"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtApplicationDate");
                    }
                    else
                    {
                        dtApplicationDate = Convert.ToDateTime(_dummy);
                    }
                    //DateTime dtApplicationDate = Convert.ToDateTime(Request.Form["txtApplicationDate"]);
                    //InsuranceType = Convert.ToString(Request.Form["cboInsuranceCategory"]);
                    //CORType = Convert.ToString(Request.Form["cboCORType"]);
                    InsuranceType = Convert.ToString(Request.Form["hdnInsuranceCategory"]);
                    CORType = Convert.ToString(Request.Form["hdnCORType"]);
                    Salutation = Convert.ToString(Request.Form["cboSalutation"]);
                    ApplicantName = Convert.ToString(Request.Form["txtApplicantName"]);
                    FHName = Convert.ToString(Request.Form["txtFHName"]);
                    DOB = Convert.ToString(Request.Form["txtDOB"]);
                    Gender = Convert.ToString(Request.Form["cboGender"]);
                    if (Gender == null || Gender == "")
                    {
                        switch (Salutation)
                        {
                            case "Mr.":
                                Gender = "M";
                                break;
                            case "Mrs.":
                            case "Ms.":
                                Gender = "F";
                                break;
                            //case "Dr.": // no need to handle this as gender wont be null in this case
                            case "Mx.":
                                Gender = "O";
                                break;
                            default:
                                Gender = "O";
                                break;
                        }
                    }

                    _dummy = Convert.ToString(Request.Form["cboCategory"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboCategory");
                    }
                    else
                    {
                        Category = Convert.ToInt32(_dummy);
                    }
                    //Int32 Category = Convert.ToInt32(Request.Form["cboCategory"]);
                    Area = Convert.ToString(Request.Form["cboArea"]);
                    _dummy = Convert.ToString(Request.Form["cboNationality"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboNationality");
                    }
                    else
                    {
                        Nationality = Convert.ToInt32(_dummy);
                    }
                    //Int32 Nationality = Convert.ToInt32(Request.Form["cboNationality"]);
                    PAN = Convert.ToString(Request.Form["txtPAN"]);

                    DrivingLicense = Convert.ToString(Request.Form["txtDrivingLic"]);
                    Passport = Convert.ToString(Request.Form["txtPassport"]);
                    VoterId = Convert.ToString(Request.Form["txtVoterId"]);
                    PhotoId = Convert.ToString(Request.Form["txtPhotoId"]);

                    Photo = null;
                    if (Request.Files["txtFilePhoto"].ContentLength != 0)
                    {
                        MemoryStream ms1 = new MemoryStream((Int32)Request.Files["txtFilePhoto"].InputStream.Length);
                        Request.Files["txtFilePhoto"].InputStream.CopyTo(ms1);
                        Photo = ms1.ToArray();
                        ms1 = null;
                    }

                    Sign = null;
                    if (Request.Files["txtFileSign"].ContentLength != 0)
                    {
                        MemoryStream ms2 = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                        Request.Files["txtFileSign"].InputStream.CopyTo(ms2);
                        Sign = ms2.ToArray();
                        ms2 = null;
                    }

                    BasicQualification = Convert.ToString(Request.Form["cboBasicQualification"]);
                    BoardName = Convert.ToString(Request.Form["txtBoardName"]);
                    RollNumber = Convert.ToString(Request.Form["txtRollnumber"]);
                    _dummy = Convert.ToString(Request.Form["txtYearofPassing"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtYearofPassing");
                    }
                    else
                    {
                        YearOfPassing = Convert.ToDateTime(_dummy);
                    }
                    //DateTime YearOfPassing = Convert.ToDateTime(Request.Form["txtYearofPassing"]);
                    ProfessionalQualification = Convert.ToString(Request.Form["cboProfessionalQualification"]);
                    OtherQualification = Convert.ToString(Request.Form["txtOtherQualification"]);

                    /* Certificate upload for CA / WA / IMF / BR and COR type = 'PO' */
                    if (CORType == "PO")
                    {   //Basic Qualification
                        if (Request.Files["txtBasicQualificationDoc"].ContentLength != 0)
                        {
                            MemoryStream ms = new MemoryStream((Int32)Request.Files["txtBasicQualificationDoc"].InputStream.Length);
                            Request.Files["txtBasicQualificationDoc"].InputStream.CopyTo(ms);
                            BasicQualificationDoc = ms.ToArray();
                            ms = null;
                        }

                        //Professional Qualification
                        QualificationDoc = null;
                        //if (!ProfessionalQualification.In(new string[] { "NA" }))
                        //{
                        //    ms = new MemoryStream((Int32)Request.Files["txtQualificationDoc"].InputStream.Length);
                        //    Request.Files["txtQualificationDoc"].InputStream.CopyTo(ms);
                        //    QualificationDoc = ms.ToArray();
                        //    ms = null;
                        //}
                        //else
                        //{

                        //}
                    }

                    /* End addition */


                    HouseNo_C = Convert.ToString(Request.Form["txtHouseNo_current"]);
                    StreetNo_C = Convert.ToString(Request.Form["txtStreet_current"]);
                    TownCity_C = Convert.ToString(Request.Form["txtCity_current"]);
                    _dummy = Convert.ToString(Request.Form["cboStates_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboStates_current");
                    }
                    else
                    {
                        StateId_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 StateId_C = Convert.ToInt32(Request.Form["cboStates_current"]);
                    _dummy = Convert.ToString(Request.Form["cboDistricts_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboDistricts_current");
                    }
                    else
                    {
                        DistrictId_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 DistrictId_C = Convert.ToInt32(Request.Form["cboDistricts_current"]);
                    _dummy = Convert.ToString(Request.Form["txtPincode_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtPincode_current");
                    }
                    else
                    {
                        Pincode_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 Pincode_C = Convert.ToInt32(Request.Form["txtPincode_current"]);

                    HouseNo_P = String.Empty;
                    StreetNo_P = String.Empty;
                    TownCity_P = String.Empty;
                    StateId_P = 0;
                    DistrictId_P = 0;
                    Pincode_P = 0;

                    if ((Request.Form["chkSameAsCurrent"]) == "on" || (Request.Form["chkSameAsCurrent"]) == "true")
                    {
                        HouseNo_P = HouseNo_C;
                        StreetNo_P = StreetNo_C;
                        TownCity_P = TownCity_C;
                        StateId_P = StateId_C;
                        DistrictId_P = DistrictId_C;
                        Pincode_P = Pincode_C;
                    }
                    else
                    {
                        HouseNo_P = Convert.ToString(Request.Form["txtHouseNo_perm"]);
                        StreetNo_P = Convert.ToString(Request.Form["txtStreet_perm"]);
                        TownCity_P = Convert.ToString(Request.Form["txtCity_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboStates_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboStates_perm");
                        }
                        else
                        {
                            StateId_P = Convert.ToInt32(_dummy);
                        }
                        //StateId_P = Convert.ToInt32(Request.Form["cboStates_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboDistricts_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboDistricts_perm");
                        }
                        else
                        {
                            DistrictId_P = Convert.ToInt32(_dummy);
                        }
                        //DistrictId_P = Convert.ToInt32(Request.Form["cboDistricts_perm"]);
                        _dummy = Convert.ToString(Request.Form["txtPincode_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid txtPincode_perm");
                        }
                        else
                        {
                            Pincode_P = Convert.ToInt32(_dummy);
                        }
                        //Pincode_P = Convert.ToInt32(Request.Form["txtPincode_perm"]);
                    }

                    Landline = Convert.ToString(Request.Form["txtLandlineNo"]);
                    Mobile = Convert.ToString(Request.Form["txtMobileNo"]);
                    WhatsAppNumber = Convert.ToString(Request.Form["txtWhatsAppNo"]);
                    _dummy = Convert.ToString(Request.Form["cboAllowWhatsappMessage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid Whatsapp message status");
                    }
                    else
                    {
                        AllowWhatsAppMessage = _dummy;
                    }
                    Email = Convert.ToString(Request.Form["txtEMailId"]);
                    CPEMail = Convert.ToString(Request.Form["txtCPEMailId"]);

                    Occupation = Convert.ToString(Request.Form["txtPrimaryOccupation"]);
                    ExmployeeCode = Convert.ToString(Request.Form["txtEmployeeCode"]);

                    //Int32 BranchState = -1;
                    //Int32 BranchDistrict = -1;
                    Branch = -1;
                    if (PortalSession.RoleCode == "CA")
                    {
                        //BranchState = Convert.ToInt32(Request.Form["cboBranchState"]);
                        //BranchDistrict = Convert.ToInt32(Request.Form["cboBranchDistrict"]);
                        _dummy = Convert.ToString(Request.Form["cboBranch"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboBranch");
                        }
                        else
                        {
                            Branch = Convert.ToInt32(_dummy);
                        }
                        //Branch = Convert.ToInt32(Request.Form["cboBranch"]);
                    }

                    ExamMode = Convert.ToString(Request.Form["txtExamMode"]);
                    ExamBody = Convert.ToString(Request.Form["cboExamBody"]);
                    ExamCenters = Convert.ToString(Request.Form["cboExamCenter"]);
                    ExamCenter = Convert.ToInt32(ExamCenters.Split('|')[0]);

                    _dummy = Convert.ToString(Request.Form["cboLanguage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboLanguage");
                    }
                    else
                    {
                        ExamLanguage = Convert.ToInt32(_dummy);
                    }
                    //Int32 ExamLanguage = Convert.ToInt32(Request.Form["cboLanguage"]);
                    TelemarketerId = -1;
                    ////if (CORType == "AV")
                    ////{
                    ////    TelemarketerId = -1;
                    ////    _dummy = Convert.ToString(Request.Form["cboTelemarketer"]);
                    ////    if (_dummy.Trim() == String.Empty)
                    ////    {
                    ////        throw new Exception("Invalid cboTelemarketer");
                    ////    }
                    ////    else
                    ////    {
                    ////        TelemarketerId = Convert.ToInt32(_dummy);
                    ////    }
                    ////}
                }
                else
                {
                    s = HelperUtilities.ToJSON(false, "Unable to proceed as you have not provided your confirmation for Declaration");
                }

            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            //Server Side Validations needs to be added... here
            //End Server Side Validations...
            if (_Status)
            {
                Boolean Success = false;
                IIIBL.URN objURN = null;
                DataTable objApplicant = null;
                try
                {
                    objApplicant = PortalApplication.Applicant;
                    objApplicant.Columns.Add("sntExamCenterID", typeof(Int32));
                    objApplicant.Columns.Add("chrExamMode", typeof(String));
                    objApplicant.Columns.Add("tntExamLanguageID", typeof(Int32));
                    objApplicant.Columns.Add("imgBasicQualificationDoc", typeof(System.Byte[]));
                    objApplicant.Columns.Add("imgQualificationDoc", typeof(System.Byte[]));

                    DataRow dr = objApplicant.NewRow();
                    dr["bntApplicantID"] = ApplicantId;
                    dr["dtApplicationDate"] = dtApplicationDate;
                    dr["chrRollNumber"] = URN;
                    dr["chrNameInitial"] = Salutation;
                    dr["varApplicantName"] = ApplicantName;
                    dr["varApplicantFatherName"] = FHName;
                    dr["tntCategory"] = Category;
                    dr["chrLicIndOrCorporate"] = "I";
                    dr["chrInsuranceCategory"] = InsuranceType;
                    dr["varPermHouseNo"] = HouseNo_P;
                    dr["varPermStreet"] = StreetNo_P;
                    dr["varPermTown"] = TownCity_P;
                    dr["sntPermDistrictID"] = DistrictId_P;
                    dr["intPermPINCode"] = Pincode_P;
                    dr["varCurrHouseNo"] = HouseNo_C;
                    dr["varCurrStreet"] = StreetNo_C;
                    dr["varCurrTown"] = TownCity_C;
                    dr["sntCurrDistrictID"] = DistrictId_C;
                    dr["intCurrPINCode"] = Pincode_C;
                    dr["chrUrbanRural"] = Area;
                    dr["chrQualification"] = ProfessionalQualification;
                    dr["varOtherQualification"] = OtherQualification;
                    dr["dtApplicantDOB"] = DOB;
                    dr["chrSex"] = Gender;
                    dr["varPhoneNo"] = Landline;
                    dr["varMobileNo"] = Mobile;
                    dr["allowwhatsapp_message"] = AllowWhatsAppMessage;
                    dr["whatsapp_number"] = WhatsAppNumber;
                    dr["varEmailID"] = Email;
                    dr["varPAN"] = PAN.ToUpper();
                    dr["varVoterID"] = VoterId.ToUpper();
                    dr["varPassportNo"] = Passport.ToUpper();
                    dr["varGovtIDCard"] = PhotoId.ToUpper();
                    dr["varDrivingLicenseNo"] = DrivingLicense.ToUpper();
                    dr["intAgntCounselorUserID"] = DBNull.Value;
                    dr["intDPUserID"] = DBNull.Value;
                    dr["intInsurerUserID"] = DBNull.Value;
                    dr["varInsurerExtnRefNo"] = ExmployeeCode;
                    dr["imgApplicantPhoto"] = Photo == null ? (Object)DBNull.Value : Photo;
                    dr["imgApplicantSign"] = Sign == null ? (Object)DBNull.Value : Sign;
                    dr["btIsCompositeLicense"] = 0;
                    dr["varProfessionBusiness"] = Occupation;
                    dr["chrNationality"] = "I";// /???
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["btIsCertificate"] = 1;
                    }
                    else
                    {
                        dr["btIsCertificate"] = 0;
                    }

                    dr["varLicenseNo"] = DBNull.Value;
                    dr["tntApplicantStatusID"] = 0;
                    dr["btIsDirectApplicant"] = 0;
                    dr["intCreatedBy"] = DBNull.Value;
                    dr["dtCreatedOn"] = DBNull.Value;
                    dr["intLastModifiedBy"] = PortalSession.UserID;
                    dr["dtLastModifiedOn"] = DBNull.Value;
                    dr["chrBasicQualification"] = BasicQualification;
                    dr["varBasicQualBoardName"] = BoardName;
                    dr["varBasicQualRollNumber"] = RollNumber;
                    dr["dtBasicQualYearOfPassing"] = YearOfPassing;
                    dr["sntNationalityCountryID"] = Nationality;
                    dr["btIsRenewalApplicant"] = 0;
                    dr["varLicenceRefNo"] = DBNull.Value;
                    dr["intAgntCounselorUserID2"] = DBNull.Value;
                    dr["intDPUserID2"] = DBNull.Value;
                    dr["intInsurerUserID2"] = DBNull.Value;
                    dr["LicenseType"] = PortalSession.RoleCode;
                    dr["CorporateType"] = CORType;
                    dr["Record_ID"] = DBNull.Value;
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["CorporateUserID"] = PortalSession.UserID;
                    }
                    dr["PaymentDoneForIIITraining"] = "N";
                    dr["intBranchNo"] = (Branch == -1) ? (Object)DBNull.Value : Branch;
                    dr["RenewalURNNo"] = DBNull.Value;
                    dr["URNFromService"] = "N";
                    dr["varModificationRemarks"] = DBNull.Value;                    
                    dr["ContactPersonEmailId"] = CPEMail;
                    dr["chrExamMode"] = "O";
                    dr["tntExamLanguageID"] = ExamLanguage;
                    dr["sntExamCenterID"] = ExamCenter;

                    if (QualificationDoc == null)
                    {
                        dr["imgQualificationDoc"] = DBNull.Value;
                    }
                    else
                    {
                        dr["imgQualificationDoc"] = QualificationDoc;
                    }
                    if (BasicQualificationDoc == null)
                    {
                        dr["imgBasicQualificationDoc"] = DBNull.Value;
                    }
                    else
                    {
                        dr["imgBasicQualificationDoc"] = BasicQualificationDoc;
                    }
                    dr["tm_id"] = (TelemarketerId < 0) ? (Object)DBNull.Value : TelemarketerId; ;

                    objApplicant.Rows.Add(dr);

                    String RollNo = String.Empty;
                    String Status = String.Empty;
                    Message = String.Empty;

                    objURN = new URN();
                    Message = objURN.UpdateURNDetails(PortalApplication.ConnectionString, objApplicant, PortalSession.UserID);

                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("URN", RollNo);
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_SUCCESS, null, kVPair);
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_FAIL + " :" + Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {
                    objApplicant = null;
                    objURN = null;
                }
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //Approval Rejection Mode::::
        [HttpGet]
        [AuthorizeExt]
        public ActionResult URNRequestModification()
        {
            ViewBag.Title = "Training Registration Request Correction";
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";

            Boolean Success = true;
            Int64 Id = -1;
            try
            {
                String dummy = Request.QueryString["Id"];
                Id = Convert.ToInt64(dummy);
            }
            catch(Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
            }
            if (Success)
            {
                ViewBag.Id = Id;
                return View();
            }
            else
            {
                return View("ERROR");
            }
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult URNRequestModification(String _dummy="")
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            Boolean _Status = true;

            Int64 Id = 0;
            try
            {
                String dummy = Request.Form["Id"];
                Id = Convert.ToInt64(dummy);
            }
            catch(Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (_Status)
            {
                try
                {
                    objURN = new URN();
                    objDataSet = objURN.GetDataForURNRequestModification(PortalApplication.ConnectionString, Id, PortalSession.UserID, out Message);
                    if (Message == String.Empty)
                    {
                        Success = true;
                        objDataTable = objDataSet.Tables[0];
                        objDataTable.Columns.Add("Photo", typeof(String));
                        objDataTable.Columns.Add("Sign", typeof(String));
                        DataRow dataRow = objDataTable.Rows[0];
                        dataRow["Photo"] = Convert.ToBase64String((byte[])dataRow["imgApplicantPhoto"]);
                        dataRow["Sign"] = Convert.ToBase64String((byte[])dataRow["imgApplicantSign"]);                        
                        
                    }
                    else
                    {
                        Success = false;
                    }
                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objURN = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveURNRequestModification()
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            DateTime dtApplicationDate = System.DateTime.Now.Date;
            String InsuranceType = String.Empty;
            String CORType = String.Empty;
            String Salutation = String.Empty;
            String ApplicantName = String.Empty;
            String FHName = String.Empty;
            String DOB = String.Empty;
            String Gender = String.Empty;
            Int32 Category = 0;
            String Area = String.Empty;
            Int32 Nationality = 0;
            String PAN = String.Empty;            
            String DrivingLicense = String.Empty;
            String Passport = String.Empty;
            String VoterId = String.Empty;
            String PhotoId = String.Empty;
            String BasicQualification = String.Empty;
            String BoardName = String.Empty;
            String RollNumber = String.Empty;
            DateTime YearOfPassing = System.DateTime.Now.Date;
            String ProfessionalQualification = String.Empty;
            String OtherQualification = String.Empty;
            String HouseNo_C = String.Empty;
            String StreetNo_C = String.Empty;
            String TownCity_C = String.Empty;
            Int32 StateId_C = 0;
            Int32 DistrictId_C = 0;
            Int32 Pincode_C = 0;

            String HouseNo_P = String.Empty;
            String StreetNo_P = String.Empty;
            String TownCity_P = String.Empty;
            Int32 StateId_P = 0;
            Int32 DistrictId_P = 0;
            Int32 Pincode_P = 0;

            String Landline = String.Empty;
            String Mobile = String.Empty;
            String WhatsAppNumber = String.Empty;
            String AllowWhatsAppMessage = String.Empty;
            Int32 TelemarketerId = -1;

            String Email = String.Empty;
            String CPEMail = String.Empty;
            String Occupation = String.Empty;
            String ExmployeeCode = String.Empty;
            Int32 Branch = -1;

            String ExamMode = String.Empty;
            String ExamBody = String.Empty;
            String ExamCenters = String.Empty;
            Int32 ExamCenter = 0;
            Int32 ExamLanguage = 0;
            Byte[] Sign = null;
            Byte[] Photo = null;
            Byte[] QualificationDoc = null;
            Byte[] BasicQualificationDoc = null;

            Int64 ApplicantId = -1;
            String URN = string.Empty;
            Int64 Id = -1;

            try
            {
                if ((Request.Form["chkDeclaration"]) == "on" || (Request.Form["chkDeclaration"]) == "true") //Declaration has been checked.
                {

                    String _dummy = Convert.ToString(Request.Form["hdnId"]);
                    //if (_dummy.Trim() == String.Empty)
                    //{
                    //    throw new Exception("Invalid hdnApplicantId");
                    //}
                    //else
                    //{
                    //    ApplicantId = Convert.ToInt64(_dummy);
                    //}
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid hdnId");
                    }
                    else
                    {
                        Id = Convert.ToInt64(_dummy);
                    }

                    //URN = Convert.ToString(Request.Form["hdnURN"]);

                    _dummy = Convert.ToString(Request.Form["txtApplicationDate"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtApplicationDate");
                    }
                    else
                    {
                        dtApplicationDate = Convert.ToDateTime(_dummy);
                    }
                    //DateTime dtApplicationDate = Convert.ToDateTime(Request.Form["txtApplicationDate"]);
                    //InsuranceType = Convert.ToString(Request.Form["cboInsuranceCategory"]);
                    //CORType = Convert.ToString(Request.Form["cboCORType"]);
                    InsuranceType = Convert.ToString(Request.Form["hdnInsuranceCategory"]);
                    CORType = Convert.ToString(Request.Form["cboCORType"]);
                    Salutation = Convert.ToString(Request.Form["cboSalutation"]);
                    ApplicantName = Convert.ToString(Request.Form["txtApplicantName"]);
                    FHName = Convert.ToString(Request.Form["txtFHName"]);
                    DOB = Convert.ToString(Request.Form["txtDOB"]);
                    Gender = Convert.ToString(Request.Form["cboGender"]);
                    if (Gender == null || Gender == "")
                    {
                        switch (Salutation)
                        {
                            case "Mr.":
                                Gender = "M";
                                break;
                            case "Mrs.":
                            case "Ms.":
                                Gender = "F";
                                break;
                            //case "Dr.": // no need to handle this as gender wont be null in this case
                            case "Mx.":
                                Gender = "O";
                                break;
                            default:
                                Gender = "O";
                                break;
                        }
                    }

                    _dummy = Convert.ToString(Request.Form["cboCategory"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboCategory");
                    }
                    else
                    {
                        Category = Convert.ToInt32(_dummy);
                    }
                    //Int32 Category = Convert.ToInt32(Request.Form["cboCategory"]);
                    Area = Convert.ToString(Request.Form["cboArea"]);
                    _dummy = Convert.ToString(Request.Form["cboNationality"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboNationality");
                    }
                    else
                    {
                        Nationality = Convert.ToInt32(_dummy);
                    }
                    //Int32 Nationality = Convert.ToInt32(Request.Form["cboNationality"]);
                    PAN = Convert.ToString(Request.Form["txtPAN"]);
                    
                    DrivingLicense = Convert.ToString(Request.Form["txtDrivingLic"]);
                    Passport = Convert.ToString(Request.Form["txtPassport"]);
                    VoterId = Convert.ToString(Request.Form["txtVoterId"]);
                    PhotoId = Convert.ToString(Request.Form["txtPhotoId"]);

                    Photo = null;
                    if (Request.Files["txtFilePhoto"].ContentLength != 0)
                    {
                        MemoryStream ms1 = new MemoryStream((Int32)Request.Files["txtFilePhoto"].InputStream.Length);
                        Request.Files["txtFilePhoto"].InputStream.CopyTo(ms1);
                        Photo = ms1.ToArray();
                        ms1 = null;
                    }

                    Sign = null;
                    if (Request.Files["txtFileSign"].ContentLength != 0)
                    {
                        MemoryStream ms2 = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                        Request.Files["txtFileSign"].InputStream.CopyTo(ms2);
                        Sign = ms2.ToArray();
                        ms2 = null;
                    }

                    BasicQualification = Convert.ToString(Request.Form["cboBasicQualification"]);
                    BoardName = Convert.ToString(Request.Form["txtBoardName"]);
                    RollNumber = Convert.ToString(Request.Form["txtRollnumber"]);
                    _dummy = Convert.ToString(Request.Form["txtYearofPassing"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtYearofPassing");
                    }
                    else
                    {
                        YearOfPassing = Convert.ToDateTime(_dummy);
                    }
                    //DateTime YearOfPassing = Convert.ToDateTime(Request.Form["txtYearofPassing"]);
                    ProfessionalQualification = Convert.ToString(Request.Form["cboProfessionalQualification"]);
                    OtherQualification = Convert.ToString(Request.Form["txtOtherQualification"]);


                    /* Certificate upload for CA / WA / IMF / BR and COR type = 'PO' */
                    if (CORType == "PO")
                    {   //Basic Qualification
                        if (Request.Files["txtBasicQualificationDoc"].ContentLength != 0)
                        {
                            MemoryStream ms3 = new MemoryStream((Int32)Request.Files["txtBasicQualificationDoc"].InputStream.Length);
                            Request.Files["txtBasicQualificationDoc"].InputStream.CopyTo(ms3);
                            BasicQualificationDoc = ms3.ToArray();
                            ms3 = null;
                        }
                        //Professional Qualification
                        if (!ProfessionalQualification.In(new string[] { "NA" }))
                        {
                            if (Request.Files["txtQualificationDoc"].ContentLength != 0)
                            {
                                MemoryStream ms4 = new MemoryStream((Int32)Request.Files["txtQualificationDoc"].InputStream.Length);
                                Request.Files["txtQualificationDoc"].InputStream.CopyTo(ms4);
                                QualificationDoc = ms4.ToArray();
                                ms4 = null;
                            }
                        }
                        else
                        {

                        }
                    }

                    HouseNo_C = Convert.ToString(Request.Form["txtHouseNo_current"]);
                    StreetNo_C = Convert.ToString(Request.Form["txtStreet_current"]);
                    TownCity_C = Convert.ToString(Request.Form["txtCity_current"]);
                    _dummy = Convert.ToString(Request.Form["cboStates_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboStates_current");
                    }
                    else
                    {
                        StateId_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 StateId_C = Convert.ToInt32(Request.Form["cboStates_current"]);
                    _dummy = Convert.ToString(Request.Form["cboDistricts_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboDistricts_current");
                    }
                    else
                    {
                        DistrictId_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 DistrictId_C = Convert.ToInt32(Request.Form["cboDistricts_current"]);
                    _dummy = Convert.ToString(Request.Form["txtPincode_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtPincode_current");
                    }
                    else
                    {
                        Pincode_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 Pincode_C = Convert.ToInt32(Request.Form["txtPincode_current"]);

                    HouseNo_P = String.Empty;
                    StreetNo_P = String.Empty;
                    TownCity_P = String.Empty;
                    StateId_P = 0;
                    DistrictId_P = 0;
                    Pincode_P = 0;

                    if ((Request.Form["chkSameAsCurrent"]) == "on" || (Request.Form["chkSameAsCurrent"]) == "true")
                    {
                        HouseNo_P = HouseNo_C;
                        StreetNo_P = StreetNo_C;
                        TownCity_P = TownCity_C;
                        StateId_P = StateId_C;
                        DistrictId_P = DistrictId_C;
                        Pincode_P = Pincode_C;
                    }
                    else
                    {
                        HouseNo_P = Convert.ToString(Request.Form["txtHouseNo_perm"]);
                        StreetNo_P = Convert.ToString(Request.Form["txtStreet_perm"]);
                        TownCity_P = Convert.ToString(Request.Form["txtCity_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboStates_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboStates_perm");
                        }
                        else
                        {
                            StateId_P = Convert.ToInt32(_dummy);
                        }
                        //StateId_P = Convert.ToInt32(Request.Form["cboStates_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboDistricts_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboDistricts_perm");
                        }
                        else
                        {
                            DistrictId_P = Convert.ToInt32(_dummy);
                        }
                        //DistrictId_P = Convert.ToInt32(Request.Form["cboDistricts_perm"]);
                        _dummy = Convert.ToString(Request.Form["txtPincode_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid txtPincode_perm");
                        }
                        else
                        {
                            Pincode_P = Convert.ToInt32(_dummy);
                        }
                        //Pincode_P = Convert.ToInt32(Request.Form["txtPincode_perm"]);
                    }

                    Landline = Convert.ToString(Request.Form["txtLandlineNo"]);
                    Mobile = Convert.ToString(Request.Form["txtMobileNo"]);
                    WhatsAppNumber = Convert.ToString(Request.Form["txtWhatsAppNo"]);
                    _dummy = Convert.ToString(Request.Form["cboAllowWhatsappMessage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid Whatsapp message status");
                    }
                    else
                    {
                        AllowWhatsAppMessage = _dummy;
                    }
                    Email = Convert.ToString(Request.Form["txtEMailId"]);
                    CPEMail = Convert.ToString(Request.Form["txtCPEMailId"]);

                    Occupation = Convert.ToString(Request.Form["txtPrimaryOccupation"]);
                    ExmployeeCode = Convert.ToString(Request.Form["txtEmployeeCode"]);

                    //Int32 BranchState = -1;
                    //Int32 BranchDistrict = -1;
                    Branch = -1;
                    if (PortalSession.RoleCode == "CA")
                    {
                        //BranchState = Convert.ToInt32(Request.Form["cboBranchState"]);
                        //BranchDistrict = Convert.ToInt32(Request.Form["cboBranchDistrict"]);
                        _dummy = Convert.ToString(Request.Form["cboBranch"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboBranch");
                        }
                        else
                        {
                            Branch = Convert.ToInt32(_dummy);
                        }
                        //Branch = Convert.ToInt32(Request.Form["cboBranch"]);
                    }

                    ExamMode = Convert.ToString(Request.Form["txtExamMode"]);
                    ExamBody = Convert.ToString(Request.Form["cboExamBody"]);
                    ExamCenters = Convert.ToString(Request.Form["cboExamCenter"]);
                    ExamCenter = Convert.ToInt32(ExamCenters.Split('|')[0]);

                    _dummy = Convert.ToString(Request.Form["cboLanguage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboLanguage");
                    }
                    else
                    {
                        ExamLanguage = Convert.ToInt32(_dummy);
                    }
                    //Int32 ExamLanguage = Convert.ToInt32(Request.Form["cboLanguage"]);
                    TelemarketerId = -1;
                    //if (CORType == "AV")
                    //{
                    //    TelemarketerId = -1;
                    //    _dummy = Convert.ToString(Request.Form["cboTelemarketer"]);
                    //    if (_dummy.Trim() == String.Empty)
                    //    {
                    //        throw new Exception("Invalid cboTelemarketer");
                    //    }
                    //    else
                    //    {
                    //        TelemarketerId = Convert.ToInt32(_dummy);
                    //    }
                    //}
                }
                else
                {
                    s = HelperUtilities.ToJSON(false, "Unable to proceed as you have not provided your confirmation for Declaration");
                }

            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            //Server Side Validations needs to be added... here
            //End Server Side Validations...
            if (_Status)
            {
                Boolean Success = false;
                IIIBL.URN objURN = null;
                DataTable objApplicant = null;
                try
                {
                    objApplicant = PortalApplication.Applicant;
                    objApplicant.Columns.Add("sntExamCenterID", typeof(Int32));
                    objApplicant.Columns.Add("chrExamMode", typeof(String));
                    objApplicant.Columns.Add("tntExamLanguageID", typeof(Int32));
                    objApplicant.Columns.Add("imgBasicQualificationDoc", typeof(System.Byte[]));
                    objApplicant.Columns.Add("imgQualificationDoc", typeof(System.Byte[]));

                    Boolean NeedsApproval = false;
                    if (CORType == "PO" && ProfessionalQualification != "NA")
                    {
                        NeedsApproval = true;
                    }

                    DataRow dr = objApplicant.NewRow();
                    dr["bntApplicantID"] = ApplicantId;
                    dr["dtApplicationDate"] = dtApplicationDate;
                    dr["chrRollNumber"] = URN;
                    dr["chrNameInitial"] = Salutation;
                    dr["varApplicantName"] = ApplicantName;
                    dr["varApplicantFatherName"] = FHName;
                    dr["tntCategory"] = Category;
                    dr["chrLicIndOrCorporate"] = "I";
                    dr["chrInsuranceCategory"] = InsuranceType;
                    dr["varPermHouseNo"] = HouseNo_P;
                    dr["varPermStreet"] = StreetNo_P;
                    dr["varPermTown"] = TownCity_P;
                    dr["sntPermDistrictID"] = DistrictId_P;
                    dr["intPermPINCode"] = Pincode_P;
                    dr["varCurrHouseNo"] = HouseNo_C;
                    dr["varCurrStreet"] = StreetNo_C;
                    dr["varCurrTown"] = TownCity_C;
                    dr["sntCurrDistrictID"] = DistrictId_C;
                    dr["intCurrPINCode"] = Pincode_C;
                    dr["chrUrbanRural"] = Area;
                    dr["chrQualification"] = ProfessionalQualification;
                    dr["varOtherQualification"] = OtherQualification;
                    dr["dtApplicantDOB"] = DOB;
                    dr["chrSex"] = Gender;
                    dr["varPhoneNo"] = Landline;
                    dr["varMobileNo"] = Mobile;
                    dr["allowwhatsapp_message"] = AllowWhatsAppMessage;
                    dr["whatsapp_number"] = WhatsAppNumber;
                    dr["varEmailID"] = Email;
                    dr["varPAN"] = PAN.ToUpper();
                    dr["varVoterID"] = VoterId.ToUpper();
                    dr["varPassportNo"] = Passport.ToUpper();
                    dr["varGovtIDCard"] = PhotoId.ToUpper();
                    dr["varDrivingLicenseNo"] = DrivingLicense.ToUpper();
                    dr["intAgntCounselorUserID"] = DBNull.Value;//May need some values in case of Insurance Agents if at all
                    dr["intDPUserID"] = DBNull.Value;//May need some values in case of Insurance Agents if at all
                    dr["intInsurerUserID"] = PortalSession.InsurerUserID;
                    dr["varInsurerExtnRefNo"] = ExmployeeCode;
                    dr["imgApplicantPhoto"] = Photo == null ? (Object)DBNull.Value : Photo;
                    dr["imgApplicantSign"] = Sign == null ? (Object)DBNull.Value : Sign;
                    dr["btIsCompositeLicense"] = 0;
                    dr["varProfessionBusiness"] = Occupation;
                    dr["chrNationality"] = "I";// /???
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["btIsCertificate"] = 1;
                    }
                    else
                    {
                        dr["btIsCertificate"] = 0;
                    }

                    dr["varLicenseNo"] = DBNull.Value;
                    dr["tntApplicantStatusID"] = 0;
                    dr["btIsDirectApplicant"] = 0;
                    dr["intCreatedBy"] = PortalSession.UserID;
                    dr["dtCreatedOn"] = DBNull.Value;
                    dr["intLastModifiedBy"] = PortalSession.UserID;
                    dr["dtLastModifiedOn"] = DBNull.Value;
                    dr["chrBasicQualification"] = BasicQualification;
                    dr["varBasicQualBoardName"] = BoardName;
                    dr["varBasicQualRollNumber"] = RollNumber;
                    dr["dtBasicQualYearOfPassing"] = YearOfPassing;
                    dr["sntNationalityCountryID"] = Nationality;
                    dr["btIsRenewalApplicant"] = 0;
                    dr["varLicenceRefNo"] = DBNull.Value;
                    dr["intAgntCounselorUserID2"] = DBNull.Value;
                    dr["intDPUserID2"] = DBNull.Value;
                    dr["intInsurerUserID2"] = DBNull.Value;
                    //if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    //{
                        dr["LicenseType"] = PortalSession.RoleCode;
                        dr["CorporateType"] = CORType;
                    //}
                    dr["Record_ID"] = DBNull.Value;
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["CorporateUserID"] = PortalSession.UserID;
                    }
                    dr["PaymentDoneForIIITraining"] = "N";
                    dr["intBranchNo"] = (Branch == -1) ? (Object)DBNull.Value : Branch;
                    dr["RenewalURNNo"] = DBNull.Value;
                    dr["URNFromService"] = "N";
                    dr["varModificationRemarks"] = DBNull.Value;                    
                    dr["ContactPersonEmailId"] = CPEMail;                                  
                    dr["chrExamMode"] = "O";
                    dr["tntExamLanguageID"] = ExamLanguage;
                    dr["sntExamCenterID"] = ExamCenter;
                    if (QualificationDoc == null)
                    {
                        dr["imgQualificationDoc"] = DBNull.Value;
                    }
                    else
                    {
                        dr["imgQualificationDoc"] = QualificationDoc;
                    }
                    if (BasicQualificationDoc == null)
                    {
                        dr["imgBasicQualificationDoc"] = DBNull.Value;
                    }
                    else
                    {
                        dr["imgBasicQualificationDoc"] = BasicQualificationDoc;
                    }
                    dr["tm_id"] = (TelemarketerId < 0) ? (Object)DBNull.Value : TelemarketerId;
                    objApplicant.Rows.Add(dr);

                    String RollNo = String.Empty;
                    String Status = String.Empty;
                    Message = String.Empty;

                    objURN = new URN();
                    Message = objURN.UpdateURNRequest(PortalApplication.ConnectionString, Id, objApplicant, PortalSession.RoleCode, PortalSession.UserID, out URN);

                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("URN", URN);
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_SUCCESS, null, kVPair);
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_FAIL + " : " + Message);
                    }
                    //if (Message.Trim() == String.Empty)
                    //{
                    //    Success = true;
                    //    Message = CommonMessages.DATA_SAVE_SUCCESS;
                    //    if (URN != String.Empty)
                    //    {
                    //        Message += " URN created : " + URN;
                    //    }
                    //    s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_SUCCESS, null);
                    //}
                    //else
                    //{
                    //    Success = false;
                    //    s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_FAIL + " :" + Message);
                    //}
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {
                    objApplicant = null;
                    objURN = null;
                }
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        #endregion

        #region Duplicate URN creation 
        [HttpGet]
        [AuthorizeExt]
        public ActionResult DuplicateURN()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult DuplicateURN(String URN)
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            try
            {
                objURN = new URN();
                objDataSet = objURN.GetDataForURNDuplication(PortalApplication.ConnectionString, URN, PortalSession.UserID, out Status, out Message);
                if (Status == "SUCCESS")
                {
                    Success = true;
                    objDataTable = objDataSet.Tables[0];
                    objDataTable.Columns.Add("Photo", typeof(String));
                    objDataTable.Columns.Add("Sign", typeof(String));
                    DataRow dataRow = objDataTable.Rows[0];
                    dataRow["Photo"] = Convert.ToBase64String((byte[])dataRow["imgApplicantPhoto"]);
                    dataRow["Sign"] = Convert.ToBase64String((byte[])dataRow["imgApplicantSign"]);
                                        
                }
                if (Status == "FAIL")
                {
                    Success = false;
                    if (Message == "NO_DATA_FOUND")
                    {
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveDuplicateURN()
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            DateTime dtApplicationDate = System.DateTime.Now.Date;
            String InsuranceType = String.Empty;
            String CORType = String.Empty;
            String Salutation = String.Empty;
            String ApplicantName = String.Empty;
            String FHName = String.Empty;
            String DOB = String.Empty;
            String Gender = String.Empty;
            Int32 Category = 0;
            String Area = String.Empty;
            Int32 Nationality = 0;
            String PAN = String.Empty;            
            String DrivingLicense = String.Empty;
            String Passport = String.Empty;
            String VoterId = String.Empty;
            String PhotoId = String.Empty;
            String BasicQualification = String.Empty;
            String BoardName = String.Empty;
            String RollNumber = String.Empty;
            DateTime YearOfPassing = System.DateTime.Now.Date;
            String ProfessionalQualification = String.Empty;
            String OtherQualification = String.Empty;
            String HouseNo_C = String.Empty;
            String StreetNo_C = String.Empty;
            String TownCity_C = String.Empty;
            Int32 StateId_C = 0;
            Int32 DistrictId_C = 0;
            Int32 Pincode_C = 0;

            String HouseNo_P = String.Empty;
            String StreetNo_P = String.Empty;
            String TownCity_P = String.Empty;
            Int32 StateId_P = 0;
            Int32 DistrictId_P = 0;
            Int32 Pincode_P = 0;

            String Landline = String.Empty;
            String Mobile = String.Empty;
            String WhatsAppNumber = String.Empty;
            String AllowWhatsAppMessage = String.Empty;
            Int32 TelemarketerId = -1;

            String Email = String.Empty;
            String CPEMail = String.Empty;
            String Occupation = String.Empty;
            String ExmployeeCode = String.Empty;
            Int32 Branch = -1;

            String ExamMode = String.Empty;
            String ExamBody = String.Empty;
            String ExamCenters = String.Empty;
            Int32 ExamCenter = 0;
            Int32 ExamLanguage = 0;
            Byte[] Sign = null;
            Byte[] Photo = null;

            Int64 ApplicantId = -1;
            String URN = string.Empty;
            try
            {
                if ((Request.Form["chkDeclaration"]) == "on" || (Request.Form["chkDeclaration"]) == "true") //Declaration has been checked.
                {
                    String _dummy = Convert.ToString(Request.Form["hdnApplicantId"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid hdnApplicantId");
                    }
                    else
                    {
                        ApplicantId = Convert.ToInt64(_dummy);
                    }
                    //Int64 ApplicantId = Convert.ToInt32(Request.Form["hdnApplicantId"]);
                    URN = Convert.ToString(Request.Form["hdnURN"]);

                    _dummy = Convert.ToString(Request.Form["txtApplicationDate"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtApplicationDate");
                    }
                    else
                    {
                        dtApplicationDate = Convert.ToDateTime(_dummy);
                    }
                    //DateTime dtApplicationDate = Convert.ToDateTime(Request.Form["txtApplicationDate"]);
                    //InsuranceType = Convert.ToString(Request.Form["cboInsuranceCategory"]);
                    //CORType = Convert.ToString(Request.Form["cboCORType"]);
                    InsuranceType = Convert.ToString(Request.Form["hdnInsuranceCategory"]);
                    CORType = Convert.ToString(Request.Form["hdnCORType"]);

                    //Salutation = Convert.ToString(Request.Form["cboSalutation"]);
                    Salutation = Convert.ToString(Request.Form["hdnSalutation"]);
                    ApplicantName = Convert.ToString(Request.Form["txtApplicantName"]);
                    FHName = Convert.ToString(Request.Form["txtFHName"]);
                    DOB = Convert.ToString(Request.Form["txtDOB"]);
                    Gender = Convert.ToString(Request.Form["cboGender"]);
                    if (Gender == null || Gender == "")
                    {
                        switch (Salutation)
                        {
                            case "Mr.":
                                Gender = "M";
                                break;
                            case "Mrs.":
                            case "Ms.":
                                Gender = "F";
                                break;
                            //case "Dr.": // no need to handle this as gender wont be null in this case
                            case "Mx.":
                                Gender = "O";
                                break;
                            default:
                                Gender = "O";
                                break;
                        }
                    }

                    _dummy = Convert.ToString(Request.Form["cboCategory"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboCategory");
                    }
                    else
                    {
                        Category = Convert.ToInt32(_dummy);
                    }
                    //Int32 Category = Convert.ToInt32(Request.Form["cboCategory"]);
                    Area = Convert.ToString(Request.Form["cboArea"]);

                    _dummy = Convert.ToString(Request.Form["cboNationality"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboNationality");
                    }
                    else
                    {
                        Nationality = Convert.ToInt32(_dummy);
                    }
                    //Int32 Nationality = Convert.ToInt32(Request.Form["cboNationality"]);
                    PAN = Convert.ToString(Request.Form["txtPAN"]);

                    DrivingLicense = Convert.ToString(Request.Form["txtDrivingLic"]);
                    Passport = Convert.ToString(Request.Form["txtPassport"]);
                    VoterId = Convert.ToString(Request.Form["txtVoterId"]);
                    PhotoId = Convert.ToString(Request.Form["txtPhotoId"]);

                    Photo = null;
                    if (Request.Files["txtFilePhoto"].ContentLength != 0)
                    {
                        MemoryStream ms1 = new MemoryStream((Int32)Request.Files["txtFilePhoto"].InputStream.Length);
                        Request.Files["txtFilePhoto"].InputStream.CopyTo(ms1);
                        Photo = ms1.ToArray();
                        ms1 = null;
                    }

                    Sign = null;
                    if (Request.Files["txtFileSign"].ContentLength != 0)
                    {
                        MemoryStream ms2 = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                        Request.Files["txtFileSign"].InputStream.CopyTo(ms2);
                        Sign = ms2.ToArray();
                        ms2 = null;
                    }

                    BasicQualification = Convert.ToString(Request.Form["cboBasicQualification"]);
                    BoardName = Convert.ToString(Request.Form["txtBoardName"]);
                    RollNumber = Convert.ToString(Request.Form["txtRollnumber"]);
                    _dummy = Convert.ToString(Request.Form["txtYearofPassing"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtYearofPassing");
                    }
                    else
                    {
                        YearOfPassing = Convert.ToDateTime(_dummy);
                    }
                    //DateTime YearOfPassing = Convert.ToDateTime(Request.Form["txtYearofPassing"]);
                    ProfessionalQualification = Convert.ToString(Request.Form["cboProfessionalQualification"]);

                    HouseNo_C = Convert.ToString(Request.Form["txtHouseNo_current"]);
                    StreetNo_C = Convert.ToString(Request.Form["txtStreet_current"]);
                    TownCity_C = Convert.ToString(Request.Form["txtCity_current"]);
                    _dummy = Convert.ToString(Request.Form["cboStates_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboStates_current");
                    }
                    else
                    {
                        StateId_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 StateId_C = Convert.ToInt32(Request.Form["cboStates_current"]);
                    _dummy = Convert.ToString(Request.Form["cboDistricts_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboDistricts_current");
                    }
                    else
                    {
                        DistrictId_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 DistrictId_C = Convert.ToInt32(Request.Form["cboDistricts_current"]);
                    _dummy = Convert.ToString(Request.Form["txtPincode_current"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid txtPincode_current");
                    }
                    else
                    {
                        Pincode_C = Convert.ToInt32(_dummy);
                    }
                    //Int32 Pincode_C = Convert.ToInt32(Request.Form["txtPincode_current"]);

                    HouseNo_P = String.Empty;
                    StreetNo_P = String.Empty;
                    TownCity_P = String.Empty;
                    StateId_P = 0;
                    DistrictId_P = 0;
                    Pincode_P = 0;
                    if ((Request.Form["chkSameAsCurrent"]) == "on" || (Request.Form["chkSameAsCurrent"]) == "true")
                    {
                        HouseNo_P = HouseNo_C;
                        StreetNo_P = StreetNo_C;
                        TownCity_P = TownCity_C;
                        StateId_P = StateId_C;
                        DistrictId_P = DistrictId_C;
                        Pincode_P = Pincode_C;
                    }
                    else
                    {
                        HouseNo_P = Convert.ToString(Request.Form["txtHouseNo_perm"]);
                        StreetNo_P = Convert.ToString(Request.Form["txtStreet_perm"]);
                        TownCity_P = Convert.ToString(Request.Form["txtCity_perm"]);
                        _dummy = Convert.ToString(Request.Form["cboStates_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboStates_perm");
                        }
                        else
                        {
                            StateId_P = Convert.ToInt32(_dummy);
                        }
                        //StateId_P = Convert.ToInt32(Request.Form["cboStates_perm"];
                        _dummy = Convert.ToString(Request.Form["cboDistricts_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboDistricts_perm");
                        }
                        else
                        {
                            DistrictId_P = Convert.ToInt32(_dummy);
                        }
                        //DistrictId_P = Convert.ToInt32(Request.Form["cboDistricts_perm"]);
                        _dummy = Convert.ToString(Request.Form["txtPincode_perm"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid txtPincode_perm");
                        }
                        else
                        {
                            Pincode_P = Convert.ToInt32(_dummy);
                        }
                        //Pincode_P = Convert.ToInt32(Request.Form["txtPincode_perm"]);
                    }

                    Landline = Convert.ToString(Request.Form["txtLandlineNo"]);
                    Mobile = Convert.ToString(Request.Form["txtMobileNo"]);
                    WhatsAppNumber = Convert.ToString(Request.Form["txtWhatsAppNo"]);
                    _dummy = Convert.ToString(Request.Form["cboAllowWhatsappMessage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid Whatsapp message status");
                    }
                    else
                    {
                        AllowWhatsAppMessage = _dummy;
                    }
                    Email = Convert.ToString(Request.Form["txtEMailId"]);
                    CPEMail = Convert.ToString(Request.Form["txtCPEMailId"]);

                    Occupation = Convert.ToString(Request.Form["txtPrimaryOccupation"]);
                    ExmployeeCode = Convert.ToString(Request.Form["txtEmployeeCode"]);

                    //Int32 BranchState = -1;
                    //Int32 BranchDistrict = -1;
                    Branch = -1;
                    if (PortalSession.RoleCode == "CA")
                    {
                        //BranchState = Convert.ToInt32(Request.Form["cboBranchState"]);
                        //BranchDistrict = Convert.ToInt32(Request.Form["cboBranchDistrict"]);
                        _dummy = Convert.ToString(Request.Form["cboBranch"]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid cboBranch");
                        }
                        else
                        {
                            Branch = Convert.ToInt32(_dummy);
                        }
                        //Branch = Convert.ToInt32(Request.Form["cboBranch"]);
                    }

                    ExamMode = Convert.ToString(Request.Form["txtExamMode"]);
                    ExamBody = Convert.ToString(Request.Form["cboExamBody"]);
                    ExamCenters = Convert.ToString(Request.Form["cboExamCenter"]);
                    ExamCenter = Convert.ToInt32(ExamCenters.Split('|')[0]);

                    _dummy = Convert.ToString(Request.Form["cboLanguage"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboLanguage");
                    }
                    else
                    {
                        ExamLanguage = Convert.ToInt32(_dummy);
                    }
                    //Int32 ExamLanguage = Convert.ToInt32(Request.Form["cboLanguage"]);
                    TelemarketerId = -1;
                    //if (CORType == "AV")
                    //{
                    //    TelemarketerId = -1;
                    //    _dummy = Convert.ToString(Request.Form["cboTelemarketer"]);
                    //    if (_dummy.Trim() == String.Empty)
                    //    {
                    //        throw new Exception("Invalid cboTelemarketer");
                    //    }
                    //    else
                    //    {
                    //        TelemarketerId = Convert.ToInt32(_dummy);
                    //    }
                    //}
                }
                else
                {
                    s = HelperUtilities.ToJSON(false, "Unable to proceed as you have not provided your confirmation for Declaration");
                }
            }
            catch(Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (_Status)
            {
                s = String.Empty;
                Boolean Success = false;
                IIIBL.URN objURN = null;
                DataTable objApplicant = null;
                try
                {
                    objApplicant = PortalApplication.Applicant;
                    objApplicant.Columns.Add("sntExamCenterID", typeof(Int32));
                    objApplicant.Columns.Add("chrExamMode", typeof(String));
                    objApplicant.Columns.Add("tntExamLanguageID", typeof(Int32));
                    objApplicant.Columns.Add("imgBasicQualificationDoc", typeof(System.Byte[]));
                    objApplicant.Columns.Add("imgQualificationDoc", typeof(System.Byte[]));

                    DataRow dr = objApplicant.NewRow();
                    dr["bntApplicantID"] = ApplicantId;
                    dr["dtApplicationDate"] = dtApplicationDate;
                    dr["chrRollNumber"] = URN;
                    dr["chrNameInitial"] = Salutation;
                    dr["varApplicantName"] = ApplicantName;
                    dr["varApplicantFatherName"] = FHName;
                    dr["tntCategory"] = Category;
                    dr["chrLicIndOrCorporate"] = "I";
                    dr["chrInsuranceCategory"] = InsuranceType;
                    dr["varPermHouseNo"] = HouseNo_P;
                    dr["varPermStreet"] = StreetNo_P;
                    dr["varPermTown"] = TownCity_P;
                    dr["sntPermDistrictID"] = DistrictId_P;
                    dr["intPermPINCode"] = Pincode_P;
                    dr["varCurrHouseNo"] = HouseNo_C;
                    dr["varCurrStreet"] = StreetNo_C;
                    dr["varCurrTown"] = TownCity_C;
                    dr["sntCurrDistrictID"] = DistrictId_C;
                    dr["intCurrPINCode"] = Pincode_C;
                    dr["chrUrbanRural"] = Area;
                    dr["chrQualification"] = ProfessionalQualification;
                    dr["varOtherQualification"] = String.Empty;
                    dr["dtApplicantDOB"] = DOB;
                    dr["chrSex"] = Gender;
                    dr["varPhoneNo"] = Landline;
                    dr["varMobileNo"] = Mobile;
                    dr["allowwhatsapp_message"] = AllowWhatsAppMessage;
                    dr["whatsapp_number"] = WhatsAppNumber;
                    dr["varEmailID"] = Email;
                    dr["varPAN"] = PAN.ToUpper();
                    dr["varVoterID"] = VoterId.ToUpper();
                    dr["varPassportNo"] = Passport.ToUpper();
                    dr["varGovtIDCard"] = PhotoId.ToUpper();
                    dr["varDrivingLicenseNo"] = DrivingLicense.ToUpper();
                    dr["intAgntCounselorUserID"] = DBNull.Value;
                    dr["intDPUserID"] = PortalSession.DPUserID;
                    dr["intInsurerUserID"] = PortalSession.InsurerUserID;
                    dr["varInsurerExtnRefNo"] = ExmployeeCode;
                    dr["imgApplicantPhoto"] = Photo == null ? (Object)DBNull.Value : Photo;
                    dr["imgApplicantSign"] = Sign == null ? (Object)DBNull.Value : Sign;
                    dr["btIsCompositeLicense"] = 0;
                    dr["varProfessionBusiness"] = Occupation;
                    dr["chrNationality"] = "I";// /???
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["btIsCertificate"] = 1;
                    }
                    else
                    {
                        dr["btIsCertificate"] = 0;
                    }

                    dr["varLicenseNo"] = DBNull.Value;
                    dr["tntApplicantStatusID"] = 0;
                    dr["btIsDirectApplicant"] = 0;
                    dr["intCreatedBy"] = DBNull.Value;
                    dr["dtCreatedOn"] = DBNull.Value;
                    dr["intLastModifiedBy"] = PortalSession.UserID;
                    dr["dtLastModifiedOn"] = DBNull.Value;
                    dr["chrBasicQualification"] = BasicQualification;
                    dr["varBasicQualBoardName"] = BoardName;
                    dr["varBasicQualRollNumber"] = RollNumber;
                    dr["dtBasicQualYearOfPassing"] = YearOfPassing;
                    dr["sntNationalityCountryID"] = Nationality;
                    dr["btIsRenewalApplicant"] = 0;
                    dr["varLicenceRefNo"] = DBNull.Value;
                    dr["intAgntCounselorUserID2"] = DBNull.Value;
                    dr["intDPUserID2"] = DBNull.Value;
                    dr["intInsurerUserID2"] = DBNull.Value;
                    dr["LicenseType"] = PortalSession.RoleCode;
                    dr["CorporateType"] = CORType;
                    dr["Record_ID"] = DBNull.Value;
                    if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                    {
                        dr["CorporateUserID"] = PortalSession.UserID;
                    }
                    dr["PaymentDoneForIIITraining"] = "N";
                    dr["intBranchNo"] = (Branch == -1) ? (Object)DBNull.Value : Branch;
                    dr["RenewalURNNo"] = DBNull.Value;
                    dr["URNFromService"] = "N";
                    dr["varModificationRemarks"] = DBNull.Value;                    
                    dr["ContactPersonEmailId"] = CPEMail;                    
                    
                    dr["chrExamMode"] = "O";
                    dr["tntExamLanguageID"] = ExamLanguage;
                    dr["sntExamCenterID"] = ExamCenter;
                    dr["tm_id"] = (TelemarketerId < 0) ? (Object)DBNull.Value : TelemarketerId;

                    objApplicant.Rows.Add(dr);

                    String RollNo = String.Empty;
                    String Status = String.Empty;
                    Message = String.Empty;

                    objURN = new URN();
                    objURN.GenerateDuplicateURN(PortalApplication.ConnectionString, objApplicant, PortalSession.RoleCode, PortalSession.UserID, out RollNo, out Message);

                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("URN", RollNo);
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_SUCCESS, null, kVPair);
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_SAVE_FAIL + " : " + Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {
                    objApplicant = null;
                    objURN = null;
                }
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult UploadDuplicateURN()
        {
            IIIBL.URN objURN = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;
            try
            {
                if ((Request.Form["chkDeclarationU"]) == "on" || (Request.Form["chkDeclarationU"]) == "true") //Declaration has been checked.
                {
                    String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                    String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                    String FilePath = String.Format("../Uploads/{0}", FileName);

                    String OutputFileNamePart = PortalSession.UserLoginID + "_SponsorshipStatusReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");
                    String OutputFileName = String.Format("../Downloads/{0}", OutputFileNamePart + ".csv");

                    //String OutputFileName = String.Format("../Downloads/{0}", FileNamePart + ".csv");
                    String OutputFileName2 = Server.MapPath(OutputFileName);

                    FilePath = Server.MapPath(FilePath);
                    Request.Files[0].SaveAs(FilePath);

                    objURN = new IIIBL.URN();
                    objDataSet = objURN.UploadDuplicateURN(PortalApplication.ConnectionString, PortalSession.UserID, PortalSession.RoleCode, FilePath, out Message);

                    DataTable objDataTable = null;
                    KeyValuePair<String, String>[] KVPair = null;
                    if (Message.Trim() == String.Empty)
                    {
                        objDataTable = objDataSet.Tables[0];
                        objDataTable.DataTable2File(OutputFileName2, "\t");
                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                    }
                    else // (Status == "SUCCESS")
                    {
                        s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                    }

                    try
                    {
                        System.IO.File.Delete(FilePath);
                    }
                    catch (Exception ex)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
                else
                {
                    s = HelperUtilities.ToJSON(false, "Unable to proceed as you have not provided your confirmation for Declaration");
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                objURN = null;
            }

            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }
        #endregion

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ExamDetails()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetExamDetails(String URN)
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null; //To be taken from application 
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;

            try
            {
                objURN = new URN();
                objDataSet = objURN.GetURNDetailForEE(PortalApplication.ConnectionString, URN);
                if (objDataSet != null && objDataSet.Tables.Count != 0)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;

                        objDataTable.Columns.Add("Photo", typeof(String));
                        //objDataTable.Columns.Add("Sign", typeof(String));
                        DataRow dataRow = objDataTable.Rows[0];
                        dataRow["Photo"] = Convert.ToBase64String((byte[])dataRow["ApplicantPhoto"]);
                        //dataRow["Sign"] = Convert.ToBase64String((byte[])dataRow["imgApplicantSign"]);
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objURN = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveExamDetails()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            String s = String.Empty;
            Boolean _Status = true;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;

            Int32 Hint = 0;
            String URN = String.Empty;
            String ExamDate = String.Empty;
            String ExamTime = String.Empty;
            String ExamineeId = String.Empty;
            Int32 Marks = -1;
            Int32 Result = -1;

            try
            {
                String _dummy = Convert.ToString(Request.Form["hdnAction"]);
                Hint = Convert.ToInt32(_dummy);

                URN = Convert.ToString(Request.Form["hdnURN"]);
                ExamDate = Convert.ToString(Request.Form["txtExaminationDate"]);
                ExamTime = Convert.ToString(Request.Form["txtExaminationTime"]);
                ExamDate = ExamDate + " " + ExamTime;
                ExamineeId = Convert.ToString(Request.Form["txtExamineeId"]);

                if (Hint == 2)
                {
                    _dummy = Convert.ToString(Request.Form["txtMarks"]);
                    Marks = Convert.ToInt32(_dummy);

                    _dummy = Convert.ToString(Request.Form["cboStatus"]);
                    Result = Convert.ToInt32(_dummy);
                }
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (_Status)
            {
                try
                {
                    objURN = new IIIBL.URN();
                    Message = objURN.SaveExamDetails(PortalApplication.ConnectionString, Hint, URN, ExamDate, ExamineeId, Marks, Result, PortalSession.UserID);

                    Success = Message == String.Empty;
                    if (Success)
                    {
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    else
                    {
                        Message = CommonMessages.DATA_SAVE_FAIL + "." + Message;
                    }
                    s = HelperUtilities.ToJSON(Success, Message, null);

                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objURN = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ExamDetailsB()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult UploadExamResults()
        {
            IIIBL.ExamReports objExamReports = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            try
            {
                String Action = Convert.ToString(Request.Form["cboAction"]);
                if (Action =="1")
                {
                    DateTime FromDate = Convert.ToDateTime(Request.Form["txtFromDate"]);
                    DateTime TillDate = Convert.ToDateTime(Request.Form["txtTillDate"]);

                    //Fetch the report and throw out.
                    objExamReports = new IIIBL.ExamReports();
                    objDataSet = objExamReports.DownloadApplicantDetails(PortalApplication.ConnectionString, PortalSession.UserID, FromDate, TillDate);

                    if (objDataSet != null && objDataSet.Tables.Count == 1 && objDataSet.Tables[0].Rows.Count > 0)
                    {
                        String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                        String OutputFileName = String.Format("../Downloads/{0}", FileNamePart + ".csv");
                        String OutputFileName2 = Server.MapPath(OutputFileName);

                        objDataTable = objDataSet.Tables[0];
                        objDataTable.DataTable2File(OutputFileName2, "\t");
                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS + " : " + Message, null, KVPair);
                    }
                    else
                    {
                        s = HelperUtilities.ToJSON(false, CommonMessages.NO_DATA_FOUND );
                    }
                }
                if (Action == "2" || Action == "3" || Action == "4")
                {
                    String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                    String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                    String FilePath = String.Format("../Uploads/{0}", FileName);
                    String OutputFileName = String.Format("../Downloads/{0}", FileNamePart + ".csv");
                    String OutputFileName2 = Server.MapPath(OutputFileName);
                    FilePath = Server.MapPath(FilePath);
                    Request.Files[0].SaveAs(FilePath);

                    objExamReports = new IIIBL.ExamReports();
                    if (Action == "2")
                    {
                        objDataSet = objExamReports.UploadExamDetails(PortalApplication.ConnectionString, FilePath, true, PortalSession.UserID, out Message);
                    }
                    if (Action == "3")
                    {
                        objDataSet = objExamReports.UploadExamDetails(PortalApplication.ConnectionString, FilePath, false, PortalSession.UserID, out Message);
                    }
                    if (Action == "4")
                    {
                        objDataSet = objExamReports.UploadAIMSResponse(PortalApplication.ConnectionString, FilePath, PortalSession.UserID, out Message);
                    }
                    
                    if (Message.Trim() == string.Empty)
                    {
                        objDataTable = objDataSet.Tables[0];
                        objDataTable.DataTable2File(OutputFileName2, "\t");
                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                    }
                    else
                    {
                        s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                    }

                    try
                    {
                        System.IO.File.Delete(FilePath);
                    }
                    catch (Exception ex)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                objExamReports = null;
            }

            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult UpdateUrnStatus()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpPost]
        public JsonResult UpdateUrnStatus(String Dummy = "")
        {
            IIIBL.URN objURN = new URN();
            JsonResult objJsonResult = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;

            string Irdaurn = Convert.ToString(Request.Form["urn"]);
            string Changedstatus = Convert.ToString(Request.Form["status"]);
            string Username = PortalSession.UserName;
            string Userid = PortalSession.UserID.ToString();
            string Usermachineip = PortalSession.ClientMachineIP;

            try
            {
                String data = objURN.UpdateUrnStatus(PortalApplication.ConnectionString, Irdaurn, Changedstatus, Username, Userid, Usermachineip);
                Success = true;
                //Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, data);
            }
            catch (Exception ex)
            {
                //Errorlogger.LogError("Home", "GetDistrictsForStates", ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objURN = null;
                //objDataTable = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        #region delete urn
        [HttpGet]
        [AuthorizeExt]
        public ActionResult DeleteURN()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult GetURNDetailsForDeletion()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;

            String URN = Convert.ToString(Request.Form["txtURN"]);
            if ( String.IsNullOrEmpty(URN.Trim()))
            {
                Success = false;
                Message = "Invalid URN";
            }
            else //(Success)
            {
                try
                {
                    objURN = new URN();
                    objDataSet = objURN.GetURNDataForDeletion(PortalApplication.ConnectionString, URN, PortalSession.UserID, out Message);
                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        objDataTable = objDataSet.Tables[0];
                    }
                    else
                    {
                        Success = false;

                    }
                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objURN = null;
                }

            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult DeleteURN(String Dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            //DataSet objDataSet = null;
            //DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;

            String URN = String.Empty;
            try
            {
                URN = Convert.ToString(Request.Form["hddURN"]).Trim();

                if (String.IsNullOrEmpty(URN))
                {
                    Success = false;
                    s = HelperUtilities.ToJSON(false, "Invalid URN");
                }
                else
                {
                    Success = true;
                    Message = String.Empty;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Success)
            {
                try
                {
                    objURN = new URN();
                    Message = objURN.DeleteURN(PortalApplication.ConnectionString, URN, PortalSession.UserID );
                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = "The URN has been marked for deletion and will be deleted tonight.";
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.DATA_DELETION_FAIL + " : " + Message;
                    }
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    //objDataTable = null;
                    //objDataSet = null;
                    objURN = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        #endregion
        #region URN Approval
        [HttpGet]
        [AuthorizeExt]
        public ActionResult URNRequestApproval()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }
        
        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult GetPendingURNApprovals()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;

            String RecStatus = Convert.ToString(Request.Form["cboStatus"]);
            Int32 Hint = -1;
            if (!RecStatus.In(new string[] { "A","P","R"}))
            {
                Success = false;
                Message = "Invalid Status";
            }
            else //(Success)
            {
                try
                {
                    switch(RecStatus)
                    {
                        case "P":
                            Hint = 1;
                            break;
                        case "A":
                            Hint = 2;
                            break;
                        case "R":
                            Hint = 3;
                            break;
                    }

                    objURN = new URN();
                    objDataSet = objURN.GetURNDataForApproval(PortalApplication.ConnectionString, Hint,-1, PortalSession.UserID, out Message);
                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        objDataTable = objDataSet.Tables[0];
                    }
                    else
                    {
                        Success = false;
                    }
                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objURN = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //Before appoval images from tbl_applicant_details 
        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult DownloadQD()
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            Int64 Id = -1;
            try
            {
                String _dummy = String.Empty;
                _dummy = Convert.ToString(Request.Form["Id"]);
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Id");
                }
                else
                {
                    Id = Convert.ToInt64(_dummy);
                }
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (_Status)
            {
                try
                {
                    objURN = new IIIBL.URN();
                    objDataSet = objURN.GetURNDataForApproval(PortalApplication.ConnectionString, 5, Id, PortalSession.UserID, out Message);

                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            DataRow dt = objDataTable.Rows[0];
                            byte[] bytes = (byte[])dt["imgQualificationDoc"];

                            _Status = true;
                            Message = String.Empty;

                            String Filename = PortalSession.UserLoginID + "_Qual_" + Id + "-" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".jpg";
                            String OutputFileName = String.Format("../Downloads/{0}", Filename);
                            String OutputFileName2 = Server.MapPath(OutputFileName);

                            System.IO.File.WriteAllBytes(OutputFileName2, bytes); 

                            KVPair = new KeyValuePair<String, String>[1];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                            s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                        else
                        {
                            _Status = true;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(_Status, Message, null);
                        }
                    }
                    else
                    {
                        _Status = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(_Status, Message, null);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult DownloadBQD()
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            Int64 Id = -1;
            try
            {
                String _dummy = String.Empty;
                _dummy = Convert.ToString(Request.Form["Id"]);
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Id");
                }
                else
                {
                    Id = Convert.ToInt64(_dummy);
                }
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (_Status)
            {
                try
                {
                    objURN = new IIIBL.URN();
                    objDataSet = objURN.GetURNDataForApproval(PortalApplication.ConnectionString, 5, Id, PortalSession.UserID, out Message);

                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            DataRow dt = objDataTable.Rows[0];
                            byte[] bytes = (byte[])dt["imgBasicQualificationDoc"];

                            _Status = true;
                            Message = String.Empty;

                            String Filename = PortalSession.UserLoginID + "_BasicQual_" + Id + "-" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".jpg";
                            String OutputFileName = String.Format("../Downloads/{0}", Filename);
                            String OutputFileName2 = Server.MapPath(OutputFileName);

                            System.IO.File.WriteAllBytes(OutputFileName2, bytes);

                            KVPair = new KeyValuePair<String, String>[1];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                            s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                        else
                        {
                            _Status = true;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(_Status, Message, null);
                        }
                    }
                    else
                    {
                        _Status = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(_Status, Message, null);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //After appoval images from tbl_applicant_qualification
        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult DownloadQualData()
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String URN = String.Empty;
            String ReqType = String.Empty;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]);
                ReqType = Convert.ToString(Request.Form["Type"]);
                if (!ReqType.In(new string[] { "B", "P" }))
                {
                    throw new Exception("Invalid Request Type : " + ReqType);
                }
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (_Status)
            {
                try
                {
                    objURN = new IIIBL.URN();
                    objDataSet = objURN.GetQualData(PortalApplication.ConnectionString, URN, PortalSession.UserID);

                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            DataRow dt = objDataTable.Rows[0];
                            byte[] bytes = null;
                            if (ReqType == "B")
                            {
                                if (dt["imgBasicQualification"] != DBNull.Value)
                                {
                                    bytes = (byte[])dt["imgBasicQualification"];
                                }
                            }
                            if (ReqType == "P")
                            {
                                if (dt["imgProfessionalQualification"] != DBNull.Value)
                                {
                                    bytes = (byte[])dt["imgProfessionalQualification"];
                                }
                            }
                            if (bytes == null)
                            {
                                _Status = false;
                                Message = CommonMessages.NO_DATA_FOUND + "\nThe Qualification Details are not available for this URN";
                                s = HelperUtilities.ToJSON(_Status, Message, null);
                            }
                            else
                            {
                                _Status = true;
                                Message = String.Empty;

                                String Filename = PortalSession.UserLoginID + "_Qual_" + URN + "_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".jpg";
                                String OutputFileName = String.Format("../Downloads/{0}", Filename);
                                String OutputFileName2 = Server.MapPath(OutputFileName);

                                System.IO.File.WriteAllBytes(OutputFileName2, bytes);

                                KVPair = new KeyValuePair<String, String>[1];
                                KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                                s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                            }
                        }
                        else
                        {
                            _Status = false;
                            Message = CommonMessages.NO_DATA_FOUND + "\nThe Qualification Details are not available for this URN";
                            s = HelperUtilities.ToJSON(_Status, Message, null);
                        }
                    }
                    else
                    {
                        _Status = false;
                        Message = CommonMessages.NO_DATA_FOUND + "\nThe Qualification Details are not available for this URN";
                        s = HelperUtilities.ToJSON(_Status, Message, null);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult ApproveRejectURN()
        {
            JsonResult objJsonResult = null;
            IIIBL.URN objURN = null;
            //DataSet objDataSet = null;
            //DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = true;
            String Status = String.Empty;
            String Message = String.Empty;

            Int64 Id = 0;
            String sStatus = String.Empty;
            String Remarks = String.Empty;
            try
            {
                String _dummy = Convert.ToString(Request.Form["hddId"]);
                Id = Convert.ToInt32(_dummy);

                sStatus = Convert.ToString(Request.Form["cboStatus2"]).Trim();
                if (!sStatus.In(new string[] { "A","R"}))
                {
                    throw new Exception( "Invalid status");
                }

                Remarks = Convert.ToString(Request.Form["txtRemarks"]).Trim();
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Success)
            {
                try
                {
                    objURN = new URN();
                    Message = objURN.ApproveRejectURN(PortalApplication.ConnectionString, Id, PortalSession.UserID, Remarks , sStatus);
                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        if (sStatus == "A")
                        {
                            Message = "The data has been approved successfully";
                        }
                        if (sStatus == "R")
                        {
                            Message = "The data has been rejected successfully";
                        }
                    }
                    else
                    {
                        Success = false;
                        if (sStatus == "A")
                        {
                            Message = CommonMessages.DATA_APPROVAL_FAIL + " : " + Message;
                        }
                        if (sStatus == "R")
                        {
                            Message = CommonMessages.DATA_REJECTION_FAIL + " : " + Message;
                        }
                    }
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    //objDataTable = null;
                    //objDataSet = null;
                    objURN = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult UpdatePS()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult UpdatePS(String dummy = "")
        {
            String[] FieldsInput = new String[] { "URN", "photo_file_name", "sign_file_name" };
            String[] FieldsOutput = new String[] { "urn", "photo_file_name", "sign_file_name", "remarks" };
            String[] FieldHeaders = new String[] { "URN", "Photo file name", "sign file name", "remarks" };
            String[] FieldsFMT = new String[] { String.Empty, String.Empty, String.Empty, String.Empty };

            IIIBL.URN objURN = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;
            try
            {
                    if (Path.GetExtension(Request.Files[0].FileName) == ".zip")
                    {
                        String OriginalFileName = Request.Files[0].FileName;
                        String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                        String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                        String FilePath = String.Format("../Uploads/{0}", FileName);
                        String OutputDirectoryName = String.Format("../Uploads/{0}", FileNamePart);

                        String OutputFileNamePart = PortalSession.UserLoginID + "_UploadPSResponse_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                        String OutputFileName = String.Format("../Downloads/{0}", OutputFileNamePart + ".xlsx"); // ToString be passed back outside 
                        String OutputFileName2 = Server.MapPath(OutputFileName); // for inner processing

                        OutputDirectoryName = Server.MapPath(OutputDirectoryName);
                        FilePath = Server.MapPath(FilePath);
                        Request.Files[0].SaveAs(FilePath);

                        HelperUtilities.UnzipFile(FilePath, OutputDirectoryName);

                        objURN = new IIIBL.URN();
                        objDataSet = objURN.UpdatePS(PortalApplication.ConnectionString, OriginalFileName, PortalSession.UserID, OutputDirectoryName, out Message);
                        DataTable objDataTable = null;
                        KeyValuePair<String, String>[] KVPair = null;

                        objDataTable = objDataSet.Tables[0];

                        XLXporter.WriteExcel(OutputFileName2, objDataTable, FieldsOutput, FieldsOutput, FieldsFMT);

                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);

                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);

                        try
                        {
                            System.IO.File.Delete(FilePath);
                        }
                        catch (Exception ex)
                        {
                            Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                        }
                    }
                    else
                    {
                        s = HelperUtilities.ToJSON(false, "Invalid file type. Please refer to the template");
                        //Invalid File
                    }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                objURN = null;
            }

            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }


        #endregion

        #region exam center update
        [HttpGet]
        [AuthorizeExt]
        public ActionResult UrnExamCentreUpdate()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult UploadUrnExamCenterUpdate()
        {
            String[] Fields = new String[] { "urn", "test_center", "center_id", "status" };
            String[] FieldsFmt = new String[] { String.Empty, String.Empty, String.Empty, String.Empty };

            IIIBL.URN objURN = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;

            try
            {
                String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                String FilePath = String.Format("../Uploads/{0}", FileName);

                String OutputFileNamePart = PortalSession.UserLoginID + "_UpdateExamCenterReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");
                String OutputFileNameExcel = String.Format("../Downloads/{0}", OutputFileNamePart + ".xlsx");
                String OutputFileNameExcel2 = Server.MapPath(OutputFileNameExcel);

                FilePath = Server.MapPath(FilePath);
                Request.Files[0].SaveAs(FilePath);

                objURN = new IIIBL.URN();
                objDataSet = objURN.UploadUrnExamCentreUpdate(PortalApplication.ConnectionString, PortalSession.UserID, FilePath, out Message);
                DataTable objDataTable = null;
                KeyValuePair<String, String>[] KVPair = null;
                if (Message.Trim() == String.Empty)
                {
                    if (objDataSet == null)
                    {
                        s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + CommonMessages.NULL_DATASET );
                    }
                    else
                    {
                        if (objDataSet.Tables.Count == 0)
                        {
                            s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + CommonMessages.EMPTY_DATASET);
                        }
                        else
                        {
                            objDataTable = objDataSet.Tables[0];
                            XLXporter.WriteExcel(OutputFileNameExcel2, objDataTable, Fields, Fields, FieldsFmt);
                            KVPair = new KeyValuePair<String, String>[1];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileNameExcel);
                            s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                    }

                }
                else // (Status == "SUCCESS")
                {
                    s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                }

                try
                {
                    System.IO.File.Delete(FilePath);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                }

            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                objURN = null;
            }

            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }
        #endregion

        [HttpGet]
        [AuthorizeExt]
        public ActionResult BulkUploadTrainingDetails()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult UploadTrainingDetails()
        {
            IIIBL.URN objURN = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;
            DataTable objDataTable = null;
            try
            {
                //if (PortalSession.RoleCode == "superadmin")
                {
                    String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                    String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                    String FilePath = String.Format("../Uploads/{0}", FileName);
                    String OutputFileNamePart = PortalSession.UserLoginID + "_TrainingDetailsUpdate_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");
                    String OutputFileName = String.Format("../Downloads/{0}", FileNamePart + ".csv");
                    String OutputFileName2 = Server.MapPath(OutputFileName);
                    FilePath = Server.MapPath(FilePath);
                    Request.Files[0].SaveAs(FilePath);

                    objURN = new IIIBL.URN();
                    objDataSet = objURN.UploadTrainingDetails(PortalApplication.ConnectionString, FilePath, PortalSession.UserID, out Message);
                    KeyValuePair<String, String>[] KVPair = null;
                    if (Message.Trim() == string.Empty)
                    {
                        objDataTable = objDataSet.Tables[0];
                        String[] Headers = new string[] { "urn|training_start_date|training_end_date|training_hours|tcc_expiry_date" };
                        String[] Fields = new string[] { "ORIGINAL_ROW","MESSAGE" };
                        objDataTable.DataTable2File(Headers, Fields, OutputFileName2,   "|");
                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                    }
                    else
                    {
                        s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                    }

                    try
                    {
                        System.IO.File.Delete(FilePath);
                    }
                    catch (Exception ex)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {

            }
            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }

        #region Payment Receipt download For Individual Candidate
        //OK
        [HttpGet]
        public ActionResult PaymentReceipt()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        //OK
        [HttpPost]
        public ActionResult GetPaymentReceipt(String Dummy = "")
        {
            String NseitLogoImagePath = Server.MapPath("~/Images/PaymentReceiptsFolder");
            //String NseitYearImagePath = Server.MapPath("~/Images/PaymentReceiptsFolder/nseit-year-image.jpg");
            String SignatureFolder = Server.MapPath("~/Images/PaymentReceiptsFolder");
            Boolean Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.HSTCPrinter objHSTCPrinter = null;
            IIIBL.URN objUrn = null;
            Boolean ValResp = true;

            String URN = String.Empty;
            DateTime DOB = DateTime.MinValue;
            DateTime ExamDate = DateTime.MinValue;
            String OutputFile = String.Empty;
            Boolean IsOutputZip = false;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]).Trim();

                String _dummy = Convert.ToString(Request.Form["txtDOB"]);
                if (_dummy.Trim() == String.Empty)
                {
                    DOB = DateTime.MinValue;
                }
                else
                {
                    DOB = Convert.ToDateTime(_dummy);
                }


                if (DOB == DateTime.MinValue)
                {
                    throw new Exception("Invalid DOB Values");
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Status)
            {
                try
                {

                    objUrn = new IIIBL.URN();
                    ValResp = objUrn.ValidateURNandDOB(PortalApplication.ConnectionString, URN, DOB);
                    if (ValResp)
                    {
                        String GuidName = Guid.NewGuid().ToString().Replace("-", String.Empty);
                        String OutputURL = "../Downloads/";
                        String DisplayFileName = String.Empty;

                        //Uri uri = new Uri(OutputURL);
                        String OutputPath = Server.MapPath("~/Downloads");
                        objHSTCPrinter = new HSTCPrinter();
                        String Results = objHSTCPrinter.PrintReceipt(PortalApplication.OAIMSConnectionString, URN, NseitLogoImagePath, SignatureFolder, OutputPath, GuidName);
                        if (Results == "SUCCESS")
                        {
                            Status = true;
                            Message = "";
                            KVPair = new KeyValuePair<String, String>[2];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_DISPLAY_FILE_NAME_", "PR_" + URN + ".zip");
                            KVPair[1] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputURL + "/" + GuidName + ".zip");
                            s = HelperUtilities.ToJSON(Status, Message, null, KVPair);
                        }
                        else if (Results == "NO_DATA_FOUND")
                        {
                            Status = false;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(Status, Message);
                        }
                        else
                        {
                            Status = false;
                            Message = Results;
                            s = HelperUtilities.ToJSON(Status, Message);
                        }
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Status, Message);
                    }

                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Status, Message);
                }
                finally
                {
                    objHSTCPrinter = null;
                }
            }
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        #endregion
    }
}