using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIIBL
{
    class Common
    {
        public static String regexPincode = "^[1-9][0-9]{5}$";       //"^([1-9][0-9]{5}|[1-9]{1}[0-9]{2}\s[0-9]{3})$"
        public static String regexEmail = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        public static String regexPhone = "";
        public static String regexMobile = "";
        public static String regexNumOnly = "^[0-9]+$";
        public static String regexDecimal2 = @"^[0-9]+\.[0-9]{2}$";
        public static String regexDecimalAny = @"^[0-9]+\.[0-9]+$";
        public static String regexAlphaOnly = "^[A-Za-z]+$";
        public static String regexAlphaOnlyWithSpace = "^[a-zA-Z]+( +[a-zA-Z]+)*$"; //"^[A-Za-z ]+$";
        public static String regexAlphaNumeric = "^[A-Za-z0-9]+$";
        public static String regexAlphaNumericWithSpace = "^[a-zA-Z0-9]+( +[a-zA-Z0-9]+)*$"; //"^[A-Za-z0-9 ]+$";
        public static String regexLowAscii = "^[!-~]+( +[!-~]+)*$"; // "^[ -~]+$";
        public static String regexLowAsciiExt = "^[ -~\t\n\r]+$";
        public static String regexPAN = "^[A-Z]{5}[0-9]{4}[A-Z]$";
        public static String regexURN = "^CAI[0-9]{10}|BAV[0-9]{10}|WAI[0-9]{10}|IMF[0-9]{10}|[A-Z]{4}[0-9]{10}$";
    }
}
