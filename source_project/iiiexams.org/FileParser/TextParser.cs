using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
//using System.Text.RegularExpressions;
using System.Threading;
//using System.Linq;

namespace Utilities.FileParser
{
    public class TextParser
    {
        public TextParser()
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
        public virtual System.Data.DataTable Parse(String FileName, System.Int32 HeaderRowCount, System.Boolean IsFixedWidth, System.String Delimiter, Field[] Fields, System.Int32 FooterRowCount, out System.Collections.Specialized.StringCollection Headers, out System.Collections.Specialized.StringCollection Footers)
        {
            System.Data.DataTable dtRecords = new DataTable();
            System.Int32 nCount = 0;
            System.Int32 nTotalCount = 0;
            Field objField = null;

            if (File.Exists(FileName))
            {
                System.IO.StreamReader strm = new StreamReader(FileName);
                lock (strm)
                {
                    while (strm.ReadLine() != null)
                    {
                        nTotalCount++;
                    }
                    strm.Close();
                    if (nTotalCount > 0)
                    {
                        if (nTotalCount < (HeaderRowCount + FooterRowCount))
                        {
                            throw new Exception("Invalid File : The Total Number Of Rows Is Less Than The Sum Of Header Row Count And Footer Row Count");
                        }
                        if (nTotalCount == (HeaderRowCount + FooterRowCount))
                        {
                            throw new Exception("Empty File : Zero Processable Rows Found In File.\nTotal Row Count Equals Sum Of Header Row Count And Footer Row Count");
                        }
                    }
                    else
                    {
                        throw new Exception("Empty File : Zero Rows Found In File");
                    }
                }

                strm = new StreamReader(FileName);
                try
                {
                    dtRecords.Columns.Clear();
                    for (Int32 i = 0; i < Fields.Length; i++)
                    {
                        objField = Fields[i];
                        dtRecords.Columns.Add(objField.Name, objField.Datatype).AllowDBNull = true;
                    }
                    dtRecords.Columns.Add("ORIGINAL_ROW", typeof(System.String));
                    dtRecords.Columns.Add("ERROR", typeof(System.String));
                    dtRecords.Columns.Add("ERROR_FLAG", typeof(System.String)).DefaultValue = "N";
                    //dtRecords.Columns.Add("ERROR_ID", typeof(System.Int16));
                    dtRecords.Columns.Add("ROW_ID", typeof(System.Int32));

                    Headers = new System.Collections.Specialized.StringCollection();
                    Headers.Clear();
                    Footers = new System.Collections.Specialized.StringCollection();
                    Footers.Clear();
                    if (!IsFixedWidth && Delimiter.Equals("\\t"))
                    {
                        Delimiter = "\t";
                    }

                    lock (strm)
                    {
                        String str = String.Empty;
                        Int32 RowNum = 0;
                        String Error = String.Empty;

                        while ((str = strm.ReadLine()) != null)
                        {
                            String strTemp = String.Copy(str) ;
                            
                            RowNum++;
                            Int32 k = RowNum;
                            if (strTemp.Trim() == String.Empty)
                            {
                                continue;
                            }

                            nCount++;
                            if (nCount <= HeaderRowCount)
                            {
                                Headers.Add(strTemp);
                            }
                            if (nCount > (nTotalCount - FooterRowCount))
                            {
                                Footers.Add(strTemp);
                            }
                            if (nCount > HeaderRowCount && nCount <= (nTotalCount - FooterRowCount)) //Ignore header & footer line...
                            {
                                #region FixedWidth
                                if (IsFixedWidth)
                                {//Fixed Width
                                    DataRow dr = dtRecords.NewRow();
                                    dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
                                    dr["ORIGINAL_ROW"] = str;
                                    for (Int32 i = 0; i < Fields.Length; i++)
                                    {
                                        objField = Fields[i];

                                        if (objField.Ordinal + objField.Length > str.Length)
                                        {
                                            throw new Exception(String.Format("Invalid File Format : Row {0} / Field Not Found : {1}", RowNum, objField.Name));
                                        }
                                        String fldval = String.Empty;
                                        fldval = str.Substring(objField.Ordinal, objField.Length).Trim();
                                        if (fldval == String.Empty || fldval == "-")
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
                                            if (objField.Datatype == typeof(System.DateTime))
                                            {
                                                if (TextValidator.Validate(fldval.ToLower(), objField.Pattern))
                                                {
                                                    dr[objField.Name] = DateTime.ParseExact(fldval, objField.Format, null);
                                                }
                                                else
                                                {
                                                    Error += "Invalid Value For " + objField.Name + ":" + fldval + "/";
                                                }
                                            }
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
                                }
                                #endregion FixedWidth

                                #region Delimited
                                if (!IsFixedWidth)
                                {//Delimited...
                                    DataRow dr = dtRecords.NewRow();
                                    dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
                                    dr["ORIGINAL_ROW"] = str;

                                    String[] strFieldsTemp = str.Split(Delimiter.ToCharArray());
                                    for (Int32 i = 0; i < Fields.Length; i++)
                                    {
                                        objField = Fields[i];

                                        if (objField.Ordinal > strFieldsTemp.Length - 1)
                                        {
                                            throw new Exception(String.Format("Invalid File Format : Row {0} / Field Not Found : {1}", RowNum, objField.Name));
                                        }
                                        String fldval = String.Empty;
                                        fldval = strFieldsTemp[objField.Ordinal].Trim();

                                        if (fldval == String.Empty || fldval == "-")
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
                                            if (objField.Datatype == typeof(System.DateTime))
                                            {
                                                if (TextValidator.Validate(fldval.ToLower(), objField.Pattern))
                                                {
                                                    fldval = fldval.ToUpper();
                                                    dr[objField.Name] = DateTime.ParseExact(fldval, objField.Format, null);
                                                }
                                                else
                                                {
                                                    Error += "Invalid Value For " + objField.Name + ":" + fldval + "/";
                                                }
                                            }
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
                                }
                                #endregion Delimited
                            }
                        }
                        strm.Close();
                    }
                }
                catch (Exception ex)
                {
                    strm.Close();
                    throw ex;
                } 
                finally
                {
                     strm = null;
                }
            }
            else
            {
                throw new Exception("File Not Found");
            }
            return dtRecords;
        }

        public System.Data.DataTable Parse2(String FileName, System.Int32 HeaderRowCount, System.Boolean IsFixedWidth, System.String Delimiter, Field[] Fields, List<String> ExpectedHeaders, out System.Collections.Specialized.StringCollection Headers, out String Message)
        {
            System.Data.DataTable dtRecords = new DataTable();
            System.Int32 nCount = 0;
            System.Int32 nTotalCount = 0;
            Field objField = null;
            Message = String.Empty ;
            //Create the table and out params in any case for proper message ....
            dtRecords.Columns.Clear();
            for (Int32 i = 0; i < Fields.Length; i++)
            {
                objField = Fields[i];
                dtRecords.Columns.Add(objField.Name, objField.Datatype).AllowDBNull = true;
            }
            dtRecords.Columns.Add("ORIGINAL_ROW", typeof(System.String));
            dtRecords.Columns.Add("MESSAGE", typeof(System.String));
            dtRecords.Columns.Add("ERROR_FLAG", typeof(System.String)).DefaultValue = "N";
            //dtRecords.Columns.Add("ERROR_ID", typeof(System.Int16));
            dtRecords.Columns.Add("ROW_ID", typeof(System.Int32));

            Headers = new System.Collections.Specialized.StringCollection();
            Headers.Clear();
            //End Create the table and out params in any case for proper message ....

            if (HeaderRowCount != ExpectedHeaders.Count)
            {
                throw new System.ArgumentException("The HeaderRowCount and no of elements in ExpectedHeaders do not match");
            }

            if (File.Exists(FileName))
            {
                System.IO.StreamReader strm = new StreamReader(FileName);
                lock (strm)
                {
                    while (strm.ReadLine() != null)
                    {
                        nTotalCount++;
                    }
                    strm.Close();
                    if (nTotalCount > 0)
                    {
                        if (nTotalCount < HeaderRowCount)
                        {
                            Message = "Invalid File : The Total Number Of Rows Is Less Than The Header Row Count";
                        }
                        if (nTotalCount == HeaderRowCount)
                        {
                            Message = "Empty File : Zero Processable Rows Found In File. Total Row Count Equals the Header Row Count";
                        }
                    }
                    else
                    {
                        Message = "Empty File : Zero Rows Found In File";
                    }
                }
                if (Message != String.Empty)
                {
                    return dtRecords;
                }

                strm = new StreamReader(FileName);
                try
                {
                    if (!IsFixedWidth && Delimiter.Equals("\\t"))
                    {
                        Delimiter = "\t";
                    }

                    lock (strm)
                    {
                        String str = String.Empty;
                        Int32 RowNum = 0;
                        String Error = String.Empty;

                        while ((str = strm.ReadLine()) != null)
                        {
                            DataRow dr = dtRecords.NewRow();
                            String strTemp = String.Copy(str);

                            RowNum++;
                            Int32 k = RowNum;

                            nCount++;
                            if (nCount <= HeaderRowCount)
                            {
                                Headers.Add(strTemp);
                            }
                           
                            if (nCount > HeaderRowCount ) //Ignore header
                            {
                                if (strTemp.Trim() == String.Empty)
                                {
                                    dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
                                    dr["ORIGINAL_ROW"] = str;
                                    dr["ERROR_FLAG"] = "Y";
                                    dr["MESSAGE"] = "Empty Row Found";
                                    dtRecords.Rows.Add(dr);
                                    continue;
                                }

                                #region FixedWidth
                                if (IsFixedWidth)
                                {//Fixed Width
                                    dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
                                    dr["ORIGINAL_ROW"] = str;
                                    for (Int32 i = 0; i < Fields.Length; i++)
                                    {
                                        objField = Fields[i];

                                        if (objField.Ordinal + objField.Length > str.Length)
                                        {
                                            Error += String.Format("Invalid File Format: Field Not Found : {1}. ", objField.Name);
                                        }
                                        else
                                        {
                                            String fldval = String.Empty;
                                            fldval = str.Substring(objField.Ordinal, objField.Length).Trim();
                                            if (fldval == String.Empty || fldval == "-")
                                            {
                                                if (objField.AllowNull)
                                                {
                                                    dr[objField.Name] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    Error += "Field Cannot Be Blank - " + objField.DisplayName + ". ";
                                                }
                                            }
                                            else
                                            {
                                                if (objField.Datatype == typeof(System.DateTime))
                                                {
                                                    if (TextValidator.Validate(fldval.ToLower(), objField.Pattern))
                                                    {
                                                        dr[objField.Name] = DateTime.ParseExact(fldval, objField.Format, null);
                                                    }
                                                    else
                                                    {
                                                        Error += "Invalid Value For " + objField.Name + ":" + fldval + ". ";
                                                    }
                                                }
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
                                                        Error += "Invalid Value For " + objField.Name + ":" + fldval + ". ";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dr["ERROR_FLAG"] = Error == String.Empty ? "N" : "Y";
                                    dr["MESSAGE"] = Error.Trim();
                                    Error = String.Empty;

                                    dtRecords.Rows.Add(dr);
                                }
                                #endregion FixedWidth

                                #region Delimited
                                if (!IsFixedWidth)
                                {//Delimited...
                                    dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
                                    dr["ORIGINAL_ROW"] = str;

                                    String[] strFieldsTemp = str.Split(Delimiter.ToCharArray());
                                    for (Int32 i = 0; i < Fields.Length; i++)
                                    {
                                        objField = Fields[i];

                                        if (objField.Ordinal > strFieldsTemp.Length - 1)
                                        {
                                            Error += String.Format("Invalid File Format: Field Not Found : {1} . ", objField.Name);
                                        }
                                        else
                                        {
                                            String fldval = String.Empty;
                                            fldval = strFieldsTemp[objField.Ordinal].Trim();

                                            if (fldval == String.Empty || fldval == "-")
                                            {
                                                if (objField.AllowNull)
                                                {
                                                    dr[objField.Name] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    Error += "Field Cannot Be Blank - " + objField.DisplayName + ". ";
                                                }
                                            }
                                            else
                                            {
                                                if (objField.Datatype == typeof(System.DateTime))
                                                {
                                                    if (TextValidator.Validate(fldval.ToLower(), objField.Pattern))
                                                    {
                                                        fldval = fldval.ToUpper();
                                                        dr[objField.Name] = DateTime.ParseExact(fldval, objField.Format, null);
                                                    }
                                                    else
                                                    {
                                                        Error += "Invalid Value For " + objField.Name + ":" + fldval + ". ";
                                                    }
                                                }
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
                                                        Error += "Invalid Value For " + objField.Name + ":" + fldval + ". ";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dr["ERROR_FLAG"] = Error == String.Empty ? "N" : "Y";
                                    dr["MESSAGE"] = Error.TrimEnd();
                                    Error = String.Empty;
                                    dtRecords.Rows.Add(dr);
                                }
                                #endregion Delimited
                            }
                        }
                        strm.Close();
                    }

                    for (Int32 kl = 0; kl < ExpectedHeaders.Count; kl++)
                    {
                        if (ExpectedHeaders[kl] != Headers[kl])
                        {
                            Message = "Expected File Header(s) Not Found. Please check the template";
                        }
                    }
                }
                catch (Exception ex)
                {
                    strm.Close();
                    throw ex;
                }
                finally
                {
                    strm = null;
                }
            }
            else
            {
                Message = "File Not Found";
            }
            return dtRecords;
        }

        public virtual void ProcessDelimited(String str, String Delimiter, Field[] Fields, DataTable dtRecords, Int32 RowNum)
        {
            DataRow dr = dtRecords.NewRow();
            dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
            dr["ORIGINAL_ROW"] = str;
            
            Field objField = null;
            String[] strFieldsTemp = str.Split(Delimiter.ToCharArray());
            String Error = String.Empty;
            for (Int32 i = 0; i < Fields.Length; i++)
            {
                objField = Fields[i];

                if (objField.Ordinal > strFieldsTemp.Length - 1)
                {
                    throw new Exception(String.Format("Invalid File Format : Row {0} / Field Not Found : {1}", RowNum, objField.Name));
                }
                String fldval = String.Empty;
                fldval = strFieldsTemp[objField.Ordinal].Trim();

                if (fldval == String.Empty || fldval == "-")
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
                    if (objField.Datatype == typeof(System.DateTime))
                    {
                        if (TextValidator.Validate(fldval.ToLower(), objField.Pattern))
                        {
                            fldval = fldval.ToUpper();
                            dr[objField.Name] = DateTime.ParseExact(fldval, objField.Format, null);
                        }
                        else
                        {
                            Error += "Invalid Value For " + objField.Name + ":" + fldval + "/";
                        }
                    }
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
                dr["ERROR_FLAG"] = Error == String.Empty ? "N" : "Y";
                dr["ERROR"] = Error.TrimEnd('/');
                Error = String.Empty;

                dtRecords.Rows.Add(dr);
            }
        }
        public virtual void ProcessFixedWidth(String str, String Delimiter, Field[] Fields, DataTable dtRecords, Int32 RowNum)
        {
            DataRow dr = dtRecords.NewRow();
            dr["ROW_ID"] = RowNum; // Guid.NewGuid().ToString();
            dr["ORIGINAL_ROW"] = str;

            Field objField = null;
            String Error = String.Empty;
            for (Int32 i = 0; i < Fields.Length; i++)
            {
                objField = Fields[i];

                if (objField.Ordinal + objField.Length > str.Length)
                {
                    throw new Exception(String.Format("Invalid File Format : Row {0} / Field Not Found : {1}", RowNum, objField.Name));
                }
                String fldval = String.Empty;
                fldval = str.Substring(objField.Ordinal, objField.Length).Trim();
                if (fldval == String.Empty || fldval == "-")
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
                    if (objField.Datatype == typeof(System.DateTime))
                    {
                        if (TextValidator.Validate(fldval.ToLower(), objField.Pattern))
                        {
                            dr[objField.Name] = DateTime.ParseExact(fldval, objField.Format, null);
                        }
                        else
                        {
                            Error += "Invalid Value For " + objField.Name + ":" + fldval + "/";
                        }
                    }
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
            
        }
    }
}