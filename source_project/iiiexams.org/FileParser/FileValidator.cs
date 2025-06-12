using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Reflection;

namespace Utilities.FileParser
{
	/// <summary>
	/// Summary description for FileValidator.
	/// </summary>
	public delegate void _QueryData (String FileFormatCode,System.String Param, out System.Object Data);
	public delegate void _UpdateStatus ( String FileFormatCode, String FileName, String Message, Int32 Value );
    public delegate void _Validate(ref DataTable dtDataTable, String FileName, String FileType, out Int32 TotalRecords, out Int32 ValidRecords, out Int32 SuccessRecords, out Int32 ErrorRecords);
	public abstract class FileDataProcessor
	{
		//public String LogPath = Application.StartupPath + "\\UploadTrace.log";
        public virtual event _QueryData evtQueryData;
        public virtual event _UpdateStatus UpdateStatus;

        public virtual String MiscValidation(String strFileNameAndPath) { return String.Empty; }
        public abstract void Validate(ref DataTable dtDataTable, String FileName, String FileType, out Int32 TotalRecords, out Int32 ValidRecords, out Int32 SuccessRecords, out Int32 ErrorRecords);

        //public virtual void QueryData(String FileFormatCode, System.String Param, out System.Object Data)
        //{
        //    Data = new object();
        //    if (evtQueryData != null)
        //    {
        //        evtQueryData(FileFormatCode,Param, out Data);
        //    }
        //}

        //public virtual void UpdateUploadStatus(String FileFormatCode, String FileName, String Message, Int32 Value )
        //{
        //    if (UpdateStatus != null)
        //    {
        //        UpdateStatus(FileFormatCode, FileName, Message, Value );
        //    }
        //}

		#region Async Calls
			_Validate dlgValidate;
			public IAsyncResult BeginValidate(ref DataTable dtDataTable, String FileName, String FileType, AsyncCallback asyncCallback,  Object o)
			{
				dlgValidate = new _Validate(Validate);
				Int32 TotalRecords;
				Int32 ValidRecords;
				Int32 SuccessRecords;
				Int32 ErrorRecords;

				return dlgValidate.BeginInvoke(ref dtDataTable,  FileName,  FileType, out  TotalRecords, out  ValidRecords, out  SuccessRecords, out  ErrorRecords, asyncCallback,  o);
			}
			public void EndValidate( out Int32 TotalRecords,out Int32 ValidRecords, out Int32 SuccessRecords, out Int32 ErrorRecords, IAsyncResult result)
			{
				DataTable dt = new DataTable();
				dlgValidate.EndInvoke(ref dt,out TotalRecords,out ValidRecords, out SuccessRecords, out ErrorRecords, result);
			}
		#endregion Async Calls

		public virtual void WriteTrace(String Hint)
		{

		}
	}

    public class ParseAndValidate
    {
        public ParseAndValidate()
        {
        }

        public DataTable Process(String FileName, String FileType, Int32 HeaderRowCount, Boolean IsFixedWidth, String Delimiter, Field []Fields, Int32 FooterRowCount, out System.Collections.Specialized.StringCollection Headers, out System.Collections.Specialized.StringCollection Footers,
            String ValidatorAssemblyName, String ValidatorClassName, out Int32 TotalRecords, out Int32 ValidRecords, out Int32 SuccessRecords, out Int32 ErrorRecords)
        {
            DataTable objDataTable = null;

            TextParser objTextParser = new TextParser();
            objDataTable = objTextParser.Parse(FileName, HeaderRowCount, IsFixedWidth, Delimiter, Fields, FooterRowCount, out Headers, out Footers);

            //System.Reflection.Assembly asm= System.Reflection.Assembly.LoadWithPartialName(ValidatorAssemblyName); //Assumption the assembly with the class is within the same folder as the executing exe...
            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(ValidatorAssemblyName); //Assumption the assembly with the class is within the same folder as the executing exe...
            FileDataProcessor objFileDataProcessor = (FileDataProcessor ) asm.CreateInstance(ValidatorClassName, false, BindingFlags.CreateInstance , null,null, null,null );
            asm = null;

            objFileDataProcessor.Validate(ref objDataTable, FileName, FileType, out TotalRecords, out ValidRecords, out SuccessRecords, out ErrorRecords);

            return objDataTable;
            //Get DataTable Using TextParser;
            //Load Appropriate Class And Call Validate...
        }
    }

    public enum FileTypes
    {
        Delimited,
        FixedWidth,
        Excel
    }
}
