package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse;
import com.example.services.SchedulingService; // IIIBL.Scheduling
import com.example.integration.CssSchedulingService; // CSSIntegration.Scheduling
import com.example.interfaces.SchedulingDataAccess; // For nested DTOs like CandidateDetailsResult
// Import DTOs from CssSchedulingService if they are public and used directly by controller
import com.example.integration.CssSchedulingService.*; 

import com.example.util.CommonMessages;
import com.example.util.DataConverter; // For safe casting from Map
import com.example.util.ErrorLogger;
import com.example.util.PortalSession;
import com.example.util.ZipUtil;
import com.utils.XLXporter;


import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.*;
import java.util.stream.Collectors;

// C# Attributes:
// [AuthorizeExt]
// [AuthorizeAJAX]

@Controller
@RequestMapping("/Scheduler")
public class SchedulerController {

    private final SchedulingService schedulingServiceIIIBL; // For IIIBL.Scheduling
    private final CssSchedulingService schedulingServiceCSS; // For CSSIntegration.Scheduling

    @Autowired
    public SchedulerController(SchedulingService schedulingServiceIIIBL, CssSchedulingService schedulingServiceCSS) {
        this.schedulingServiceIIIBL = schedulingServiceIIIBL;
        this.schedulingServiceCSS = schedulingServiceCSS;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName();
    }

    private String getCurrentMethodName() {
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }
    
    private String getDownloadsDir() {
        String path = PortalApplication.getDownloadsPath(); 
        if (path == null || path.isEmpty()) {
            path = System.getProperty("java.io.tmpdir") + File.separator + "iiireg_downloads_fallback";
            new File(path).mkdirs(); 
        }
        return path;
    }

    @GetMapping("/BookSeat")
    public String bookSeatView(Model model, HttpServletRequest request) {
        // model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        // model.addAttribute("ClassName", "col-sm-9"); // If needed by the view
        return "BookSeat"; // View name
    }

    @PostMapping(value = "/GetCandidateDetails", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Map<String, List<Map<String, Object>>>> getCandidateDetails(HttpServletRequest request) {
        boolean success = false;
        String message = "";
        Map<String, List<Map<String, Object>>> responseData = null; // For multiple tables

        try {
            String urn = request.getParameter("txtURN");
            if (urn == null || urn.trim().isEmpty()) {
                return new ApiResponse<>(false, CommonMessages.INVALID_INPUT + ": URN is required", null);
            }

            SchedulingDataAccess.CandidateDetailsResult result = schedulingServiceIIIBL.getCandidateDetails(
                    PortalApplication.getConnectionString(), urn);
            message = result.getMessage();

            if (message == null || message.isEmpty()) {
                List<Map<String, Object>> dsTablesContainer = result.getDataSet(); // This is List<Map> for first table in mock
                                                                                    // If IIIDL actually returns 3 tables in one list or map:
                // To match C# expecting 3 tables: The DTO should hold Map<String, List<Map<String, Object>>>
                // For now, assuming the service DTO returns a structure that can be interpreted as 3 tables.
                // If `result.getDataSet()` is `Map<String, List<Map<String, Object>>>`, then it's direct.
                // If it's `List<List<Map<String, Object>>>`, then need to map by index.
                // Assuming the service DTO `CandidateDetailsResult` is structured to give access to these 3 tables.
                // Let's assume the service method now returns a `Map<String, List<Map<String,Object>>>` directly
                // if the underlying DTO from DataAccess has this structure for `getDataSet()`.
                // For `ToJSON2`, the C# passes `new DataTable[]`. So, the `data` field of ApiResponse should be `Map<String, List<Map<String, Object>>>`
                
                // This part needs refinement based on how SchedulingDataAccess.CandidateDetailsResult.getDataSet() is structured
                // If it's a single List<Map> for the first table as per simple DTO:
                // This won't work: if (dsTablesContainer != null && dsTablesContainer.size() == 3)
                // We need to assume the Service/DAO layer returns a Map for multi-table results
                // Let's modify the DTO expectation or service method return type if strictly following ToJSON2
                // For now, assume the DTO `result.getDataSet()` itself is the map of tables:
                // e.g. Map<String, List<Map<String, Object>>> tables = result.getAllTables();

                // For direct translation of `objDataSet.Tables.Count == 3`:
                // The DTO or service method needs to return something reflecting this (e.g., Map or a custom object)
                // Let's assume `getCandidateDetails` service method is changed to return `Map<String, List<Map<String, Object>>>`
                Map<String, List<Map<String, Object>>> allTables = schedulingServiceIIIBL.getCandidateDetailsMultiTable(
                        PortalApplication.getConnectionString(), urn); // A new conceptual service method for multi-table

                if (allTables != null && allTables.size() == 3 &&
                    allTables.containsKey("Table0") && allTables.containsKey("Table1") && allTables.containsKey("Table2") &&
                    !allTables.get("Table0").isEmpty() && !allTables.get("Table1").isEmpty() && !allTables.get("Table2").isEmpty()) {
                    
                    success = true;
                    responseData = allTables;
                } else {
                    success = false;
                    message = CommonMessages.ERROR_OCCURED + " (Incomplete data returned)";
                }
            } else { // Message from service indicates an error
                success = false;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, responseData); // responseData is Map<String, List<Map>>
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetCandidateDetailsRC", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Map<String, List<Map<String, Object>>>> getCandidateDetailsRC(HttpServletRequest request) {
        // Similar logic to GetCandidateDetails, but calls GetCandidateDetailsRC service method
        boolean success = false;
        String message = "";
        Map<String, List<Map<String, Object>>> responseData = null;
        try {
            String urn = request.getParameter("txtURN");
             if (urn == null || urn.trim().isEmpty()) {
                return new ApiResponse<>(false, CommonMessages.INVALID_INPUT + ": URN is required", null);
            }
            // Assuming schedulingServiceIIIBL.getCandidateDetailsRCMultiTable returns Map<String, List<Map>>
            Map<String, List<Map<String, Object>>> allTables = schedulingServiceIIIBL.getCandidateDetailsRCMultiTable(
                    PortalApplication.getConnectionString(), urn);
            
            // The message handling in C# for this was `Message = objScheduler.GetCandidateDetailsRC(...)`
            // So, if the service method returns a DTO with message, use that.
            // For simplicity, assume `allTables` is null if there's an issue communicated by service's message.
            // If `schedulingServiceIIIBL.getCandidateDetailsRCMultiTable` returns a DTO:
            // SchedulingDataAccess.CandidateDetailsResult rcResult = schedulingServiceIIIBL.getCandidateDetailsRC(...);
            // message = rcResult.getMessage(); if(message is empty) { allTables = rcResult.getDataSetAsMap(); }
            
            if (allTables != null && allTables.size() == 3 &&
                allTables.containsKey("Table0") && !allTables.get("Table0").isEmpty() &&
                allTables.containsKey("Table1") && !allTables.get("Table1").isEmpty() &&
                allTables.containsKey("Table2") && !allTables.get("Table2").isEmpty()) {
                success = true;
                responseData = allTables;
            } else {
                success = false;
                message = (allTables == null) ? "Service error or URN not found" : CommonMessages.ERROR_OCCURED + " (Incomplete RC data)";
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, responseData);
    }

    @PostMapping(value = "/GetDatesForCenter", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getDatesForCenter(HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = true; // C# initial
        String message = "";
        int centerId = 0;

        try {
            String centerIdStr = request.getParameter("center_id");
            if (centerIdStr == null || centerIdStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid center_id"); // C# used cboCategory for exception message
            centerId = Integer.parseInt(centerIdStr.trim());
        } catch (Exception ex) {
            success = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message, null);
        }

        if (success) { // Will be true if parsing didn't fail
            try {
                // CssSchedulingService.getDatesForCenter should return a DTO similar to CssDataTableResult
                CssDataTableResult result = schedulingServiceCSS.getDatesForCenter(
                    PortalApplication.getCSSConnectionString(), PortalApplication.getCSSClientId(),
                    centerId, PortalApplication.getExamDuration());
                message = result.getMessage();

                if (message == null || message.isEmpty()) { // Success from CSS layer
                    if (result.getDataTable() != null && !result.getDataTable().isEmpty()) {
                        dataTable = result.getDataTable();
                        success = true;
                    } else {
                        success = false; // C# sets false here
                        message = "No dates found the selected center";
                    }
                } else { // Error message from CSS layer
                    success = false;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                success = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    @PostMapping(value = "/GetBatchesForCenterDate", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getBatchesForCenterDate(HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = true;
        String message = "";
        int centerId = 0;
        Date preferredDate = null;

        try {
            String centerIdStr = request.getParameter("center_id");
            if (centerIdStr == null || centerIdStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid center_id");
            centerId = Integer.parseInt(centerIdStr.trim());

            String preferredDateStr = request.getParameter("preferred_date");
            if (preferredDateStr == null || preferredDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid preferred_date");
            preferredDate = new SimpleDateFormat("yyyy-MM-dd").parse(preferredDateStr.trim()); // Adjust format as per client

        } catch (Exception ex) {
            success = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message, null);
        }

        if (success) {
            try {
                CssDataTableResult result = schedulingServiceCSS.getBatchesForCenterDate(
                    PortalApplication.getCSSConnectionString(), PortalApplication.getCSSClientId(),
                    preferredDate, centerId, PortalApplication.getExamDuration());
                message = result.getMessage();

                if (message == null || message.isEmpty()) {
                    if (result.getDataTable() != null && !result.getDataTable().isEmpty()) {
                        dataTable = result.getDataTable();
                        success = true;
                    } else {
                        success = false;
                        message = "No batches found the selected center & date";
                    }
                } else {
                    success = false;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                success = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(success, message, dataTable);
    }
    
    // Helper for GetScheduledBatchesForCenterDate & GetScheduledBatchCountForCenterDate
    private ApiResponse<List<Map<String, Object>>> getScheduledBatchesInternal(int hint, HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = true;
        String message = "";
        int centerId = 0;
        Date preferredDate = null;
        String slot = "";

        try {
            String centerIdStr = request.getParameter("center_id");
            if (centerIdStr == null || centerIdStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid center_id");
            centerId = Integer.parseInt(centerIdStr.trim());

            String preferredDateStr = request.getParameter("preferred_date");
            if (preferredDateStr == null || preferredDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid preferred_date");
            preferredDate = new SimpleDateFormat("yyyy-MM-dd").parse(preferredDateStr.trim());
            
            if (hint == 2) { // For count
                slot = request.getParameter("slot");
                if (slot == null) slot = ""; // Match C# behavior of passing empty string
            }
        } catch (Exception ex) {
            success = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), "getScheduledBatchesInternal(hint=" + hint + ") (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message, null);
        }

        if (success) {
            try {
                // schedulingServiceIIIBL.getScheduledBatchesForCenterDate returns List<Map> (DataSet in C#)
                dataTable = schedulingServiceIIIBL.getScheduledBatchesForCenterDate(
                    PortalApplication.getConnectionString(), hint, preferredDate, centerId, slot);
                
                if (dataTable != null && !dataTable.isEmpty()) {
                    success = true;
                } else {
                    success = false; // C# logic
                    message = "No dates found the selected center and date"; // C# specific message
                    dataTable = new ArrayList<>(); // ensure empty list for JSON
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), "getScheduledBatchesInternal(hint=" + hint + ")", ex, request.getParameterMap());
                success = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetScheduledBatchesForCenterDate", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getScheduledBatchesForCenterDate(HttpServletRequest request) {
        return getScheduledBatchesInternal(1, request);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetScheduledBatchCountForCenterDate", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getScheduledBatchCountForCenterDate(HttpServletRequest request) {
        return getScheduledBatchesInternal(2, request);
    }
    
    private String getTimeString(int tmInMinutes) throws IllegalArgumentException {
        if (tmInMinutes < 0 || tmInMinutes > 1440) {
            throw new IllegalArgumentException("Invalid Time (minutes out of range 0-1440)");
        }
        if (tmInMinutes == 1440) return "00:00 AM"; // Midnight special case

        int hours = tmInMinutes / 60;
        int minutes = tmInMinutes % 60;
        String ampm = "AM";

        if (hours == 0) { hours = 12; } // 12 AM
        else if (hours == 12) { ampm = "PM"; } // 12 PM
        else if (hours > 12) { hours -= 12; ampm = "PM"; }
        
        return String.format("%02d:%02d %s", hours, minutes, ampm);
    }

    @PostMapping(value = "/BookSeat", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> bookSeatAction(HttpServletRequest request,
                                           @RequestParam(value="dummy", required=false) String dummy) {
        boolean success = true;
        String message = ""; // Used for final status message and for error messages from CSS
        String cssStatus = ""; // Status from CSS call

        String urn = ""; int cssCenterId = 0; Date testDate = null; String fromTime = ""; int languageId = 0;
        long clientSideIdentifier = 0; String salutation = "", candidateName = "", candidateEmail = "", candidatePhone = "";
        String cssReferenceNumber = "";

        try {
            urn = request.getParameter("urn");
            languageId = Integer.parseInt(request.getParameter("language_id"));
            cssCenterId = Integer.parseInt(request.getParameter("center_id"));
            testDate = new SimpleDateFormat("yyyy-MM-dd").parse(request.getParameter("preferred_date")); // Adjust format
            fromTime = getTimeString(Integer.parseInt(request.getParameter("start_time")));

            if (urn == null || urn.isEmpty()) throw new IllegalArgumentException("URN is required.");
            // Add other input validations as needed

        } catch (Exception ex) {
            success = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message);
        }

        if (success) { // If parsing was fine
            try {
                // 1. Get Candidate Details from IIIBL
                SchedulingDataAccess.CandidateDetailsResult cdResult = schedulingServiceIIIBL.getCandidateDetails(
                    PortalApplication.getConnectionString(), urn);
                message = cdResult.getMessage(); // Message from IIIBL service

                if (message == null || message.isEmpty()) { // IIIBL GetCandidateDetails was successful
                    // Assuming getDataSet returns a single table as List<Map> for candidate details.
                    // If it returns multiple tables, the DTO/service needs to expose them appropriately.
                    List<Map<String, Object>> candidateDataList = cdResult.getDataSet(); 
                    if (candidateDataList != null && !candidateDataList.isEmpty()) {
                        Map<String, Object> candidateRow = candidateDataList.get(0);
                        clientSideIdentifier = DataConverter.getLong(candidateRow, "client_reference_number");
                        salutation = DataConverter.getString(candidateRow, "salutation");
                        candidateName = DataConverter.getString(candidateRow, "applicant_name");
                        candidateEmail = DataConverter.getString(candidateRow, "email");
                        candidatePhone = DataConverter.getString(candidateRow, "mobile_no");

                        // 2. Call CSS BookSeat
                        CssBookingResult cssBookingResult = schedulingServiceCSS.bookSeat(
                            PortalApplication.getCSSConnectionString(), PortalApplication.getCSSClientId(),
                            String.valueOf(clientSideIdentifier), salutation, candidateName, candidateEmail, candidatePhone,
                            cssCenterId, testDate, fromTime, PortalApplication.getExamDuration());
                        
                        cssStatus = cssBookingResult.getStatus();
                        message = cssBookingResult.getMessage(); // Message from CSS
                        cssReferenceNumber = cssBookingResult.getCssReferenceNumber();

                        if ("SUCCESS".equalsIgnoreCase(cssStatus)) {
                            // 3. Update Booking Status in IIIBL
                            schedulingServiceIIIBL.updateBookingStatus(PortalApplication.getConnectionString(), urn,
                                clientSideIdentifier, cssReferenceNumber, testDate, fromTime, cssCenterId, languageId);
                            success = true;
                            message = "Seat Booked Successfully";
                        } else { // CSS Booking Failed
                            success = false;
                            // Map CSS error messages to user-friendly ones
                            switch (message != null ? message.toUpperCase() : "UNKNOWN_CSS_ERROR") {
                                case "ERROR_OCCURED": message = "Error occured while booking the seat.\nTry again after sometime.\nIf the problem persists, contact technical support team."; break;
                                case "INVALID_CLIENT": message = "Invalid client id.\nPlease contact technical support team"; break;
                                case "DUPLICATE_REFERENCE_NUMBER_FOR_CLIENT": message = "Duplicate client reference number.\nPlease contact technical support team"; break;
                                case "NO_SUCH_BATCH": message = "Invalid batch.\nTry again after some time. If the problem persists, contact technical support team"; break;
                                case "SEAT_NOT_BOOKED": message = "Seat booking failed.\nTry again after some time. If the problem persists, contact technical support team"; break;
                                default: message = cssStatus + " - " + message + "\nUnknown Error Occured.\nTry again after sometime.\nIf the problem persists, contact technical support team."; break;
                            }
                        }
                    } else {
                        success = false;
                        message = "Unable to fetch candidate data. Please contact helpdesk.";
                    }
                } else { // Error from IIIBL GetCandidateDetails
                    success = false;
                    // message already contains the error reason
                }
            } catch (Exception ex) {
                success = false;
                message = CommonMessages.ERROR_OCCURED;
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            }
        }
        return new ApiResponse<>(success, message);
    }
    
    // --- Common logic for bulk operations: Reschedule and Cancel (Single & Batch) ---
    private ApiResponse<List<Map<String, Object>>> processBulkSchedulingOperation(
            String operationType, // "RescheduleSingle", "CancelSingle", "RescheduleBatch", "CancelBatch"
            HttpServletRequest request,
            // CSS SDK method (functional interface)
            CssOperationFunction cssOperation,
            // IIIBL DB update method (functional interface)
            IiiDbUpdateFunction iiiDbUpdateOperation) {
        
        boolean initialParseSuccess = true;
        String message = "";
        List<Map<String, Object>> finalResultTable = null;

        // Parameters that vary based on operation type
        String urnForSingle = null;
        int currentCenterForBatch = 0; Date currentDateForBatch = null; String currentSlotForBatch = null;
        int newCssCenterId = 0; Date newTestDate = null; String newFromTime = "";
        String remarks = "";

        try {
            remarks = request.getParameter("remarks");
            if (remarks == null) remarks = "";

            if (operationType.endsWith("Single")) {
                urnForSingle = request.getParameter("urn");
                if (urnForSingle == null || urnForSingle.isEmpty()) throw new IllegalArgumentException("URN required.");
                if (operationType.startsWith("Reschedule")) {
                    newCssCenterId = Integer.parseInt(request.getParameter("center_id"));
                    newTestDate = new SimpleDateFormat("yyyy-MM-dd").parse(request.getParameter("preferred_date"));
                    newFromTime = getTimeString(Integer.parseInt(request.getParameter("start_time")));
                }
            } else { // Batch operation
                currentCenterForBatch = Integer.parseInt(request.getParameter("cboExamCenterOld"));
                currentDateForBatch = new SimpleDateFormat("yyyy-MM-dd").parse(request.getParameter("txtExamDateOld"));
                currentSlotForBatch = request.getParameter("cboSlotsOld");
                if (operationType.startsWith("Reschedule")) {
                    newCssCenterId = Integer.parseInt(request.getParameter("cboExamCenterNew"));
                    newTestDate = new SimpleDateFormat("yyyy-MM-dd").parse(request.getParameter("cboExamDateNew")); // Check param: cboExamDateNew vs txtExamDateNew
                    newFromTime = getTimeString(Integer.parseInt(request.getParameter("start_time")));
                }
            }
        } catch (Exception ex) {
            initialParseSuccess = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), operationType + " (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message, null);
        }

        if(initialParseSuccess) {
            try {
                List<Map<String, Object>> candidatesToProcess;
                // 1. Get Candidate Details from IIIBL
                if (urnForSingle != null) {
                    SchedulingDataAccess.CandidateDetailsResult cdResult = schedulingServiceIIIBL.getCandidateDetailsRC(PortalApplication.getConnectionString(), urnForSingle);
                    message = cdResult.getMessage();
                    if (message != null && !message.isEmpty()) throw new Exception("III_DB_FETCH_ERROR: " + message);
                    // Assuming getDataSet() returns first table of candidate(s) for this URN
                    candidatesToProcess = cdResult.getDataSet(); 
                    if (candidatesToProcess == null || candidatesToProcess.isEmpty()) throw new Exception("Unable to fetch candidate data for URN: " + urnForSingle);
                } else { // Batch
                    Date currentDateTimeForQuery = new SimpleDateFormat("dd-MMM-yyyy HH:mm").parse( // Match C# DateTime.Parse format
                        new SimpleDateFormat("dd-MMM-yyyy").format(currentDateForBatch) + " " + currentSlotForBatch);
                    candidatesToProcess = schedulingServiceIIIBL.getCandidateDetailsRCB(PortalApplication.getConnectionString(), currentCenterForBatch, currentDateTimeForQuery);
                    if (candidatesToProcess == null || candidatesToProcess.isEmpty()) throw new Exception("Unable to fetch candidate data for batch.");
                }
                
                // 2. Prepare data for CSS (Add new schedule details if rescheduling)
                // C# adds these columns even for Cancel. If CSS CancelSeat doesn't need them, this can be conditional.
                for (Map<String, Object> candidate : candidatesToProcess) {
                    if (operationType.startsWith("Reschedule")) {
                        candidate.put("center_id", newCssCenterId);
                        candidate.put("preferred_test_date", newTestDate);
                        candidate.put("exam_duration", PortalApplication.getExamDuration());
                        candidate.put("from_time", newFromTime);
                    } else { // For Cancel, C# still added these (potentially as placeholders for a generic DataTable structure to CSS)
                        candidate.putIfAbsent("center_id", 0); 
                        candidate.putIfAbsent("preferred_test_date", new Date(0)); // Some default date
                        candidate.putIfAbsent("exam_duration", 0);
                        candidate.putIfAbsent("from_time", "");
                    }
                }

                // 3. Call CSS Operation
                String userLoginId = PortalSession.getUserLoginID(request);
                String cssRemarksSource = "NSEIT-IIIExams.org-" + 
                                          (operationType.startsWith("Reschedule") ? "Rescheduler" : "ScheduleCanceller") + 
                                          " - " + userLoginId;
                
                CssBulkOperationResult cssResult = cssOperation.apply(
                    PortalApplication.getCSSConnectionString(), PortalApplication.getCSSClientId(),
                    candidatesToProcess, remarks, cssRemarksSource);

                message = cssResult.getMessage(); // Overall message from CSS
                String cssStatus = cssResult.getStatus();

                if ("FAIL".equalsIgnoreCase(cssStatus)) {
                    initialParseSuccess = false; // Final operation status
                    // message already contains CSS failure reason
                } else if ("SUCCESS".equalsIgnoreCase(cssStatus)) {
                    if (cssResult.getDataTable() == null || cssResult.getDataTable().isEmpty()) {
                        initialParseSuccess = false;
                        message = "Unable to receive " + operationType.toLowerCase().replace("single", "").replace("batch", "") + " data from CSS. Please contact helpdesk";
                    } else {
                        List<Map<String,Object>> cssResultsTable = cssResult.getDataTable();
                        // C# removes these columns before passing to IIIBL UpdateBookingStatusBulk.
                        // For cancellation, C# doesn't remove them before UpdateCancellationStatusBulk.
                        if(operationType.startsWith("Reschedule")){
                             for(Map<String,Object> row : cssResultsTable){
                                row.remove("SALUTATION"); row.remove("CANDIDATE_NAME");
                                row.remove("CANDIDATE_EMAIL"); row.remove("CANDIDATE_PHONE");
                            }
                        }
                        
                        // 4. Update III DB with results from CSS
                        List<Map<String, Object>> iiiUpdateResultTable = iiiDbUpdateOperation.apply(PortalApplication.getConnectionString(), cssResultsTable);
                        
                        if (iiiUpdateResultTable == null || iiiUpdateResultTable.isEmpty()) {
                            initialParseSuccess = false; 
                            message += " | Unable to receive final status from III DB update.";
                        } else {
                            finalResultTable = iiiUpdateResultTable;
                            initialParseSuccess = true; // Overall success
                            if(message == null || message.isEmpty() || "SUCCESS".equalsIgnoreCase(message)){
                                message = "Process completed. Please check the status below for individual URNs";
                            }
                        }
                    }
                } else { // Unknown status from CSS
                    initialParseSuccess = false;
                    if (message == null || message.isEmpty()) message = "Unknown status from CSS: " + cssStatus;
                }
            } catch (Exception ex) {
                initialParseSuccess = false;
                message = CommonMessages.ERROR_OCCURED + (ex.getMessage() != null ? ": " + ex.getMessage() : "");
                ErrorLogger.logError(getCurrentClassName(), operationType, ex, request.getParameterMap());
            }
        }
        return new ApiResponse<>(initialParseSuccess, message, finalResultTable);
    }

    // Functional interfaces for the CSS and III DB operations
    @FunctionalInterface
    private interface CssOperationFunction {
        CssBulkOperationResult apply(String p1, String p2, List<Map<String,Object>> p3, String p4, String p5) throws Exception;
    }
    @FunctionalInterface
    private interface IiiDbUpdateFunction {
        List<Map<String,Object>> apply(String p1, List<Map<String,Object>> p2) throws Exception;
    }

    // @AuthorizeExt (added for consistency)
    @GetMapping("/RC") public String rescheduleCandidateView() { return "RC"; }

    // @AuthorizeAJAX
    @PostMapping(value="/Reschedule", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<List<Map<String, Object>>> rescheduleSingleAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
        return processBulkSchedulingOperation("RescheduleSingle", request, 
            (cs, cid, data, rem, src) -> schedulingServiceCSS.rescheduleSeat(cs, cid, data, rem, src),
            (cs, data) -> schedulingServiceIIIBL.updateBookingStatusBulk(cs, data)
        );
    }

    // @AuthorizeExt
    @GetMapping("/RCB") public String rescheduleBatchView() { return "RCB"; }

    // @AuthorizeAJAX
    @PostMapping(value="/RescheduleBatch", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<List<Map<String, Object>>> rescheduleBatchAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
         return processBulkSchedulingOperation("RescheduleBatch", request,
            (cs, cid, data, rem, src) -> schedulingServiceCSS.rescheduleSeat(cs, cid, data, rem, src),
            (cs, data) -> schedulingServiceIIIBL.updateBookingStatusBulk(cs, data)
        );
    }
    
    // @AuthorizeExt
    @GetMapping("/CC") public String cancelCandidateView() { return "CC"; }

    // @AuthorizeAJAX
    @PostMapping(value="/CC", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<List<Map<String, Object>>> cancelCandidateAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
        return processBulkSchedulingOperation("CancelSingle", request,
            (cs, cid, data, rem, src) -> schedulingServiceCSS.cancelSeat(cs, cid, data, rem, src),
            (cs, data) -> schedulingServiceIIIBL.updateCancellationStatusBulk(cs, data)
        );
    }
    
    // @AuthorizeExt
    @GetMapping("/CB") public String cancelBatchView() { return "CB"; }

    // @AuthorizeAJAX
    @PostMapping(value="/CB", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<List<Map<String, Object>>> cancelBatchAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
         return processBulkSchedulingOperation("CancelBatch", request,
            (cs, cid, data, rem, src) -> schedulingServiceCSS.cancelSeat(cs, cid, data, rem, src),
            (cs, data) -> schedulingServiceIIIBL.updateCancellationStatusBulk(cs, data)
        );
    }
    
    // @AuthorizeExt
    @GetMapping("/ReconcileBooking")
    public String reconcileBookingView(Model model, HttpServletRequest request){
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ReconcileBooking";
    }

    // @AuthorizeAJAX
    @PostMapping(value="/ReconcileBooking", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> reconcileBookingAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
        boolean success = true;
        String message = "";
        Map<String, String> additionalParams = null;
        Date reconDate = null;

        try {
            String reconDateStr = request.getParameter("txtReconDate");
            if(reconDateStr == null || reconDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid recon date");
            reconDate = new SimpleDateFormat("yyyy-MM-dd").parse(reconDateStr.trim()); // Adjust format
        } catch (Exception ex){
            success = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message, null, null); // Added null for dataSet arg
        }

        if(success){
            try {
                CssDataTableResult cssResult = schedulingServiceCSS.getReconcileBoookingDetails(
                    PortalApplication.getCSSConnectionString(), reconDate, PortalApplication.getCSSClientId());

                if (cssResult == null || cssResult.getDataTable() == null) {
                    message = "Unable to fetch reconciliation data (error code:1/3 from CSS). Please contact helpdesk";
                    success = false;
                } else if (cssResult.getDataTable().isEmpty()) {
                    message = "No data found for reconciliation for given date (Information code:4 from CSS). If you are expecting data to be present for this date then contact helpdesk";
                    success = true; // C# logic
                } else {
                    List<Map<String,Object>> iiiReconResultTable = schedulingServiceIIIBL.reconcileBoookings(
                        PortalApplication.getConnectionString(), cssResult.getDataTable());
                    
                    if(iiiReconResultTable == null){
                        message = "No data found for reconciliation for given date (error code:5 from III). If you are expecting data to be present for this date then contact helpdesk";
                        success = false;
                    } else if (iiiReconResultTable.isEmpty()){
                         message = "No data found for reconciliation for given date (error code:7 from III). If you are expecting data to be present for this date then contact helpdesk";
                         success = false; // C# sets success=false if final table is empty after III recon
                    } else {
                        success = true;
                        message = "Process completed successfully. Please check the response file for details";

                        String downloadsDir = getDownloadsDir();
                        String dateSuffix = new SimpleDateFormat("dd-MMM-yyyy").format(reconDate);
                        String timeSuffix = new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
                        String filename = String.format("%s_ReconciliationReport_%s_%s.xlsx", PortalSession.getUserLoginID(request), dateSuffix, timeSuffix);
                        String fullOutputFilePath = Paths.get(downloadsDir, filename).toString();
                        String clientDownloadPath = Paths.get("Downloads", filename).toString().replace("\\", "/");

                        String[] displayColumns = { "CANDIDATE_ID", "CLIENT_ID", "CLIENT_SIDE_IDENTIFIER", "CENTER_ID", "TEST_DATE","START_TIME_N", "IS_CANCELLED", "CANCELLATION_REMARKS", "CANCELLATION_SOURCE", "BOOKING_TIMESTAMP", "CANCELLATION_TIMESTAMP", "OLD_CANDIDATE_ID", "STATUS","REMARKS", "center_id_local", "varExamCenterName" };
                        String[] displayHeaders = { "CSS Candidate Id", "CSS Client Id", "Reg Portal Candidate Id", "Center Id", "Test Date","Test Time", "Is Cancelled?", "Cancellation Remarks", "Cancellation Source", "Booking Timestamp", "Cancellation Timestamp", "Old CSS Candidate Id", "STATUS","REMARKS", "Center id local", "Exam Center Name" };
                        String[] displayFormat = new String[displayColumns.length]; Arrays.fill(displayFormat, "");

                        XLXporter.writeExcel(fullOutputFilePath, iiiReconResultTable, displayColumns, displayHeaders, displayFormat);
                        
                        additionalParams = new HashMap<>();
                        additionalParams.put("_RESPONSE_FILE_", clientDownloadPath);
                    }
                }
            } catch (Exception ex){
                 success = false;
                 message = CommonMessages.ERROR_OCCURED;
                 ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            }
        }
        // Original C# HelperUtilities.ToJSON(Success, Message, null, KVPair);
        // Using ApiResponse(success, message, data=null, additionalParams=KVPair)
        return new ApiResponse<>(success, message, null, additionalParams);
    }
}