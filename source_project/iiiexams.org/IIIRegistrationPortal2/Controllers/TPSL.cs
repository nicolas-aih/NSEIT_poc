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
        [HttpPost]
        public ActionResult TPSL_PGRequest_T(String TransactionId, Decimal Amount)
        {
            //Previous Unique Id
            //Previous Attempt Date
            String ViewName = String.Empty;

            String RequestType = "T";
            String Key = PortalApplication.PGKey;
            String IV = PortalApplication.PGIV;
            String MerchantCode = PortalApplication.PGMerchantCode;
            String CurrencyCode = PortalApplication.PGCurrencyCode;
            String ReturnURL = PortalApplication.PGReturnURL;

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = "";
            String strAmount = Amount.ToString(nfi);

            IIIBL.BatchMgmt objBatchMgmt = null;
            String NSEITReferenceNumber = String.Empty;
            DateTime PaymentDate = DateTime.Now;
            try
            {
                objBatchMgmt = new BatchMgmt();
                objBatchMgmt.UpdatePaymentAttempt(PortalApplication.ConnectionString, TransactionId, "TPSL" , out NSEITReferenceNumber, out PaymentDate);

                String response = "";
                RequestURL objRequestURL = new RequestURL();
                //if (RequestType.ToUpper() == "T" || RequestType.ToUpper() == "S" || RequestType.ToUpper() == "O" || RequestType.ToUpper() == "R")
                //{
                response = objRequestURL.SendRequest
                            (
                            RequestType,
                            MerchantCode,
                            NSEITReferenceNumber,
                            TransactionId,
                            strAmount,
                            CurrencyCode,
                            String.Empty,
                            ReturnURL,
                            String.Empty,
                            String.Empty,
                            String.Format("Test_{0}_0.0", strAmount),
                            PaymentDate.ToString("dd-MM-yyyy"),
                            String.Empty,
                            String.Empty,
                            String.Empty,
                            String.Empty,
                            String.Empty,
                            String.Empty,
                            Key,
                            IV
                            );
                //                  }
                String strResponse = response.ToUpper();

                bool IsValid = false;

                if (strResponse.StartsWith("ERROR")) //Display...
                {
                    //Log the response to database. Move to else once error73 call is clarified.

                    IsValid = false;
                    if (strResponse == "ERROR073")
                    { //?What does this do.... ?
                        response = objRequestURL.SendRequest
                                    (
                                        RequestType,
                                        MerchantCode,
                                        NSEITReferenceNumber, 
                                        TransactionId,
                                        strAmount,
                                        CurrencyCode,
                                        String.Empty,
                                        ReturnURL,
                                        String.Empty,
                                        String.Empty,
                                        String.Format("Test_{0}_0.0", strAmount),
                                        System.DateTime.Now.ToString("dd-MM-yyyy"),
                                        String.Empty,
                                        String.Empty,
                                        String.Empty,
                                        String.Empty,
                                        String.Empty,
                                        String.Empty,
                                        Key,
                                        IV
                                    );
                        strResponse = response.ToUpper();
                        //?? What is to be done ??? 
                    }
                    else
                    {
                        ViewBag.Status = "Error Occured";
                        ViewBag.ShowTable = false;
                        ViewName = "PGResponse";
                    }
                }
                else
                {
                    IsValid = true;
                }

                if (RequestType == "T")
                {
                    if (IsValid)
                    {
                        //Generate nseit_ref_no
                        Session["Merchant_Code"] = MerchantCode;
                        Session["IsKey"] = Key;
                        Session["IsIv"] = IV;

                        ViewBag.Response = response;
                        ViewName = "PG";
                    }
                }
                //else
                //{
                //    if (response == "")
                //    {
                //        ViewBag.Response = response;
                //    }
                //    else
                //    {
                //        ViewBag.Response = response;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return View(ViewName);
        }

        private Boolean TPSL_PGRequest_O(String TransactionId, Decimal Amount, String NSEITReferenceNumber, DateTime PaymentDate)
        {
            Boolean IsAlreadyPaid = true;
            String RequestType = "O"; // OR S. Can be used interchangeably practically.
            String Key = PortalApplication.PGKey;
            String IV = PortalApplication.PGIV;
            String MerchantCode = PortalApplication.PGMerchantCode;
            String CurrencyCode = PortalApplication.PGCurrencyCode;
            String ReturnURL = PortalApplication.PGReturnURL;

            IIIBL.URN objURN = null;
            //IIIBL.BatchMgmt objBatchMgmt = null;

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = "";
            String strAmount = Amount.ToString(nfi);
            try
            {
                //if (NSEITReferenceNumber.Trim() == String.Empty) //Will be empty only during 1st attempt.
                //{
                //    IsAlreadyPaid = false;
                //}
                //else
                //{
                String response = "";
                RequestURL objRequestURL = new RequestURL();

                if (RequestType.ToUpper() == "T" || RequestType.ToUpper() == "S" || RequestType.ToUpper() == "O" || RequestType.ToUpper() == "R")
                {
                    response = objRequestURL.SendRequest
                                (
                                RequestType,
                                MerchantCode,
                                NSEITReferenceNumber,
                                TransactionId,
                                strAmount,
                                CurrencyCode,
                                String.Empty,
                                ReturnURL,
                                String.Empty,
                                String.Empty,
                                String.Format("Test_{0}_0.0", strAmount),
                                PaymentDate.ToString("dd-MM-yyyy"),
                                String.Empty,
                                String.Empty,
                                String.Empty,
                                String.Empty,
                                String.Empty,
                                String.Empty,
                                Key,
                                IV
                                );
                }
                String strResponse = response.ToUpper();
                String[] strSplitDecryptedResponse = strResponse.Split('|');
                String TXN_STATUS = String.Empty;
                String CLNT_TXN_REF = String.Empty;
                String TPSL_TXN_ID = String.Empty;
                String TXN_AMT = String.Empty;
                foreach (String str in strSplitDecryptedResponse)
                {
                    string[] KeyValuePair = str.Split('=');
                    switch (KeyValuePair[0].ToUpper())
                    {
                        case "TXN_STATUS":
                            TXN_STATUS = KeyValuePair[1];
                            break;
                        case "CLNT_TXN_REF":
                            CLNT_TXN_REF = KeyValuePair[1];
                            break;
                        case "TPSL_TXN_ID":
                            TPSL_TXN_ID = KeyValuePair[1];
                            break;
                        case "TXN_AMT":
                            TXN_AMT = KeyValuePair[1];
                            break;
                        default:
                            break;
                    }
                }

                IsAlreadyPaid = (TXN_STATUS == "0300");
                ViewBag.NSEITTxnsId = CLNT_TXN_REF;
                ViewBag.PGTxnsId = TPSL_TXN_ID;
                ViewBag.Amount = TXN_AMT;
                ViewBag.StatusCode = TXN_STATUS;
                //Update the database
                try
                {
                    objURN = new IIIBL.URN();
                    objURN.UpdatePaymentStatus(PortalApplication.ConnectionString, TransactionId, NSEITReferenceNumber, TPSL_TXN_ID, TXN_STATUS, strResponse);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                }
                finally
                {
                    objURN = null;
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                //objBatchMgmt = null;
            }
            return IsAlreadyPaid;
        }

        [HttpPost]
        public ActionResult TPSL_PGResponse()
        {
            IIIBL.URN objURN = null;
            try
            {
                String strPGResponse = Request["msg"].ToString();  //Reading response of PG

                if (strPGResponse != "" || strPGResponse != null)
                {
                    RequestURL objRequestURL = new RequestURL();    //Creating Object of Class DotNetIntegration_1_1.RequestURL
                    string strDecryptedVal = null;                  //Decrypting the PG response

                    string strIsKey = PortalApplication.PGKey;
                    string strIsIv = PortalApplication.PGIV;

                    strDecryptedVal = objRequestURL.VerifyPGResponse(strPGResponse, strIsKey, strIsIv);
                    Errorlogger.LogInfo("Payments", "PGResponse", strDecryptedVal);
                    if (strDecryptedVal.StartsWith("ERROR"))
                    {
                        ViewBag.ShowTable = false;
                        ViewBag.Status = strDecryptedVal;
                    }
                    else
                    {
                        String[] strSplitDecryptedResponse = strDecryptedVal.Split('|');
                        String TXN_STATUS = String.Empty;
                        String CLNT_TXN_REF = String.Empty;
                        String TransactionId = String.Empty;
                        String TPSL_TXN_ID = String.Empty;
                        String TXN_AMT = String.Empty;
                        foreach (String str in strSplitDecryptedResponse)
                        {
                            string[] KeyValuePair = str.Split('=');
                            switch (KeyValuePair[0].ToUpper())
                            {
                                case "TXN_STATUS":
                                    TXN_STATUS = KeyValuePair[1];
                                    break;
                                case "CLNT_TXN_REF":
                                    CLNT_TXN_REF = KeyValuePair[1];
                                    break;
                                case "TPSL_TXN_ID":
                                    TPSL_TXN_ID = KeyValuePair[1];
                                    break;
                                case "TXN_AMT":
                                    TXN_AMT = KeyValuePair[1];
                                    break;
                                case "CLNT_RQST_META":
                                    TransactionId = KeyValuePair[1];
                                    TransactionId = TransactionId.Replace("{itc:", "").Replace("}", "");
                                    break;
                                default:
                                    break;
                            }
                        }

                        //Update the database
                        try
                        {
                            objURN = new IIIBL.URN();
                            objURN.UpdatePaymentStatus(PortalApplication.ConnectionString, TransactionId, CLNT_TXN_REF, TPSL_TXN_ID, TXN_STATUS, strDecryptedVal );
                        }
                        catch (Exception ex)
                        {
                            Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                        }
                        finally
                        {
                            objURN = null;
                        }

                        ViewBag.NSEITTxnsId = CLNT_TXN_REF;
                        ViewBag.PGTxnsId = TPSL_TXN_ID;
                        ViewBag.Amount = TXN_AMT;
                        ViewBag.StatusCode = TXN_STATUS;
                        if (TXN_STATUS == "0300")
                        {
                            ViewBag.Status = "Payment Successful.";
                            ViewBag.ShowTable = true;
                        }
                        else
                        {
                            ViewBag.Status = "Transaction Failed.";
                            ViewBag.ShowTable = false;
                        }
                        //update database.
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Error Occured.";
            }
            return View("PGResponse");
        }

        //This is intermediate page as needed by the T type request.
        [HttpGet]
        public ActionResult PG2()
        {//Put some additional validations here
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

    }
}