// File: src/main/java/com/yourcompany/controllers/ExamBatchesController.java
package com.example.controller;

import java.io.File; // Helper for ApiResponse
import java.nio.file.Files; // Constants
import java.nio.file.Paths; // App state stub
import java.text.SimpleDateFormat; // Session stub
import java.util.ArrayList; // Logging stub
import java.util.Date; // Excel writing service
import java.util.HashMap; // Your API response wrapper
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import com.example.common.CommonConstants;
import com.example.common.Errorlogger; // To get method name for logging
import com.example.common.PortalApplicationStub;
import com.example.common.PortalSessionStub;
import com.example.services.ExcelService;
import com.example.services.ResponseHelper;
import com.example.web.response.ApiResponse;

import iiibl.BulkUploadResult;
import iiibl.ExamRegistration; // For Arrays.asList
import iiibl.MasterData;
import jakarta.servlet.http.HttpServletRequest;

// @RestController annotation combines @Controller and @ResponseBody
@RestController
@RequestMapping("/batches") // Base path for all endpoints in this controller
public class ExamBatchesController {

    // Inject translated BLL dependencies (assuming they are also Spring components or instantiated directly)
    // If BLLs are Spring @Service components, inject them:
    // private final ExamRegistration examRegistrationBLL;
    // private final MasterData masterDataBLL;

    // If BLLs are NOT Spring components (like in your C# which uses 'new'), instantiate them inside methods or here
    // Let's instantiate them inside methods to match the C# pattern more closely
    // private ExamRegistration examRegistrationBLL = new ExamRegistration(); // Example if not using Spring beans
    // private MasterData masterDataBLL = new MasterData(); // Example if not using Spring beans


    // Inject helper services (these *should* be Spring components)
    private final ResponseHelper responseHelper;
    private final ExcelService excelService;

     // Inject HttpServletRequest to get form parameters manually and for logging context
     private final HttpServletRequest request;


    // Autowire dependencies in the constructor
    @Autowired
    public ExamBatchesController(ResponseHelper responseHelper, ExcelService excelService, HttpServletRequest request) {
        this.responseHelper = responseHelper;
        this.excelService = excelService;
        this.request = request; // Inject the current request
    }

    // C# [AuthorizeExt] [HttpGet] ActionResult methods (ExamRegistration, ManageBatches, ManageBatchesB)
    // These return Views (HTML) and are not REST endpoints. They are not translated here.

    // C# [HttpPost] [AuthorizeAJAX] JsonResult GetTrainedApplicants()
    // Reads form data, calls BLL, returns JSON
    @PostMapping("/getTrainedApplicants")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation
    public ResponseEntity<ApiResponse<List<Map<String, Object>>>> getTrainedApplicants(
            // Parameters bound automatically by Spring from form data
            @RequestParam("txtFromDate") String fromDateStr,
            @RequestParam("txtToDate") String toDateStr,
            @RequestParam("ddlExamBody") int examBodyId,
            @RequestParam("ddlCenter") int examCenterId
    ) {
        String methodName = "getTrainedApplicants";
        String className = this.getClass().getName();
        ApiResponse<List<Map<String, Object>>> response; // Use ApiResponse wrapper

        try {
            // Authorization check equivalent (basic stub)
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response);
            }

            // Parse dates from the format sent by the datepicker ('dd-M-yy')
            SimpleDateFormat dateFormat = new SimpleDateFormat("dd-M-yy");
            Date fromDate = null;
            Date toDate = null;
            try {
                fromDate = dateFormat.parse(fromDateStr);
                toDate = dateFormat.parse(toDateStr);
            } catch (java.text.ParseException e) {
                 Errorlogger.logError(className, methodName, e, request.getParameterMap()); // Log with parameters
                 response = responseHelper.createErrorResponse(CommonConstants.INVALID_INPUT + ": Invalid date format.");
                 return ResponseEntity.badRequest().body(response); // Return 400 Bad Request
            }

            // Instantiate BLLs as in the C# code
            ExamRegistration examRegistrationBLL = new ExamRegistration();
            MasterData masterDataBLL = new MasterData();


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
            // Add PaymentMode data as _EXTRA_
             response = responseHelper.addExtraToResponse(response, "PaymentMode", paymentModeJsonString);

            return ResponseEntity.ok(response); // Return 200 OK

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, request.getParameterMap());
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response); // Return 500 Internal Server Error
        }
    }

    // C# [AuthorizeAJAX] [HttpPost] JsonResult UpdateApplicantDetails()
    // Reads form data (checkboxes, dates, emails), builds DataTable, calls BLL (BulkUploadExamRegData2), writes Excel, returns JSON
    @PostMapping("/updateApplicantDetails")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation
     // RequestBody not used as it's form data, HttpServletRequest to read manually
    public ResponseEntity<ApiResponse<Void>> updateApplicantDetails() {
        String methodName = "updateApplicantDetails";
        String className = this.getClass().getName();
        ApiResponse<Void> response; // Use ApiResponse wrapper, Void as no _DATA_ on success

        try {
             // Authorization check equivalent (basic stub)
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response);
            }

            // --- Replicate C# logic to build DataTable equivalent (List<Map>) from Request.Form ---
            List<Map<String, Object>> dataTableList = new ArrayList<>();
            int maxRows = 50; // Based on the JS loop limit (0 to 49)
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
                 // Else keep whatever value was sent, or handle invalid value
            }

            // Adjust SchedulingMode based on ddl value
             if ("1".equals(schedulingMode)) {
                 schedulingMode = "AUTO";
             } else if ("2".equals(schedulingMode)) { // Assuming 2 is SELF
                 schedulingMode = "SELF";
             }
             // Else keep whatever value was sent, or handle invalid value


            SimpleDateFormat dateParseFormat = new SimpleDateFormat("dd M yy"); // Date format from the datepicker in JS
            dateParseFormat.setLenient(false);
            SimpleDateFormat dateOutputFormat = new SimpleDateFormat("dd-MMM-yyyy"); // Format expected by the BLL/DAL

            for (int i = 0; i < maxRows; i++) {
                String chkParamName = String.format("chk%d", i);
                String txtOnDateParamName = String.format("txtOnDate%d", i);
                String txtEmailParamName = String.format("txtEmail%d", i);
                String txtURNParamName = String.format("txtURN%d", i);

                String chkValue = request.getParameter(chkParamName); // Only present if checkbox is checked

                if (chkValue != null) { // If the checkbox parameter is present, it was checked

                    String urn = request.getParameter(txtURNParamName);
                    String emailIds = request.getParameter(txtEmailParamName);
                    String onOrAfterDateStr = request.getParameter(txtOnDateParamName);

                    // C# throws if date is empty. Let's replicate.
                    if (onOrAfterDateStr == null || onOrAfterDateStr.trim().isEmpty()) {
                         throw new Exception("Invalid txtApplicationDate for row " + i + " (empty)");
                    }

                    // Parse and format the date
                     String onOrAfterDateFormatted = null;
                     try {
                          Date parsedDate = dateParseFormat.parse(onOrAfterDateStr.trim());
                          onOrAfterDateFormatted = dateOutputFormat.format(parsedDate); // Format to "dd-MMM-yyyy"
                     } catch (java.text.ParseException e) {
                          // Handle date parsing errors specifically for this row
                          throw new Exception("Invalid txtApplicationDate format for row " + i + ": '" + onOrAfterDateStr + "'", e);
                     }

                    // Add row to the DataTable equivalent (List<Map>)
                    Map<String, Object> row = new HashMap<>();
                    // Use the keys expected by the DAL/BLL structured parameter AND the Excel writer
                    // Based on the C# BLL's DisplayColumns array for Excel output, these are the keys:
                    row.put("IRDA URN", urn);
                    row.put("Payment Mode", paymentMode);
                    row.put("Insurer Remark", insurerRemark);
                    row.put("Enrollment No", enrollmentNo);
                    row.put("OnOrAfterDate", onOrAfterDateFormatted); // Formatted date string
                    row.put("EmailIds", emailIds);
                    row.put("Batch Mode", batchMode);
                    row.put("Scheduling Mode", schedulingMode);
                    row.put("ExamBatchNo", null); // Will be populated by DAL/BLL
                    row.put("IsValidRecord", null); // Will be populated by DAL/BLL
                    row.put("UploadRemark", null); // Will be populated by DAL/BLL
                    dataTableList.add(row);
                }
            }
             // --- End building DataTable equivalent ---

            // Check if any rows were selected/processed
             if (dataTableList.isEmpty()) {
                  // C# shows an alert "No Row Selected" and returns, without calling BLL/DAL
                  // Return a response indicating no rows were processed
                  response = responseHelper.createErrorResponse("No Row Selected"); // Use an error response for this status
                  return ResponseEntity.badRequest().body(response); // 400 Bad Request
             }

            // Note: C# had client-side JS validation (validator3) and then duplicated
            // some validation checks (date empty, email format) before calling the DAL.
            // The server-side should ideally re-validate critical fields here as client-side can be bypassed.
            // The date parsing/formatting above serves as one such validation check.
            // Email validation is also done client-side but should be done server-side too.
            // Let's add the email validation here as well.

             // Simple Email Format Validation (replicates C# JS regex test)
             // C# JS regex = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
             // Java equivalent regex (needs Pattern and Matcher)
             java.util.regex.Pattern emailRegex = java.util.regex.Pattern.compile("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
             boolean canProceed = true;
             StringBuilder validationErrors = new StringBuilder(); // To collect server-side validation errors

            for (Map<String, Object> row : dataTableList) {
                String emailIds = (String) row.get("EmailIds");
                 if (emailIds != null && !emailIds.trim().isEmpty()) {
                     String[] mailIds = emailIds.split(",");
                      for (String mailId : mailIds) {
                           String trimmedMailId = mailId.trim();
                           if (!trimmedMailId.isEmpty() && !emailRegex.matcher(trimmedMailId).matches()) {
                               // Found an invalid email format
                                if (validationErrors.length() > 0) validationErrors.append(", ");
                                validationErrors.append("Invalid email format in row for URN '").append(row.get("IRDA URN")).append("': '").append(trimmedMailId).append("'");
                                canProceed = false; // Mark overall process as cannot proceed
                                // Note: C# client-side JS would mark the specific row. Server-side might aggregate errors.
                                // We'll aggregate here and return a general error message if any row fails.
                           }
                      }
                 }
                 // Date format check is implicitly done by the dateParseFormat.parse call above which throws exception.
                 // Date range check is typically done in the BLL/DAL stored procedure, as seen in the BulkUploadExamRegData logic.
            }

             if (!canProceed) {
                 // Client-side JS would show an alert. Server-side, we return an error response.
                 response = responseHelper.createErrorResponse("Server-side validation failed: " + validationErrors.toString());
                  return ResponseEntity.badRequest().body(response); // 400 Bad Request
             }


            // Call BLL to bulk upload the data
            ExamRegistration objExamRegistration = new ExamRegistration();
            // C# calls BulkUploadExamRegData2 which expects DataTable and returns DataSet
            List<Map<String, Object>> objDataSetOutputTable = objExamRegistration.bulkUploadExamRegData2(
                    PortalApplicationStub.getConnectionString(),
                    dataTableList, // Pass the List<Map>
                    PortalSessionStub.getUserID()
            );

            // --- Write Response Excel File ---
            // C# path: Server.MapPath("../downloads") + "\\" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
            String filePrefix = PortalSessionStub.getUserLoginID() + "_Exam_Response_";
            String timestamp = new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
            String outputFileName = filePrefix + timestamp + ".xlsx";
            String webDownloadPath = "/downloads/" + outputFileName; // Web path for the client link
            String serverDownloadPath = excelService.getServerFilePath(webDownloadPath); // Server file system path using ExcelService helper

            // Ensure the directory exists for the response file
             File downloadDir = new File(serverDownloadPath).getParentFile();
             if (downloadDir != null && !downloadDir.exists()) {
                 Files.createDirectories(Paths.get(downloadDir.getAbsolutePath()));
             }

            // Prepare columns for Excel writing (replicates C# string arrays)
             String[] displayColumns = new String[] { "IRDA URN", // Use the keys that were put into the Map
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo", // Expected from DAL result
                                                        "Upload Remark" }; // Expected from DAL result
            // Headers match DisplayColumns in this case
             String[] displayHeaders = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark"
                                                        };
             String[] displayFormats = new String[] { "", "", "", "", "", "", "", "", "", ""};


            // Call Excel Service to write the output data returned by the BLL/DAL
             excelService.writeExcel(serverDownloadPath, objDataSetOutputTable, displayColumns, displayHeaders, displayFormats);


            // Build success response
            response = responseHelper.createSuccessResponse(
                    CommonConstants.FILE_PROCESS_SUCCESS,
                    null // Data is null in C# success response
            );
            // Add the download file path as _EXTRA_
             response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath); // Web path for client

            return ResponseEntity.ok(response); // 200 OK

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, request.getParameterMap());
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response); // 500 Internal Server Error
        }
    }

    // C# [AuthorizeAJAX] [HttpPost] JsonResult UploadRegistration()
    // Handles file upload, calls BLL (BulkUploadExamRegData with out params), writes Excel, returns JSON
    @PostMapping("/uploadRegistration")
    // @PreAuthorize("isAuthenticated()") // Example Spring Security annotation
    public ResponseEntity<ApiResponse<Void>> uploadRegistration(
            @RequestParam("blkfile") MultipartFile file // Bind the uploaded file
            // Other form fields like ddlpaymentmode, etc. might also be sent with the file upload
            // If needed, add them as @RequestParam or read manually from HttpServletRequest
    ) {
        String methodName = "uploadRegistration";
        String className = this.getClass().getName();
        ApiResponse<Void> response;
        File uploadedTempFile = null; // Keep track of the saved file for deletion

        try {
             // Authorization check equivalent (basic stub)
            if (PortalSessionStub.getUserID() == 0) {
                 response = responseHelper.createErrorResponse("User session expired. Kindly re-login.");
                 // C# returned text/html directly here. Returning JSON is better for AJAX.
                 return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(response); // 401 Unauthorized
            }

             // Basic file validation (check if empty or has invalid name)
             if (file.isEmpty()) {
                 response = responseHelper.createErrorResponse(CommonConstants.INVALID_INPUT + ": Uploaded file is empty.");
                 return ResponseEntity.badRequest().body(response); // 400 Bad Request
             }
             String originalFileName = file.getOriginalFilename();
              if (originalFileName == null || originalFileName.trim().isEmpty()) {
                  response = responseHelper.createErrorResponse(CommonConstants.INVALID_INPUT + ": Invalid file name.");
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
             // Let's not strictly validate extension here and let the BLL/parser handle it, but log.
             if (!fileExtension.equals(".xls") && !fileExtension.equals(".xlsx")) {
                 System.out.println("Warning: Uploaded file extension is not .xls or .xlsx: " + fileExtension);
                 // Depending on requirements, you might return an error here.
                 // response = responseHelper.createErrorResponse("Invalid file type. Only .xls or .xlsx files are allowed.");
                 // return ResponseEntity.badRequest().body(response);
             }


            // --- Save the uploaded file to a temporary location ---
            // C# path: Server.MapPath("~/Uploads") + "\\" + strExtension;
            String filePrefix = PortalSessionStub.getUserLoginID() + "_Upload_"; // Use a clear prefix for uploads
            String timestamp = new SimpleDateFormat("yyyyMMddHHmmssSSS").format(new Date()); // Use a detailed timestamp
            String savedFileName = filePrefix + timestamp + fileExtension; // Use original extension
            String serverUploadPath = excelService.getServerFilePath("/Uploads/") + File.separator + savedFileName; // Server file system path


             // Ensure the directory exists
             File uploadDir = new File(serverUploadPath).getParentFile();
             if (uploadDir != null && !uploadDir.exists()) {
                 Files.createDirectories(Paths.get(uploadDir.getAbsolutePath()));
             }

            uploadedTempFile = new File(serverUploadPath);
            file.transferTo(uploadedTempFile); // Save the uploaded file


            // --- Call BLL to process the uploaded file ---
            ExamRegistration oBLExamBody = new ExamRegistration(); // Instantiate BLL
            // C# calls BulkUploadExamRegData which has out Status and Message
            BulkUploadResult bllResult = oBLExamBody.bulkUploadExamRegData(
                    PortalApplicationStub.getConnectionString(),
                    serverUploadPath, // Pass the server file path to the BLL
                    PortalSessionStub.getUserID()
                    // Status and Message are part of the returned BulkUploadResult object
            );

            // --- Delete the uploaded file ---
            if (uploadedTempFile != null && uploadedTempFile.exists()) {
                 try {
                     uploadedTempFile.delete();
                     System.out.println("Deleted uploaded temporary file: " + uploadedTempFile.getAbsolutePath());
                 } catch (Exception deleteEx) {
                     System.err.println("Failed to delete uploaded temporary file: " + uploadedTempFile.getAbsolutePath() + " - " + deleteEx.getMessage());
                     Errorlogger.logError(className, methodName, deleteEx, "Failed to delete uploaded file: " + uploadedTempFile.getAbsolutePath());
                     // Logging is sufficient, don't fail the request just because deletion failed
                 }
            }

            // --- Write Response Excel File based on BLL result data ---
            // C# path: Server.MapPath("../downloads") + "\\" + PortalSession.UserLoginID + "_Exam_Response_" + DateTime.Now.ToString("ddMMMyyyyhhmmsstt") + ".xlsx";
             // Note: The C# uses the *same* file naming convention as UpdateApplicantDetails response
            String responseFilePrefix = PortalSessionStub.getUserLoginID() + "_Exam_Response_"; // C# uses the same prefix
            String responseTimestamp = new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date()); // New timestamp for response file
            String responseOutputFileName = responseFilePrefix + responseTimestamp + ".xlsx";
            String webDownloadPath = "/downloads/" + responseOutputFileName; // Web path for client
            String serverDownloadPath = excelService.getServerFilePath(webDownloadPath); // Server file system path

             // Ensure the directory exists for the response file
             File responseDownloadDir = new File(serverDownloadPath).getParentFile();
             if (responseDownloadDir != null && !responseDownloadDir.exists()) {
                 Files.createDirectories(Paths.get(responseDownloadDir.getAbsolutePath()));
             }


            // Prepare columns for Excel writing (replicates C# string arrays)
            // Note: These are the same as in UpdateApplicantDetails, matching the expected output from BulkUploadExamRegData
             String[] displayColumns = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark" }; // These keys should match the Maps returned by BLL/DAL stub
             String[] displayHeaders = new String[] { "IRDA URN",
                                                        "Payment Mode",
                                                        "Insurer Remark",
                                                        "Enrollment No",
                                                        "OnOrAfterDate",
                                                        "EmailIds",
                                                        "Batch Mode",
                                                        "Scheduling Mode",
                                                        "ExamBatchNo",
                                                        "Upload Remark"
                                                        };
             String[] displayFormats = new String[] { "", "", "", "", "", "", "", "", "", ""};

            // Call Excel Service to write the output data returned by the BLL
             if (bllResult.getUploadData() != null && !bllResult.getUploadData().isEmpty()) {
                  excelService.writeExcel(serverDownloadPath, bllResult.getUploadData(), displayColumns, displayHeaders, displayFormats);
                  System.out.println("Excel response file written: " + serverDownloadPath);
             } else {
                  System.out.println("BLL BulkUploadExamRegData returned no data to write to Excel response file.");
                  // The C# code doesn't explicitly handle the case where objResult.Tables[0] is empty.
                  // It would likely proceed to write an empty Excel or throw an error later.
                  // We created an empty file in the excelService stub if data is null/empty.
             }


            // Build response based on BLL result status and message
            if (!bllResult.isStatus()) { // If BLL processing status is FAIL
                 // C# used FILE_PROCESS_FAIL + " : " + Message
                 response = responseHelper.createErrorResponse(CommonConstants.FILE_PROCESS_FAIL + " : " + bllResult.getMessage());
                 // C# added the response file link even on failure if it existed.
                 // Let's add the link here if the file was attempted to be written.
                 // Check if the response file path was determined (i.e., no path mapping error occurred)
                 if (serverDownloadPath != null) { // We determined a path, assuming writeExcel handled empty data
                     response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath);
                 }
                 return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(response); // Use 400 Bad Request for processing errors
            } else { // BLL status is SUCCESS
                 response = responseHelper.createSuccessResponse(CommonConstants.FILE_PROCESS_SUCCESS, null);
                 // Add the download file path as _EXTRA_
                 if (serverDownloadPath != null) {
                     response = responseHelper.addExtraToResponse(response, "_RESPONSE_FILE_", webDownloadPath);
                 }
                 return ResponseEntity.ok(response); // 200 OK
            }

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, request.getParameterMap());

            // Attempt to delete the uploaded file even if other errors occurred
             if (uploadedTempFile != null && uploadedTempFile.exists()) {
                 try {
                     uploadedTempFile.delete();
                     System.out.println("Deleted uploaded temporary file after error: " + uploadedTempFile.getAbsolutePath());
                 } catch (Exception deleteEx) {
                      System.err.println("Failed to delete uploaded temporary file after error: " + uploadedTempFile.getAbsolutePath() + " - " + deleteEx.getMessage());
                 }
             }

            // Build a generic error response for unexpected exceptions
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response); // Return 500 Internal Server Error
        }
    }


    // C# [AuthorizeAJAX] [HttpPost] JsonResult GetExamCenters(Int16 ExamBodyID)
    // Calls MasterData BLL, returns JSON
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

            // Instantiate MasterData BLL as in C#
            MasterData masterDataBLL = new MasterData();

            // Call MasterData BLL
            List<Map<String, Object>> examCentersData = masterDataBLL.getExamCenter(
                    PortalApplicationStub.getConnectionString(),
                    examBodyID,
                    (short) -1 // C# passes -1
            );

            // Build success response
            response = responseHelper.createSuccessResponse(
                    "", // Message empty in C# success
                    examCentersData // Data payload
            );

            return ResponseEntity.ok(response); // 200 OK

        } catch (Exception ex) {
            Errorlogger.logError(className, methodName, ex, request.getParameterMap());
            response = responseHelper.createErrorResponse(CommonConstants.ERROR_OCCURED);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response); // 500 Internal Server Error
        }
    }

     // --- Helper method for logging request parameters (used in Errorlogger) ---
     // This is now in Errorlogger.java

     // --- Helper method to convert List<Map> to JSON string (used for PaymentMode) ---
     // This is now in ResponseHelper.java


     // You might want a method to simulate login for testing purposes if using the session stub
     // @GetMapping("/simulate-login")
     // public String simulateLoginForTesting(@RequestParam int userId, @RequestParam String loginId, @RequestParam int insurerId, @RequestParam String role) {
     //     PortalSessionStub.simulateLogin(userId, loginId, insurerId, role);
     //     return "Session attributes set for user: " + loginId;
     // }

}