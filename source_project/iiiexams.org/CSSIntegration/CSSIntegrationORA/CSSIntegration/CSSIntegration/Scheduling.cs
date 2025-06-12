using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OraDatabases.OracleDB;

namespace CSSIntegration
{
    public class Scheduling
    {
        public String GetDatesForCenter(String ConnectionString, Int32 ClientId, Int32 CenterId, Int32 ExamDuration, out DataSet objDataSet)
        {
            OraDatabases.OracleDB.Database objDatabase = null;
            objDataSet = null; //Comment this line if the procedure does not return data set.;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {


                System.String ProcedureName = "SP_GET_DATES_FOR_BOOKING";
                String[] Params = new String[] { "p_client_id", "p_center_id", "p_exam_duration", "p_overrideCSS", "p_start_date", "p_scheduling_window", "p_message", "p_curMain" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.RefCursor };
                //OraDatabases.OracleDB.ParamLength []ParamLengths  = new OraDatabases.OracleDB.ParamLength[] { };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { ClientId, CenterId, ExamDuration, 0, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value };
                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, /*ParamLengths,*/ ParamDirections, Values, null, out AllParameters, out objDataSet, true);

                Message = (AllParameters[6] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[6]);
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

        /*
        SP_GET_BATCHES 
        (
            p_client_id     number,
            p_center_id     number,
            p_preferred_date     date,
            p_exam_duration     number,
            p_message       out varchar2,
            p_curMain   out SYS_REFCURSOR
        )
        */
        public String GetBatchesForCenterDate(String ConnectionString, Int32 ClientId, DateTime PreferredDate, Int32 CenterId, Int32 ExamDuration, out DataSet objDataSet)
        {
            OraDatabases.OracleDB.Database objDatabase = null;
            objDataSet = null; //Comment this line if the procedure does not return data set.;
            Object[] AllParameters = null;
            String Message = String.Empty;
            try
            {
                System.String ProcedureName = "SP_GET_BATCHES";
                String[] Params = new String[] { "p_client_id", "p_center_id", "p_preferred_date", "p_exam_duration", "p_message", "p_curMain" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.RefCursor };
                OraDatabases.OracleDB.ParamLength[] ParamLengths = new OraDatabases.OracleDB.ParamLength[] { };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { ClientId, CenterId, PreferredDate, ExamDuration, DBNull.Value, DBNull.Value };
                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, /*ParamLengths,*/ ParamDirections, Values, null, out AllParameters, out objDataSet, true);
                Message = (AllParameters[4] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[4]);
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
        public void BookSeat(String ConnectionString, System.Int32 ClientId, System.String ClientSideIdentifier, System.String Salutation, System.String CandidateName, System.String CandidateEmail,
            System.String CandidatePhone, System.Int32 CenterId, System.DateTime TestDate, System.String FromTime, System.Int32 ExamDuration, out System.String Status, out System.String Message, out System.String CSSReferenceNumber)
        {
            OraDatabases.OracleDB.Database objDatabase = null;
            Object[] AllParameters = null;
            Message = String.Empty;
            Status = String.Empty;
            CSSReferenceNumber = String.Empty;
            try
            {
                System.String ProcedureName = "SP_BOOK_SEAT";
                String[] Params = new String[] { "p_client_id", "p_client_side_identifier", "p_salutation", "p_candidate_name", "p_candidate_email", "p_candidate_phone", "p_center_id", "p_test_date", "p_from_time", "p_exam_duration", "p_status", "p_message", "p_css_reference_number", "p_overrideCSS" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Decimal };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Input };
                Object[] Values = new Object[] { ClientId, ClientSideIdentifier, Salutation, CandidateName, CandidateEmail, CandidatePhone, CenterId, TestDate, FromTime, ExamDuration, DBNull.Value, DBNull.Value, DBNull.Value, 0 };
                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, null, out AllParameters);

                Status = (AllParameters[10] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[10]);
                Message = (AllParameters[11] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[11]);

                if (AllParameters[12] == DBNull.Value)
                {
                    CSSReferenceNumber = String.Empty;
                }
                else
                {
                    OracleDecimal oracleDecimal = (OracleDecimal)AllParameters[12];
                    CSSReferenceNumber = oracleDecimal.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
        }

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
        public DataSet RescheduleSeat(String ConnectionString, System.Int32 ClientId, DataTable objDataTable, String Remarks, String Source, out System.String Status, out System.String Message)
        {
            OraDatabases.OracleDB.Database objDatabase = null;
            DataSet objDataSet = null;
            Object[] AllParameters = null;
            Message = String.Empty;
            Status = String.Empty;
            try
            {
                DataRow[] rows = objDataTable.Select();
                String[] URNs = Array.ConvertAll(rows, row => Convert.ToString(row["urn"]));
                String[] ClientSideIdentifierOld = Array.ConvertAll(rows, row => Convert.ToString(row["client_reference_number"]));
                String[] CSSReferenceNumberOld = Array.ConvertAll(rows, row => Convert.ToString(row["css_reference_number"]));
                String[] ClientSideIdentifierNew = Array.ConvertAll(rows, row => Convert.ToString(row["client_reference_number"]));
                String[] Salutation = Array.ConvertAll(rows, row => Convert.ToString(row["salutation"]));
                String[] CandidateName = Array.ConvertAll(rows, row => Convert.ToString(row["applicant_name"]));
                String[] CandidateEmail = Array.ConvertAll(rows, row => Convert.ToString(row["email"]));
                String[] CandidatePhone = Array.ConvertAll(rows, row => Convert.ToString(row["mobile_no"]));

                Int32[] CenterId = Array.ConvertAll(rows, row => Convert.ToInt32(row["center_id"]));
                DateTime[] PreferredTestDate = Array.ConvertAll(rows, row => Convert.ToDateTime(row["preferred_test_date"]));
                Int32[] ExamDuration = Array.ConvertAll(rows, row => Convert.ToInt32(row["exam_duration"]));
                String[] FromTime = Array.ConvertAll(rows, row => Convert.ToString(row["from_time"]));

                //p_client_side_identifier_old    in typ_client_side_identifier_old,--(50 byte)
                //p_css_reference_number_old      in typ_css_reference_number_old,
                //p_client_side_identifier_new    in typ_client_side_identifier_new,--(50 byte)
                //p_salutation                    in typ_salutation,--(10 byte)
                //p_candidate_name                in typ_candidate_name,--(255 byte)
                //p_candidate_email               in typ_candidate_email,--(255 byte)
                //p_candidate_phone               in typ_candidate_phone,--(255 byte)
                //p_center_id                     in typ_center_id,
                //p_preferred_test_date           in typ_preferred_test_date,
                //p_test_duration                 in typ_test_duration,
                //p_start_time                    in typ_start_time,--(20 byte)

                System.String ProcedureName = "PKG_RESCHEDULE_CANDIDATE.SP_RESCHEDULE_CANDIDATES_NET2";
                String[] Params = new String[] { "p_client_id", "p_urn", "p_client_side_identifier_old", "p_css_reference_number_old", "p_client_side_identifier_new", "p_salutation", "p_candidate_name", "p_candidate_email", "p_candidate_phone", "p_center_id", "p_preferred_test_date", "p_test_duration", "p_start_time", "p_remarks", "p_source", "p_curMain", "p_status", "p_message" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.RefCursor, OracleDbType.Varchar2, OracleDbType.Varchar2 };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { ClientId, URNs, ClientSideIdentifierOld, CSSReferenceNumberOld, ClientSideIdentifierNew, Salutation, CandidateName, CandidateEmail, CandidatePhone, CenterId, PreferredTestDate, ExamDuration, FromTime, Remarks, Source, DBNull.Value, DBNull.Value, DBNull.Value };
                OracleCollectionType[] CollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };

                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType, out AllParameters, out objDataSet);

                Status = (AllParameters[16] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[16]);
                Message = (AllParameters[17] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[17]);
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
        public DataSet CancelSeat(String ConnectionString, System.Int32 ClientId, DataTable objDataTable, String Remarks, String Source, out System.String Status, out System.String Message)
        {
            OraDatabases.OracleDB.Database objDatabase = null;
            DataSet objDataSet = null;
            Object[] AllParameters = null;
            Message = String.Empty;
            Status = String.Empty;
            try
            {
                DataRow[] rows = objDataTable.Select();
                String[] URNs = Array.ConvertAll(rows, row => Convert.ToString(row["urn"]));
                String[] ClientSideIdentifierOld = Array.ConvertAll(rows, row => Convert.ToString(row["client_reference_number"]));
                String[] CSSReferenceNumberOld = Array.ConvertAll(rows, row => Convert.ToString(row["css_reference_number"]));

                System.String ProcedureName = "PKG_RESCHEDULE_CANDIDATE.sp_cancel_schedule_net2";
                String[] Params = new String[] { "p_client_id", "p_urn", "p_client_side_identifier_old", "p_css_reference_number_old", "p_remarks", "p_source", "p_curMain", "p_status", "p_message" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Decimal, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.RefCursor, OracleDbType.Varchar2, OracleDbType.Varchar2 };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output };
                Object[] Values = new Object[] { ClientId, URNs, ClientSideIdentifierOld, CSSReferenceNumberOld, Remarks, Source, DBNull.Value, DBNull.Value, DBNull.Value };
                OracleCollectionType[] CollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.PLSQLAssociativeArray, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };

                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType, out AllParameters, out objDataSet);

                Status = (AllParameters[7] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[7]);
                Message = (AllParameters[8] == DBNull.Value) ? String.Empty : Convert.ToString(AllParameters[8]);
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

        public DataSet GetReconcileBoookingDetails(System.String ConnectionString, System.DateTime ReconDate, Int32 ClientId)
        {
            //create or replace procedure sp_get_recon_data
            //(
            //p_client_id   number,
            //p_recon_date date,
            //p_curMain     out sys_refcursor
            //)
            DataSet objDataSet = null;
            OraDatabases.OracleDB.Database objDatabase = null;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_recon_data";
                String[] Params = new String[] { "p_client_id", "p_recon_date", "p_curMain" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Date, OracleDbType.RefCursor };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                OracleCollectionType[] CollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };
                Object[] Values = new Object[] { ClientId, ReconDate, DBNull.Value };
                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType, out AllParameters, out objDataSet, true);
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

        //Currently gets from oaims... Change to get from CSS 
        //create or replace procedure sp_get_dashboard
        //(
        //p_state_id number,
        //p_center_id number,
        //p_from_date date,
        //p_curMain       out SYS_REFCURSOR
        //)
        public DataSet GetDashBoardOAIMS(String ConnectionString, Int32 StateId, Int32 CenterId, DateTime TestDate )
        {
            DataSet objDataSet = null;
            OraDatabases.OracleDB.Database objDatabase = null;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_dashboard";
                String[] Params = new String[] { "p_state_id", "p_center_id", "p_from_date", "p_curMain" };
                OracleDbType[] ParamTypes = new OracleDbType[] { OracleDbType.Decimal, OracleDbType.Decimal, OracleDbType.Date, OracleDbType.RefCursor };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output };
                OracleCollectionType[] CollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };
                Object[] Values = new Object[] { StateId == -1? (Object)DBNull.Value : StateId, CenterId == -1? (Object)DBNull.Value: CenterId, TestDate, DBNull.Value };
                objDatabase = new OraDatabases.OracleDB.Database();
                //Comment the below line if the procedure does not return data set.
                objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType, out AllParameters, out objDataSet, true);
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
