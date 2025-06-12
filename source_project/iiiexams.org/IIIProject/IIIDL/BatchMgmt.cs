using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;

namespace IIIDL
{
    public class BatchMgmt
    {
        public void VerifyBatch (System.String ConnectionString, System.String Transactionid, out System.String PaymentMode, out System.Decimal TotalAmount, out System.String Message, out Boolean CanProceed )
        {
            Databases.SQLServer.Database objDatabase = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            PaymentMode = String.Empty;
            TotalAmount = 0;
            Message = String.Empty;
	        try
	        {
		        System.String ProcedureName = "SP_VerifyBatch";
                String[] Params = new String[] { "@TransactionId", "@payment_mode_o", "@total_amount_o", "@Message", "@CanProceed" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(50, 0, 0), new ParamLength(2, 0, 0), new ParamLength(9, 12, 2), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { Transactionid, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure return data set.
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);

                PaymentMode = Convert.ToString(AllParameters[1]);
                TotalAmount = Convert.ToDecimal(AllParameters[2]);
                Message = Convert.ToString(AllParameters[3]);
                CanProceed = (Convert.ToString(AllParameters[4]).ToUpper() == "Y");
            }
            catch (Exception ex)
	        {
		        throw(ex);
	        }
	        finally
	        {
		        objDatabase = null;
	        }
        }

        public System.String DeleteBatches(System.String ConnectionString, System.Int32 Hint, System.String TransactionId, System.Int32 Userid)
        {
            Databases.SQLServer.Database objDatabase = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            System.String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_delete_batch";
                String[] Params = new String[] { "@Hint", "@transaction_id", "@UserId", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(50, 0, 0), new ParamLength(4, 10, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Hint, TransactionId, Userid, DBNull.Value };
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
            return Message;
        }

        public DataSet GetTransactionList(System.String ConnectionString, System.Int32 Hint, System.String TransactionId, System.DateTime dtFromdate, System.DateTime dtTodate, Int32 Status, Int32 intUserID)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Object[] Values = null;
            try
            {
                System.String ProcedureName = "sp_get_transaction_list";
                String[] Params = new String[] { "@Hint", "@transaction_id", "@dtFromDate", "@dtToDate", "@Status", "@intUserId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Date, SqlDbType.Date, SqlDbType.Int, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(100, 0, 0), new ParamLength(3, 10, 0), new ParamLength(3, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                if (Hint == 1)
                {
                    Values = new Object[] { Hint, TransactionId, DBNull.Value, DBNull.Value,DBNull.Value, intUserID };
                }
                if (Hint == 2)
                {
                    Values = new Object[] { Hint, String.Empty, dtFromdate, dtTodate, Status, intUserID };
                }

                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
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

        public DataSet GetBatchDetailsForPayment(System.String ConnectionString, Int32 Hint, System.String TransactionId, out System.String Message)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_get_transaction_details";
                String[] Params = new String[] { "@Hint", "@transaction_id", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 0, 0), new ParamLength(100, 0, 0), new ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { Hint, TransactionId, DBNull.Value };
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

        public DataSet GetBatchDetailsForPayment_PG(System.String ConnectionString, System.String TransactionId, out System.String Message, out System.Boolean CanProceed)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_get_transaction_details_PG";
                String[] Params = new String[] { "@transaction_id", "@message", "@CanProceed" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { TransactionId, DBNull.Value, DBNull.Value };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[1]);
                CanProceed = (Convert.ToString(AllParameters[2]).ToUpper() == "Y");
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

        public DataSet GetPreviousPaymentAttemptDetails(System.String ConnectionString, System.String ExamBatchNo)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_get_prev_nseit_ref_no";
                String[] Params = new String[] { "@exam_batch_no" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { ExamBatchNo };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
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
    
        public DataSet UpdatePaymentAttempt(System.String ConnectionString, System.String ExamBatchNo, String PGIdentifier)
        {
	        Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_update_new_pg_attempt";
                String[] Params = new String[] { "@exam_batch_no" , "@pg_identifier" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(100, 0, 0), new ParamLength(20, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { ExamBatchNo , PGIdentifier };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
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

        public DataSet GetCenterWisePendingScheduleCount(System.String ConnectionString)
        {//sp_get_pending_scheduling_count
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_pending_scheduling_count";
                String[] Params = new String[] { };
                SqlDbType[] ParamTypes = new SqlDbType[] { };
                ParamLength[] ParamLengths = new ParamLength[] { };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { };
                Object[] Values = new Object[] { };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
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
