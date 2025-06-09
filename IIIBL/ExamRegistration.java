package IIIBL;

import IIIDL.ExamRegistrations; // Import the translated DAL
import utilities.Common; // Import utility classes
import utilities.TextValidator;
import utilities.fileparser.ExcelParser;

import java.util.List;
import java.util.Map;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Date; // Using java.util.Date for translation fidelity of C# DateTime
import java.util.Calendar; // For date calculations
import java.util.regex.Pattern; // For regex validation
import java.text.ParseException;
import java.text.SimpleDateFormat; // For date parsing and formatting
import java.util.Arrays; // To use Arrays.asList for the Fields array check


// Equivalent of your C# IIIBL.ExamRegistration class
public class ExamRegistration {

    // IIIDL.ExamRegistrations objDP = null; // Instance variable is kept in C#, but local variable is also used.
                                            // Let's follow the pattern used in most methods and use local variables.

    // C# DataSet -> Java List<Map<String, Object>>
    public List<Map<String, Object>> getExambody(String connectionString) {
        IIIDL.ExamRegistrations objDP = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet

        try {
            objDP = new IIIDL.ExamRegistrations();
            // Note: C# passes 0, "A", 0. Translate directly.
            resultDataSetTable = objDP.getExamBody(connectionString, (short) 0, "A", 0); // Cast 0 to short
        } catch (Exception ex) {
            throw ex; // Re-throw
        } finally {
            // In Java, explicit nulling in finally is often unnecessary due to GC.
            objDP = null;
        }
        return resultDataSetTable;
    }

    // C# DataSet -> Java List<Map<String, Object>>
    public List<Map<String, Object>> getTrainedApplicants(String connectionString, int userId, int examBodyId, int examCenterId, Date fromDate, Date toDate) {
        IIIDL.ExamRegistrations objDP = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet

        try {
            objDP = new IIIDL.ExamRegistrations();
            resultDataSetTable = objDP.getTrainedApplicants(connectionString, userId, examBodyId, examCenterId, fromDate, toDate);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDP = null;
        }
        return resultDataSetTable;
    }

    // This method was commented out in the C# source.
    // public DataSet ConfirmExamination(...) { ... }

    // C# DataSet -> Java List<Map<String, Object>>
    // C# out Boolean Status, out String Message -> Java BulkUploadResult object
    public BulkUploadResult bulkUploadExamRegData(String connectionString, String inputFile, int userid) {
        // These are the expected column headers from the Excel file
        List<String> requiredFields = Arrays.asList("IRDA URN", "Payment Mode", "Insurer Remark", "Enrollment No", "OnOrAfterDate", "EmailIds", "Batch Mode", "Scheduling Mode");

        IIIDL.ExamRegistrations objSp = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet returned by DAL
        List<Map<String, Object>> oExcelDataList = null; // Translated from DataTable read from Excel
        boolean status = true; // Translated from out Status
        String message = ""; // Translated from out Message

        try {
            ExcelParser objExcelParser = new ExcelParser();
            // C# ParseAllAsData returns DataTable, Java stub returns List<Map>
            oExcelDataList = objExcelParser.parseAllAsData(inputFile, "Sheet1$", true);

            // --- Replicate DataTable operations on List<Map> ---

            // 1. Remove columns not in the requiredFields list
            // Get the initial headers from the parsed data (from the first row keys)
            List<String> initialHeaders = oExcelDataList.isEmpty() ? new ArrayList<>() : new ArrayList<>(oExcelDataList.get(0).keySet());

            List<Map<String, Object>> processedExcelDataList = new ArrayList<>();
            for (Map<String, Object> row : oExcelDataList) {
                 Map<String, Object> processedRow = new HashMap<>();
                 for (String field : initialHeaders) { // Iterate over existing columns
                     if (requiredFields.contains(field)) {
                         processedRow.put(field, row.get(field));
                     }
                     // Columns not in requiredFields are simply not added to processedRow
                 }
                 // Ensure all required fields are present in the processed map, add null if missing
                 for (String requiredField : requiredFields) {
                      if (!processedRow.containsKey(requiredField)) {
                           processedRow.put(requiredField, null);
                      }
                 }
                 processedExcelDataList.add(processedRow);
            }
            oExcelDataList = processedExcelDataList; // Update to the processed list

            // 2. Remove specific columns if they exist (redundant after the above filter based on requiredFields,
            //    but if they were *not* in requiredFields but existed in the original Excel, the above
            //    loop would have removed them. The C# explicitly removes these three *after* checking Fields.)
            //    Let's re-add them based on the original C# logic, even if they are immediately removed.
             oExcelDataList.forEach(row -> {
                 // Add dummy values for these columns if they weren't in the original Excel,
                 // just so the subsequent remove operations don't fail on non-existent keys.
                 // This mimics DataTable behavior where you can reference columns.
                 if (!row.containsKey("ExamBatchNo")) row.put("ExamBatchNo", null);
                 if (!row.containsKey("IsValidRecord")) row.put("IsValidRecord", null);
                 if (!row.containsKey("UploadRemark")) row.put("UploadRemark", null);

                 // Now remove them as in C#
                 row.remove("ExamBatchNo");
                 row.remove("IsValidRecord");
                 row.remove("UploadRemark");
             });


            // 3. Add new columns with default values
            oExcelDataList.forEach(row -> {
                row.put("ExamBatchNo", ""); // C# adds String column
                row.put("IsValidRecord", true); // C# adds Boolean with default true
                row.put("UploadRemark", ""); // C# adds String column
            });
             // --- End DataTable operations ---


            if (oExcelDataList.isEmpty()) { // Check if the processed list is empty
                status = false;
                message = "File has no record";
            } else {
                // Check for missing required fields based on original headers
                StringBuilder missingFieldsMsg = new StringBuilder();
                 // Re-get original headers from the parser stub, assuming it's consistent
                 List<Map<String, Object>> originalExcelDataList = objExcelParser.parseAllAsData(inputFile, "Sheet1$", true);
                 List<String> originalHeaders = originalExcelDataList.isEmpty() ? new ArrayList<>() : new ArrayList<>(originalExcelDataList.get(0).keySet());

                for (String fld : requiredFields) {
                     if (!originalHeaders.contains(fld)) {
                         missingFieldsMsg.append(fld).append(",");
                     }
                }
                String s = missingFieldsMsg.toString().trim();
                if (s.endsWith(",")) {
                    s = s.substring(0, s.length() - 1);
                }

                if (!s.isEmpty()) {
                    status = false;
                    message = String.format("Invalid file format. The required fields : %s not found.", s);
                } else {
                    // --- Validation loop through rows ---
                    Pattern lowAsciiRegex = Pattern.compile(Common.regexLowAscii);
                    Pattern emailRegex = Pattern.compile(Common.regexEmail);
                    SimpleDateFormat dateFormat = new SimpleDateFormat("dd-MMM-yyyy");
                    dateFormat.setLenient(false); // Strict date parsing

                    // Calculate date range boundaries (current date + 3 days and + 30 days)
                    Calendar calendar = Calendar.getInstance();
                    calendar.add(Calendar.DAY_OF_YEAR, 3);
                    Date currDatePlus3 = calendar.getTime(); // Equivalent of C# DateTime.Now.AddDays(3)
                    String currDatePlus3Str = new SimpleDateFormat("dd-MMM-yyyy").format(currDatePlus3); // Format for comparison message

                    calendar = Calendar.getInstance(); // Reset calendar state
                    calendar.add(Calendar.DAY_OF_YEAR, 30);
                    Date currDatePlus30 = calendar.getTime(); // Equivalent of C# DateTime.Now.AddDays(30)
                    String currDatePlus30Str = new SimpleDateFormat("dd-MMM-yyyy").format(currDatePlus30); // Format for comparison message

                    //Int32 InvalidRowCount = 0; // Commented out in C#

                    for (Map<String, Object> dr : oExcelDataList) {
                        boolean isRecordValid = true;
                        StringBuilder error = new StringBuilder();

                        // --- 1. Character validation (low ASCII) ---
                         // Iterate over all columns that exist in the current row map
                         List<String> columnNamesToCheck = new ArrayList<>(dr.keySet()); // Get keys *before* potential modifications
                         columnNamesToCheck.remove("IsValidRecord"); // Exclude added columns from this check
                         columnNamesToCheck.remove("UploadRemark");
                         columnNamesToCheck.remove("ExamBatchNo");


                        for (String columnName : columnNamesToCheck) {
                            Object cellValue = dr.get(columnName);
                            String cellString = (cellValue == null) ? "" : cellValue.toString().trim();

                            if (!cellString.isEmpty()) {
                                // Check for invalid low ASCII characters
                                // C# regex.IsMatch(s) checks if the pattern occurs anywhere in the string.
                                if (lowAsciiRegex.matcher(cellString).find()) {
                                    if (error.length() > 0) error.append(", ");
                                    error.append(columnName);
                                    // Don't set isRecordValid = false and break yet, collect all invalid columns first
                                }
                            }
                        }

                        String invalidCharCols = error.toString().trim();
                        if (!invalidCharCols.isEmpty()) {
                            error.setLength(0); // Clear error builder for the main error message
                            error.append("Invalid characters found in columns: ").append(invalidCharCols);
                            isRecordValid = false;
                        }


                        // --- 2. Required field validation (IRDA URN, Payment Mode - commented out in C# but included here based on code structure) ---
                        // The C# code had commented out checks for these. Let's keep them commented in the translation logic as well.
                        /*
                        String fldvalIrdaUrn = dr.get("IRDA URN") != null ? dr.get("IRDA URN").toString().trim() : "";
                         if (fldvalIrdaUrn.isEmpty()) {
                              if (error.length() > 0) error.append(" ");
                              error.append("[IRDA URN] is required field.");
                              isRecordValid = false;
                         }

                         String fldvalPaymentMode = dr.get("Payment Mode") != null ? dr.get("Payment Mode").toString().trim() : "";
                         if (fldvalPaymentMode.isEmpty()) {
                             if (error.length() > 0) error.append(" ");
                             error.append("[Payment Mode] is required field.");
                             isRecordValid = false;
                         } else {
                              if (!(fldvalPaymentMode.equalsIgnoreCase("ONLINE PAYMENT") || fldvalPaymentMode.equalsIgnoreCase("CREDIT"))) {
                                   if (error.length() > 0) error.append(" ");
                                   error.append("Invalid [Payment Mode]. Expected - Online Payment OR Credit.");
                                   isRecordValid = false;
                              }
                         }
                        */


                        // --- 3. Date validation for "OnOrAfterDate" ---
                        String strDate = dr.get("OnOrAfterDate") != null ? dr.get("OnOrAfterDate").toString().trim() : "";

                        if (strDate.isEmpty()) {
                            if (error.length() > 0) error.append(" ");
                            error.append("[OnOrAfterDate] is required field.");
                            isRecordValid = false;
                        } else {
                            // C# uses TextValidator.ValidateDate2 and then DateTime.Compare
                            String regexDatePattern = TextValidator.getPattern(Date.class, "dd-MMM-yyyy"); // Get dummy pattern
                            if (TextValidator.validateDate2(strDate, "dd-MMM-yyyy", regexDatePattern)) {
                                try {
                                    Date d1 = dateFormat.parse(strDate); // OnOrAfterDate
                                    Date d2 = currDatePlus3; // current date + 3 days
                                    Date d3 = currDatePlus30; // current date + 30 days

                                    // C# logic: if (value >= 0 && value1 <= 0) { ... } else { Error += ... }
                                    // Compare returns negative if d1 < d2, zero if d1 == d2, positive if d1 > d2
                                    int value = d1.compareTo(d2);
                                    int value1 = d1.compareTo(d3);

                                    if (!(value >= 0 && value1 <= 0)) {
                                        if (error.length() > 0) error.append(" ");
                                        error.append("On or After Date must be between ").append(currDatePlus3Str).append(" & ").append(currDatePlus30Str).append(".");
                                        isRecordValid = false;
                                        // The C# had a 'break' here inside the date validation loop.
                                        // This 'break' would exit the *outermost* foreach (DataRow dr in oExcelData.Rows) loop.
                                        // This means if *any* row had a date range validation error, the entire row processing stopped.
                                        // This seems like potentially incorrect logic, as you'd usually want to validate *all* rows
                                        // and report errors for each. Let's assume this 'break' was intended to break out of
                                        // the date validation checks for *this row* if the format is wrong, or maybe it was a typo.
                                        // Re-reading the C# structure: the `break` is inside the `if (TextValidator.ValidateDate2(...))` block,
                                        // and inside the `else` of the date range check. Yes, it seems it intended to stop
                                        // processing further columns/validations for the *current row* if the date range is invalid,
                                        // but critically, it does NOT break the outer row loop.
                                        // The C# break is inside the else of the date range check.
                                        // So, if date range is invalid, mark invalid, add error, and stop further validation for *this row*.
                                         // To replicate, set isRecordValid and error, then use 'continue rowLoop;' if we had a label,
                                         // or simply mark invalid and let the loop for the current row finish.
                                         // Let's set isRecordValid = false and add the error message. The outer loop will continue to the next row.
                                    }
                                } catch (ParseException e) {
                                    // validateDate2 is already called and checks format. This catch might be for other issues during parsing.
                                    if (error.length() > 0) error.append(" ");
                                    error.append("Invalid date format or value for [OnOrAfterDate].");
                                    isRecordValid = false;
                                }
                            } else {
                                if (error.length() > 0) error.append(" ");
                                error.append("Invalid [OnOrAfterDate]."); // TextValidator.validateDate2 failed format/pattern check
                                isRecordValid = false;
                            }
                        }

                        // --- 4. Email validation for "EmailIds" ---
                        String emails = dr.get("EmailIds") != null ? dr.get("EmailIds").toString().trim() : "";

                        if (!emails.isEmpty()) { // Only validate if the field is not empty
                           String[] mailIds = emails.split(",");
                            boolean anyEmailInvalid = false;
                            for (String mailId : mailIds) {
                                String trimmedMailId = mailId.trim();
                                // Only check non-empty email strings after trimming
                                if (!trimmedMailId.isEmpty() && !emailRegex.matcher(trimmedMailId).matches()) {
                                    if (error.length() > 0) error.append(" ");
                                    error.append("Invalid Email Id found: ").append(trimmedMailId).append(".");
                                    isRecordValid = false;
                                    anyEmailInvalid = true;
                                    // Do NOT break the inner email loop immediately, collect all invalid emails in the cell value if multiple provided
                                }
                            }
                            // If any email was invalid within the cell value, the row is invalid.
                            if (anyEmailInvalid) {
                                isRecordValid = false;
                            }
                        }
                         // Note: The C# didn't check if EmailIds is required, only if it's not empty.

                        // --- Update row with validation result ---
                        dr.put("IsValidRecord", isRecordValid);
                        dr.put("UploadRemark", error.toString().trim());

                        // if (!isRecordValid) InvalidRowCount++; // Commented out in C#
                    } // End foreach DataRow

                    // --- Bulkupload Excel Data To Database ---
                    // Pass the validated and modified List<Map> to the DAL
                    objSp = new IIIDL.ExamRegistrations();
                    resultDataSetTable = objSp.bulkUploadToDatabase(connectionString, oExcelDataList, userid);

                    // Note: The C# BLL does not read Status/Message from the DAL call result here.
                    // Status and Message are set based on initial file checks and row validations
                    // BEFORE the DAL call. The logic flow implies if we reached here without throwing
                    // and without setting status=false during initial checks, the status is true,
                    // UNLESS the DAL call itself threw an exception (handled by the catch block below).
                    // However, the catch block sets status=false and provides a generic error.
                    // If the SP returned validation results in the DataSet, the BLL might process
                    // the 'IsValidRecord' and 'UploadRemark' columns returned in resultDataSetTable
                    // to refine the overall message *after* the DAL call, but the current C# code doesn't show this.
                    // We will return the final status/message determined before the DAL call, unless an exception occurs.
                }
            }
        } catch (Exception ex) {
            // If any exception occurs during processing (file read, validation, or DAL call)
            status = false; // Mark status as false
            message = "An error occurred during bulk upload processing: " + ex.getMessage(); // Set error message
            // If objSp was instantiated before the exception, null it in finally
            if (objSp != null) {
                 try{ objSp = null; } catch(Exception ignored){} // Ignore potential errors during nulling
            }
            throw ex; // Re-throw the original exception after setting status/message
        } finally {
            // objSp is already nulled above if exception occurred during processing before DAL call
            if (objSp != null) {
                objSp = null;
            }
        }

        // Return the results wrapped in the custom class
        // resultDataSetTable contains the data returned *by the stored procedure*.
        // The status and message reflect the outcome of the BLL's *validation* and processing steps *before* the DAL call,
        // or a general failure if an exception occurred.
        return new BulkUploadResult(resultDataSetTable, status, message);
    }

    // C# DataSet -> Java List<Map<String, Object>>
     // C# DataTable dataTable -> Java List<Map<String, Object>> dataTableList
     // This method is simpler, assuming the DataTable validation is done elsewhere before passing it here.
    public List<Map<String, Object>> bulkUploadExamRegData2(String connectionString, List<Map<String, Object>> dataTableList, int userid) {
        IIIDL.ExamRegistrations objSp = null;
        List<Map<String, Object>> resultTable = null; // Translated from DataSet

        try {
            objSp = new IIIDL.ExamRegistrations();
            // Directly call the DAL method with the List<Map>
            // C# calls ExecProcedure(..., out AllParameters, out objDataSet); (no 'true')
            // Let's assume this overload also implies readDataSet=true, as it receives objDataSet.
            // If not, the stub needs adjustment or the BLL needs to handle a null resultTable.
            // The C# signature implies it fills objDataSet. Let's use readDataSet=true in stub call.
            resultTable = objSp.bulkUploadToDatabase(connectionString, dataTableList, userid);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objSp = null;
        }
        return resultTable;
    }

    // C# DataSet -> Java List<Map<String, Object>>
    public List<Map<String, Object>> getPaymentModes(String connectionString, int insurerId) {
        IIIDL.ExamRegistrations objDP = null;
        List<Map<String, Object>> resultDataSetTable = null; // Translated from DataSet

        try {
            objDP = new IIIDL.ExamRegistrations();
            resultDataSetTable = objDP.getPaymentMode(connectionString, insurerId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDP = null;
        }
        return resultDataSetTable;
    }


    // --- Main function for testing ---
    public static void main(String[] args) {
        System.out.println("--- Testing IIIBL.ExamRegistration ---");

        ExamRegistration bll = new ExamRegistration();
        String testConnectionString = "jdbc:sqlserver://localhost:1433;databaseName=AgencyLicensingPortal;user=sa;password=sa123"; // Dummy connection string
        int dummyUserId = 303;
        String dummyExcelFilePath = "C:/Users/akliv/Downloads/titanic.csv"; // Path doesn't need to exist due to stub

        // Test GetExambody
        System.out.println("\nTesting getExambody...");
        try {
            List<Map<String, Object>> examBodies = bll.getExambody(testConnectionString);
            System.out.println("Exam Bodies received: " + examBodies);
        } catch (Exception e) {
            System.err.println("Error getting exam bodies: " + e.getMessage());
            e.printStackTrace();
        }

         // Test GetTrainedApplicants
        System.out.println("\nTesting getTrainedApplicants...");
        try {
             // Use java.util.Date for dummy dates
             Calendar cal = Calendar.getInstance();
             cal.set(2023, Calendar.NOVEMBER, 1); // Month is 0-indexed
             Date fromDate = cal.getTime();
             cal.set(2023, Calendar.DECEMBER, 31);
             Date toDate = cal.getTime();

            List<Map<String, Object>> applicants = bll.getTrainedApplicants(testConnectionString, 101, 1, 0, fromDate, toDate);
            System.out.println("Trained Applicants received: " + applicants);
        } catch (Exception e) {
            System.err.println("Error getting trained applicants: " + e.getMessage());
            e.printStackTrace();
        }

        // Test GetPaymentModes
        System.out.println("\nTesting getPaymentModes...");
        try {
            List<Map<String, Object>> paymentModes = bll.getPaymentModes(testConnectionString, 202);
            System.out.println("Payment Modes received: " + paymentModes);
        } catch (Exception e) {
            System.err.println("Error getting payment modes: " + e.getMessage());
            e.printStackTrace();
        }


        // Test BulkUploadExamRegData (involves Excel reading stub and BLL validation)
        System.out.println("\nTesting bulkUploadExamRegData...");
         // This test uses the dummy data returned by the ExcelParser stub.
         try {
             BulkUploadResult uploadResult = bll.bulkUploadExamRegData(testConnectionString, dummyExcelFilePath, dummyUserId);
             System.out.println("Bulk Upload Result: " + uploadResult);
             // Examine uploadResult.getUploadData() to see validation results added by BLL and potentially modified by DAL stub
         } catch (Exception e) {
             System.err.println("Error during bulk upload processing: " + e.getMessage());
             e.printStackTrace();
         }

         // Test BulkUploadExamRegData2 (assuming you have preprocessed data)
        System.out.println("\nTesting bulkUploadExamRegData2...");
         try {
             // Create some dummy List<Map> data that would typically come from preprocessing
             List<Map<String, Object>> preprocessedData = new ArrayList<>();
             Map<String, Object> row = new HashMap<>();
             row.put("IRDA URN", "PRE123");
             row.put("Payment Mode", "Online Payment");
             row.put("OnOrAfterDate", "10-Dec-2023");
             row.put("IsValidRecord", true); // Assume validation was done elsewhere
             row.put("UploadRemark", "Preprocessed");
             preprocessedData.add(row);

             Map<String, Object> row2 = new HashMap<>();
             row2.put("IRDA URN", "PRE456");
             row2.put("Payment Mode", "Credit");
             row2.put("OnOrAfterDate", "20-Dec-2023");
             row2.put("IsValidRecord", false); // Assume validation failed elsewhere
             row2.put("UploadRemark", "Previous validation failed");
             preprocessedData.add(row2);


             List<Map<String, Object>> uploadResult2 = bll.bulkUploadExamRegData2(testConnectionString, preprocessedData, dummyUserId);
             System.out.println("Bulk Upload 2 Returned Data (from DAL stub): " + uploadResult2);
         } catch (Exception e) {
              System.err.println("Error during bulk upload 2 processing: " + e.getMessage());
              e.printStackTrace();
         }


        System.out.println("\n--- Testing Complete ---");
    }
}