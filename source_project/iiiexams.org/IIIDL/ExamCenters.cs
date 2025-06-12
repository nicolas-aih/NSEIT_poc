using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;

namespace IIIDL
{
    public class ExamCenters
    {
        public DataSet FindNearestExamCenter(System.String ConnectionString, System.Int32 Pincode, Int32 ExamBodyId =5 /*for nseit - III online exams*/)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_CMN_FindNearestExamCenter2";
                String[] Params = new String[] { "@intPINCode", "@tntExamBodyID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.TinyInt };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(1, 3, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Pincode, ExamBodyId };
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

        public DataSet GetExamCenters(System.String ConnectionString, System.Int32 Pincode, Int32 ExamBodyId = 5 /*for nseit - III online exams*/)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "STP_CMN_FindNearestExamCenter3";
                String[] Params = new String[] { "@intPINCode", "@tntExamBodyID" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.TinyInt };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(1, 3, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Pincode, ExamBodyId };
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

        public DataSet ExamCentersForState(System.String ConnectionString, System.Int32 StateId, Boolean IsTbxCenter)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_examcenters";
                String[] Params = new String[] { "@stateid", "@IsTBXCenter" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) , new ParamLength(1,0,0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input , ParameterDirection.Input};
                Object[] Values = new Object[] { StateId , IsTbxCenter ? "Y" : "N" };
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

        public DataSet ExamCentersForState(System.String ConnectionString, System.Int32 StateId, Int32 CenterId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_examcenters2";
                String[] Params = new String[] { "@stateid", "@centerid" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { StateId <= 0 ? (Object)DBNull.Value : StateId, CenterId <= 0 ? (Object)DBNull.Value : CenterId };
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

        public DataSet ExamCentersForStateEx(System.String ConnectionString, System.Int32 StateId, Int32 CenterId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_examcenters5";
                String[] Params = new String[] { "@stateid", "@centerid" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { StateId <= 0 ? (Object)DBNull.Value : StateId, CenterId <= 0 ? (Object)DBNull.Value : CenterId };
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


        public DataSet ExamCentersForDownload(System.String ConnectionString)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_examcenters4";
                String[] Params = new String[] {};
                SqlDbType[] ParamTypes = new SqlDbType[] {};
                ParamLength[] ParamLengths = new ParamLength[] {};
                ParameterDirection[] ParamDirections = new ParameterDirection[] {};
                Object[] Values = new Object[] {};
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

        public DataSet SimilarExamCenters(System.String ConnectionString, Int32 CenterId)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_examcenters3";
                String[] Params = new String[] { "@centerid" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { CenterId };
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

        public String SaveCenterDetails(System.String ConnectionString, System.Int32 CenterId, System.String CenterName, System.String CenterCode, Int32 CSSCode, System.Int32 ExamBody, System.String AddressLine1, System.String AddressLine2, System.String AddressLine3, System.Int32 DistrictId, System.Int32 Pincode, System.Boolean IsActive, String CenterType, System.Int32 CurrentUser)
        {
            Databases.SQLServer.Database objDatabase = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;

            try
	        {
                System.String ProcedureName = "STP_ADM_SaveExamCenterDetails_New";
                String[] Params = new String[] { "@sntExamCenterID", "@varExamCenterName", "@varExamCenterCode", "@tntExamBodyID", "@varHouseNo", "@varStreet", "@varTown", "@sntDistrictID", "@intPinCode", "@btIsActive", "@CurrentUser", "@css_id", "@center_type", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.SmallInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.TinyInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.SmallInt, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar,  SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(2, 5, 0), new ParamLength(256, 0, 0), new ParamLength(5, 0, 0), new ParamLength(1, 3, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), new ParamLength(2, 5, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(20, 0, 0),  new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CenterId, CenterName, CenterCode, ExamBody, AddressLine1 , AddressLine2, AddressLine3, DistrictId, Pincode, IsActive, CurrentUser, CSSCode, CenterType, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[13]);

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
    }
}
