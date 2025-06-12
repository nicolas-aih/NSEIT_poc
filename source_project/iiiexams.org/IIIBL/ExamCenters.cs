using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IIIBL
{
    public class ExamCenters
    {
        public DataSet FindNearestExamCenter(System.String ConnectionString, System.Int32 Pincode, Int32 ExamBodyId = 5 /*for nseit - III online exams*/)
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.FindNearestExamCenter(ConnectionString, Pincode, ExamBodyId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        public DataSet GetExamCenters(System.String ConnectionString, System.Int32 Pincode, Int32 ExamBodyId = 5 /*for nseit - III online exams*/)
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.GetExamCenters(ConnectionString, Pincode, ExamBodyId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        public DataSet ExamCentersForState(System.String ConnectionString, System.Int32 StateId, Boolean IsTbxCenter)
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.ExamCentersForState(ConnectionString, StateId, IsTbxCenter);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        ///Methods for Exam Center Master Data Maintenance
        public DataSet ExamCentersForState(System.String ConnectionString, System.Int32 StateId, Int32 CenterId )
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.ExamCentersForState(ConnectionString, StateId, CenterId );
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        public DataSet ExamCentersForStateEx(System.String ConnectionString, System.Int32 StateId, Int32 CenterId)
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.ExamCentersForStateEx(ConnectionString, StateId, CenterId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        public DataSet ExamCentersForDownload(System.String ConnectionString)
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.ExamCentersForDownload(ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        public DataSet SimilarExamCenters  (System.String ConnectionString, Int32 CenterId)
        {
            IIIDL.ExamCenters objExamCenters = null;
            DataSet objDataSet = null;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                objDataSet = objExamCenters.SimilarExamCenters(ConnectionString, CenterId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return objDataSet;
        }

        public String SaveCenterDetails(System.String ConnectionString, System.Int32 CenterId, System.String CenterName, System.String CenterCode, Int32 CSSCode, System.Int32 ExamBody, System.String AddressLine1, System.String AddressLine2, System.String AddressLine3, System.Int32 DistrictId, System.Int32 Pincode, System.Boolean IsActive, String CenterType, System.Int32 CurrentUser)
        {
            IIIDL.ExamCenters objExamCenters = null;
            String Message = String.Empty;
            try
            {
                objExamCenters = new IIIDL.ExamCenters();
                Message = objExamCenters.SaveCenterDetails(ConnectionString, CenterId, CenterName, CenterCode, CSSCode, ExamBody, AddressLine1, AddressLine2, AddressLine3, DistrictId, Pincode, IsActive, CenterType, CurrentUser);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamCenters = null;
            }
            return Message;
        }
    }
}
