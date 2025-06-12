using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;

namespace IIIDL
{
    public class Branches
    {
        public String InsertBranches(System.String ConnectionString, System.Int32 UserId, System.String BranchAddress, System.String BranchCode, System.String BranchName, System.String BranchPlace, System.Int32 StateId, System.Int32 DistrictId, System.Boolean IsActive )
        {
            String Retval = String.Empty;
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_insert_branch_details";
                String[] Params = new String[] { "@UserID", "@BranchAddress", "@BranchCode", "@BranchName", "@BranchPlace", "@tntStateID", "@sntDistrictID",  "@IsActive", "@error" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(512, 0, 0), new ParamLength(50, 0, 0), new ParamLength(50, 0, 0), new ParamLength(50, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0),new ParamLength(1,0,0), new ParamLength(250, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { UserId, BranchAddress, BranchCode, BranchName, BranchPlace, StateId, DistrictId, IsActive ? "Y":"N", DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Retval = AllParameters[8] == DBNull.Value ? String.Empty : Convert.ToString(AllParameters[8]);
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
	        return Retval;
        }

        public String UpdateBranches(System.String ConnectionString, Int64 BranchId, System.Int32 UserId, System.String BranchAddress, System.String BranchCode, System.String BranchName, System.String BranchPlace, System.Int32 StateId, System.Int32 DistrictId, System.Boolean IsActive)
        {
            String Retval = String.Empty;
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_update_branch_details";
                String[] Params = new String[] { "@UserID", "@BranchNo", "@BranchAddress", "@BranchCode", "@BranchName", "@BranchPlace", "@tntStateID", "@sntDistrictID", "@IsActive", "@error" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(8, 19, 0), new ParamLength(512, 0, 0), new ParamLength(50, 0, 0), new ParamLength(50, 0, 0), new ParamLength(50, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(1, 0, 0), new ParamLength(250, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { UserId, BranchId, BranchAddress, BranchCode, BranchName, BranchPlace, StateId, DistrictId, IsActive ? "Y": "N", DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Retval = AllParameters[9] == DBNull.Value ? String.Empty : Convert.ToString(AllParameters[9]);
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
            return Retval;
        }

        public DataSet GetBranchDetails (System.String ConnectionString, System.Int32 UserId, System.Int32 StateId, System.Int32 DistrictId)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_branch_details";
                String[] Params = new String[] { "@UserID", "@tntStateID", "@sntDistrictId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.Int};
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0)};
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { UserId, (StateId == -1) ? (Object)DBNull.Value : StateId, (DistrictId == -1) ? (Object)DBNull.Value : DistrictId };
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

        public DataSet GetBranchDetails(System.String ConnectionString, System.Int32 UserId, System.Int32 BranchId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_branch_details2";
                String[] Params = new String[] { "@UserID", "@branchno" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { UserId, BranchId };
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

        public DataSet UploadBranches(System.String ConnectionString, System.Int32 CorporateUserId, System.Data.DataTable BranchDetails, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_UploadBranchDetails";
                String[] Params = new String[] { "@intUserID", "@BranchDetails", "@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Structured, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(-1, 0, 0), new ParamLength(255, 0, 0)};
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CorporateUserId, BranchDetails, DBNull.Value };
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

        /**********************************************************************************************************************/
        public DataSet GetBranchReport(System.String ConnectionString, System.Int32 Userid)
        {
            Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_Get_Branch_BulkData";
                String[] Params = new String[] { "@UserID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { Userid };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
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
    }
}
