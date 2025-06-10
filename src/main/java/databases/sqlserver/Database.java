
package databases.sqlserver;

import java.util.List;
import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;

// This is a STUB/MOCK for the Database helper class used by IIIDL.
// In a real application, this would contain JDBC code to connect to SQL Server,
// prepare and execute stored procedures, handle parameters (including TVPs),
// and map results (ResultSet) to List<Map<String, Object>>.
public class Database {

    // Mimics the C# ExecProcedure signature that returns a DataSet and output parameters
    // The 'readDataSet' boolean is used in C# but its logic is embedded here in the stub.
    public ProcedureResult ExecProcedure(
            String connectionString,
            String procedureName,
            String[] paramNames,
            SqlDbType[] paramTypes,
            ParamLength[] paramLengths,
            ParameterDirection[] paramDirections,
            Object[] values,
            // In C#, these are 'out'. In Java, they are part of the return object.
            // out Object[] allParameters, // Simulating this via the returned ProcedureResult object
            // out DataSet objDataSet,     // Simulating this via the returned ProcedureResult object
            boolean readDataSet) {

        System.out.println("--- DAL ExecProcedure Called ---");
        System.out.println("Connection String: " + connectionString);
        System.out.println("Procedure Name: " + procedureName);
        System.out.println("Read DataSet: " + readDataSet);
        System.out.println("Parameters:");
        Map<String, Object> outputParams = new HashMap<>();
        Object[] finalParameters = new Object[paramNames.length]; // Array to hold input values and populated output values

        for (int i = 0; i < paramNames.length; i++) {
            System.out.println("  " + paramNames[i] + " (" + paramTypes[i] + ", " + paramDirections[i] + "): " + (values[i] == null ? "null" : values[i].toString()));
            finalParameters[i] = values[i]; // Copy input values

            // Simulate setting output parameters for specific procedures if needed
            if (paramDirections[i] == ParameterDirection.Output || paramDirections[i] == ParameterDirection.InputOutput) {
                // Add a dummy value for testing output parameter retrieval
                 if (procedureName.equals("STP_LIC_BulkUploadExamRegistration_New") && paramNames[i].equals("@varExamBatchNo")) {
                     finalParameters[i] = "BATCH-" + System.currentTimeMillis();
                     outputParams.put(paramNames[i], finalParameters[i]);
                 } else if (procedureName.equals("STP_LIC_BulkUploadExamRegistration_New") && paramNames[i].equals("@IntSuccess")) {
                      finalParameters[i] = 1; // Simulate success
                      outputParams.put(paramNames[i], finalParameters[i]);
                 }
                 // Add other specific output parameter simulations here
            }
        }
         System.out.println("--- End DAL ExecProcedure Call ---");


        List<Map<String, Object>> dummyResultSet = null;
        if (readDataSet) {
            // Simulate returning a dummy result set based on the procedure name
            dummyResultSet = new ArrayList<>();
            Map<String, Object> row1 = new HashMap<>();
            row1.put("Column1", "Value1");
            row1.put("Column2", 123);
            dummyResultSet.add(row1);

             Map<String, Object> row2 = new HashMap<>();
            row2.put("Column1", "Value2");
            row2.put("Column2", 456);
            dummyResultSet.add(row2);

             // Add more specific dummy data based on procedure names if the BLL expects specific columns
             if (procedureName.equals("stp_ADM_GetExamBodyDetails")) {
                  dummyResultSet = new ArrayList<>();
                  Map<String, Object> examBodyRow = new HashMap<>();
                  examBodyRow.put("ExamBodyId", 1);
                  examBodyRow.put("ExamBodyName", "IIISLA");
                  dummyResultSet.add(examBodyRow);
             } else if (procedureName.equals("STP_LIC_GetTrainedApplicantsToConfirm_New")) {
                  dummyResultSet = new ArrayList<>();
                   Map<String, Object> applicantRow = new HashMap<>();
                   applicantRow.put("ApplicantId", 789);
                   applicantRow.put("Name", "John Doe");
                   applicantRow.put("ExamCenter", "Center A");
                   dummyResultSet.add(applicantRow);
             } else if (procedureName.equals("STP_LIC_GetPaymentMode")) {
                  dummyResultSet = new ArrayList<>();
                   Map<String, Object> paymentRow = new HashMap<>();
                   paymentRow.put("PaymentModeId", 1);
                   paymentRow.put("PaymentModeName", "Online Payment");
                   dummyResultSet.add(paymentRow);
                   Map<String, Object> paymentRow2 = new HashMap<>();
                   paymentRow2.put("PaymentModeId", 2);
                   paymentRow2.put("PaymentModeName", "Credit");
                   dummyResultSet.add(paymentRow2);
             } else if (procedureName.equals("STP_LIC_BulkUploadExamRegistration_New")) {
                  // Simulate returning the uploaded data potentially with validation results
                  // In a real scenario, this SP might return a temp table or similar
                  // For the stub, let's return the input excel data with dummy upload remarks
                  List<Map<String, Object>> inputData = null;
                   for (int i = 0; i < paramNames.length; i++) {
                       if (paramTypes[i] == SqlDbType.Structured && paramNames[i].equals("@ExcelData")) {
                            inputData = (List<Map<String, Object>>) values[i]; // Cast the input data
                            break;
                       }
                   }
                   if (inputData != null) {
                       dummyResultSet = new ArrayList<>();
                       for(Map<String, Object> row : inputData) {
                           Map<String, Object> resultRow = new HashMap<>(row); // Copy all input fields
                           // Add/override columns that the SP might return (like validation results)
                           resultRow.put("IsValidRecord", row.getOrDefault("IsValidRecord", true)); // Pass validation result from BLL
                           resultRow.put("UploadRemark", row.getOrDefault("UploadRemark", "Processed")); // Pass validation remark from BLL
                           dummyResultSet.add(resultRow);
                       }
                   } else {
                       dummyResultSet = new ArrayList<>(); // Return empty if no data was input
                   }

             }
        }

        // Simulate a scalar return value (usually 0 for success)
        int dummyReturnValue = 0; // Or based on procedure logic

        return new ProcedureResult(dummyReturnValue, finalParameters, dummyResultSet);
    }

     // Mimics the C# ExecProcedure signature that only returns output parameters (no DataSet)
     // This overload wasn't used in the *uncommented* C# code provided for ExamRegistrations,
     // but is included for completeness if the commented out code becomes relevant.
    public ProcedureResult ExecProcedure(
            String connectionString,
            String procedureName,
            String[] paramNames,
            SqlDbType[] paramTypes,
            ParamLength[] paramLengths,
            ParameterDirection[] paramDirections,
            Object[] values) {
        return ExecProcedure(connectionString, procedureName, paramNames, paramTypes, paramLengths, paramDirections, values, false); // Don't expect a dataset
    }

    // Add a dummy close method to match C# resource management pattern if needed,
    // though Java GC handles it differently.
    public void close() {
        System.out.println("DAL Database object 'closed' (stub)");
    }
}