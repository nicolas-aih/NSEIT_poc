using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using sql = Databases.SQLServer;
using System.IO;

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using ora = OraDatabases.OracleDB;


namespace IIIDL
{
    public partial class URN
    {
        public void SaveModificationOAIMS(System.String ConnectionString, String OracleConnectionString, System.String URN, System.Int32 Languageid, System.Int32 Examcenterid, Byte[] Photo, Byte[] Sign, System.String AllowWhatsappMessage, System.String WhatsappNumber, out System.String Status, out System.String Message,
String BasePathP, String BasePathS, String ShareUser, String SharePass, Int32 UserId, Int32 InsurerUserId, out Exception exOut)
        {
            sql.Database objDatabase = null;
            ora.Database objOraDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Object[] AllOraParameters = null;

            DataSet objOraDataSet = null;
            DataTable objDataTableOra = null;

            Boolean SQLSuccess = false;
            Boolean OracleSuccess = false;
            //String Error = String.Empty;

            exOut = null;

            Int32 ModuleId = -1;
            String URNForExamBody = string.Empty;
            String ExamLanguage = string.Empty;
            Int32 OAIMSExamCenterId = -1;
            Int32 UpdateAims = 0;
            try
            {
                System.String ProcedureName = "sp_save_candidate_details2";
                String[] Params = new String[] { "@URN", "@LanguageId", "@ExamCenterId", "@Photo", "@Sign", "@AllowWhatsapp_message", "@Whatsapp_number", "@STATUS", "@MESSAGE", "@module_id", "@URNForExamBody", "@ExamLanguage", "@OAIMSExamCenterId", "@UpdateAims", "@UserId", "@InsurerUserId" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarBinary, SqlDbType.VarBinary, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int };
                sql.ParamLength[] ParamLengths = new sql.ParamLength[] { new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(1, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(255, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0) };
                ParameterDirection[] ParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Output, ParameterDirection.Input, ParameterDirection.Input };
                Object[] Values = new Object[] { URN, Languageid == -1 ? (Object)DBNull.Value : Languageid, Examcenterid == -1 ? (Object)DBNull.Value : Examcenterid, Photo, Sign, AllowWhatsappMessage, WhatsappNumber, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, UserId, InsurerUserId };
                try
                {
                    objDatabase = new sql.Database(ConnectionString);
                    objDatabase.OpenConnection();
                    objDatabase.BeginTransaction();
                    //Comment the below line if the procedure does not return data set.
                    //ProcReturnValue = objDatabase.ExecProcedure(ConnectionString, ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters, out objDataSet);
                    //Comment the below line if the procedure return data set.
                    ProcReturnValue = objDatabase.ExecProcedure(ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values, out AllParameters);
                    Status = Convert.ToString(AllParameters[7]);
                    Message = Convert.ToString(AllParameters[8]);

                    SQLSuccess = (Status == "SUCCESS");
                    if (!SQLSuccess)
                    {
                        if (objDatabase != null)
                        {
                            if (objDatabase.IsInTransaction())
                            {
                                objDatabase.RollbackTransaction();
                            }
                            if (objDatabase.IsConnectionOpen())
                            {
                                objDatabase.CloseConnection();
                            }
                            objDatabase = null;
                        }
                        return;
                    }
                    else
                    {//Dont commit here... Commit only after oracle executes success                                                
                        ModuleId = Convert.ToInt32(AllParameters[9]);
                        URNForExamBody = Convert.ToString(AllParameters[10]);
                        ExamLanguage = Convert.ToString(AllParameters[11]);
                        OAIMSExamCenterId = Convert.ToInt32(AllParameters[12]);
                        UpdateAims = Convert.ToInt32(AllParameters[13]);
                    }
                }
                catch (Exception ex)
                {
                    exOut = ex;
                    Status = "FAIL";
                    Message = "Error Occured While Updating The Data.";
                    if (objDatabase != null)
                    {
                        if (objDatabase.IsInTransaction())
                        {
                            objDatabase.RollbackTransaction();
                        }
                        if (objDatabase.IsConnectionOpen())
                        {
                            objDatabase.CloseConnection();
                        }
                        objDatabase = null;
                    }
                    return;
                    //SQLSuccess = false;
                }

                if (SQLSuccess)
                {
                    if (UpdateAims == 1) //Only for confirmed for exam candidates.
                    {
                        try
                        {
                            System.String OraProcedureName = "sp_modify_candidate_details";
                            String[] OraParams = new String[] { "p_URN", "p_ExamCenterId", "p_ExamModule", "p_ExamLanguage", "p_AllowWhatsAppMessage", "p_WhatsAppNumber", "p_Error", "p_Main" };
                            OracleDbType[] OraParamTypes = new OracleDbType[] { OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.Varchar2, OracleDbType.RefCursor };
                            //sql.ParamLength[] OraParamLengths = new sql.ParamLength[] { new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(255, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0) };
                            OracleCollectionType[] oracleCollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None, OracleCollectionType.None };
                            ParameterDirection[] OraParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Output, ParameterDirection.Output };
                            Object[] OraValues = new Object[] { URNForExamBody, OAIMSExamCenterId, ModuleId, ExamLanguage.ToUpper(), AllowWhatsappMessage, WhatsappNumber, DBNull.Value, DBNull.Value };

                            objOraDatabase = new ora.Database(OracleConnectionString);
                            objOraDatabase.OpenConnection();
                            objOraDatabase.BeginTransaction();
                            objOraDatabase.ExecProcedure(OraProcedureName, OraParams, OraParamTypes, OraParamDirections, OraValues, oracleCollectionType, out AllOraParameters, out objOraDataSet);
                            //objOraDatabase.CommitTransaction();

                            Message = Convert.ToString(AllOraParameters[6]);
                            Message = Message.Trim();

                            //Check Condition is missing.
                            OracleSuccess = (Message == String.Empty);
                            if (OracleSuccess)
                            {
                                if (objDatabase != null)
                                {   //commit SQL
                                    if (objDatabase.IsInTransaction())
                                    {
                                        objDatabase.CommitTransaction();
                                    }
                                    if (objDatabase.IsConnectionOpen())
                                    {
                                        objDatabase.CloseConnection();
                                    }
                                    objDatabase = null;
                                }
                                if (objOraDatabase != null)
                                {   //commit Oracle
                                    if (objOraDatabase.IsInTransaction())
                                    {
                                        objOraDatabase.CommitTransaction();
                                    }
                                    if (objOraDatabase.IsConnectionOpen())
                                    {
                                        objOraDatabase.CloseConnection();
                                    }
                                    objOraDatabase = null;
                                }
                            }
                            else
                            {
                                Status = "FAIL";
                                //Message = Message;
                                //Rollback SQL
                                if (objDatabase != null)
                                {   //Rollback SQL
                                    if (objDatabase.IsInTransaction())
                                    {
                                        objDatabase.RollbackTransaction();
                                    }
                                    if (objDatabase.IsConnectionOpen())
                                    {
                                        objDatabase.CloseConnection();
                                    }
                                    objDatabase = null;
                                }
                                //Rollback Oracle
                                if (objOraDatabase != null)
                                {   //Rollback Oracle
                                    if (objOraDatabase.IsInTransaction())
                                    {
                                        objOraDatabase.RollbackTransaction();
                                    }
                                    if (objOraDatabase.IsConnectionOpen())
                                    {
                                        objOraDatabase.CloseConnection();
                                    }
                                    objOraDatabase = null;
                                }
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            exOut = ex;
                            Status = "FAIL";
                            Message = "Error Occured While Updating The Data (2).";

                            if (objDatabase != null)
                            {   //Rollback SQL
                                if (objDatabase.IsInTransaction())
                                {
                                    objDatabase.RollbackTransaction();
                                }
                                if (objDatabase.IsConnectionOpen())
                                {
                                    objDatabase.CloseConnection();
                                }
                                objDatabase = null;
                            }
                            if (objOraDatabase != null)
                            {   //Rollback Oracle
                                if (objOraDatabase.IsInTransaction())
                                {
                                    objOraDatabase.RollbackTransaction();
                                }
                                if (objOraDatabase.IsConnectionOpen())
                                {
                                    objOraDatabase.CloseConnection();
                                }
                                objOraDatabase = null;
                            }
                            return;
                        }

                        //Set the photos...
                        if (Message.Trim() == String.Empty)
                        {
                            try
                            {
                                if (objOraDataSet == null)
                                {
                                    throw new Exception("objOraDataSet is null");
                                }
                                else
                                {
                                    if (objOraDataSet.Tables.Count == 1)
                                    {
                                        objDataTableOra = objOraDataSet.Tables[0];
                                        if (objDataTableOra == null)
                                        {
                                            throw new Exception("objDataTableOra is null");
                                        }
                                        else
                                        {
                                            if (objDataTableOra.Rows.Count == 1)
                                            {
                                                DataRow dr = objDataTableOra.Rows[0];
                                                String Path = String.Empty;
                                                if (Photo != null)
                                                {
                                                    NetworkShare.ConnectToShare(BasePathP, ShareUser, SharePass);
                                                    Path = BasePathP + "\\" + Convert.ToString(dr["EXM_CRTE_DT"]) + "\\" + Convert.ToString(dr["EXM_PHOTO_PATH"]);
                                                    File.WriteAllBytes(Path, Photo);
                                                    NetworkShare.DisconnectFromShare(BasePathP, true);
                                                }
                                                if (Sign != null)
                                                {
                                                    NetworkShare.ConnectToShare(BasePathP, ShareUser, SharePass);
                                                    Path = BasePathS + "\\" + Convert.ToString(dr["EXM_CRTE_DT"]) + "\\" + Convert.ToString(dr["EXM_SIGN_PATH"]);
                                                    File.WriteAllBytes(Path, Sign);
                                                    NetworkShare.DisconnectFromShare(BasePathP, true);
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("objDataTableOra has zero or more than 1 row.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("objOraDataSet returned multiple datatable");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                exOut = ex;
                                Status = "FAIL";
                                Message = "The database update was successful. However error occured while moving the images. Please contact help desk.";
                            }
                        }
                    }
                    else
                    {
                        //commit SQL
                        if (objDatabase != null)
                        {
                            if (objDatabase.IsInTransaction())
                            {
                                objDatabase.CommitTransaction();
                            }
                            if (objDatabase.IsConnectionOpen())
                            {
                                objDatabase.CloseConnection();
                            }
                            objDatabase = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {//No need to care for the transactions here as these are already handled above in their individual try... catch...
                exOut = ex;
                Message = "Error Occured. Please contact help desk.";
                throw ex;
            }
            finally
            {//to just be on safer side...
                if (objDatabase != null)
                {
                    if (objDatabase.IsConnectionOpen())
                    {
                        objDatabase.CloseConnection();
                    }
                }
                objDatabase = null;

                if (objOraDatabase != null)
                {
                    if (objOraDatabase.IsConnectionOpen())
                    {
                        objOraDatabase.CloseConnection();
                    }
                }
                objOraDatabase = null;
            }
        }

        public DataSet GetPaymentReceiptData(String OracleConnectionString, System.String URN)
        {
            ora.Database objOraDatabase = null;
            //DataSet objDataSet = null; //Comment this line if the procedure does not return data set.;
            Int32 ProcReturnValue = 0;
            Object[] AllParameters = null;
            Object[] AllOraParameters = null;
            DataTable objDataTableOra = null;
            DataSet objOraDataSet = null;
            Boolean OracleSuccess = false;
            Exception exOut = null;
            try
            {
                objOraDatabase = new ora.Database();
                System.String OraProcedureName = "sp_get_receipts_for_urn";
                String[] OraParams = new String[] { "p_urn", "vMain" };
                OracleDbType[] OraParamTypes = new OracleDbType[] { OracleDbType.Varchar2, OracleDbType.RefCursor };
                //sql.ParamLength[] OraParamLengths = new sql.ParamLength[] { new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(-1, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(255, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(20, 0, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0), new sql.ParamLength(4, 10, 0) };
                OracleCollectionType[] oracleCollectionType = new OracleCollectionType[] { OracleCollectionType.None, OracleCollectionType.None };
                ParameterDirection[] OraParamDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Output };
                Object[] OraValues = new Object[] { URN, DBNull.Value };

                objOraDatabase.ExecProcedure(OracleConnectionString, OraProcedureName, OraParams, OraParamTypes, OraParamDirections, OraValues, oracleCollectionType, out AllOraParameters, out objOraDataSet, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDataTableOra = null;    
            }
            return objOraDataSet;
        }
    }
}
