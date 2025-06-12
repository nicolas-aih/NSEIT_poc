using System;
using System.Data;
using System.Data.SqlClient;

namespace Databases.SQLServer
{
    public partial class Database : IDisposable
    {
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

        public Database()
        {
            Invokation = "NONTRANSACTIONAL";
        }
        #endregion

        #region Non Transactional Mode Functions

        public System.Data.DataTable FetchAdhoc( System.String ConnectionString, System.String sSQL, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);

            SqlConnection objConnectionNT = null;
            SqlDataAdapter objDataAdapter = null;
            DataTable dtDataTable = null;
            SqlCommand objCommand = null;
            SqlTransaction objTransactionNT = null;
            try
            {
                if (UseTransaction)
                {
                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new SqlCommand();
                    objCommand.CommandText = sSQL;
                    objCommand.CommandType = System.Data.CommandType.Text;

                    objTransactionNT = objConnectionNT.BeginTransaction();

                    objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;

                    objDataAdapter = new SqlDataAdapter();
                    objDataAdapter.SelectCommand = objCommand;
                    dtDataTable = new System.Data.DataTable();
                    CheckConnectionState(objConnectionNT);
                    objDataAdapter.Fill(dtDataTable);

                    objTransactionNT.Commit();
                }
                else
                {
                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new SqlCommand();
                    objCommand.CommandText = sSQL;
                    objCommand.CommandType = System.Data.CommandType.Text;

                    //objTransactionNT = objConnectionNT.BeginTransaction();
                    //objCommand.Transaction = objTransactionNT;
                    objCommand.Connection = objConnectionNT;

                    objDataAdapter = new SqlDataAdapter();
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


        public int ExecuteAdhoc(System.String ConnectionString, System.String sSQL, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);

            SqlConnection objConnectionNT = null;
            SqlCommand objCommand = null;
            System.Int32 nRowsAffected = 0;
            SqlTransaction objTransactionNT = null;
            try
            {
                if (UseTransaction)
                {
                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new SqlCommand();
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
                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objCommand = new SqlCommand();
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


        public Int32 ExecProcedure(System.String ConnectionString, System.String ProcedureName, System.String[] Params, SqlDbType[] ParamTypes, ParamLength[] ParamLengths, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, out System.Object[] AllParameters, out System.Data.DataSet objDataSet, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);
            System.Int32 nCount = 0;
            SqlConnection objConnectionNT = null;
            SqlCommand objCommand = null;
            SqlDataAdapter objDataAdapter = null;
            SqlTransaction objTransactionNT = null;
            Int32 RetValue = 0;
            try
            {
                if (UseTransaction)
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values);

                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    objTransactionNT = objConnectionNT.BeginTransaction();

                    objDataAdapter = new SqlDataAdapter();
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
                    for (nCount = 0; nCount < objCommand.Parameters.Count - 1; nCount++)
                    {
                        AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                    }
                    RetValue = (Int32)objCommand.Parameters["@_ReturnValue"].Value;
                    //End New Addition 
                }
                else
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values);

                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    objDataAdapter = new SqlDataAdapter();
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
                    for (nCount = 0; nCount < objCommand.Parameters.Count - 1; nCount++)
                    {
                        AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                    }
                    RetValue = (Int32)objCommand.Parameters["@_ReturnValue"].Value;
                }
                return RetValue;
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

        public Int32 ExecProcedure(System.String ConnectionString, System.String ProcedureName, System.String[] Params, SqlDbType[] ParamTypes, ParamLength[] ParamLengths, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, out System.Object[] AllParameters, Boolean UseTransaction = false)
        {
            ValidateNonTransactionalInvokation();
            CheckConnectionString(ConnectionString);
            Int32 nCount = 0;
            SqlConnection objConnectionNT = null;
            SqlCommand objCommand = null;
            SqlDataAdapter objDataAdapter = null;
            SqlTransaction objTransactionNT = null;
            Int32 RetValue = 0;
            try
            {
                if (UseTransaction)
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values);

                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    objTransactionNT = objConnectionNT.BeginTransaction();

                    //objDataAdapter = new SqlDataAdapter();
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
                    for (nCount = 1; nCount < objCommand.Parameters.Count - 1; nCount++)
                    {
                        AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                    }
                    RetValue = Convert.ToInt32(objCommand.Parameters["@_ReturnValue"].Value);
                    //End New Addition 
                }
                else
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values);

                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();
                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    //objDataAdapter = new SqlDataAdapter();
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
                    for (nCount = 1; nCount < objCommand.Parameters.Count - 1; nCount++)
                    {
                        AllParameters[nCount] = objCommand.Parameters[nCount].Value;
                    }
                    RetValue = Convert.ToInt32(objCommand.Parameters["@_ReturnValue"].Value);
                    //End New Addition 
                }
                return RetValue;
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

            SqlConnection objConnectionNT = null;
            SqlTransaction objTransactionNT = null;
            SqlBulkCopy bcp = null;
            try
            {
                if (UseTransaction)
                {
                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    objTransactionNT = objConnectionNT.BeginTransaction();

                    bcp = new SqlBulkCopy(objConnectionNT, SqlBulkCopyOptions.Default, objTransactionNT);
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
                    objConnectionNT = new SqlConnection();
                    objConnectionNT.ConnectionString = ConnectionString;
                    objConnectionNT.Open();

                    //objTransactionNT = objConnectionNT.BeginTransaction();

                    bcp = new SqlBulkCopy(objConnectionNT, SqlBulkCopyOptions.Default, null);
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

        #endregion
        #endregion
    }
}