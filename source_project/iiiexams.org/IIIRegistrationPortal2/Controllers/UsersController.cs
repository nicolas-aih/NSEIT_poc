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
using System.Reflection;

namespace IIIRegistrationPortal2.Controllers
{
    public class UsersController : Controller
    {
        //OK
        [HttpGet]
        public ActionResult Logout()
        {
            //Commented as the procedure does nothing. If it starts doing anythinh then this code needs to be invoked.
            /*
            IIIBL.Users objUsers = null;
            try
            {
                objUsers = new Users();
                objUsers.Logout(PortalApplication.ConnectionString, PortalSession.UserID);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                //Only log error... do nothing
            }
            finally
            {
                objUsers = null;
            }
            */

            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
            RedirectResult redirectResult = new RedirectResult("~/Home/Index");
            return redirectResult;
        }

        //OK
        [HttpPost]
        public JsonResult Login(String UserId, String Password)
        {
            JsonResult objJsonResult = null;
            Boolean Status = false;
            String Message = String.Empty;

            if (UserId.Trim() == String.Empty)
            {
                Message += "Please enter user id";
            }
            if (Password.Trim() == String.Empty)
            {
                Message += "Please enter password";
            }

            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                String EncryptedPassword = LoginEncryptor.Encrypt(Password);
                objUsers = new IIIBL.Users();
                objDataSet = objUsers.Login(PortalApplication.ConnectionString, UserId, EncryptedPassword, out Message /*, PortalApplication.IsLive*/);

                switch (Message)
                {
                    case "SUCCESS1":
                    case "SUCCESS":
                        {
                            DataTable objDataTable = objDataSet.Tables[0];
                            DataRow dr = objDataTable.Rows[0];
                            string strClientMachineIP1 = PortalSession.ClientMachineIP;
                            Session.Clear();
                            PortalSession.ClientMachineIP = strClientMachineIP1;
                            PortalSession.UserID = Convert.ToInt32(dr["intUserID"]);
                            PortalSession.UserName = Convert.ToString(dr["varUserName"]);
                            PortalSession.RoleID = Convert.ToInt16(dr["sntRoleID"]);
                            PortalSession.RoleName = Convert.ToString(dr["varRoleName"]);
                            PortalSession.InsurerUserID = Convert.ToInt32(dr["intTblMstInsurerUserID"]);
                            PortalSession.DPUserID = Convert.ToInt32(dr["intTblMstDPUserID"]);
                            PortalSession.AgentCounselorUserID = Convert.ToInt32(dr["intTblMstAgntCounselorUserID"]);
                            PortalSession.LastLoggedInDateTime = Convert.ToDateTime(dr["dtLastLoggedIn"]);
                            PortalSession.InsurerName = Convert.ToString(dr["varInsurerName"]);
                            PortalSession.DPName = Convert.ToString(dr["varDPName"]);
                            PortalSession.AgentCounselorName = Convert.ToString(dr["varCounselorName"]);
                            PortalSession.UserLoginID = UserId;
                            PortalSession.InsurerType = Convert.ToString(dr["InsurerType"]);
                            PortalSession.UserEmailID = Convert.ToString(dr["varEmailID"]);
                            //PortalSession.PortabilityDPUserID = dr[""];
                            //PortalSession.PortabilityDPName = dr[""];
                            PortalSession.InsurerCode = Convert.ToString(dr["varInsurerID"]);
                            PortalSession.InsurerTypeNew = Convert.ToString(dr["InsurerTypeNew"]);
                            PortalSession.CAid = Convert.ToChar(dr["CAid"]);
                            PortalSession.RoleCode = Convert.ToString(dr["role_code"]);
                            PortalSession.InsurerTypeNew = Convert.ToString(dr["InsurerTypeNew"]);

                            PortalSession.TopUserLoginID = Convert.ToString(dr["TopLevelUserLoginId"]);

                            PortalSession.Key = AadhaarEncryptorDecryptor.GenerateKey(EncryptionAlgorithm.TripleDES);
                            PortalSession.IV = AadhaarEncryptorDecryptor.GenerateIV(EncryptionAlgorithm.TripleDES);

                            Status = true;
                            Message = "Success";
                        }
                        break;
                    case "INVALID_ID":
                        Message = "User id or Password is invalid."; // "Login ID is invalid.";
                        break;
                    case "INVALID_PWD":
                        Message = "User id or Password is invalid."; // "Password is invalid.";
                        break;
                    case "INACTIVE":
                        Message = "User is not active.";
                        break;
                    case "EXCEPTION":
                        Message = "Could not login.";
                        break;
                    case "LOGGEDIN":
                        Message = "User is already logged in.";
                        break;
                    case "ROLE_NOT_MAPPED":
                        Message = "Role not assigned to user.";
                        break;
                    case "LOGIN_ATTEMPT_EXCEEDED":
                        Message = "User has been suspended.";
                        break;
                    case "SUSPENDED":
                        Message = "User has been suspended.";
                        break;
                    case "CHANGE_PWD":
                        Message = "Change password before login.";
                        break;
                    case "NO_DATA":
                        Message = "Temporarily Suspended: Agency Data not Submitted";
                        break;
                    default:
                        Message = "Unknown error.";
                        break;
                }

                if (Status)
                {
                    String CoookieString = PortalSession.UserID + "|" + PortalSession.UserName;
                    System.Web.Security.FormsAuthentication.SetAuthCookie(CoookieString, false);

                    //Fetch Menu 

                    try
                    {
                        objUsers = new IIIBL.Users();
                        objDataSet = objUsers.MenuPermissions(PortalApplication.ConnectionString, PortalSession.UserID /*, PortalApplication.IsLive*/);
                        if (objDataSet != null && objDataSet.Tables.Count != 0)
                        {
                            DataTable dataTable = objDataSet.Tables[0].Copy();
                            dataTable.DefaultView.RowFilter = "MenuAccess = 1";
                            dataTable = dataTable.DefaultView.ToTable();
                            PortalSession.MenuAccess = dataTable;
                            PortalSession.ParentMenu = dataTable.DefaultView.ToTable(true, new[] { "ParentMenuName" });
                            PortalSession.ChildMenu = dataTable;
                        }
                        else
                        {
                            PortalSession.ParentMenu = new DataTable();
                            PortalSession.ChildMenu = new DataTable();
                            // Log the issue
                            //Status = false;
                            //Message = "Unable to fetch menu details";
                        }
                    }
                    catch (Exception ex)
                    {
                        PortalSession.ParentMenu = new DataTable();
                        PortalSession.ChildMenu = new DataTable();
                        //  Log the issue
                        //Status = false;
                        //Message = "Unable to fetch menu details";
                    }
                    finally
                    {
                        objDataSet = null;
                        objUsers = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                //Navigate to error page.
                Status = false;
                Message = CommonMessages.ERROR_OCCURED;
            }
            finally
            {
                objUsers = null;
                objDataSet = null;
            }
            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //OK
        [HttpPost]
        public JsonResult ChangePassword(String UserId, String Password, String NewPassword, String ConfirmPassword)
        {
            JsonResult objJsonResult = null;
            Boolean Status = false;
            String Message = String.Empty;
            IIIBL.Users objUsers = null;
            try
            {
                //Validate
                if (UserId.Trim() == String.Empty)
                {
                    Message += "Please enter user id";
                }
                if (Password.Trim() == String.Empty)
                {
                    Message += "Please enter old password";
                }
                if (NewPassword.Trim() == String.Empty)
                {
                    Message += "Please enter new password";
                }
                if (NewPassword.Trim() != ConfirmPassword)
                {
                    Message += "Password and confirm password do not match";
                }

                //Change
                if (Message == String.Empty)
                {
                    String EncryptedPassword = LoginEncryptor.Encrypt(Password);
                    String EncryptedNewPassword = LoginEncryptor.Encrypt(NewPassword);
                    objUsers = new IIIBL.Users();
                    Message = objUsers.ChangePassword(PortalApplication.ConnectionString, UserId, EncryptedPassword, EncryptedNewPassword);

                    if (Message.Trim() == String.Empty)
                    {
                        Status = true;
                        Message = "Password changed successfully";
                    }
                    else
                    {
                        Status = false;
                    }
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
                objUsers = null;
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //OK
        [HttpPost]
        public JsonResult ResetPassword(String UserId, String EmailId)
        {
            JsonResult objJsonResult = null;
            Boolean Status = false;
            String Message = String.Empty;
            IIIBL.Users objUsers = null;
            try
            {
                //Validate
                if (UserId.Trim() == String.Empty)
                {
                    Message += "Please enter user id";
                }
                if (EmailId.Trim() == String.Empty)
                {
                    Message += "Please enter email id associated with this user";
                }

                //Change
                if (Message == String.Empty)
                {
                    String[] Password = Guid.NewGuid().ToString().Split('-');

                    String EncryptedPassword = LoginEncryptor.Encrypt(Password[0]);
                    String EncryptedTxnPassword = LoginEncryptor.Encrypt(Password[3]);
                    objUsers = new IIIBL.Users();
                    Message = objUsers.ResetPassword(PortalApplication.ConnectionString, UserId, EmailId, EncryptedPassword, EncryptedTxnPassword);
                    if (Message.Trim() == String.Empty)
                    {
                        String To = EmailId;
                        String Subject = "Password Resolved";

                        string strEmailBody;
                        strEmailBody = "Dear User,<br><br>Your password has been reset as below:<br/><br/>";
                        strEmailBody += "Login ID : " + UserId;
                        strEmailBody += "<br/><br/>";
                        strEmailBody += "Login Password : " + Password[0];
                        strEmailBody += "<br/><br/>";
                        //strEmailBody += "Transaction Password : " + Password[3];
                        //strEmailBody += "<br/>";
                        //strEmailBody += "<br/>";
                        strEmailBody += "Kindly change password on next login.";
                        strEmailBody += "<br/><br/><br/>";
                        strEmailBody += "with regards";
                        strEmailBody += "<br/><br/>";
                        strEmailBody += "IIIExams.org";

                        try
                        {
                            HelperUtilities.SendMail(Subject, EmailId, String.Empty, String.Empty, strEmailBody, true);
                            Status = true;
                            Message = "Password has been reset succesfully and e-mail sent to the registered e-mail id";
                        }
                        catch (Exception ex)
                        {
                            Status = true;
                            Message = "Password has been reset succesfully, but error occured while sending e-mail to the registered e-mail id. Kindly contact support team.";
                        }
                    }
                    else
                    {
                        Status = false;
                    }
                }
                else
                {
                    Status = false;
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                //Navigate to error page.
                Status = false;
                Message = CommonMessages.ERROR_OCCURED;
            }
            finally
            {
                objUsers = null;
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        //OK
        [HttpGet]
        [AuthorizeExt]
        public ActionResult MyProfile()
        {
            String UserType = String.Empty;
            String ViewName = String.Empty;
            switch (PortalSession.RoleName)
            {
                case "Corporate Agent User":
                    ViewName = "MyProfile";
                    UserType = "CA";
                    break;
                case "Web Aggregator":
                    ViewName = "MyProfile";
                    UserType = "WA";
                    break;
                case "Insurance Marketing Firm":
                    ViewName = "MyProfile";
                    UserType = "IMF";
                    break;
                case "Insurance Broker":
                    ViewName = "MyProfile";
                    UserType = "BR";
                    break;
                case "Corporate Designated Person":
                    ViewName = "MyProfile2";
                    UserType = "CDP";
                    break;
                case "Designated Person":
                    ViewName = "MyProfile2";
                    UserType = "DP";
                    break;
                case "Agent Counselor":
                    ViewName = "MyProfile2";
                    UserType = "AC";
                    break;
                default:
                    UserType = String.Empty;
                    break;
            }
            ViewBag.UserType = UserType;
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View(ViewName);
        }


        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetProfile(String Dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String UserType = String.Empty;

            Boolean Status = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                switch (PortalSession.RoleName)
                {
                    case "Corporate Agent User":
                        UserType = "CA";
                        break;
                    case "Web Aggregator":
                        UserType = "WA";
                        break;
                    case "Insurance Marketing Firm":
                        UserType = "IMF";
                        break;
                    case "Insurance Broker":
                        UserType = "BR";
                        break;
                    case "Corporate Designated Person":
                        UserType = "CDP";
                        break;
                    case "Designated Person":
                        UserType = "DP";
                        break;
                    case "Agent Counselor":
                        UserType = "AC";
                        break;
                    default:
                        UserType = String.Empty;
                        break;
                }

                objUsers = new IIIBL.Users();
                objDataSet = objUsers.GetUserDetails(PortalApplication.ConnectionString, PortalSession.UserID, UserType);
                if (objDataSet != null && objDataSet.Tables.Count != 0)
                {
                    objDataTable = objDataSet.Tables[0];
                    Status = true;
                    Message = String.Empty;
                    s = HelperUtilities.ToJSON(Status, Message, objDataTable);
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
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Status = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Status, Message);
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
        public JsonResult SaveProfile(String Address1, String POName, String EMailId, String MobileNo, String STDCode, String PhoneNo)
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            String UserType = String.Empty;
            String Message = String.Empty;
            Boolean Status = false;
            String s = String.Empty;
            try
            {
                switch (PortalSession.RoleName)
                {
                    case "Corporate Agent User":
                        UserType = "CA";
                        break;
                    case "Web Aggregator":
                        UserType = "WA";
                        break;
                    case "Insurance Marketing Firm":
                        UserType = "IMF";
                        break;
                    case "Insurance Broker":
                        UserType = "BR";
                        break;
                    case "Corporate Designated Person":
                        UserType = "CDP";
                        break;
                    case "Designated Person":
                        UserType = "DP";
                        break;
                    case "Agent Counselor":
                        UserType = "AC";
                        break;
                    default:
                        UserType = String.Empty;
                        break;
                }

                objUsers = new IIIBL.Users();
                Message = objUsers.SaveUserProfile(PortalApplication.ConnectionString, UserType, PortalSession.UserID, Address1, String.Empty, String.Empty, -1, String.Empty, String.Empty, String.Empty, POName, EMailId, MobileNo, STDCode, PhoneNo, -1);
                Status = (Message.Trim() == String.Empty);
                if (Message.Trim() == String.Empty)
                {
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
                objUsers = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult UpdateProfile2(String Address1, String Address2, String Address3, Int32 Pincode,
            String TelephoneOffice, String TelephoneResidence, String Fax,
            String EMailId, Int32 DistrictId, String Mobile
            )
        {
            JsonResult objJsonResult = null;
            IIIBL.Users objUsers = null;
            String UserType = String.Empty;
            String Message = String.Empty;
            Boolean Status = false;
            String s = String.Empty;
            try
            {
                switch (PortalSession.RoleName)
                {
                    case "Corporate Agent User":
                        UserType = "CA";
                        break;
                    case "Web Aggregator":
                        UserType = "WA";
                        break;
                    case "Insurance Marketing Firm":
                        UserType = "IMF";
                        break;
                    case "Insurance Broker":
                        UserType = "BR";
                        break;
                    case "Corporate Designated Person":
                        UserType = "CDP";
                        break;
                    case "Designated Person":
                        UserType = "DP";
                        break;
                    case "Agent Counselor":
                        UserType = "AC";
                        break;
                    default:
                        UserType = String.Empty;
                        break;
                }

                objUsers = new IIIBL.Users();
                Message = objUsers.SaveUserProfile(PortalApplication.ConnectionString, UserType, PortalSession.UserID, Address1, Address2, Address3, Pincode, TelephoneOffice, TelephoneResidence, Fax, String.Empty, EMailId, Mobile, String.Empty, String.Empty, DistrictId);
                Status = (Message.Trim() == String.Empty);
                if (Message.Trim() == String.Empty)
                {
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
                objUsers = null;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        //To be looked into
        [AuthorizeExt]
        [HttpGet]
        public ActionResult ViewDP()
        {
            IIIBL.MasterData objMasterData = null;
            DataSet objCDP = null;
            String ViewName = String.Empty;
            try
            {
                objMasterData = new MasterData();
                if (PortalSession.RoleCode == "I" || PortalSession.RoleCode == "superadmin")
                {
                    if (PortalSession.RoleName == "Corporate Designated Person" || PortalSession.RoleName == "Designated Person")
                    {
                        objCDP = objMasterData.GetInsurers(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                    }
                    else
                    {
                        objCDP = objMasterData.GetInsurers(PortalApplication.ConnectionString, -1);
                    }

                    ViewBag.CDP = objCDP.Tables[0];
                }
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
            return View();
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SaveDP()
        {
            JsonResult objJsonResult = null;

            Int32 InsurerUserId = Convert.ToInt32(Request.Form["hdnInsurerUserId"]);
            Int32 DPUserId = Convert.ToInt32(Request.Form["hdnDPUserId"]);
            String Name = Convert.ToString(Request.Form["txtName"]);
            String Address = Convert.ToString(Request.Form["txtAddress"]);
            String Street = Convert.ToString(Request.Form["txtStreet"]);
            String Town = Convert.ToString(Request.Form["txtTown"]);
            String Pincode = Convert.ToString(Request.Form["txtPincode"]);
            String DPId = Convert.ToString(Request.Form["txtDPId"]);
            String TelephoneO = Convert.ToString(Request.Form["txtTelephoneO"]);
            String TelephoneR = Convert.ToString(Request.Form["txtTelephoneR"]);
            String Mobile = Convert.ToString(Request.Form["txtMobileNo"]);
            String Fax = Convert.ToString(Request.Form["txtFax"]);
            String EMailId = Convert.ToString(Request.Form["txtEmailID"]);

            //= Convert.ToString(Request.Form["ImgSign"]);
            Byte[] Sign = null;
            if ((Int32)Request.Files["txtFileSign"].ContentLength != 0)
            {
                MemoryStream ms = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                Request.Files["txtFileSign"].InputStream.CopyTo(ms);
                Sign = ms.ToArray();
                ms = null;
            }

            //String StateId = Convert.ToInt32(Request.Form["cboStates"]);
            Int32 DistrictId = Convert.ToInt32(Request.Form["cboDistricts"]);
            Boolean IsActive = Convert.ToString(Request.Form["chkActive"]) == "on" || Convert.ToString(Request.Form["chkActive"]) == "true";

            //
            Boolean isAddition = DPUserId == 0;
            Int32 CreatedBy = -1;
            Boolean ChangePasswordOnNextLogin = false;
            Int32 IncorrectLoginAttempts = 0;
            String Password     = Guid.NewGuid().ToString().Split('-')[0];
            String TxnPassword  = Guid.NewGuid().ToString().Split('-')[0];
            String EncPassword = String.Empty; // Guid.NewGuid().ToString().Split('-')[0];
            String EncTxnPassword = String.Empty; //Guid.NewGuid().ToString().Split('-')[0];
            Int32 LastModifiedBy = -1;

            if (isAddition)
            {
                CreatedBy = PortalSession.UserID;
                ChangePasswordOnNextLogin = true;
                IncorrectLoginAttempts = 0;
                EncPassword = LoginEncryptor.Encrypt(Password);
                EncTxnPassword = LoginEncryptor.Encrypt(TxnPassword);
                LastModifiedBy = -1;
            }
            else
            {
                CreatedBy = -1;
                ChangePasswordOnNextLogin = false;
                IncorrectLoginAttempts = 0;
                EncPassword = LoginEncryptor.Encrypt(Password);
                EncTxnPassword = LoginEncryptor.Encrypt(TxnPassword);
                LastModifiedBy = PortalSession.UserID;
            }

            Boolean Status = false;
            String Message = String.Empty;
            IIIBL.Users objUsers = null;
            Boolean SaveSuccessful = false;
            Boolean MailSuccessful = false;
            try
            {

                objUsers = new Users();
                Int32 iSuccess = objUsers.SaveDPDetails(PortalApplication.ConnectionString, DPUserId, DPId, InsurerUserId, Name, Address, Street, Town, DistrictId, Pincode, TelephoneO, TelephoneR, Fax, EMailId,CreatedBy ,EncPassword , EncTxnPassword, Mobile, IsActive, ChangePasswordOnNextLogin , IncorrectLoginAttempts , false, LastModifiedBy , 0, Sign);
                if (iSuccess == -100 || iSuccess == -300)
                {
                    Status = false;
                    Message = CommonMessages.DATA_SAVE_FAIL + " : EmailID already Exists.";
                }
                if (iSuccess == 100)
                {
                    SaveSuccessful = true;
                    //SendingMail(varPwd, varTrnxPwd, varClientName, varClientID, varEmailID);
                    String Subject = "System Mail - Login Information";
                    String To = EMailId;
                    String CC = String.Empty;
                    String Bcc = String.Empty;
                    String Body = String.Format("<B>Dear {0},</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> {1}<br><b>Password:</b> {2}<br><br>Thanks and regards", Name, DPId, Password);
                    Boolean IsHtml = true;
 
                    HelperUtilities.SendMail(Subject, To, CC, Bcc, Body, IsHtml);
                    MailSuccessful = true;
                    Status = true;
                    Message = CommonMessages.DATA_SAVE_SUCCESS  + ". Password has been sent to designated person on his Email id";
                }
                if (iSuccess == 200)
                {
                    SaveSuccessful = true;
                    Status = true;
                    Message = CommonMessages.DATA_SAVE_SUCCESS;
                }
                if (iSuccess == -200)
                {
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                if (DPUserId== 0 && SaveSuccessful && !MailSuccessful)
                {
                    Status = true;
                    Message = CommonMessages.DATA_SAVE_SUCCESS + ". However error occured while sending mail to designated person";
                }
                else
                {
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                }
            }
            finally
            {
                objUsers = null;
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public ActionResult GenerateNewDPId(Int32 InsurerUserId)
        {
            JsonResult objJsonResult = new JsonResult();
            IIIBL.MasterData objMasterData = null;
            Int32 DPUserId = -1;
            Boolean Status = false;
            String Message = String.Empty;    
            String s = String.Empty;
            try
            {
                objMasterData = new IIIBL.MasterData();
                DPUserId = objMasterData.GetNewDPId(PortalApplication.ConnectionString, InsurerUserId); 
                if (DPUserId == -1)
                {
                    Status = false;
                    Message = "Unable to register new Designated Person as DP ID range is exhausted.\nKindly contact technical support team";
                }
                else
                {
                    Status = true;
                    Message = String.Empty;
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
                objMasterData = null;
            }

            KeyValuePair<String, String>[] KVPair = new KeyValuePair<String, String>[1];
            KVPair[0] = new KeyValuePair<string, string>("DPUserID", Convert.ToString(DPUserId));
            s = HelperUtilities.ToJSON(Status, Message, null, KVPair);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeExt]
        [HttpGet]
        public ActionResult NewAC()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeExt]
        [HttpGet]
        public ActionResult ViewAC()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeAJAX]
        [HttpPost]
        public ActionResult SaveAC()
        {
            JsonResult objJsonResult = null;
            Boolean Status = true;
            String Message = String.Empty;
            Int32 RetVal = 0;
            IIIBL.Users objUser = null;

            Int32 AgentCounsellorUserId = 0;
            Int32 DPUserid = 0;
            Int32 InsurerUserId = 0;
            Int32 UserId = 0;
            String Name = String.Empty;
            String LoginId = String.Empty;
            String Password;
            String TxnsPassword;
            String HouseNo = String.Empty;
            String Street = String.Empty;
            String Town = String.Empty;
            Int32  DistrictId = 0;
            Int32 Pincode = 0;
            String Teloffice = String.Empty;
            String TelResidence = String.Empty;
            String Fax = String.Empty;
            String EMailId = String.Empty;
            String MobileNo = String.Empty;
            Boolean IsSytemDefined = false;
            Boolean IsActive = false;
            Boolean ChangePwdOnNextLogin = false;
            Int32 CreatedBy = 0;
            Int32 ModifiedBy = 0;
            Boolean IsNewAddition = false;
            try
            {
                String _dummy = Request.Form["hdnAgentCounsellorUserId"];
                if (_dummy.Trim() == "0")
                {
                    AgentCounsellorUserId = 0;
                }
                else
                {
                    AgentCounsellorUserId = Convert.ToInt32(_dummy);
                }

                _dummy = Request.Form["hdnDPUserId"];
                DPUserid = Convert.ToInt32(_dummy); //hdnDPUserId
                _dummy = Request.Form["hdnInsurerUserId"];
                InsurerUserId = Convert.ToInt32(_dummy); //hdnInsurerUserId
                _dummy = Request.Form["hdnACUserId"];
                UserId = Convert.ToInt32(_dummy); //hdnACUserId

                Name = Convert.ToString(Request.Form["txtName"]);
                if (AgentCounsellorUserId == 0) // Add Mode
                {
                    LoginId = Convert.ToString(Request.Form["txtLoginId"]);
                    IsNewAddition = true;
                }
                else
                {
                    LoginId = Convert.ToString(Request.Form["hdnLoginId"]);
                    IsNewAddition = false;
                }
                //Password;
                //TxnsPassword;
                HouseNo = Convert.ToString(Request.Form["txtAddress"]);
                Street = Convert.ToString(Request.Form["txtStreet"]);
                Town = Convert.ToString(Request.Form["txtTown"]);

                _dummy = Convert.ToString(Request.Form["cboDistricts"]);
                DistrictId = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["txtPincode"]);
                Pincode = Convert.ToInt32(_dummy);
                Teloffice = Convert.ToString(Request.Form["txtTelephoneO"]);
                TelResidence = Convert.ToString(Request.Form["txtTelephoneR"]);
                Fax = Convert.ToString(Request.Form["txtFax"]);
                EMailId = Convert.ToString(Request.Form["txtEmailID"]);
                MobileNo = Convert.ToString(Request.Form["txtMobileNo"]);
                IsSytemDefined = false;
                _dummy = Convert.ToString(Request.Form["chkActive"]);
                IsActive = (_dummy == "on" || _dummy == "true");
                _dummy = Convert.ToString(Request.Form["chkChangePwd"]);
                ChangePwdOnNextLogin = (_dummy == "on" || _dummy == "true");
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Status = false;
                Message = CommonMessages.INVALID_INPUT;
            }
            
            if (Status)
            {
                Boolean SaveSuccessful = false;
                Boolean MailSuccessful = false;
                Message = String.Empty;
                try
                {
                    Password = Guid.NewGuid().ToString().Split('-')[0];
                    TxnsPassword = Guid.NewGuid().ToString().Split('-')[0];
                    String EncPassword = LoginEncryptor.Encrypt(Password);
                    String EncTxnsPassword = LoginEncryptor.Encrypt(TxnsPassword);

                    CreatedBy = AgentCounsellorUserId == 0 ? PortalSession.UserID : -1; 
                    ModifiedBy = AgentCounsellorUserId == 0 ? -1 : PortalSession.UserID;

                    objUser = new Users();
                    RetVal = objUser.SaveAC(PortalApplication.ConnectionString, AgentCounsellorUserId, InsurerUserId, DPUserid, UserId, Name, LoginId, EncPassword, EncTxnsPassword, HouseNo, Street, Town, DistrictId, Pincode, Teloffice, TelResidence, Fax, EMailId, MobileNo, CreatedBy, ModifiedBy, IsSytemDefined, IsActive, ChangePwdOnNextLogin, out Message);

                    if (RetVal == 0)
                    {
                        Status = false;
                        Message = CommonMessages.DATA_SAVE_FAIL + " : " + Message;
                    }
                    else if (RetVal == 1)
                    {
                        if (IsNewAddition)
                        {
                            SaveSuccessful = true;
                            //SendingMail(varPwd, varTrnxPwd, varClientName, varClientID, varEmailID);
                            String Subject = "System Mail - Login Information";
                            String To = EMailId;
                            String CC = String.Empty;
                            String Bcc = String.Empty;
                            String Body = String.Format("<B>Dear {0},</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> {1}<br><b>Password:</b> {2}<br><br>Thanks and regards", Name, LoginId, Password);
                            Boolean IsHtml = true;

                            HelperUtilities.SendMail(Subject, To, CC, Bcc, Body, IsHtml);
                            MailSuccessful = true;
                            Status = true;
                            Message = CommonMessages.DATA_SAVE_SUCCESS + " : Mail sent to " + Name + " at " + "(" + EMailId + ")";
                        }
                        else
                        {
                            Status = true;
                            Message = CommonMessages.DATA_SAVE_SUCCESS;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    if (InsurerUserId == 0 && SaveSuccessful && !MailSuccessful)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS + ". However error occured while sending mail to agenc counselor";
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.ERROR_OCCURED;
                    }
                }
                finally
                {
                    objUser = null;
                }
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public ActionResult DeleteAC()
        {
            JsonResult objJsonResult = null;
            Boolean Status = true;
            String Message = String.Empty;
            IIIBL.Users objUser = null;

            String _AgentCounsellorUserId = Request.Form["AgentCounsellorUserId"];
            Int32 AgentCounsellorUserId = 0;
            if (!Int32.TryParse(_AgentCounsellorUserId,out AgentCounsellorUserId))
            {
                //Error
                Status = false;
                Message = CommonMessages.INVALID_INPUT;
            }
            if (Status)
            {
                try
                {
                    objUser = new Users();
                    Message = objUser.DeleteAC(PortalApplication.ConnectionString, AgentCounsellorUserId, PortalSession.UserID);
                    if (Message == String.Empty)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_DELETION_SUCCESS;
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.DATA_DELETION_FAIL + " : " + Message;
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
                    objUser = null;
                }
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult Roles()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult SaveRole()
        {
            JsonResult objJsonResult = null;
            IIIBL.Users obj = null;
            Boolean Status = true;
            String Message = String.Empty;

            Int32 RoleId = 0;
            String RoleCode = String.Empty;
            String RoleName = String.Empty;
            String Remark = String.Empty;
            Boolean IsActive = false;

            try
            {
                String _dummy = Convert.ToString(Request.Form["hdnRoleId"]);
                if (_dummy.Trim() == "0")
                {
                    RoleId = 0;
                }
                else
                {
                    RoleId = Convert.ToInt32(_dummy);
                }

                RoleCode = Convert.ToString(Request.Form["txtRoleCode"]);
                RoleName = Convert.ToString(Request.Form["txtRoleName"]);
                Remark = Convert.ToString(Request.Form["txtRoleDescription"]);

                _dummy = Convert.ToString(Request.Form["chkIsActive"]);
                if (_dummy == "on" || _dummy == "true")
                {
                    IsActive = true;
                }
                else
                {
                    IsActive = false;
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Status = false;
                Message = CommonMessages.INVALID_INPUT;
            }

            if (Status)
            {
                Message = String.Empty;
                try
                {
                    obj = new IIIBL.Users();
                    Message = obj.SaveRole(PortalApplication.ConnectionString, RoleId, RoleCode, RoleName, Remark, IsActive, false, PortalSession.UserID );
                    if (Message == String.Empty )
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
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
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

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SaveInsurer()
        {
            JsonResult objJsonResult = null;
            Boolean Status = true;
            String Message = String.Empty;
            Int32 RetVal = 0;
            IIIBL.Users objUsers = null;

            Int32 InsurerUserId = 0;
            Int32 UserId = 0;
            String InsurerName = String.Empty; 
            String CDPName = String.Empty;
            String InsurerLoginId = String.Empty;
            String InsurerRegNo = String.Empty;
            String EncPassword = String.Empty;
            String EncTxnPassword = String.Empty;
            Int32 InsurerType = 0;
            String AddressLine1 = String.Empty;
            String AddressLine2 = String.Empty;
            String AddressLine3 = String.Empty;
            Int32 DistrictId = 0;
            Int32 Pincode = 0;
            String TelephoneO = String.Empty;
            String TelephoneR = String.Empty;
            String Fax = String.Empty;
            String EMailId = String.Empty;
            byte[] Sign = null;
            String Password = Guid.NewGuid().ToString().Split('-')[0];
            String TxnPassword = Guid.NewGuid().ToString().Split('-')[0];

            try
            {
                String _dummy = Convert.ToString(Request.Form["hdnInttblmstinsureruserid"]);
                if (_dummy.Trim() == "0" || _dummy.Trim() == String.Empty)
                {
                    InsurerUserId = 0;
                }
                else
                {
                    InsurerUserId = Convert.ToInt32(_dummy);
                }
                
                _dummy = Convert.ToString(Request.Form["hdnIntUserID"]);
                if (_dummy.Trim() == "0" || _dummy.Trim() == String.Empty)
                {
                    UserId = 0;
                }
                else
                {
                    UserId = Convert.ToInt32(_dummy);
                }

                InsurerLoginId = Convert.ToString(Request.Form["txtInsurerCode"]);
                InsurerRegNo = Convert.ToString(Request.Form["txtInsurerRegistrationNumber"]);
                InsurerName = Convert.ToString(Request.Form["txtName"]);
                CDPName = Convert.ToString(Request.Form["txtCDPName"]);

                _dummy = Convert.ToString(Request.Form["cboInsurerType"]);
                InsurerType = Convert.ToInt32(_dummy);

                AddressLine1 = Convert.ToString(Request.Form["txtAddress1"]);
                AddressLine2 = Convert.ToString(Request.Form["txtAddress2"]);
                AddressLine3 = Convert.ToString(Request.Form["txtAddress3"]);
                
                _dummy = Convert.ToString(Request.Form["cboDistricts"]);
                DistrictId = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["txtPincode"]);
                Pincode = Convert.ToInt32(_dummy);

                TelephoneO = Convert.ToString(Request.Form["txtTelephoneO"]);
                TelephoneR = Convert.ToString(Request.Form["txtTelephoneR"]);
                Fax = Convert.ToString(Request.Form["txtFax"]);
                EMailId = Convert.ToString(Request.Form["txtEmailId"]);

                EncPassword = LoginEncryptor.Encrypt(Password); // Guid.NewGuid().ToString().Split('-')[0];
                EncTxnPassword = LoginEncryptor.Encrypt(TxnPassword); //Guid.NewGuid().ToString().Split('-')[0];

            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Status = false;
                Message = CommonMessages.INVALID_INPUT;
            }

            if (Status)
            {
                Message = String.Empty;
                //= Convert.ToString(Request.Form["ImgSign"]);
                if ((Int32)Request.Files["txtFileSign"].ContentLength != 0)
                {
                    MemoryStream ms = new MemoryStream((Int32)Request.Files["txtFileSign"].InputStream.Length);
                    Request.Files["txtFileSign"].InputStream.CopyTo(ms);
                    Sign = ms.ToArray();
                    ms = null;
                }

                Boolean SaveSuccessful = false;
                Boolean MailSuccessful = false;
                try
                {

                    objUsers = new Users();
                    Int32 Success = objUsers.SaveInsurer(PortalApplication.ConnectionString, InsurerUserId, UserId, InsurerName, CDPName, InsurerLoginId, InsurerRegNo, EncPassword, EncTxnPassword, InsurerType, AddressLine1, AddressLine2, AddressLine3, DistrictId, Pincode, TelephoneO, TelephoneR, Fax, EMailId, PortalSession.UserID, true, Sign, out Message);

                    //-200 / -400 -- User Login Id is already in use
                    //-600 / -800 -- The registration no is already in use
                    //-700 / -900 -- Email id is already in use
                    // 200 / 400  -- Saved Successful
                    if (Success == 0)
                    {
                        Status = false;
                        Message = CommonMessages.DATA_SAVE_FAIL + " : " + Message;
                    }
                    if (Success == 1)//New Addition
                    {
                        SaveSuccessful = true;
                        //SendingMail(varPwd, varTrnxPwd, varClientName, varClientID, varEmailID);
                        String Subject = "System Mail - Login Information";
                        String To = EMailId;
                        String CC = String.Empty;
                        String Bcc = String.Empty;
                        String Body = String.Format("<B>Dear {0},</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> {1}<br><b>Password:</b> {2}<br><br>Thanks and regards", CDPName, InsurerLoginId, Password);
                        Boolean IsHtml = true;

                        HelperUtilities.SendMail(Subject, To, CC, Bcc, Body, IsHtml);
                        MailSuccessful = true;
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS + ". Password has been sent to corporate designated person on his Email id";
                    }
                    if (Success == 2)//Modification
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    if (InsurerUserId == 0 && SaveSuccessful && !MailSuccessful)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS + ". However error occured while sending mail to corporate designated person";
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.ERROR_OCCURED;
                    }
                }
                finally
                {
                    objUsers = null;
                }
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult Users()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult SaveUser()
        {
            JsonResult objJsonResult = null;
            Boolean Status = true;
            String Message = String.Empty;
            IIIBL.Users objUsers = null;

            Int32 UserId = 0;

            String UserLoginId = String.Empty;
            String UserName = String.Empty;
            String MobileNo = String.Empty;
            String EMailId = String.Empty;
            Boolean IsActive = false;
            Int32 RoleId = 0;

            String EncPassword = String.Empty;
            String EncTxnPassword = String.Empty;
            String Password = Guid.NewGuid().ToString().Split('-')[0];
            String TxnPassword = Guid.NewGuid().ToString().Split('-')[0];

            Boolean IsAddition = false;
            try
            {
                String _dummy = Convert.ToString(Request.Form["hdnUserId"]);
                if (_dummy.Trim() == "0" || _dummy.Trim() == String.Empty)
                {
                    UserId = 0;
                    IsAddition = true;
                }
                else
                {
                    UserId = Convert.ToInt32(_dummy);
                    IsAddition = false;
                }
                UserLoginId = Convert.ToString(Request.Form["txtLoginId"]);
                UserName = Convert.ToString(Request.Form["txtUserName"]);
                MobileNo = Convert.ToString(Request.Form["txtMobileNo"]);
                EMailId = Convert.ToString(Request.Form["txtEmailID"]);

                _dummy = Convert.ToString(Request.Form["cboRoles"]);
                RoleId = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["chkIsActive"]);
                if (_dummy == "on" || _dummy == "true")
                {
                    IsActive = true;
                }
                else
                {
                    IsActive = false;
                }

                EncPassword = LoginEncryptor.Encrypt(Password); // Guid.NewGuid().ToString().Split('-')[0];
                EncTxnPassword = LoginEncryptor.Encrypt(TxnPassword); //Guid.NewGuid().ToString().Split('-')[0];

            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Status = false;
                Message = CommonMessages.INVALID_INPUT;
            }

            if (Status)
            {
                Boolean SaveSuccessful = false;
                Boolean MailSuccessful = false;
                try
                {
                    objUsers = new Users();
                    Message = objUsers.SaveUser(PortalApplication.ConnectionString, UserId, UserLoginId, UserName, EncPassword, EncTxnPassword, MobileNo, EMailId, IsActive, RoleId, PortalSession.UserID);
                    if (Message == String.Empty)
                    {
                        SaveSuccessful = true;
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.DATA_SAVE_FAIL + " : " + Message;
                    }
                    
                    if (Status && IsAddition)
                    {
                        SaveSuccessful = true;
                        String Subject = "System Mail - Login Information";
                        String To = EMailId;
                        String CC = String.Empty;
                        String Bcc = String.Empty;
                        String Body = String.Format("<B>Dear {0},</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> {1}<br><b>Password:</b> {2}<br><br>Thanks and regards", UserName, UserLoginId, Password);
                        Boolean IsHtml = true;

                        HelperUtilities.SendMail(Subject, To, CC, Bcc, Body, IsHtml);
                        MailSuccessful = true;
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS + ". Password has been sent to the user on his Email id";
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    if (UserId == 0 && SaveSuccessful && !MailSuccessful)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS + ". However error occured while sending mail to the User";
                    }
                    else
                    {
                        Status = false;
                        Message = CommonMessages.ERROR_OCCURED;
                    }
                }
                finally
                {
                    objUsers = null;
                }
            }

            String s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        public ActionResult RolePermission()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }
    }
}