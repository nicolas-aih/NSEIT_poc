using Databases.SQLServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIIDL
{
    public class DLSponsorshipForm
    {
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

        public DataSet BulkUploadToDatabase(System.String ConnectionString, System.Data.DataTable Exceldata, System.Int32 Intuserid,  out System.String Status, out System.String Message)
        {

            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_LIC_BulkUploadExamRegistration_New";
                String[] Params = new String[] { "@ExcelData", "@intUserID", "@IntSucess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Exceldata, Intuserid, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
               ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Status = Convert.ToString(AllParameters[2]);
                Message = "success";
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
