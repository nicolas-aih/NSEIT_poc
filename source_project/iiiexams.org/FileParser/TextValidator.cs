using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;


namespace Utilities.FileParser
{
    public class TextValidator
    {
        public static String GetPattern(Type type, String Format)
        {
            String strPattern = String.Empty;
            if (type == typeof(System.Int16) || type == typeof(System.Int32) || type == typeof(System.Int64) ||
                type == typeof(System.UInt16) || type == typeof(System.UInt32) || type == typeof(System.UInt64) ||
                type == typeof(System.Decimal) || type == typeof(System.Double) || type == typeof(System.DateTime)
                )
            {
                if (type == typeof(System.Int16) || type == typeof(System.Int32) || type == typeof(System.Int64))
                {
                    strPattern = "^[+-]{0,1}[0-9]{1}[0-9]*$";
                }
                if (type == typeof(System.UInt16) || type == typeof(System.UInt32) || type == typeof(System.UInt64))
                {
                    strPattern = "^[0-9]*$";
                }
                if (type == typeof(System.Decimal) || type == typeof(System.Double))
                {
                    strPattern = "^[+-]{0,1}[0-9]{0,}[.]{0,1}[0-9]*$";
                }
                if (type == typeof(System.DateTime))
                {
                    strPattern = Format;
                    Int32 TimeFmt = 0;
                    if (strPattern.IndexOf("H") == -1 && strPattern.IndexOf("HH") == -1)
                    {
                        TimeFmt = 12;
                    }
                    else
                    {
                        TimeFmt = 24;
                        if (strPattern.IndexOf("tt") != -1)
                        {
                            throw new Exception("Invalid Time Format.");
                        }
                    }

                    //d: date			
                    //M: MONTH
                    //m: minute
                    //Y: YEAR
                    //H: H24
                    //h: h12
                    //s: second
                    //t: AMPM

                    strPattern = strPattern.Replace("d", "<d>");
                    strPattern = strPattern.Replace("M", "<M>");
                    strPattern = strPattern.Replace("m", "<m>");
                    strPattern = strPattern.Replace("Y", "<Y>");
                    strPattern = strPattern.Replace("y", "<y>");
                    strPattern = strPattern.Replace("H", "<H>");
                    strPattern = strPattern.Replace("h", "<h>");
                    strPattern = strPattern.Replace("s", "<s>");
                    strPattern = strPattern.Replace("t", "<t>");

                    //str1 = str1.Replace("dddd","[a-zA-Z]{6,9}");
                    strPattern = strPattern.Replace("<d><d><d><d>", "(sunday|monday|tuesday|wednesday|thursday|friday|saturday)");
                    //str1 = str1.Replace("dd","[0-9]{2}");
                    strPattern = strPattern.Replace("<d><d>", "(01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31)");
                    /////strPattern = strPattern.Replace("<d><d>", "(0[1-9]|[1-2][0-9]|3[0-1])");
                    //str1 = str1.Replace("d","[0-9]{1,2}");
                    strPattern = strPattern.Replace("<d>", "(1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31)");
                    /////strPattern = strPattern.Replace("<d>", "([1-9]|[1-2][0-9]|3[0-1])");

                    //str1 = str1.Replace("MMMM","[a-zA-Z]{3,9}");
                    strPattern = strPattern.Replace("<M><M><M><M>", "(january|february|march|april|may|june|july|august|september|october|november|december)");
                    //str1 = str1.Replace("MMM","[a-zA-Z]{3}");
                    strPattern = strPattern.Replace("<M><M><M>", "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)");

                    //str1 = str1.Replace("MM","[0-9]{2}");
                    strPattern = strPattern.Replace("<M><M>", "(01|02|03|04|05|06|07|08|09|10|11|12)");
                    //str1 = str1.Replace("M","[0-9]{1,2}");
                    strPattern = strPattern.Replace("<M>", "(1|2|3|4|5|6|7|8|9|10|11|12)");

                    strPattern = strPattern.Replace("<y><y><y><y>", "[0-9]{4}");
                    strPattern = strPattern.Replace("<y><y>", "[0-9]{2}");

                    if (TimeFmt == 12)
                    {
                        //str1 = str1.Replace("hh", "[0-9]{2}");		
                        //str1 = str1.Replace("h", "[0-9]{1,2}");
                        strPattern = strPattern.Replace("<h><h>", "(01|02|03|04|05|06|07|08|09|10|11|12)");
                        strPattern = strPattern.Replace("<h>", "(1|2|3|4|5|6|7|8|9|10|11|12)");
                    }
                    else
                    {
                        //str1 = str1.Replace("HH", "[0-9]{2}");		
                        //str1 = str1.Replace("H", "[0-9]{1,2}");
                        strPattern = strPattern.Replace("<H><H>", "(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23)");
                        strPattern = strPattern.Replace("<H>", "(0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23)");
                    }

                    //str1 = str1.Replace("mm","[0-9]{2}");
                    strPattern = strPattern.Replace("<m><m>", "(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)");

                    //str1 = str1.Replace("ss","[0-9]{2}");
                    strPattern = strPattern.Replace("<s><s>", "(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)");

                    if (TimeFmt == 12)
                    {
                        //strPattern = strPattern.Replace("<t><t>","(AM|PM)"); --Usman on 17-Sep-2007
                        strPattern = strPattern.Replace("<t><t>", "(am|pm)");
                    }

                    strPattern = "^" + strPattern + "$";
                }
            }
            else
            {
                throw new Exception("Data Type Is Not Currently Supported");
            }
            return strPattern;
        }
        public static Boolean Validate(String str, String strPattern)
        {
            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //following are not in use...
        public static Boolean ValidateAlpha(String str)
        {
            String strPattern = "^[a-zA-Z]*$";
            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateNumeric(String str)
        {
            String strPattern = "^[0-9]*$";
            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateAlphaNumeric(String str)
        {
            String strPattern = "^[a-zA-Z0-9]*$";
            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateNumber(String str)
        {
            String strPattern = "^[+-]{0,1}[0-9]{1}[0-9]*$";
            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateDecimals(String str)
        {
            String strPattern = "^[+-]{0,1}[0-9]{0,}[.]{0,1}[0-9]*$";

            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                strPattern = "^[+-]{0,}[.]{0,}$";
                r = new Regex(strPattern);
                m = r.Match(str);
                if (m.Success)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateDate(String str, String Format)
        {
            String strPattern = Format;
            str = str.ToLower(); //Usman on 17-Sep-2007

            Int32 TimeFmt = 0;
            if (strPattern.IndexOf("H") == -1 && strPattern.IndexOf("HH") == -1)
            {
                TimeFmt = 12;
            }
            else
            {
                TimeFmt = 24;
                if (strPattern.IndexOf("tt") != -1)
                {
                    throw new Exception("Invalid Time Format.");
                }
            }

            //d: date			
            //M: MONTH
            //m: minute
            //Y: YEAR
            //H: H24
            //h: h12
            //s: second
            //t: AMPM

            strPattern = strPattern.Replace("d", "<d>");
            strPattern = strPattern.Replace("M", "<M>");
            strPattern = strPattern.Replace("m", "<m>");
            strPattern = strPattern.Replace("Y", "<Y>");
            strPattern = strPattern.Replace("y", "<y>");
            strPattern = strPattern.Replace("H", "<H>");
            strPattern = strPattern.Replace("h", "<h>");
            strPattern = strPattern.Replace("s", "<s>");
            strPattern = strPattern.Replace("t", "<t>");

            //str1 = str1.Replace("dddd","[a-zA-Z]{6,9}");
            strPattern = strPattern.Replace("<d><d><d><d>", "(sunday|monday|tuesday|wednesday|thursday|friday|saturday)");
            //str1 = str1.Replace("dd","[0-9]{2}");
            strPattern = strPattern.Replace("<d><d>", "(01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31)");
            //str1 = str1.Replace("d","[0-9]{1,2}");
            strPattern = strPattern.Replace("<d>", "(1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31)");

            //str1 = str1.Replace("MMMM","[a-zA-Z]{3,9}");
            strPattern = strPattern.Replace("<M><M><M><M>", "(january|february|march|april|may|june|july|august|september|october|november|december)");
            //str1 = str1.Replace("MMM","[a-zA-Z]{3}");
            strPattern = strPattern.Replace("<M><M><M>", "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)");

            //str1 = str1.Replace("MM","[0-9]{2}");
            strPattern = strPattern.Replace("<M><M>", "(01|02|03|04|05|06|07|08|09|10|11|12)");
            //str1 = str1.Replace("M","[0-9]{1,2}");
            strPattern = strPattern.Replace("<M>", "(1|2|3|4|5|6|7|8|9|10|11|12)");

            strPattern = strPattern.Replace("<y><y><y><y>", "[0-9]{4}");
            strPattern = strPattern.Replace("<y><y>", "[0-9]{2}");

            if (TimeFmt == 12)
            {
                //str1 = str1.Replace("hh", "[0-9]{2}");		
                //str1 = str1.Replace("h", "[0-9]{1,2}");
                strPattern = strPattern.Replace("<h><h>", "(01|02|03|04|05|06|07|08|09|10|11|12)");
                strPattern = strPattern.Replace("<h>", "(1|2|3|4|5|6|7|8|9|10|11|12)");
            }
            else
            {
                //str1 = str1.Replace("HH", "[0-9]{2}");		
                //str1 = str1.Replace("H", "[0-9]{1,2}");
                strPattern = strPattern.Replace("<H><H>", "(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23)");
                strPattern = strPattern.Replace("<H>", "(0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23)");
            }

            //str1 = str1.Replace("mm","[0-9]{2}");
            strPattern = strPattern.Replace("<m><m>", "(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)");

            //str1 = str1.Replace("ss","[0-9]{2}");
            strPattern = strPattern.Replace("<s><s>", "(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)");

            if (TimeFmt == 12)
            {
                //strPattern = strPattern.Replace("<t><t>","(AM|PM)"); --Usman on 17-Sep-2007
                strPattern = strPattern.Replace("<t><t>", "(am|pm)");
            }

            strPattern = "^" + strPattern + "$";

            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean ValidateDate2(String valuetovalidate, String ExpectedDateformat, String Regex)
        {
            Regex r = new Regex(Regex);
            Match m = r.Match(valuetovalidate.ToLower());
            if (m.Success)
            {
                DateTime dt;
                if (DateTime.TryParseExact( valuetovalidate, ExpectedDateformat, System.Globalization.DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out dt))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //End following are not in use...
        public static Boolean ValidateEMail(String str)
        {
            String strPattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex r = new Regex(strPattern);
            Match m = r.Match(str);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Number <T>
    {
        private T val;
        public Number(T val)
        {
            Type ty = val.GetType();
            if ( ty == typeof(Int16) ||
                    ty == typeof(Int32) ||
                    ty == typeof(Int64) ||
                    ty == typeof(UInt16) ||
                    ty == typeof(UInt32) ||
                    ty == typeof(UInt64) ||
                    ty == typeof(Decimal) ||
                    ty == typeof(Double)
                )
            {
                
            }
            else
            {

            }
        }
    }
}
