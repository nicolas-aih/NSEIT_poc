using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIIBL
{
    public class ReportsInfra
    {
        public String SaveReportRequest(System.String ConnectionString, Int64 CompanyId, String ReportType, String ReportParameters)
        {
            IIIDL.ReportsInfra obj = null;
            String Message = String.Empty;
            try
            {
                obj = new IIIDL.ReportsInfra();
                Message = obj.SaveReportRequest(ConnectionString, CompanyId, ReportType, ReportParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return Message;
        }

        public DataSet GetReportRequests(System.String ConnectionString, System.Int32 InsurerUserID)
        {
            IIIDL.ReportsInfra obj = null;
            DataSet objDataSet = null;
            try
            {
                obj = new IIIDL.ReportsInfra();
                objDataSet = obj.GetReportRequests(ConnectionString, InsurerUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                obj = null;
            }
            return objDataSet;
        }

        //Future Methods
        //GetReportConfig
        //SaveReportConfig
    }
}
