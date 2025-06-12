using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;

namespace IIIBL
{
    public class Users
    {
        public DataSet Login(System.String ConnectionString, System.String LoginId, System.String Password, out System.String Result /*, System.Boolean IsLive*/)
        {
            DataSet objDataSet = null;
            IIIDL.Users objUsers = null;
            Result = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.Login(ConnectionString, LoginId, Password, out Result /*, IsLive*/ );
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }

        public void Logout(System.String ConnectionString, System.Int32 UserId)
        {
            IIIDL.Users objUsers = null;
            try
            {
                objUsers = new IIIDL.Users();
                objUsers.Logout(ConnectionString, UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
        }

        public String ChangePassword(System.String ConnectionString, System.String LoginId, System.String Password, System.String NewPassword)
        {
            IIIDL.Users objUsers = null;
            String Result = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Result = objUsers.ChangePassword(ConnectionString, LoginId, Password, NewPassword);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Result;
        }

        public String ResetPassword(System.String ConnectionString, System.String LoginId, System.String EmailId, System.String Password, System.String TransactionPassword)
        {
            IIIDL.Users objUsers = null;
            String Result = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Result = objUsers.ResetPassword(ConnectionString, LoginId, EmailId, Password, TransactionPassword);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Result;
        }

        public DataSet MenuPermissions(String ConnectionString, Int32 UserId /*, Boolean IsLive*/)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.MenuPermissions(ConnectionString, UserId /*, IsLive*/);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }

        //public String SaveLoginRequest(System.String ConnectionString, /*System.String Userid, System.String LoginRequestType,*/ System.String CompanyName, System.String CompanyTypeCode, System.String Address, System.String StdCode, System.String PhoneNumber, System.String MobileNumber, /*System.String Pan,*/ System.String EmailId, System.String ContactPersonName, /*System.String DeedofIncorporation,*/ out System.Int64 RequestId)
        //{
        //    IIIDL.Users objUsers = null;
        //    String Result = String.Empty;
        //    try
        //    {
        //        objUsers = new IIIDL.Users();
        //        Result = objUsers.SaveLoginRequest(ConnectionString, CompanyName, CompanyTypeCode, Address, StdCode, PhoneNumber, MobileNumber, EmailId, ContactPersonName, out RequestId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objUsers = null;
        //    }
        //    return Result;
        //}

        public DataSet GetUserDetails(String ConnectionString, Int32 UserId, String UserType)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.GetUserDetails(ConnectionString, UserId, UserType);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }

        public String SaveUserProfile(System.String ConnectionString, System.String UserType, System.Int32 UserId, System.String Address1, System.String Address2, System.String Address3, System.Int32 Pincode, System.String TelephoneOffice, System.String TelephoneResidence, System.String Fax, System.String POName, System.String EMailId, System.String MobileNo, System.String STDCode, System.String PhoneNo, System.Int32 DistrictId)
        {
            IIIDL.Users objUsers = null;
            String Result = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Result = objUsers.SaveUserProfile(ConnectionString, UserType, UserId, Address1, Address2, Address3, Pincode, TelephoneOffice, TelephoneResidence, Fax, POName, EMailId, MobileNo, STDCode, PhoneNo, DistrictId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Result;

        }

        public Int32 SaveDPDetails(System.String ConnectionString, Int32 DPUserId, System.String DPId, System.Int32 InsurerUserId, System.String Name,
    System.String HouseNo, System.String Street, System.String Town, System.Int32 DistrictId, System.String Pincode,
    System.String TelephoneOffice, System.String TelephoneResidence, System.String Fax, System.String EmailId, System.Int32 CreatedBy
    , System.String Password, System.String TxnPassword, System.String MobileNo, System.Boolean IsActive, System.Boolean ChangePwdOnNextLogin,
    System.Int32 IncorrectLoginAttempts, System.Boolean IsSystemDefined, System.Int32 LastModifiedBy, System.Int32 Flag,
    System.Byte[] DPSignature)
        {
            IIIDL.Users objUsers = null;
            Int32 Success = 0;
            try
            {
                objUsers = new IIIDL.Users();
                Success = objUsers.SaveDPDetails(
                                         ConnectionString, DPUserId, DPId, InsurerUserId, Name,
                         HouseNo, Street, Town, DistrictId, Pincode,
                         TelephoneOffice, TelephoneResidence, Fax, EmailId, CreatedBy, Password, TxnPassword, MobileNo, IsActive, ChangePwdOnNextLogin,
                         IncorrectLoginAttempts, IsSystemDefined, LastModifiedBy, Flag,
                        DPSignature);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Success;
        }

        public Int32 SaveAC(System.String ConnectionString, System.Int32 AgentCounsellorUserId, System.Int32 InsurerUserId, System.Int32 DPUserid, System.Int32 UserId, System.String Name, System.String LoginId, System.String Password, System.String TxnsPassword, System.String HouseNo, System.String Street, System.String Town, System.Int32 DistrictId, System.Int32 Pincode, System.String Teloffice, System.String TelResidence, System.String Fax, System.String EMailId, System.String MobileNo, System.Int32 CreatedBy, System.Int32 Modifiedby, System.Boolean IsSytemDefined, Boolean IsActive, Boolean ChangePwdOnNextLogin, out String Message)
        {
            IIIDL.Users objUsers = null;
            Int32 Result = 0;
            Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Result = objUsers.SaveAC(ConnectionString, AgentCounsellorUserId, InsurerUserId, DPUserid, UserId, Name, LoginId, Password, TxnsPassword, HouseNo, Street, Town, DistrictId, Pincode, Teloffice, TelResidence, Fax, EMailId, MobileNo, CreatedBy, Modifiedby, IsSytemDefined, IsActive, ChangePwdOnNextLogin, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Result;
        }

        public String DeleteAC(System.String ConnectionString, System.Int32 AgentCounsellorUserId, System.Int32 CreatedBy)
        {
            IIIDL.Users objUsers = null;
            String Result = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Result = objUsers.DeleteAC(ConnectionString, AgentCounsellorUserId, CreatedBy);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Result;
        }

        public DataSet GetRoles(String ConnectionString, Int32 RoleId = -1)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.GetRoles(ConnectionString, RoleId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }

        public DataSet GetRolesForUserCreation(System.String ConnectionString)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.GetRolesForUserCreation(ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }

        public String SaveRole(System.String ConnectionString, System.Int32 RoleId, System.String RoleCode, System.String RoleName, System.String Remark, System.Boolean IsActive, System.Boolean IsSystemDefined, System.Int32 UserId)
        {
            IIIDL.Users objUsers = null;
            Int32 Success = 0;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Message = objUsers.SaveRole(ConnectionString, RoleId, RoleCode, RoleName, Remark, IsActive, IsSystemDefined, UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Message;
        }

        public Int32 SaveInsurer(System.String ConnectionString, System.Int32 InsurerUserId, System.Int32 UserId, System.String InsurerName, System.String CDPName, System.String InsurerLoginId, System.String InsurerRegNo, System.String Password, System.String TxnPassword, System.Int32 InsurerType, System.String AddressLine1, System.String AddressLine2, System.String AddressLine3, System.Int32 DistrictId, System.Int32 Pincode, System.String TelNoOffice, System.String TelNoResidence, System.String FaxNo, System.String EmailId, System.Int32 CurrentUserId, Boolean IsActive, Byte[] CDPSignature,out String Message)
        {
            IIIDL.Users objUsers = null;
            Int32 Success = 0;
            Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Success = objUsers.SaveInsurer(ConnectionString, InsurerUserId, UserId, InsurerName, CDPName, InsurerLoginId, InsurerRegNo, Password, TxnPassword, InsurerType, AddressLine1, AddressLine2, AddressLine3, DistrictId, Pincode, TelNoOffice, TelNoResidence, FaxNo, EmailId, CurrentUserId, IsActive, CDPSignature, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Success;
        }

        public String SaveUser(System.String ConnectionString, System.Int32 UserId, System.String UserLoginId, System.String UserName, System.String Password, System.String TxnPassword, System.String MobileNo, System.String EMailId, System.Boolean IsActive, System.Int32 RoleId, System.Int32 CurrentUserId)
        {
            IIIDL.Users objUsers = null;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Message = objUsers.SaveUser(ConnectionString, UserId, UserLoginId, UserName, Password, TxnPassword, MobileNo, EMailId, IsActive, RoleId, CurrentUserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Message;
        }

        public DataSet GetUsers(System.String ConnectionString, Int32 UserId )
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.GetUsers(ConnectionString, UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }
        public DataSet GetAllUsers(System.String ConnectionString)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.GetAllUsers(ConnectionString,0);
    }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }


        public DataSet GetMenuPermissions(System.String ConnectionString,int IntSearchID, bool IsRole)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.GetAllMenuPermissions(ConnectionString,IntSearchID,IsRole);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }
        public DataSet SaveMenuPermissions(System.String ConnectionString, int IntSearchID, bool IsRole,string txtXml)
        {
            IIIDL.Users objUsers = null;
            DataSet objDataSet = null;
            try
            {
                objUsers = new IIIDL.Users();
                objDataSet = objUsers.SaveMenuPermissions(ConnectionString, txtXml,IntSearchID, IsRole);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return objDataSet;
        }

        public String GetCompanyPaymentModes(String ConnectionString, String CompanyType, String CompanyLoginId, out String CompanyName, out Boolean CreditMode, out Boolean OnlineMode)
        {
            IIIDL.Users objUsers = null;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Message = objUsers.GetCompanyPaymentModes(ConnectionString, CompanyType, CompanyLoginId, out CompanyName, out CreditMode, out OnlineMode);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Message;
        }

        public String SaveCompanyPaymentModes(String ConnectionString, String CompanyType, String CompanyLoginId, Boolean CreditMode, Boolean OnlineMode)
        {
            IIIDL.Users objUsers = null;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Users();
                Message = objUsers.SaveCompanyPaymentModes(ConnectionString, CompanyType, CompanyLoginId, CreditMode, OnlineMode);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Message;
        }

    }


}