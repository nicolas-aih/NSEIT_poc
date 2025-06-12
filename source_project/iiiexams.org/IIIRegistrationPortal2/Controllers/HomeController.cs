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
using DotNetIntegrationKit;
using System.Globalization;
using System.Reflection;

namespace IIIRegistrationPortal2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Reset()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            RedirectResult redirectResult = new RedirectResult("~/Home/Index");
            return redirectResult;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IIIBL.MasterData objMasterData = null; //To be taken from application 
            DataSet objDataSet = null;
            try
            {
                objMasterData = new MasterData();
                objDataSet = objMasterData.GetNotifications(PortalApplication.ConnectionString, "N", PortalSession.RoleCode);
                if (objDataSet != null && objDataSet.Tables.Count != 0)
                {
                    ViewBag.Notifications = objDataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                objMasterData = null;
            }

            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            return View("Error");
        }

        ////OK
        //[HttpGet]
        //public ActionResult Loginrequest()
        //{
        //    ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
        //    return View();
        //}

        ////OK --> Part pending.
        //[HttpPost]
        //public JsonResult Loginrequest(String CompanyType, String CompanyName, String CompanyAddress, String STDCode, String Landline, String Mobile,
        //        String EmailId, String POName
        //    )
        //{
        //    JsonResult objJsonResult = null;
        //    Boolean Status = false;
        //    String Message = String.Empty;
        //    IIIBL.Users objUsers = null;
        //    Int64 RequestId = 0;
        //    try
        //    {
        //        //Validate
        //        if (CompanyName.Trim() == String.Empty)
        //        {
        //            Message += "Please enter company name";
        //        }
        //        if (CompanyAddress.Trim() == String.Empty)
        //        {
        //            Message += "Please enter company address";
        //        }
        //        if (STDCode.Trim() == String.Empty)
        //        {
        //            Message += "Please enter std code";
        //        }
        //        if (Landline.Trim() == String.Empty)
        //        {
        //            Message += "Please enter landline number";
        //        }
        //        if (EmailId.Trim() == String.Empty)
        //        {
        //            Message += "Please enter email id";
        //        }
        //        if (POName.Trim() == String.Empty)
        //        {
        //            Message += "Please enter name of principal officer / designated persosn";
        //        }

        //        //Change
        //        if (Message == String.Empty)
        //        {
        //            objUsers = new IIIBL.Users();
        //            Message = objUsers.SaveLoginRequest(PortalApplication.ConnectionString, CompanyName, CompanyType, CompanyAddress, STDCode, Landline, Mobile, EmailId, POName, out RequestId);
        //            Status = (Message.Trim() == String.Empty);
        //            if (Status)
        //            {
        //                //Set Message.
        //                Message = String.Format("Your request for login is saved successfully and the request id is {0}", RequestId);
        //                //Send Mail.

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objUsers = null;
        //        Status = false;
        //        Message = CommonMessages.ERROR_OCCURED;
        //    }
        //    finally
        //    {
        //        objUsers = null;
        //    }

        //    String s = HelperUtilities.ToJSON(Status, Message);
        //    objJsonResult = new JsonResult();
        //    objJsonResult.Data = s;
        //    return objJsonResult;
        //}

        //OK
        [HttpGet]
        public ActionResult Syllabus()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        //OK
        [HttpGet]
        public ActionResult ExamCenters()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        ////OK
        //[HttpGet]
        //public ActionResult TBXExamCenters()
        //{
        //    ViewBag.SelectedStateId = -1;
        //    ViewBag.States = PortalApplication.States;
        //    ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
        //    ViewBag.ClassName = "col-sm-6";
        //    ViewBag.IsPostback = false;
        //    return View();
        //}

        ////OK
        //[HttpPost]
        //public ActionResult TBXExamCenters(String Dummy = "")
        //{
        //    Int32 StateId = Convert.ToInt32(Request.Form["States"]);
        //    ViewBag.SelectedStateId = StateId;
        //    IIIBL.ExamCenters objExamCenters = null;
        //    DataSet objDataSet2 = null;
        //    DataTable objDataTable2 = null;
        //    try
        //    {
        //        objExamCenters = new IIIBL.ExamCenters();
        //        objDataSet2 = objExamCenters.ExamCentersForState(PortalApplication.ConnectionString, StateId, true);
        //        if (objDataSet2 != null && objDataSet2.Tables.Count != 0)
        //        {
        //            objDataTable2 = objDataSet2.Tables[0];
        //            if (objDataTable2.Rows.Count == 0)
        //            {
        //                ViewBag._STATUS_ = true;
        //                ViewBag._MESSAGE_ = CommonMessages.NO_DATA_FOUND;
        //                ViewBag._Data_ = null;
        //            }
        //            else
        //            {
        //                ViewBag._STATUS_ = true;
        //                ViewBag._MESSAGE_ = String.Empty;
        //                ViewBag._Data_ = objDataTable2;
        //            }
        //        }
        //        else
        //        {
        //            ViewBag._STATUS_ = true;
        //            ViewBag._MESSAGE_ = CommonMessages.NO_DATA_FOUND;
        //            ViewBag._Data_ = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag._STATUS_ = false;
        //        ViewBag._MESSAGE_ = CommonMessages.ERROR_OCCURED;
        //        ViewBag._Data_ = null;
        //    }
        //    finally
        //    {
        //        objExamCenters = null;
        //    }

        //    ViewBag.States = PortalApplication.States;
        //    ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
        //    ViewBag.ClassName = "col-sm-6";
        //    ViewBag.IsPostback = true;
        //    return View();
        //}
        
        [HttpPost]
        public JsonResult cl()
        {
            JsonResult objJsonResult = null;

            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            try
            {
                objExamCenters = new IIIBL.ExamCenters();
                objDataSet = objExamCenters.ExamCentersForDownload(PortalApplication.ConnectionString);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;

                        //Write to excel...
                        String Filename = PortalSession.UserLoginID + "_CenterList_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                        Filename = Filename.Trim('_');

                        String OutputFileName = String.Format("../Downloads/{0}", Filename);
                        String OutputFileName2 = Server.MapPath(OutputFileName);

                        String[] DisplayColumns = null;
                        String[] DisplayHeaders = null;
                        String[] DisplayFormat = null;

                        /*
                         varExamCenterName
                         varHouseNo
                         varStreet
                         varTown
                         varDistrictName
                         varStateName
                         intPincode
                         btIsActive
                         center_type
                        */

                        DisplayColumns = new String[] { "varExamCenterName", "varHouseNo","varStreet", "varTown", "varDistrictName", "varStateName", "intPincode", "btIsActive", "center_type" };
                        DisplayHeaders = new String[] { "Center Name", "Address line 1", "Address line 2", "Address line 3", "District", "State", "Pincode", "Is active?", "Center Type" };
                        DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

                        XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormat);

                        //objDataTable.DataTable2File(OutputFileName2, "\t");

                        KVPair = new KeyValuePair<String, String>[1];
                        KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                        s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                else
                {
                    Success = false;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Success, Message);
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
                objExamCenters = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public JsonResult DownloadDistricts()
        {
            JsonResult objJsonResult = null;

            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            DataTable objDataTable = null;
            try
            {
                objDataTable = PortalApplication.Districts.Copy();
                if (objDataTable.Rows.Count > 0)
                {
                    Success = true;
                    Message = String.Empty;

                    //Write to excel...
                    String Filename = PortalSession.UserLoginID + "_DistrictList_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                    Filename = Filename.Trim('_');

                    String OutputFileName = String.Format("../Downloads/{0}", Filename);
                    String OutputFileName2 = Server.MapPath(OutputFileName);

                    String[] DisplayColumns = null;
                    String[] DisplayHeaders = null;
                    String[] DisplayFormat = null;

                    /*
                        varExamCenterName
                        varHouseNo
                        varStreet
                        varTown
                        varDistrictName
                        varStateName
                        intPincode
                        btIsActive
                        center_type
                    */

                    DisplayColumns = new String[] {"district_id", "state_name", "district_name", "from_pincode", "to_pincode" };
                    DisplayHeaders = new String[] {"District Id", "State Name", "District Name", "Pincode From", "Pincode till" };
                    DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

                    XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormat);

                    KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                    s = HelperUtilities.ToJSON(true, CommonMessages.REPORT_PROCESSING_SUCCESS_MIN, null, KVPair);
                }
                else
                {
                    Success = false;
                    Message = CommonMessages.NO_DATA_FOUND;
                    s = HelperUtilities.ToJSON(Success, Message);
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
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        //OK
        [HttpPost]
        public JsonResult FindNearest3ExamCenter(Int32 Pincode)
        {
            JsonResult objJsonResult = null;
            IIIBL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objExamCenters = new IIIBL.ExamCenters();
                objDataSet = objExamCenters.FindNearestExamCenter(PortalApplication.ConnectionString, Pincode);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        for (Int32 i = objDataTable.Rows.Count - 1; i > 2; i--)
                        {
                            objDataTable.Rows.RemoveAt(i);
                        }
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
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objExamCenters = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult Dashboard()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        public JsonResult GetDistrictsForStates(Int32 StateId)
        {
            JsonResult objJsonResult = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objDataTable = PortalApplication.Districts;
                if (StateId > 0)
                {
                    objDataTable.DefaultView.RowFilter = String.Format("state_id = {0}", StateId);
                    objDataTable = objDataTable.DefaultView.ToTable();
                }

                if (objDataTable.Rows.Count > 0)
                {
                    Success = true;
                    Message = String.Empty;
                }
                else
                {
                    Success = false;
                    Message = CommonMessages.NO_DATA_FOUND;
                }

                s = HelperUtilities.ToJSON(Success, Message, objDataTable);

            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s= HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public JsonResult GetCentersForDistricts(Int32 DistrictId)
        {
            JsonResult objJsonResult = null;
            IIIBL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            
            try
            {
                objMasterData = new MasterData();
                objDataSet = objMasterData.GetCenterListForDistrict(PortalApplication.ConnectionString, DistrictId);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
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
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objMasterData = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult Districts()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult States()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ExamBodyDetail()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult Insurer()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpGet]
        public ActionResult UnauthorizedAccess()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpGet]
        public ActionResult TbxSchedule()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }


        [HttpGet]
        [AuthorizeExt]
        public ActionResult Notifications()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        [ValidateInput(false)]
        public JsonResult SaveNotification()
        {
            JsonResult objJsonResult = null;
            IIIBL.Notifications obj = null;
            Boolean Status = true;
            String Message = String.Empty;

            Int32 NotificationId = 0;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            String Caption = String.Empty;
            String NotificationText = String.Empty;
            String Displayca = String.Empty;
            String Displaywa = String.Empty;
            String Displayimf = String.Empty;
            String Displaybr = String.Empty;
            String Displayi = String.Empty;
            String DisplayOthers = String.Empty;
            String strFileName = String.Empty;
            String HaltDisplay = String.Empty;
            try
            {
                String _dummy = Convert.ToString(Request.Form["hdnNotificationId"]);
                if (_dummy.Trim() == "0" || _dummy.Trim() == String.Empty)
                {
                    NotificationId = 0;
                }
                else
                {
                    NotificationId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["txtFromDate"]);
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtFromDate");
                }
                else
                {
                    FromDate = Convert.ToDateTime(_dummy);
                }
                _dummy = Convert.ToString(Request.Form["txtToDate"]);
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtToDate");
                }
                else
                {
                    ToDate = Convert.ToDateTime(_dummy);
                }
                Caption = Convert.ToString( Request.Form["txtcaption"]);
                NotificationText = Convert.ToString(Request.Unvalidated.Form["txtnotificationtxt"]);
                Displayca = Convert.ToString(Request.Form["chkRoleCA"]);
                if (Displayca == "on" || Displayca == "true")
                {
                    Displayca = "Y";
                }
                else
                {
                    Displayca = "N";
                }
                Displaywa = Convert.ToString(Request.Form["chkRoleWA"]);
                if (Displaywa == "on" || Displaywa == "true")
                {
                    Displaywa = "Y";
                }
                else
                {
                    Displaywa = "N";
                }

                Displayimf = Convert.ToString(Request.Form["chkRoleIMF"]);
                if (Displayimf == "on" || Displayimf == "true")
                {
                    Displayimf = "Y";
                }
                else
                {
                    Displayimf = "N";
                }
                Displaybr = Convert.ToString(Request.Form["chkRoleBR"]);
                if (Displaybr == "on" || Displaybr == "true")
                {
                    Displaybr = "Y";
                }
                else
                {
                    Displaybr = "N";
                }
                Displayi = Convert.ToString(Request.Form["chkRoleInsurer"]);
                if (Displayi == "on" || Displayi == "true")
                {
                    Displayi = "Y";
                }
                else
                {
                    Displayi = "N";
                }
                DisplayOthers = Convert.ToString(Request.Form["chkRoleOther"]);
                if (DisplayOthers == "on" || DisplayOthers == "true")
                {
                    DisplayOthers = "Y";
                }
                else
                {
                    DisplayOthers = "N";
                }

                HaltDisplay = Convert.ToString(Request.Form["chkHaltDisplay"]);
                if (HaltDisplay == "on" || HaltDisplay == "true")
                {
                    HaltDisplay = "Y";
                }
                else
                {
                    HaltDisplay = "N";
                }

                strFileName = Request.Files[0].FileName;
                if (strFileName != "")
                {
                    string strSaveLocation = Server.MapPath("~/NotificationDocs") + "\\" + strFileName;
                    Request.Files[0].SaveAs(strSaveLocation);
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Status = false;
                Message = CommonMessages.INVALID_INPUT;
            }

            if (Status)
            {
                try
                {
                    obj = new IIIBL.Notifications();
                    int returnvalue = obj.SaveNotification(PortalApplication.ConnectionString, NotificationText, FromDate, ToDate, strFileName, Displayca, Displaywa, Displayimf, Displaybr, Displayi, Caption, DisplayOthers, HaltDisplay, NotificationId);
                    if (returnvalue == 0)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                }
                finally
                {
                    obj = null;
                }
            }
            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ExaminationCenters()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpGet]
        public ActionResult Relogin()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }
        
        [HttpGet]
        public String Ticker()
        {
            String Path = Server.MapPath("~/App_Data/Ticker.txt");
            String s = String.Empty;
            try
            {
                s = System.IO.File.ReadAllText(Path);
            }
            catch(Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = String.Empty;
            }
            finally
            {

            }
            return s;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult SeatsAvailability()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult PaymentModes()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }


    }
}