using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using IIIBL;
using System.Configuration;
using System.Text;

namespace IIIRegistrationPortal2
{
    public static class PortalApplication
    {
        public static DataTable Countries
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["Countries"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["Countries"] = value;
            }
        }
        public static DataTable States {
            get
            {
                return ((DataTable)HttpContext.Current.Application["States"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["States"] = value;
            }
        }
        public static DataTable Area
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["Area"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["Area"] = value;
            }
        }
        public static DataTable ExamBody
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["ExamBody"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["ExamBody"] = value;
            }
        }
        public static DataTable ExamLanguage
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["ExamLanguage"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["ExamLanguage"] = value;
            }
        }

        public static DataTable BasicQualification
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["BasicQualification"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["BasicQualification"] = value;
            }
        }
        public static DataTable ProQualification
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["ProQualification"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["ProQualification"] = value;
            }
        }

        public static DataTable BasicQualificationSpecific
        {
            get
            {
                using (DataView dv = new DataView(BasicQualification))
                {
                    dv.RowFilter = String.Format("role_code='{0}'", PortalSession.RoleCode);
                    return dv.ToTable();
                }
            }
        }
        public static DataTable ProQualificationSpecific
        {
            get
            {
                using (DataView dv = new DataView(ProQualification))
                {
                    dv.RowFilter = String.Format("role_code='{0}'", PortalSession.RoleCode);
                    return dv.ToTable();
                }
            }
        }

        public static DataTable CasteCategory
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["CasteCategory"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["CasteCategory"] = value;
            }
        }
        public static DataTable InsuranceType
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["InsuranceTypeAgents"]).Copy();
            }
            set
            {
                HttpContext.Current.Application["InsuranceTypeAgents"] = value;
            }
        }
        public static DataTable InsuranceTypeSpecific
        {
            get
            {
                using (DataView dv = new DataView(InsuranceType))
                {
                    dv.RowFilter = String.Format("role_code='{0}'", PortalSession.RoleCode);
                    return dv.ToTable();
                }
            }
        }

        public static DataTable Salutation
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["Salutation"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["Salutation"] = value;
            }
        }
        public static DataTable Gender
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["Gender"]).Copy();
            }
            private set
            {
                HttpContext.Current.Application["Gender"] = value;
            }
        }

        public static DataTable CompanyType
        {
            get { return ((DataTable)HttpContext.Current.Application["CompanyType"]).Copy(); }
            private set { HttpContext.Current.Application["CompanyType"] = value; }
        }

        public static DataTable CORType
        {
            get { return ((DataTable)HttpContext.Current.Application["CORType"]).Copy(); }
            private set { HttpContext.Current.Application["CORType"] = value; }
        }
        public static DataTable CORTypeSpecific
        {
            get
            {
                using (DataView dv = new DataView(CORType))
                {
                    dv.RowFilter = String.Format("role_code='{0}'", PortalSession.RoleCode);
                    return dv.ToTable();
                }
            }
        }

        //public static Boolean IsLive
        //{
        //    get; private set;
        //}

        public static String OAIMSConnectionString
        {
            get;
            private set;
        }
        public static String CSSConnectionString
        {
            get;
            private set;
        }
        public static String ConnectionString
        {
            get; private set;
        }
        public static String ConnectionStringReports
        {
            get; private set;
        }

        public static Int32 CSSClientId
        {
            get;
            private set;
        }
        public static Int32 ExamDuration
        {
            get;
            private set;
        }
        public static String IntegrationMode
        {
            get;
            private set;
        }

        public static String PGKey
        {
            get; set;
        }
        public static String PGIV
        {
            get; set;
        }
        public static String PGMerchantCode
        {
            get; set;
        }
        public static String PGCurrencyCode
        {
            get; set;
        }
        public static String PGReturnURL
        {
            get; set;
        }

        public static Byte[] AKey
        {
            get; set;
        }
        public static Byte[] AIV
        {
            get; set;
        }

        public static DataTable Applicant
        {
            get { return ((DataTable)HttpContext.Current.Application["Applicant"]).Copy(); }
            private set { HttpContext.Current.Application["Applicant"] = value; }
        }
        public static DataTable ApplicantTraining
        {
            get { return ((DataTable)HttpContext.Current.Application["ApplicantTraining"]).Copy(); }
            private set { HttpContext.Current.Application["ApplicantTraining"] = value; }
        }
        public static DataTable ApplicantExamination
        {
            get { return ((DataTable)HttpContext.Current.Application["ApplicantExamination"]).Copy(); }
            private set { HttpContext.Current.Application["ApplicantExamination"] = value; }
        }


        public static void LoadMasterData()
        {
            IIIBL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
                CSSConnectionString = ConfigurationManager.AppSettings.Get("CSSConnectionString");
                OAIMSConnectionString= ConfigurationManager.AppSettings.Get("OAIMSConnectionString");
                ConnectionStringReports = ConfigurationManager.AppSettings.Get("ConnectionStringReports");
                PGKey = ConfigurationManager.AppSettings.Get("PGKey");
                PGIV = ConfigurationManager.AppSettings.Get("PGIV");
                PGMerchantCode = ConfigurationManager.AppSettings.Get("PGMerchantCode");
                PGCurrencyCode = ConfigurationManager.AppSettings.Get("PGCurrencyCode");
                PGReturnURL = ConfigurationManager.AppSettings.Get("PGReturnURL");

                CSSClientId = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CSSClientId"));
                ExamDuration = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ExamDuration"));
                //CSSIntegration = ConfigurationManager.AppSettings.Get("CSSIntegration") == "Y";

                //IsLive = ConfigurationManager.AppSettings.Get("IsLive") == "Y";

                AKey = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
                AIV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());

                objMasterData = new MasterData();
                objDataSet = objMasterData.GetMasterData(ConnectionString);

                Countries = objDataSet.Tables[0];
                States = objDataSet.Tables[1];
                Area = objDataSet.Tables[2];
                ExamBody = objDataSet.Tables[3];
                ExamLanguage = objDataSet.Tables[4];

                BasicQualification = objDataSet.Tables[5];
                ProQualification = objDataSet.Tables[6];

                CasteCategory = objDataSet.Tables[7];

                InsuranceType = objDataSet.Tables[8];
                Salutation = objDataSet.Tables[9];
                Gender = objDataSet.Tables[10];
                Districts = objDataSet.Tables[11];
                CompanyType = objDataSet.Tables[12];
                CORType = objDataSet.Tables[13];

                Applicant = objDataSet.Tables[14];
                ApplicantTraining = objDataSet.Tables[15];
                ApplicantExamination = objDataSet.Tables[16];

                try
                {
                    DataTable dataTable = objDataSet.Tables[17];
                    IntegrationMode = Convert.ToString(dataTable.Rows[0]["integration_mode"]);
                }
                catch ( Exception ex1)
                {
                    IntegrationMode = "OAIMS"; //Default to regular mode.
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                objMasterData = null;
            }
        }
        public static DataTable GetDistrictsForStates(Int32 StateId)
        {
            DataTable objDataTable = PortalApplication.Districts;
            objDataTable.DefaultView.RowFilter = String.Format("state_id = {0}", StateId);
            objDataTable = objDataTable.DefaultView.ToTable();
            return objDataTable;
        }
        public static DataTable Districts
        {
            get
            {
                return ((DataTable)HttpContext.Current.Application["Districts"]).Copy();
            }
            set
            {
                HttpContext.Current.Application["Districts"] = value;
            }
        }

        public static String ReportsDump
        {
            get { return "../ReportsDump"; }
        }
    }
}