using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;

namespace IIIBL
{
    public class MasterData
    {
        public DataSet GetStates(System.String ConnectionString)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetStates(ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetNotifications(System.String ConnectionString, String NotificationType /*T-icker or N-otification panel*/, String RoleCode)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetNotifications(ConnectionString, NotificationType, RoleCode);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetMasterData(System.String ConnectionString)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetMasterData(ConnectionString );
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetCenterListForDistrict(System.String ConnectionString, Int32 DistrictId)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetCenterListForDistrict(ConnectionString, DistrictId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetInsurers(System.String ConnectionString, Int32 InsurerId)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetInsurers(ConnectionString, InsurerId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetInsurers2(System.String ConnectionString, Int32 InsurerId)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetInsurers2(ConnectionString, InsurerId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetDPForInsurer(String ConnectionString, Int32 InsurerId, Int32 DPId)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetDPForInsurer(ConnectionString, InsurerId, DPId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }
        public DataSet GetACForDPs(String ConnectionString, Int32 InsurerId, Int32 DPId, Int32 ACId)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetACForDPs(ConnectionString, InsurerId, DPId, ACId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public DataSet GetExamCenter(System.String ConnectionString, System.Int16 ExamBodyId, System.Int16 ExamCenterId)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetExamCenter(ConnectionString, ExamBodyId, ExamCenterId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }

        public Int32 GetNewDPId(System.String ConnectionString, System.Int32 InsurerUserId)
        {
            IIIDL.MasterData objMasterData = null;
            Int32 DPId = -1;
            try
            {
                objMasterData = new IIIDL.MasterData();
                DPId = objMasterData.GetNewDPId(ConnectionString, InsurerUserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return DPId;
        }

        public DataSet GetCompanyListForVoucherEntry(System.String ConnectionString, String CompanyType)
        {
            IIIDL.MasterData objMasterData = null;
            DataSet objDataSet = null;
            try
            {
                objMasterData = new IIIDL.MasterData();
                objDataSet = objMasterData.GetCompanyListForVoucherEntry(ConnectionString, CompanyType);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objMasterData = null;
            }
            return objDataSet;
        }
    }
}
