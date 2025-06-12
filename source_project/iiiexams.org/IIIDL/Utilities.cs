using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databases.SQLServer;

namespace IIIDL
{
    public class Utility
    {
        public void SaveCandidateProfile(System.String ConnectionString, System.String UpdatedValue, System.String URN, Int32 UserId, System.String UpdateAction, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Error = String.Empty;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "STP_Details_update";
                String[] Params = new String[] { "@NewVal", "@URN", "@UserId", "@action", "@varSuccess" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(265, 0, 0), new ParamLength(14, 0, 0), new ParamLength(4, 10, 0), new ParamLength(50, 0, 0), new ParamLength(256, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { UpdatedValue, URN, UserId, UpdateAction, Message };
                try
                {
                    objDatabase = new Database();
                    //Comment the below line if the procedure does not return data set.
                    //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                    //Comment the below line if the procedure return data set.
                    ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                    Message = Convert.ToString(AllParameters[4]);

                }
                catch (Exception ex)
                {
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

        public DataSet GetCompanyDetails(System.String ConnectionString, System.String CompanyName, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Error = String.Empty;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "Sp_LoadCompanyDetails";
                String[] Params = new String[] { "@CompanyName" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(265, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { CompanyName };
                try
                {
                    objDatabase = new Database();
                    //Comment the below line if the procedure does not return data set.
                    //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                    //Comment the below line if the procedure return data set.
                    ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                    //Message = Convert.ToString(AllParameters[4]);

                }
                catch (Exception ex)
                {
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
            return objDataSet;
        }

        public String GetUserPassword(System.String ConnectionString, System.String UserLoginId, out System.String Password)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Password = String.Empty;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "Sp_GetUserPassword";
                String[] Params = new String[] { "@varUserLoginId", "@varPassword", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(128, 0, 0), new ParamLength(1024, 0, 0), new ParamLength(100, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { UserLoginId, Password, Message };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                Password = AllParameters[1].ToString();
                Message = AllParameters[2].ToString();
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
