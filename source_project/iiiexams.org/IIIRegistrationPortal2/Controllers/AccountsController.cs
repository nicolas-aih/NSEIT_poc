using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Web.Services.Description;

namespace IIIRegistrationPortal2.Controllers
{
    public class AccountsController : Controller
    {
        [HttpGet]
        [AuthorizeExt]
        public ActionResult CreditBalance()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3, 2 };
            
            String Output = String.Empty;
            Decimal Amount = 0M;

            if (@PortalSession.RoleCode.In(new string[] { "BR", "CA", "WA", "IMF", "I" }))
            {
                IIIBL.CreditBalance objCreditBalance = new IIIBL.CreditBalance();
                try
                {
                    objCreditBalance = new IIIBL.CreditBalance();
                    if (PortalApplication.IntegrationMode == "CSS")
                    {
                        Amount = objCreditBalance.GetCurrentBalance(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
                        Output = String.Format("Credit Balance as on {0} is Rupees {1}", @DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"), Amount.ToString("N", nfi));
                    }
                    else
                    {
                        //Check if Subscribed to credit balance
                        if (objCreditBalance.ValidateCreditMode(PortalApplication.ConnectionString, PortalSession.RoleCode, PortalSession.InsurerUserID) == "Y")
                        {
                            Amount = objCreditBalance.GetCurrentBalanceOAIMS(PortalApplication.OAIMSConnectionString, PortalSession.TopUserLoginID);
                            Output = String.Format("Credit Balance as on {0} is Rupees {1}", @DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"), Amount.ToString("N", nfi));
                        }
                        else
                        {
                            Output = "You have not opted for Credit Balance facility.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    Output = CommonMessages.ERROR_OCCURED;
                }
            }
            
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            if (@PortalSession.RoleCode.In(new string[] { "BR", "CA", "WA", "IMF", "I" }))
            {
                ViewBag.Output = Output;
            }
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult CreditBalance2()
        {
            JsonResult objJsonResult = null;
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSizes = new int[] { 3, 2 };

            Boolean Success = true;

            String Output = String.Empty;
            Decimal Amount = 0M;

            String CompanyType = String.Empty;
            Int32 CompanyCode = -1;
            IIIBL.CreditBalance objCreditBalance = new IIIBL.CreditBalance();
            try
            {
                CompanyType = Convert.ToString(Request.Form["cboCompanyType"]);

                CompanyCode = Convert.ToInt32(Request.Form["cboCompany"]);

                objCreditBalance = new IIIBL.CreditBalance();
                if (PortalApplication.IntegrationMode == "CSS")
                {
                    Amount = objCreditBalance.GetCurrentBalance(PortalApplication.ConnectionString, CompanyCode);
                    Output = String.Format("Credit Balance as on {0} is Rupees {1}", @DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"), Amount.ToString("N", nfi));
                }
                else
                {
                    String TopLevelCompanyCode = String.Empty;
                    //Check if Subscribed to credit balance
                    if (objCreditBalance.ValidateCreditMode(PortalApplication.ConnectionString, CompanyType, CompanyCode,out TopLevelCompanyCode) == "Y")
                    {
                        Amount = objCreditBalance.GetCurrentBalanceOAIMS(PortalApplication.OAIMSConnectionString, TopLevelCompanyCode);
                        Output = String.Format("Credit Balance as on {0} is Rupees {1}", @DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"), Amount.ToString("N", nfi));
                    }
                    else
                    {
                        Output = "You have not opted for Credit Balance facility.";
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                Success = false;
                Output = CommonMessages.ERROR_OCCURED;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = HelperUtilities.ToJSON(Success, Output); ;
            return objJsonResult;
        }


        [HttpGet]
        [AuthorizeExt]
        public ActionResult NewEntry()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult AddVoucher()
        {
            JsonResult objJsonResult = null;
            IIIBL.CreditBalance objCreditBalance = null;
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            String InstructionType = String.Empty;
            String ReferenceNo = String.Empty;
            Decimal Amount = 0;
            String ModeOfPayment = String.Empty;
            DateTime DateOfPayment = DateTime.Now;
            String Remarks = String.Empty;
            String Narration = String.Empty;
            Int32 CompanyCode = 0;
            try
            {
                String _dummy = String.Empty;

                InstructionType = Convert.ToString(Request.Form["cboInstructionType"]).Trim().ToUpper();
                if (!(InstructionType == "C" || InstructionType == "D"))
                {
                    throw new Exception("Invalid cboInstructionType");
                }

                ReferenceNo = Convert.ToString(Request.Form["txtReferenceNo"]);

                _dummy = Convert.ToString(Request.Form["txtDateOfPayment"]); ;
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtDateOfPayment");
                }
                else
                {
                    DateOfPayment = Convert.ToDateTime(_dummy.Trim());
                }

                _dummy = Convert.ToString(Request.Form["txtAmount"]); ;
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtAmount");
                }
                else
                {
                    Amount = Convert.ToDecimal(_dummy.Trim());
                }

                ModeOfPayment = Convert.ToString(Request.Form["cboModeOfPayment"]).Trim().ToUpper();
                if (!(ModeOfPayment == "NEFT" || ModeOfPayment == "RTGS" || ModeOfPayment == "IMPS" || ModeOfPayment == "CHEQUE" || ModeOfPayment == "DD"))
                {
                    throw new Exception("Invalid cboModeOfPayment");
                }

                if(@PortalSession.RoleCode == "BO")
                {
                    _dummy = Convert.ToString(Request.Form["cboCompany"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboCompany");
                    }
                    else
                    {
                        CompanyCode = Convert.ToInt32(_dummy);
                    }
                }
                if (PortalSession.RoleCode.In(new String[] {"I","CA","WA","IMF","BR" }))
                {
                    CompanyCode = PortalSession.InsurerUserID;
                }

                Remarks = Convert.ToString(Request.Form["txtRemarks"]);
                Narration = Convert.ToString(Request.Form["txtNarration"]);
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            if (_Status)
            {
                try
                {
                    objCreditBalance = new IIIBL.CreditBalance();
                    Message = objCreditBalance.SaveCreditBalanceEntry(PortalApplication.ConnectionString, CompanyCode, InstructionType , ReferenceNo, Amount, ModeOfPayment, DateOfPayment, Remarks, PortalSession.UserID, Narration);
                    _Status = (Message == String.Empty);
                    if (_Status)
                    {
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    s = HelperUtilities.ToJSON(_Status, Message, null);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objCreditBalance = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        public ActionResult ApprovalRejection()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult ApproveReject()
        {
            JsonResult objJsonResult = null;
            IIIBL.CreditBalance objCreditBalance = null;
            String s = String.Empty;
            Boolean _Status = true;
            String Message = String.Empty;

            Int32 InstructionId = 0;
            String Status = String.Empty;
            String ApproversRemark = String.Empty;
            String _dummy = String.Empty;
            try
            {
                _dummy = Request.Form["hddInstructionId"];

                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid InstructionId");
                }
                else
                {
                    InstructionId = Convert.ToInt32(_dummy);
                }
                Status = Convert.ToString( Request.Form["cboStatus"]);
                ApproversRemark = Request.Form["txtApproversRemark"];

            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }
            
            if (_Status)
            {
                try
                {
                    objCreditBalance = new IIIBL.CreditBalance();
                    Message = objCreditBalance.ApproveRejectCreditBalanceEntry(PortalApplication.ConnectionString, InstructionId, Status, ApproversRemark, PortalSession.UserID);
                    _Status = (Message == String.Empty);
                    if (_Status)
                    {
                        Message = Status == "A" ? CommonMessages.DATA_APPROVAL_SUCCESS : CommonMessages.DATA_REJECTION_SUCCESS;
                    }
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objCreditBalance = null;
                }
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetInstructions( Int32 Hint, Int32 InstructionId = -1, Int32 CompanyCode = -1)
        {
            JsonResult objJsonResult = null;
            IIIBL.CreditBalance objCreditBalance = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            try
            {
                objCreditBalance = new IIIBL.CreditBalance();
                objDataSet = objCreditBalance.GetCreditBalanceEntries(PortalApplication.ConnectionString, Hint, InstructionId, CompanyCode);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
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
                objCreditBalance = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult LedgerReport()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public ActionResult LedgerReport(String Dummy = "")
        {
            JsonResult objJsonResult = null;

            Boolean _Status = true;
            String Message = String.Empty;
            String s = String.Empty;
            KeyValuePair<String, String>[] KVPair = null;
            IIIBL.CreditBalance objCreditBalance = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;

            Int32 CompanyCode = -1;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            try
            {
                String _dummy = String.Empty;

                if (@PortalSession.RoleCode == "BO")
                {
                    _dummy = Convert.ToString(Request.Form["cboCompany"]);
                    if (_dummy.Trim() == String.Empty)
                    {
                        throw new Exception("Invalid cboCompany");
                    }
                    else
                    {
                        CompanyCode = Convert.ToInt32(_dummy);
                    }
                }
                if (PortalSession.RoleCode.In(new String[] { "I", "CA", "WA", "IMF", "BR" }))
                {
                    CompanyCode = PortalSession.InsurerUserID;
                }

                _dummy = Convert.ToString(Request.Form["txtFromDate"]); ;
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtFromDate");
                }
                else
                {
                    FromDate = Convert.ToDateTime(_dummy.Trim());
                }

                _dummy = Convert.ToString(Request.Form["txtToDate"]); ;
                if (_dummy.Trim() == String.Empty)
                {
                    throw new Exception("Invalid txtToDate");
                }
                else
                {
                    ToDate = Convert.ToDateTime(_dummy.Trim());
                }
            }
            catch (Exception ex)
            {
                _Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            if (_Status)
            {
                try
                {
                    objCreditBalance = new IIIBL.CreditBalance();
                    objDataSet = objCreditBalance.GetLedger(PortalApplication.ConnectionString, CompanyCode, FromDate, ToDate);
                    
                    if (objDataSet != null && objDataSet.Tables.Count == 1)
                    {
                        objDataTable = objDataSet.Tables[0];
                        if (objDataTable.Rows.Count > 0)
                        {
                            _Status = true;
                            Message = String.Empty;

                            String Filename = PortalSession.UserLoginID + "_LedgerReport_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
                            String OutputFileName = String.Format("../Downloads/{0}", Filename);
                            String OutputFileName2 = Server.MapPath(OutputFileName);

                            String [] DisplayColumns = new String[] { "entry_date", "narration", "opening_balance", "entry_type", "amount", "closing_balance", "instrument_no" };
                            String [] DisplayHeaders = new String[] { "Date", "Narration", "Opening balance", "Credit / Debit", "Amount", "Closing balance" , "Reference Number" };
                            String [] DisplayFormat = new String[] { "dd-MMM-yyyy hh:mm:ss AM/PM", String.Empty, "#,##0.00", String.Empty, "#,##0.00", "#,##0.00" , String.Empty};

                            XLXporter.WriteExcel(OutputFileName2, objDataTable, DisplayColumns, DisplayHeaders, DisplayFormat);

                            //objDataTable.DataTable2File(OutputFileName2, "\t");

                            KVPair = new KeyValuePair<String, String>[1];
                            KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                            s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                        }
                        else
                        {
                            _Status = true;
                            Message = CommonMessages.NO_DATA_FOUND;
                            s = HelperUtilities.ToJSON(_Status, Message, null);
                        }
                    }
                    else
                    {
                        _Status = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                        s = HelperUtilities.ToJSON(_Status, Message, null);
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    _Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                    s = HelperUtilities.ToJSON(_Status, Message);
                }
                finally
                {
                    objDataTable = null;
                    objDataSet = null;
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetCompanyList()
        {
            JsonResult objJsonResult = null;
            IIIBL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String s = String.Empty;
            Boolean Success = false;
            String Message = String.Empty;
            String CompanyType = String.Empty;
            try
            {
                CompanyType = Convert.ToString(Request.Form["cboCompanyType"]);

                objMasterData = new IIIBL.MasterData();
                objDataSet = objMasterData.GetCompanyListForVoucherEntry(PortalApplication.ConnectionString, CompanyType);

                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = false;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                else
                {
                    Success = false;
                    Message = CommonMessages.NO_DATA_FOUND;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
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
                objMasterData = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

    }
}