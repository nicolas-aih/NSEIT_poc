using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using ora = OraDatabases.OracleDB;

namespace IIIDL
{
    public class ExamReports
    {
        public DataSet GetCorporateExaminationReport(System.String ConnectionString, System.String ExamMonth, System.Int32 ExamYear, System.String UserRole)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_RPT_CorporateExaminationReport";
                String[] Params = new String[] { "@dtExamMonth", "@dtExamYear", "@varLoggedInUserRole" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(10, 0, 0), new ParamLength(4, 10, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { ExamMonth, ExamYear, UserRole };
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

        public DataSet GetApprovedCorporateAgent(System.String ConnectionString)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
                                       // DataTable objdatatable = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_RPT_ApprovedCorporateAgent";
                String[] Params = new String[] {  };
                SqlDbType[] ParamTypes = new SqlDbType[] {  };
                ParamLength[] ParamLengths = new ParamLength[] {  };
                ParameterDirection[] ParamDirections = new ParameterDirection[] {  };
                Object[] Values = new Object[] { };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                //objdatatable = objDataSet.Tables[0];

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

        public DataSet GetSponsorshipReportForCorporates(System.String ConnectionString, System.Int32 InsurerUserId, System.DateTime ApplicationDateFrom, 
            System.DateTime ApplicationDateTo, System.String UserRole, System.String URN, System.String InsurerExtnRefNo, 
            System.String ExamBatch, System.Int32 ExamBodyId, System.Int32 ExamCenterId, 
            Boolean appStatusALL, Boolean appStatusS, Boolean appStatusT, Boolean appStatusEC, Boolean appStatusEA,
            Boolean appStatusE, Boolean includephoto, Boolean includesign, System.String ExamDateFrom, System.String ExamDateTo)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {

                System.String ProcedureName = "STP_RPT_SponsorshipStatusForCorporate_New";

                String[] Params = new String[] { "@intTblMstInsurerUserID", "@dtAppFrom", "@dtAppTo", "@varLoggedInUserRole", "@chrRollNumber", "@varInsurerExtnRefNo", "@varExamBatchID", "@tntExamBodyID", "@sntExamCenterID", "@bitAppStatus_ALL", "@bitAppStatus_S", "@bitAppStatus_T", "@bitAppStatus_EC", "@bitAppStatus_EA", "@bitAppStatus_E", "@bitIncludePhoto", "@bitIncludeSign", "@dtExamFrom", "@dtExamTo" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Char, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.TinyInt, SqlDbType.SmallInt, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(8, 23, 3), new ParamLength(8, 23, 3), new ParamLength(256, 0, 0), new ParamLength(14, 0, 0), new ParamLength(100, 0, 0), new ParamLength(30, 0, 0), new ParamLength(1, 3, 0), new ParamLength(2, 5, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(11, 0, 0), new ParamLength(11, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { InsurerUserId, ApplicationDateFrom, ApplicationDateTo, UserRole, URN, InsurerExtnRefNo, ExamBatch, ExamBodyId, ExamCenterId, appStatusALL, appStatusS, appStatusT, appStatusEC, appStatusEA, appStatusE, includephoto, includesign, ExamDateFrom, ExamDateTo };
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

        //public DataSet GetSponsorshipReport(System.String ConnectionString, System.DateTime Dtappfrom, System.DateTime Dtappto, 
        //    System.String Varloggedinuserrole, System.Int16 Tntinstypeid, System.Int32 Inttblmstinsureruserid, System.Int32 Inttblmstdpuserid, 
        //    System.Int32 Inttblmstagntcounseloruserid, System.String Chrrollnumber, System.String Varinsurerextnrefno, 
        //    System.Int32 Inttblmstatiuserid, System.Int16 Tntexambodyid, System.Int16 Sntexamcenterid, SqlDbType.Bit BitappstatusALL, SqlDbType.Bit BitappstatusS, SqlDbType.Bit BitappstatusTA, 
        //    SqlDbType.Bit BitappstatusT, SqlDbType.Bit BitappstatusEC, SqlDbType.Bit BitappstatusEA, SqlDbType.Bit BitappstatusE, 
        //    SqlDbType.Bit BitappstatusL, SqlDbType.Bit BitappstatusR, SqlDbType.Bit BitappstatusCA, SqlDbType.Bit BitappstatusCR, 
        //    SqlDbType.Bit Bitincludephoto, SqlDbType.Bit Bitincludesign, System.String Dtexamfrom, System.String Dtexamto)

        public DataSet GetSponsorshipReport(System.String ConnectionString, System.DateTime ApplicationDateFrom, System.DateTime ApplicationDateTo, 
            System.String UserRole,Int16 Tntinstypeid, System.Int32 InsurerUserId, Int32 DPUserId, Int32 ACUserId, System.String URN, System.String InsurerExtnRefNo, 
            System.String ExamBatch, System.Int32 ExamBodyId, System.Int32 ExamCenterId, 
            Boolean appStatusALL, Boolean appStatusS, Boolean appStatusT, Boolean appStatusEC, Boolean appStatusEA,
            Boolean appStatusE, Boolean includephoto, Boolean includesign, System.String ExamDateFrom, System.String ExamDateTo)
        {
	        Databases.SQLServer.Database objDatabase = null;
                DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
                Int32 ProcReturnValue = 0;
                Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "STP_RPT_SponsorshipStatus";
                String[] Params = new String[] { "@dtAppFrom", "@dtAppTo", "@varLoggedInUserRole", "@tntInsTypeID", "@intTblMstInsurerUserID", "@intTblMstDPUserID", "@intTblMstAgntCounselorUserID", "@chrRollNumber", "@varInsurerExtnRefNo", "@intTblMstATIUserID", "@tntExamBodyID", "@sntExamCenterID", "@bitAppStatus_ALL", "@bitAppStatus_S", "@bitAppStatus_TA", "@bitAppStatus_T", "@bitAppStatus_EC", "@bitAppStatus_EA", "@bitAppStatus_E", "@bitAppStatus_L", "@bitAppStatus_R", "@bitAppStatus_CA", "@bitAppStatus_CR", "@bitIncludePhoto", "@bitIncludeSign", "@dtExamFrom", "@dtExamTo" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.TinyInt, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Char, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.TinyInt, SqlDbType.SmallInt, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 23, 3), new ParamLength(8, 23, 3), new ParamLength(256, 0, 0), new ParamLength(1, 3, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(14, 0, 0), new ParamLength(100, 0, 0), new ParamLength(4, 10, 0), new ParamLength(1, 3, 0), new ParamLength(2, 5, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(1, 1, 0), new ParamLength(11, 0, 0), new ParamLength(11, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { ApplicationDateFrom, ApplicationDateTo, UserRole, 0, InsurerUserId, DPUserId, ACUserId, URN, InsurerExtnRefNo, 0 , ExamBodyId, ExamCenterId, appStatusALL, appStatusS, 0, appStatusT, appStatusEC, appStatusEA, appStatusE, 0, 0, 0, 0, includephoto, includesign, ExamDateFrom, ExamDateTo };
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


        public DataSet UploadExamDetails(System.String ConnectionString, System.Data.DataTable InputData, Boolean AllocateSlot, System.Int32 UserId )
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
	        {
		        System.String ProcedureName = "STP_LIC_BulkUploadExamDetails_New";
                String[] Params = new String[] { "@InputData", "@bitAllocateSlot", "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Bit, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(1, 1, 0), new ParamLength(4, 10, 0)};
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { InputData, AllocateSlot, UserId };
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

        public DataSet UploadAIMSResponse(System.String ConnectionString, System.Data.DataTable InputData, System.Int32 UserId)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
	        {
		        System.String ProcedureName = "STP_LIC_BulkUploadExcludedBatches_New";
                String[] Params = new String[] { "@InputData", "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { InputData, UserId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
               
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

        public DataSet DownloadApplicantDetails(System.String ConnectionString, System.Int32 UserId, System.DateTime FromDate, System.DateTime TillDate)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "STP_LIC_GetExamDetails_N";
                String[] Params = new String[] { "@intLoggedInUserID", "@varFromDate", "@varToDate" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Date, SqlDbType.Date };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(3, 10, 0), new ParamLength(3, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { UserId, FromDate, TillDate };
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

        public DataSet GetExaminationReport(System.String ConnectionString, System.Int32 Option, System.Int32 UserId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_TrainingExaminationReport";
                String[] Params = new String[] { "@intOption", "@intUserId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Option, UserId };
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

        public DataSet GeneratePaymentReportOAIMS(String OracleConnectionString, System.String CompanyCode, System.DateTime FromDate, System.DateTime ToDate )
        {
            ora.Database objOraDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Object[] AllOraParameters = null;
            DataTable objDataTableOra = null;
            DataSet objOraDataSet = null;
            Boolean OracleSuccess = false;
            try
            {
                objOraDatabase = new ora.Database();
                System.String OraProcedureName = "SP_GET_PAYMENT_REPORT";
                String[] OraParams = new String[] { "p_company_code", "p_from_date", "p_till_date", "p_Main" };
                OracleDbType[] OraParamTypes = new OracleDbType[] { OracleDbType.Varchar2, OracleDbType.Date, OracleDbType.Date, OracleDbType.RefCursor };
                //sql.ParamLength[] OraParamLengths = new sql.ParamLength[] { new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(255, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0) };
                OracleCollectionType[] oracleCollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };
                ParameterDirection[] OraParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] OraValues = new Object[] { CompanyCode, FromDate, ToDate, DBNull.Value };

                objOraDatabase.ExecProcedure(OracleConnectionString, OraProcedureName, OraParams, OraParamTypes, OraParamDirections, OraValues, oracleCollectionType, out AllOraParameters, out objOraDataSet, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objOraDatabase = null;
            }
            return objOraDataSet;
        }

    }
}
