using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIIDL;

namespace IIIBL
{
    public class DPRangeMst
    {
        public DataSet GetDPRangeList(System.String ConnectionString, System.Int32 Intdprangeid)
        {
            IIIDL.DPRangeMst objGetDPRangeList = null;
            DataSet objdataset = null;

            try
            {
                objGetDPRangeList = new IIIDL.DPRangeMst();
                objdataset = objGetDPRangeList.GetDPRangeList(ConnectionString, Intdprangeid);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objGetDPRangeList = null;
            }
            return objdataset;
        }

        public DataSet GetInsurerName(System.String ConnectionString, System.Int32 Inttblmstinsureruserid)
        {
            IIIDL.DPRangeMst objGetDPRangeList = null;
            DataSet objDataSet = null;
            try
            {
                objGetDPRangeList = new IIIDL.DPRangeMst();
                objDataSet = objGetDPRangeList.GetInsurerName(ConnectionString, Inttblmstinsureruserid);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objGetDPRangeList = null;
            }
            return objDataSet;
        }


        public String SaveDPRange(System.String ConnectionString, System.Int32 Intdprangeid, System.Int32 Insurercode, Int32 dpCount, System.Int32 Intcreatedby)
        {
            IIIDL.DPRangeMst objGetDPRangeList = null;
            //DataSet objdataset = null;
            String Message;
            try
            {
                objGetDPRangeList = new IIIDL.DPRangeMst();
                Message = objGetDPRangeList.SaveDPRange(ConnectionString, Intdprangeid, Insurercode, dpCount, Intcreatedby);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objGetDPRangeList = null;
            }
            return Message;
        }
    }
}
