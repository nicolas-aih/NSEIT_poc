﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Databases.SQLServer;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OraDatabases.OracleDB;


namespace IIIDL
{
    public class Scheduling
    {
        public String GetCandidateDetails(String ConnectionString, String URN, out DataSet objDataSet)
        {
            Databases.SQLServer.Database objDatabase = null;
            objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_get_candidate_details_for_booking";
                String[] Params = new String[] { "@URN", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, DBNull.Value };
                objDatabase = new Databases.SQLServer.Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[1]);
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

        public String GetCandidateDetailsRC(String ConnectionString, String URN, out DataSet objDataSet)
        {
            Databases.SQLServer.Database objDatabase = null;
            objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_get_candidate_details_for_booking_rc";
                String[] Params = new String[] { "@URN", "@message" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(255, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Object[] Values = new Object[] { URN, DBNull.Value };
                objDatabase = new Databases.SQLServer.Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                Message = Convert.ToString(AllParameters[1]);
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

        public DataSet GetCandidateDetailsRCB(String ConnectionString, Int32 CenterId, DateTime ExamDateTime)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "sp_get_candidate_details_for_booking_rc_bulk";
                String[] Params = new String[] { "@exam_date", "@center_id" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.DateTime, SqlDbType.Int };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(8, 23, 3), new Databases.SQLServer.ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { ExamDateTime, CenterId };
                objDatabase = new Databases.SQLServer.Database();
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


        //public String GetDatesForCenter(String ConnectionString, Int32 ClientId, Int32 CenterId, Int32 ExamDuration, out DataSet objDataSet)
        //{
        //    OraDatabases.OracleDB.Database objDatabase = null;
        //    objDataSet = null; //Comment this line if the procedure does not return data set.;
        //    Object[] AllParameters = null;
        //    String Message = String.Empty;
        //    try
        //    {
                

        //        System.String ProcedureName = "SP_GET_DATES_FOR_BOOKING";
        //        String[] Params = new String[] { "p_client_id", "p_center_id" , "p_exam_duration", "p_overrideCSS", "p_start_date", "p_scheduling_window", "p_message", "p_curMain"};
        //        OracleDbType[] ParamTypes = new OracleDbType[]{ OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.RefCursor };
        //        //OraDatabases.OracleDB.ParamLength []ParamLengths  = new OraDatabases.OracleDB.ParamLength[] { };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
        //        Object[] Values = new Object[] { ClientId, CenterId, ExamDuration, 0, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value };
        //        objDatabase = new OraDatabases.OracleDB.Database();
        //        //Comment the below line if the procedure does not return data set.
        //        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, /*ParamLengths,*/ ParamDirections, Values, null, out AllParameters, out objDataSet, true);
                 
        //        Message = (AllParameters[6] == DBNull.Value)? String.Empty : Convert.ToString(AllParameters[6]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
        //    //Comment the below line if the procedure does not return data set.
        //    return Message;
        //}

        ///*
        //SP_GET_BATCHES 
        //(
        //    p_client_id     number,
        //    p_center_id     number,
        //    p_preferred_date     date,
        //    p_exam_duration     number,
        //    p_message       out varchar2,
        //    p_curMain   out SYS_REFCURSOR
        //)
        //*/
        //public String GetBatchesForCenterDate(String ConnectionString, Int32 ClientId, DateTime PreferredDate, Int32 CenterId, Int32 ExamDuration, out DataSet objDataSet)
        //{
        //    OraDatabases.OracleDB.Database objDatabase = null;
        //    objDataSet = null; //Comment this line if the procedure does not return data set.;
        //    Object[] AllParameters = null;
        //    String Message = String.Empty;
        //    try
        //    {
        //        System.String ProcedureName = "SP_GET_BATCHES";
        //        String[] Params = new String[] { "p_client_id", "p_center_id", "p_preferred_date", "p_exam_duration", "p_message", "p_curMain" };
        //        OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Date , OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.RefCursor };
        //        OraDatabases.OracleDB.ParamLength[] ParamLengths = new OraDatabases.OracleDB.ParamLength[] { };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
        //        Object[] Values = new Object[] { ClientId, CenterId, PreferredDate, ExamDuration, DBNull.Value, DBNull.Value };
        //        objDatabase = new OraDatabases.OracleDB.Database();
        //        //Comment the below line if the procedure does not return data set.
        //        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, /*ParamLengths,*/ ParamDirections, Values, null, out AllParameters, out objDataSet, true);
        //        Message = (AllParameters[4] == DBNull.Value ) ? String.Empty : Convert.ToString(AllParameters[4]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
        //    //Comment the below line if the procedure does not return data set.
        //    return Message;
        //}

        public DataSet GetScheduledBatchesForCenterDate(String ConnectionString,Int32 Hint, DateTime ExamDate, Int32 CenterId, String Slot)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "SP_GET_SCHEDULED_BATCHES";
                String[] Params = new String[] { "@hint", "@center_id", "@exam_date" ,"@slot"};
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.Date, SqlDbType.VarChar};
                Databases.SQLServer.ParamLength [] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(4,10,0), new Databases.SQLServer.ParamLength(3,10,0), new Databases.SQLServer.ParamLength(20, 0, 0), };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] {Hint, CenterId, ExamDate, Slot};
                objDatabase = new Databases.SQLServer.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
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

        /*
            p_client_id number,
            p_client_side_identifier varchar2,
            p_salutation varchar2,
            p_candidate_name varchar2,
            p_candidate_email varchar2,
            p_candidate_phone  varchar2,
            p_center_id number,
            p_test_date date,
            p_from_time varchar2,
            --    p_till_time number,
            p_exam_duration number,
            p_status  out varchar2,  -- FAIL for failure -- SUCCESS for Success
            p_message out varchar2,
            p_css_reference_number out number        
        */
        //public void BookSeat(String ConnectionString, System.Int32 ClientId, System.String ClientSideIdentifier, System.String Salutation, System.String CandidateName, System.String CandidateEmail,
        //    System.String CandidatePhone, System.Int32 CenterId, System.DateTime TestDate, System.String FromTime, System.Int32 ExamDuration,out System.String Status,out System.String Message,out System.String CSSReferenceNumber)
        //{
        //    OraDatabases.OracleDB.Database objDatabase = null;
        //    Object[] AllParameters = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    CSSReferenceNumber = String.Empty;
        //    try
        //    {
        //        System.String ProcedureName = "SP_BOOK_SEAT";
        //        String[] Params = new String[] { "p_client_id","p_client_side_identifier","p_salutation","p_candidate_name","p_candidate_email","p_candidate_phone", "p_center_id", "p_test_date","p_from_time", "p_exam_duration","p_status", "p_message", "p_css_reference_number", "p_overrideCSS" };
        //        OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Varchar2 , OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2 , OracleDbType.Decimal, OracleDbType.Date , OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Decimal };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Input };
        //        Object[] Values = new Object[] { ClientId, ClientSideIdentifier, Salutation, CandidateName, CandidateEmail, CandidatePhone, CenterId, TestDate, FromTime, ExamDuration, DBNull.Value, DBNull.Value, DBNull.Value , 0};
        //        objDatabase = new OraDatabases.OracleDB.Database();
        //        //Comment the below line if the procedure does not return data set.
        //        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, null, out AllParameters);

        //        Status = (AllParameters[10] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[10]);
        //        Message = (AllParameters[11] == DBNull.Value) ? String.Empty: Convert.ToString(AllParameters[11]);

        //        if (AllParameters[12] == DBNull.Value)
        //        {
        //            CSSReferenceNumber = String.Empty;
        //        }
        //        else
        //        {
        //            OracleDecimal oracleDecimal = (OracleDecimal)AllParameters[12];
        //            CSSReferenceNumber = oracleDecimal.Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
        //}

        /*
        create or replace PROCEDURE "SP_RESCHEDULE_CANDIDATES_NET" 
        (
        p_client_id						number,
        p_client_side_identifier_old	varchar2,--(50 byte)
        p_css_reference_number_old		number,
        p_client_side_identifier_new	varchar2,--(50 byte)
        p_salutation					varchar2,--(10 byte)
        p_candidate_name				varchar2,--(255 byte)
        p_candidate_email				varchar2,--(255 byte)
        p_candidate_phone				varchar2,--(255 byte)
        p_center_id						number,
        p_preferred_test_date			date,
        p_test_duration					number,
        p_start_time					varchar2,--(20 byte)
        -----
        p_remarks                       varchar2,
        p_source                        varchar2,
        p_css_reference_number_new		out number,
        p_status                        out varchar2,
        p_message                       out varchar2
        --p_curMain                       out SYS_REFCURSOR        
        )
        */
        //public DataSet RescheduleSeat(String ConnectionString, System.Int32 ClientId, DataTable objDataTable, String Remarks, String Source, out System.String Status, out System.String Message)
        //{
        //    OraDatabases.OracleDB.Database objDatabase = null;
        //    DataSet objDataSet = null;
        //    Object[] AllParameters = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    try
        //    {
        //        DataRow [] rows = objDataTable.Select();
        //        String [] URNs = Array.ConvertAll(rows, row => Convert.ToString(row["urn"]));
        //        String [] ClientSideIdentifierOld = Array.ConvertAll(rows, row => Convert.ToString(row["client_reference_number"]));
        //        String [] CSSReferenceNumberOld = Array.ConvertAll(rows, row => Convert.ToString(row["css_reference_number"]));
        //        String [] ClientSideIdentifierNew = Array.ConvertAll(rows, row => Convert.ToString(row["client_reference_number"]));
        //        String [] Salutation = Array.ConvertAll(rows, row => Convert.ToString(row["salutation"]));
        //        String [] CandidateName = Array.ConvertAll(rows, row => Convert.ToString(row["applicant_name"]));
        //        String [] CandidateEmail = Array.ConvertAll(rows, row => Convert.ToString(row["email"]));
        //        String [] CandidatePhone = Array.ConvertAll(rows, row => Convert.ToString(row["mobile_no"]));

        //        Int32  [] CenterId = Array.ConvertAll(rows, row => Convert.ToInt32(row["center_id"]));
        //        DateTime [] PreferredTestDate = Array.ConvertAll(rows, row => Convert.ToDateTime(row["preferred_test_date"]));
        //        Int32 [] ExamDuration = Array.ConvertAll(rows, row => Convert.ToInt32(row["exam_duration"]));
        //        String[] FromTime = Array.ConvertAll(rows, row => Convert.ToString(row["from_time"]));

        //        //p_client_side_identifier_old    in typ_client_side_identifier_old,--(50 byte)
        //        //p_css_reference_number_old      in typ_css_reference_number_old,
        //        //p_client_side_identifier_new    in typ_client_side_identifier_new,--(50 byte)
        //        //p_salutation                    in typ_salutation,--(10 byte)
        //        //p_candidate_name                in typ_candidate_name,--(255 byte)
        //        //p_candidate_email               in typ_candidate_email,--(255 byte)
        //        //p_candidate_phone               in typ_candidate_phone,--(255 byte)
        //        //p_center_id                     in typ_center_id,
        //        //p_preferred_test_date           in typ_preferred_test_date,
        //        //p_test_duration                 in typ_test_duration,
        //        //p_start_time                    in typ_start_time,--(20 byte)

        //        System.String ProcedureName = "PKG_RESCHEDULE_CANDIDATE.SP_RESCHEDULE_CANDIDATES_NET2";
        //        String[] Params = new String[] { "p_client_id","p_urn", "p_client_side_identifier_old", "p_css_reference_number_old", "p_client_side_identifier_new", "p_salutation", "p_candidate_name", "p_candidate_email", "p_candidate_phone", "p_center_id", "p_preferred_test_date", "p_test_duration", "p_start_time",  "p_remarks", "p_source", "p_curMain", "p_status",  "p_message"  };
        //        OracleDbType[] ParamTypes = new OracleDbType[] {  OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2  ,OracleDbType.Decimal   ,OracleDbType.Varchar2  ,OracleDbType.Varchar2  ,OracleDbType.Varchar2  ,OracleDbType.Varchar2  ,OracleDbType.Varchar2  ,OracleDbType.Decimal  ,OracleDbType.Date   ,OracleDbType.Decimal   ,OracleDbType.Varchar2  ,OracleDbType.Varchar2  ,OracleDbType.Varchar2  ,OracleDbType.RefCursor   ,OracleDbType.Varchar2  ,OracleDbType.Varchar2 };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
        //        Object[] Values = new Object[] { ClientId,URNs, ClientSideIdentifierOld, CSSReferenceNumberOld, ClientSideIdentifierNew, Salutation, CandidateName, CandidateEmail, CandidatePhone, CenterId, PreferredTestDate, ExamDuration, FromTime, Remarks, Source, DBNull.Value, DBNull.Value, DBNull.Value };
        //        OracleCollectionType[] CollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };

        //        objDatabase = new OraDatabases.OracleDB.Database();
        //        //Comment the below line if the procedure does not return data set.
        //        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType ,  out AllParameters, out objDataSet);

        //        Status = (AllParameters[16] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[16]);
        //        Message = (AllParameters[17] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[17]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
        //    return objDataSet;
        //}

        /*
        procedure sp_cancel_schedule_net2
        (
            p_client_id number,
            p_urn                           in typ_urn, -- varchar2(20)
            p_client_side_identifier_old	in typ_client_side_identifier_old,--(50 byte)
            p_css_reference_number_old		in typ_css_reference_number_old,
            -----
            p_remarks varchar2,
            p_source                        varchar2,
            p_curMain                       out SYS_REFCURSOR,
            p_status                        out varchar2, -- This indicates if error occured in proc execution.For individual row check ref cursor
            p_message                       out varchar2  -- This indicates if error occured in proc execution. For individual row check ref cursor
        );
        */
        //public DataSet CancelSeat(String ConnectionString, System.Int32 ClientId, DataTable objDataTable, String Remarks, String Source, out System.String Status, out System.String Message)
        //{
        //    OraDatabases.OracleDB.Database objDatabase = null;
        //    DataSet objDataSet = null;
        //    Object[] AllParameters = null;
        //    Message = String.Empty;
        //    Status = String.Empty;
        //    try
        //    {
        //        DataRow[] rows = objDataTable.Select();
        //        String[] URNs = Array.ConvertAll(rows, row => Convert.ToString(row["urn"]));
        //        String[] ClientSideIdentifierOld = Array.ConvertAll(rows, row => Convert.ToString(row["client_reference_number"]));
        //        String[] CSSReferenceNumberOld = Array.ConvertAll(rows, row => Convert.ToString(row["css_reference_number"]));
                
        //        System.String ProcedureName = "PKG_RESCHEDULE_CANDIDATE.sp_cancel_schedule_net2";
        //        String[] Params = new String[] { "p_client_id", "p_urn", "p_client_side_identifier_old", "p_css_reference_number_old", "p_remarks", "p_source", "p_curMain", "p_status", "p_message" };
        //        OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal,  OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.RefCursor, OracleDbType.Varchar2, OracleDbType.Varchar2 };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
        //        Object[] Values = new Object[] { ClientId, URNs, ClientSideIdentifierOld, CSSReferenceNumberOld, Remarks, Source, DBNull.Value, DBNull.Value, DBNull.Value };
        //        OracleCollectionType[] CollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };

        //        objDatabase = new OraDatabases.OracleDB.Database();
        //        //Comment the below line if the procedure does not return data set.
        //        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType, out AllParameters, out objDataSet);

        //        Status = (AllParameters[7] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[7]);
        //        Message = (AllParameters[8] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[8]);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
        //    return objDataSet;
        //}


    //    public void RescheduleSeatOld(String ConnectionString, System.Int32 ClientId, System.String ClientSideIdentifierOld, Int64 CSSReferenceNumberOld, String ClientSideIdentifierNew, System.String Salutation, System.String CandidateName, System.String CandidateEmail,
    //System.String CandidatePhone, System.Int32 CenterId, System.DateTime PreferredTestDate, System.Int32 ExamDuration, System.String FromTime, String Remarks, String Source, out System.String CSSReferenceNumberNew, out System.String Status, out System.String Message)
    //    {
    //        OraDatabases.OracleDB.Database objDatabase = null;
    //        Object[] AllParameters = null;
    //        Message = String.Empty;
    //        Status = String.Empty;
    //        CSSReferenceNumberNew = String.Empty;
    //        try
    //        {
    //            System.String ProcedureName = "SP_RESCHEDULE_CANDIDATES_NET";
    //            String[] Params = new String[] { "p_client_id", "p_client_side_identifier_old", "p_css_reference_number_old", "p_client_side_identifier_new", "p_salutation", "p_candidate_name", "p_candidate_email", "p_candidate_phone", "p_center_id", "p_preferred_test_date", "p_test_duration", "p_start_time", "p_remarks", "p_source", "p_css_reference_number_new", "p_status", "p_message" };
    //            OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2 };
    //            ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
    //            Object[] Values = new Object[] { ClientId, ClientSideIdentifierOld, CSSReferenceNumberOld, ClientSideIdentifierNew, Salutation, CandidateName, CandidateEmail, CandidatePhone, CenterId, PreferredTestDate, ExamDuration, FromTime, Remarks, Source, DBNull.Value, DBNull.Value, DBNull.Value };
    //            objDatabase = new OraDatabases.OracleDB.Database();
    //            //Comment the below line if the procedure does not return data set.
    //            objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, null, out AllParameters);


    //            Status = (AllParameters[15] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[15]);
    //            Message = (AllParameters[16] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[16]);

    //            if (AllParameters[14] == DBNull.Value)
    //            {
    //                CSSReferenceNumberNew = String.Empty;
    //            }
    //            else
    //            {
    //                OracleDecimal oracleDecimal = (OracleDecimal)AllParameters[14];
    //                CSSReferenceNumberNew = oracleDecimal.Value.ToString();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw (ex);
    //        }
    //        finally
    //        {
    //            objDatabase = null;
    //        }
    //    }

        public void UpdateBookingStatus(System.String ConnectionString, System.String URN, System.Int64 ClientReferenceNumber, System.String CssReferenceNumber, System.DateTime TestDate, System.String TestTime, System.Int32 CenterId, System.Int32 LanguageId)
        {
	        Databases.SQLServer.Database objDatabase = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
	        try
	        {
		        System.String ProcedureName = "sp_update_booking_status";
                String[] Params = new String[] { "@URN", "@client_reference_number", "@css_reference_number", "@test_Date", "@test_time", "@center_id", "@language_id" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.Date, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(8, 19, 0), new Databases.SQLServer.ParamLength(50, 0, 0), new Databases.SQLServer.ParamLength(3, 10, 0), new Databases.SQLServer.ParamLength(20, 0, 0), new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { URN, ClientReferenceNumber, CssReferenceNumber, TestDate, TestTime, CenterId, LanguageId };
                objDatabase = new Databases.SQLServer.Database();
		        ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
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

        public DataSet UpdateBookingStatusBulk(System.String ConnectionString, DataTable Data)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_update_booking_status_bulk";
                String[] Params = new String[] { "@Data" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(-1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { Data }; objDatabase = new Databases.SQLServer.Database();
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
            return objDataSet;
        }

        //  procedure[dbo].[sp_update_cancellation_status_bulk]
        //  (
        //      @Data TYP_CANCELBOOKING_DATA readonly
        //--@URN varchar(20),
        //--@client_reference_number bigint,
        //--@css_reference_number varchar(50),
        //--@test_Date date,
        //--@test_time varchar(20),
        //--@center_id int,
        //--@language_id int
        //  )
        public DataSet UpdateCancellationStatusBulk(System.String ConnectionString, DataTable Data)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_update_cancellation_status_bulk";
                String[] Params = new String[] { "@Data" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(-1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { Data }; objDatabase = new Databases.SQLServer.Database();
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
            return objDataSet;
        }

        /**********************************************************************************************************************/

        public DataSet GetScheduleReport(System.String ConnectionString,Int32 Hint, System.DateTime FromDate, System.DateTime ToDate)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_scheduled_candidate_list";
                String[] Params = new String[] { "@Hint", "@from_date", "@to_date" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Date, SqlDbType.Date };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(4, 10, 0), new Databases.SQLServer.ParamLength(3, 10, 0), new Databases.SQLServer.ParamLength(3, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Hint, FromDate, ToDate };
                objDatabase = new Databases.SQLServer.Database();
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

        //public DataSet GetReconcileBoookingDetails(System.String  ConnectionString, System.DateTime ReconDate, Int32 ClientId )
        //{
        //    //create or replace procedure sp_get_recon_data
        //    //(
        //    //p_client_id   number,
        //    //p_recon_date date,
        //    //p_curMain     out sys_refcursor
        //    //)
        //    DataSet objDataSet = null;
        //    OraDatabases.OracleDB.Database objDatabase = null;
        //    Object[] AllParameters = null;
        //    try
        //    {
        //        System.String ProcedureName = "sp_get_recon_data";
        //        String[] Params = new String[] { "p_client_id", "p_recon_date", "p_curMain" };
        //        OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Date, OracleDbType.RefCursor };
        //        ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
        //        Object[] Values = new Object[] { ClientId, ReconDate, DBNull.Value };
        //        objDatabase = new OraDatabases.OracleDB.Database();
        //        //Comment the below line if the procedure does not return data set.
        //        objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, null, out AllParameters, out objDataSet, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDatabase = null;
        //    }
          
        //    //Comment the below line if the procedure does not return data set.
        //    return objDataSet;
        //}

        public DataSet ReconcileBoookings(System.String ConnectionString, DataTable objDataTable)
        {
            //Send Data To SQL Server...
            //ALTER procedure[dbo].[sp_reconcile_bookings]
            //(
            //   @data typ_booking_recon_details readonly
            //)                

            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_reconcile_bookings";
                String[] Params = new String[] { "@data" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Structured };
                Databases.SQLServer.ParamLength[] ParamLengths = new Databases.SQLServer.ParamLength[] { new Databases.SQLServer.ParamLength(-1, 0, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { objDataTable };
                objDatabase = new Databases.SQLServer.Database();
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
