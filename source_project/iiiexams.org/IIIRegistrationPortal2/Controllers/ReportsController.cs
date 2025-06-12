using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using IIIBL;
using System.Text;
using System.Reflection;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office.MetaAttributes;
using System.Web.Http.Results;

namespace IIIRegistrationPortal2.Controllers
{
    public partial class ReportsController : Controller
    {

        [AuthorizeExt]
        [HttpGet]
        public ActionResult CorporateAgentExaminationReport()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [AuthorizeExt]
        [HttpPost]
        public JsonResult CorporateAgentExaminationReport(string dummy = "")
        {
            JsonResult objJsonResult = null;
            String exammonth = Convert.ToString(Request.Form["dropdownMonth"]);
            Int32 examyear = Convert.ToInt32(Request.Form["dropdownYear"]);
            exammonth = exammonth.Substring(0, 3);
            IIIBL.ExamReports objExamReports = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            try
            {
                objExamReports = new ExamReports();
                objDataSet = objExamReports.GetCorporateExaminationReport(PortalApplication.ConnectionString, exammonth, examyear, PortalSession.RoleName);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;

                        String Filename = Guid.NewGuid().ToString().Replace("-","");
                        String OutputFileName = String.Format("../Downloads/{0}", Filename + ".csv");
                        String OutputZipFileName = String.Format("../Downloads/{0}", Filename + ".zip");

                        String OutputFileName2 = Server.MapPath(OutputFileName);
                        String OutputZipFileName2 = Server.MapPath(OutputZipFileName);

                        objDataTable.DataTable2File(OutputFileName2, ",");
                        HelperUtilities.ZipTheFile(OutputZipFileName2, OutputFileName2);

                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputZipFileName);

                        Message = CommonMessages.REPORT_PROCESSING_SUCCESS;
                        s = HelperUtilities.ToJSON(false, Message, null, KVPair);
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Success, Message, null);
                }
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
                objExamReports = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ApprovedCorporateAgent()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [AuthorizeExt]
        [HttpPost]
        public JsonResult ApprovedCorporateAgent(string dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.ExamReports obj = null;
            DataTable objDataTable = null;
            DataSet objDataSet = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;

            try
            {
                obj = new ExamReports();
                objDataSet = obj.GetApprovedCorporateAgent(PortalApplication.ConnectionString);

                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;

                        String Filename = Guid.NewGuid().ToString().Replace("-","");
                        String OutputFileName = String.Format("../Downloads/{0}", Filename + ".xlsx");

                        String OutputFileName2 = Server.MapPath(OutputFileName);

                        String[] DisplayColumns = new string[] { "company_name", "address", "std_code", "landline_no", "mobile_po", "email_po", "name_po", "corporate_user_id", "intermediary_type" };
                        String[] DisplayHeaders = new string[] { "Company Name", "Address", "STD Code", "Landline Number", "Mobile Number of Principal Officer / Designated Person", "Email ID of Principal Officer / Designated Person", "Name of Principal Officer / Designated Person", "Corporate User ID", "Intermediary Type" };
                        String[] DisplayFormat = new string[] { String.Empty, String.Empty,  String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

                        XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormat);

                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.REPORT_PROCESSING_SUCCESS_MIN, null, KVPair);
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Success, Message, null);
                }
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
                obj = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeExt]
        [HttpGet]
        public ActionResult SponsorshipStatus()
        {
            IIIBL.MasterData objMasterData = null;

            String ViewName = String.Empty;
            try
            {
                objMasterData = new MasterData();
                DataSet objExamCenters = objMasterData.GetExamCenter(PortalApplication.ConnectionString, 5, -1);

                if (PortalSession.RoleCode == "CA" || PortalSession.RoleCode == "WA" || PortalSession.RoleCode == "IMF" || PortalSession.RoleCode == "BR")
                {
                    ViewName = "SponsorshipStatusForCorporate";
                }
                else if (PortalSession.RoleCode == "I")
                {
                    ViewName = "SponsorshipStatus";

                    DataSet objCDP = null;
                    DataSet objDP = null;
                    DataSet objAC = null;
                    if (PortalSession.RoleName == "Corporate Designated Person")
                    {
                        objCDP = objMasterData.GetInsurers(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                        objDP = objMasterData.GetDPForInsurer(PortalApplication.ConnectionString, PortalSession.InsurerUserID, -1);
                        ViewBag.CDP = objCDP.Tables[0];
                        ViewBag.DP = objDP.Tables[0];
                    }
                    else if (PortalSession.RoleName == "Designated Person")
                    {
                        objCDP = objMasterData.GetInsurers(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                        objDP = objMasterData.GetDPForInsurer(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.DPUserID);
                        objAC = objMasterData.GetACForDPs(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.DPUserID, -1);
                        ViewBag.CDP = objCDP.Tables[0];
                        ViewBag.DP = objDP.Tables[0];
                        ViewBag.AC = objAC.Tables[0];
                    }
                    else if (PortalSession.RoleName == "Agent Counselor")
                    {
                        objCDP = objMasterData.GetInsurers(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                        objDP = objMasterData.GetDPForInsurer(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.DPUserID);
                        objAC = objMasterData.GetACForDPs(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.DPUserID, PortalSession.AgentCounselorUserID);
                        ViewBag.CDP = objCDP.Tables[0];
                        ViewBag.DP = objDP.Tables[0];
                        ViewBag.AC = objAC.Tables[0];
                    }
                    else
                    {
                        objCDP = objMasterData.GetInsurers(PortalApplication.ConnectionString, -1);
                        ViewBag.CDP = objCDP.Tables[0];
                    }
                }
                else
                {
                    ViewName = String.Empty;
                }
                ViewBag.ExamCenters = objExamCenters.Tables[0];
            }
            catch (Exception ex)
            {

            }
            finally
            {
                objMasterData = null;
            }

            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View(ViewName);
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SponsorshipStatusP()
        {
            JsonResult objJsonResult = null;

            Int32       InsurerUserId       = Convert.ToInt32(Request.Form["cboInsurer"]);
            Int32       DPUserId            = Convert.ToInt32(Request.Form["cboDP"]);
            Int32       ACUserId            = Convert.ToInt32(Request.Form["cboAgentCounsellor"]);

            String      InsurerRefNo        = Convert.ToString(Request.Form["txtInsurersRefNo"]);
            String      ExamBatch         = Convert.ToString(Request.Form["txtExaminationBatchID"]);
            DateTime    ApplicationDateFrom = Convert.ToDateTime(Request.Form["txtAppDateFrom"]);
            DateTime    ApplicationDateTo   = Convert.ToDateTime(Request.Form["txtAppDateTo"]);
            String      ExamDateFrom        = Convert.ToString(Request.Form["txtExamDateFrom"]);
            String      ExamDateTo          = Convert.ToString(Request.Form["txtExamDateTo"]);
            Int32       ExamBodyId          = Convert.ToInt32(Request.Form["cboExaminationBody"]);
            Int32       ExamCenterId        = Convert.ToInt32(Request.Form["cboExaminationCenter"]);
            String      ApplicantStatus     = Convert.ToString(Request.Form["cboApplicationStatus"]);
            // Process this...
            String[] Status = ApplicantStatus.Split(',');
            Boolean StatusAll       = false;
            Boolean StatusSponsored = false;
            Boolean StatusTrained   = false;
            Boolean StatusEC        = false;
            Boolean StatusEA        = false;
            Boolean StatusE         = false;

            foreach (String str in Status)
            {
                switch (str)
                {
                    case "A":
                        StatusAll = true;
                        break;
                    case "S":
                        StatusSponsored = true;
                        break;
                    case "T":
                        StatusTrained = true;
                        break;
                    case "EC":
                        StatusEC = true;
                        break;
                    case "EA":
                        StatusEA = true;
                        break;
                    case "E":
                        StatusE = true;
                        break;
                    default:
                        break;
                }
            }
            //A / S / T / EC / EA / E
            Boolean Photo = false;
            
            if ( Request.Form["chkPhoto"] == "on" || Request.Form["chkPhoto"] == "true" )
            {
                Photo = true;
            }

            Boolean Sign = false;
            if (Request.Form["chkSign"] == "on" || Request.Form["chkSign"] == "true")
            {
                Sign = true;
            }

            String      URN                 = Convert.ToString(Request.Form["txtURN"]);

            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.ExamReports objExamReports = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            try
            {
                objExamReports = new ExamReports();
                if ( PortalSession.RoleCode == "I")
                {
                    objDataSet = objExamReports.GetSponsorshipReport(PortalApplication.ConnectionStringReports, ApplicationDateFrom, ApplicationDateTo, PortalSession.RoleName, 0, InsurerUserId, DPUserId, ACUserId, URN, 
                        InsurerRefNo, ExamBatch, ExamBodyId, ExamCenterId,
                        StatusAll, StatusSponsored, StatusTrained, StatusEC, StatusEA, StatusE, Photo, Sign, ExamDateFrom, ExamDateTo);
                }
                else
                {
                    objDataSet = objExamReports.GetSponsorshipReportForCorporates(PortalApplication.ConnectionString, PortalSession.UserID , ApplicationDateFrom, ApplicationDateTo, PortalSession.RoleName, URN, InsurerRefNo, ExamBatch, ExamBodyId, ExamCenterId,
                        StatusAll, StatusSponsored, StatusTrained, StatusEC, StatusEA, StatusE, Photo, Sign, ExamDateFrom, ExamDateTo);
                }

                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;

                        String Filename = PortalSession.UserLoginID + "_SponsorshipStatusReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                        //String Filename = Guid.NewGuid().ToString().Replace("-","");
                        String OutputDirName = String.Format("../Downloads/{0}", Filename );
                        String OutputZipFileName = String.Format("../Downloads/{0}", Filename + ".zip");

                        String Parent = Server.MapPath("../Downloads");
                        DirectoryInfo dirParent = new DirectoryInfo(Parent);
                        dirParent.CreateSubdirectory(Filename);

                        String OutputDirName2 = Server.MapPath(OutputDirName);
                        String OutputZipFileName2 = Server.MapPath(OutputZipFileName);

                        DirectoryInfo oDir = new DirectoryInfo(OutputDirName2);


                        String strCSVPath = oDir.FullName + "\\" + Filename + ".csv"; //"SponsorShipStatus_" + DateTime.Today.ToString("ddMMMyy") + ".csv";

                        String ImageFilePath = String.Empty;
                        if (Photo)
                        {
                            DirectoryInfo oPhotoDir = oDir.CreateSubdirectory("Photo");
                            foreach (DataRow oRow in objDataTable.Rows)
                            {
                                ImageFilePath = oPhotoDir.FullName + "\\Photo_" + oRow["IRDA URN"].ToString() + ".jpg";
                                System.IO.File.WriteAllBytes(ImageFilePath, (byte[])oRow["imgApplicantPhoto"]);
                            }
                            objDataTable.Columns.Remove("imgApplicantPhoto");
                        }
                        else
                        {
                            objDataTable.Columns.Remove("Photo File Name");
                            objDataTable.Columns.Remove("imgApplicantPhoto");
                        }
                        if (Sign)
                        {
                            DirectoryInfo oPhotoDir = oDir.CreateSubdirectory("Signature");
                            foreach (DataRow oRow in objDataTable.Rows)
                            {
                                ImageFilePath = oPhotoDir.FullName + "\\Sign_" + oRow["IRDA URN"].ToString() + ".jpg";
                                System.IO.File.WriteAllBytes(ImageFilePath, (byte[])oRow["imgApplicantSign"]);
                            }
                            objDataTable.Columns.Remove("imgApplicantSign");
                        }
                        else
                        {
                            objDataTable.Columns.Remove("Sign File Name");
                            objDataTable.Columns.Remove("imgApplicantSign");
                        }
                        String Aadhaar = String.Empty;
                        foreach (DataRow row in objDataTable.Rows)
                        {
                            Aadhaar = Convert.ToString(row["Aadhaar Number"]);
                            if (Aadhaar != String.Empty)
                            {
                                Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
                                Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());
                                row["Aadhaar Number"] = IIIBL.AadhaarEncryptorDecryptor.DecryptAadhaar(Aadhaar, key, IV);
                            }
                        }

                        objDataTable.DataTable2File(strCSVPath, ",");
                        HelperUtilities.ZipTheDirectory(OutputDirName2, OutputZipFileName2);

                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputZipFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Success, Message, null);
                }
            }
            catch(Exception ex)
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
                objExamReports = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ExaminationReport()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult ExaminationReport(String Dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.ExamReports objreport = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;

            Int32 Option = 0;
            Boolean Status = true;
            try
            {
                String dummy = Convert.ToString (Request.Form["Option"]);
                Option = Convert.ToInt32(dummy);
                if (Option != 1 && Option != 2 && Option != 3)
                {
                    throw new Exception("Invalid Option");
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
                    objreport = new ExamReports();
                    objDataSet = objreport.GetExaminationReport(PortalApplication.ConnectionString, Option , PortalSession.UserID);
                    objDataTable = objDataSet.Tables[0];

                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        if (objDataTable.Rows.Count > 0)
                        {
                            DataTable dataTableCSV = objDataTable.Copy(); //For CSV output.

                            String[] Fields = null;
                            String[] Formats = null;

                            String Filename = String.Empty;
                            if (Option == 1)
                            {
                                dataTableCSV.Columns.Remove("Application Date");
                                dataTableCSV.Columns.Remove("Exam Date");
                                dataTableCSV.Columns["Application Date csv"].ColumnName = "Application Date";
                                dataTableCSV.Columns["Exam Date csv"].ColumnName = "Exam Date";

                                Filename = PortalSession.UserLoginID + "_Last2DaysExaminationReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                                Fields = new string[] { "Application Date", "URN", "Applicant Name", "Exam Date", "Exam Roll Number", "Exam Marks", "Result" };
                                Formats = new string[] { "dd-MMM-yyyy", string.Empty, string.Empty, "dd-MMM-yyyy hh:mm:ss AM/PM", string.Empty, string.Empty, string.Empty };

                            }
                            if (Option == 2)
                            {
                                Filename = PortalSession.UserLoginID + "_CandidatesRegisterdForExam_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                                if (PortalSession.RoleCode.In(new string[] { "CA","WA","IMF","BR"}))
                                {
                                    Fields = new string[] { "Application Date", "URN", "Applicant Name", "Training Start Date", "Training End Date", "TCC Expiry Date", "Total Training Hrs", "Center Name", "Preferred Date", "Payment mode", "Scheduling mode", "Exam Date" };
                                    Formats = new string[] { "dd-MMM-yyyy", string.Empty, string.Empty, "dd-MMM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy", string.Empty, string.Empty, "dd-MMM-yyyy", string.Empty, string.Empty, "dd-MMM-yyyy hh:mm:ss AM/PM" };
                                }
                                else
                                {
                                    Fields = new string[] { "Application Date", "URN", "Applicant Name", "Center Name", "Preferred Date", "Payment mode", "Scheduling mode", "Exam Date" };
                                    Formats = new string[] { "dd-MMM-yyyy", string.Empty, string.Empty, string.Empty, "dd-MMM-yyyy", string.Empty, string.Empty, "dd-MMM-yyyy hh:mm:ss AM/PM" };
                                }
                            }
                            if (Option == 3)
                            {
                                Filename = PortalSession.UserLoginID + "_CandidatesRegisterdForExam_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                                if (PortalSession.RoleCode.In(new string[] { "CA", "WA", "IMF", "BR" }))
                                {
                                    Fields = new string[] { "Application Date", "URN", "Applicant Name", "Training Start Date", "Training End Date", "TCC Expiry Date", "Total Training Hrs" };
                                    Formats = new string[] { "dd-MMM-yyyy", string.Empty, string.Empty, "dd-MMM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy", string.Empty };
                                }
                                else
                                {
                                    Fields = new string[] { "Application Date", "URN", "Applicant Name", "Center Name" };
                                    Formats = new string[] { "dd-MMM-yyyy", string.Empty, string.Empty, string.Empty };
                                }
                            }


                            //String Filename = Guid.NewGuid().ToString();
                            String OutputFileName = String.Format("../Downloads/{0}", Filename + ".xlsx");
                            String OutputFileName2 = Server.MapPath(OutputFileName);

                            String OutputFileNameCSV = String.Format("../Downloads/{0}", Filename + ".csv");
                            String OutputFileName2CSV = Server.MapPath(OutputFileNameCSV);

                            XLXporter.WriteExcel(OutputFileName2, objDataTable, Fields, Fields, Formats);

                            dataTableCSV.DataTable2File(OutputFileName2CSV, "\t");

                            KVPair = new KeyValuePair<String, String>[2];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                            KVPair[1] = new KeyValuePair<string, string>("_RESPONSE_FILE2_", OutputFileNameCSV);

                            s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.NO_DATA_FOUND_FOR_SELECTED_CRITERIA;
                            s = HelperUtilities.ToJSON(Success, Message, null);
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND_FOR_SELECTED_CRITERIA;
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
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
                    objreport = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ScheduleReport()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult ScheduleReport(String dummy = "")
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.Scheduling objScheduling = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            Int32 Hint = -1;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["cboReportType"]);
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid cboReportType");
                }
                else
                {
                    Hint = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["txtFromDate"]); 
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtFromDate");
                }
                else
                {
                    FromDate = Convert.ToDateTime(_dummy.Trim());
                }

                _dummy = Convert.ToString(Request.Form["txtToDate"]); ;
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtToDate");
                }
                else
                {
                    ToDate = Convert.ToDateTime(_dummy.Trim());
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
                    objScheduling = new IIIBL.Scheduling();
                    objDataSet = objScheduling.GetScheduleReport(PortalApplication.ConnectionString,Hint, FromDate, ToDate);

                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            _Status = true;
                            Message = String.Empty;

                            String Filename = PortalSession.UserLoginID + "_ScheduleReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                            String OutputFileName = String.Format("../Downloads/{0}", Filename);
                            String OutputFileName2 = Server.MapPath(OutputFileName);

                            String[] DisplayColumns = null;
                            String[] DisplayHeaders = null;
                            String[] DisplayFormat = null;
                            switch ( Hint )
                            {
                                case 1: //date wise count
                                    DisplayColumns = new String[] { "exam_date", "candidate_count" };
                                    DisplayHeaders = new String[] { "Exam Date", "Candidate Count" };
                                    DisplayFormat = new String[] { String.Empty, String.Empty };

                                    break;

                                case 2: //Date wise / center wise count
                                    DisplayColumns = new String[] { "exam_date","center_id", "center_name", "candidate_count" };
                                    DisplayHeaders = new String[] { "Exam Date", "Center Id", "Center Name", "Candidate Count" };
                                    DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty, String.Empty };
                                    break;

                                case 3: //Date wise / center wise / batch wise count
                                    DisplayColumns = new String[] { "exam_date", "center_id", "center_name","exam_batch", "candidate_count" };
                                    DisplayHeaders = new String[] { "Exam Date", "Center Id", "Center Name","Exam Batch", "Candidate Count" };
                                    DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty,String.Empty, String.Empty };
                                    break;

                                default:
                                    throw new Exception("Invalid Hint");
                                    break;
                            }


                            XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormat);
                            
                            //objDataTable.DataTable2File(OutputFileName2, "\t");

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

        [HttpGet]
        [AuthorizeExt]
        public ActionResult CenterWisePendingScheduleCountReport()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult CenterWisePendingScheduleCountReport(String dummy = "")
        {
            JsonResult objJsonResult = null;
            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            try
            {
                objBatchMgmt = new IIIBL.BatchMgmt();
                objDataSet = objBatchMgmt.GetCenterWisePendingScheduleCount(PortalApplication.ConnectionString);

                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        _Status = true;
                        Message = String.Empty;

                        String Filename = PortalSession.UserLoginID + "_CenterWisePendingScheduleCountReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                        String OutputFileName = String.Format("../Downloads/{0}", Filename);
                        String OutputFileName2 = Server.MapPath(OutputFileName);

                        XLXporter.WriteExcel(OutputFileName2, objDataTable);

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
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult PaymentReport()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }


        [AuthorizeExt]
        [HttpPost]
        public ActionResult PaymentReport(String dummy = "")
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.ExamReports objReport = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            Int32 Hint = -1;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            try
            {
                String _dummy = String.Empty;
                _dummy = Convert.ToString(Request.Form["txtFromDate"]);
                if (_dummy == null || _dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtFromDate");
                }
                else
                {
                    FromDate = Convert.ToDateTime(_dummy.Trim());
                }
                _dummy = Convert.ToString(Request.Form["txtToDate"]);
                if (_dummy == null || _dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtToDate");
                }
                else
                {
                    ToDate = Convert.ToDateTime(_dummy.Trim());
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
                    objReport = new IIIBL.ExamReports();
                    objDataSet = objReport.GeneratePaymentReport(PortalApplication.OAIMSConnectionString, PortalSession.TopUserLoginID, FromDate, ToDate);

                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            _Status = true;
                            Message = String.Empty;

                            String Filename = PortalSession.UserLoginID + "_PaymentReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                            String OutputFileName = String.Format("../Downloads/{0}", Filename);
                            String OutputFileName2 = Server.MapPath(OutputFileName);

                            String[] DisplayColumns = new String[] { "ROWNUM", "PG_TRANS_Id", "TRANSACTION_DATE", "TRANSACTION_TIME", "EED_TXNS_ID", "EED_EXM_IRDA_URN", "APPLICATION_DATE", "AMOUNT" };
                            String[] DisplayHeaders = new String[] { "Sr. No.", "Transaction Id", "Transaction Date", "Transaction Time", "'Batch ID", "URN Details", "Application Date", "Transaction Amount" };
                            String[] FieldsFormat = new String[] { String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty, String.Empty, String.Empty, "dd-MMM-yyyy", String.Empty };
                            XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, FieldsFormat);
                            KVPair = new KeyValuePair<String, String>[2];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                            KVPair[1] = new KeyValuePair<string, string>("_RESPONSE_FILE_NAME_", Filename);
                            s = HelperUtilities.ToJSON(_Status, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                        else
                        {
                            _Status = false;
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

        [HttpGet]
        [AuthorizeExt]
        public ActionResult PaymentReceipts()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult RequestPaymentReceipts(String Dummy="")
        {
            JsonResult objJsonResult = null;
            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.ReportsInfra objReports = null;
            //DataSet objDataSet = null;
            Dictionary<string, string> objParams = null;
            try
            {
                objParams = new Dictionary<string, string>();

                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["txtMonth"]);
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtMonth");
                }

                objParams.Add("Month", _dummy);
                if (PortalSession.RoleCode.In(new string[] {"CA","WA","IMF","BR"}))
                {
                    objParams.Add("CompanyLoginCode", PortalSession.UserLoginID);
                }
                else if (PortalSession.RoleCode.In(new string[] { "I"}))
                {
                    objParams.Add("CompanyLoginCode", PortalSession.InsurerCode);
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
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
                    string ParamsJsonString = javaScriptSerializer.Serialize(objParams);

                    objReports = new IIIBL.ReportsInfra();
                    Message = objReports.SaveReportRequest(PortalApplication.ConnectionString, PortalSession.InsurerUserID, "PAYMENT_RECEIPT", ParamsJsonString);

                    if (Message != String.Empty)
                    {
                        _Status = true;
                        s = HelperUtilities.ToJSON(true, Message, null);
                    }
                    else
                    {
                        _Status = true;
                        Message = CommonMessages.ERROR_OCCURED;
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
                    objReports = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;


        }
    }
}