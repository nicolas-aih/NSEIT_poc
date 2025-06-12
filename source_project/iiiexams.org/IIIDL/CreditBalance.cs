using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;
using OraDatabases.OracleDB;

namespace IIIDL
{
    public class CreditBalance
    {
        public String ApproveRejectCreditBalanceEntry(System.String ConnectionString, System.Int64 InstructionId, System.String Status, System.String ApproversRemark, System.Int64 ApprovedBy)
        {
	        Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "sp_ar_credit_balance";
                String[] Params = new String[] { "@instruction_id", "@status", "@approvers_remark", "@approved_by","@Message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(1, 0, 0), new Databases.SQLServer.ParamLength(255, 0, 0), new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(255,0,0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { InstructionId, Status, ApproversRemark, ApprovedBy, DBNull.Value };
                objDatabase = new Databases.SQLServer.Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
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
            //Comment the below line if the procedure does not return data set.
	        return Message;
        }

        /**********************************************************************************************************************/
        public String SaveCreditBalanceEntry(System.String ConnectionString, System.Int64 CompanyCode, System.String InstructionType, System.String InstrumentNo, System.Decimal Amount, System.String ModeOfPayment, System.DateTime DateOfPayment, System.String Remark, System.Int64 CreatedBy, String Narration)
        {
	        Databases.SQLServer.Database objDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "sp_insert_credit_balance";
                String[] Params = new String[] { "@company_code", "@instruction_type", "@instrument_no", "@amount", "@mode_of_payment", "@date_of_payment", "@remark", "@created_by", "@narration", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.VarChar, SqlDbType.Date, SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(1, 0, 0), new Databases.SQLServer.ParamLength(50, 0, 0), new Databases.SQLServer.ParamLength(9, 12, 2), new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(3, 10, 0), new Databases.SQLServer.ParamLength(255, 0, 0), new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(255, 0, 0), new Databases.SQLServer.ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input , ParameterDirection.Input, ParameterDirection.Output};
                Object[] Values = new Object[] { CompanyCode, InstructionType, InstrumentNo, Amount, ModeOfPayment, DateOfPayment, Remark, CreatedBy, Narration , DBNull.Value};
                objDatabase = new Databases.SQLServer.Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Message = Convert.ToString(AllParameters[9]);
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

        /**********************************************************************************************************************/
        public DataSet GetCreditBalanceEntries(System.String ConnectionString, System.Int32 Hint, System.Int64 InstructionId, System.Int32 CompanyCode)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_credit_balance";
                String[] Params = new String[] { "@hint", "@instruction_id", "@company_code" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.BigInt, SqlDbType.Int };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Hint, InstructionId <=0 ? (Object)DBNull.Value : InstructionId, CompanyCode <= 0 ? (Object)DBNull.Value : CompanyCode };
                objDatabase = new Databases.SQLServer.Database();
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

        public DataSet GetLedger(System.String ConnectionString, System.Int64 CompanyCode, System.DateTime FromDate, System.DateTime ToDate)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_ledger";
                String[] Params = new String[] { "@company_code", "@from_date", "@to_date" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.BigInt, SqlDbType.Date, SqlDbType.Date };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(3, 10, 0), new Databases.SQLServer.ParamLength(3, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { CompanyCode, FromDate, ToDate };
                objDatabase = new Databases.SQLServer.Database();
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
        /**********************************************************************************************************************/

        public Decimal GetCurrentBalance(System.String ConnectionString, System.Int32 CompanyCode)
        {
            Databases.SQLServer.Database objDatabase = null;
            System.Decimal Amount = 0M;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_current_balance";
                String[] Params = new String[] { "@company_code", "@amount" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Decimal };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(9, 12, 2) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CompanyCode, DBNull.Value };
                objDatabase = new Databases.SQLServer.Database();
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                Amount = Convert.ToDecimal(AllParameters[1]);
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
	        return Amount;
        }

        public String ValidateCreditMode(System.String ConnectionString, String RoleCode, System.Int32 CompanyCode)
        {
            Databases.SQLServer.Database objDatabase = null;
            System.String HasSubscribedCreditBalance = "N";
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_has_subscribed_credit_balance";
                String[] Params = new String[] { "@companyid", "@role_code", "@HasSubscribedCredit" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { CompanyCode, RoleCode, DBNull.Value };
                objDatabase = new Databases.SQLServer.Database();
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                HasSubscribedCreditBalance = Convert.ToString(AllParameters[2]);
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
            return HasSubscribedCreditBalance;
        }

        public String ValidateCreditMode(System.String ConnectionString, String RoleCode, System.Int32 CompanyCode, out String TopLevelCompanyCode)
        {
            Databases.SQLServer.Database objDatabase = null;
            System.String HasSubscribedCreditBalance = "N";
            TopLevelCompanyCode = String.Empty;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_has_subscribed_credit_balance2";
                String[] Params = new String[] { "@companyid", "@role_code", "@HasSubscribedCredit", "@TopLevelLoginId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(1, 0, 0), new Databases.SQLServer.ParamLength(50, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output , ParameterDirection.Output };
                Object[] Values = new Object[] { CompanyCode, RoleCode, DBNull.Value, DBNull.Value };
                objDatabase = new Databases.SQLServer.Database();
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
                HasSubscribedCreditBalance = Convert.ToString(AllParameters[2]);
                TopLevelCompanyCode = Convert.ToString(AllParameters[3]);
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
            return HasSubscribedCreditBalance;
        }

        public Decimal GetCurrentBalanceOAIMS(System.String ConnectionString, System.String CompanyCode)
        {
            OraDatabases.OracleDB.Database objDatabase = null;
            System.Decimal Amount = 0M;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_current_balance";
                String[] Params = new String[] { "p_IRDA_COMPANY_CODE", "p_BALANCE" };
                Oracle.ManagedDataAccess.Client.OracleDbType []ParamTypes = new Oracle.ManagedDataAccess.Client.OracleDbType[] { Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, Oracle.ManagedDataAccess.Client.OracleDbType.Decimal };
                //OraDatabases.OracleDB.ParamLength[] ParamLengths = new OraDatabases.OracleDB.ParamLength[] { new OraDatabases.OracleDB.ParamLength(4, 10, 0), new OraDatabases.OracleDB.ParamLength(9, 12, 2) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Oracle.ManagedDataAccess.Client.OracleCollectionType[] CollectionType = new Oracle.ManagedDataAccess.Client.OracleCollectionType[] { Oracle.ManagedDataAccess.Client.OracleCollectionType.None, Oracle.ManagedDataAccess.Client.OracleCollectionType.None };


                Object[] Values = new Object[] { CompanyCode, DBNull.Value };
                objDatabase = new OraDatabases.OracleDB.Database();
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType, out AllParameters, true);
                Amount = Convert.ToDecimal(AllParameters[1].ToString());
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
            return Amount;
        }


    }
}
