using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;


namespace IIIBL
{
    public class Scheduling
    {
        public String GetCandidateDetails(String ConnectionString, String URN, out DataSet objDataSet)
        {
            IIIDL.Scheduling objScheduling = null;
            String Message = String.Empty;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                Message = objScheduling.GetCandidateDetails(ConnectionString, URN, out objDataSet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return Message;
        }

        public String GetCandidateDetailsRC(String ConnectionString, String URN, out DataSet objDataSet)
        {
            IIIDL.Scheduling objScheduling = null;
            String Message = String.Empty;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                Message = objScheduling.GetCandidateDetailsRC(ConnectionString, URN, out objDataSet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return Message;
        }

        public DataSet GetCandidateDetailsRCB(String ConnectionString, Int32 CenterId, DateTime ExamDateTime)
        {
            IIIDL.Scheduling objScheduling = null;
            //String Message = String.Empty;
            DataSet objDataSet = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objDataSet = objScheduling.GetCandidateDetailsRCB(ConnectionString, CenterId, ExamDateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return objDataSet;
        }


        //public String GetDatesForCenter(String ConnectionString, Int32 ClientId, Int32 CenterId, Int32 ExamDuration, out DataSet objDataSet)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    String Message = String.Empty;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        Message = objScheduling.GetDatesForCenter(ConnectionString, ClientId, CenterId, ExamDuration,out objDataSet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //    return Message;
        //}

        //public String GetBatchesForCenterDate(String ConnectionString, Int32 ClientId, DateTime PreferredDate, Int32 CenterId, Int32 ExamDuration, out DataSet objDataSet)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    String Message = String.Empty;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        Message = objScheduling.GetBatchesForCenterDate(ConnectionString, ClientId, PreferredDate, CenterId, ExamDuration , out objDataSet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //    return Message;
        //}

        public DataSet GetScheduledBatchesForCenterDate(String ConnectionString, Int32 Hint, DateTime ExamDate, Int32 CenterId, String Slot)
        {
            IIIDL.Scheduling objScheduling = null;
            String Message = String.Empty;
            DataSet objDataSet = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objDataSet = objScheduling.GetScheduledBatchesForCenterDate(ConnectionString,Hint, ExamDate, CenterId, Slot);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return objDataSet;
        }

        //public void BookSeat(String ConnectionString,  String URN, System.Int32 ClientId, Int64 ClientSideIdentifier, String Salutation, String CandidateName, String CandidateEmail, String CandidatePhone, System.Int32 CSSCenterId, System.DateTime TestDate, System.String FromTime, System.Int32 ExamDuration, out System.String Status, out System.String Message, out System.String CSSReferenceNumber)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    CSSReferenceNumber = String.Empty;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        objScheduling.BookSeat(ConnectionString, ClientId, Convert.ToString(ClientSideIdentifier), Salutation, CandidateName, CandidateEmail, CandidatePhone, CSSCenterId, TestDate, FromTime, ExamDuration, out Status, out Message, out CSSReferenceNumber );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //}

        //public DataSet Reschedule(String ConnectionString, System.Int32 ClientId, DataTable objDataTable, String Remarks, String Source, out System.String Status, out System.String Message)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    DataSet objDataSet = null;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        objDataSet = objScheduling.RescheduleSeat(ConnectionString, ClientId, objDataTable, Remarks, Source, out Status, out Message );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //    return objDataSet;
        //}
        ///*
        //public void RescheduleOld(String ConnectionString, String URN, System.Int32 ClientId, Int64 ClientSideIdentifierOld, Int64 CSSReferenceNumberOld, Int64 ClientSideIdentifierNew, String Salutation, String CandidateName, String CandidateEmail, String CandidatePhone, System.Int32 CSSCenterId, System.DateTime TestDate, System.String FromTime, System.Int32 ExamDuration, String Remarks, String Source, out System.String CSSReferenceNumberNew, out System.String Status, out System.String Message)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    CSSReferenceNumberNew = String.Empty;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        objScheduling.RescheduleSeat(ConnectionString, ClientId, Convert.ToString(ClientSideIdentifierOld), CSSReferenceNumberOld, Convert.ToString(ClientSideIdentifierNew), Salutation, CandidateName, CandidateEmail, CandidatePhone, CSSCenterId, TestDate, ExamDuration, FromTime, Remarks, Source, out CSSReferenceNumberNew, out Status, out Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //}*/

        //public DataSet CancelSeat(String ConnectionString, System.Int32 ClientId, DataTable objDataTable, String Remarks, String Source, out System.String Status, out System.String Message)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    DataSet objDataSet = null;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        objDataSet = objScheduling.CancelSeat(ConnectionString, ClientId, objDataTable, Remarks, Source, out Status, out Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //    return objDataSet;
        //}

        public void UpdateBookingStatus (String ConnectionString, String URN, Int64 ClientSideIdentifier, String CssReferenceNumber, System.DateTime TestDate, System.String FromTime, System.Int32 CSSCenterId,Int32 LanguageId)
        {
            IIIDL.Scheduling objScheduling = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objScheduling.UpdateBookingStatus(ConnectionString, URN, ClientSideIdentifier, CssReferenceNumber, TestDate, FromTime, CSSCenterId, LanguageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
        }

        public DataSet UpdateBookingStatusBulk(String ConnectionString, DataTable Data)
        {
            IIIDL.Scheduling objScheduling = null;
            DataSet objDataSet = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objDataSet = objScheduling.UpdateBookingStatusBulk(ConnectionString, Data );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return objDataSet;
        }

        public DataSet UpdateCancellationStatusBulk(String ConnectionString, DataTable Data)
        {
            IIIDL.Scheduling objScheduling = null;
            DataSet objDataSet = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objDataSet = objScheduling.UpdateCancellationStatusBulk(ConnectionString, Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return objDataSet;
        }


        public DataSet GetScheduleReport(System.String ConnectionString, Int32 Hint, DateTime FromDate, DateTime ToDate)
        {
            IIIDL.Scheduling objScheduling = null;
            DataSet objDataSet = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objDataSet = objScheduling.GetScheduleReport(ConnectionString,Hint, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return objDataSet;
        }

        //public DataSet GetReconcileBoookingDetails(System.String ConnectionString, System.DateTime ReconDate, Int32 ClientId)
        //{
        //    IIIDL.Scheduling objScheduling = null;
        //    DataSet objDataSet = null;
        //    try
        //    {
        //        objScheduling = new IIIDL.Scheduling();
        //        objDataSet = objScheduling.GetReconcileBoookingDetails(ConnectionString, ReconDate, ClientId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objScheduling = null;
        //    }
        //    return objDataSet;

        //}

        public DataSet ReconcileBoookings(System.String ConnectionString, DataTable objDataTable)
        {
            IIIDL.Scheduling objScheduling = null;
            DataSet objDataSet = null;
            try
            {
                objScheduling = new IIIDL.Scheduling();
                objDataSet = objScheduling.ReconcileBoookings(ConnectionString, objDataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objScheduling = null;
            }
            return objDataSet;
        }
    }
}
