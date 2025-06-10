// File: src/main/java/com/example/controller/ExamBatchesController.java
package com.example.controller;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import com.example.common.CommonConstants;
import com.example.common.Errorlogger;
import com.example.common.PortalApplicationStub;
import com.example.common.PortalSessionStub;
import com.example.services.ExcelService;
import com.example.services.ResponseHelper;
import com.example.web.response.ApiResponse;

import iiibl.BulkUploadResult;
import iiibl.ExamRegistration; // Assuming iiibl.ExamRegistration is needed here (Arrays.asList note seems misplaced)
import iiibl.MasterData; // Assuming iiibl.MasterData is needed here
import jakarta.servlet.http.HttpServletRequest;

// @RestController annotation combines @Controller and @ResponseBody
@RestController
@RequestMapping("/batches") // Base path for all endpoints in this controller
public class ExamBatchesController {

    // Inject helper services (these are marked with @Service, so Spring manages them)
    private final ResponseHelper responseHelper;
    private final ExcelService excelService;
     private final HttpServletRequest request; // Injected by Spring for request context

    // Autowire dependencies in the constructor
    @Autowired
    public ExamBatchesController(ResponseHelper responseHelper, ExcelService excelService, HttpServletRequest request) {
        this.responseHelper = responseHelper;
        this.excelService = excelService;
        this.request = request;
    }

    // Note: C# [AuthorizeExt] [HttpGet] ActionResult methods are for serving HTML views.
    // These are not translated as this is a REST controller.

    // C# [HttpPost] [AuthorizeAJAX] JsonResult GetTrainedApplicants()
    @PostMapping("/getTrainedApplicants")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation for method security
    public ResponseEntity<ApiResponse<List<Map<String, Object>>>> getTrainedApplicants(
            @RequestParam("txtFromDate") String fromDateStr,
            @RequestParam("txtToDate") String toDateStr,
            @RequestParam("ddlExamBody") int examBodyId,
            @RequestParam("ddlCenter") int examCenterId
    ) {
        String methodName = "getTrainedApplicants";
        String className = this.getClass().getName();
        ApiResponse<List<Map<String, Object>>> response;

        try {
            // Authorization check equivalent (basic stub)
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response);
            }

            // Parse dates from the format sent by the datepicker ('dd-M-yy')
            SimpleDateFormat dateFormat = new SimpleDateFormat("dd-M-yy");
            dateFormat.setLenient(false); // Be strict about the format
            Date fromDate = null;
            Date toDate = null;
            try {
                fromDate = dateFormat.parse(fromDateStr);
                toDate = dateFormat.parse(toDateStr);
            } catch (java.text.ParseException e) {
                 Errorlogger.logError(className, methodName, e, "Request Parameters: " + request.getParameterMap()); // Log with parameters
                 response = responseHelper.createErrorResponse(CommonConstants.INVALID_INPUT + ": Invalid date format for FromDate or ToDate.");
                 return ResponseEntity.badRequest().body(response);
            }

            // Instantiate BLLs manually as in the C# code (Consider injecting these as @Service beans instead)
            ExamRegistration examRegistrationBLL = new ExamRegistration();
            MasterData masterDataBLL = new MasterData(); // Used for getExamCenters, but instantiated here

            // Call BLL to get Payment Modes
            List<Map<String, Object>> paymentModesData = examRegistrationBLL.getPaymentModes(
                    PortalApplicationStub.getConnectionString(),
                    PortalSessionStub.getInsurerUserID()
            );
            // Convert PaymentModes to JSON string for _EXTRA_ field, as the client expects
            String paymentModeJsonString = responseHelper.convertListMapToJsonString(paymentModesData);

            // Call BLL to get Trained Applicants data
            List<Map<String, Object>> trainedApplicantsData = examRegistrationBLL.getTrainedApplicants(
                    PortalApplicationStub.getConnectionString(),
                    PortalSessionStub.getUserID(),
                    examBodyId,
                    examCenterId,
                    fromDate,
                    toDate
            );

            // Build success response
            response = responseHelper.createSuccessResponse(
                    "", // C# message is empty on success
                    trainedApplicantsData // Data payload
            );
            // Add PaymentMode data as _EXTRA_. Using addExtraToResponse requires the ApiResponse object already exists.
             response = responseHelper.addExtraToResponse(response, "PaymentMode", paymentModeJsonString); // response is reassigned, though addExtraToResponse modifies in place

            return ResponseEntity.ok(response);

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, "Request Parameters: " + request.getParameterMap());
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
        }
    }

    // C# [AuthorizeAJAX] [HttpPost] JsonResult UpdateApplicantDetails()
    @PostMapping("/updateApplicantDetails")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation
    public ResponseEntity<ApiResponse<Void>> updateApplicantDetails() {
        String methodName = "updateApplicantDetails";
        String className = this.getClass().getName();
        ApiResponse<Void> response;

        try {
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response);
            }

            // --- Replicate C# logic to build DataTable equivalent (List<Map>) from Request.Form ---
            List<Map<String, Object>> dataTableList = new ArrayList<>();
            int maxRows = 50; // Based on the client-side JS loop limit (0 to 49)
            String paymentMode = request.getParameter("ddlpaymentmode");
            String batchMode = request.getParameter("ddlBatchmode");
            String schedulingMode = request.getParameter("ddlSchedulingMode");
            String insurerRemark = request.getParameter("txtRemarks");
            String enrollmentNo = ""; // Keep it blank as in C#

            // Adjust BatchMode based on PaymentMode and ddl value
            if ("Credit".equalsIgnoreCase(paymentMode)) {
                batchMode = "BULK"; // Overridden if payment mode is Credit
            } else {
                 // C# logic: if BatchMode == "1" then "BULK", if "2" then "SINGLE"
                 if ("1".equals(batchMode)) batchMode = "BULK";
                 else if ("2".equals(batchMode)) batchMode = "SINGLE";
                 // Else keep whatever value was sent, or handle invalid value - validation might be needed
            }

            // Adjust SchedulingMode based on ddl value
             if ("1".equals(schedulingMode)) {
                 schedulingMode = "AUTO";
             } else if ("2".equals(schedulingMode)) { // Assuming 2 is SELF
                 schedulingMode = "SELF";
             }
             // Else keep whatever value was sent - validation might be needed


            // Date format expected from the datepicker in JS is 'dd M yy' (e.g., "15 12 23")
            SimpleDateFormat dateParseFormat = new SimpleDateFormat("dd M yy");
            dateParseFormat.setLenient(false); // Make parsing strict
            // Format expected by the BLL/DAL is 'dd-MMM-yyyy' (e.g., "15-Dec-2023")
            SimpleDateFormat dateOutputFormat = new SimpleDateFormat("dd-MMM-yyyy");

            for (int i = 0; i < maxRows; i++) {
                String chkParamName = String.format("chk%d", i);
                String txtOnDateParamName = String.format("txtOnDate%d", i);
                String txtEmailParamName = String.format("txtEmail%d", i);
                String txtURNParamName = String.format("txtURN%d", i);

                String chkValue = request.getParameter(chkParamName);

                if (chkValue != null) { // If the checkbox parameter is present, it means the row was selected client-side

                    String urn = request.getParameter(txtURNParamName);
                    String emailIds = request.getParameter(txtEmailParamName);
                    String onOrAfterDateStr = request.getParameter(txtOnDateParamName);

                    // C# client-side JS checks for empty date, and the C# server-side BLL throws if date is empty.
                    // Replicate the server-side check and throw an exception if date is missing.
                    if (onOrAfterDateStr == null || onOrAfterDateStr.trim().isEmpty()) {
                         Errorlogger.logError(className, methodName, new IllegalArgumentException("Missing required date"), "txtOnDate" + i + " is empty.");
                         throw new Exception("Missing date for row " + i); // Throw a generic exception for flow control
                    }

                    // Parse the date from the client-side format ('dd M yy')
                     String onOrAfterDateFormatted = null;
                     try {
                          Date parsedDate = dateParseFormat.parse(onOrAfterDateStr.trim());
                          // Format the date to the format expected by the BLL/DAL ('dd-MMM-yyyy')
                          onOrAfterDateFormatted = dateOutputFormat.format(parsedDate);
                     } catch (java.text.ParseException e) {
                          // Handle date parsing errors specifically for this row's date string
                          Errorlogger.logError(className, methodName, e, "Invalid date format for txtOnDate" + i + ": '" + onOrAfterDateStr + "'");
                          throw new Exception("Invalid date format for row " + i + ": '" + onOrAfterDateStr + "'", e); // Re-throw with detail and original exception
                     }

                    // Add row to the DataTable equivalent (List<Map>)
                    Map<String, Object> row = new HashMap<>();
                    // Use the keys expected by the DAL/BLL structured parameter and the Excel writer
                    // These keys should match the column names expected by STP_LIC_BulkUploadExamRegistration_New
                    // and the headers/columns used for writing the response Excel.
                    row.put("IRDA URN", urn);
                    row.put("Payment Mode", paymentMode);
                    row.put("Insurer Remark", insurerRemark);
                    row.put("Enrollment No", enrollmentNo); // This seems to be hardcoded as empty string in C#
                    row.put("OnOrAfterDate", onOrAfterDateFormatted); // Formatted date string
                    row.put("EmailIds", emailIds);
                    row.put("Batch Mode", batchMode);
                    row.put("Scheduling Mode", schedulingMode);
                    // These columns are added before sending to the DAL but are populated BY the DAL/SP
                    row.put("ExamBatchNo", null);
                    row.put("IsValidRecord", null); // The BLL BulkUploadExamRegData *validates* and sets this. BulkUploadExamRegData2 *assumes* validation is done.
                    row.put("UploadRemark", null); // The BLL BulkUploadExamRegData *validates* and sets this. BulkUploadExamRegData2 *assumes* validation is done.
                    dataTableList.add(row);
                }
            } // End loop through potential rows

             // Check if any rows were actually selected/processed
             if (dataTableList.isEmpty()) {
                  // Replicate C# behavior: show an alert "No Row Selected" and return without calling BLL/DAL
                  response = responseHelper.createErrorResponse("No Row Selected");
                  // Use 400 Bad Request as this is a client-side issue (no selection)
                  return ResponseEntity.badRequest().body(response);
             }

            // Note: C# had client-side JS validation (validator3) and then duplicated
            // some validation checks (date empty, email format) before calling the DAL.
            // The server-side should ideally re-validate critical fields here as client-side can be bypassed.
            // The date parsing/formatting above serves as one such server-side validation.
            // Email validation is also done client-side but should be done server-side too.
            // Let's add the email format validation here using the Java regex Pattern.
            // The C# BLL also does character validation and date range validation before calling BulkUploadExamRegData.
            // This method calls BulkUploadExamRegData2, implying validation might be done *before* this method is called,
            // or the SP handles validation and returns results in the DataSet.
            // Based on the C# BulkUploadExamRegData code, some validation (character, date range, email format) happens *before* the DAL call.
            // The UpdateApplicantDetails calls BulkUploadExamRegData2, which *doesn't* have this BLL-side validation logic.
            // This suggests BulkUploadExamRegData2 expects already validated data, OR the SP is relied upon for validation.
            // Given the C# BLL BulkUploadExamRegData *does* server-side validation, it's safer to add critical server-side checks here too,
            // although the original C# UpdateApplicantDetails code *didn't* explicitly show them before calling BulkUploadExamRegData2.

             // Add critical server-side validation before calling BLL/DAL for UpdateApplicantDetails:
             // (Character validation is complex; let's add Email format validation using the same regex)
             java.util.regex.Pattern emailRegex = java.util.regex.Pattern.compile("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
             StringBuilder validationErrors = new StringBuilder();

            for (Map<String, Object> row : dataTableList) {
                String emailIds = (String) row.get("EmailIds");
                 if (emailIds != null && !emailIds.trim().isEmpty()) {
                     String[] mailIds = emailIds.split(",");
                      for (String mailId : mailIds) {
                           String trimmedMailId = mailId.trim();
                           if (!trimmedMailId.isEmpty() && !emailRegex.matcher(trimmedMailId).matches()) {
                                if (validationErrors.length() > 0) validationErrors.append("; "); // Use semicolon to separate errors per row/email
                                validationErrors.append("Row URN '").append(row.get("IRDA URN")).append("': Invalid email format '").append(trimmedMailId).append("'");
                                // Do not break here; continue checking other emails in the list or other rows
                           }
                      }
                 }
                 // We could add checks for URN format, Payment Mode values ("Online Payment", "Credit") here if needed
            }

             if (validationErrors.length() > 0) {
                 // Client-side JS would show an alert. Server-side, return an error response.
                 response = responseHelper.createErrorResponse("Server-side validation failed: " + validationErrors.toString());
                  return ResponseEntity.badRequest().body(response); // 400 Bad Request
             }


            // Call BLL to bulk upload the data. This calls DAL's BulkUploadToDatabase with the List<Map>.
            ExamRegistration objExamRegistration = new ExamRegistration(); // Instantiate BLL
            // C# calls BulkUploadExamRegData2 which expects DataTable and returns DataSet
            List<Map<String, Object>> objDataSetOutputTable = objExamRegistration.bulkUploadExamRegData2(
                    PortalApplicationStub.getConnectionString(),
                    dataTableList, // Pass the List<Map> data prepared from form
                    PortalSessionStub.getUserID()
            );

            // --- Write Response Excel File ---
            // Replicate C# response file naming convention: Server.MapPath("../downloads") + "\\" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
            String filePrefix = PortalSessionStub.getUserLoginID() + "_Exam_Response_";
            String timestamp = new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
            String outputFileName = filePrefix + timestamp + ".xlsx";
            String webDownloadPath = "/downloads/" + outputFileName; // This is the URL path the client will use
            String serverDownloadPath = excelService.getServerFilePath(webDownloadPath); // This is the actual path on the server file system

            // Ensure the directory exists for the response file
             File downloadDir = new File(serverDownloadPath).getParentFile();
             if (downloadDir != null && !downloadDir.exists()) {
                 try {
                      Files.createDirectories(Paths.get(downloadDir.getAbsolutePath()));
                 } catch (IOException e) {
                     Errorlogger.logError(className, methodName, e, "Failed to create download directory: " + downloadDir.getAbsolutePath());
                     // Decide how to handle this error - perhaps return 500?
                     response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED + ": Could not create download directory.");
                     return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
                 }
             }


            // Prepare columns for Excel writing (replicates C# string arrays)
            // These should match the keys you put into the List<Map> and the columns returned by the SP if applicable.
             String[] displayColumns = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate", // Note: This field might need formatting to dd-MMM-yyyy in Excel if written as a Date object
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo", // Expected to be populated by the SP
                                                        "Upload Remark" }; // Expected to be populated by the SP (from BLL validation if called BulkUploadExamRegData)
            // Headers for the Excel file
             String[] displayHeaders = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "On Or After Date", // Use spaces for readability in header
                                                        "Email Ids",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "Exam Batch No",
                                                        "Upload Remark"
                                                        };
             // Formats for the Excel columns (e.g., "dd-MMM-yyyy" for dates). Stubbed currently.
             String[] displayFormats = new String[] { "@", "@", "@", "@", "dd-MMM-yyyy", "@", "@", "@", "@", "@"}; // Use '@' for text, specify date format


            // Call Excel Service to write the output data returned by the BLL/DAL
            // objDataSetOutputTable should contain the data returned from the DAL call (STP_LIC_BulkUploadExamRegistration_New)
             if (objDataSetOutputTable != null && !objDataSetOutputTable.isEmpty()) {
                  excelService.writeExcel(serverDownloadPath, objDataSetOutputTable, displayColumns, displayHeaders, displayFormats);
                  System.out.println("Excel response file written: " + serverDownloadPath);
             } else {
                  System.out.println("BLL BulkUploadExamRegData2 returned no data from DAL to write to Excel response file.");
                  // The C# code might proceed to write an empty Excel or crash.
                  // The writeExcel stub should handle null/empty data gracefully (e.g., write header only).
                  // Let's still call writeExcel to create the file with headers if the data list is empty but not null.
                  // If objDataSetOutputTable is null, writeExcel might throw, or we skip writing the file.
                  // Let's check if objDataSetOutputTable is null before attempting to write.
                  if (objDataSetOutputTable != null) {
                       excelService.writeExcel(serverDownloadPath, new ArrayList<>(), displayColumns, displayHeaders, displayFormats); // Write empty data with headers
                       System.out.println("Created empty Excel response file with headers: " + serverDownloadPath);
                  } else {
                       System.err.println("objDataSetOutputTable was null. Cannot write response file.");
                       // Decide error handling: return error response?
                       // For now, let's proceed but log.
                  }
             }


            // Build success response. C# returns success if the BLL call completes without exception.
            response = responseHelper.createSuccessResponse(
                    CommonConstants.FILE_PROCESS_SUCCESS,
                    null // Data is null in C# success response
            );
            // Add the download file path as _EXTRA_. Client JS expects "_RESPONSE_FILE_".
             // Only add the download link if the serverDownloadPath was successfully determined
             if (serverDownloadPath != null) {
                 response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath); // Web path for client
             }


            return ResponseEntity.ok(response); // 200 OK

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, "Request Parameters: " + request.getParameterMap());
            // Build a generic error response for unexpected exceptions
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            // You might still want to include the download link in the error response if the file was written partially or with errors
            // if (serverDownloadPath != null) {
            //      response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath);
            // }
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
        }
    }

    // C# [AuthorizeAJAX] [HttpPost] JsonResult UploadRegistration()
    @PostMapping("/uploadRegistration")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation
    public ResponseEntity<ApiResponse<Void>> uploadRegistration(
            @RequestParam("blkfile") MultipartFile file // Bind the uploaded file by form field name
            // Other form fields like ddlpaymentmode, etc. might also be sent with the file upload
            // If needed, add them as @RequestParam or read manually from HttpServletRequest
    ) {
        String methodName = "uploadRegistration";
        String className = this.getClass().getName();
        ApiResponse<Void> response;
        File uploadedTempFile = null; // Keep track of the saved temporary file for deletion
        String serverUploadPath = null; // Keep track of the path where the file was saved

        try {
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 // C# returned text/html directly here. Returning JSON is better for AJAX.
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response); // 401 Unauthorized
            }

             // Basic file validation (check if empty or has invalid name)
             if (file == null || file.isEmpty()) { // Check for null in case parameter binding fails unexpectedly
                 response = responseHelper.createErrorResponse(CommonConstants.INVALID_INPUT + ": Uploaded file is missing or empty.");
                 return ResponseEntity.badRequest().body(response); // 400 Bad Request
             }
             String originalFileName = file.getOriginalFilename();
              if (originalFileName == null || originalFileName.trim().isEmpty()) {
                  response = responseHelper.createErrorResponse(CommonConstants.INVALID_INPUT + ": Invalid file name provided.");
                  return ResponseEntity.badRequest().body(response); // 400 Bad Request
              }
              // Basic extension check (C# only checked for "xls")
              String fileExtension = "";
              if (originalFileName.contains(".")) {
                  fileExtension = originalFileName.substring(originalFileName.lastIndexOf(".")).toLowerCase();
              }
             // Note: C# JS validator checks for .xls. BLL BulkUploadExamRegData expects Excel file.
             // Apache POI can read .xls and .xlsx. Checking for both might be safer.
             // C# BLL GetExcelData uses OLEDB with "Excel 12.0" which implies .xlsx is also supported.
             // Let's add a check and potentially return a bad request if it's not an Excel file.
             if (!fileExtension.equals(".xls") && !fileExtension.equals(".xlsx")) {
                 String invalidExtMsg = CommonConstants.INVALID_INPUT + ": Invalid file type. Only .xls or .xlsx files are allowed. Detected: " + fileExtension;
                 System.err.println(invalidExtMsg + " for file: " + originalFileName);
                 response = responseHelper.createErrorResponse(invalidExtMsg);
                 return ResponseEntity.badRequest().body(response); // 400 Bad Request
             }


            // --- Save the uploaded file to a temporary location ---
            // Replicate C# path: Server.MapPath("~/Uploads") + "\\" + strExtension;
            String filePrefix = PortalSessionStub.getUserLoginID() + "_Upload_"; // Use user login ID for clarity/isolation
            // Use a detailed timestamp to make filenames unique
            String timestamp = new SimpleDateFormat("yyyyMMddHHmmssSSS").format(new Date());
            // Use the original file extension
            String savedFileName = filePrefix + timestamp + fileExtension;
            // Get the server file system path using ExcelService helper (maps web path to server path)
            serverUploadPath = excelService.getServerFilePath("/Uploads/") + File.separator + savedFileName;

             // Ensure the directory exists before saving the file
             File uploadDir = new File(serverUploadPath).getParentFile();
             if (uploadDir != null && !uploadDir.exists()) {
                 try {
                     Files.createDirectories(Paths.get(uploadDir.getAbsolutePath()));
                 } catch (IOException e) {
                     Errorlogger.logError(className, methodName, e, "Failed to create upload directory: " + uploadDir.getAbsolutePath());
                     // If directory creation fails, we cannot save the file. Return internal server error.
                     response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED + ": Could not save the uploaded file.");
                     return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
                 }
             }

            // Save the uploaded file to the determined temporary location
            uploadedTempFile = new File(serverUploadPath);
            file.transferTo(uploadedTempFile); // Spring's MultipartFile saves the file

            System.out.println("Uploaded file saved to temporary location: " + serverUploadPath);


            // --- Call BLL to process the uploaded file ---
            // This BLL method handles parsing the Excel, validation, and calling the DAL.
            ExamRegistration oBLExamBody = new ExamRegistration(); // Instantiate BLL
            // C# calls BulkUploadExamRegData which has out Status and Message
            BulkUploadResult bllResult = oBLExamBody.bulkUploadExamRegData(
                    PortalApplicationStub.getConnectionString(),
                    serverUploadPath, // Pass the server file path to the BLL
                    PortalSessionStub.getUserID()
                    // Status and Message are returned within the BulkUploadResult object
            );

            System.out.println("BLL BulkUploadExamRegData completed. Status: " + bllResult.isStatus() + ", Message: " + bllResult.getMessage());


            // --- Delete the uploaded temporary file ---
            // Delete the file whether the BLL call succeeded or failed, unless an exception prevented saving it initially.
            if (uploadedTempFile != null && uploadedTempFile.exists()) {
                 try {
                     uploadedTempFile.delete();
                     System.out.println("Deleted uploaded temporary file: " + uploadedTempFile.getAbsolutePath());
                 } catch (Exception deleteEx) {
                     System.err.println("Failed to delete uploaded temporary file: " + uploadedTempFile.getAbsolutePath() + " - " + deleteEx.getMessage());
                     Errorlogger.logError(className, methodName, deleteEx, "Failed to delete uploaded file after processing: " + uploadedTempFile.getAbsolutePath());
                     // Logging is sufficient, don't fail the HTTP request just because cleanup failed
                 }
            }


            // --- Write Response Excel File based on BLL result data ---
            // Replicate C# response file naming convention: Server.MapPath("../downloads") + "\\" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
             // Note: The C# uses the *same* file naming convention as UpdateApplicantDetails response
            String responseFilePrefix = PortalSessionStub.getUserLoginID() + "_Exam_Response_"; // C# uses the same prefix
            String responseTimestamp = new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date()); // New timestamp for the response file
            String responseOutputFileName = responseFilePrefix + responseTimestamp + ".xlsx";
            String webDownloadPath = "/downloads/" + responseOutputFileName; // This is the URL path the client will use for download
            String serverDownloadPath = excelService.getServerFilePath(webDownloadPath); // This is the actual path on the server file system


             // Ensure the directory exists for the response file
             File responseDownloadDir = new File(serverDownloadPath).getParentFile();
             if (responseDownloadDir != null && !responseDownloadDir.exists()) {
                 try {
                      Files.createDirectories(Paths.get(responseDownloadDir.getAbsolutePath()));
                 } catch (IOException e) {
                      Errorlogger.logError(className, methodName, e, "Failed to create response download directory: " + responseDownloadDir.getAbsolutePath());
                     // Decide how to handle this error - perhaps return 500?
                      System.err.println("Cannot write response Excel file due to directory creation failure.");
                      // Continue processing the BLL result status/message, but the download link won't work.
                 }
             }


            // Prepare columns for Excel writing (replicates C# string arrays)
            // These should match the keys put into the List<Map> by the BLL after parsing and validation,
            // and the columns returned by the SP if applicable.
             String[] displayColumns = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate", // This field might need formatting to dd-MMM-yyyy in Excel if written as a Date object
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo", // Expected to be populated by the SP
                                                        "IsValidRecord", // Populated by BLL validation
                                                        "Upload Remark" }; // Populated by BLL validation
            // Headers for the Excel file
             String[] displayHeaders = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "On Or After Date", // Use spaces for readability in header
                                                        "Email Ids",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "Exam Batch No",
                                                        "Is Valid Record", // Use spaces for readability in header
                                                        "Upload Remark"
                                                        };
             // Formats for the Excel columns (e.g., "dd-MMM-yyyy" for dates). Stubbed currently.
             String[] displayFormats = new String[] { "@", "@", "@", "@", "dd-MMM-yyyy", "@", "@", "@", "@", "@", "@"}; // Use '@' for text, specify date format


            // Call Excel Service to write the output data returned by the BLL
            // bllResult.getUploadData() contains the data returned from the DAL call (STP_LIC_BulkUploadExamRegistration_New),
            // which itself should include the columns added by the BLL's validation logic (IsValidRecord, UploadRemark).
             List<Map<String, Object>> dataToWriteToExcel = bllResult.getUploadData();
             if (dataToWriteToExcel != null && !dataToWriteToExcel.isEmpty()) {
                  try {
                       excelService.writeExcel(serverDownloadPath, dataToWriteToExcel, displayColumns, displayHeaders, displayFormats);
                       System.out.println("Excel response file written: " + serverDownloadPath);
                  } catch (IOException e) {
                       Errorlogger.logError(className, methodName, e, "Failed to write Excel response file: " + serverDownloadPath);
                       System.err.println("Failed to write response Excel file.");
                       // Decide how to handle this error - maybe return an error response?
                       // For now, we'll proceed with the BLL status but log the write failure.
                  }
             } else {
                  System.out.println("BLL returned no data from DAL to write to Excel response file.");
                  // The writeExcel stub should handle null/empty data gracefully (e.g., write header only).
                   if (serverDownloadPath != null) { // Only attempt to write if directory path was determined
                       try {
                           excelService.writeExcel(serverDownloadPath, new ArrayList<>(), displayColumns, displayHeaders, displayFormats); // Write empty data with headers
                           System.out.println("Created empty Excel response file with headers: " + serverDownloadPath);
                       } catch (IOException e) {
                           Errorlogger.logError(className, methodName, e, "Failed to write empty Excel response file: " + serverDownloadPath);
                            System.err.println("Failed to write empty response Excel file.");
                       }
                   }
             }


            // Build response based on BLL result status and message
            if (!bllResult.isStatus()) { // If BLL processing status is FAIL (due to validation or DAL error)
                 // C# used FILE_PROCESS_FAIL + " : " + Message
                 response = responseHelper.createErrorResponse(CommonConstants.FILE_PROCESS_FAIL + " : " + bllResult.getMessage());
                 // C# added the response file link even on failure if it existed.
                 // Let's add the link here if the serverDownloadPath was successfully determined, regardless of write success (ExcelService logs write errors).
                 if (serverDownloadPath != null) {
                     response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath); // Add web path for client
                 }
                 // Use 400 Bad Request if the BLL explicitly returned status=false (typically validation issues)
                 return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(response);
            } else { // BLL status is SUCCESS
                 response = responseHelper.createSuccessResponse(CommonConstants.FILE_PROCESS_SUCCESS, null);
                 // Add the download file path as _EXTRA_
                 if (serverDownloadPath != null) {
                     response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath); // Add web path for client
                 }
                 return ResponseEntity.ok(response); // 200 OK
            }

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, "Request Parameters: " + request.getParameterMap());

            // Attempt to delete the uploaded file even if other errors occurred
             if (uploadedTempFile != null && uploadedTempFile.exists()) {
                 try {
                     uploadedTempFile.delete();
                     System.out.println("Deleted uploaded temporary file after error: " + uploadedTempFile.getAbsolutePath());
                 } catch (Exception deleteEx) {
                      System.err.println("Failed to delete uploaded temporary file after error: " + uploadedTempFile.getAbsolutePath() + " - " + deleteEx.getMessage());
                      // Log the deletion failure, but don't re-throw or change the main error response.
                 }
             }

            // Build a generic error response for unexpected exceptions during processing
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED + ": " + ex.getMessage()); // Include exception message for debugging
            // Decide if you want to include the potential response file link here on unexpected errors
            // if (serverDownloadPath != null) {
            //      response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath);
            // }
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response); // Return 500 Internal Server Error for unexpected errors
        }
    }


    // C# [AuthorizeAJAX] [HttpPost] JsonResult GetExamCenters(Int16 ExamBodyID)
    @PostMapping("/getExamCenters")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation
    public ResponseEntity<ApiResponse<List<Map<String, Object>>>> getExamCenters(
             @RequestParam("ExamBodyID") short examBodyID // Bind parameter name from JS
    ) {
        String methodName = "getExamCenters";
        String className = this.getClass().getName();
        ApiResponse<List<Map<String, Object>>> response;

        try {
             // Authorization check equivalent (basic stub)
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response);
            }

            // Instantiate MasterData BLL manually (Consider injecting as @Service)
            MasterData masterDataBLL = new MasterData();

            // Call MasterData BLL to get exam centers
            List<Map<String, Object>> examCentersData = masterDataBLL.getExamCenter(
                    PortalApplicationStub.getConnectionString(),
                    examBodyID,
                    (short) -1 // C# passes -1 to get all centers for the body
            );

            // Build success response
            response = responseHelper.createSuccessResponse(
                    "", // Message empty in C# success
                    examCentersData // Data payload (List of Maps)
            );

            return ResponseEntity.ok(response); // 200 OK

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, "Request Parameters: " + request.getParameterMap());
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response); // 500 Internal Server Error
        }
    }

     // --- Helper method for logging request parameters (used in Errorlogger) ---
     // This is now in Errorlogger.java

     // --- Helper method to convert List<Map> to JSON string (used for PaymentMode) ---
     // This is now in ResponseHelper.java


     // You might want a method to simulate login for testing purposes if using the session stub
     // Uncomment and enable this method if you are using curl/Postman for testing with the session stub.
     @GetMapping("/simulate-login")
     public ResponseEntity<String> simulateLoginForTesting(
         @RequestParam int userId,
         @RequestParam String loginId,
         @RequestParam int insurerId,
         @RequestParam String role) {
         PortalSessionStub.simulateLogin(userId, loginId, insurerId, role);
         return ResponseEntity.ok("Session attributes set for user: " + loginId);
     }

}