using Databases.SQLServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIIDL
{
    public class Notifications
    {
        public int SaveNotification(System.String ConnectionString, System.String Notificationtext, System.DateTime Fromdate, System.DateTime Todate, System.String Attachmentfile, System.String Displayca, System.String Displaywa, System.String Displayimf, System.String Displaybr, System.String Displayi,System.String Notificationcaption ,System.String Displayothers, String HaltDisplay, Int32 NotificationId = -1)
        {
            Databases.SQLServer.Database objDatabase = null;
           // DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "SP_add_new_notification";
                String[] Params = new String[] { "@fromDate", "@toDate", "@notificationText", "@attachmentFile", "@displayCA", "@displayWA", "@displayIMF", "@displayBR", "@displayI", "@displayOthers" , "@halt_display", "@NotificationCaption", "@NotificationId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(8, 23, 3), new ParamLength(8, 23, 3), new ParamLength(4000, 0, 0), new ParamLength(255, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(1, 0, 0), new ParamLength(2000, 0, 0), new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { Fromdate, Todate, Notificationtext, Attachmentfile, Displayca, Displaywa, Displayimf, Displaybr, Displayi, Displayothers, HaltDisplay , Notificationcaption, NotificationId <= 0 ? (Object)DBNull.Value : NotificationId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                //Comment the below line if the procedure return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, true);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return ProcReturnValue;
        }

        public DataSet GetAllNotifications(System.String ConnectionString, Int32 NotificationId = -1 )
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_all_notifications";
                String[] Params = new String[] { "@notification_id" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.Int };
                ParamLength[] ParamLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input };
                Object[] Values = new Object[] { NotificationId <=0? (Object)DBNull.Value : NotificationId };
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }

        public DataSet GetTBXSchedule(System.String ConnectionString)
        {
            Databases.SQLServer.Database objDatabase = null;
            DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            try
            {
                System.String ProcedureName = "sp_get_tbx_schedule";
                String[] Params = new String[] {};
                SqlDbType[] ParamTypes = new SqlDbType[] {};
                ParamLength[] ParamLengths = new ParamLength[] {};
                ParameterDirection[] ParamDirections = new ParameterDirection[] {};
                Object[] Values = new Object[] {};
                objDatabase = new Database();
                //Comment the below line if the procedure does not return data set.
                ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet, true);
                //Comment the below line if the procedure return data set.
                //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDatabase = null;
            }
            //Comment the below line if the procedure does not return data set.
            return objDataSet;
        }
    }
}
