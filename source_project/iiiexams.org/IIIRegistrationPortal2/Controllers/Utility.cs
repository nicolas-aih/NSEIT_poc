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
using IIIBL;

namespace IIIRegistrationPortal2.Controllers
{
    public class UtilityController : Controller
    {
        [AuthorizeExt]
        public ActionResult UpdateCandidateProfile()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SaveCandidateProfile()
        {
            JsonResult objJsonResult = null;
            IIIBL.Utility objUtility = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            Exception DBException = null;
            try
            {
                String UpdateAction = Convert.ToString(Request.Form["cboUpdateAction"]);
                if (UpdateAction == String.Empty)
                {
                    Message = "Invalid Update Action;";

                }
                String URN = Convert.ToString(Request.Form["txtURN"]);

                if (UpdateAction == String.Empty)
                {
                    Message += "Invalid URN;";

                }

                String UpdateValue = Convert.ToString(Request.Form["txtValue"]);
                String Language = Convert.ToString(Request.Form["cboLanguage"]);
                if (UpdateAction != "Update_Lang" && UpdateValue == String.Empty)
                {
                    Message += "Invalid Update Value;";

                }
                else if (UpdateAction == "Update_Lang" && Language == String.Empty)
                {
                    Message += "Invalid Language Value;";
                }

                if (UpdateAction == "Update_Lang")
                {
                    UpdateValue = Language;
                }


                if (Message == String.Empty)
                {
                    objUtility = new IIIBL.Utility();
                    objUtility.SaveCandidateProfile(PortalApplication.ConnectionString, UpdateValue, URN, PortalSession.UserID, UpdateAction, out Message);

                    if (DBException != null)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, DBException, Request.Form);
                    }

                    if (Message == String.Empty)
                    {
                        Success = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    else
                    {
                        Message = CommonMessages.DATA_SAVE_FAIL + ". " + Message;
                    }
                    s = HelperUtilities.ToJSON(Success, Message, null);
                }
                else
                {
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
                objUtility = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeExt]
        public ActionResult CompanyLookup()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult LoadCompanyDetails()
        {
            JsonResult objJsonResult = null;
            IIIBL.Utility objUtility = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            Exception DBException = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            try
            {
                String CompanyName = Convert.ToString(Request.Form["txtCompanyName"]);
                if (CompanyName == String.Empty)
                {
                    Message = "Invalid Input;";
                }

                if (Message == String.Empty)
                {
                    objUtility = new IIIBL.Utility();
                    objDataSet = objUtility.GetCompanyDetails(PortalApplication.ConnectionString, CompanyName, out Message);
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
                    if (DBException != null)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, DBException, Request.Form);
                    }

                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                }
                else
                {
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
                objUtility = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeExt]
        public ActionResult GetUserPassword()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeAJAX]
        [HttpPost]
        public ActionResult GetUserPassword(String Dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.Utility objUtility = null;
            String s = String.Empty;
            Boolean Success = false;
            String Status = String.Empty;
            String Message = String.Empty;
            String Password = String.Empty;
            Exception DBException = null;
            try
            {
                String UserLoginId = Convert.ToString(Request.Form["txtUserLoginId"]);
                if (UserLoginId == String.Empty)
                {
                    Message = "Invalid Login ID;";
                }

                if (Message == String.Empty)
                {
                    objUtility = new IIIBL.Utility();
                    Message = objUtility.GetUserPassword(PortalApplication.ConnectionString, UserLoginId, out Password);
                    if (DBException != null)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, DBException, Request.Form);
                    }

                    if (Message == String.Empty)
                    {
                        Success = true;
                        Password = LoginEncryptor.Decrypt(Password);
                        KeyValuePair<String, String>[] kVPair = new KeyValuePair<string, string>[1];
                        kVPair[0] = new KeyValuePair<string, string>("Password", Password);
                        s = HelperUtilities.ToJSON(Success, Message, null, kVPair);
                    }
                    else
                    {
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
                }
                else
                {
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
                Password = null;
                objUtility = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
    }
}