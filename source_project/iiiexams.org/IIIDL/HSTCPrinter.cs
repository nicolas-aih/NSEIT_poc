using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;

namespace IIIDL
{
    public class HSTCPrinter
    {
        public DataSet GetScoreCard(System.String ConnectionString, System.String URN, DateTime DOB, DateTime ExamDate, Int32 Source, out String Error)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_scorecard";
                String[] Params = new String[] { "@URN", "@DOB", "@ExamDate", "@Source", "@ERROR" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(16, 0, 0), new ParamLength(8, 23, 3), new ParamLength(8, 23, 3), new ParamLength(4, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, DOB == DateTime.MinValue ? (Object)DBNull.Value : DOB, ExamDate == DateTime.MinValue ? (Object)DBNull.Value:ExamDate , Source, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Error = Convert.ToString(AllParameters[4]);
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

        public DataSet GetHallTicket(System.String ConnectionString, String URN, DateTime DOB, DateTime ExamDate, Int32 Source, out String Error )
        {
            Error = String.Empty;
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_hallticket";
                String[] Params = new String[] { "@URN", "@DOB", "@ExamDate", "@Source" , "@ERROR" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(16, 0, 0), new ParamLength(8, 23, 3), new ParamLength(8, 23, 3), new ParamLength(4, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, DOB == DateTime.MinValue ? (Object)DBNull.Value : DOB, ExamDate == DateTime.MinValue ? (Object)DBNull.Value : ExamDate, Source, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Error = Convert.ToString(AllParameters[4]);
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
    }
}
