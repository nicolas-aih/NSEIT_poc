using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;

namespace IIIDL
{
    public class Telemarketer
    {
        public String RegisterTelemarketer(String ConnectionString, Int32 InsurerUserId, Int32 CreatedByUserId, String TraiRegNo, String Name, String Address, String CpName, String CpEmailId, String CpContactNo, String DpName, String DpEmailId, String DpContactNo, String IsActive)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_insert_telemarketeer";
                String[] Params = new String[] { "@tm_name", "@tm_trai_reg_no", "@tm_address", "@tm_contact_person_name", "@tm_cp_email_id", "@tm_cp_contact_no", "@tm_designated_person_name", "@tm_dp_email_id", "@tm_dp_contact_no", "@tm_created_by", "@tm_insurer_user_id", "@is_active", "@error" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(1, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Name, TraiRegNo, Address, CpName, CpEmailId, CpContactNo, DpName, DpEmailId, DpContactNo, CreatedByUserId, InsurerUserId, IsActive, Message };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[12]);
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

        public String UpdateTelemarketer(String ConnectionString, Int32 InsurerUserId, Int32 CreatedByUserId, Int64 TmId, String TraiRegNo, String Name, String Address, String CpName, String CpEmailId, String CpContactNo, String DpName, String DpEmailId, String DpContactNo, String IsActive)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_update_telemarketeer";
                String[] Params = new String[] { "@tm_id", "@tm_name", "@tm_trai_reg_no", "@tm_address", "@tm_contact_person_name", "@tm_cp_email_id", "@tm_cp_contact_no", "@tm_designated_person_name", "@tm_dp_email_id", "@tm_dp_contact_no", "@tm_created_by", "@tm_insurer_user_id", "@is_active", "@error" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 19, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(255, 0, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(1, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { TmId, Name, TraiRegNo, Address, CpName, CpEmailId, CpContactNo, DpName, DpEmailId, DpContactNo, CreatedByUserId, InsurerUserId, IsActive, Message };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[13]);
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
        public String DeleteTelemarketer(String ConnectionString, Int32 InsurerUserId, Int32 CreatedByUserId, Int64 TmId)
        {
            Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_delete_telemarketeer";
                String[] Params = new String[] { "@tm_created_by", "@tm_insurer_user_id", "@tm_id", "@error" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.BigInt, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(8, 19, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CreatedByUserId, InsurerUserId, TmId, Message };
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
            //return objDataSet;
            return Message;
        }

        public DataSet GetTelemarketerData(System.String ConnectionString, System.Int32 InsurerUserId, System.Int64 TmId, System.Int32 IsActive)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_telemarketeer";
                String[] Params = new String[] { "@tm_insurer_user_id", "@tm_id", "@active" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(8, 19, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { InsurerUserId, TmId < 0 ? (Object)DBNull.Value : TmId, IsActive };
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
