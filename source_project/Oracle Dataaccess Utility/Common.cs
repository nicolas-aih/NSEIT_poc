using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Reflection;


namespace OraDatabases.OracleDB
{
    /// <summary>
    /// The class for interaction with Microsoft SQL Server.
    /// </summary>
    public partial class Database : IDisposable
    {
        private System.String Invokation;

        #region Common Function
        /// <summary>
        /// For internal use only. Checks if the connection string is initialized and throws exception if found null or empty string. Any other value is taken as correct value.
        /// </summary>
        /// <param name="ConnectionString">The connection string to be evaluated</param>
        private void CheckConnectionString(System.String ConnectionString)
        {
            if (String.IsNullOrEmpty(ConnectionString.Trim()) || String.IsNullOrWhiteSpace(ConnectionString.Trim()))
            {
                throw new Exception("Please Provide Connection String");
            }
        }

        private void ValidateCommandParams(System.String[] Params, OracleDbType[] ParamTypes, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, OracleCollectionType []CollectionType = null)
        {
            System.Int32 nCount = 0;
            if (Params != null)
            {
                if ((Params.Length != ParamTypes.Length) || (Params.Length != ParamDirections.Length) || (Params.Length != Values.Length) )
                {
                    throw (new Exception("The Element's Count In Parameters / Parameters Type / Parameters Direction / Parameters Values Array Is Not Same"));
                }
                if (CollectionType != null)
                {
                    if (Params.Length != CollectionType.Length)
                    {
                        throw (new Exception("The Element's Count In Parameters / CollectionType Array Is Not Same"));
                    }
                    else
                    {
                        for(nCount = 0; nCount < CollectionType.Length; nCount++)
                        {
                            if (CollectionType[nCount] == OracleCollectionType.PLSQLAssociativeArray)
                            {
                                if (!Values[nCount].GetType().IsArray)
                                {
                                    throw ( new Exception (String.Format("The value passed is not an array. Parameter : {0}", Params[nCount])));
                                }
                            }
                        }
                    }
                }
                for (nCount = 0; nCount < ParamTypes.Length; nCount++)
                {
                    OracleDbType PType = ParamTypes[nCount];
                    if (PType == OracleDbType.Char || PType == OracleDbType.Varchar2 || PType == OracleDbType.NChar || PType == OracleDbType.NVarchar2
                        || PType == OracleDbType.Date || PType == OracleDbType.Decimal ||
                        PType == OracleDbType.RefCursor
                        )
                    {

                    }
                    else
                    {
                        throw (new Exception(String.Format("Parameter Of Type '{0}' Are Not Supported Currently.", Enum.GetName(typeof(SqlDbType), PType))));
                    }
                    //Additional validation for RefCursor
                    if (PType == OracleDbType.RefCursor && ParamDirections[nCount] != ParameterDirection.Output )
                    {
                        throw (new Exception("Ref Cursor Parameters Are Supported Only As Out Parameter"));
                    }
                }
            }
        }

        /// <summary>
        /// For internal use only. Using the parameters; Prepares and returns the OracleCommand object for executing the procedure.
        /// </summary>
        /// <param name="ProcedureName">Name of the stored procedure to be executed</param>
        /// <param name="Params">String srray of parameter names</param>
        /// <param name="ParamTypes">Int32 srray of parameter type. SQLDBType enumerator typecasted to Int32</param>
        /// <param name="ParamDirections">System.Data.ParameterDirection array indicating the parameter direction</param>
        /// <param name="Values">System.Object array of the parameter values</param>
        /// <returns></returns>
        private OracleCommand PrepareCommand(System.String ProcedureName, System.String[] Params, OracleDbType [] ParamTypes, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values, OracleCollectionType []CollectionType = null)
        {
            ValidateCommandParams(Params, ParamTypes, ParamDirections, Values, CollectionType);
            OracleCommand objCommand = new OracleCommand();
            Int32 nCount = 0;
            objCommand.CommandText = ProcedureName;
            objCommand.CommandType = System.Data.CommandType.StoredProcedure;
            for (nCount = 0; nCount < Params.Length; nCount++)
            {
                String ParamName = Params[nCount];
                OracleParameter objParam = objCommand.Parameters.Add(ParamName, (OracleDbType)ParamTypes[nCount]);
                //Set Collection Type
                if (CollectionType != null)
                {
                    objParam.CollectionType = CollectionType[nCount];
                }
                objParam.Direction = ParamDirections[nCount];

                if (ParamDirections[nCount] == ParameterDirection.Input || ParamDirections[nCount] == ParameterDirection.InputOutput)
                {
                    objParam.Value = DBNull.Value;
                    if (Convert.ToString(Values[nCount]) == String.Empty)
                    {
                        objParam.Value = DBNull.Value;
                    }
                    else
                    {
                        objParam.Value = Values[nCount];
                    }
                }
                if (ParamDirections[nCount] == ParameterDirection.Output)
                {
                    //IF Collection Type is not null
                    if (CollectionType != null)
                    {
                        if (CollectionType[nCount] == OracleCollectionType.PLSQLAssociativeArray)
                        {
                            if ( Values[nCount].GetType().IsArray)
                            {
                                Object[] o = (Object [])Values[nCount];
                                objParam.Size = o.Length;
                            }
                            else
                            {//Case will not arise as handled in ValidateCommandParams
                                
                            }
                        }
                        else //Regular... OracleCollectionType.None
                        {
                            if ((OracleDbType)ParamTypes[nCount] == OracleDbType.Varchar2 || (OracleDbType)ParamTypes[nCount] == OracleDbType.NVarchar2)
                            {
                                objParam.Size = 32767;
                            }
                        }
                    }
                    else //Regular...
                    {
                        if ((OracleDbType)ParamTypes[nCount] == OracleDbType.Varchar2 || (OracleDbType)ParamTypes[nCount] == OracleDbType.NVarchar2)
                        {
                            objParam.Size = 32767;
                        }
                    }
                    objParam.Value = DBNull.Value;
                }
                objParam = null;
            }
            return objCommand;
        }

        /// <summary>
        /// For internal use only. The methods checks if the connection is open, else throws an exception. The method is used by other methods to check the connection state just before making database call.
        /// </summary>
        /// <param name="Connection">Connection object</param>
        private void CheckConnectionState(OracleConnection Connection)
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection Not Open. Unable To Process The SQL Query");
            }
        }
        #endregion

        #region Code Generator Helper Functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OracleType"></param>
        /// <returns></returns>
        public static String GetParamType(String OracleType)
        {
            String str = String.Empty;
            switch (OracleType.ToLower())
            {
                case "char": //
                    str = "OracleType.Char";
                    break;
                case "varchar"://
                    str = "OracleType.VarChar";
                    break;
                case "nchar"://
                    str = "OracleType.NChar";
                    break;
                case "nvarchar"://
                    str = "OracleType.NVarChar";
                    break;
                case "raw": //
                    str = "OracleType.Raw";
                    break;
                case "date":
                    str = "OracleType.Date";
                    break;
                case "decimal":
                    str = "OracleType.Decimal";
                    break;
                case "refcursor":
                    str = "OracleType.Cursor";
                    break;
                default:
                    throw (new Exception(String.Format("Parameter Of Type '{0}' Is Not Supported Currently.", OracleType)));
                    break;

            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OracleType"></param>
        /// <returns></returns>
        public static String GetMethodParamType(String OracleType)
        {
            String str = String.Empty;
            switch (OracleType.ToLower())
            {
                case "char": //
                case "varchar"://
                case "nchar"://
                case "nvarchar"://
                    str = "System.String";
                    break;

                case "raw": //
                    str = "Byte []";
                    break;

                case "date":
                    str = "System.DateTime";
                    break;

                case "decimal":
                    str = "System.Decimal";
                    break;
                
                default:
                    throw (new Exception(String.Format("Parameter Of Type '{0}' Is Not Supported Currently.", OracleType)));
                    break;

            }
            return str;
        }
        #endregion
    }
}