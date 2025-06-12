using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Reflection;
using IIIBL;

namespace IIIRegistrationPortal2.Controllers
{
    public class BatchesController : Controller
    {
        //// GET: Batches
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AuthorizeExt]
        [HttpGet]
        public ActionResult ExamRegistration()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [AuthorizeExt]
        [HttpGet]
        public ActionResult ManageBatches()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        //[AuthorizeExt]
        [HttpGet]
        public ActionResult ManageBatchesB()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetTrainedApplicants()
        {
            JsonResult result = new JsonResult();

            Boolean Status = true;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            Int32 ExamBodyId = 0;
            Int32 ExamCenterId = 0;
            String s = String.Empty;
            try
            {
                Status = true;

                String _dummy = Request.Form["txtFromDate"];
                FromDate = Convert.ToDateTime( _dummy );

                _dummy = Request.Form["txtToDate"];
                ToDate = Convert.ToDateTime(_dummy);

                _dummy = Request.Form["ddlExamBody"];
                ExamBodyId = Convert.ToInt32(_dummy);

                _dummy = Request.Form["ddlCenter"];
                ExamCenterId = Convert.ToInt32(_dummy);
            }
            catch(Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (Status)
            {
                ExamRegistration objExamBody = null;
                DataTable objPaymentMode = null;
                DataTable objCandidateDetails = null;
                DataSet objDataSet = null;
                try
                {
                    objExamBody = new IIIBL.ExamRegistration();
                    objDataSet = objExamBody.GetPaymentModes(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                    objPaymentMode = objDataSet.Tables[0].Copy();

                    objDataSet = objExamBody.GetTrainedApplicants(PortalApplication.ConnectionString, PortalSession.UserID, ExamBodyId, ExamCenterId, FromDate, ToDate);
                    objCandidateDetails = objDataSet.Tables[0];

                    String PaymentMode = HelperUtilities.DataTable2JSON(objPaymentMode);
                    KeyValuePair<String, String>[] KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("PaymentMode", PaymentMode);
                    s = HelperUtilities.ToJSON(true, String.Empty, objCandidateDetails, KVPair);

                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {
                    objExamBody = null;
                    objPaymentMode = null;
                    objCandidateDetails = null;
                    objDataSet = null;
                }
            }
            else
            {
                //Do nothing
            }

            result.Data = s;
            return result;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult UpdateApplicantDetails()
        {
            JsonResult result = new JsonResult();
            System.String s = String.Empty;
            System.Boolean Status = true;
            System.String Message = String.Empty;

            DataSet objDataSetOutput = null;

            String chk = String.Empty;
            String txtOnDate = String.Empty;
            String txtEmail = String.Empty;
            String txtURN = String.Empty;
            String _dummy = String.Empty;

            String URN = String.Empty;
            String OnorAfterDate = String.Empty;
            String EmailIds = String.Empty;
            DataTable dataTable = null;

            String SchedulingMode = String.Empty;
            String BatchMode = String.Empty;
            String PaymentMode = String.Empty;
            String InsurerRemark = String.Empty;
            String EnrollmentNo = String.Empty;
            try
            {
                //50
                Int32 MaxRows = 50;

                dataTable = new DataTable();
                dataTable.Columns.Add("URN", typeof(String));
                dataTable.Columns.Add("PaymentMode", typeof(String));
                dataTable.Columns.Add("InsurerRemark", typeof(String));
                dataTable.Columns.Add("EnrollmentNo", typeof(String));
                dataTable.Columns.Add("OnOrAfterDate", typeof(String));
                dataTable.Columns.Add("EmailIds", typeof(String));
                dataTable.Columns.Add("BatchMode", typeof(String));
                dataTable.Columns.Add("SchedulingMode", typeof(String));
                dataTable.Columns.Add("ExamBatchNo", typeof(String));
                dataTable.Columns.Add("IsValidRecord", typeof(Boolean));
                dataTable.Columns.Add("UploadRemark", typeof(String));

                PaymentMode = Convert.ToString(Request.Form["ddlpaymentmode"]);
                BatchMode = Convert.ToString(Request.Form["ddlBatchmode"]);
                SchedulingMode = Convert.ToString(Request.Form["ddlSchedulingMode"]);
                InsurerRemark = Convert.ToString(Request.Form["txtRemarks"]);
                EnrollmentNo = String.Empty; //Keep it blank for this mode....

                if (PaymentMode == "Credit")
                {
                    BatchMode = "BULK";
                }
                if (BatchMode == "1")// 1 means BULK
                {
                    BatchMode = "BULK";
                }
                if (BatchMode == "2")// 1 means BULK
                {
                    BatchMode = "SINGLE";
                }

                if (SchedulingMode == "1")
                {
                    SchedulingMode = "AUTO";
                }
                else
                {
                    SchedulingMode = "SELF";
                }

                for (Int32 i = 0; i < MaxRows; i++)
                {
                    //
                    chk = String.Format("chk{0}", i);
                    txtOnDate = String.Format("txtOnDate{0}", i);
                    txtEmail = String.Format("txtEmail{0}", i);
                    txtURN = String.Format("txtURN{0}", i);

                    _dummy = String.Empty;

                    URN = String.Empty;
                    OnorAfterDate = String.Empty;
                    EmailIds = String.Empty;

                    if (Request.Form[chk] == null)
                    {
                        continue;
                    }
                    else if ((Request.Form[chk] == "on") || (Request.Form[chk] == "true"))
                    {
                        URN = Convert.ToString(Request.Form[txtURN]);
                        EmailIds = Convert.ToString(Request.Form[txtEmail]);

                        _dummy = Convert.ToString(Request.Form[txtOnDate]);
                        if (_dummy.Trim() == String.Empty)
                        {
                            throw new Exception("Invalid txtApplicationDate");
                        }
                        else
                        {
                            OnorAfterDate = Convert.ToDateTime(_dummy).ToString("dd-MMM-yyyy");
                        }
                        dataTable.Rows.Add(new Object[] { URN, PaymentMode, InsurerRemark, EnrollmentNo, OnorAfterDate, EmailIds, BatchMode, SchedulingMode, DBNull.Value, DBNull.Value, DBNull.Value });
                    }
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            //-->
            if (Status)
            {
                ExamRegistration objExamRegistration = null;
                try
                {
                    objExamRegistration = new IIIBL.ExamRegistration();
                    objDataSetOutput = objExamRegistration.BulkUploadExamRegData2(PortalApplication.ConnectionString, dataTable, PortalSession.UserID);

                    String OutputFileName = Server.MapPath("../downloads") + "\\" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                    String OutputFileName2 = "../downloads/" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";

                    DataTable objDataTable = objDataSetOutput.Tables[0];
                    //objDataTable.DataTable2File(OutputFileName, "\t");

                    String[] DisplayColumns = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark" };
                    String[] DisplayHeaders = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark"
                                                        };
                    String[] DisplayFormats = new String[] { String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty};


                    XLXporter.WriteExcel(OutputFileName, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormats);

                    KeyValuePair<String, String>[] KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName2);
                    s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);

                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
                }
                finally
                {
                    objExamRegistration = null;
                }
            }

            result.Data = s;
            return result;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult UploadRegistration()
        {
            String s = String.Empty;
            JsonResult result = new JsonResult();
            if (PortalSession.UserID == 0)
            {
                Response.ContentType = "text/html";
                Response.Write("UPLOAD FAILED: User session expired. Kindly re-login.");
                return result;
            }

            ExamRegistration oBLExamBody = null;
            DataSet objResult = null;
            FileInfo oFile = null;
            try
            {
                String Message = String.Empty;
                Boolean Status = true;
                //string strFileName = DateTime.Now.Ticks.ToString();
                String strFileName = PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt");

                int userid = PortalSession.UserID;
                string strExtension = strFileName + Path.GetExtension(Request.Files[0].FileName);
                string strSaveLocation = Server.MapPath("~/Uploads") + "\\" + strExtension;
                Request.Files[0].SaveAs(strSaveLocation);


                oBLExamBody = new IIIBL.ExamRegistration();
                objResult = oBLExamBody.BulkUploadExamRegData(PortalApplication.ConnectionString, strSaveLocation, userid, out Status, out Message);

                //Delete Downloaded file
                String OutputFileName = Server.MapPath("../downloads") + "\\" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                String OutputFileName2 = "../downloads/" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";

                oFile = new FileInfo(strSaveLocation);
                oFile.Delete();
                if (Status == false )
                {
                    s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                }
                else 
                {
                    //New
                    DataTable objDataTable = objResult.Tables[0];
                    //objDataTable.DataTable2File(OutputFileName, "\t");

                    String[] DisplayColumns = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark" };
                    String[] DisplayHeaders = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark"
                                                        };
                    String[] DisplayFormats = new String[] { String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty};

                    XLXporter.WriteExcel(OutputFileName, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormats);

                    KeyValuePair<String, String>[] KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName2);
                    s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                    //End New
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                oBLExamBody = null;
                objResult = null;
                oFile = null;
            }
            result.Data = s;
            return result;
        }

        [AuthorizeAJAX]
        [HttpPost]
        public JsonResult GetExamCenters(Int16 ExamBodyID)
        {
            JsonResult result = null;
            DataSet objdatatable = null;
            String s = String.Empty;
            MasterData masterData = new IIIBL.MasterData();
            objdatatable = masterData.GetExamCenter(PortalApplication.ConnectionString, ExamBodyID, -1);
            masterData = null;

            DataTable dtDetails = objdatatable.Tables[0];
            s = HelperUtilities.DataTable2JSON(dtDetails);
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = s;
            result = jsonResult;
            return result;
        }

    }
}