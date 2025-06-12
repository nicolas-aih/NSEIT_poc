using System;
using System.Data;
using System.Data.SqlClient;

namespace Databases.SQLServer
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

        /// <summary>
        /// For internal use only. Validates the command parameter for the correctness. 
        /// 1. Checks for the lenghts of array which should be equal for all the arrays.
        /// 2. Checks for the parameter type. Currently only SqlDbType.Xml, SqlDbType.Udt, SqlDbType.Structured, SqlDbType.Variant , SqlDbType.NText, SqlDbType.Text, SqlDbType.Image are not supported.
        /// </summary>
        /// <param name="Params">String array containing the parameter names</param>
        /// <param name="ParamTypes">System.Data.SqlDbType typecasted to Int32 array containing the parameter types</param>
        /// <param name="ParamLengths">ParamLength Array for lengths of Parameters</param>
        /// <param name="ParamDirections">System.Data.ParameterDirection Array of ParameterDirection in/out/in-out etc</param>
        /// <param name="Values">Object Array of parameter values</param>
        private void ValidateCommandParams(System.String[] Params, SqlDbType[] ParamTypes, ParamLength[] ParamLengths, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values)
        {
            System.Int32 nCount = 0;
            if (Params != null)
            {
                if ((Params.Length != ParamTypes.Length) || (Params.Length != ParamDirections.Length) || (Params.Length != Values.Length) || (Params.Length != ParamLengths.Length))
                {
                    throw (new Exception("The Element's Count In Parameters / Parameters Type / Parameters Direction / Parameters Values Array Is Not Same"));
                }
                for (nCount = 0; nCount < ParamTypes.Length; nCount++)
                {
                    SqlDbType PType = ParamTypes[nCount];
                    if (PType == SqlDbType.Xml || PType == SqlDbType.Udt || PType == SqlDbType.Variant ||
                        PType == SqlDbType.NText || PType == SqlDbType.Text || PType == SqlDbType.Image
                        )
                    {
                        throw (new Exception(String.Format("Parameter Of Type '{0}' Are Not Supported Currently.", Enum.GetName(typeof(SqlDbType), PType))));
                    }
                }
            }
        }

        /// <summary>
        /// For internal use only. Using the parameters; Prepares and returns the SQLCommand object for executing the procedure.
        /// </summary>
        /// <param name="ProcedureName">Name of the stored procedure to be executed</param>
        /// <param name="Params">String srray of parameter names</param>
        /// <param name="ParamTypes">Int32 srray of parameter type. SQLDBType enumerator typecasted to Int32</param>
        /// <param name="ParamLengths">ParamLength struct array indicating the lenght of the  parameter</param>
        /// <param name="ParamDirections">System.Data.ParameterDirection array indicating the parameter direction</param>
        /// <param name="Values">System.Object array of the parameter values</param>
        /// <returns></returns>
        private SqlCommand PrepareCommand(System.String ProcedureName, System.String[] Params, System.Data.SqlDbType [] ParamTypes, ParamLength[] ParamLengths, System.Data.ParameterDirection[] ParamDirections, System.Object[] Values)
        {
            ValidateCommandParams(Params, ParamTypes, ParamLengths, ParamDirections, Values);
            SqlCommand objCommand = new SqlCommand();
            Int32 nCount = 0;
            objCommand.CommandText = ProcedureName;
            objCommand.CommandType = System.Data.CommandType.StoredProcedure;
            for (nCount = 0; nCount < Params.Length; nCount++)
            {
                String ParamName = Params[nCount].StartsWith("@") ? Params[nCount] : "@" + Params[nCount];
                SqlParameter objParam = objCommand.Parameters.Add(ParamName, (SqlDbType)ParamTypes[nCount]);
                objParam.Direction = ParamDirections[nCount];
                switch (ParamTypes[nCount])
                {
                    case SqlDbType.Char:
                    case SqlDbType.VarChar:
                    case SqlDbType.NChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Binary:
                    case SqlDbType.VarBinary:
                        objParam.Size = ParamLengths[nCount].Size;
                        break;

                    case SqlDbType.UniqueIdentifier:
                    case SqlDbType.Bit:
                    case SqlDbType.TinyInt:
                    case SqlDbType.SmallInt:
                    case SqlDbType.Int:
                    case SqlDbType.BigInt:
                    case SqlDbType.Float:
                    case SqlDbType.Real:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                    case SqlDbType.DateTime2:
                    case SqlDbType.Time:
                    case SqlDbType.Timestamp:
                    case SqlDbType.Structured:
                        break;

                    case SqlDbType.SmallMoney:
                    case SqlDbType.Money:
                    case SqlDbType.Decimal:
                        objParam.Precision = (byte)ParamLengths[nCount].Precision;
                        objParam.Scale = (byte)ParamLengths[nCount].Scale;
                        break;
                }

                if (ParamDirections[nCount] == ParameterDirection.Input || ParamDirections[nCount] == ParameterDirection.InputOutput)
                {
                    objParam.Value = DBNull.Value;
                    if (ParamTypes[nCount] == SqlDbType.Structured)
                    {
                        objParam.Value = Values[nCount];
                    }
                    else
                    {
                        if (Convert.ToString(Values[nCount]) == String.Empty)
                        {
                            objParam.Value = DBNull.Value;
                        }
                        else
                        {
                            objParam.Value = Values[nCount];
                        }
                    }
                }
                objParam = null;
            }
            objCommand.Parameters.Add("@_ReturnValue", System.Data.SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            return objCommand;
        }

        /// <summary>
        /// For internal use only. The methods checks if the connection is open, else throws an exception. The method is used by other methods to check the connection state just before making database call.
        /// </summary>
        /// <param name="Connection">Connection object</param>
        private void CheckConnectionState(SqlConnection Connection)
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new Exception("Connection Not Open. Unable To Process The SQL Query");
            }
        }
        #endregion

        #region Code Generator Helper Functions
       
        public static String GetParamType(String SQLType, Boolean IsUserDefined, Boolean IsTableType)
        {
            String str = String.Empty;
            switch (SQLType.ToLower())
            {
                case "char": //
                    str = "SqlDbType.Char";
                    break;
                case "varchar"://
                    str = "SqlDbType.VarChar";
                    break;
                case "nchar"://
                    str = "SqlDbType.NChar";
                    break;
                case "nvarchar"://
                    str = "SqlDbType.NVarChar";
                    break;
                case "binary": //
                    str = "SqlDbType.Binary";
                    break;
                case "varbinary"://
                    str = "SqlDbType.VarBinary";
                    break;
                case "uniqueidentifier": //
                    str = "SqlDbType.UniqueIdentifier";
                    break;
                case "bit": //
                    str = "SqlDbType.Bit";
                    break;
                case "tinyint"://
                    str = "SqlDbType.TinyInt";
                    break;
                case "smallint"://
                    str = "SqlDbType.SmallInt";
                    break;
                case "int"://
                    str = "SqlDbType.Int";
                    break;
                case "bigint": //
                    str = "SqlDbType.BigInt";
                    break;
                case "float":
                    str = "SqlDbType.Float";
                    break;
                case "real":
                    str = "SqlDbType.Real";
                    break;
                case "smalldatetime":
                    str = "SqlDbType.SmallDateTime";
                    break;
                case "date":
                    str = "SqlDbType.Date";
                    break;
                case "datetime":
                    str = "SqlDbType.DateTime";
                    break;
                case "datetime2":
                    str = "SqlDbType.DateTime2";
                    break;
                case "time":
                    str = "SqlDbType.Time";
                    break;
                case "timestamp":
                    str = "SqlDbType.Timestamp";
                    break;
                case "decimal":
                    str = "SqlDbType.Decimal";
                    break;
                case "smallmoney":
                    str = "SqlDbType.SmallMoney";
                    break;
                case "money":
                    str = "SqlDbType.Money";
                    break;
                case "numeric":
                    str = "SqlDbType.Decimal";
                    break;
                case "structured":
                    str = "SqlDbType.Structured";
                    break;
                /*
            case "sysname":
            case "text": //
            case "ntext": //
            case "sql_variant": //
            case "xml": //
            case "geography": //
            case "geometry": //
            case "hierarchyid"://
            case "image": //
            case "datetimeoffset":/*/
                default:
                    if (IsUserDefined && IsTableType)
                    {
                        str = "SqlDbType.Structured";
                    }
                    else
                    {
                        str = "<<Appropriate Type Goes Here>>";
                    }
                    //throw (new Exception(String.Format("Parameter Of Type '{0}' Is Not Supported Currently.", SQLType)));
                    break;

            }
            return str;
        }

 
        public static String GetMethodParamType(String SQLType, Boolean IsUserDefined, Boolean IsTableType)
        {
            String str = String.Empty;
            switch (SQLType.ToLower())
            {
                case "char": //
                case "varchar"://
                case "nchar"://
                case "nvarchar"://
                case "uniqueidentifier": //
                    str = "System.String";
                    break;

                case "binary": //
                case "varbinary"://
                    str = "Byte []";
                    break;

                case "bit": //
                    str = "SqlDbType.Bit";
                    break;

                case "tinyint"://
                    str = "System.Int16";
                    break;

                case "smallint"://
                    str = "System.Int16";
                    break;

                case "int"://
                    str = "System.Int32";
                    break;

                case "bigint": //
                    str = "System.Int64";
                    break;

                case "smalldatetime":
                    str = "System.DateTime";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "time":
                    str = "System.DateTime";
                    break;
                case "timestamp":
                    str = "System.Byte []";
                    break;

                case "float":
                case "real":
                case "decimal":
                case "smallmoney":
                case "money":
                case "numeric":
                    str = "System.Decimal";
                    break;

                case "structured":
                    str = "System.Data.DataTable";
                    break;
                /*
            case "sysname":
            case "text": //
            case "ntext": //
            case "sql_variant": //
            case "xml": //
            case "geography": //
            case "geometry": //
            case "hierarchyid"://
            case "image": //
            case "datetimeoffset":/*/
                default:
                    if (IsUserDefined && IsTableType)
                    {
                        str = "System.Data.DataTable";
                    }
                    else
                    {
                        str = "<<Appropriate Type Goes Here>>";
                    }
                    //throw (new Exception(String.Format("Parameter Of Type '{0}' Is Not Supported Currently.", SQLType)));
                    break;

            }
            return str;
        }
        #endregion
    }
}