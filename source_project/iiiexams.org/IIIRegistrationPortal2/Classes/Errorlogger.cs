using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace IIIRegistrationPortal2
{
    public class Errorlogger
    {
        static String _Directory = String.Empty;
        static Boolean _EnableLogging = false;
        public static void Initialize(String TargetDirectory, Boolean EnableLogging =  false)
        {
            DirectoryInfo di = new DirectoryInfo(TargetDirectory);
            if (!di.Exists)
            {
                throw new Exception("Log target directory does not exists");
            }
            _EnableLogging = EnableLogging;
            _Directory = TargetDirectory;
        }

        public static void LogError (String Controller, String Action, Exception ex, System.Collections.Specialized.NameValueCollection FormCollection = null)
        {
            StringBuilder sb = null;
            try
            {
                String FileName = _Directory + "\\" + System.DateTime.Now.ToString("ddMMMyyyyHH") + ".txt";
                sb = new StringBuilder();
                sb.AppendLine(String.Empty);
                sb.AppendLine(String.Format("START : {0}/{1} @{2}", Controller, Action, System.DateTime.Now));
                sb.AppendLine("**********************************************************");
                HttpContext httpContext = HttpContext.Current;
                if (httpContext != null)
                {
                    sb.AppendLine("User Agent : " + httpContext.Request.UserAgent);
                }
                sb.AppendLine(ex.ToString());
                sb.AppendLine("**********************************************************");
                
                if (FormCollection != null)
                {
                    sb.AppendLine("Data :-");
                    sb.AppendLine("**********************************************************");
                    foreach (String Key in FormCollection.AllKeys)
                    {
                        sb.AppendLine(String.Format("{0} --- {1}", Key, Convert.ToString(FormCollection[Key])));
                    }
                }
                sb.AppendLine("******************************************** END");
                sb.AppendLine(String.Empty);
                File.AppendAllText(FileName, sb.ToString());
            }
            catch(Exception e)
            {//Eat this one.
                
            }
            finally
            {
                sb.Clear();
                sb = null;
            }
        }

        public static void LogInfo(String Controller, String Action, String Info)
        {
            StringBuilder sb = null;
            try
            {
                String FileName = _Directory + "\\" + System.DateTime.Now.ToString("ddMMMyyyyHH") + ".txt";
                sb = new StringBuilder();
                sb.AppendLine(String.Empty);
                sb.AppendLine(String.Format("START : {0}/{1} @{2}", Controller, Action, System.DateTime.Now));
                sb.AppendLine("**********************************************************");
                sb.AppendLine(Info);
                sb.AppendLine("**********************************************************");
                sb.AppendLine("******************************************** END");
                sb.AppendLine(String.Empty);
                File.AppendAllText(FileName, sb.ToString());
            }
            catch (Exception e)
            {//Eat this one.

            }
            finally
            {
                sb.Clear();
                sb = null;
            }
        }

    }
}