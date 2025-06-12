using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;

namespace IIIBL
{
    public class Telemarketer
    {
        public String RegisterTelemarketer(String ConnectionString, Int32 InsurerUserId, Int32 CreatedByUserId, String TraiRegNo, String Name, String Address, String CpName, String CpEmailId, String CpContactNo, String DpName, String DpEmailId, String DpContactNo, String IsActive)
        {
            IIIDL.Telemarketer objTelemarketer = null;
            String Success = String.Empty;
            try
            {
                objTelemarketer = new IIIDL.Telemarketer();
                Success = objTelemarketer.RegisterTelemarketer(ConnectionString, InsurerUserId, CreatedByUserId, TraiRegNo, Name, Address, CpName, CpEmailId, CpContactNo, DpName, DpEmailId, DpContactNo, IsActive);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objTelemarketer = null;
            }
            return Success;
        }

        public DataSet GetTelemarketerData(System.String ConnectionString, System.Int32 InsurerUserId, System.Int64 TmId, System.Int32 IsActive)
        {
            IIIDL.Telemarketer objTelemarketer = null;
            DataSet objDataSet = null;
            try
            {
                objTelemarketer = new IIIDL.Telemarketer();
                objDataSet = objTelemarketer.GetTelemarketerData(ConnectionString, InsurerUserId, TmId, IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objTelemarketer = null;
            }
            return objDataSet;
        }
        public String UpdateTelemarketer(String ConnectionString, Int32 InsurerUserId, Int32 CreatedByUserId, Int64 TmId, String TraiRegNo, String Name, String Address, String CpName, String CpEmailId, String CpContactNo, String DpName, String DpEmailId, String DpContactNo, String IsActive)
        {
            IIIDL.Telemarketer objTelemarketer = null;
            String Success = String.Empty;
            try
            {
                objTelemarketer = new IIIDL.Telemarketer();
                Success = objTelemarketer.UpdateTelemarketer(ConnectionString, InsurerUserId, CreatedByUserId, TmId, TraiRegNo, Name, Address, CpName, CpEmailId, CpContactNo, DpName, DpEmailId, DpContactNo, IsActive);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objTelemarketer = null;
            }
            return Success;
        }

        public String DeleteTelemarkiter(String ConnectionString, Int32 InsurerUserId, Int32 CreatedByUserId, Int64 TmId)
        {
            IIIDL.Telemarketer objTelemarketer = null;
            String Success = String.Empty;
            try
            {
                objTelemarketer = new IIIDL.Telemarketer();
                Success = objTelemarketer.DeleteTelemarketer(ConnectionString, InsurerUserId, CreatedByUserId, TmId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objTelemarketer = null;
            }
            return Success;
        }
    }
}
