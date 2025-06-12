using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Web.Services.Description;
using System.IO;
using System.Web.UI;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.NetworkInformation;

namespace IIIRegistrationPortal2.Controllers
{
    public class TelemarketerController : Controller
    {
        [HttpGet]
        [AuthorizeExt]
        public ActionResult RegisterTelemarketer()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult RegisterTelemarketer(String Dummy = "")
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            String TraiRegNo = String.Empty;
            String Name = String.Empty;
            String Address = String.Empty;
            String CpName = String.Empty;
            String CpEmailId = String.Empty;
            String CpContactNo = String.Empty;
            String DpName = String.Empty;
            String DpEmailId = String.Empty;
            String DpContactNo = String.Empty;
            String IsActive = String.Empty;
            Boolean Success = false;

            try
            {
                TraiRegNo = Convert.ToString(Request.Form["txtTraiRegNo"]);
                Name = Convert.ToString(Request.Form["txtName"]);
                Address = Convert.ToString(Request.Form["txtAddress"]);
                CpName = Convert.ToString(Request.Form["txtCPName"]);
                CpEmailId = Convert.ToString(Request.Form["txtCPEmailId"]);
                CpContactNo = Convert.ToString(Request.Form["txtCPContactNo"]);
                DpName = Convert.ToString(Request.Form["txtDPName"]);
                DpEmailId = Convert.ToString(Request.Form["txtDPEmailId"]);
                DpContactNo = Convert.ToString(Request.Form["txtDPContactNo"]);
                IsActive = Convert.ToString(Request.Form["cboIsActive"]);

                if (TraiRegNo.Trim() == String.Empty)
                {
                    throw new Exception("Invalid TraiRegNo");
                }

                if (Name.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Name");
                }
                if (Address.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Address");
                }
                if (CpName.Trim() == String.Empty)
                {
                    throw new Exception("Invalid CpName");
                }
                if (CpEmailId.Trim() == String.Empty)
                {
                    throw new Exception("Invalid CpEmailId");
                }

                if (CpContactNo.Trim() != String.Empty && CpContactNo.Trim().Length != 10)
                {
                    throw new Exception("Invalid CpContactNo");
                }
                if (DpName.Trim() == String.Empty)
                {
                    throw new Exception("Invalid DpName");
                }
                if (DpEmailId.Trim() == String.Empty)
                {
                    throw new Exception("Invalid DpEmailId");
                }

                if (DpContactNo.Trim() != String.Empty && DpContactNo.Trim().Length != 10)
                {
                    throw new Exception("Invalid DpContactNo");
                }
                if (IsActive.Trim() == String.Empty)
                {
                    throw new Exception("Invalid IsActive");
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
                IIIBL.Telemarketer objTelemarketer = null;
                try
                {
                    //form is valid, now saving the form data
                    objTelemarketer = new IIIBL.Telemarketer();
                    Message = objTelemarketer.RegisterTelemarketer(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.UserID, TraiRegNo, Name, Address, CpName, CpEmailId, CpContactNo, DpName, DpEmailId, DpContactNo, IsActive);

                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("Success message", CommonMessages.DATA_SAVE_SUCCESS);
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
                    objTelemarketer = null;
                }
            }

            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ManageTelemarketer()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }


        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult EditTelemarketer(String Dummy = "")
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;
            String TraiRegNo = String.Empty;
            Int64 TmId = 0;
            String Name = String.Empty;
            String Address = String.Empty;
            String CpName = String.Empty;
            String CpEmailId = String.Empty;
            String CpContactNo = String.Empty;
            String DpName = String.Empty;
            String DpEmailId = String.Empty;
            String DpContactNo = String.Empty;
            String IsActive = String.Empty;
            String dummy = String.Empty;
            Boolean Success = false;

            try
            {
                TraiRegNo = Convert.ToString(Request.Form["txtTraiRegNo"]);
                Name = Convert.ToString(Request.Form["txtName"]);
                Address = Convert.ToString(Request.Form["txtAddress"]);
                CpName = Convert.ToString(Request.Form["txtCPName"]);
                CpEmailId = Convert.ToString(Request.Form["txtCPEmailId"]);
                CpContactNo = Convert.ToString(Request.Form["txtCPContactNo"]);
                DpName = Convert.ToString(Request.Form["txtDPName"]);
                DpEmailId = Convert.ToString(Request.Form["txtDPEmailId"]);
                DpContactNo = Convert.ToString(Request.Form["txtDPContactNo"]);
                IsActive = Convert.ToString(Request.Form["cboIsActive"]);

                dummy = Convert.ToString(Request.Form["txt_tmid"]);
                if (dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Tmid");
                }
                else
                {
                    TmId = Convert.ToInt64(dummy);
                }

                if (TraiRegNo.Trim() == String.Empty)
                {
                    throw new Exception("Invalid TraiRegNo");
                }

                if (Name.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Name");
                }
                if (Address.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Address");
                }
                if (CpName.Trim() == String.Empty)
                {
                    throw new Exception("Invalid CpName");
                }
                if (CpEmailId.Trim() == String.Empty)
                {
                    throw new Exception("Invalid CpEmailId");
                }

                if (CpContactNo.Trim() != String.Empty && CpContactNo.Trim().Length != 10)
                {
                    throw new Exception("Invalid CpContactNo");
                }
                if (DpName.Trim() == String.Empty)
                {
                    throw new Exception("Invalid DpName");
                }
                if (DpEmailId.Trim() == String.Empty)
                {
                    throw new Exception("Invalid DpEmailId");
                }

                if (DpContactNo.Trim() != String.Empty && DpContactNo.Trim().Length != 10)
                {
                    throw new Exception("Invalid DpContactNo");
                }
                if (IsActive.Trim() == String.Empty)
                {
                    throw new Exception("Invalid IsActive");
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
                IIIBL.Telemarketer objTelemarketer = null;
                try
                {
                    //form is valid, now saving the form data
                    objTelemarketer = new IIIBL.Telemarketer();
                    Message = objTelemarketer.UpdateTelemarketer(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.UserID, TmId, TraiRegNo, Name, Address, CpName, CpEmailId, CpContactNo, DpName, DpEmailId, DpContactNo, IsActive);

                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("Success message", CommonMessages.DATA_SAVE_SUCCESS);
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
                    objTelemarketer = null;
                }
            }

            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult LoadTelemarketerData()
        {
            //loading telemarketer data for ajax call
            Boolean _Status = true;

            IIIBL.Telemarketer objTelemarketer = null;
            JsonResult objJsonResult = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            Int32 TMId = -1;
            Int32 IsActive = 0;
            try
            {
                String dummy = Convert.ToString(Request.Form["tm_id"]);
                TMId = Convert.ToInt32(dummy);

                dummy = Convert.ToString(Request.Form["is_active"]);
                IsActive = Convert.ToInt32(dummy);
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
                    objTelemarketer = new IIIBL.Telemarketer();
                    objDataSet = objTelemarketer.GetTelemarketerData(PortalApplication.ConnectionString, PortalSession.InsurerUserID, TMId, 0);
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

                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult DeleteTelemarketer(Int64 TmId)
        {
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;
            Boolean Success = false;

            try
            {
                if (TmId == 0)
                {
                    throw new Exception("Invalid Tmid");
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
                IIIBL.Telemarketer objTelemarketer = null;
                try
                {
                    //form is valid, now saving the form data
                    objTelemarketer = new IIIBL.Telemarketer();
                    Message = objTelemarketer.DeleteTelemarkiter(PortalApplication.ConnectionString, PortalSession.InsurerUserID, PortalSession.UserID, TmId);

                    if (Message.Trim() == String.Empty)
                    {
                        Success = true;
                        Message = String.Empty;
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("Success message", CommonMessages.DATA_DELETION_SUCCESS);
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_DELETION_SUCCESS, null, kVPair);
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, CommonMessages.DATA_DELETION_FAIL + " : " + Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {


                }
            }

            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult DownloadData()
        {
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;

            IIIBL.Telemarketer objTelemarketer = null;
            try
            {
                objTelemarketer = new IIIBL.Telemarketer();
                objDataSet = objTelemarketer.GetTelemarketerData(PortalApplication.ConnectionString, PortalSession.InsurerUserID, -1, 0);
                DataTable dtTelemarketer = objDataSet.Tables[0];

                String OutputFileName = String.Format("../Downloads/{0}_TelemarketerData_" , PortalSession.InsurerUserID) + DateTime.Today.ToString("ddMMMyy") + ".xlsx";
                String OutputFileName2 = Server.MapPath(OutputFileName);

                String[] DisplayColumns = new String[] { "tm_name", "tm_trai_reg_no", "tm_address", "tm_contact_person_name", "tm_cp_email_id", "tm_cp_contact_no", "tm_designated_person_name", "tm_dp_email_id", "tm_dp_contact_no", "is_active" };
                String[] DisplayHeaders = new String[] { "Telemarketer's Name", "TRAI Reg. No.", "Address", "Contact Person's Name", "Contact Person's Email Id", "Contact Person's Mobile No.", "Designated Person's Name", "Designated Person's Email Id", "Designated Person's Mobile No.", "Is Active ?" };
                String[] DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

                XLXporter.WriteExcel(OutputFileName2, dtTelemarketer, DisplayColumns, DisplayHeaders, DisplayFormat);

                KeyValuePair<String, String>[] KVPair = null; KVPair = new KeyValuePair<String, String>[1];
                KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);

                s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }

            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }
    }
}