using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace OraDatabases.OracleDB
{
    public partial class Database : IDisposable
    {
        //private System.String sConnectionString = null;
        //private OracleConnection objConnection = null;
        //private OracleTransaction objTransaction = null;
        //private System.Boolean _IsInTransaction = false;
        //private System.String Invokation;

        /// <summary>
        /// For internal use only. validates the invokation mode i.e. whether transactional mode / non transactional mode.
        /// overloads are available for transactional / non transactional mode invokation, and object needs to be constructed accordingly.
        /// Object created using transactional mode cant be used to invoke non transactional overloads and vice-a-versa.
        /// This methode is used by other methods to determine whether the method can continue in given mode. 
        /// </summary>
        private void ValidateNonTransactionalInvokation()
        {
            switch (Invokation)
            {
                case "TRANSACTIONAL":
                    throw new Exception("The Component Has Been Invoked In Transactional Mode. This Overload Can Be Invoked Only In Non Transactional Mode");
                    break;
                case "NONTRANSACTIONAL":
                    //Do Nothing
                    break;
            }
        }

        #region Non-Transactional
        #region Constructor
        /// <summary>
        /// /// Constructor for Non Transactional Mode Invoke
        /// </summary>
        public Database()
        {
            Invokation = "NONTRANSACTIONAL";
        }
        #endregion

        #region Non Transactional Mode Functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="sSQL"></param>
        /// <param name="UseTransaction"></param>
        /// <returns></returns>
        public System.Data.DataTable FetchAdhoc( System.String ConnectionString, System.String sSQL, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);

            OracleConnection objConnectionNT = null;
            OracleDataAdapter objDataAdapter = null;
            DataTable dtDataTable = null;
            OracleCommand objCommand = null;
            OracleTransaction objTransactionNT = null;
            try
            {
                if (UseTransaction)
                {
                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new OracleCommand();
                    objCommand.CommandText = sSQL;
                    objCommand.CommandType = System.Data.CommandType.Text;

                    objTransactionNT = objConnectionNT.BeginTransaction();

                    objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;

                    objDataAdapter = new OracleDataAdapter();
                    objDataAdapter.SelectCommand = objCommand;
                    dtDataTable = new System.Data.DataTable();
                    CheckConnectionState(objConnectionNT);
                    objDataAdapter.Fill(dtDataTable);

                    objTransactionNT.Commit();
                }
                else
                {
                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new OracleCommand();
                    objCommand.CommandText = sSQL;
                    objCommand.CommandType = System.Data.CommandType.Text;

                    //objTransactionNT = objConnectionNT.BeginTransaction();
                    //objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;

                    objDataAdapter = new OracleDataAdapter();
                    objDataAdapter.SelectCommand = objCommand;
                    dtDataTable = new System.Data.DataTable();
                    CheckConnectionState(objConnectionNT);
                    objDataAdapter.Fill(dtDataTable);

                    //objTransactionNT.Commit();
                }
                return dtDataTable;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Rollback();
                    }
                }
                throw;
                //throw new Exception(ex.Message);
            }
            finally
            {
                objDataAdapter.Dispose();
                objDataAdapter = null;
                objCommand.Dispose();
                objCommand = null;
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Dispose();
                        objTransactionNT = null;
                    }
                }
                if (objConnectionNT != null)
                {
                    if (objConnectionNT.State == ConnectionState.Open)
                    {
                        objConnectionNT.Close();
                    }
                    objConnectionNT.Dispose();
                    objConnectionNT = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="sSQL"></param>
        /// <param name="UseTransaction"></param>
        /// <returns></returns>
        public int ExecuteAdhoc(System.String ConnectionString, System.String sSQL, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);

            OracleConnection objConnectionNT = null;
            OracleCommand objCommand = null;
            System.Int32 nRowsAffected = 0;
            OracleTransaction objTransactionNT = null;
            try
            {
                if (UseTransaction)
                {
                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new OracleCommand();
                    objCommand.CommandText = sSQL;
                    objCommand.CommandType = System.Data.CommandType.Text;

                    objTransactionNT = objConnectionNT.BeginTransaction();

                    objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;

                    CheckConnectionState(objConnectionNT);
                    nRowsAffected = objCommand.ExecuteNonQuery();

                    objTransactionNT.Commit();
                }
                else
                {
                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new OracleCommand();
                    objCommand.CommandText = sSQL;
                    objCommand.CommandType = System.Data.CommandType.Text;

                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    //objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;

                    CheckConnectionState(objConnectionNT);
                    nRowsAffected = objCommand.ExecuteNonQuery();

                    //objTransactionNT.Commit();
                }

                return nRowsAffected;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Rollback();
                    }
                }
                throw;
                //throw new Exception(ex.Message);
            }
            finally
            {
                objCommand.Dispose();
                objCommand = null;
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Dispose();
                        objTransactionNT = null;
                    }
                }
                if (objConnectionNT != null)
                {
                    if (objConnectionNT.State == ConnectionState.Open)
                    {
                        objConnectionNT.Close();
                    }
                    objConnectionNT.Dispose();
                    objConnectionNT = null;
                }
            }
        }

        public void ExecProcedure(System.String ConnectionString, System.String ProcedureName, System.String[] Params, OracleDbType[] ParamTypes, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, OracleCollectionType [] CollectionType, out System.Object[] AllParameters, out System.Data.DataSet objDataSet, Boolean UseTransaction = true)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);
            System.Int32 nCount = 0;
            OracleConnection objConnectionNT = null;
            OracleCommand objCommand = null;
            OracleDataAdapter objDataAdapter = null;
            OracleTransaction objTransactionNT = null;
            //Int32 RetValue = 0;

            try
            {
                if (UseTransaction)
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType);

                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    objTransactionNT = objConnectionNT.BeginTransaction();

                    objDataAdapter = new OracleDataAdapter();
                    objCommand.CommandTimeout = 0;
                    objDataAdapter.SelectCommand = objCommand;

                    objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;
                    CheckConnectionState(objConnectionNT);

                    objDataSet = new DataSet();
                    objDataAdapter.Fill(objDataSet);

                    objTransactionNT.Commit();

                    //Added Newly to return the out parameters from this procedure
                    AllParameters = new System.Object[Params.Length];

                    //End at count - 1 to exclude the return value...
                    for (nCount = 0; nCount < objCommand.Parameters.Count; nCount++)
                    {
                        if (objCommand.Parameters[nCount].Value.ToString() == "null")
                        {
                            AllParameters[nCount] = DBNull.Value;
                        }
                        else
                        {
                            AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                        }
                    }
                    //RetValue = (Int32)objCommand.Parameters["@_ReturnValue"].Value;
                    //End New Addition 
                }
                else
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType);

                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    objDataAdapter = new OracleDataAdapter();
                    objCommand.CommandTimeout = 0;
                    objDataAdapter.SelectCommand = objCommand;

                    //objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;
                    CheckConnectionState(objConnectionNT);

                    objDataSet = new DataSet();
                    objDataAdapter.Fill(objDataSet);
                    
                    //objTransactionNT.Commit();

                    //Added Newly to return the out parameters from this procedure
                    AllParameters = new System.Object[Params.Length];

                    //End at count - 1 to exclude the return value...
                    for (nCount = 0; nCount < objCommand.Parameters.Count; nCount++)
                    {
                        if (objCommand.Parameters[nCount].Value.ToString() == "null")
                        {
                            AllParameters[nCount] = DBNull.Value;
                        }
                        else
                        {
                            AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                        }
                    }
                    //RetValue = (Int32)objCommand.Parameters["@_ReturnValue"].Value;
                }
                //return RetValue;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Rollback();
                    }
                }
                throw;
                //throw new Exception(ex.Message);
            }
            finally
            {
                //objCommand.Dispose();
                objCommand = null;
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Dispose();
                        objTransactionNT = null;
                    }
                }
                if (objConnectionNT != null)
                {
                    if (objConnectionNT.State == ConnectionState.Open)
                    {
                        objConnectionNT.Close();
                    }
                    objConnectionNT.Dispose();
                    objConnectionNT = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="ProcedureName"></param>
        /// <param name="Params"></param>
        /// <param name="ParamTypes"></param>
        /// <param name="ParamDirections"></param>
        /// <param name="Values"></param>
        /// <param name="AllParameters"></param>
        /// <param name="UseTransaction"></param>
        public void ExecProcedure(System.String ConnectionString, System.String ProcedureName, System.String[] Params, OracleDbType[] ParamTypes, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, OracleCollectionType [] CollectionType, out System.Object[] AllParameters, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);
            Int32 nCount = 0;
            OracleConnection objConnectionNT = null;
            OracleCommand objCommand = null;
            //OracleDataAdapter objDataAdapter = null;
            OracleTransaction objTransactionNT = null;
            //Int32 RetValue = 0;
            try
            {
                if (UseTransaction)
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType);

                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    objTransactionNT = objConnectionNT.BeginTransaction();

                    //objDataAdapter = new OracleDataAdapter();
                    objCommand.CommandTimeout = 0;
                    //objDataAdapter.SelectCommand = objCommand;

                    objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;
                    CheckConnectionState(objConnectionNT);

                    objCommand.ExecuteNonQuery();

                    objTransactionNT.Commit();

                    //Added Newly to return the out parameters from this procedure
                    AllParameters = new System.Object[Params.Length];

                    //End at count - 1 to exclude the return value...
                    for (nCount = 0; nCount < objCommand.Parameters.Count; nCount++)
                    {
                        if (objCommand.Parameters[nCount].Value.ToString() == "null")
                        {
                            AllParameters[nCount] = DBNull.Value;
                        }
                        else
                        {
                            AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                        }
                    }
                    //RetValue = Convert.ToInt32(objCommand.Parameters["@_ReturnValue"].Value);
                    //End New Addition 
                }
                else
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamDirections, Values, CollectionType);

                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    //objDataAdapter = new OracleDataAdapter();
                    objCommand.CommandTimeout = 0;
                    //objDataAdapter.SelectCommand = objCommand;

                    //objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;
                    CheckConnectionState(objConnectionNT);

                    objCommand.ExecuteNonQuery();

                    //objTransactionNT.Commit();

                    //Added Newly to return the out parameters from this procedure
                    AllParameters = new System.Object[Params.Length];

                    //End at count - 1 to exclude the return value...
                    for (nCount = 0; nCount < objCommand.Parameters.Count; nCount++)
                    {
                        if (objCommand.Parameters[nCount].Value.ToString() == "null")
                        {
                            AllParameters[nCount] = DBNull.Value;
                        }
                        else
                        {
                            AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                        }
                    }
                    //RetValue = Convert.ToInt32(objCommand.Parameters["@_ReturnValue"].Value);
                    //End New Addition 
                }
                //return RetValue;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Rollback();
                    }
                }
                throw;
                //throw new Exception(ex.Message);
            }
            finally
            {
                //objCommand.Dispose();
                objCommand = null;
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Dispose();
                        objTransactionNT = null;
                    }
                }
                if (objConnectionNT != null)
                {
                    if (objConnectionNT.State == ConnectionState.Open)
                    {
                        objConnectionNT.Close();
                    }
                    objConnectionNT.Dispose();
                    objConnectionNT = null;
                }
            }
        }

/*
        /// <summary>
        /// Use this method to bulk copy the data into the destination table
        /// </summary>
        /// <param name="ConnectionString">Valid Connection String</param>
        /// <param name="DestinationTableName"></param>
        /// <param name="SourceColumns">Source Column Names as String Array</param>
        /// <param name="DestColumns">Destination Column Names as String Array</param>
        /// <param name="BatchSize">Batch size in terms of number of rows per batch</param>
        /// <param name="dtDataTable">Data as DataTable</param>
        public void BulkCopy(System.String ConnectionString, String DestinationTableName, String[] SourceColumns, String[] DestColumns, Int32 BatchSize, DataTable dtDataTable, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);

            if ((SourceColumns.Length == 0 || DestColumns.Length == 0) || (SourceColumns.Length != DestColumns.Length))
            {
                throw new Exception("SourceColumns / DestColumns Length Cannot Be Zero. SourceColumns / DestColumns Lengths must be equal");
            }

            OracleConnection objConnectionNT = null;
            OracleTransaction objTransactionNT = null;

            OracleBulkCopy bcp = null;
            try
            {
                if (UseTransaction)
                {
                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objTransactionNT = objConnectionNT.BeginTransaction();

                    bcp = new OracleBulkCopy(objConnectionNT, OracleBulkCopyOptions.Default, objTransactionNT);

                    bcp.DestinationTableName = DestinationTableName;
                    for (Int32 i = 0; i < SourceColumns.Length; i++)
                    {
                        bcp.ColumnMappings.Add(SourceColumns[i], DestColumns[i]);
                    }

                    CheckConnectionState(objConnectionNT);
                    bcp.WriteToServer(dtDataTable);
                    objTransactionNT.Commit();
                }
                else
                {
                    objConnectionNT = new OracleConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    bcp = new OracleBulkCopy(objConnectionNT, OracleBulkCopyOptions.Default, null);
                    bcp.DestinationTableName = DestinationTableName;
                    for (Int32 i = 0; i < SourceColumns.Length; i++)
                    {
                        bcp.ColumnMappings.Add(SourceColumns[i], DestColumns[i]);
                    }

                    CheckConnectionState(objConnectionNT);
                    bcp.WriteToServer(dtDataTable);
                    //objTransactionNT.Commit();
                }
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Rollback();
                    }
                }
                throw;        
                //throw new Exception(ex.Message);
            }
            finally
            {
                bcp = null;
                if (UseTransaction)
                {
                    if (objTransactionNT != null)
                    {
                        objTransactionNT.Dispose();
                        objTransactionNT = null;
                    }
                }
                if (objConnectionNT != null)
                {
                    if (objConnectionNT.State == ConnectionState.Open)
                    {
                        objConnectionNT.Close();
                    }
                    objConnectionNT.Dispose();
                    objConnectionNT = null;
                }
            }
        }
*/
        #endregion
        #endregion
    }
}