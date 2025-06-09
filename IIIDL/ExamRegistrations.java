package IIIDL;

import databases.sqlserver.Database; // Import the stub DAL helper
import databases.sqlserver.SqlDbType;
import databases.sqlserver.ParameterDirection;
import databases.sqlserver.ParamLength;
import databases.sqlserver.ProcedureResult; // Import the result wrapper

import java.util.List;
import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Date; // Using java.util.Date as equivalent to C# DateTime
import java.sql.Connection; // Just for type hinting if needed, not used in stub
import java.sql.Types; // Potential JDBC type equivalent

// Equivalent of your C# IIIDL.ExamRegistrations class
public class ExamRegistrations {

    // C# DataSet -> Java List<Map<String, Object>>
    public List<Map<String, Object>> getExamBody(String connectionString, short tntexambodyid, String chrexammode, int intuserid) {
        Database objDatabase = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet
        ProcedureResult procResult = null; // Wrapper for procedure results
        Object[] allParameters = null; // To capture out parameters, though not used by C# BLL method

        try {
            String procedureName = "stp_ADM_GetExamBodyDetails";
            String[] paramNames = new String[] { "@tntExamBodyID", "@chrExamMode", "@intUserID" };
            SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.TinyInt, SqlDbType.Char, SqlDbType.Int };
            ParamLength[] paramLengths = new ParamLength[] { new ParamLength(1, 3, 0), new ParamLength(1, 0, 0), new ParamLength(4, 10, 0) }; // Kept for fidelity
            ParameterDirection[] paramDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };
            Object[] values = new Object[] { tntexambodyid, chrexammode, intuserid };

            objDatabase = new Database(); // Instantiate the DAL helper stub

            // Call the stub's ExecProcedure method. It returns the ProcedureResult object.
            // The C# calls `ExecProcedure(..., out AllParameters, out objDataSet, true);`
            procResult = objDatabase.ExecProcedure(connectionString, procedureName, paramNames, paramTypes, paramLengths, paramDirections, values, true);

            // Extract the first result set table and output parameters from the ProcedureResult
            resultDataSetTable = procResult.getResultSetTable();
            allParameters = procResult.getAllParameters(); // Capture output parameters if any were returned by the stub

        } catch (Exception ex) {
            // In Java, simply 'throw ex;' re-throws the exception preserving the stack trace
            throw ex;
        } finally {
             // In Java, explicit nulling in finally is often unnecessary due to GC.
             // If objDatabase had real resources (like a Connection), they should be closed here.
            objDatabase = null;
        }
        return resultDataSetTable; // Return the extracted data
    }

    // C# DataSet -> Java List<Map<String, Object>>
    public List<Map<String, Object>> getTrainedApplicants(String connectionString, int userId, int examBodyId, int examCenterId, Date fromDate, Date toDate) {
        Database objDatabase = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet
        ProcedureResult procResult = null;
        Object[] allParameters = null; // To capture out parameters

        try {
            String procedureName = "STP_LIC_GetTrainedApplicantsToConfirm_New";
            String[] paramNames = new String[] { "@intUserId", "@IntExamBodyID", "@IntExamCenterID", "@dtApplicationDate", "@dtApplicationToDate" };
            SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Date, SqlDbType.Date };
            ParamLength[] paramLengths = new ParamLength[] { new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(3, 10, 0), new ParamLength(3, 10, 0) };
            ParameterDirection[] paramDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input, ParameterDirection.Input };

            // Handle DBNull.Value equivalent (use null)
            // C# FromDate.Date, ToDate.Date gets just the date part. Passing java.util.Date might pass time.
            // For the stub, java.util.Date is fine. For a real JDBC driver, you might use java.sql.Date.
            Object centerIdValue = (examCenterId <= 0) ? null : (Object)examCenterId;
            Object[] values = new Object[] { userId, examBodyId, centerIdValue, fromDate, toDate }; // Pass Date objects

            objDatabase = new Database();

            // C# calls `ExecProcedure(..., out AllParameters, out objDataSet, true);`
            procResult = objDatabase.ExecProcedure(connectionString, procedureName, paramNames, paramTypes, paramLengths, paramDirections, values, true);

            resultDataSetTable = procResult.getResultSetTable();
            allParameters = procResult.getAllParameters(); // Capture output parameters

        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        return resultDataSetTable;
    }

    // This method was commented out in the C# source.
    // public DataSet ConfirmExamination(...) { ... }


    // C# DataSet -> Java List<Map<String, Object>>
    public List<Map<String, Object>> getPaymentMode(String connectionString, int inttblmstinsureruserid) {
        Database objDatabase = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet
        ProcedureResult procResult = null;
        Object[] allParameters = null; // To capture out parameters

        try {
            String procedureName = "STP_LIC_GetPaymentMode";
            String[] paramNames = new String[] { "@intTblMstInsurerUserID" };
            SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.Int };
            ParamLength[] paramLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
            ParameterDirection[] paramDirections = new ParameterDirection[] { ParameterDirection.Input };
            Object[] values = new Object[] { inttblmstinsureruserid };

            objDatabase = new Database();

            // C# calls `ExecProcedure(..., out AllParameters, out objDataSet, true);`
            procResult = objDatabase.ExecProcedure(connectionString, procedureName, paramNames, paramTypes, paramLengths, paramDirections, values, true);

            resultDataSetTable = procResult.getResultSetTable();
            allParameters = procResult.getAllParameters(); // Capture output parameters

        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        return resultDataSetTable;
    }

    // C# DataTable -> Java List<Map<String, Object>>
    // This method reads from an Excel file using OLEDB. It needs reimplementation.
    // The C# IIIDL layer itself reads the Excel file here. This is unusual for a
    // typical layered architecture (UI/BLL usually handle file I/O, DAL handles DB).
    // We will translate this directly but note that this logic might better reside
    // in the BLL or a dedicated Utility layer in a Java application.
    // For this translation, we will use the ExcelParser stub defined in utilities.fileparser
    // which was used by the BLL in the C# code. This makes the dependency flow clearer.
    public List<Map<String, Object>> getExcelData(String strExcelFile) {
         System.out.println("IIIDL.ExamRegistrations.getExcelData called for file: " + strExcelFile);
         // In the original C# this method used OLEDB directly.
         // However, in the provided C# IIIBL.ExamRegistration class, the BulkUploadExamRegData method
         // calls Utilities.FileParser.ExcelParser.ParseAllAsData instead of this IIIDL method.
         // This indicates this IIIDL.GetExcelData method might be unused or deprecated in the BLL provided.
         // For direct translation of the *provided* IIIDL code, we could implement OLEDB equivalent (complex!).
         // Given the BLL uses a utility class, let's assume the Excel parsing should happen outside DAL.
         // Let's make this method throw an exception or return an empty list,
         // as the BLL uses a different method for Excel parsing.
         System.err.println("IIIDL.ExamRegistrations.getExcelData is likely deprecated or unused by the provided BLL code.");
         System.err.println("BLL uses Utilities.FileParser.ExcelParser instead.");
         // Returning an empty list to allow compilation/basic flow.
         return new ArrayList<>();
         // If this method *were* used by the BLL, you'd replace the OLEDB code with Apache POI here.
    }

    // C# DataSet -> Java List<Map<String, Object>>
    // C# DataTable Exceldata -> Java List<Map<String, Object>> excelDataList
    // C# SqlDbType.Structured -> Requires specific handling in Java JDBC (e.g., SQLServerDataTable or XML/JSON).
    // For the stub, we pass the List<Map> as the parameter value.
    // The C# method calls ExecProcedure with `out AllParameters, out objDataSet`.
    // This implies it expects a DataSet and output parameters back.
    public List<Map<String, Object>> bulkUploadToDatabase(String connectionString, List<Map<String, Object>> excelDataList, int intuserid) {
        Database objDatabase = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet
        ProcedureResult procResult = null;
        Object[] allParameters = null; // To capture output parameters if any were returned

        try {
            String procedureName = "STP_LIC_BulkUploadExamRegistration_New";
            String[] paramNames = new String[] { "@ExcelData", "@intUserID" };
            SqlDbType[] paramTypes = new SqlDbType[] { SqlDbType.Structured, SqlDbType.Int };
            ParamLength[] paramLengths = new ParamLength[] { new ParamLength(-1, 0, 0), new ParamLength(4, 10, 0) }; // -1 for max size
            ParameterDirection[] paramDirections = new ParameterDirection[] { ParameterDirection.Input, ParameterDirection.Input };
            Object[] values = new Object[] { excelDataList, intuserid }; // Pass the List<Map> as the structured parameter

            objDatabase = new Database();

            // Call ExecProcedure. C# signature implies it expects DataSet and AllParameters back.
            // Let's use the 'readDataSet = true' version in our stub call to simulate DataSet return.
            procResult = objDatabase.ExecProcedure(connectionString, procedureName, paramNames, paramTypes, paramLengths, paramDirections, values, true);

            resultDataSetTable = procResult.getResultSetTable();
             allParameters = procResult.getAllParameters(); // Capture output parameters if the stub provides them

             // Note: The C# BLL method BulkUploadExamRegData does *not* seem to read anything specific
             // from 'objDataSet' or 'AllParameters' returned by THIS DAL call according to the *uncommented* code.
             // If the SP returned status/batch via output params, the BLL might need to read them here
             // in a real Java implementation using procResult.getAllParameters().
             // But based *only* on the uncommented C#, we just return the DataSet equivalent.


        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        return resultDataSetTable; // Return the result set from the SP
    }

}