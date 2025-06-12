using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIIBL
{
    public class Utility
    {
        public void SaveCandidateProfile(System.String ConnectionString, System.String UpdatedValue, System.String URN, Int32 UserId, System.String UpdateAction, out System.String Message)
        {
            IIIDL.Utility objURN = null;
            try
            {
                objURN = new IIIDL.Utility();
                objURN.SaveCandidateProfile(ConnectionString, UpdatedValue, URN, UserId, UpdateAction, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
        }

        public DataSet GetCompanyDetails(System.String ConnectionString, System.String CompanyName, out System.String Message)
        {
            IIIDL.Utility objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.Utility();
                objDataSet = objURN.GetCompanyDetails(ConnectionString, CompanyName, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public String GetUserPassword(System.String ConnectionString, System.String UserLoginId, out System.String Password)
        {
            IIIDL.Utility objUsers = null;
            String Message = String.Empty;
            try
            {
                objUsers = new IIIDL.Utility();
                Message = objUsers.GetUserPassword(ConnectionString, UserLoginId, out Password);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objUsers = null;
            }
            return Message;
        }

    }
}
