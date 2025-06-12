using Databases.SQLServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace IIIDL
{
    public class ExamRegistrations
    {
        public DataSet GetExamBody(System.String ConnectionString, System.Int16 Tntexambodyid, System.String Chrexammode, System.Int32 Intuserid)
        {
            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "stp_ADM_GetExamBodyDetails";
                String[] Params = new String[] { "@tntExamBodyID", "@chrExamMode", "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.TinyInt, SqlDbType.Char, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(1, 3, 0), new ParamLength(1, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Tntexambodyid, Chrexammode, Intuserid };
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

        public DataSet GetTrainedApplicants(System.String ConnectionString, System.Int32 UserId , System.Int32 ExamBodyId, System.Int32 ExamCenterId, System.DateTime FromDate, System.DateTime ToDate)
        {
            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_GetTrainedApplicantsToConfirm_New";
                String[] Params = new String[] { "@intUserId", "@IntExamBodyID", "@IntExamCenterID", "@dtApplicationDate", "@dtApplicationToDate" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Date, SqlDbType.Date };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(3, 10, 0), new ParamLength(3, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { UserId, ExamBodyId, ExamCenterId <= 0 ? (Object)DBNull.Value : ExamCenterId, FromDate.Date, ToDate.Date };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                // ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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

        //To be deprecated going ahead. FE to call BulkUploadToDatabase...
        //public DataSet ConfirmExamination(System.String ConnectionString, string Txtapplicantdata, System.String Varpaymentmode, System.String Varremarks, System.Int32 Intloggedinuserid, System.String Varbatchmode, System.String Varschedulingmode, out System.String Status)
        //{

        //    Database objDatabase = null;
        //    DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
        //    Int32 ProcReturnValue = 0;
        //    Object[] AllParameters = null;
        //    try
        //    {
        //        System.String ProcedureName = "STP_LIC_ConfirmApplicantForExamination_New";
        //        String[] Params = new String[] { "@txtApplicantData", "@varPaymentMode", "@varRemarks", "@intLoggedInUserID", "@varBatchMode", "@varSchedulingMode", "@varExamBatchNo", "@IntSuccess" };
        //        SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int };
        //        ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(20, 0, 0), new ParamLength(1000, 0, 0), new ParamLength(4, 10, 0), new ParamLength(20, 0, 0), new ParamLength(20, 0, 0), new ParamLength(100, 0, 0), new ParamLength(4, 10, 0) };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
        //        Object[] Values = new Object[] { Txtapplicantdata, Varpaymentmode, Varremarks, Intloggedinuserid, Varbatchmode, Varschedulingmode, DBNull.Value, DBNull.Value };
        //        objDatabase = new Database();
        //        //Comment the below line if the procedure does not return data set.
        //        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
        //        Status = Convert.ToString(AllParameters[7]);
        //        //Comment the below line if the procedure return data set.
        //        // ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
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
        //    return objDataSet;
        //}

        public DataSet GetPaymentMode(System.String ConnectionString, System.Int32 Inttblmstinsureruserid)
        {
            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_GetPaymentMode";
                String[] Params = new String[] { "@intTblMstInsurerUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { Inttblmstinsureruserid };
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

        public DataTable GetExcelData(string strExcelFile)
        {
            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelFile + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
            OleDbConnection oleExcelConnection = new OleDbConnection(excelConnectionString);
            DataTable dtExcelData = new DataTable();

            try
            {
                oleExcelConnection.Open();

                OleDbCommand oleCommand = new OleDbCommand("Select * FROM [Sheet1$]", oleExcelConnection);
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter(oleCommand);

                oleAdapter.Fill(dtExcelData);
                oleExcelConnection.Close();
            }
            catch (Exception ex)
            {
                if (oleExcelConnection.State == ConnectionState.Open)
                    oleExcelConnection.Close();

                throw ex;
            }

            return dtExcelData;
        }

        public DataSet BulkUploadToDatabase(System.String ConnectionString, System.Data.DataTable Exceldata, System.Int32 Intuserid)
        {

            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_BulkUploadExamRegistration_New";
                String[] Params = new String[] { "@ExcelData", "@intUserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Exceldata, Intuserid };
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

    }
}
