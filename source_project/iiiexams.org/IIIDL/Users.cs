using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;

namespace IIIDL
{
    public class Users
    {
        public DataSet Login(System.String ConnectionString, System.String LoginId, System.String Password, out System.String Result /*, Boolean IsLive*/)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = String.Empty;
                //if (IsLive)
                //{
                    ProcedureName = "STP_CMN_ValidateLogin_ForExamPortal_New";
                //}
                //else
                //{
                //    ProcedureName = "STP_CMN_ValidateLogin_ForExamPortal_New_UAT";
                //}
                
                String[] Params = new String[] { "@varLoginID", "@varPassword", "@varResult" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(128, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(50, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { LoginId, Password, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                Result = Convert.ToString(AllParameters[2]);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public void Logout(System.String ConnectionString, System.Int32 UserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_CMN_InValidateLogin";
                String[] Params = new String[] { "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { UserId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            //return objDataSet;
        }

        public String ChangePassword(System.String ConnectionString, System.String LoginId, System.String Password, System.String NewPassword)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Result = String.Empty;
            try
            {
                System.String ProcedureName = "STP_CMN_ChangePassword_New";
                String[] Params = new String[] { "@varLoginID", "@varPassword", "@varNewPassword", "@varResult" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(128, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input,  ParameterDirection.Output };
                Object[] Values = new Object[] { LoginId, Password, NewPassword, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Result = Convert.ToString(AllParameters[3]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return Result;
        }

        public String ResetPassword(System.String ConnectionString, System.String LoginId, System.String EmailId, System.String Password, System.String TransactionPassword)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Result = String.Empty;
            try
            {
                System.String ProcedureName = "STP_CMN_ResolvePassword_New";
                String[] Params = new String[] { "@varLoginID", "@varEmailID", "@varPassword", "@varTrxnPassword", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(128, 0, 0), new ParamLength(256, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(50, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { LoginId, EmailId, Password, TransactionPassword, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Result = Convert.ToString(AllParameters[4]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return Result;
        }

        public DataSet MenuPermissions(System.String ConnectionString, System.Int64 UserId /*, Boolean IsLive*/)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_GetMenuPermission_New2"; 
                String[] Params = new String[] { "@IntSearchID", "@IsRole" /*,"@IsLive"*/ };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.Bit /*, SqlDbType.VarChar*/ };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 19, 0), new ParamLength(1, 1, 0) /*, new ParamLength(1, 0, 0)*/ };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input/*, ParameterDirection.Input*/ };
                Object[] Values = new Object[] { UserId, 0 /*, IsLive ? "Y" : "N"*/ };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet,true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        //public String SaveLoginRequest(System.String ConnectionString, /*System.String Userid, System.String LoginRequestType,*/ System.String CompanyName, System.String CompanyTypeCode, System.String Address, System.String StdCode, System.String PhoneNumber, System.String MobileNumber, /*System.String Pan,*/ System.String EmailId, System.String ContactPersonName, /*System.String DeedofIncorporation,*/ out System.Int64 RequestId)
        //{
        //    Databases.SQLServer.Database objDatabase = null;
        //    //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
        //    Int32 ProcReturnValue = 0;
        //    Object[] AllParameters = null;
        //    String Result = String.Empty;
        //    try
        //    {
        //        System.String ProcedureName = "STP_SaveLoginDetails_CA";
        //        String[] Params = new String[] { "@UserID", "@LoginRequestType", "@CompanyName", "@CompanyTypeCode", "@Address", "@STDCode", "@PhoneNumber", "@MobileNumber", "@Pan", "@Emailid", "@ContactPersonName", "@DeedOfIncorporation", "@ERROR", "@RequestId" };
        //        SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.BigInt };
        //        ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(25, 0, 0), new ParamLength(50, 0, 0), new ParamLength(255, 0, 0), new ParamLength(10, 0, 0), new ParamLength(512, 0, 0), new ParamLength(5, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(10, 0, 0), new ParamLength(50, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(8, 19, 0) };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
        //        Object[] Values = new Object[] { DBNull.Value, DBNull.Value, CompanyName, CompanyTypeCode, Address, StdCode, PhoneNumber, MobileNumber, DBNull.Value, EmailId, ContactPersonName, DBNull.Value, DBNull.Value, DBNull.Value };
        //        objDatabase = new Database();
        //        //Comment the below line if the procedure does not return data set.
        //        //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
        //        //Comment the below line if the procedure return data set.
        //        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
        //        Result = (AllParameters[12] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[12]);
        //        RequestId = Convert.ToInt64(AllParameters[13]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
        //    //Comment the below line if the procedure does not return data set.
        //    return Result;
        //}

        public DataSet GetUserDetails(String ConnectionString, Int32 UserId, String UserType)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = String.Empty;
                switch (UserType)
                {
                    case "CA":
                    case "WA":
                    case "IMF":
                    case "BR":
                        ProcedureName = "stp_ADM_GetUserProfile_New";
                        break;
                    case "CDP":
                    case "DP":
                    case "AC":
                        ProcedureName = "sp_get_profile_new";
                        break;
                    default:
                        ProcedureName = "stp_ADM_GetUserProfile_New";
                        break;
                }


                String[] Params = new String[] { "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { UserId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public String SaveUserProfile(System.String ConnectionString, System.String UserType, System.Int32 UserId, System.String Address1, System.String Address2, System.String Address3, System.Int32 Pincode, System.String TelephoneOffice, System.String TelephoneResidence, System.String Fax, System.String POName, System.String EMailId, System.String MobileNo, System.String STDCode, System.String PhoneNo, System.Int32 DistrictId)
        {
            String Result;
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_EditProfile";
                String[] Params = new String[] { "@UserType", "@UserId", "@varHouseNo", "@varStreet", "@varTown", "@intPINCode", "@varTelOffice", "@varTelResidence", "@varFax", "@varPOName", "@varEmailID", "@varMobileNo", "@varSTDCode", "@varPhoneNo", "@sntDistrictID", "@Error" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.SmallInt, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0), new ParamLength(4, 10, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(4, 10, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(14, 0, 0), new ParamLength(4, 0, 0), new ParamLength(10, 0, 0), new ParamLength(2, 5, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { UserType, UserId, Address1, Address2, Address3, Pincode == -1 ? (Object)DBNull.Value : Pincode, TelephoneOffice, TelephoneResidence, Fax, POName, EMailId, MobileNo, STDCode, PhoneNo, DistrictId == -1 ? (Object)DBNull.Value : DistrictId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Result = Convert.ToString(AllParameters[15]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return Result;
        }

        public Int32 SaveDPDetails(System.String ConnectionString, Int32 DPUserId, System.String DPId, System.Int32 InsurerUserId, System.String Name,
            System.String HouseNo, System.String Street, System.String Town, System.Int32 DistrictId, System.String Pincode,
            System.String TelephoneOffice, System.String TelephoneResidence, System.String Fax, System.String EmailId, System.Int32 CreatedBy
            , System.String Password, System.String TxnPassword, System.String MobileNo, System.Boolean IsActive, System.Boolean ChangePwdOnNextLogin,
            System.Int32 IncorrectLoginAttempts, System.Boolean IsSystemDefined, System.Int32 LastModifiedBy, System.Int32 Flag,
            System.Byte[] DPSignature)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 Success = 0;

            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_ADM_SaveDPDetails";
                String[] Params = new String[] { "@intTblMstDPUserID", "@varDPID", "@sntInsurerID", "@varName", "@varHouseNo", "@varStreet", "@varTown", "@sntDistrictID", "@intPINCode", "@varTelOffice", "@varTelResidence", "@varFax", "@varEMailID", "@intCreatedBy", "@intLastModifiedBy", "@varPassword", "@varTrxnPassword", "@MobileNo", "@bitIsActive", "@bitChgPwdOnNxtLogin", "@sntIncorrectLoginAttempts", "@bitIsSystemDefined", "@bitFlag", "@imgDPSignature", "@IntSucess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.SmallInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.SmallInt, SqlDbType.Char, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.SmallInt, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.VarBinary, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(5, 0, 0), new ParamLength(2, 5, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(2, 5, 0), new ParamLength(6, 0, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(256, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(1024, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(20, 0, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(2, 5, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(2097152, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { DPUserId, DPId, InsurerUserId, Name, HouseNo, Street, Town, DistrictId, Pincode, TelephoneOffice, TelephoneResidence, Fax, EmailId, CreatedBy, LastModifiedBy, Password, TxnPassword, MobileNo, IsActive, ChangePwdOnNextLogin, IncorrectLoginAttempts, IsSystemDefined, Flag, DPSignature, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                // = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.

                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Success = Convert.ToInt32(AllParameters[24]);
            }

            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            return Success;
            //Comment the below line if the procedure does not return data set.
        }


        public Int32 SaveAC(System.String ConnectionString, System.Int32 AgentCounsellorUserId, System.Int32 InsurerUserId, System.Int32 DPUserid, System.Int32 UserId, System.String Name, System.String LoginId, System.String Password, System.String TxnsPassword, System.String HouseNo, System.String Street, System.String Town, System.Int32 DistrictId, System.Int32 Pincode, System.String Teloffice, System.String TelResidence, System.String Fax, System.String EMailId, System.String MobileNo, System.Int32 CreatedBy, System.Int32 Modifiedby, System.Boolean IsSytemDefined, Boolean IsActive, Boolean ChangePwdOnNextLogin,out String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Int32 RetVal = 0;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "stp_ADM_MaintainAgentCounselorUser_New";
                String[] Params = new String[] { "@intTblMstAgntCounselorUserID", "@intTblMstDPUserID", "@intTblMstInsurerUserID", "@intUserID", "@varName", "@varCounselorLoginID", "@varPassword", "@varTrnxPassword", "@varHouseNo", "@varStreet", "@varTown", "@sntDistrictID", "@intPINCode", "@varTelOffice", "@varTelResidence", "@varFax", "@varEmailID", "@varMobileNo", "@intCreatedBy", "@intModifiedBy", "@bitIsSytemDefined", "@bitIsActive", "@bitChgPwdOnNxtLogin", "@IntSucess","@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.SmallInt, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(256, 0, 0), new ParamLength(5, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(2, 5, 0), new ParamLength(4, 10, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(256, 0, 0), new ParamLength(14, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { AgentCounsellorUserId, DPUserid, InsurerUserId, UserId, Name, LoginId, Password, TxnsPassword, HouseNo, Street, Town, DistrictId, Pincode, Teloffice, TelResidence, Fax, EMailId, MobileNo, CreatedBy == -1 ? (Object)DBNull.Value : CreatedBy, Modifiedby == -1 ? (Object)DBNull.Value : Modifiedby, IsSytemDefined, IsActive, ChangePwdOnNextLogin, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RetVal = Convert.ToInt32(AllParameters[23]);
                Message = Convert.ToString(AllParameters[24]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return RetVal;
        }

        public String DeleteAC(System.String ConnectionString, System.Int32 AgentCounsellorUserId, System.Int32 CreatedBy)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String RetVal = String.Empty;
            try
            {
                System.String ProcedureName = "stp_DeleteAgentCounselorUser_New";
                String[] Params = new String[] { "@intTblMstAgntCounselorUserID", "@intCreatedBy", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { AgentCounsellorUserId, CreatedBy, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                RetVal = Convert.ToString(AllParameters[2]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return RetVal;
        }

        public DataSet GetRoles(System.String ConnectionString, Int32 RoleId = -1)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "stp_GetRoles_New";
                String[] Params = new String[] { "@RoleID", "@Hint" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { RoleId <= 0 ? (Object)DBNull.Value : RoleId , 1};
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public DataSet GetRolesForUserCreation(System.String ConnectionString)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "stp_GetRoles_New";
                String[] Params = new String[] { "@RoleID", "@Hint" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { (Object)DBNull.Value , 2 };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public String SaveRole(System.String ConnectionString, System.Int32 RoleId, System.String RoleCode, System.String RoleName, System.String Remark, System.Boolean IsActive, System.Boolean IsSystemDefined, System.Int32 UserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            String Message = String.Empty;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_ADM_SaveRoles_New";
                String[] Params = new String[] { "@RoleID", "@RoleCode", "@RoleName", "@Remark", "@IsActive", "@IsSystemDefined", "@UserId", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(10, 0, 0), new ParamLength(256, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { RoleId <=0 ? (Object)DBNull.Value: RoleId, RoleCode, RoleName, Remark, IsActive, IsSystemDefined, UserId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[7]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return Message;
        }

        public Int32 SaveInsurer(System.String ConnectionString, System.Int32 InsurerUserId, System.Int32 UserId, System.String InsurerName, System.String CDPName, System.String InsurerLoginId, System.String InsurerRegNo, System.String Password, System.String TxnPassword, System.Int32 InsurerType, System.String AddressLine1, System.String AddressLine2, System.String AddressLine3, System.Int32 DistrictId, System.Int32 Pincode, System.String TelNoOffice, System.String TelNoResidence, System.String FaxNo, System.String EmailId, System.Int32 CurrentUserId, Boolean IsActive, Byte[] CDPSignature, out String Message )
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Int32 RetVal = 0;
            Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "stp_ADM_InsurerMaster_New";
                String[] Params = new String[] { "@IntTblMstInsurerUserID", "@intUserID", "@varName", "@varCDPName", "@varInsurerID", "@varInsurerRegNo", "@varPassword", "@varTrnxPassword", "@tntInsurerType", "@varHouseNo", "@varStreet", "@varTown", "@sntDistrictID", "@intPINCode", "@varTelOffice", "@varTelResidence", "@varFax", "@varEmailID", "@intCurrentUserID", "@bitIsActive", "@imgCDPSignature", "@IntSucess", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.TinyInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.SmallInt, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Bit, SqlDbType.VarBinary , SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(5, 0, 0), new ParamLength(3, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1, 3, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(2, 5, 0), new ParamLength(4, 10, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(14, 0, 0), new ParamLength(256, 0, 0), new ParamLength(4, 10, 0), new ParamLength(1, 1, 0), new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { InsurerUserId, UserId, InsurerName, CDPName, InsurerLoginId, InsurerRegNo, Password, TxnPassword, InsurerType, AddressLine1, AddressLine2, AddressLine3, DistrictId, Pincode, TelNoOffice, TelNoResidence, FaxNo, EmailId, CurrentUserId, IsActive, CDPSignature, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RetVal = Convert.ToInt32(AllParameters[21]);
                Message = Convert.ToString(AllParameters[22]);
            }
	        catch (Exception ex)
	        {
		        throw(ex);
	        }
	        finally
	        {
		        objDatabase = null;
	        }
            //Comment the below line if the procedure does not return data set.
	        return RetVal;
        }

        public String SaveUser(System.String ConnectionString, System.Int32 UserId, System.String UserLoginId, System.String UserName, System.String Password, System.String TxnPassword, System.String MobileNo, System.String EMailId, System.Boolean IsActive, System.Int32 RoleId, System.Int32 CurrentUserId )
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "SP_SAVE_User";
                String[] Params = new String[] { "@intUserID", "@varLoginID", "@varName", "@varPassword", "@varTrxnPassword", "@varMobileNo", "@varEMailID", "@BitIsActive", "@sntRoleID", "@intCurrentUser", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit, SqlDbType.SmallInt, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(20, 0, 0), new ParamLength(256, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(20, 0, 0), new ParamLength(256, 0, 0), new ParamLength(1, 1, 0), new ParamLength(2, 5, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { UserId, UserLoginId, UserName, Password, TxnPassword, MobileNo, EMailId, IsActive, RoleId, CurrentUserId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[10]);
            }
	        catch (Exception ex)
	        {
		        throw(ex);
	        }
	        finally
	        {
		        objDatabase = null;
	        }
            //Comment the below line if the procedure does not return data set.
	        return Message;
        }

        public DataSet GetUsers(System.String ConnectionString, Int32 UserId = -1)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_users";
                String[] Params = new String[] { "@intUserId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { UserId <= 0 ? (Object)DBNull.Value : UserId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public DataSet GetAllUsers(System.String ConnectionString, int Intuserid)
        {
            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "stp_ADM_GetUserDetails";
                String[] Params = new String[] { "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { Intuserid };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;

        }

        public DataSet GetAllMenuPermissions(System.String ConnectionString, System.Int64 Intsearchid, bool Isrole)
        {
            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_GetMenuPermission_New";
                String[] Params = new String[] { "@IntSearchID", "@IsRole" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.Bit };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 19, 0), new ParamLength(1, 1, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Intsearchid, Isrole };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public DataSet SaveMenuPermissions(System.String ConnectionString,string Txtmenuxml , System.Int64 Intsearchid,bool Isrole)
        {

            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_ADM_SaveMenuPermission";
                String[] Params = new String[] { "@txtMenuXML", "@IntSearchID", "@IsRole" };
                SqlDbType[] ParamTypes = new SqlDbType[] {SqlDbType.VarChar , SqlDbType.BigInt, SqlDbType.Bit };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(2000, 0, 0), new ParamLength(8, 19, 0), new ParamLength(1, 1, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Txtmenuxml, Intsearchid, Isrole };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public String GetCompanyPaymentModes(String ConnectionString, String CompanyType, String CompanyLoginId, out String CompanyName, out Boolean CreditMode, out Boolean OnlineMode)
        {
            Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            CompanyName = String.Empty;
            CreditMode = false;
            OnlineMode = false;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_get_company_details";
                String[] Params = new String[] { "@CompanyType", "@LoginId","@CompanyName","@CreditMode","@OnlineMode", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0), new ParamLength(20, 0, 0), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { CompanyType, CompanyLoginId, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                CompanyName = Convert.ToString(AllParameters[2]);
                CreditMode = Convert.ToString(AllParameters[3]) == "Y";
                OnlineMode = Convert.ToString(AllParameters[4]) == "Y";
                Message = Convert.ToString(AllParameters[5]);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return Message;
        }

        public String SaveCompanyPaymentModes(String ConnectionString, String CompanyType, String CompanyLoginId, Boolean CreditMode, Boolean OnlineMode)
        {
            Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_save_company_payment_details";
                String[] Params = new String[] { "@CompanyType", "@LoginId", "@CreditMode", "@OnlineMode","@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0), new ParamLength(20, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input,  ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CompanyType, CompanyLoginId, CreditMode? "Y" : "N", OnlineMode? "Y" : "N" , DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Message = Convert.ToString(AllParameters[4]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return Message;
        }


    }

}