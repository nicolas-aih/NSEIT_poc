using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;
using System.IO;
using System.Reflection;

namespace IIIDL
{
    public partial class URN
    {
        public DataSet GetURNForPAN(System.String ConnectionString, System.String PAN)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_urns_for_pan";
                String[] Params = new String[] { "@PAN" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { PAN };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
		        //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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
	        return objDataSet;
        }

        public String UnarchiveURN(System.String ConnectionString, System.String URN)
        {
            Databases.SQLServer.Database objDatabase = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_restore_urn";
                String[] Params = new String[] { "@URN" , "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN , DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Message = Convert.ToString(AllParameters[1]);
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

        public DataSet GetURNDetails(System.String ConnectionString, System.String URN, System.Int32 UserId )
        { 
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "STP_CMN_GetURNDetails_New2";
                String[] Params = new String[] { "@URN", "@intUserID", "@varErrorText" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Char, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(14, 0, 0), new ParamLength(4, 10, 0), new ParamLength(1000, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, UserId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
		        //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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
	        return objDataSet;
        }

        public DataSet GetPhotoAndSignature(System.String ConnectionString, System.String URN)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "STP_FEtch_RollNo_ApplicantPhoto";
                String[] Params = new String[] { "@chrRollNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(14, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { URN };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
        		//ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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
	        return objDataSet;
        }

        public void GenerateURN(System.String ConnectionString, System.Data.DataTable Applicantdata, System.String Rolecode, System.Int32 UserId, out System.String RollNo, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            RollNo = String.Empty;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "STP_LIC_SaveSponsorshipForm_New";
                String[] Params = new String[] { "@ApplicantData", "@RoleCode", "@UserId", "@varRollNo", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(14, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { Applicantdata, Rolecode, UserId, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RollNo = Convert.ToString(AllParameters[3]);
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
        }

        public DataSet GetDataForPartialModification(System.String ConnectionString, System.String URN, System.DateTime DOB, out String Message, Int32 InsurerUserId)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_details_for_quick_modification";
                String[] Params = new String[] { "@URN", "@DOB", "@MESSAGE", "@InsurerUserId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Date, SqlDbType.VarChar, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(3, 10, 0), new ParamLength(255, 0, 0) , new ParamLength(4,10,0)};
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Input };
                Object[] Values = new Object[] { URN, DOB, DBNull.Value, InsurerUserId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[2]);

                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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
	        return objDataSet;
        }

        public DataSet GetDataForModification(System.String ConnectionString, System.String URN, Int32 UserId, out String Status, out String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_all_details_for_modifiction";
                String[] Params = new String[] { "@URN", "@UserId", "@STATUS", "@MESSAGE" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int , SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(4,0,0) , new ParamLength(20, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, UserId, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Status = Convert.ToString(AllParameters[2]);
                Message = Convert.ToString(AllParameters[3]);

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

        public DataSet GetDataForURNRequestModification(System.String ConnectionString, System.Int64 Id, Int32 UserId, out String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_all_details_for_request_modification";
                String[] Params = new String[] { "@Id", "@UserId", "@MESSAGE" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 19, 0), new ParamLength(4, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Id, UserId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[2]);

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

        public DataSet GetDataForURNDuplication(System.String ConnectionString, System.String URN, Int32 UserId, out String Status, out String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_all_details_for_duplicate_URN";
                String[] Params = new String[] { "@URN", "@UserId", "@STATUS", "@MESSAGE" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(4, 0, 0), new ParamLength(20, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, UserId, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Status = Convert.ToString(AllParameters[2]);
                Message = Convert.ToString(AllParameters[3]);

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

        public void GenerateDuplicateURN(System.String ConnectionString, System.Data.DataTable Applicantdata, System.String Rolecode, System.Int32 UserId, out System.String RollNo, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_GenerateDuplicateURN";
                String[] Params = new String[] { "@ApplicantData", "@RoleCode", "@UserId", "@varRollNo", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(14, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { Applicantdata, Rolecode, UserId, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                RollNo = Convert.ToString(AllParameters[3]);
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
        }

        public void SaveModification(System.String ConnectionString, System.String URN, System.Int32 Languageid, System.Int32 Examcenterid, Byte[] Photo, Byte[] Sign, System.String AllowWhatsappMessage, System.String WhatsappNumber, out System.String Status, out System.String Message,
            Int32 UserId, Int32 InsurerUserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Error = String.Empty;

            Int32 ModuleId = -1;
            String URNForExamBody = string.Empty;
            String ExamLanguage = string.Empty;
            Int32 OAIMSExamCenterId = -1;
            Int32 UpdateAims = 0;
            try
            {
                System.String ProcedureName = "sp_save_candidate_details2";
                String[] Params = new String[] { "@URN", "@LanguageId", "@ExamCenterId", "@Photo", "@Sign", "@AllowWhatsapp_message", "@Whatsapp_number", "@STATUS", "@MESSAGE", "@module_id", "@URNForExamBody", "@ExamLanguage", "@OAIMSExamCenterId", "@UpdateAims", "@UserId", "@InsurerUserId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarBinary, SqlDbType.VarBinary, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(-1, 0, 0), new ParamLength(-1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(20, 0, 0), new ParamLength(20, 0, 0), new ParamLength(255, 0, 0), new ParamLength(4, 10, 0), new ParamLength(20, 0, 0), new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { URN, Languageid == -1 ? (Object)DBNull.Value : Languageid, Examcenterid == -1 ? (Object)DBNull.Value : Examcenterid, Photo, Sign, AllowWhatsappMessage, WhatsappNumber,DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, UserId, InsurerUserId };
                try
                {
                    objDatabase = new Database();
                    //Comment the below line if the procedure does not return data set.
                    //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                    //Comment the below line if the procedure return data set.
                    ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                    Status = Convert.ToString(AllParameters[7]);
                    Message = Convert.ToString(AllParameters[8]);
                    /*ModuleId = Convert.ToInt32(AllParameters[9]);
                    URNForExamBody = Convert.ToString(AllParameters[10]);
                    ExamLanguage = Convert.ToString(AllParameters[11]);
                    OAIMSExamCenterId = Convert.ToInt32(AllParameters[12]);
                    UpdateAims = Convert.ToInt32(AllParameters[13]);*/
                }
                catch (Exception ex)
                {
                    Status = "FAIL";
                    Message = "Error Occured While Updating The Data.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDatabase = null;
            }
        }

        public String UpdateURNDetails(System.String ConnectionString, System.Data.DataTable ApplicantData, System.Int32 UserId )
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "sp_Update_SponsorshipForm_New";
                String[] Params = new String[] { "@ApplicantData", "@UserId", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output};
                Object[] Values = new Object[] { ApplicantData, UserId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[2]);
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
            //return objDataSet;
            return Message;
        }

        public String /*Message*/ UpdateURNRequest(System.String ConnectionString, Int64 Id, System.Data.DataTable ApplicantData, System.String RoleCode , System.Int32 UserId, out String URN)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            URN = String.Empty;
            try
            {
                System.String ProcedureName = "sp_Update_URNRequest_New";
                String[] Params = new String[] {"@Id", "@ApplicantData", "@RoleCode", "@UserId", "@URN", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] {SqlDbType.BigInt, SqlDbType.Structured, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 19, 0), new ParamLength(-1, 0, 0), new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(20, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] {Id, ApplicantData, RoleCode, UserId, DBNull.Value , DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                URN = Convert.ToString(AllParameters[4]);
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
            //return objDataSet;
            return Message;
        }

        public DataSet UploadDuplicateURN(System.String ConnectionString, DataTable ExcelData , System.Int32 UserId, String RoleCode )
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "SP_DuplicateURNBulkCreation_New";
                String[] Params = new String[] { "@ExcelData", "@intUserID", "@RoleCode" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(20, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { ExcelData, UserId, RoleCode };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
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

        public DataSet UploadSponsorshipFileCorporates(System.String ConnectionString, System.Data.DataTable ApplicantData, System.Int32 UserId, System.String RoleCode, System.String OriginalFileName, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
            try
	        {
		        System.String ProcedureName = "STP_BulkUploadSponsorship_CorporateUser_New";
                String[] Params = new String[] { "@ApplicantData", "@intUserID", "@RoleCode", "@OriginalFileName", "@Message", "@IsSourceService" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(20, 0, 0), new ParamLength(256, 0, 0), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Input };
                Object[] Values = new Object[] { ApplicantData, UserId, RoleCode, OriginalFileName, DBNull.Value, "N" }; //N Hardcoded for Web Site / Y For FTP Service
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Message = Convert.ToString(AllParameters[4]);
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
	        return objDataSet;
        }

        public DataSet UploadSponsorshipFileAgents(System.String ConnectionString, System.Data.DataTable ApplicantData, System.Int32 UserId, System.String InsuranceType, System.String OriginalFileName, out System.String Message )
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "STP_LIC_BulkUploadSponsorship_ForExamPortal_New";
                String[] Params = new String[] { "@ApplicantData", "@intUserID", "@chrInsuranceType", "@OriginalFileName", "@Message", "@IsSourceService" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int, SqlDbType.Char, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(1, 0, 0), new ParamLength(256, 0, 0), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Input };
                Object[] Values = new Object[] { ApplicantData,UserId, InsuranceType, OriginalFileName, DBNull.Value, "N" }; //N Hardcoded for Web Site / Y For FTP Service
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Message = Convert.ToString(AllParameters[4]);
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
	        return objDataSet;
        }

        //Validator functions....
        //Just checks if the Reference no is in use for any other URN of same company.
        public Boolean ValidateInternalRefNo(System.String ConnectionString, System.String InternalRefNo, System.Int32 InsurerId, System.Int64 ApplicantId = 0)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Boolean RetVal = false;
	        try
	        {
		        System.String ProcedureName = "STP_LIC_ValidateExternalRefNo_New";
                String[] Params = new String[] { "@varExtRefNo", "@intInsurerID", "@bntApplicantID", "@blnSucess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.BigInt, SqlDbType.Bit };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(256, 0, 0), new ParamLength(4, 10, 0), new ParamLength(8, 19, 0), new ParamLength(1, 1, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { InternalRefNo, InsurerId, ApplicantId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RetVal = Convert.ToInt32(AllParameters[3]) == 1;
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

        public Boolean ValidateInternalRefNoApp(System.String ConnectionString, System.String InternalRefNo, System.Int32 InsurerId, System.Int64 ApplicantDataId = 0)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Boolean RetVal = false;
            try
            {
                System.String ProcedureName = "STP_LIC_ValidateExternalRefNo_New_App";

                String[] Params = new String[] { "@varExtRefNo", "@intInsurerID", "@bntApplicantDataId", "@blnSucess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.BigInt, SqlDbType.Bit };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(256, 0, 0), new ParamLength(4, 10, 0), new ParamLength(8, 19, 0), new ParamLength(1, 1, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { InternalRefNo, InsurerId, ApplicantDataId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RetVal = Convert.ToInt32(AllParameters[3]) == 1;
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

        // Just checks if the PAN is debarred
        public String ValidatePAN(System.String ConnectionString, System.String PAN, Int64 ApplicationId, Int32 InsurerUserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String RetVal = String.Empty;
            try
            {

                //sp_validate_PAN
                //@varPAN         VARCHAR(10),
                //@bntApplicantID BIGINT = 0,
                //@InsurerUserId INT,
                //@varSuccess VARCHAR
                System.String ProcedureName = "sp_validate_PAN";
                String[] Params = new String[] { "@varPAN", "@bntApplicantID", "@InsurerUserId", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0), new ParamLength(8, 19, 0), new ParamLength(4, 10, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { PAN, ApplicationId <= 0 ? (Object)DBNull.Value : ApplicationId, InsurerUserId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RetVal = Convert.ToString(AllParameters[3]);
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

        public String ValidatePAN(System.String ConnectionString, System.String PAN )
        {
	        Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String RetVal = String.Empty;
	        try
	        {
		        System.String ProcedureName = "STP_LIC_ValidatePAN_New";
                String[] Params = new String[] { "@varPAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters,true);
                RetVal = Convert.ToString(AllParameters[1]);
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

        public String  ValidateAadhaarCorporates(System.String ConnectionString, System.String AadhaarNo, System.String PAN, System.String URN)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            String Status = String.Empty;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_ValidateAadhaar_PAN_Corporates_New";
                String[] Params = new String[] { "@AadhaarNo", "@PAN","@URN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(256, 0, 0), new ParamLength(10, 0, 0), new ParamLength(20, 0, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { AadhaarNo, PAN, (String.IsNullOrEmpty(URN.Trim()) ? (Object)DBNull.Value : URN) , DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Status = Convert.ToString(AllParameters[3]);
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
            return Status;
        }

        public String ValidateAadhaarCorporatesApp(System.String ConnectionString, System.String AadhaarNo, System.String PAN, Int64 ApplicantDataId)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            String Status = String.Empty;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_ValidateAadhaar_PAN_Corporates_New_APP";
                String[] Params = new String[] { "@AadhaarNo", "@PAN", "@bntApplicantDataId", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(256, 0, 0), new ParamLength(10, 0, 0), new ParamLength(8, 19, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { AadhaarNo, PAN, ApplicantDataId, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Status = Convert.ToString(AllParameters[3]);
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
            return Status;
        }

        public String ValidateEmailCorporates(System.String ConnectionString, System.String EmailId, System.Int64 Applicantid, System.String PAN)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "sp_validate_email_corporates";
                String[] Params = new String[] { "@email_id", "@bntApplicantId", "@PAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0), new ParamLength(8, 19, 0), new ParamLength(10, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { EmailId, Applicantid, PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ValidateEmailCorporatesApp(System.String ConnectionString, System.String EmailId, System.Int64 ApplicaDataId, System.String PAN)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_validate_email_corporates_App";

                String[] Params = new String[] { "@email_id", "@bntApplicantDataId", "@PAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0), new ParamLength(8, 19, 0), new ParamLength(10, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { EmailId, ApplicaDataId, PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ValidateMobileCorporates(System.String ConnectionString, System.String MobileNo, System.Int64 Applicantid, System.String PAN)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_validate_mobile_corporates";
                String[] Params = new String[] { "@mobile", "@bntApplicantId", "@PAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(8, 19, 0), new ParamLength(10, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { MobileNo, Applicantid, PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ValidateWhatsAppCorporates(System.String ConnectionString, System.String MobileNo, System.Int64 Applicantid, System.String PAN)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_validate_whatsapp_corporates";
                String[] Params = new String[] { "@mobile", "@bntApplicantId", "@PAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(8, 19, 0), new ParamLength(10, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { MobileNo, Applicantid, PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ValidateMobileCorporatesApp(System.String ConnectionString, System.String MobileNo, System.Int64 ApplicantDataId, System.String PAN)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_validate_mobile_corporates_APP";
                String[] Params = new String[] { "@mobile", "@bntApplicantDataId", "@PAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(8, 19, 0), new ParamLength(10, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { MobileNo, ApplicantDataId, PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ValidateWhatsAppCorporatesApp(System.String ConnectionString, System.String MobileNo, System.Int64 ApplicantDataId, System.String PAN)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_validate_whatsapp_corporates_APP";
                String[] Params = new String[] { "@mobile", "@bntApplicantDataId", "@PAN", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(8, 19, 0), new ParamLength(10, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { MobileNo, ApplicantDataId, PAN, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ValidateWhatsAppCorporatesForMod(System.String ConnectionString, System.String URN, System.String MobileNo)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                //CREATE procedure[dbo].[sp_validate_whatsapp_corporates_for_mod] (
                //@urnIn          varchar(20),
                //@mobile varchar(20),
                //@varSuccess varchar(255) out
                //)

                System.String ProcedureName = "sp_validate_whatsapp_corporates_for_mod";
                String[] Params = new String[] {"@urnIn", "@mobile", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(20, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] {URN, MobileNo, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[2]);
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

        public DataSet GetURNDetailForEE(System.String ConnectionString, System.String URN)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "STP_LIC_Applicant_RollNo_Lookup_ForExamDetails_New";
                String[] Params = new String[] { "@chrRollNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(14, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { URN };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
		        //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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
	        return objDataSet;
        }

        public String SaveExamDetails(System.String ConnectionString, System.Int32 Hint, System.String URN, System.String ExamDate, System.String ExamineeId, System.Int32 Marks, System.Int32 Result, System.Int32 CurrentUser)
        {
	        Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            String RetVal = String.Empty;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "STP_LIC_Enter_ExamDetails_New";
                String[] Params = new String[] { "@Hint", "@URN", "@ExamDate", "@ExamineeId", "@Marks", "@Result", "@CurrentUser", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(20, 0, 0), new ParamLength(8, 23, 3), new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Hint, URN, ExamDate, ExamineeId, Marks == -1 ? (Object)DBNull.Value: Marks, Result == -1? (Object)DBNull.Value: Result, CurrentUser, String.Empty };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                RetVal = Convert.ToString(AllParameters[7]);
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

        public String UpdateUrnStatus(System.String ConnectionString, System.String URN, System.String ChangedStatus, System.String UserName, System.String UserId, System.String UserMachineIP )
        {
            String RetVal = String.Empty;
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_CS_UpdateUrnStatus";
                String[] Params = new String[] { "@IRDAUrn", "@ChangedStatus", "@userName", "@userID", "@userMachineIP", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0), new ParamLength(100, 0, 0), new ParamLength(100, 0, 0), new ParamLength(100, 0, 0), new ParamLength(100, 0, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, ChangedStatus, UserName, UserId, UserMachineIP, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                RetVal = Convert.ToString(AllParameters[5]);
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

        #region Payment Related
        public void UpdatePaymentStatus(System.String ConnectionString, System.String TransactionId, System.String NseitRefNo, System.String PgRefNo, System.String PgStatus, System.String PgResponse )
        {
	        Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_update_batch_summary";
                String[] Params = new String[] { "@transaction_id", "@nseit_ref_no", "@pg_ref_no", "@pg_status", "@pg_response" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0), new ParamLength(50, 0, 0), new ParamLength(50, 0, 0), new ParamLength(20, 0, 0), new ParamLength(-1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input};
                Object[] Values = new Object[] { TransactionId, NseitRefNo, PgRefNo, PgStatus, PgResponse };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
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
	        //return objDataSet;
        }

        public DataSet GetURNDataForDeletion(String ConnectionString, String URN, Int32 UserID, out String Message )
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_urn_details_for_deletion";
                String[] Params = new String[] { "@urn", "@intUserId", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output};
                Object[] Values = new Object[] { URN, UserID, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[2]);
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

        public String DeleteURN(String ConnectionString, String URN, Int32 UserID)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_delete_urn";
                String[] Params = new String[] { "@urn", "@intUserId", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, UserID, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[2]);
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
        #endregion

        public DataSet GetQualData(String ConnectionString, String URN,  Int32 UserID)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_qualification_docs";
                String[] Params = new String[] { "@urn", "@userid"  };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] {new ParamLength(20, 0, 0),  new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { URN, UserID};
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
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
        
        public DataSet GetURNDataForApproval(String ConnectionString,Int32 Hint, Int64 Id, Int32 UserID, out String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_urn_details_for_approval";
                String[] Params = new String[] {"@hint", "@id", "@userid", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.BigInt, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(8, 19, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] {Hint, Id < 0 ?(Object)DBNull.Value:Id, UserID, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[3]);
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

        public String ApproveRejectURN(System.String ConnectionString, System.Int64 Id, System.Int32 UserId, System.String ApproversRemarks, System.String Status )
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
		        System.String ProcedureName = "SP_AR_SponsorshipForm";
                String[] Params = new String[] { "@bntApplicantDataID", "@IIIUserId", "@Approvers_remarks", "@status", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 19, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Id, UserId, ApproversRemarks, Status, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[4]);
            }
	        catch (Exception ex)
	        {
		        throw(ex);
	        }
	        finally
	        {
		        objDatabase = null;
	        }
	        return Message;
        }

        public DataSet UpdatePS(System.String ConnectionString, System.Data.DataTable ApplicantData, System.Int32 UserId, System.String OriginalFileName)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_BulkUpdateApplicantPhotoSign_New";
                String[] Params = new String[] { "@ApplicantData", "@intUserID", "@OriginalFileName" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0)};
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input};
                Object[] Values = new Object[] { ApplicantData, UserId, OriginalFileName };
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

        public DataSet UploadUrnExamCentreUpdate(System.String ConnectionString, DataTable ExcelData, System.Int32 UserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_reset_testcenter";
                String[] Params = new String[] { "@data", "@user_id" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { ExcelData, UserId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
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

        public DataSet UploadTrainingDetails(System.String ConnectionString, System.Data.DataTable InputData, System.Int32 UserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_upload_training_details";
                String[] Params = new String[] { "@data", "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { InputData, UserId };
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


        public Boolean ValidateURNandDOB(System.String ConnectionString, System.String URN, System.DateTime DOB)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            Boolean ValResp = false;
            try
            {
                //CREATE procedure[dbo].[sp_validate_whatsapp_corporates_for_mod] (
                //@urnIn          varchar(20),
                //@mobile varchar(20),
                //@varSuccess varchar(255) out
                //)

                System.String ProcedureName = "Sp_Validate_Urn_Dob";
                String[] Params = new String[] { "@varURN", "@varDOB", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Bit };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(20, 0, 0), new ParamLength(20, 0, 0), new ParamLength(1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, DOB, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                ValResp = Convert.ToBoolean(AllParameters[2]);
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
            return ValResp;
        }


    }
}
