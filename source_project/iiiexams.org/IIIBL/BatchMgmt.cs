using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIIDL;
using System.Data;

namespace IIIBL
{
    public class BatchMgmt
    {
        public void VerifyBatch(System.String ConnectionString, System.String Transactionid, out System.String PaymentMode, out System.Decimal TotalAmount, out System.String Message, out Boolean CanProceed)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objBatchMgmt.VerifyBatch(ConnectionString, Transactionid, out PaymentMode , out TotalAmount , out Message, out CanProceed);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objBatchMgmt = null;
            }
        }

        public DataSet GetTransactionList(System.String ConnectionString, System.Int32 Hint, System.String TransactionId, System.DateTime dtFromdate, System.DateTime dtTodate,Int32 Status, Int32 intUserID)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objDataSet = objBatchMgmt.GetTransactionList(ConnectionString, Hint, TransactionId, dtFromdate, dtTodate,Status, intUserID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objBatchMgmt = null;
            }
            return objDataSet;
        }

        public DataSet GetBatchDetailsForPayment(System.String ConnectionString, Int32 Hint, System.String TransactionId, out System.String Message)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objDataSet = objBatchMgmt.GetBatchDetailsForPayment(ConnectionString, Hint, TransactionId, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objBatchMgmt = null;
            }
            return objDataSet;
        }

        public DataSet GetBatchDetailsForPayment_PG(System.String ConnectionString, System.String TransactionId, out System.String Message, out Boolean CanProceed)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            Message = String.Empty;
            CanProceed = false;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objDataSet = objBatchMgmt.GetBatchDetailsForPayment_PG(ConnectionString, TransactionId, out Message,out CanProceed);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objBatchMgmt = null;
            }
            return objDataSet;
        }

        public System.String DeleteBatches(System.String ConnectionString, System.Int32 Hint, System.String TransactionId, System.Int32 Userid)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            String Message = String.Empty;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                Message = objBatchMgmt.DeleteBatches(ConnectionString, Hint, TransactionId, Userid);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objBatchMgmt = null;
            }
            return Message;
        }

        public void GetPreviousPaymentAttemptDetails(System.String ConnectionString, System.String ExamBatchNo,
            out String NSEITReferenceNumber, out DateTime PaymentDate , out String PaymentGateway/*, out Int64 PGId*/
            )
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            NSEITReferenceNumber = String.Empty;
            PaymentDate = DateTime.Now;
            PaymentGateway = String.Empty;
            //PGId = -1;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objDataSet = objBatchMgmt.GetPreviousPaymentAttemptDetails(ConnectionString, ExamBatchNo);
                if (objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        NSEITReferenceNumber = Convert.ToString(objDataSet.Tables[0].Rows[0]["nseit_ref_no"]);
                        PaymentDate = Convert.ToDateTime(objDataSet.Tables[0].Rows[0]["payment_date"]);
                        PaymentGateway = Convert.ToString(objDataSet.Tables[0].Rows[0]["pg_identifier"]);
                        //PGId = Convert.ToInt64(objDataSet.Tables[0].Rows[0]["pg_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDataTable = null;
                objBatchMgmt = null;
                objDataSet = null;
            }
        }

        public void UpdatePaymentAttempt(System.String ConnectionString, System.String ExamBatchNo,  String PGIdentifier, out String NSEITReferenceNumber, out DateTime PaymentDate/*, out Int64 PGId*/)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            NSEITReferenceNumber = String.Empty;
            PaymentDate = DateTime.Now;
            //PGId = 0;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objDataSet = objBatchMgmt.UpdatePaymentAttempt(ConnectionString, ExamBatchNo, PGIdentifier);
                if (objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        NSEITReferenceNumber = Convert.ToString(objDataSet.Tables[0].Rows[0]["nseit_ref_no"]);
                        PaymentDate = Convert.ToDateTime(objDataSet.Tables[0].Rows[0]["payment_date"]);
                        //PGId = Convert.ToInt64(objDataSet.Tables[0].Rows[0]["pg_id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDataTable = null;
                objBatchMgmt = null;
                objDataSet = null;
            }
        }

        public DataSet GetCenterWisePendingScheduleCount(System.String ConnectionString)
        {
            IIIDL.BatchMgmt objBatchMgmt = null;
            DataSet objDataSet = null;
            try
            {
                objBatchMgmt = new IIIDL.BatchMgmt();
                objDataSet = objBatchMgmt.GetCenterWisePendingScheduleCount(ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objBatchMgmt = null;
            }
            return objDataSet;
        }
    }
}
