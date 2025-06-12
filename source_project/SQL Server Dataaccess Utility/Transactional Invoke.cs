using System;
using System.Data;
using System.Data.SqlClient;

namespace Databases.SQLServer
{
    public partial class Database : IDisposable
    {
		private System.String sConnectionString = null;
		private SqlConnection objConnection = null;
		private SqlTransaction objTransaction = null;
		private System.Boolean _IsInTransaction = false;

        /// <summary>
        /// For internal use only. validtaes the invokation mode i.e. whether transactional mode / non transactional mode.
        /// overloads are available for transactional / non transactional mode invokation, and object needs to be constructed accordingly.
        /// Object created using transactional mode cant be used to invoke non transactional overloads and vice-a-versa.
        /// This methode is used by other methods to determine whether the method can continue in given mode. 
        /// </summary>
        private void ValidateTransactionalInvokation()
        {
            switch (Invokation)
            {
                case "TRANSACTIONAL":
                    //Do Nothing
                    break;
                case "NONTRANSACTIONAL":
                    throw new Exception("The Component Has Been Invoked In Non Transactional Mode. This Overload Can Be Invoked Only In Transactional Mode");
                    break;
            }
        }

        //Transactional 
        #region Transactional
            #region Constructor
            /// <summary>
            /// Constructor for Transactional Mode Invoke
            /// </summary>
            /// <param name="ConnectionString"></param>
                public Database(System.String ConnectionString)
		        {
			        CheckConnectionString(ConnectionString);
			        sConnectionString = ConnectionString;
			        Invokation = "TRANSACTIONAL";
                }
            #endregion
            #region Connection Mgmt : Transactional Mode
        /// <summary>
        /// Call this method to open the connection.
        /// </summary>
                public void OpenConnection()
		        {
			        ValidateTransactionalInvokation();
			        try
			        {
				        CheckConnectionString(sConnectionString);
				        if (objConnection != null && objConnection.State == ConnectionState.Open )
				        {
					        throw new Exception("Connection Already Open! Close The Connection Before Reopening");
				        }
				        else
				        {
                            objConnection = null;
                            objConnection = new SqlConnection();
                            objConnection.ConnectionString = sConnectionString;
                            objConnection.Open();
				        }
			        }
			        catch (Exception ex)
			        {
				        objConnection = null;
                        throw; // new Exception(ex.Message);
			        }
		        }

        /// <summary>
        /// Call this method to check if the connection is open
        /// </summary>
        /// <returns></returns>
                public System.Boolean IsConnectionOpen()
                {
                    if (objConnection != null)
                    {
                        return (objConnection.State == ConnectionState.Open) ? true : false;
                    }
                    else
                    {
                        return false;
                    }
                }

        /// <summary>
        /// Call this method to close the open connection
        /// </summary>
                public void CloseConnection()
                {
                    ValidateTransactionalInvokation();
                    if (objConnection != null && objConnection.State == ConnectionState.Open)
                    {
                        objConnection.Close();
                    }
                }

       
            #endregion
            #region Transaction Mgmt : Transactional Mode
        /// <summary>
        /// Call this method to initiate new database transaction
        /// </summary>
            public void BeginTransaction()
		    {
			    ValidateTransactionalInvokation();
			    try
			    {
				    CheckConnectionState(objConnection);
				    objTransaction = objConnection.BeginTransaction(); 
				    _IsInTransaction = true;
			    }
			    catch (Exception ex)
			    {
				    objTransaction = null;
				    _IsInTransaction = false;
                    throw;
                    //throw new Exception(ex.Message);
			    }
		    }

        /// <summary>
        /// Call this method to commit the existing transaction
        /// </summary>
		    public void CommitTransaction()
		    {
			    ValidateTransactionalInvokation();
			    if (IsInTransaction() && objTransaction != null)
			    {
				    objTransaction.Commit();
				    objTransaction = null;
				    _IsInTransaction = false;
			    }
			    else
			    {
				    objTransaction = null;
				    throw new Exception("Commit Failed: No Active Transaction");
			    }
		    }

        /// <summary>
        /// Call this method to roll back the existing transaction
        /// </summary>
		    public void RollbackTransaction()
		    {
			    ValidateTransactionalInvokation();
			    if (IsInTransaction() && objTransaction != null)
			    {
				    objTransaction.Rollback();
				    objTransaction = null;
				    _IsInTransaction = false;
			    }
			    else
			    {
				    objTransaction = null;
				    throw new Exception("Rollback Failed: No Active Transaction");
			    }
		    }

        /// <summary>
        /// Call this method to check whether the object has initiated the transaction.
        /// </summary>
        /// <returns>Boolean: True: In Transaction / False : Not in Transaction</returns>
		    public System.Boolean IsInTransaction()
		    {
			    ValidateTransactionalInvokation();
			    if (_IsInTransaction)
			    {
				    return (objTransaction == null) ? false : true;
			    }
			    else
			    {
				    return false;
			    }
            }
        #endregion
        #region Transaction Mode Functions
        /// <summary>
        /// Use this method to execute adhoc select statement
        /// </summary>
        /// <param name="sSQL">SQL SELECT Query</param>
        /// <returns></returns>
        public System.Data.DataTable FetchAdhoc(System.String sSQL)
		    {
			    ValidateTransactionalInvokation();
			    SqlDataAdapter objDataAdapter = null;  
			    DataTable dtDataTable = null;
			    SqlCommand objCommand = null;

			    try
			    {
				    objCommand = new SqlCommand(); 
				    objCommand.CommandText = sSQL;
				    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.Connection = objConnection;
				    if (this.IsInTransaction())
				    {
					    objCommand.Transaction = objTransaction;
				    }

				    objDataAdapter = new SqlDataAdapter();
				    objDataAdapter.SelectCommand = objCommand;
				    dtDataTable  = new System.Data.DataTable(); 
				    CheckConnectionState(objConnection);
				    objDataAdapter.Fill(dtDataTable );  

				    return dtDataTable ; 
			    }
			    catch (Exception ex)
			    {
                    throw;
                    //throw new Exception(ex.Message);
			    }
			    finally
			    {
				    objDataAdapter = null;
				    objCommand= null;
			    }
		    }

        /// <summary>
        /// Use this method to execute adhoc sql statement
        /// </summary>
        /// <param name="sSQL">SQL Query</param>
        /// <returns></returns>
        public int ExecuteAdhoc(System.String sSQL)
		    {
			    ValidateTransactionalInvokation();

			    SqlCommand objCommand = null;
			    System.Int32 nRowsAffected = 0;
			    try
			    {
				    objCommand = new SqlCommand(); 
				    objCommand.CommandText = sSQL;
				    objCommand.CommandType = System.Data.CommandType.Text;
                    objCommand.Connection = objConnection;
				    if (this.IsInTransaction())
				    {
					    objCommand.Transaction = objTransaction;
				    }

				    CheckConnectionState(objConnection);
				    nRowsAffected = objCommand.ExecuteNonQuery();

				    return nRowsAffected;
			    }
			    catch (Exception ex)
			    {
                    throw;
				    //throw new Exception(ex.Message);
			    }
			    finally
			    {
				    objCommand = null;
			    }
            }

        /// <summary>
        /// Use this method to executes the stored procedure which returns data set.
        /// </summary>
        /// <param name="ProcedureName">Name of the procedure to be executed</param>
        /// <param name="Params">String Array Containing the parameter names without "@" prefix... (excluding the return value )</param>
        /// <param name="ParamTypes">Array of type SQLDBType, with values for each parameter (excluding the return value)</param>
        /// <param name="ParamLengths">ParamLength Array, with values for each parameter (excluding the return value)</param>
        /// <param name="ParamDirections">ParamDirection Array, with values for each parameter (excluding the return value)</param>
        /// <param name="Values">Object Array, with values for each parameter (excluding the return value). Set value to DBNull.Value for output parameters.</param>
        /// <param name="AllParameters">The parameters values returned in same orders</param>
        /// <param name="objDataSet">The Dataset returned if it contains any datatable</param>
        /// <returns>return value returned from the stored procedure</returns>
        public Int32 ExecProcedure(System.String ProcedureName, System.String[] Params, SqlDbType[] ParamTypes, ParamLength[] ParamLengths, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, out System.Object[] AllParameters, out System.Data.DataSet objDataSet)
            {
                ValidateTransactionalInvokation();
                System.Int32 nCount = 0;
                SqlCommand objCommand = null;
                SqlDataAdapter objDataAdapter = null;
                try
                {
                    objCommand = PrepareCommand( ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values);

                    objDataAdapter = new SqlDataAdapter();
                    objCommand.CommandTimeout = 0;
                    objDataAdapter.SelectCommand = objCommand;

                    objCommand.Transaction = objTransaction;
                    objCommand.Connection = objConnection;
                    CheckConnectionState(objConnection);

                    objDataSet = new DataSet();
                    objDataAdapter.Fill(objDataSet);

                    //Added Newly to return the out parameters from this procedure
                    AllParameters = new System.Object[Params.Length];

                    //End at Count - 1 to exclude the return value...
                    for (nCount = 0; nCount < objCommand.Parameters.Count - 1; nCount++)
                    {
                        AllParameters[nCount ] = objCommand.Parameters[nCount].Value;
                    }
                    Int32 RetValue = (Int32)objCommand.Parameters["@_ReturnValue"].Value;
                    //End New Addition 
                    return RetValue;
                }
                catch (Exception ex)
                {
                    throw;
                    //throw new Exception(ex.Message);
                }
                finally
                {
                    objCommand = null;
                }
            }

        /// <summary>
        /// Use this method to executes the stored procedure that does not return any data in dataset.
        /// </summary>
        /// <param name="ProcedureName">Name of the procedure to be executed</param>
        /// <param name="Params">String Array Containing the parameter names without "@" prefix... (excluding the return value )</param>
        /// <param name="ParamTypes">Array of type SQLDBType, with values for each parameter (excluding the return value)</param>
        /// <param name="ParamLengths">ParamLength Array, with values for each parameter (excluding the return value)</param>
        /// <param name="ParamDirections">ParamDirection Array, with values for each parameter (excluding the return value)</param>
        /// <param name="Values">Object Array, with values for each parameter (excluding the return value). Set value to DBNull.Value for output parameters.</param>
        /// <param name="AllParameters">The parameters values returned in same orders</param>
        /// <returns>return value returned from the stored procedure</returns>
        public Int32 ExecProcedure(System.String ProcedureName, System.String[] Params, SqlDbType[] ParamTypes, ParamLength[] ParamLengths, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, out System.Object[] AllParameters)
            {
                ValidateTransactionalInvokation();
                System.Int32 nCount = 0;
                SqlCommand objCommand = null;
                SqlDataAdapter objDataAdapter = null;
                try
                {
                    objCommand = PrepareCommand(ProcedureName, Params, ParamTypes, ParamLengths, ParamDirections, Values);

                    objDataAdapter = new SqlDataAdapter();
                    objDataAdapter.SelectCommand = objCommand;

                    objCommand.Transaction = objTransaction;
                    objCommand.Connection = objConnection;
                    CheckConnectionState(objConnection);

                    objCommand.ExecuteNonQuery();

                    //Added Newly to return the out parameters from this procedure
                    AllParameters = new System.Object[Params.Length];

                    //End at count 1 to exclude the return value...
                    for (nCount = 0; nCount < objCommand.Parameters.Count-1; nCount++)
                    {
                        AllParameters[nCount ] = objCommand.Parameters[nCount].Value;
                    }
                    Int32 RetValue = (Int32)objCommand.Parameters["@_ReturnValue"].Value;
                    //End New Addition 
                    return RetValue;
                }
                catch (Exception ex)
                {
                    throw;
                    //throw new Exception(ex.Message);
                }
                finally
                {
                    objCommand = null;
                }
            }

        /// <summary>
        /// Use this method to bulk copy the data into the destination table
        /// </summary>
        /// <param name="DestinationTableName"></param>
        /// <param name="SourceColumns">Source Column Names as String Array</param>
        /// <param name="DestColumns">Destination Column Names as String Array</param>
        /// <param name="BatchSize">Batch size in terms of number of rows per batch</param>
        /// <param name="dtDataTable">Data as DataTable</param>
        public void BulkCopy(String DestinationTableName, String[] SourceColumns, String[] DestColumns, Int32 BatchSize, DataTable dtDataTable)
            {
                ValidateTransactionalInvokation();

                if ((SourceColumns.Length == 0 || DestColumns.Length == 0) || (SourceColumns.Length != DestColumns.Length))
                {
                    throw new Exception("SourceColumns / DestColumns Length Cannot Be Zero. SourceColumns / DestColumns Lengths must be equal");
                }

                SqlBulkCopy bcp = null;
                try
                {
                    bcp = new SqlBulkCopy(objConnection, SqlBulkCopyOptions.Default, objTransaction);
                    bcp.DestinationTableName = DestinationTableName;
                    for (Int32 i = 0; i < SourceColumns.Length; i++)
                    {
                        bcp.ColumnMappings.Add(SourceColumns[i], DestColumns[i]);
                    }

                    CheckConnectionState(objConnection);
                    bcp.WriteToServer(dtDataTable);
                }
                catch (Exception ex)
                {
                    throw;
                    //throw new Exception(ex.Message);
                }
                finally
                {
                    bcp = null;
                }
        }

            #endregion
        #endregion
 
        /// <summary>
        /// 
        /// </summary>
                    
        public void Dispose()
        {
            objConnection.Dispose();
        }
    }
}