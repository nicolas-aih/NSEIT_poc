using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IIIBL;
using System.Configuration;
using System.IO;
using DotNetIntegrationKit;
using System.Globalization;
using System.Reflection;

namespace IIIRegistrationPortal2.Controllers
{
    public partial class PaymentsController : Controller
    {
        [HttpGet]
        public ActionResult ExamFeesPayment()
        {
            ViewBag.Response = "";
            ViewBag.ClassName = "col-sm-6";
            return View();
        }

        [HttpPost]
        public ActionResult PGRequest(String Dummy = "")
        {
            String TransactionId = Convert.ToString(Request.Form["hdBatchNo"]);
            Decimal Amount = Convert.ToDecimal(Request.Form["hdAmount"]);
            String PaymentGateWay = Convert.ToString(Request.Form["selPG"]);

            ActionResult actionResult = null;

            Errorlogger.LogInfo(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "Going into CheckPreviousTransactionRequest");

            if (CheckPreviousTransactionRequest(TransactionId, Amount))
            {
                ViewBag.Status = "Payment is already done";
                ViewBag.ShowTable = true;
                actionResult = View("PGResponse");
            }
            else
            {
                if (PaymentGateWay == "TPSL")//TechProcess
                {
                    actionResult = TPSL_PGRequest_T(TransactionId, Amount);
                }
                else if (PaymentGateWay == "PAYTM")//PayTM
                {
                    //PayTM Sepcific method.
                    actionResult = PGRequestPayTM(TransactionId, Amount);
                }
            }
            return actionResult;
        }

        private Boolean CheckPreviousTransactionRequest( String TransactionId, Decimal Amount )
        {
            Boolean IsAlreadyPaid = true; //Kept as true to be on safer side...
            IIIBL.BatchMgmt objBatchMgmt = null;
            
            String PaymentGateway = String.Empty;
            String NSEITReferenceNumber = String.Empty;
            DateTime PaymentDate = DateTime.Now;
            try
            {
                objBatchMgmt = new BatchMgmt();

                objBatchMgmt.GetPreviousPaymentAttemptDetails(PortalApplication.ConnectionString, TransactionId, out NSEITReferenceNumber, out PaymentDate, out PaymentGateway);

                if (NSEITReferenceNumber.Trim() == String.Empty) //Will be empty only during 1st attempt.
                {
                    IsAlreadyPaid = false;
                }
                else
                {
                    if (PaymentGateway == "TPSL")
                    {
                        IsAlreadyPaid = TPSL_PGRequest_O(TransactionId, Amount, NSEITReferenceNumber, PaymentDate);

                        Errorlogger.LogInfo(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "TPSL: IsAlreadyPaid : " + IsAlreadyPaid);
                    }
                    if (PaymentGateway == "PAYTM")
                    {
                        IsAlreadyPaid = PaytmPaymentStatusCheck(TransactionId, Amount, NSEITReferenceNumber, PaymentDate);

                        Errorlogger.LogInfo(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "PAYTM: IsAlreadyPaid : " + IsAlreadyPaid);
                    }
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            finally
            {
                objBatchMgmt = null;
            }
            return IsAlreadyPaid;
        }



    }
}