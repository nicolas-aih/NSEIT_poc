using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databases.SQLServer;

namespace IIIDL
{
    public  class ReportsInfra
    {
        public String SaveReportRequest(System.String ConnectionString, Int64 CompanyId, String ReportType, String ReportParameters)
        {
            Databases.SQLServer.Database objDatabase = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;

            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "Sp_SaveReportRequest";
                String[] Params = new String[] { "@CompanyId", "@ReportType", "@ReportParameters", "@MESSAGE" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(50, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CompanyId, ReportType, ReportParameters, DBNull.Value };
                objDatabase = new Database();
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

            return Message;
        }
        public DataSet GetReportRequests(System.String ConnectionString, System.Int32 InsurerUserID)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "Sp_GetOpenReportRequests";
                String[] Params = new String[] { "@CompanyId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { InsurerUserID };
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
    }
}
