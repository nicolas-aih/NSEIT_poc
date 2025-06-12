using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace Utilities.FileParser
{
    public class ExcelParser
    {
        public ExcelParser()
        {

        }

        public event _UpdateStatus UpdateStatus;
        protected void UpdateUploadStatus(String FileFormatCode, String FileName, String Message, Int32 Value)
        {
            if (UpdateStatus != null)
            {
                UpdateStatus(FileFormatCode, FileName, Message, Value);
            }
        }

        public virtual System.Data.DataTable Parse(String FileName, String SheetName, String Range, System.Boolean HasHeaderRow, Field[] Fields)
        {
            System.Data.DataTable dtRecords = new DataTable();
            Field objField = null;

            if (File.Exists(FileName))
            {
                FileInfo fi = new FileInfo(FileName);
                String Hint = String.Empty;
                switch (fi.Extension)
                {
                    case ".xls":
                        Hint = String.Format("Extended Properties='Excel 12.0;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                    case ".xlsx":
                        Hint = String.Format("Extended Properties='Excel 12.0 Xml;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                    case ".xlsb":
                        Hint = String.Format("Extended Properties='Excel 12.0;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                    default:
                        Hint = String.Format("Extended Properties='Excel 12.0 Xml;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                }

                System.Data.OleDb.OleDbConnection objConnection = null;
                OleDbCommand objCommand = null;
                OleDbDataAdapter objDataAdapter = null;
                DataTable objDataTable = null;
                try
                {
                    String Connection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';", FileName) + Hint;
                    objConnection = new OleDbConnection(Connection);
                    objConnection.Open();

                    objCommand = new OleDbCommand();

                    objCommand.CommandText = String.Format("select * from [{0}{1}]", SheetName, Range); // the $ in SheetName has to come along with the sheet name...
                    objCommand.CommandType = CommandType.Text;
                    objCommand.Connection = objConnection;

                    objDataAdapter = new OleDbDataAdapter();
                    objDataAdapter.SelectCommand = objCommand;
                    objDataTable = new DataTable();
                    objDataAdapter.Fill(objDataTable);

                    objConnection.Close();
                    objConnection = null;
                    objCommand = null;
                    objDataAdapter = null;

                    if (objDataTable.Rows.Count == 0)
                    {
                        throw new Exception("Empty File : Zero Rows Found In File");
                    }

                    dtRecords.Columns.Clear();
                    for (Int32 i = 0; i < Fields.Length; i++)
                    {
                        objField = Fields[i];
                        dtRecords.Columns.Add(objField.Name, objField.Datatype).AllowDBNull = true;
                    }
                    dtRecords.Columns.Add("ROW_NUM", typeof(System.Int32));
                    dtRecords.Columns.Add("ORIGINAL_ROW", typeof(System.String));
                    dtRecords.Columns.Add("ERROR", typeof(System.String));
                    dtRecords.Columns.Add("ERROR_FLAG", typeof(System.String)).DefaultValue = "N";
                    dtRecords.Columns.Add("ERROR_ID", typeof(System.Int16));
                    dtRecords.Columns.Add("ROW_ID", typeof(System.String));

                    String str = String.Empty;
                    String fldval = String.Empty;
                    String Error = String.Empty;

                    for (Int32 i = 0; i < Fields.Length; i++)
                    {
                        objField = Fields[i];
                        if (objField.Ordinal > objDataTable.Columns.Count - 1)
                        {
                            throw new Exception(String.Format("Invalid File Format : Field Not Found : {1}", objField.Name));
                        }
                    }

                    Int32 k = 0;
                    bool IsRowEmpty;
                    foreach (DataRow drSource in objDataTable.Rows)
                    {
                        #region Added Later to remove empty rows
                        IsRowEmpty = true;
                        for (int i = 0; i < objDataTable.Columns.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(drSource[i])))
                            {
                                IsRowEmpty = false;
                                break;
                            }
                        }

                        if (IsRowEmpty)
                        {
                            continue;
                        } 
                        #endregion

                        DataRow dr = dtRecords.NewRow();
                        dr["ROW_NUM"] = k++;
                        dr["ROW_ID"] = Guid.NewGuid().ToString();
                        dr["ORIGINAL_ROW"] = str;

                        for (Int32 i = 0; i < Fields.Length; i++)
                        {
                            objField = Fields[i];

                            if (objField.Datatype == typeof(System.DateTime))
                            {
                                if (drSource[objField.Ordinal] != DBNull.Value)
                                {
                                    dr[objField.Name] = drSource[objField.Ordinal];
                                }
                                else
                                {
                                    if (objField.AllowNull)
                                    {
                                        dr[objField.Name] = DBNull.Value;
                                    }
                                    else
                                    {
                                        Error += "Field Cannot Be Blank - " + objField.DisplayName + "/";
                                    }
                                }
                                continue;
                            }

                            fldval = Convert.ToString(drSource[objField.Ordinal]).Trim();
                            if (fldval == String.Empty)
                            {
                                if (objField.AllowNull)
                                {
                                    dr[objField.Name] = DBNull.Value;
                                }
                                else
                                {
                                    Error += "Field Cannot Be Blank - " + objField.DisplayName + "/";
                                }
                            }
                            else
                            {
                                if (objField.Datatype == typeof(System.String))
                                {
                                    dr[objField.Name] = fldval;
                                }
                                if (
                                    objField.Datatype == typeof(System.Int16)
                                    || objField.Datatype == typeof(System.Int32)
                                    || objField.Datatype == typeof(System.Int64)
                                    || objField.Datatype == typeof(System.UInt16)
                                    || objField.Datatype == typeof(System.UInt32)
                                    || objField.Datatype == typeof(System.UInt64)
                                    || objField.Datatype == typeof(System.Decimal)
                                    || objField.Datatype == typeof(System.Double)
                                    )
                                {
                                    fldval = fldval.Replace(",", "");
                                    if (TextValidator.Validate(fldval, objField.Pattern))
                                    {
                                        dr[objField.Name] = Convert.ChangeType(fldval, objField.Datatype, null);
                                    }
                                    else
                                    {
                                        Error += "Invalid Value For " + objField.Name + ":" + fldval + "/";
                                    }
                                }
                            }
                        }
                        dr["ERROR_FLAG"] = Error == String.Empty ? "N" : "Y";
                        dr["ERROR"] = Error.TrimEnd('/');
                        Error = String.Empty;
                        dtRecords.Rows.Add(dr);
                        dr = null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (objConnection != null)
                    {
                        if (objConnection.State == ConnectionState.Open)
                        {
                            objConnection.Close();
                        }
                    }
                    objConnection = null;
                    objCommand = null;
                    objDataAdapter = null;
                }
            }
            else
            {
                throw new Exception("File Not Found");
            }
            return dtRecords;
        }

        public virtual System.Data.DataTable ParseAllAsData(String FileName, String SheetName, Boolean HasHeaderRow )
        {
            System.Data.DataTable dtRecords = new DataTable();

            if (File.Exists(FileName))
            {
                FileInfo fi = new FileInfo(FileName);
                String Hint = String.Empty;
                switch (fi.Extension)
                {
                    case ".xls":
                        //Hint = "Extended Properties='Excel 12.0;HDR=NO ;IMEX=1'";
                        Hint = String.Format("Extended Properties='Excel 12.0;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                    case ".xlsx":
                        //Hint = "Extended Properties='Excel 12.0 Xml;HDR=NO;IMEX=1'";
                        Hint = String.Format("Extended Properties='Excel 12.0 Xml;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                    case ".xlsb":
                        //Hint = "Extended Properties='Excel 12.0;HDR=NO;IMEX=1'";
                        Hint = String.Format("Extended Properties='Excel 12.0;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                    default:
                        //Hint = "Extended Properties='Excel 12.0 Xml;HDR=NO;IMEX=1'";
                        Hint = String.Format("Extended Properties='Excel 12.0 Xml;HDR={0};IMEX=1'", HasHeaderRow ? "YES" : "NO");
                        break;
                }

                System.Data.OleDb.OleDbConnection objConnection = null;
                OleDbCommand objCommand = null;
                OleDbDataAdapter objDataAdapter = null;
                DataTable objDataTable = null;
                try
                {
                    String Connection = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';", FileName) + Hint;
                    objConnection = new OleDbConnection(Connection);
                    objConnection.Open();

                    objCommand = new OleDbCommand();

                    objCommand.CommandText = String.Format("select * from [{0}]", SheetName); 
                    objCommand.CommandType = CommandType.Text;
                    objCommand.Connection = objConnection;

                    objDataAdapter = new OleDbDataAdapter();
                    objDataAdapter.SelectCommand = objCommand;
                    objDataTable = new DataTable();
                    objDataAdapter.Fill(objDataTable);

                    objConnection.Close();
                    objConnection = null;
                    objCommand = null;
                    objDataAdapter = null;

                    dtRecords = objDataTable;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (objConnection != null)
                    {
                        if (objConnection.State == ConnectionState.Open)
                        {
                            objConnection.Close();
                        }
                    }
                    objConnection = null;
                    objCommand = null;
                    objDataAdapter = null;
                }
            }
            else
            {
                throw new Exception("File Not Found");
            }
            return dtRecords;
        }
    }
}
