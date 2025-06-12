using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Globalization;
using System.Reflection;
using CSSIntegration;


namespace IIIRegistrationPortal2.Controllers
{
    public class SchedulerController : Controller
    {
        [HttpGet]
        public ActionResult BookSeat()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCandidateDetails()
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objTableCandidateDetails = null;
            DataTable objTableExamCenters = null;
            DataTable objTableLanguages = null;

            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            String URN = String.Empty;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]);

                objScheduler = new IIIBL.Scheduling();
                Message = objScheduler.GetCandidateDetails(PortalApplication.ConnectionString, URN, out objDataSet);
                if (Message == String.Empty)
                {
                    if (objDataSet != null && objDataSet.Tables.Count == 3)
                    {
                        objTableCandidateDetails    = objDataSet.Tables[0];
                        objTableExamCenters         = objDataSet.Tables[1];
                        objTableLanguages           = objDataSet.Tables[2];
                        if (objTableCandidateDetails.Rows.Count > 0 && objTableExamCenters.Rows.Count > 0 && objTableLanguages.Rows.Count > 0 )
                        {
                            Success = true;
                            Message = String.Empty;
                            s = HelperUtilities.ToJSON2(Success, Message, new DataTable[] { objTableCandidateDetails, objTableExamCenters, objTableLanguages });
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.ERROR_OCCURED;
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.ERROR_OCCURED;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                else
                {
                    Success = false;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objTableCandidateDetails = null;
                objTableExamCenters = null;
                objTableLanguages = null;
                objDataSet = null;
                objScheduler = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetCandidateDetailsRC()
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objTableCandidateDetails = null;
            DataTable objTableExamCenters = null;
            DataTable objTableLanguages = null;

            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            String URN = String.Empty;
            try
            {
                URN = Convert.ToString(Request.Form["txtURN"]);

                objScheduler = new IIIBL.Scheduling();
                Message = objScheduler.GetCandidateDetailsRC(PortalApplication.ConnectionString, URN, out objDataSet);
                if (Message == String.Empty)
                {
                    if (objDataSet != null && objDataSet.Tables.Count == 3)
                    {
                        objTableCandidateDetails = objDataSet.Tables[0];
                        objTableExamCenters = objDataSet.Tables[1];
                        objTableLanguages = objDataSet.Tables[2];
                        if (objTableCandidateDetails.Rows.Count > 0 && objTableExamCenters.Rows.Count > 0 && objTableLanguages.Rows.Count > 0)
                        {
                            Success = true;
                            Message = String.Empty;
                            s = HelperUtilities.ToJSON2(Success, Message, new DataTable[] { objTableCandidateDetails, objTableExamCenters, objTableLanguages });
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.ERROR_OCCURED;
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.ERROR_OCCURED;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                else
                {
                    Success = false;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objTableCandidateDetails = null;
                objTableExamCenters = null;
                objTableLanguages = null;
                objDataSet = null;
                objScheduler = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public JsonResult GetDatesForCenter()
        {
            JsonResult objJsonResult = null;
            CSSIntegration.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty;

            Int32 CenterId = 0;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid cboCategory");
                }
                else
                {
                    CenterId = Convert.ToInt32(_dummy);
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new CSSIntegration.Scheduling();
                    Message = objScheduler.GetDatesForCenter(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, CenterId, PortalApplication.ExamDuration, out objDataSet);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count == 1)
                        {
                            objDataTable = objDataSet.Tables[0];
                            if (objDataTable.Rows.Count > 0)
                            {
                                Success = true;
                                Message = String.Empty;
                                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                            }
                            else
                            {
                                Success = false;
                                Message = "No dates found the selected center";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.ERROR_OCCURED;
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objScheduler = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        public JsonResult GetBatchesForCenterDate()
        {
            JsonResult objJsonResult = null;
            CSSIntegration.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty;

            Int32 CenterId = 0;
            DateTime PreferredDate = DateTime.Now;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid center_id");
                }
                else
                {
                    CenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["preferred_date"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred_date");
                }
                else
                {
                    PreferredDate = Convert.ToDateTime(_dummy);
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new CSSIntegration.Scheduling();
                    Message = objScheduler.GetBatchesForCenterDate(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, PreferredDate , CenterId, PortalApplication.ExamDuration, out objDataSet);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count == 1)
                        {
                            objDataTable = objDataSet.Tables[0];
                            if (objDataTable.Rows.Count > 0)
                            {
                                Success = true;
                                Message = String.Empty;
                                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                            }
                            else
                            {
                                Success = false;
                                Message = "No batches found the selected center & date";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = CommonMessages.ERROR_OCCURED;
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objScheduler = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetScheduledBatchesForCenterDate()
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty;

            Int32 CenterId = 0;
            DateTime PreferredDate = DateTime.Now;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid center_id");
                }
                else
                {
                    CenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["preferred_date"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred_date");
                }
                else
                {
                    PreferredDate = Convert.ToDateTime(_dummy);
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new IIIBL.Scheduling();
                    objDataSet = objScheduler.GetScheduledBatchesForCenterDate(PortalApplication.ConnectionString,1, PreferredDate, CenterId, String.Empty);
                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            Success = true;
                            Message = String.Empty;
                            s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                        }
                        else
                        {
                            Success = false;
                            Message = "No dates found the selected center and date";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.ERROR_OCCURED;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objScheduler = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetScheduledBatchCountForCenterDate()
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty;

            Int32 CenterId = 0;
            DateTime PreferredDate = DateTime.Now;
            String Slot = String.Empty;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid center_id");
                }
                else
                {
                    CenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["preferred_date"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred_date");
                }
                else
                {
                    PreferredDate = Convert.ToDateTime(_dummy);
                }

                Slot = Convert.ToString(Request.Form["slot"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new IIIBL.Scheduling();
                    objDataSet = objScheduler.GetScheduledBatchesForCenterDate(PortalApplication.ConnectionString, 2, PreferredDate, CenterId, Slot);
                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            Success = true;
                            Message = String.Empty;
                            s = HelperUtilities.ToJSON(Success, Message, objDataTable);
                        }
                        else
                        {
                            Success = false;
                            Message = "No dates found the selected center and date";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.ERROR_OCCURED;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Success = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objScheduler = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpPost]
        public JsonResult BookSeat(String dummy="")
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objSchedulerLocal = null;
            CSSIntegration.Scheduling objSchedulerCSS = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            //From the form
            String URN = String.Empty;
            Int32 CSSCenterId = 0;
            DateTime TestDate = DateTime.Now;
            String FromTime = String.Empty;
            Int32 LanguageId = 0;
            // To be populated within.
            Int64 ClientSideIdentifier = 0;
            String Salutation = String.Empty;
            String CandidateName = String.Empty;
            String CandidateEmail = String.Empty;
            String CandidatePhone = String.Empty;
            //
            String CssReferenceNumber = String.Empty;
            DataTable objTableCandidateDetails = null;
            try
            {
                String _dummy = String.Empty;

                URN = Convert.ToString(Request.Form["urn"]);

                _dummy = Convert.ToString(Request.Form["language_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid language_id");
                }
                else
                {
                    LanguageId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid center_id");
                }
                else
                {
                    CSSCenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["preferred_date"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred_date");
                }
                else
                {
                    TestDate = Convert.ToDateTime(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["start_time"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid start_time");
                }
                else
                {
                    FromTime = GetTimeString(Convert.ToInt32(_dummy)); //additional validations required.
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objSchedulerLocal = new IIIBL.Scheduling();
                    objSchedulerCSS = new CSSIntegration.Scheduling();

                    Message = objSchedulerLocal.GetCandidateDetails(PortalApplication.ConnectionString, URN, out objDataSet);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count >= 1)
                        {
                            objTableCandidateDetails = objDataSet.Tables[0];
                            if (objTableCandidateDetails.Rows.Count > 0 )
                            {
                                DataRow dr = objTableCandidateDetails.Rows[0];
                                ClientSideIdentifier = Convert.ToInt64(dr["client_reference_number"]);
                                Salutation = Convert.ToString(dr["salutation"]);
                                CandidateName = Convert.ToString(dr["applicant_name"]);
                                CandidateEmail = Convert.ToString(dr["email"]);
                                CandidatePhone = Convert.ToString(dr["mobile_no"]);

                                objSchedulerCSS.BookSeat(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, Convert.ToString( ClientSideIdentifier), Salutation, CandidateName, CandidateEmail, CandidatePhone, CSSCenterId, TestDate, FromTime, PortalApplication.ExamDuration, out Status, out Message, out CssReferenceNumber);
                                //Log -- 
                                if (Status == "SUCCESS")
                                {
                                    objSchedulerLocal.UpdateBookingStatus(PortalApplication.ConnectionString, URN, ClientSideIdentifier, CssReferenceNumber, TestDate, FromTime, CSSCenterId, LanguageId);
                                    Success = true;
                                    Message = "Seat Booked Successfully";
                                    s = HelperUtilities.ToJSON(Success, Message);

                                }
                                else
                                {
                                    Success = false;
                                    switch (Message)
                                    {
                                        case "ERROR_OCCURED":
                                            Message = "Error occured while booking the seat.\nTry again after sometime.\nIf the problem persists, contact technical support team.";
                                            break;
                                        case "INVALID_CLIENT":
                                            Message = "Invalid client id.\nPlease contact technical support team";
                                            break;
                                        case "DUPLICATE_REFERENCE_NUMBER_FOR_CLIENT":
                                            Message = "Duplicate client reference number.\nPlease contact technical support team";
                                            break;
                                        case "NO_SUCH_BATCH":
                                            Message = "Invalid batch.\nTry again after some time. If the problem persists, contact technical support team";
                                            break;
                                        case "SEAT_NOT_BOOKED":
                                            Message = "Seat booking failed.\nTry again after some time. If the problem persists, contact technical support team";
                                            break;
                                        default:
                                            Message = Status + " - " + Message + "\nUnknown Error Occured.\nTry again after sometime.\nIf the problem persists, contact technical support team.";
                                            break;
                                    }
                                    s = HelperUtilities.ToJSON(Success, Message);
                                }
                            }
                            else
                            {
                                Success = false;
                                Message = "Unable to fetch candidate data. Please contact helpdesk.";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = "Unable to fetch candidate data. Please contact helpdesk.";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch(Exception ex)
                {
                    Success = false;
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objSchedulerLocal = null;
                    objSchedulerCSS = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpGet]//Single Candidate
        [AuthorizeExt]
        public ActionResult RC()
        {
            return View();
        }

        [HttpPost]//Single Candidate
        [AuthorizeAJAX]
        public ActionResult Reschedule(String dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objSchedulerLocal = null;
            CSSIntegration.Scheduling objSchedulerCSS = null;

            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            //From the form
            String URN = String.Empty;
            Int32 CSSCenterId = 0;
            DateTime TestDate = DateTime.Now;
            String FromTime = String.Empty;
            Int32 LanguageId = 0;
            // To be populated within.
            Int64 ClientSideIdentifier = 0;
            String Salutation = String.Empty;
            String CandidateName = String.Empty;
            String CandidateEmail = String.Empty;
            String CandidatePhone = String.Empty;
            //
            String CssReferenceNumberOld = String.Empty;
            String CssReferenceNumberNew = String.Empty;
            String Remarks = String.Empty;
            DataTable objTableCandidateDetails = null;
            DataSet objReschedulingDetails = null; 
            try
            {
                String _dummy = String.Empty;

                URN = Convert.ToString(Request.Form["urn"]);

                //_dummy = Convert.ToString(Request.Form["language_id"]);
                //if (_dummy.Trim() == String.Empty || _dummy == null)
                //{
                //    throw new Exception("Invalid language_id");
                //}
                //else
                //{
                //    LanguageId = Convert.ToInt32(_dummy);
                //}

                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid center_id");
                }
                else
                {
                    CSSCenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["preferred_date"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred_date");
                }
                else
                {
                    TestDate = Convert.ToDateTime(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["start_time"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid start_time");
                }
                else
                {
                    FromTime = GetTimeString(Convert.ToInt32(_dummy)); //additional validations required.
                }

                Remarks = Convert.ToString(Request.Form["remarks"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objSchedulerLocal = new IIIBL.Scheduling();
                    objSchedulerCSS = new CSSIntegration.Scheduling();

                    Message = objSchedulerLocal.GetCandidateDetailsRC(PortalApplication.ConnectionString, URN, out objDataSet);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count >= 1)
                        {
                            objTableCandidateDetails = objDataSet.Tables[0];
                            if (objTableCandidateDetails.Rows.Count > 0)
                            {
                                //Add 4 columns:
                                objTableCandidateDetails.Columns.Add("center_id", typeof(Int32));
                                objTableCandidateDetails.Columns.Add("preferred_test_date", typeof(DateTime));
                                objTableCandidateDetails.Columns.Add("exam_duration", typeof(Int32));
                                objTableCandidateDetails.Columns.Add("from_time", typeof(String));
                                foreach(DataRow dr in objTableCandidateDetails.Rows)
                                {
                                    dr["center_id"] = CSSCenterId;
                                    dr["preferred_test_date"] = TestDate;
                                    dr["exam_duration"] = PortalApplication.ExamDuration ;
                                    dr["from_time"] = FromTime;
                                }

                                objReschedulingDetails = objSchedulerCSS.RescheduleSeat(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, objTableCandidateDetails, Remarks, "NSEIT-IIIExams.org-Rescheduler - " + PortalSession.UserLoginID, out Status, out Message);

                                if (Status == "FAIL")
                                {
                                    Success = false;
                                    //Message = Message;
                                }
                                else if (Status == "SUCCESS")
                                {
                                    if (objReschedulingDetails == null)
                                    {
                                        Success = false;
                                        Message = "Unable to receive rescheduling data - 1. Please contact helpdesk";
                                        s = HelperUtilities.ToJSON(Success, Message);
                                    }
                                    else
                                    {
                                        if (objReschedulingDetails.Tables.Count == 0)
                                        {
                                            Success = false;
                                            Message = "Unable to receive rescheduling data - 2. Please contact helpdesk";
                                            s = HelperUtilities.ToJSON(Success, Message);
                                        }
                                        else
                                        {
                                            DataTable dataTable = objReschedulingDetails.Tables[0];
                                            dataTable.Columns.Remove("SALUTATION");
                                            dataTable.Columns.Remove("CANDIDATE_NAME");
                                            dataTable.Columns.Remove("CANDIDATE_EMAIL");
                                            dataTable.Columns.Remove("CANDIDATE_PHONE");

                                            objReschedulingDetails = objSchedulerLocal.UpdateBookingStatusBulk(PortalApplication.ConnectionString, objReschedulingDetails.Tables[0]);

                                            if (objReschedulingDetails == null)
                                            {
                                                Success = false;
                                                Message = "Unable to receive rescheduling data (3). Please contact helpdesk";
                                                s = HelperUtilities.ToJSON(Success, Message);
                                            }
                                            else
                                            {
                                                if (objReschedulingDetails.Tables.Count == 0)
                                                {
                                                    Success = false;
                                                    Message = "Unable to receive rescheduling data (4). Please contact helpdesk";
                                                    s = HelperUtilities.ToJSON(Success, Message);
                                                }
                                                else
                                                {
                                                    //return resultset back to JSON.
                                                    /*
                                                    urn,
                                                    client_id,
                                                    client_reference_number_old,
                                                    css_reference_number_old,
                                                    client_reference_number_new,
                                                    css_reference_number_new,
                                                    center_id,
                                                    test_date,
                                                    test_duration,
                                                    test_time,
                                                    status,
                                                    reason,
                                                    iii_status,
                                                    iii_reason,
                                                    sntExamCenterId
                                                    */
                                                    Message = "Process completed. Please check the status below for individual URNs";
                                                    s = HelperUtilities.ToJSON(Success, Message, objReschedulingDetails.Tables[0]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Success = false;
                                Message = "Unable to fetch candidate data. Please contact helpdesk.";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = "Unable to fetch candidate data. Please contact helpdesk.";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Success = false;
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objSchedulerLocal = null;
                    objSchedulerCSS = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult RCB()
        {
            return View();
        }

        [HttpPost]//Single Candidate
        [AuthorizeAJAX]
        public ActionResult RescheduleBatch(String dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objSchedulerLocal = null;
            CSSIntegration.Scheduling objSchedulerCSS = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            //From the form
            Int32 CurrentCenter = 0;
            Int32 CSSCenterId = 0;

            DateTime CurrentDate = DateTime.Now;
            DateTime PreferredDate = DateTime.Now;

            String CurrentSlot = String.Empty;
            String PreferredSlot = String.Empty;

            // To be populated within.
            String Salutation = String.Empty;
            String CandidateName = String.Empty;
            String CandidateEmail = String.Empty;
            String CandidatePhone = String.Empty;
            //
            String Remarks = String.Empty;
            DataTable objTableCandidateDetails = null;
            DataSet objReschedulingDetails = null;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["cboExamCenterOld"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid Old Center");
                }
                else
                {
                    CurrentCenter = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["cboExamCenterNew"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid New Center");
                }
                else
                {
                    CSSCenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["txtExamDateOld"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid current date");
                }
                else
                {
                    CurrentDate = Convert.ToDateTime(_dummy);
                }

               _dummy = Convert.ToString(Request.Form["cboExamDateNew"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred date");
                }
                else
                {
                    PreferredDate = Convert.ToDateTime(_dummy);
                }


                CurrentSlot = Convert.ToString(Request.Form["cboSlotsOld"]);

                _dummy = Convert.ToString(Request.Form["start_time"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid start_time");
                }
                else
                {
                    PreferredSlot = GetTimeString(Convert.ToInt32(_dummy)); //additional validations required.
                }

                Remarks = Convert.ToString(Request.Form["remarks"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objSchedulerLocal = new IIIBL.Scheduling();
                    objSchedulerCSS = new CSSIntegration.Scheduling();


                    objDataSet = objSchedulerLocal.GetCandidateDetailsRCB(PortalApplication.ConnectionString, CurrentCenter, DateTime.Parse( CurrentDate.ToString("dd-MMM-yyyy") + " " + CurrentSlot ));
                    //if (Message == String.Empty)
                    //{
                        if (objDataSet != null && objDataSet.Tables.Count >= 1)
                        {
                            objTableCandidateDetails = objDataSet.Tables[0];
                            if (objTableCandidateDetails.Rows.Count > 0)
                            {
                                //Add 4 columns:
                                objTableCandidateDetails.Columns.Add("center_id", typeof(Int32));
                                objTableCandidateDetails.Columns.Add("preferred_test_date", typeof(DateTime));
                                objTableCandidateDetails.Columns.Add("exam_duration", typeof(Int32));
                                objTableCandidateDetails.Columns.Add("from_time", typeof(String));
                                foreach (DataRow dr in objTableCandidateDetails.Rows)
                                {
                                    dr["center_id"] = CSSCenterId;
                                    dr["preferred_test_date"] = PreferredDate;
                                    dr["exam_duration"] = PortalApplication.ExamDuration;
                                    dr["from_time"] = PreferredSlot;
                                }

                                objReschedulingDetails = objSchedulerCSS.RescheduleSeat(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, objTableCandidateDetails, Remarks, "NSEIT-IIIExams.org-Rescheduler - " + PortalSession.UserLoginID, out Status, out Message);

                                if (Status == "FAIL")
                                {
                                    Success = false;
                                    //Message = Message;
                                }
                                else if (Status == "SUCCESS")
                                {
                                    if (objReschedulingDetails == null)
                                    {
                                        Success = false;
                                        Message = "Unable to receive rescheduling data - 1. Please contact helpdesk";
                                        s = HelperUtilities.ToJSON(Success, Message);
                                    }
                                    else
                                    {
                                        if (objReschedulingDetails.Tables.Count == 0)
                                        {
                                            Success = false;
                                            Message = "Unable to receive rescheduling data - 2. Please contact helpdesk";
                                            s = HelperUtilities.ToJSON(Success, Message);
                                        }
                                        else
                                        {
                                            DataTable dataTable = objReschedulingDetails.Tables[0];
                                            dataTable.Columns.Remove("SALUTATION");
                                            dataTable.Columns.Remove("CANDIDATE_NAME");
                                            dataTable.Columns.Remove("CANDIDATE_EMAIL");
                                            dataTable.Columns.Remove("CANDIDATE_PHONE");

                                            objReschedulingDetails = objSchedulerLocal.UpdateBookingStatusBulk(PortalApplication.ConnectionString, objReschedulingDetails.Tables[0]);

                                            if (objReschedulingDetails == null)
                                            {
                                                Success = false;
                                                Message = "Unable to receive rescheduling data (3). Please contact helpdesk";
                                                s = HelperUtilities.ToJSON(Success, Message);
                                            }
                                            else
                                            {
                                                if (objReschedulingDetails.Tables.Count == 0)
                                                {
                                                    Success = false;
                                                    Message = "Unable to receive rescheduling data (4). Please contact helpdesk";
                                                    s = HelperUtilities.ToJSON(Success, Message);
                                                }
                                                else
                                                {
                                                    //return resultset back to JSON.
                                                    /*
                                                    urn,
                                                    client_id,
                                                    client_reference_number_old,
                                                    css_reference_number_old,
                                                    client_reference_number_new,
                                                    css_reference_number_new,
                                                    center_id,
                                                    test_date,
                                                    test_duration,
                                                    test_time,
                                                    status,
                                                    reason,
                                                    iii_status,
                                                    iii_reason,
                                                    sntExamCenterId
                                                    */
                                                    Message = "Process completed. Please check the status below for individual URNs";
                                                    s = HelperUtilities.ToJSON(Success, Message, objReschedulingDetails.Tables[0]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Success = false;
                                Message = "Unable to fetch candidate data. Please contact helpdesk.";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = "Unable to fetch candidate data. Please contact helpdesk.";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    //}
                    //else
                    //{
                    //    Success = false;
                    //    s = HelperUtilities.ToJSON(Success, Message);
                    //}
                }
                catch (Exception ex)
                {
                    Success = false;
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objSchedulerLocal = null;
                    objSchedulerCSS = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        
        /*
        [HttpPost]//Single Candidate
        [AuthorizeAJAX]
        public ActionResult RescheduleBatch(String dummy="")
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            //From the form
            String URN = String.Empty;
            Int32 CSSCenterId = 0;
            DateTime TestDate = DateTime.Now;
            String FromTime = String.Empty;
            String Remarks = String.Empty;
            //
            DataTable objTableCandidateDetails = null;
            DataSet objDataSetResults = null;
            DataTable objDataTableResults = null;
            try
            {
                String _dummy = String.Empty;
                _dummy = Convert.ToString(Request.Form["center_id"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid center_id");
                }
                else
                {
                    CSSCenterId = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["preferred_date"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid preferred_date");
                }
                else
                {
                    TestDate = Convert.ToDateTime(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["start_time"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid start_time");
                }
                else
                {
                    FromTime = GetTimeString(Convert.ToInt32(_dummy)); //additional validations required.
                }

                Remarks = Convert.ToString(Request.Form["remarks"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError("Scheduling", "BookSeat", ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new IIIBL.Scheduling();

                    Message = objScheduler.GetCandidateDetailsRC(PortalApplication.ConnectionString, URN, out objDataSet);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count >= 1)
                        {
                            objTableCandidateDetails = objDataSet.Tables[0];
                            if (objTableCandidateDetails.Rows.Count > 0)
                            {
                                objDataSetResults = objScheduler.Reschedule(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, objDataTable , Remarks, "NSEIT-IIIExams.org-Rescheduler - " + PortalSession.UserLoginID, out Status, out Message);
                                //Log -- 
                                if (Status == "SUCCESS")
                                {
                                    objScheduler.UpdateBookingStatus(PortalApplication.ConnectionString, URN, ClientSideIdentifier, CssReferenceNumberNew, TestDate, FromTime, CSSCenterId, LanguageId);
                                    Success = true;
                                    Message = "Candidate(s) Rescheduled Successfully";
                                    s = HelperUtilities.ToJSON(Success, Message);
                                }
                                else
                                {
                                    Success = false;
                                    switch (Message)
                                    {
                                        case "ERROR_OCCURED":
                                            Message = "Error occured while rescheduling the candidate.\nTry again after sometime.\nIf the problem persists, contact technical support team.";
                                            break;
                                        case "INVALID_CLIENT":
                                            Message = "Invalid client id.\nPlease contact technical support team";
                                            break;
                                        case "DUPLICATE_REFERENCE_NUMBER_FOR_CLIENT":
                                            Message = "Duplicate client reference number.\nPlease contact technical support team";
                                            break;
                                        case "NO_SUCH_BATCH":
                                            Message = "Invalid batch.\nTry again after some time. If the problem persists, contact technical support team";
                                            break;
                                        case "SEAT_NOT_BOOKED":
                                            Message = "Candidate rescheduling failed.\nTry again after some time. If the problem persists, contact technical support team";
                                            break;
                                        case "INVALID_CANDIDATE_DETAILS":
                                            Message = "Invalid Candiate Details OR Booking is already cancelled";
                                            break;
                                        default:
                                            Message = Status + " - " + Message + "\nUnknown Error Occured.\nTry again after sometime.\nIf the problem persists, contact technical support team.";
                                            break;
                                    }
                                    s = HelperUtilities.ToJSON(Success, Message);
                                }
                            }
                            else
                            {
                                Success = false;
                                Message = "Unable to fetch candidate data. Please contact helpdesk.";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = "Unable to fetch candidate data. Please contact helpdesk.";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Success = false;
                    Errorlogger.LogError("Scheduler", "BookSeat", ex, Request.Form);
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objScheduler = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }
        */

        [HttpGet]//Cancel Candidate
        [AuthorizeExt]
        public ActionResult CC()
        {
            return View();
        }

        [HttpGet]//Cancel Batch
        [AuthorizeExt]
        public ActionResult CB()
        {
            return View();
        }

        [HttpPost]//Cancel Candidate
        [AuthorizeAJAX]
        public ActionResult CC(String dummy= "")
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            CSSIntegration.Scheduling objSchedulerCSS = null;

            DataSet objDataSet = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            //From the form
            String URN = String.Empty;
            String Remarks = String.Empty;

            DataTable objTableCandidateDetails = null;
            DataSet objReschedulingDetails = null;
            try
            {
                String _dummy = String.Empty;

                URN = Convert.ToString(Request.Form["urn"]);
                Remarks = Convert.ToString(Request.Form["remarks"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new IIIBL.Scheduling();
                    objSchedulerCSS = new CSSIntegration.Scheduling();

                    Message = objScheduler.GetCandidateDetailsRC(PortalApplication.ConnectionString, URN, out objDataSet);
                    if (Message == String.Empty)
                    {
                        if (objDataSet != null && objDataSet.Tables.Count >= 1)
                        {
                            objTableCandidateDetails = objDataSet.Tables[0];
                            if (objTableCandidateDetails.Rows.Count > 0)
                            {
                                //Add 4 columns:
                                objTableCandidateDetails.Columns.Add("center_id", typeof(Int32));
                                objTableCandidateDetails.Columns.Add("preferred_test_date", typeof(DateTime));
                                objTableCandidateDetails.Columns.Add("exam_duration", typeof(Int32));
                                objTableCandidateDetails.Columns.Add("from_time", typeof(String));

                                objReschedulingDetails = objSchedulerCSS.CancelSeat(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, objTableCandidateDetails, Remarks, "NSEIT-IIIExams.org-ScheduleCanceller - " + PortalSession.UserLoginID, out Status, out Message);

                                if (Status == "FAIL")
                                {
                                    Success = false;
                                    //Message = Message;
                                }
                                else if (Status == "SUCCESS")
                                {
                                    if (objReschedulingDetails == null)
                                    {
                                        Success = false;
                                        Message = "Unable to receive cancellation data - 1. Please contact helpdesk";
                                        s = HelperUtilities.ToJSON(Success, Message);
                                    }
                                    else
                                    {
                                        if (objReschedulingDetails.Tables.Count == 0)
                                        {
                                            Success = false;
                                            Message = "Unable to receive cancellation data - 2. Please contact helpdesk";
                                            s = HelperUtilities.ToJSON(Success, Message);
                                        }
                                        else
                                        {
                                            DataTable dataTable = objReschedulingDetails.Tables[0];

                                            objReschedulingDetails = objScheduler.UpdateCancellationStatusBulk(PortalApplication.ConnectionString, dataTable);

                                            if (objReschedulingDetails == null)
                                            {
                                                Success = false;
                                                Message = "Unable to receive cancellation data (3). Please contact helpdesk";
                                                s = HelperUtilities.ToJSON(Success, Message);
                                            }
                                            else
                                            {
                                                if (objReschedulingDetails.Tables.Count == 0)
                                                {
                                                    Success = false;
                                                    Message = "Unable to receive cancellation data (4). Please contact helpdesk";
                                                    s = HelperUtilities.ToJSON(Success, Message);
                                                }
                                                else
                                                {
                                                    //return resultset back to JSON.
                                                    /*
                                                    urn,
                                                    client_id,
                                                    client_reference_number_old,
                                                    css_reference_number_old,
                                                    client_reference_number_new,
                                                    css_reference_number_new,
                                                    center_id,
                                                    test_date,
                                                    test_duration,
                                                    test_time,
                                                    status,
                                                    reason,
                                                    iii_status,
                                                    iii_reason,
                                                    sntExamCenterId
                                                    */
                                                    Message = "Process completed. Please check the status below for individual URNs";
                                                    s = HelperUtilities.ToJSON(Success, Message, objReschedulingDetails.Tables[0]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Success = false;
                                Message = "Unable to fetch candidate data. Please contact helpdesk.";
                                s = HelperUtilities.ToJSON(Success, Message);
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = "Unable to fetch candidate data. Please contact helpdesk.";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                }
                catch (Exception ex)
                {
                    Success = false;
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataSet = null;
                    objScheduler = null;
                    objSchedulerCSS = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;

        }

        [HttpPost]//Cancel Batch
        [AuthorizeAJAX]
        public ActionResult CB(String dummy = "")
        {
            JsonResult objJsonResult = null;
            IIIBL.Scheduling objScheduler = null;
            CSSIntegration.Scheduling objSchedulerCSS = null;

            DataSet objDataSet = null;
            DataTable objDataTable = null;

            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            //From the form
            Int32 CurrentCenter = 0;
            Int32 CSSCenterId = 0;

            DateTime CurrentDate = DateTime.Now;
            DateTime PreferredDate = DateTime.Now;

            String CurrentSlot = String.Empty;
            String PreferredSlot = String.Empty;

            // To be populated within.
            String Salutation = String.Empty;
            String CandidateName = String.Empty;
            String CandidateEmail = String.Empty;
            String CandidatePhone = String.Empty;
            //
            String Remarks = String.Empty;
            DataTable objTableCandidateDetails = null;
            DataSet objReschedulingDetails = null;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["cboExamCenterOld"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid Old Center");
                }
                else
                {
                    CurrentCenter = Convert.ToInt32(_dummy);
                }

                _dummy = Convert.ToString(Request.Form["txtExamDateOld"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid current date");
                }
                else
                {
                    CurrentDate = Convert.ToDateTime(_dummy);
                }

                CurrentSlot = Convert.ToString(Request.Form["cboSlotsOld"]);

                Remarks = Convert.ToString(Request.Form["remarks"]);
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (Success)
            {
                try
                {
                    objScheduler = new IIIBL.Scheduling();
                    objSchedulerCSS = new CSSIntegration.Scheduling();

                    objDataSet = objScheduler.GetCandidateDetailsRCB(PortalApplication.ConnectionString, CurrentCenter, DateTime.Parse(CurrentDate.ToString("dd-MMM-yyyy") + " " + CurrentSlot));
                    //if (Message == String.Empty)
                    //{
                    if (objDataSet != null && objDataSet.Tables.Count >= 1)
                    {
                        objTableCandidateDetails = objDataSet.Tables[0];
                        if (objTableCandidateDetails.Rows.Count > 0)
                        {
                            //Add 4 columns:
                            objTableCandidateDetails.Columns.Add("center_id", typeof(Int32));
                            objTableCandidateDetails.Columns.Add("preferred_test_date", typeof(DateTime));
                            objTableCandidateDetails.Columns.Add("exam_duration", typeof(Int32));
                            objTableCandidateDetails.Columns.Add("from_time", typeof(String));
                            
                            objReschedulingDetails = objSchedulerCSS.CancelSeat(PortalApplication.CSSConnectionString, PortalApplication.CSSClientId, objTableCandidateDetails, Remarks, "NSEIT-IIIExams.org-Rescheduler - " + PortalSession.UserLoginID, out Status, out Message);

                            if (Status == "FAIL")
                            {
                                Success = false;
                                //Message = Message;
                            }
                            else if (Status == "SUCCESS")
                            {
                                if (objReschedulingDetails == null)
                                {
                                    Success = false;
                                    Message = "Unable to receive cancellation data - 1. Please contact helpdesk";
                                    s = HelperUtilities.ToJSON(Success, Message);
                                }
                                else
                                {
                                    if (objReschedulingDetails.Tables.Count == 0)
                                    {
                                        Success = false;
                                        Message = "Unable to receive cancellation data - 2. Please contact helpdesk";
                                        s = HelperUtilities.ToJSON(Success, Message);
                                    }
                                    else
                                    {
                                        DataTable dataTable = objReschedulingDetails.Tables[0];
                                        /*dataTable.Columns.Remove("SALUTATION");
                                        dataTable.Columns.Remove("CANDIDATE_NAME");
                                        dataTable.Columns.Remove("CANDIDATE_EMAIL");
                                        dataTable.Columns.Remove("CANDIDATE_PHONE");*/

                                        objReschedulingDetails = objScheduler.UpdateCancellationStatusBulk(PortalApplication.ConnectionString, objReschedulingDetails.Tables[0]);

                                        if (objReschedulingDetails == null)
                                        {
                                            Success = false;
                                            Message = "Unable to receive cancellation data (3). Please contact helpdesk";
                                            s = HelperUtilities.ToJSON(Success, Message);
                                        }
                                        else
                                        {
                                            if (objReschedulingDetails.Tables.Count == 0)
                                            {
                                                Success = false;
                                                Message = "Unable to receive cancellation data (4). Please contact helpdesk";
                                                s = HelperUtilities.ToJSON(Success, Message);
                                            }
                                            else
                                            {
                                                //return resultset back to JSON.
                                                /*
                                                urn,
                                                client_id,
                                                client_reference_number_old,
                                                css_reference_number_old,
                                                client_reference_number_new,
                                                css_reference_number_new,
                                                center_id,
                                                test_date,
                                                test_duration,
                                                test_time,
                                                status,
                                                reason,
                                                iii_status,
                                                iii_reason,
                                                sntExamCenterId
                                                */
                                                Message = "Process completed. Please check the status below for individual URNs";
                                                s = HelperUtilities.ToJSON(Success, Message, objReschedulingDetails.Tables[0]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Success = false;
                            Message = "Unable to fetch candidate data. Please contact helpdesk.";
                            s = HelperUtilities.ToJSON(Success, Message);
                        }
                    }
                    else
                    {
                        Success = false;
                        Message = "Unable to fetch candidate data. Please contact helpdesk.";
                        s = HelperUtilities.ToJSON(Success, Message);
                    }
                    //}
                    //else
                    //{
                    //    Success = false;
                    //    s = HelperUtilities.ToJSON(Success, Message);
                    //}
                }
                catch (Exception ex)
                {
                    Success = false;
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(Success, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                    objScheduler = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }


        [HttpGet]//Reconcile
        [AuthorizeExt]
        public ActionResult ReconcileBooking()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]//Reconcile
        [AuthorizeAJAX]
        public JsonResult ReconcileBooking(String dummy = "")
        {
            //Get Data From Oracle...
            //create or replace procedure sp_get_recon_data
            //(
            //p_client_id   number,
            //p_recon_date date,
            //p_curMain     out sys_refcursor
            //)
            //Send Data To SQL Server...
            //ALTER procedure[dbo].[sp_reconcile_bookings]
            //(
            //   @data typ_booking_recon_details readonly
            //)                
            //Dump Data received... 
            JsonResult objJsonResult = null;
            String s = String.Empty;
            Boolean Success = true;
            String Message = String.Empty; //Double Use...
            String Status = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;

            DataSet objDataSet = null;
            DataTable objDataTable = null;

            DateTime ReconDate = DateTime.Now;
            try
            {
                String _dummy = String.Empty;

                _dummy = Convert.ToString(Request.Form["txtReconDate"]);
                if (_dummy.Trim() == String.Empty || _dummy == null)
                {
                    throw new Exception("Invalid recon date");
                }
                else
                {
                    ReconDate = Convert.ToDateTime(_dummy);
                }
            }
            catch (Exception ex)
            {
                Success = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Success)
            {
                IIIBL.Scheduling objSchedulingLocal = new IIIBL.Scheduling();
                CSSIntegration.Scheduling objSchedulingCSS = new CSSIntegration.Scheduling();

                objDataSet = objSchedulingCSS.GetReconcileBoookingDetails( PortalApplication.CSSConnectionString , ReconDate, PortalApplication.CSSClientId);

                if (objDataSet == null)
                {
                    Success = false;
                    Message = "Unable to fetch reconciliation data (error code:1). Please contact helpdesk";
                    s = HelperUtilities.ToJSON(Success, Message, null);
                }
                else
                {
                    if (objDataSet.Tables.Count ==0)
                    {
                        Success = false;
                        Message = "Unable to fetch reconciliation data (error code:2). Please contact helpdesk";
                        s = HelperUtilities.ToJSON(Success, Message, null);
                    }
                    else
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable == null )
                        {
                            Success = false;
                            Message = "Unable to fetch reconciliation data (error code:3). Please contact helpdesk";
                            s = HelperUtilities.ToJSON(Success, Message, null);
                        }
                        else
                        {
                            if (objDataTable.Rows.Count == 0)
                            {
                                Success = true;
                                Message = "No data found for reconciliation for given date (Information code:4). If you are expecting data to be present for this date then contact helpdesk";
                                s = HelperUtilities.ToJSON(Success, Message, null);
                            }
                            else
                            {
                                objDataSet = objSchedulingLocal.ReconcileBoookings(PortalApplication.ConnectionString, objDataTable);
                                if (objDataSet == null )
                                {
                                    Success = false;
                                    Message = "No data found for reconciliation for given date (error code:5). If you are expecting data to be present for this date then contact helpdesk";
                                    s = HelperUtilities.ToJSON(Success, Message, null);
                                }
                                else
                                {
                                    if (objDataSet.Tables.Count == 0)
                                    {
                                        Success = false;
                                        Message = "No data found for reconciliation for given date (error code:6). If you are expecting data to be present for this date then contact helpdesk";
                                        s = HelperUtilities.ToJSON(Success, Message, null);
                                    }
                                    else
                                    {
                                        objDataTable = objDataSet.Tables[0];
                                        if (objDataTable.Rows.Count > 0)
                                        {
                                            Success = true;
                                            Message = "Process completed successfully. Please check the response file for details";

                                            String Filename = PortalSession.UserLoginID + "_ReconciliationReport_" + ReconDate.ToString("dd-MMM-yyyy") + "_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                                            String OutputFileName = String.Format("../Downloads/{0}", Filename);
                                            String OutputFileName2 = Server.MapPath(OutputFileName);

                                            /*
                                                CANDIDATE_ID				
                                                CLIENT_ID                   
                                                CLIENT_SIDE_IDENTIFIER      
                                                CENTER_ID                   
                                                TEST_DATE                   
                                                START_TIME_N                
                                                IS_CANCELLED                
                                                CANCELLATION_REMARKS        
                                                CANCELLATION_SOURCE         
                                                BOOKING_TIMESTAMP           
                                                CANCELLATION_TIMESTAMP      
                                                OLD_CANDIDATE_ID			
                                                STATUS						
                                                REMARKS						
                                                center_id_local
                                                varExamCenterName
                                            */
                                            String[] DisplayColumns = new String[] { "CANDIDATE_ID", "CLIENT_ID", "CLIENT_SIDE_IDENTIFIER", "CENTER_ID",
                                                                                     "TEST_DATE","START_TIME_N", "IS_CANCELLED", "CANCELLATION_REMARKS",
                                                                                     "CANCELLATION_SOURCE", "BOOKING_TIMESTAMP", "CANCELLATION_TIMESTAMP", "OLD_CANDIDATE_ID",
                                                                                      "STATUS","REMARKS", "center_id_local", "varExamCenterName"
                                                                                    };
                                            String[] DisplayHeaders = new String[] { "CSS Candidate Id", "CSS Client Id", "Reg Portal Candidate Id", "Center Id",
                                                                                     "Test Date","Test Time", "Is Cancelled?", "Cancellation Remarks",
                                                                                     "Cancellation Source", "Booking Timestamp", "Cancellation Timestamp", "Old CSS Candidate Id",
                                                                                      "STATUS","REMARKS", "Center id local", "Exam Center Name" };
                                            String[] DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

                                            XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormat);

                                            //objDataTable.DataTable2File(OutputFileName2, "\t");

                                            KVPair = new KeyValuePair<String, String>[1];
                                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                                            s = HelperUtilities.ToJSON(Success, Message, null, KVPair);
                                        }
                                        else
                                        {
                                            Success = false;
                                            Message = Message = "No data found for reconciliation for given date (error code:7). If you are expecting data to be present for this date then contact helpdesk";
                                            s = HelperUtilities.ToJSON(Success, Message, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        private String GetTimeString(Int32 tm)
        {
            String retval = String.Empty;
            Int32 nHours = 0 ;
            Int32 nMinutes = 0 ;
            
            if (tm > 1440)
            {
                throw new Exception("Invalid Time");
            }
            else
            {
                nHours = (tm - (tm % 60)) / 60;
                nMinutes = tm % 60;
            }

            if (tm == 1440)
            {
                retval = "00:00 AM";
            }
            else if (tm >= 720 && tm < 780)
            {
                retval = "12" + ":" + Convert.ToString(nMinutes).PadLeft(2, '0') + " PM";
            }
            else if (tm >= 780)//??
            {
                retval = Convert.ToString(nHours - 12).PadLeft(2, '0') + ':' + Convert.ToString(nMinutes).PadLeft(2, '0') + " PM";
            }
            else
            {
                retval = Convert.ToString(nHours).PadLeft( 2, '0') + ':' + Convert.ToString(nMinutes).PadLeft( 2, '0') + " AM";
            }
            return retval;
        }
    }
}