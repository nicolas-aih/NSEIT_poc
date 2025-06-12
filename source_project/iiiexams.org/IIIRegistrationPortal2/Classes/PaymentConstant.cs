using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace IIIRegistrationPortal2
{
    public class PaymentConstant
    {
        public static string mid = "";
        public static string key = "";
        public static string websiteName = "";
        public static string TransUrl = "";
        public void GetData()
        {
            // Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            mid = ConfigurationManager.AppSettings["PAYTM_MID"];
            key = ConfigurationManager.AppSettings["PAYTM_MERCHANT_KEY"];
            websiteName = ConfigurationManager.AppSettings["PAYTM_WEBSITE"];
            TransUrl = ConfigurationManager.AppSettings["PAYTM_ENVIRONMENT"];
        }
    }

    public class txnAmountObject
    {
        public decimal value = 0;
        public string currency = "INR";
    }

    public class userInfoObject
    {
        public string custId = "Cust101";
    }

    public class APIBody
    {
        public string requestType = "Payment";
        public string mid = PaymentConstant.mid;
        public string orderId = Convert.ToString(DateTime.Now.Ticks);
        public string websiteName = PaymentConstant.websiteName;
        //public string test = "test11";
        public txnAmountObject txnAmount = new txnAmountObject();
        public userInfoObject userInfo = new userInfoObject();
        public string callbackUrl = ConfigurationManager.AppSettings["PAYTM_CALLBACKURL"];
    }

    public class APIHead
    {
        public string signature = "";
    }

    public class APIResponse
    {
        public APIHead head { get; set; }
        public APIBody body { get; set; }
    }

    public class APITrans
    {
        public APIHead head { get; set; }
        public APIBody body { get; set; }
    }
}