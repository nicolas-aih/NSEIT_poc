using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using System.Reflection;
using IIIBL;
using CSSIntegration;
using System.Net.NetworkInformation;
using System.Web.Services.Description;
//using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;

namespace IIIRegistrationPortal2.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateDOB(DateTime date)
        {
            String s = String.Empty;
            Boolean Success = false;
            if (DateTime.Compare(date.Date, System.DateTime.Now.Date.AddYears(-18)) <= 0)
            {
                Success = true;
            }
            else
            {
                Success = false;
            }
            s = HelperUtilities.ToJSON(Success, String.Empty);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateYOP(DateTime date)
        {
            String s = String.Empty;
            Boolean Success = false;
            if (DateTime.Compare(date.Date, System.DateTime.Now.Date) <= 0)
            {
                Success = true;
            }
            else
            {
                Success = false;
            }
            s = HelperUtilities.ToJSON(Success, String.Empty);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateFromTillDate(DateTime FromDate, DateTime TillDate)
        {
            //  < 0 − If date1 is earlier than date2.
            //  0 − If date1 is the same as date2.
            //  > 0 − If date1 is later than date2.
            String s = String.Empty;
            Boolean Success = false;
            if (DateTime.Compare(FromDate.Date, TillDate) <= 0)
            {
                Success = true;
            }
            else
            {
                Success = false;
            }
            s = HelperUtilities.ToJSON(Success, String.Empty);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateNotificationFromDate(DateTime date)
        {
            String s = String.Empty;
            Boolean Success = false;
            if (DateTime.Compare(date.Date, System.DateTime.Now.Date) >= 0)
            {
                Success = true;
            }
            else
            {
                Success = false;
            }
            s = HelperUtilities.ToJSON(Success, String.Empty);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateYOP2(DateTime dateDOB, DateTime dateYOP)
        {
            String s = String.Empty;
            Boolean Success = false;
            if (DateTime.Compare(dateDOB.Date, dateYOP.Date) < 0)
            {
                Success = true;
            }
            else
            {
                Success = false;
            }
            s = HelperUtilities.ToJSON(Success, String.Empty);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidatePAN(String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidatePAN(PortalApplication.ConnectionString, PAN);
                switch (Message)
                {
                    case "DEBARRED":
                        Status = false;
                        Message = "The PAN entered is debarred";
                        break;
                    case "AVAILABLE":
                        Status = true;
                        Message = String.Empty;
                        break;
                    case "EXCEPTION":
                        Status = false;
                        Message = "Exception occured while validating PAN";
                        break;
                    default:
                        Status = false;
                        Message = "Exception occured while validating PAN";
                        break;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidatePAN2(String PAN, Int64 ApplicantId)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidatePAN(PortalApplication.ConnectionString, PAN, ApplicantId, PortalSession.InsurerUserID);
                Status = (Message == null || Message.Length == 0);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Status = false;
                Message = CommonMessages.ERROR_OCCURED;
            }
            finally
            {
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateInternalRefNo(String InternalRefNo, /*Int32 InsurerUserId,*/ Int32 ApplicantId)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Status = objURN.ValidateInternalRefNo(PortalApplication.ConnectionString, InternalRefNo, PortalSession.InsurerUserID, ApplicantId);
                if (!Status)
                {
                    Message = "The entered number is already in use.";
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateInternalRefNoApp(String InternalRefNo, /*Int32 InsurerUserId,*/ Int32 ApplicantDataId)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Status = objURN.ValidateInternalRefNoApp(PortalApplication.ConnectionString, InternalRefNo, PortalSession.InsurerUserID, ApplicantDataId);
                if (!Status)
                {
                    Message = "The entered number is already in use.";
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateAadhaarCorporates(String AadhaarNo, String PAN, System.String URN = "")
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
                Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());
                objURN = new IIIBL.URN();
                Message = objURN.ValidateAadhaarCorporates(PortalApplication.ConnectionString, IIIBL.AadhaarEncryptorDecryptor.EncryptAadhaar(AadhaarNo, key, IV), PAN, URN);
                if (Message == "AVAILABLE")
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateAadhaarCorporatesApp(String AadhaarNo, String PAN, Int64 ApplicantDataId)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
                Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());
                objURN = new IIIBL.URN();
                Message = objURN.ValidateAadhaarCorporatesApp(PortalApplication.ConnectionString, IIIBL.AadhaarEncryptorDecryptor.EncryptAadhaar(AadhaarNo, key, IV), PAN, ApplicantDataId);
                if (Message == "AVAILABLE")
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateEmailCorporates(System.String EmailId, System.Int64 Applicantid, System.String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateEmailCorporates(PortalApplication.ConnectionString, EmailId, Applicantid, PAN);
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateEmailCorporatesApp(System.String EmailId, System.Int64 ApplicantDataId, System.String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateEmailCorporatesApp(PortalApplication.ConnectionString, EmailId, ApplicantDataId, PAN);
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateMobileCorporates(System.String MobileNo, System.Int64 Applicantid, System.String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateMobileCorporates(PortalApplication.ConnectionString, MobileNo, Applicantid, PAN);
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateWhatsAppCorporates(System.String MobileNo, System.Int64 Applicantid, System.String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateWhatsAppCorporates(PortalApplication.ConnectionString, MobileNo, Applicantid, PAN);
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateMobileCorporatesApp(System.String MobileNo, System.Int64 ApplicantDataId, System.String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateMobileCorporatesApp(PortalApplication.ConnectionString, MobileNo, ApplicantDataId, PAN);
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateWhatsAppCorporatesApp(System.String MobileNo, System.Int64 ApplicantDataId, System.String PAN)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateWhatsAppCorporatesApp(PortalApplication.ConnectionString, MobileNo, ApplicantDataId, PAN);
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //[AuthorizeAJAX]
        [HttpPost]
        public JsonResult ValidateWhatsAppCorporatesForMod(System.String URN, System.String MobileNo)
        {
            IIIBL.URN objURN = null;
            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objURN = new IIIBL.URN();
                Message = objURN.ValidateWhatsAppCorporatesForMod(PortalApplication.ConnectionString, URN, MobileNo );
                if (Message.Trim() == String.Empty)
                {
                    Message = String.Empty;
                    Status = true;
                }
                else
                {
                    Status = false;
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
                objURN = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            JsonResult objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetACforDP(Int32 InsurerID, Int32 DPUserID, Int32 ACUserId)
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
                objMasterData = new IIIBL.MasterData();
                objDataSet = objMasterData.GetACForDPs(PortalApplication.ConnectionString, InsurerID, DPUserID, ACUserId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetAllNotifications(Int32 NotificationId = -1)
        {
            JsonResult objJsonResult = null;
            IIIBL.Notifications objNotifications = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objNotifications = new IIIBL.Notifications();
                objDataSet = objNotifications.GetAllNotifications(PortalApplication.ConnectionString, NotificationId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
                s = HelperUtilities.ToJSON(Success, Message, null);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objNotifications = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public JsonResult GetTbxSchedule()
        {
            JsonResult objJsonResult = null;
            IIIBL.Notifications objNotifications = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objNotifications = new IIIBL.Notifications();
                objDataSet = objNotifications.GetTBXSchedule(PortalApplication.ConnectionString);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
                s = HelperUtilities.ToJSON(Success, Message, null);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objNotifications = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetAllRoles(Int32 RoleId = -1)
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIBL.Users();
                objDataSet = objUsers.GetRoles(PortalApplication.ConnectionString, RoleId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objUsers = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetRolesForUserCreation()
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIBL.Users();
                objDataSet = objUsers.GetRolesForUserCreation(PortalApplication.ConnectionString);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objUsers = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetUsers(Int32 UserId = -1)
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIBL.Users();
                objDataSet = objUsers.GetUsers(PortalApplication.ConnectionString, UserId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objUsers = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        public JsonResult GetAllUsers(Int32 UserId = -1)
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIBL.Users();
                objDataSet = objUsers.GetAllUsers(PortalApplication.ConnectionString);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objUsers = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            objJsonResult.ContentType = "application/json; charset=utf-8";
            objJsonResult.MaxJsonLength = Int32.MaxValue;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetDPForInsurer(Int32 InsurerId, Int32 DPId)
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

                objMasterData = new IIIBL.MasterData();

                if (PortalSession.RoleCode == "I" || PortalSession.RoleCode == "superadmin")
                {
                    if (PortalSession.RoleName == "Designated Person")
                    {
                        objDataSet = objMasterData.GetDPForInsurer(PortalApplication.ConnectionString, InsurerId, PortalSession.DPUserID);
                    }
                    else
                    {
                        objDataSet = objMasterData.GetDPForInsurer(PortalApplication.ConnectionString, InsurerId, DPId);
                    }
                }


                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Columns.Contains("imgDPSignature"))
                    {
                        objDataTable.Columns.Remove("imgDPSignature");
                    }
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

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetDPForInsurerEx(Int32 InsurerId, Int32 DPId)
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
                objMasterData = new IIIBL.MasterData();
                objDataSet = objMasterData.GetDPForInsurer(PortalApplication.ConnectionString, InsurerId, DPId);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        DataRow dr = objDataTable.Rows[0];
                        objDataTable.Columns.Add("imgDPSignatureB64", typeof(String));
                        if (dr["imgDPSignature"] == DBNull.Value)
                        {
                            dr["imgDPSignatureB64"] = DBNull.Value;
                            dr["imgDPSignature"] = DBNull.Value;
                        }
                        else
                        {
                            dr["imgDPSignatureB64"] = Convert.ToBase64String((byte[])dr["imgDPSignature"]);
                            dr["imgDPSignature"] = DBNull.Value;
                        }
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetInsurer()
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
                objMasterData = new IIIBL.MasterData();

                if (PortalSession.RoleCode == "I" || PortalSession.RoleCode == "superadmin")
                {
                    if (PortalSession.RoleName == "Corporate Designated Person" || PortalSession.RoleName == "Designated Person")
                    {
                        objDataSet = objMasterData.GetInsurers(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                    }
                    else
                    {
                        objDataSet = objMasterData.GetInsurers(PortalApplication.ConnectionString, -1);
                    }
                }

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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetInsurer2(Int32 InsurerId = -1)
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
                objMasterData = new IIIBL.MasterData();
                objDataSet = objMasterData.GetInsurers2(PortalApplication.ConnectionString, InsurerId);

                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        if (InsurerId != -1)
                        {
                            DataRow dr = objDataTable.Rows[0];
                            objDataTable.Columns.Add("imgCDPSignatureB64", typeof(String));
                            if (dr["imgCDPSignature"] == DBNull.Value)
                            {
                                dr["imgCDPSignatureB64"] = DBNull.Value;
                                dr["imgCDPSignature"] = DBNull.Value;
                            }
                            else
                            {
                                dr["imgCDPSignatureB64"] = Convert.ToBase64String((byte[])dr["imgCDPSignature"]);
                                dr["imgCDPSignature"] = DBNull.Value;
                            }
                        }
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetBasicQualificationForCOR(String CORType)
        {
            JsonResult objJsonResult = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objDataTable = PortalApplication.BasicQualificationSpecific;
                if (CORType != String.Empty)
                {
                    DataView dv = new DataView(objDataTable);
                    dv.RowFilter = String.Format("cor_type = '{0}'", CORType);
                    objDataTable = dv.ToTable();
                }
                s = HelperUtilities.ToJSON(true, Message, objDataTable);
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

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetProQualificationForCOR()
        {
            Boolean _Status = true;
            JsonResult objJsonResult = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;

            String CORType = String.Empty;
            try
            {
                CORType = Convert.ToString(Request.Form["cboCorType"]);
                if (!CORType.In(new String[] { "IA", "AV", "PO", "SP", "ISP" }))
                {
                    throw new Exception(String.Format("Invalid CORType {0}", CORType));
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
                try
                {
                    objDataTable = PortalApplication.ProQualification;
                    DataView dv = new DataView(objDataTable);
                    dv.RowFilter = String.Format("role_code = '{0}' and cor_type = '{1}'", PortalSession.RoleCode, CORType);
                    objDataTable = dv.ToTable();
                    s = HelperUtilities.ToJSON(true, Message, objDataTable);
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
            }
           
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetDetailsForCOR()
        {
            Boolean _Status = true;
            JsonResult objJsonResult = null;
            System.Data.DataTable objDataTable1 = null; //BAsic Qual
            System.Data.DataTable objDataTable2 = null; //Pro Qual
            System.Data.DataTable objDataTable3 = null; //List of Active TM 
            System.Data.DataTable [] objDataTables = null;

            IIIBL.Telemarketer objTelemarketer = null;
            DataSet objDataSet = null;

            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;

            String CORType = String.Empty;
            try
            {
                CORType = Convert.ToString(Request.Form["cboCorType"]);
                if (!CORType.In(new String[] { "IA", "AV", "PO", "SP", "ISP" }))
                {
                    throw new Exception(String.Format("Invalid CORType {0}", CORType));
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
                DataView dv = null;
                try
                {
                    objDataTable1 = PortalApplication.BasicQualificationSpecific;
                    dv = new DataView(objDataTable1);
                    dv.RowFilter = String.Format("role_code = '{0}' and cor_type = '{1}'", PortalSession.RoleCode, CORType);
                    objDataTable1 = dv.ToTable();
                    dv = null;

                    objDataTable2 = PortalApplication.ProQualification;
                    dv = new DataView(objDataTable2);
                    dv.RowFilter = String.Format("role_code = '{0}' and cor_type = '{1}'", PortalSession.RoleCode, CORType);
                    objDataTable2 = dv.ToTable();


                    objTelemarketer = new Telemarketer();
                    objDataSet = objTelemarketer.GetTelemarketerData(PortalApplication.ConnectionString, PortalSession.InsurerUserID, -1, 1);
                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable3 = objDataSet.Tables[0];
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }

                    objDataTables = new System.Data.DataTable[3];
                    objDataTables[0] = objDataTable1;
                    objDataTables[1] = objDataTable2;
                    objDataTables[2] = objDataTable3;
                    /*
                    if ( objDataTable1.Rows.Count > 0 && objDataTable2.Rows.Count > 0 && objDataTable3.Rows.Count > 0 )
                    {
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                    */
                    s = HelperUtilities.ToJSON2(true, Message, objDataTables);
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
                    objDataTable1 = null;
                    objDataTable2 = null; 
                    objDataTable3 = null;
                    objDataTables = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult FindNearestExamCenter(Int32 Pincode)
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
                objDataSet = objExamCenters.GetExamCenters(PortalApplication.ConnectionString, Pincode);
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
                objExamCenters = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //For Post Login
        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetCentersForState(Int32 StateId, Int32 CenterId)
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
                objDataSet = objExamCenters.ExamCentersForState(PortalApplication.ConnectionString, StateId, CenterId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
        [AuthorizeAJAX]
        public JsonResult GetCentersForStateEx(Int32 StateId, Int32 CenterId)
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
                objDataSet = objExamCenters.ExamCentersForStateEx(PortalApplication.ConnectionString, StateId, CenterId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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
        [AuthorizeAJAX]
        public JsonResult GetSimilarCenters( Int32 CenterId)
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
                objDataSet = objExamCenters.SimilarExamCenters(PortalApplication.ConnectionString, CenterId);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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


        //For Pre Login
        [HttpPost]
        public JsonResult GetCentersForStatePreL(Int32 StateId /*, Int32 CenterId*/)
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
                objDataSet = objExamCenters.ExamCentersForState(PortalApplication.ConnectionString, StateId, -1);
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
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
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


        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SaveExamCenter()
        {
            JsonResult objJsonResult = null;
            String s = String.Empty;
            Boolean Status = true;
            String Message = String.Empty;
            Int32 RetVal = 0;
            IIIBL.ExamCenters objExamCenters = null;

            Int32 CenterId = 0;
            String CenterName = String.Empty;
            String CenterCode = String.Empty;
            Int32 CSSCode = 0;
            String AddressLine1 = String.Empty;
            String AddressLine2 = String.Empty;
            String AddressLine3 = String.Empty;
            Int32 DistrictId = 0;
            Int32 Pincode = 0;
            Boolean IsActive = false;
            String CenterType = String.Empty;
            try
            {
                String _dummy = Convert.ToString(Request.Form["hdnExamCenterId"]);
                if (_dummy.Trim() == "0" || _dummy.Trim() == String.Empty)
                {
                    CenterId = 0;
                }
                else
                {
                    CenterId = Convert.ToInt32(_dummy);
                }

                CenterName = Convert.ToString(Request.Form["txtExamCenterName"]);
                CenterCode = Convert.ToString(Request.Form["txtExamCenterCode"]);
                AddressLine1 = Convert.ToString(Request.Form["txtAddress1"]);
                AddressLine2 = Convert.ToString(Request.Form["txtAddress2"]);
                AddressLine3 = Convert.ToString(Request.Form["txtAddress3"]);

                if (PortalApplication.IntegrationMode == "OAIMS")
                {
                    CSSCode = -1;
                }
                if (PortalApplication.IntegrationMode == "CSS")
                {
                    _dummy = Convert.ToString(Request.Form["txtCSSCode"]);
                    CSSCode = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["cboDistricts"]);
                DistrictId = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["txtPincode"]);
                Pincode = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["chkIsActive"]);
                if (_dummy == "on" || _dummy == "true")
                {
                    IsActive = true;
                }
                else
                {
                    IsActive = false;
                }

                if (Request.Form["cboCenterType"] != null)
                {
                    CenterType = Convert.ToString(Request.Form["cboCenterType"]);
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
                    objExamCenters = new IIIBL.ExamCenters();

                    Message = objExamCenters.SaveCenterDetails(PortalApplication.ConnectionString, CenterId, CenterName, CenterCode, CSSCode, 5, AddressLine1, AddressLine2, AddressLine3, DistrictId, Pincode, IsActive, CenterType, PortalSession.UserID);

                    if (Message.Trim() == String.Empty)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.DATA_SAVE_FAIL + " : " + Message;
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
                    objExamCenters = null;
                }
                s = HelperUtilities.ToJSON(Status, Message);
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult GetMenuData(int intSearchId, int isRole)
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            bool isRoles;
            try
            {
                if (isRole == 1)
                {
                    isRoles = true;
                }
                else
                {
                    isRoles = false;
                }
                objUsers = new IIIBL.Users();
                objDataSet = objUsers.GetMenuPermissions(PortalApplication.ConnectionString, intSearchId, isRoles);
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
                objUsers = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SaveMenuPermission(int intSearchId, int isRole, string[] MenuId, string[] oldvalue, string[] newValue)
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            bool isRoles;
            if (isRole == 1)
            {
                isRoles = true;
            }
            else
            {
                isRoles = false;
            }
            string[] arr1 = MenuId;
            string[] arr2 = oldvalue;
            string[] arr3 = newValue;

            DataTable TblData = new DataTable("TblData");
            TblData.Columns.Add("MenuId");
            TblData.Columns.Add("OldValue");
            TblData.Columns.Add("NewValue");



            for (int i = 1; i < arr1.Length; i++)
            {
                DataRow row = TblData.NewRow();
                row[0] = arr1[i];
                row[1] = arr2[i];
                row[2] = arr3[i];
                TblData.Rows.Add(row);
            }
            int tblMenuCount = TblData.Rows.Count;


            DataTable TblMenu = new DataTable("TblMenu");
            TblMenu.Columns.Add("sntMenuId");
            TblMenu.Columns.Add("varActionName");
            TblMenu.Columns.Add("IsRevoked");
            DataRow dr;

            try
            {
                for (int i = 0; i < TblData.Rows.Count; i++)
                {
                    if (TblData.Rows[i]["OldValue"].ToString() != TblData.Rows[i]["NewValue"].ToString())
                    {
                        dr = TblMenu.NewRow();
                        dr[0] = TblData.Rows[i][0];
                        dr[1] = "MenuAccess";
                        dr[2] = TblData.Rows[i][2];

                        TblMenu.Rows.Add(dr);
                    }
                }

                DataSet ds = new DataSet("dsMenu");
                ds.Tables.Add(TblMenu);
                string txtxml = ds.GetXml();

                objUsers = new IIIBL.Users();
                objDataSet = objUsers.SaveMenuPermissions(PortalApplication.ConnectionString, intSearchId, isRoles, txtxml);

                Success = true;
                Message = "Roles Saved Successfully";

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
                objUsers = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetDPRangeData()
        {
            JsonResult objJsonResult = null;
            IIIBL.DPRangeMst objDPRangeMst = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objDPRangeMst = new IIIBL.DPRangeMst();
                objDataSet = objDPRangeMst.GetDPRangeList(PortalApplication.ConnectionString, 0);
                if (objDataSet != null && objDataSet.Tables.Count > 0)
                {
                    objDataTable = objDataSet.Tables[0];
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
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objDPRangeMst = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveDPRange(System.Int32 Insurercode, Int32 dpCount)
        {
            JsonResult objson = null;
            IIIBL.DPRangeMst objDPRangeMst = null;
            String Message = String.Empty;
            Int32 Intsuccess;
            Int32 Intcreatedby = PortalSession.UserID;
            Int32 Intdprangeid = 0;
            bool Status = false;

            try
            {
                objDPRangeMst = new IIIBL.DPRangeMst();
                Message = objDPRangeMst.SaveDPRange(PortalApplication.ConnectionString, Intdprangeid, Insurercode, dpCount, Intcreatedby);
                if (Message.Trim() == String.Empty)
                {
                    Status = true;
                    Message = CommonMessages.DATA_SAVE_SUCCESS;
                }
                else
                {
                    Status = false;
                    Message = CommonMessages.DATA_SAVE_FAIL + " : " + Message;

                }
            }
            catch (Exception ex)
            {
                Message = CommonMessages.ERROR_OCCURED;
            }
            finally
            {
                objDPRangeMst = null;
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objson = new JsonResult();
            objson.Data = s;
            return objson;
        }

        #region Batch Management
        //Used for Payment...
        [HttpPost]
        public JsonResult GetBatchDetailsForPayment()
        {
            JsonResult objJsonResult = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            DataTable objDataTable2 = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            Decimal GrandTotal = 0;
            String PaymentMode = String.Empty;
            Int32 URNCount = 0;
            Boolean CanProceed = false;
            try
            {
                String TransactionId = Convert.ToString(Request.Form["txtBatchNo"]);
                if (TransactionId == String.Empty)
                {
                    Success = false;
                    Message = "Required Parameter Not Found";
                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                }
                else
                {
                    objBatchMgmt = new BatchMgmt();
                    objDataSet = objBatchMgmt.GetBatchDetailsForPayment_PG(PortalApplication.ConnectionString, TransactionId, out Message, out CanProceed);
                    if (CanProceed)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count == 2)
                        {
                            objDataTable = objDataSet.Tables[0];
                            if (objDataTable.Rows.Count > 0)
                            {
                                objDataTable2 = objDataSet.Tables[1];
                                if (objDataTable2.Rows.Count == 1)
                                {
                                    GrandTotal = Convert.ToDecimal(objDataTable2.Rows[0]["grand_total"]);
                                    PaymentMode = Convert.ToString(objDataTable2.Rows[0]["payment_mode"]);
                                    URNCount = Convert.ToInt32(objDataTable2.Rows[0]["total_urns"]);
                                    KeyValuePair<String, String>[] KVPairs = new KeyValuePair<string, string>[4];
                                    KVPairs[0] = new KeyValuePair<string, string>("transaction_id", TransactionId);
                                    KVPairs[1] = new KeyValuePair<string, string>("grand_total", GrandTotal.ToString());
                                    KVPairs[2] = new KeyValuePair<string, string>("payment_mode", PaymentMode);
                                    KVPairs[3] = new KeyValuePair<string, string>("total_urns", URNCount.ToString());

                                    Success = true;
                                    Message = String.Empty;
                                    s = HelperUtilities.ToJSON(Success, Message, objDataTable, KVPairs);
                                }
                                else
                                {
                                    objDataTable = null;
                                    Success = false;
                                    Message = CommonMessages.NO_DATA_FOUND;
                                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                                }
                            }
                            else
                            {
                                Success = false;
                                Message = CommonMessages.NO_DATA_FOUND;
                                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
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
                objBatchMgmt = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //Used for Batch Management
        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetBatchDetailsForMgmt()
        {
            JsonResult objJsonResult = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            DataTable objDataTable2 = null;
            DataTable objDataTable3 = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            Decimal GrandTotal = 0;
            String PaymentMode = String.Empty;
            Int32 URNCount = 0;
            String StatusId = String.Empty;
            String PaymentStatus = String.Empty;
            String PaymentDate = String.Empty;

            try
            {
                String TransactionId = Convert.ToString(Request.Form["txtBatchNo"]);
                if (TransactionId == String.Empty)
                {
                    Success = false;
                    Message = "Required Parameter Not Found";
                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                }
                else
                {
                    objBatchMgmt = new BatchMgmt();
                    objDataSet = objBatchMgmt.GetBatchDetailsForPayment(PortalApplication.ConnectionString, 2, TransactionId, out Message);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count == 3)
                        {
                            objDataTable = objDataSet.Tables[0];
                            if (objDataTable.Rows.Count > 0)
                            {
                                objDataTable2 = objDataSet.Tables[1];
                                if (objDataTable2.Rows.Count == 1)
                                {
                                    objDataTable3 = objDataSet.Tables[2];

                                    GrandTotal = Convert.ToDecimal(objDataTable2.Rows[0]["grand_total"]);
                                    PaymentMode = Convert.ToString(objDataTable2.Rows[0]["payment_mode"]);
                                    URNCount = Convert.ToInt32(objDataTable2.Rows[0]["total_urns"]);
                                    StatusId = Convert.ToString(objDataTable2.Rows[0]["status_id"]);
                                    PaymentStatus =   Convert.ToString(objDataTable2.Rows[0]["payment_status"]);
                                    PaymentDate = Convert.ToString(objDataTable2.Rows[0]["payment_date"]);

                                    KeyValuePair<String, String>[] KVPairs = new KeyValuePair<string, string>[7];
                                    KVPairs[0] = new KeyValuePair<string, string>("transaction_id", TransactionId);
                                    KVPairs[1] = new KeyValuePair<string, string>("grand_total", GrandTotal.ToString());
                                    KVPairs[2] = new KeyValuePair<string, string>("payment_mode", PaymentMode);
                                    KVPairs[3] = new KeyValuePair<string, string>("total_urns", URNCount.ToString());

                                    KVPairs[4] = new KeyValuePair<string, string>("StatusId", StatusId.ToString());
                                    KVPairs[5] = new KeyValuePair<string, string>("PaymentStatus", PaymentStatus.ToString());
                                    KVPairs[6] = new KeyValuePair<string, string>("PaymentDate", PaymentDate.ToString());

                                    Success = true;
                                    Message = String.Empty;
                                    s = HelperUtilities.ToJSON3(Success, Message, new DataTable[]{objDataTable, objDataTable3 }, KVPairs);
                                }
                                else
                                {
                                    objDataTable = null;
                                    Success = true;
                                    Message = CommonMessages.NO_DATA_FOUND;
                                    s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                                }
                            }
                            else
                            {
                                Success = true;
                                Message = CommonMessages.NO_DATA_FOUND;
                                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                            }
                        }
                        else
                        {
                            Success = true;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                        }
                    }
                    else
                    {
                        Success = false;
                        //Message = Message;
                        s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                    }
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
                objBatchMgmt = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetBatchList()
        {
            JsonResult objJsonResult = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty;
            String TransactionId = String.Empty;
            DateTime dtFromDate = DateTime.Now;
            DateTime dtToDate = DateTime.Now;
            Int32 Hint = 0;
            Int32 Status = -1;
            try
            {
                String _dummy = String.Empty;

                String Search = Convert.ToString(Request.Form["cboSearchBy"]);
                if (Search == "BATCHNO")
                {
                    TransactionId = Convert.ToString(Request.Form["txtBatchNo"]);
                    Hint = 1;
                }
                if (Search == "DATERANGE")
                {
                    Hint = 2;
                    _dummy = Convert.ToString(Request.Form["txtFromDate"]);
                    if (_dummy == String.Empty)
                    {
                        throw new Exception("Invalid txtFromDate");
                    }
                    else
                    {
                        if (DateTime.TryParse(_dummy,out dtFromDate) == false)
                        {
                            throw new Exception("Invalid txtFromDate");
                        }
                    }

                    _dummy = Convert.ToString(Request.Form["txtToDate"]);
                    if (_dummy == String.Empty)
                    {
                        throw new Exception("Invalid txtToDate");
                    }
                    else
                    {
                        if (DateTime.TryParse(_dummy, out dtToDate) == false)
                        {
                            throw new Exception("Invalid txtToDate");
                        }
                    }

                    _dummy = Convert.ToString(Request.Form["cboStatus"]);
                    if (_dummy == String.Empty)
                    {
                        throw new Exception("Invalid cboStatus");
                    }
                    else
                    {
                        if (Int32.TryParse(_dummy, out Status) == false)
                        {
                            throw new Exception("Invalid cboStatus");
                        }
                    }
                }
            }
            catch ( Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objBatchMgmt = new BatchMgmt();
                    objDataSet = objBatchMgmt.GetTransactionList(PortalApplication.ConnectionString, Hint, TransactionId, dtFromDate, dtToDate, Status, PortalSession.UserID);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count == 1)
                        {
                            objDataTable = objDataSet.Tables[0];
                            if (objDataTable.Rows.Count > 0)
                            {
                                Success = true;
                                Message = String.Empty;
                                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                            }
                            else
                            {
                                Success = true;
                                Message = CommonMessages.NO_DATA_FOUND;
                                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                            }
                        }
                        else
                        {
                            Success = true;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                        }
                    }
                    else
                    {
                        Success = false;
                        //Message = Message;
                        s = HelperUtilities.ToJSON(Success, Message, objDataTable);
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
                    objBatchMgmt = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult DeleteBatch()
        {
            JsonResult objJsonResult = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            String TransactionId = String.Empty;
            try
            {
                TransactionId = Convert.ToString(Request.Form["hdBatchNo"]);

                objBatchMgmt = new IIIBL.BatchMgmt();
                Message = objBatchMgmt.DeleteBatches(PortalApplication.ConnectionString, 1, TransactionId, PortalSession.UserID);

                Success = true;
                Message = Message.Trim() == String.Empty ? "Batch Deleted Successfully" : Message;

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
                objBatchMgmt = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult DeleteProbURNs()
        {
            JsonResult objJsonResult = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            String TransactionId = String.Empty;
            try
            {
                TransactionId = Convert.ToString(Request.Form["hdBatchNo"]);

                objBatchMgmt = new IIIBL.BatchMgmt();
                Message = objBatchMgmt.DeleteBatches(PortalApplication.ConnectionString, 2, TransactionId, PortalSession.UserID);

                Success = true;
                Message = Message.Trim() == String.Empty ? "URNs Removed Successfully From The Batch No" : Message;

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
                objBatchMgmt = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;

        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult VerifyBatch()
        {
            JsonResult objJsonResult = null;
            IIIBL.BatchMgmt objBatchMgmt = null;
            String TransactionId = String.Empty;
            String PaymentMode = String.Empty;
            Decimal TotalAmount = 0M;
            String Message = String.Empty;
            String s = String.Empty;
            Boolean Success = false;
            Boolean CanProceed = false;
            try
            {
                TransactionId = Request.Form["hdBatchNo"];
                String PgName = Request.Form["selPG"]; //???


                objBatchMgmt = new BatchMgmt();
                objBatchMgmt.VerifyBatch(PortalApplication.ConnectionString, TransactionId, out PaymentMode, out TotalAmount, out Message, out CanProceed);
                if (PaymentMode == "CR")
                {
                    Success = CanProceed;
                    s = HelperUtilities.ToJSON(Success, Message, null);
                }
                if (PaymentMode == "PG")
                {
                    if (CanProceed)
                    {
                        Session["batch_no_pmt"] = TransactionId;
                        Session["amt_pmt"] = TotalAmount;
                        Session["selPG"] = PgName; //???

                        KeyValuePair<String, String>[] KVPairs = new KeyValuePair<string, string>[1];
                        KVPairs[0] = new KeyValuePair<string, string>("redirect_to", "../Payments/PG2");
                        Success = true;
                        s = HelperUtilities.ToJSON(Success, Message, null, KVPairs);
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
                }
            }
            catch (Exception  ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objBatchMgmt = null;
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        #endregion

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetAvailableSeats()
        {
            JsonResult objJsonResult = null;
            CSSIntegration.Scheduling objScheduling = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            Boolean Status = true;
            Int32 StateId = -1;
            Int32 CenterId = -1;
            DateTime FromDate = DateTime.MaxValue;
            try
            {
                String _dummy = String.Empty;
                if (Request.Form.AllKeys.Contains("cboStates"))
                {
                    _dummy = Convert.ToString(Request.Form["cboStates"]);
                    StateId = Convert.ToInt32(_dummy);
                }
                else
                {
                    StateId = -1;
                }

                if (Request.Form.AllKeys.Contains("cboCenter"))
                {
                    _dummy = Convert.ToString(Request.Form["cboCenter"]);
                    CenterId = Convert.ToInt32(_dummy);
                }
                else
                {
                    CenterId = -1;
                }

                _dummy = Convert.ToString(Request.Form["txtFromDate"]);
                FromDate = Convert.ToDateTime(_dummy);
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Status)
            {
                StringBuilder HTML = new StringBuilder();
                String Data = String.Empty;
                try
                {
                    objScheduling = new CSSIntegration.Scheduling();
                    objDataSet = objScheduling.GetDashBoardOAIMS(PortalApplication.OAIMSConnectionString, StateId, CenterId, FromDate);
                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            if (StateId > 0 && CenterId == -1 )
                            {
                                HTML.AppendLine("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                                String temp = String.Empty;
                                foreach (DataColumn dc in objDataTable.Columns)
                                {
                                    dc.ColumnName = dc.ColumnName.Replace("'", "");
                                    if (dc.ColumnName == "TCM_ID")
                                    {
                                        continue;
                                    }
                                    else if (dc.ColumnName == "CENTER_NAME")
                                    {
                                        temp += String.Format("<th>{0}</th>", "Center Name");
                                    }
                                    else
                                    {
                                        temp += String.Format("<th>{0}</th>", dc.ColumnName);
                                    }
                                }
                                temp += "</tr></thead>";
                                HTML.AppendLine(temp);

                                temp = String.Empty;
                                foreach (DataRow dr in objDataTable.Rows)
                                {
                                    temp += "<tr>";
                                    foreach (DataColumn dc in objDataTable.Columns)
                                    {
                                        if (dc.ColumnName == "TCM_ID")
                                        {
                                            continue;
                                        }
                                        else if (dc.ColumnName == "CENTER_NAME")
                                        {
                                            temp += String.Format("<td>{0}</td>", dr[dc.ColumnName]);
                                        }
                                        else
                                        {
                                            if (dr[dc.ColumnName] == DBNull.Value)
                                            {
                                                dr[dc.ColumnName] = 0;
                                            }
                                            if (Convert.ToInt32(dr[dc.ColumnName]) == 0)
                                            {
                                                temp += String.Format("<td>{0}</td>", dr[dc.ColumnName]);
                                            }
                                            else
                                            {
                                                String func = String.Format("Javascript:d({0},'{1}');", dr["TCM_ID"], dc.ColumnName);
                                                temp += String.Format("<td><a href=\"{1}\">{0}</a></td>", dr[dc.ColumnName], func);
                                            }
                                        }
                                    }
                                    temp += "</tr>";
                                    HTML.AppendLine(temp);
                                    temp = String.Empty;
                                }

                                HTML.AppendLine("</table>");
                            }
                            if (StateId == -1 && CenterId > 0 )
                            {
                                DataRow dr1 = objDataTable.Rows[0];

                                HTML.AppendLine("<table class='table table-striped table-bordered table-hover'><thead>");
                                HTML.AppendLine(String.Format("<tr><th colspan='2' align='center'>{0}</th></tr>", dr1["CENTER_NAME"]));
                                HTML.AppendLine(String.Format("<tr><th colspan='2' align='left'>{0}</th></tr>", Convert.ToDateTime(dr1["SCH_TST_DT"]).ToString("dd-MMM-yyyy")));
                                HTML.AppendLine("<tr><th>Slot</th><th>Available Seats</th></tr></thead>");
                                String temp = String.Empty;

                                foreach (DataRow dr in objDataTable.Rows)
                                {
                                    temp += "<tr>";
                                    temp += String.Format("<td>{0}</td><td>{1}</td>", dr["SCH_SLOT"], dr["SCH_AVL_SEATS"]);
                                    temp += "</tr>";
                                    HTML.AppendLine(temp);
                                    temp = String.Empty;
                                }
                                HTML.AppendLine("</table>");
                            }

                            Data = HTML.ToString();
                            Success = true;
                            Message = String.Empty;
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.NO_DATA_FOUND;
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                    s = HelperUtilities.ToJSON4(Success, Message, Data);
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
                    objScheduling = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetCompanyPaymentModes()
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.Users objUsers = null;

            String CompanyType = String.Empty;
            String LoginId = String.Empty;

            try
            {
                CompanyType = Convert.ToString(Request.Form["cboCompanyType"]);
                if (!CompanyType.In(new string[] { "IA", "CA", "WA", "IMF", "BR" }))
                {
                    throw new Exception("Invalid cboCompanyType value: " + CompanyType);
                }

                LoginId = Convert.ToString(Request.Form["txtCompanyLoginId"]).Trim();
                if (LoginId == String.Empty)
                {
                    throw new Exception("Invalid txtCompanyLoginId: " + LoginId);
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
                String CompanyName = String.Empty;
                Boolean CreditMode = false;
                Boolean OnlineMode = false;
                try
                {
                    objUsers = new Users();
                    Message = objUsers.GetCompanyPaymentModes(PortalApplication.ConnectionString, CompanyType, LoginId, out CompanyName, out CreditMode, out OnlineMode);
                    if (Message.Trim()== String.Empty)
                    {
                        _Status = true;
                        KVPair = new KeyValuePair<string, string>[3];
                        KVPair[0] = new KeyValuePair<string, string>("CNAME", CompanyName);
                        KVPair[1] = new KeyValuePair<string, string>("CMODE", CreditMode ? "Y" : "N");
                        KVPair[2] = new KeyValuePair<string, string>("OMODE", OnlineMode ? "Y" : "N");
                        s = HelperUtilities.ToJSON(_Status, Message, null, KVPair);
                    }
                    else
                    {
                        _Status = false;
                        s = HelperUtilities.ToJSON(_Status, Message);
                    }
                }
                catch(Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objUsers = null;
                    KVPair = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveCompanyPaymentModes()
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            //KeyValuePair<String, String>[] KVPair = null;
            IIIBL.Users objUsers = null;

            String CompanyType = String.Empty;
            String LoginId = String.Empty;
            Boolean CreditMode = false;
            Boolean OnlineMode = false;
            try
            {
                CompanyType = Convert.ToString(Request.Form["hddCompanyType"]);
                if (!CompanyType.In(new string[] { "IA", "CA", "WA", "IMF", "BR" }))
                {
                    throw new Exception("Invalid hddCompanyType value: " + CompanyType);
                }

                LoginId = Convert.ToString(Request.Form["hddCompanyLoginId"]).Trim();
                if (LoginId == String.Empty)
                {
                    throw new Exception("Invalid hddCompanyLoginId: " + LoginId);
                }

                if ((Request.Form["chkCreditBalance"]) == "on" || (Request.Form["chkCreditBalance"]) == "true")
                {
                    CreditMode = true;
                }
                else
                {
                    CreditMode = false;
                }

                if ((Request.Form["chkOnlinePayment"]) == "on" || (Request.Form["chkOnlinePayment"]) == "true")
                {
                    OnlineMode = true;
                }
                else
                {
                    OnlineMode = false;
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
                String CompanyName = String.Empty;
                try
                {
                    objUsers = new Users();
                    Message = objUsers.SaveCompanyPaymentModes(PortalApplication.ConnectionString, CompanyType, LoginId, CreditMode, OnlineMode);
                    if (Message.Trim() == String.Empty)
                    {
                        _Status = true;
                        s = HelperUtilities.ToJSON(_Status, CommonMessages.DATA_SAVE_SUCCESS );
                    }
                    else
                    {
                        _Status = false;
                        s = HelperUtilities.ToJSON(_Status, CommonMessages.DATA_SAVE_FAIL + " : " + Message);
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
                    objUsers = null;
                    //KVPair = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
    }
}