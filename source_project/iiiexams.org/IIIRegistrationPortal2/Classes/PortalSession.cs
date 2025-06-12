using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;

namespace IIIRegistrationPortal2
{
    public static class PortalSession
    {

        public static Int32 UserID
        {
            get { return HttpContext.Current.Session["UserID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["UserID"]); }
            set { HttpContext.Current.Session.Add("UserID", value); }
        }

        public static long CAid      // please set it ????Used only once for setting
        {
            get { return HttpContext.Current.Session["CAid"] == null ? -1 : Convert.ToInt64(HttpContext.Current.Session["CAid"]); }
            set { HttpContext.Current.Session["CAid"] = value; }
        }

        public static string UserName
        {
            get { return HttpContext.Current.Session["UserName"] == null ? string.Empty : HttpContext.Current.Session["UserName"].ToString(); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }

        public static string UserLoginID
        {
            get { return HttpContext.Current.Session["UserLoginID"] == null ? string.Empty : HttpContext.Current.Session["UserLoginID"].ToString(); }
            set { HttpContext.Current.Session["UserLoginID"] = value; }
        }

        public static string TopUserLoginID
        {
            get { return HttpContext.Current.Session["TopUserLoginID"] == null ? string.Empty : HttpContext.Current.Session["TopUserLoginID"].ToString(); }
            set { HttpContext.Current.Session["TopUserLoginID"] = value; }
        }


        public static Int16 RoleID //????Used only once for setting
        {
            get { return HttpContext.Current.Session["RoleID"] == null ? Convert.ToInt16(0) : Convert.ToInt16(HttpContext.Current.Session["RoleID"]); }
            set { HttpContext.Current.Session["RoleID"] = value; }
        }

        public static string RoleName
        {
            get { return HttpContext.Current.Session["RoleName"] == null ? string.Empty : HttpContext.Current.Session["RoleName"].ToString(); }
            set { HttpContext.Current.Session["RoleName"] = value; }
        }

        public static string RoleCode
        {
            get { return HttpContext.Current.Session["RoleCode"] == null ? string.Empty : HttpContext.Current.Session["RoleCode"].ToString(); }
            set { HttpContext.Current.Session["RoleCode"] = value; }
        }
        

        public static string ResourceServerURL          //????Not in use
        {
            get { return ConfigurationManager.AppSettings.Get("ResourceServer") == null ? string.Empty : ConfigurationManager.AppSettings.Get("ResourceServer"); }
        }

        public static string LocalResourceServerURL     //????Not in use
        {
            get { return ConfigurationManager.AppSettings.Get("LocalResourceServer") == null ? string.Empty : ConfigurationManager.AppSettings.Get("LocalResourceServer"); }
        }

        /*public static string LiveChatURL
        {
            get { return ConfigurationSettings.AppSettings["LiveChatUrl"] == null ? string.Empty : ConfigurationSettings.AppSettings["LiveChatUrl"]; }
        }*/


        public static string ApplicationName  //????Not in use
        {
            get { return "IIIExams.org"; }
        }

        public static Int32 InsurerUserID
        {
            get { return HttpContext.Current.Session["InsurerUserID"] == null ? Convert.ToInt16(0) : Convert.ToInt16(HttpContext.Current.Session["InsurerUserID"]); ; }
            set { HttpContext.Current.Session.Add("InsurerUserID", value); }
        }

        public static Int32 DPUserID
        {
            get { return HttpContext.Current.Session["DPUserID"] == null ? Convert.ToInt16(0) : Convert.ToInt16(HttpContext.Current.Session["DPUserID"]); ; }
            set { HttpContext.Current.Session.Add("DPUserID", value); }
        }

        //public static Int32 PortabilityDPUserID
        //{
        //    get { return HttpContext.Current.Session["PortabilityDPUserID"] == null ? Convert.ToInt16(0) : Convert.ToInt16(HttpContext.Current.Session["PortabilityDPUserID"]); ; }
        //    set { HttpContext.Current.Session.Add("PortabilityDPUserID", value); }
        //}

        public static Int32 AgentCounselorUserID
        {
            get { return HttpContext.Current.Session["AgentCounselorUserID"] == null ? Convert.ToInt16(0) : Convert.ToInt16(HttpContext.Current.Session["AgentCounselorUserID"]); ; }
            set { HttpContext.Current.Session.Add("AgentCounselorUserID", value); }
        }

        public static DateTime LastLoggedInDateTime   //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["LastLoggedInDateTime"] == null ? Convert.ToDateTime(new DateTime(1900, 1, 1)) : Convert.ToDateTime(HttpContext.Current.Session["LastLoggedInDateTime"]); }
            set { HttpContext.Current.Session.Add("LastLoggedInDateTime", value); }
        }

        public static string InsurerName   //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["InsurerName"] == null ? string.Empty : HttpContext.Current.Session["InsurerName"].ToString(); }
            set { HttpContext.Current.Session["InsurerName"] = value; }
        }

        public static String InsurerType   //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["InsurerType"] == null ? String.Empty : Convert.ToString(HttpContext.Current.Session["InsurerType"]); }
            set { HttpContext.Current.Session["InsurerType"] = value; }
        }

        public static string UserEmailID   //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["UserEmailID"] == null ? string.Empty : HttpContext.Current.Session["UserEmailID"].ToString(); }
            set { HttpContext.Current.Session["UserEmailID"] = value; }
        }

        public static string DPName
        {
            get { return HttpContext.Current.Session["DPName"] == null ? string.Empty : HttpContext.Current.Session["DPName"].ToString(); }
            set { HttpContext.Current.Session["DPName"] = value; }
        }

        //public static string PortabilityDPName
        //{
        //    get { return HttpContext.Current.Session["PortabilityDPName"] == null ? string.Empty : HttpContext.Current.Session["PortabilityDPName"].ToString(); }
        //    set { HttpContext.Current.Session["PortabilityDPName"] = value; }
        //}

        public static string AgentCounselorName  //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["AgentCounselorName"] == null ? string.Empty : HttpContext.Current.Session["AgentCounselorName"].ToString(); }
            set { HttpContext.Current.Session["AgentCounselorName"] = value; }
        }

        public static DataTable MenuAccess  //????Not in use only assigned
        {
            get { return (DataTable)HttpContext.Current.Session["MenuAccess"]; }
            set { HttpContext.Current.Session["MenuAccess"] = value; }
        }

        public static string RenderredMenu   //????Not referred anywhere
        {
            get { return HttpContext.Current.Session["RenderredMenu"] == null ? string.Empty : HttpContext.Current.Session["RenderredMenu"].ToString(); }
            set { HttpContext.Current.Session["RenderredMenu"] = value; }
        }

        public static Boolean ShowAlert   //????Not referred anywhere
        {
            get { return HttpContext.Current.Session["ShowAlert"] == null ? false : Convert.ToBoolean(HttpContext.Current.Session["RenderredMenu"]); }
            set { HttpContext.Current.Session["ShowAlert"] = value; }
        }

        public static string ClientMachineIP
        {
            get { return HttpContext.Current.Session["ClientMachineIP"] == null ? string.Empty : HttpContext.Current.Session["ClientMachineIP"].ToString(); }
            set { HttpContext.Current.Session["ClientMachineIP"] = value; }
        }

        public static string InsurerCode  //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["InsurerCode"] == null ? string.Empty : HttpContext.Current.Session["InsurerCode"].ToString(); }
            set { HttpContext.Current.Session["InsurerCode"] = value; }
        }
        public static String InsurerTypeNew
        {
            get { return HttpContext.Current.Session["InsurerTypeNew"] == null ? string.Empty : Convert.ToString(HttpContext.Current.Session["InsurerTypeNew"]); }
            set { HttpContext.Current.Session["InsurerTypeNew"] = value; }
        }

        public static DataTable ParentMenu  //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["ParentMenu"] == null ? null : (DataTable)HttpContext.Current.Session["ParentMenu"]; }
            set { HttpContext.Current.Session["ParentMenu"] = value; }
        }

        public static DataTable ChildMenu  //????Not in use only assigned
        {
            get { return HttpContext.Current.Session["ChildMenu"] == null ? null : (DataTable)HttpContext.Current.Session["ChildMenu"]; }
            set { HttpContext.Current.Session["ChildMenu"] = value; }
        }

        public static Boolean HasAccess(String Controller, String ActionMethod)
        {
            Boolean retval = false;
            DataTable objDataTable = null;
            try
            {
                objDataTable = ChildMenu;
                String searchString = String.Format("{0}/{1}", Controller, ActionMethod);
                foreach (DataRow dr in objDataTable.Rows)
                {
                    if (Convert.ToString(dr["NewURL"]).EndsWith(searchString))
                    {
                        retval = true;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDataTable = null;
            }
            return retval;
        }

        public static byte[] Key
        {
            get { return (byte[])HttpContext.Current.Session["Key"];}
            set { HttpContext.Current.Session["Key"] = value; }
        }

        public static byte[] IV
        {
            get { return (byte[])HttpContext.Current.Session["IV"]; }
            set { HttpContext.Current.Session["IV"] = value; }
        }
    }
}