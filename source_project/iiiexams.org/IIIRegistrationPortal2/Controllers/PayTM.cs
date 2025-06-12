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
using Paytm; //??
using Newtonsoft.Json;
using System.Net; 
using Newtonsoft.Json.Linq; 


namespace IIIRegistrationPortal2.Controllers
{
    public partial class PaymentsController : Controller
    {
        [HttpPost]
        public ActionResult PGRequestPayTM(String TransactionId, Decimal Amount)
        {

            String ViewName = String.Empty;
            string TransUrl = "";
            APITrans apibody = new APITrans();
            APIResponse apiresponse = new APIResponse();
            string key = "";
            try
            {

                PaymentConstant constant = new PaymentConstant();
                constant.GetData();

                apibody = new APITrans();
                apibody.body = new APIBody();
                apibody.head = new APIHead();
                key = PaymentConstant.key;


                apibody.body.txnAmount.value = Amount;


                IIIBL.BatchMgmt objBatchMgmt = null;
                String NSEITReferenceNumber = String.Empty;
                DateTime PaymentDate = DateTime.Now;
                //long PGId = 0;

                TransUrl = PaymentConstant.TransUrl;

                objBatchMgmt = new BatchMgmt();
                objBatchMgmt.UpdatePaymentAttempt(PortalApplication.ConnectionString, TransactionId, "PAYTM", out NSEITReferenceNumber, out PaymentDate);

                string order = apibody.body.orderId;
                apibody.body.orderId = NSEITReferenceNumber;
               

                string signature = Checksum.generateSignature(JsonConvert.SerializeObject(apibody.body), key);
                apibody.head.signature = signature;
                string postData = JsonConvert.SerializeObject(apibody);


                String response = "";
                RequestURL objRequestURL = new RequestURL();

                String strResponse = response.ToUpper();

                bool IsValid = false;
                //requesting to paytm payment gateway
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(TransUrl + "theia/api/v1/initiateTransaction?mid=" + apibody.body.mid + "&orderId=" + apibody.body.orderId);

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = postData.Length;

                using (StreamWriter requestWriter2 = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter2.Write(postData);
                }

                string responseData = string.Empty;

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    apiresponse = JsonConvert.DeserializeObject<APIResponse>(responseData);
                }

                JObject jObject = JObject.Parse(responseData);

                //reading paytm response
                if (jObject.SelectToken("body.resultInfo.resultStatus").Value<string>() == "S")
                {
                    string displayName = jObject.SelectToken("body.txnToken").Value<string>();
                    ViewBag.vbOrderId = apibody.body.orderId;
                    ViewBag.vbTraToken = displayName;
                    txnAmountObject tx = new txnAmountObject();
                    ViewBag.vbTraValue = Amount; //tx.value;
                    ViewBag.vbMid = apibody.body.mid.ToString();
                    ViewBag.vbAmount = Amount;
                    ViewName = "PaytmReceivePayment";
                }
                else
                {
                    ViewBag.Status = "Error Occured";
                    ViewBag.ShowTable = false;
                    ViewName = "PGResponse";
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }

            return View(ViewName);
        }

        [HttpPost]
        public ActionResult PaytmPGResponse()
        {
            IIIBL.URN objURN = null;
            try
            {
                Dictionary<string, string> postData = new Dictionary<string, string>();
                string tableContent = "";
                string strDecryptedVal = "";

                string[] keys = Request.Form.AllKeys;
                if (Request.Form.Keys.Count > 0)
                {
                    for (int i = 0; i < keys.Length; i++)
                    {
                        postData.Add(keys[i], Request.Form[keys[i]]);
                    }

                    strDecryptedVal = Request.Form.ToString();

                    var getcheksum = postData["CHECKSUMHASH"];
                    var TxnResp = postData["RESPMSG"];
                    postData.Remove("CHECKSUMHASH");
                    var checksignature = Checksum.verifySignature(postData, PaymentConstant.key, getcheksum);
                    String Status = Request["STATUS"].ToString();  //Reading response of PG
                    if (checksignature && Status == "TXN_SUCCESS")
                    {
                        String TXN_STATUS = Request["STATUS"].ToString();
                        string NSEITReferenceNumber = Request["ORDERID"].ToString();
                        String TXNID = Request["TXNID"].ToString();
                        String TXNAMOUNT = Request["TXNAMOUNT"].ToString();
                        String TXNDATE = Request["TXNDATE"].ToString();
                        String TransactionId = NSEITReferenceNumber.Split('-')[0]; ;

                        //Update the database
                        try
                        {
                            objURN = new IIIBL.URN();
                            objURN.UpdatePaymentStatus(PortalApplication.ConnectionString, TransactionId, NSEITReferenceNumber, TXNID, TXN_STATUS, strDecryptedVal);
                        }
                        catch (Exception ex)
                        {
                            Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                        }
                        finally
                        {
                            objURN = null;
                        }

                        ViewBag.NSEITTxnsId = TransactionId;
                        ViewBag.PGTxnsId = TXNID;
                        ViewBag.Amount = TXNAMOUNT;
                        ViewBag.vbPaymentGateway = "Paytm";
                        ViewBag.StatusCode = Status;
                        ViewBag.Status = "Payment Successful.";
                        ViewBag.ShowTable = true;
                    }
                    else
                    {
                        ViewBag.Status = "Transaction Failed.";
                        ViewBag.ShowTable = false;
                    }
                }
                else
                {
                    ViewBag.Status = "Transaction Failed.";
                    ViewBag.ShowTable = false;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Error Occured.";
            }
            return View("PGResponse");
        }

        private bool PaytmPaymentStatusCheck(String TransactionId, Decimal Amount, String NSEITReferenceNumber, DateTime PaymentDate)
        {
            String ViewName = String.Empty;
            string TransUrl = "";
            APITrans apibody = new APITrans();
            APIResponse apiresponse = new APIResponse();
            string key = "";
            IIIBL.URN objURN = null;
            Boolean IsAlreadyPaid = true;

            try
            {

                PaymentConstant constant = new PaymentConstant();
                constant.GetData();

                apibody = new APITrans();
                apibody.body = new APIBody();

                apibody.head = new APIHead();
                key = PaymentConstant.key;


                apibody.body.txnAmount.value = Amount;


                string order = apibody.body.orderId;
                apibody.body.orderId = NSEITReferenceNumber;


                string signature = Checksum.generateSignature(JsonConvert.SerializeObject(apibody.body), key);
                apibody.head.signature = signature;
                string postData = JsonConvert.SerializeObject(apibody);
                TransUrl = PaymentConstant.TransUrl;

                String response = "";
                RequestURL objRequestURL = new RequestURL();

                String strResponse = response.ToUpper();

                bool IsValid = false;
                //requesting to paytm payment gateway
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(TransUrl + "v3/order/status");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = postData.Length;

                using (StreamWriter requestWriter2 = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter2.Write(postData);
                }
                string responseData = string.Empty;

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();

                }

                JObject jObject = JObject.Parse(responseData);

                //reading paytm response
                if (jObject.SelectToken("body.resultInfo.resultStatus").Value<string>() == "TXN_SUCCESS")
                {
                    IsAlreadyPaid = true;
                    String TXN_STATUS = jObject.SelectToken("body.resultInfo.resultStatus").Value<string>();
                    NSEITReferenceNumber = jObject.SelectToken("body.orderId").Value<string>();
                    String TXNID = jObject.SelectToken("body.txnId").Value<string>();
                    String TXNAMOUNT = jObject.SelectToken("body.txnAmount").Value<string>();
                    String TXNDATE = jObject.SelectToken("body.txnDate").Value<string>();
                    TransactionId = NSEITReferenceNumber.Split('-')[0];
                    string strDecryptedVal = jObject.ToString();

                    //Update the database
                    try
                    {
                        objURN = new IIIBL.URN();
                        objURN.UpdatePaymentStatus(PortalApplication.ConnectionString, TransactionId, NSEITReferenceNumber, TXNID, TXN_STATUS, strDecryptedVal);
                    }
                    catch (Exception ex)
                    {
                        Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    }
                    finally
                    {
                        objURN = null;
                    }

                    ViewBag.NSEITTxnsId = TransactionId;
                    ViewBag.PGTxnsId = TXNID;
                    ViewBag.Amount = TXNAMOUNT;
                    ViewBag.vbPaymentGateway = "Paytm";
                    ViewBag.StatusCode = TXN_STATUS;
                    ViewBag.Status = "Payment Successful.";
                    ViewBag.ShowTable = true;
                }
                else
                {
                    IsAlreadyPaid = false;
                }
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }

            return IsAlreadyPaid;

        }
    }
}