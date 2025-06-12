using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IIIBL
{
   public class Notifications
    {
        public int SaveNotification(System.String ConnectionString, System.String Notificationtext, System.DateTime Fromdate, System.DateTime Todate, System.String Attachmentfile, System.String Displayca, System.String Displaywa, System.String Displayimf, System.String Displaybr, System.String Displayi, System.String Notificationcaption, System.String Displayothers, String HaltDisplay, Int32 NotificationId = -1)
        {
            IIIDL.Notifications obj = null;
            // DataSet objdataset = null;
            int  Intsucess = 0;

            try
            {
                obj = new IIIDL.Notifications();
                Intsucess = obj.SaveNotification(ConnectionString ,Notificationtext, Fromdate, Todate, Attachmentfile, Displayca, Displaywa, Displayimf, Displaybr, Displayi, Notificationcaption, Displayothers, HaltDisplay, NotificationId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return Intsucess;
        }

        public DataSet GetAllNotifications(System.String ConnectionString,Int32 NotificationId = -1)
        {
            IIIDL.Notifications obj = null;
            DataSet objDataSet = null;
            try
            {
                obj = new IIIDL.Notifications();
                objDataSet = obj.GetAllNotifications(ConnectionString, NotificationId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return objDataSet;
        }

        public DataSet GetTBXSchedule(System.String ConnectionString)
        {
            IIIDL.Notifications obj = null;
            DataSet objDataSet = null;
            try
            {
                obj = new IIIDL.Notifications();
                objDataSet = obj.GetTBXSchedule(ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return objDataSet;
        }
    }
}
