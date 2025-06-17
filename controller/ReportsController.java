package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse;
import com.example.services.*; // Assuming all IIIBL services are here
import com.example.interfaces.MasterDataDataAccess; // For nested DTOs like ExamCenterResult
import com.example.interfaces.ExamReportsDataAccess; // For nested DTOs like ExamReportResult
import com.example.util.CommonMessages;
import com.example.util.DataConverter;
import com.example.util.ErrorLogger;
import com.example.util.PortalSession;
import com.example.util.ZipUtil; // Your zipping utility
import com.iiibl.AadhaarEncryptorDecryptor; // Assuming this path for IIIBL utility
import com.utils.XLXporter; // Your Excel/CSV utility

import com.fasterxml.jackson.databind.ObjectMapper; // For JSON serialization of params

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.InputStreamResource;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import javax.servlet.ServletContext;
import javax.servlet.http.HttpServletRequest;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
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
@RequestMapping("/Reports") // Ensure this matches if merging with existing ReportsController
public class ReportsController {

    private final ExamReportsService examReportsService;
    private final MasterDataService masterDataService;
    private final SchedulingService schedulingService; // If GetScheduleReport is here
    private final BatchMgmtService batchMgmtService; // If GetCenterWisePendingScheduleCount is here
    private final ReportsInfraService reportsInfraService; // From previous ReportsController part
    private final ServletContext servletContext; // From previous
    private final ObjectMapper jacksonObjectMapper; // For serializing params

    @Autowired
    public ReportsController(ExamReportsService examReportsService, MasterDataService masterDataService,
                             SchedulingService schedulingService, BatchMgmtService batchMgmtService,
                             ReportsInfraService reportsInfraService, ServletContext servletContext,
                             ObjectMapper jacksonObjectMapper) {
        this.examReportsService = examReportsService;
        this.masterDataService = masterDataService;
        this.schedulingService = schedulingService;
        this.batchMgmtService = batchMgmtService;
        this.reportsInfraService = reportsInfraService;
        this.servletContext = servletContext;
        this.jacksonObjectMapper = jacksonObjectMapper;
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

    // --- Methods from ReportsController (Infra part, if merging) ---
    // @AuthorizeExt
    @GetMapping("/ReportsDashboard")
    public String reportsDashboardView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ReportsDashboard";
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/LoadReportRequests", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> loadReportRequests(HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = reportsInfraService.getReportRequests(
                    PortalApplication.getConnectionString(),
                    PortalSession.getInsurerUserID(request));
            if (dataTable != null) {
                success = true;
                if (dataTable.isEmpty()) message = CommonMessages.NO_DATA_FOUND;
            } else {
                success = true; message = CommonMessages.NO_DATA_FOUND; dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false; message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeExt
    @GetMapping("/DownloadReport")
    public ResponseEntity<InputStreamResource> downloadReport(@RequestParam("FileName") String fileName) {
        String reportsDumpDir = PortalApplication.getReportsDumpPath(); // Use specific dump path
        if (reportsDumpDir == null || reportsDumpDir.isEmpty()) reportsDumpDir = getDownloadsDir();

        try {
            Path filePath = Paths.get(reportsDumpDir, fileName);
            File file = filePath.toFile();
            if (file.exists() && file.isFile()) {
                InputStreamResource resource = new InputStreamResource(new FileInputStream(file));
                String contentType = Files.probeContentType(filePath);
                if (contentType == null) contentType = servletContext.getMimeType(file.getAbsolutePath());
                if (contentType == null) contentType = "application/octet-stream";
                String attachmentFilename = "PaymentReceipts.zip"; // As per C#
                return ResponseEntity.ok()
                        .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + attachmentFilename + "\"")
                        .contentType(MediaType.parseMediaType(contentType)).contentLength(file.length()).body(resource);
            } else {
                return ResponseEntity.notFound().build();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, null);
            return ResponseEntity.status(500).build();
        }
    }

    // --- Methods from the new ReportsController.cs part ---

    // @AuthorizeExt
    @GetMapping("/CorporateAgentExaminationReport")
    public String corporateAgentExaminationReportView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-6");
        return "CorporateAgentExaminationReport";
    }

    // @AuthorizeExt
    @PostMapping(value="/CorporateAgentExaminationReport", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> corporateAgentExaminationReportAction(HttpServletRequest request, 
                                                                   @RequestParam(value="dummy", required=false) String dummy) {
        boolean success = false;
        String message = "";
        Map<String, String> additionalParams = null;

        try {
            String examMonthFull = request.getParameter("dropdownMonth");
            int examYear = Integer.parseInt(request.getParameter("dropdownYear"));
            String examMonth = (examMonthFull != null && examMonthFull.length() >= 3) ? examMonthFull.substring(0, 3) : "";

            // Assuming ExamReportsService.getCorporateExaminationReport returns List<Map>
            List<Map<String, Object>> dataTable = examReportsService.getCorporateExaminationReport(
                PortalApplication.getConnectionString(), examMonth, examYear, PortalSession.getRoleName(request));

            if (dataTable != null && !dataTable.isEmpty()) {
                success = true; 
                String guidFilenameBase = UUID.randomUUID().toString().replace("-", "");
                String downloadsDir = getDownloadsDir();
                
                String csvFileName = guidFilenameBase + ".csv";
                String zipFileName = guidFilenameBase + ".zip";
                String csvFullFilePath = Paths.get(downloadsDir, csvFileName).toString();
                String zipFullFilePath = Paths.get(downloadsDir, zipFileName).toString();
                String clientZipPath = Paths.get("Downloads", zipFileName).toString().replace("\\", "/");

                XLXporter.dataTableToCsvFile(dataTable, csvFullFilePath, ",");
                ZipUtil.zipSingleFile(csvFullFilePath, zipFullFilePath);
                try { Files.deleteIfExists(Paths.get(csvFullFilePath)); } catch (IOException e) { /* log cleanup error */ }

                additionalParams = new HashMap<>();
                additionalParams.put("_RESPONSE_FILE_", clientZipPath);
                message = CommonMessages.REPORT_PROCESSING_SUCCESS;
                // C# returned HelperUtilities.ToJSON(false, Message, null, KVPair);
                // This means success was false in JSON but message was success.
                // For consistency, if file processing is successful, success should be true.
                // If this 'false' has specific client-side handling, then set success = false;
                // success = false; // Uncomment if client specifically expects success:false for this report
            } else {
                success = true; 
                message = CommonMessages.NO_DATA_FOUND;
            }
        } catch (NumberFormatException nfe) {
            success = false; message = CommonMessages.INVALID_INPUT + ": Invalid year.";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false; message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(success, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }
    
    // @AuthorizeExt
    @GetMapping("/ApprovedCorporateAgent")
    public String approvedCorporateAgentView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-6");
        return "ApprovedCorporateAgent";
    }

    // @AuthorizeExt
    @PostMapping(value="/ApprovedCorporateAgent", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> approvedCorporateAgentAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy) {
        boolean success = false;
        String message = "";
        Map<String, String> additionalParams = null;
        try {
            List<Map<String, Object>> dataTable = examReportsService.getApprovedCorporateAgent(PortalApplication.getConnectionString());

            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
                String guidFilenameBase = UUID.randomUUID().toString().replace("-", "");
                String downloadsDir = getDownloadsDir();
                String excelFileName = guidFilenameBase + ".xlsx";
                String excelFullFilePath = Paths.get(downloadsDir, excelFileName).toString();
                String clientExcelPath = Paths.get("Downloads", excelFileName).toString().replace("\\", "/");

                String[] displayColumns = { "company_name", "address", "std_code", "landline_no", "mobile_po", "email_po", "name_po", "corporate_user_id", "intermediary_type" };
                String[] displayHeaders = { "Company Name", "Address", "STD Code", "Landline Number", "Mobile Number of Principal Officer / Designated Person", "Email ID of Principal Officer / Designated Person", "Name of Principal Officer / Designated Person", "Corporate User ID", "Intermediary Type" };
                String[] displayFormat = new String[displayColumns.length]; Arrays.fill(displayFormat, "");

                XLXporter.writeExcel(excelFullFilePath, dataTable, displayColumns, displayHeaders, displayFormat);

                additionalParams = new HashMap<>();
                additionalParams.put("_RESPONSE_FILE_", clientExcelPath);
                message = CommonMessages.REPORT_PROCESSING_SUCCESS_MIN;
            } else {
                success = true; 
                message = CommonMessages.NO_DATA_FOUND;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false; message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(success, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }

    // @AuthorizeExt
    @GetMapping("/SponsorshipStatus")
    public String sponsorshipStatusView(Model model, HttpServletRequest request) {
        String viewName = "ErrorView"; // Default
        try {
            // Assuming MasterDataService methods return List<Map>
            List<Map<String, Object>> examCenters = masterDataService.getExamCenter(PortalApplication.getConnectionString(), 5, -1);
            model.addAttribute("ExamCenters", examCenters != null ? examCenters : new ArrayList<>());

            String roleCode = PortalSession.getRoleCode(request);
            String roleName = PortalSession.getRoleName(request);
            int insurerUserId = PortalSession.getInsurerUserID(request);
            int dpUserId = PortalSession.getDPUserID(request);
            int agentCounselorUserId = PortalSession.getAgentCounselorUserID(request);

            if (Arrays.asList("CA", "WA", "IMF", "BR").contains(roleCode)) {
                viewName = "SponsorshipStatusForCorporate";
            } else if ("I".equals(roleCode) || "superadmin".equals(roleCode)) { // Added superadmin as per C#
                viewName = "SponsorshipStatus";
                List<Map<String,Object>> cdpList, dpList, acList;
                if ("Corporate Designated Person".equals(roleName)) {
                    cdpList = masterDataService.getInsurers(PortalApplication.getConnectionString(), insurerUserId);
                    dpList = masterDataService.getDPForInsurer(PortalApplication.getConnectionString(), insurerUserId, -1);
                    model.addAttribute("CDP", cdpList != null ? cdpList : new ArrayList<>());
                    model.addAttribute("DP", dpList != null ? dpList : new ArrayList<>());
                } else if ("Designated Person".equals(roleName)) {
                    cdpList = masterDataService.getInsurers(PortalApplication.getConnectionString(), insurerUserId);
                    dpList = masterDataService.getDPForInsurer(PortalApplication.getConnectionString(), insurerUserId, dpUserId);
                    acList = masterDataService.getACForDPs(PortalApplication.getConnectionString(), insurerUserId, dpUserId, -1);
                    model.addAttribute("CDP", cdpList != null ? cdpList : new ArrayList<>());
                    model.addAttribute("DP", dpList != null ? dpList : new ArrayList<>());
                    model.addAttribute("AC", acList != null ? acList : new ArrayList<>());
                } else if ("Agent Counselor".equals(roleName)) {
                    cdpList = masterDataService.getInsurers(PortalApplication.getConnectionString(), insurerUserId);
                    dpList = masterDataService.getDPForInsurer(PortalApplication.getConnectionString(), insurerUserId, dpUserId);
                    acList = masterDataService.getACForDPs(PortalApplication.getConnectionString(), insurerUserId, dpUserId, agentCounselorUserId);
                    model.addAttribute("CDP", cdpList != null ? cdpList : new ArrayList<>());
                    model.addAttribute("DP", dpList != null ? dpList : new ArrayList<>());
                    model.addAttribute("AC", acList != null ? acList : new ArrayList<>());
                } else { // Admin or other "I" role
                    cdpList = masterDataService.getInsurers(PortalApplication.getConnectionString(), -1);
                    model.addAttribute("CDP", cdpList != null ? cdpList : new ArrayList<>());
                }
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            model.addAttribute("ExamCenters", new ArrayList<>()); // Ensure attributes exist even on error
            model.addAttribute("CDP", new ArrayList<>());
            model.addAttribute("DP", new ArrayList<>());
            model.addAttribute("AC", new ArrayList<>());
        }
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return viewName.isEmpty() ? "DefaultReportView" : viewName;
    }

    // @AuthorizeAJAX
    @PostMapping(value="/SponsorshipStatusP", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> sponsorshipStatusPAction(HttpServletRequest request) {
        boolean success = false;
        String message = "";
        Map<String, String> additionalParams = null;
        try {
            int insurerUserIdParam = Integer.parseInt(request.getParameter("cboInsurer"));
            int dpUserIdParam = Integer.parseInt(request.getParameter("cboDP"));
            int acUserIdParam = Integer.parseInt(request.getParameter("cboAgentCounsellor"));
            String insurerRefNo = request.getParameter("txtInsurersRefNo");
            String examBatch = request.getParameter("txtExaminationBatchID");
            Date applicationDateFrom = new SimpleDateFormat("yyyy-MM-dd").parse(request.getParameter("txtAppDateFrom"));
            Date applicationDateTo = new SimpleDateFormat("yyyy-MM-dd").parse(request.getParameter("txtAppDateTo"));
            String examDateFromStr = request.getParameter("txtExamDateFrom");
            String examDateToStr = request.getParameter("txtExamDateTo");
            int examBodyId = Integer.parseInt(request.getParameter("cboExaminationBody"));
            int examCenterId = Integer.parseInt(request.getParameter("cboExaminationCenter"));
            String applicantStatusCsv = request.getParameter("cboApplicationStatus");
            String[] statusCodes = (applicantStatusCsv != null) ? applicantStatusCsv.split(",") : new String[0];
            
            boolean statusAll = false, statusSponsored = false, statusTrained = false;
            boolean statusEC = false, statusEA = false, statusE = false;
            for (String code : statusCodes) {
                switch (code.trim().toUpperCase()) {
                    case "A": statusAll = true; break;
                    case "S": statusSponsored = true; break;
                    case "T": statusTrained = true; break;
                    case "EC": statusEC = true; break;
                    case "EA": statusEA = true; break;
                    case "E": statusE = true; break;
                }
            }
            boolean photo = "on".equalsIgnoreCase(request.getParameter("chkPhoto")) || "true".equalsIgnoreCase(request.getParameter("chkPhoto"));
            boolean sign = "on".equalsIgnoreCase(request.getParameter("chkSign")) || "true".equalsIgnoreCase(request.getParameter("chkSign"));
            String urn = request.getParameter("txtURN");

            List<Map<String,Object>> dataTable;
            if ("I".equals(PortalSession.getRoleCode(request))) {
                dataTable = examReportsService.getSponsorshipReport(PortalApplication.getConnectionStringReports(), 
                    applicationDateFrom, applicationDateTo, PortalSession.getRoleName(request), 0, 
                    insurerUserIdParam, dpUserIdParam, acUserIdParam, urn, insurerRefNo, examBatch, examBodyId, examCenterId,
                    statusAll, statusSponsored, statusTrained, statusEC, statusEA, statusE, photo, sign, examDateFromStr, examDateToStr);
            } else {
                dataTable = examReportsService.getSponsorshipReportForCorporates(PortalApplication.getConnectionString(), 
                    PortalSession.getUserID(request), applicationDateFrom, applicationDateTo, PortalSession.getRoleName(request),
                    urn, insurerRefNo, examBatch, examBodyId, examCenterId,
                    statusAll, statusSponsored, statusTrained, statusEC, statusEA, statusE, photo, sign, examDateFromStr, examDateToStr);
            }

            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
                String filenameBase = PortalSession.getUserLoginID(request) + "_SponsorshipStatusReport_" + new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
                String downloadsDir = getDownloadsDir();
                Path outputDirPath = Paths.get(downloadsDir, filenameBase); // Subdirectory for this report's contents
                Files.createDirectories(outputDirPath);

                String zipFileName = filenameBase + ".zip";
                String zipFullFilePath = Paths.get(downloadsDir, zipFileName).toString();
                String clientZipPath = Paths.get("Downloads", zipFileName).toString().replace("\\", "/");
                String csvFullFilePath = outputDirPath.resolve(filenameBase + ".csv").toString();

                List<Map<String, Object>> processedDataTable = new ArrayList<>();
                for (Map<String, Object> row : dataTable) {
                    Map<String, Object> newRow = new HashMap<>(row);
                    String currentUrn = DataConverter.getString(newRow, "IRDA URN"); // Assuming key from C#

                    if (photo && newRow.get("imgApplicantPhoto") instanceof byte[]) {
                        Path photoDir = outputDirPath.resolve("Photo"); Files.createDirectories(photoDir);
                        Files.write(photoDir.resolve("Photo_" + currentUrn + ".jpg"), (byte[]) newRow.get("imgApplicantPhoto"));
                    }
                    newRow.remove("imgApplicantPhoto");
                    if (!photo) newRow.remove("Photo File Name");

                    if (sign && newRow.get("imgApplicantSign") instanceof byte[]) {
                        Path signDir = outputDirPath.resolve("Signature"); Files.createDirectories(signDir);
                        Files.write(signDir.resolve("Sign_" + currentUrn + ".jpg"), (byte[]) newRow.get("imgApplicantSign"));
                    }
                    newRow.remove("imgApplicantSign");
                    if (!sign) newRow.remove("Sign File Name");
                    
                    String encryptedAadhaar = DataConverter.getString(newRow,"Aadhaar Number");
                    if (encryptedAadhaar != null && !encryptedAadhaar.isEmpty()) {
                        try {
                            byte[] keyBytes = PortalApplication.getAadhaarKey().getBytes(StandardCharsets.UTF_8);
                            byte[] ivBytes = PortalApplication.getAadhaarIV().getBytes(StandardCharsets.UTF_8);
                            newRow.put("Aadhaar Number", AadhaarEncryptorDecryptor.DecryptAadhaar(encryptedAadhaar, keyBytes, ivBytes));
                        } catch (Exception e) { newRow.put("Aadhaar Number", "DECRYPTION_ERROR"); }
                    }
                    processedDataTable.add(newRow);
                }
                
                XLXporter.dataTableToCsvFile(processedDataTable, csvFullFilePath, ",");
                ZipUtil.zipDirectory(outputDirPath.toString(), zipFullFilePath);

                // Cleanup temporary directory
                Files.walk(outputDirPath).sorted(Comparator.reverseOrder()).map(Path::toFile).forEach(File::delete);

                additionalParams = new HashMap<>();
                additionalParams.put("_RESPONSE_FILE_", clientZipPath);
                message = CommonMessages.FILE_PROCESS_SUCCESS;
            } else {
                success = true; message = CommonMessages.NO_DATA_FOUND;
            }
        } catch (ParseException pe) {
            success = false; message = CommonMessages.INVALID_INPUT + ": Invalid date format.";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (DateParsing)", pe, request.getParameterMap());
        } catch (NumberFormatException nfe) {
            success = false; message = CommonMessages.INVALID_INPUT + ": Invalid number format.";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (NumberParsing)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false; message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(success, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }
    
    // @AuthorizeExt
    @GetMapping("/ExaminationReport")
    public String examinationReportView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ExaminationReport";
    }

    // @AuthorizeAJAX
    @PostMapping(value="/ExaminationReport", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> examinationReportAction(HttpServletRequest request, @RequestParam(value="Dummy", required=false) String dummy) {
        boolean success = false; // C# Status variable for this method logic
        String message = "";
        Map<String, String> additionalParams = null;
        int option = 0;

        try {
            String optionStr = request.getParameter("Option");
            if (optionStr == null || optionStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid Option");
            option = Integer.parseInt(optionStr.trim());
            if (option < 1 || option > 3) throw new IllegalArgumentException("Option value out of range (1-3).");
        } catch (Exception ex) {
            success = false; // Corresponds to C# Status = false
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
            return new ApiResponse<>(false, message);
        }

        // If parsing successful (C# 'if (Status)')
        try {
            List<Map<String, Object>> dataTable = examReportsService.getExaminationReport(
                PortalApplication.getConnectionString(), option, PortalSession.getUserID(request));

            if (dataTable != null && !dataTable.isEmpty()) {
                List<Map<String, Object>> dataTableForCsv = dataTable.stream()
                                                               .map(HashMap::new) // Create copies for manipulation
                                                               .collect(Collectors.toList());
                
                String[] fields = null, formats = null;
                String filenameBase = ""; String roleCode = PortalSession.getRoleCode(request);
                String timestamp = new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());

                if (option == 1) {
                    dataTableForCsv.forEach(row -> { // Rename columns for CSV
                        row.put("Application Date", row.remove("Application Date csv"));
                        row.put("Exam Date", row.remove("Exam Date csv"));
                    });
                    filenameBase = PortalSession.getUserLoginID(request) + "_Last2DaysExaminationReport_" + timestamp;
                    fields = new String[] { "Application Date", "URN", "Applicant Name", "Exam Date", "Exam Roll Number", "Exam Marks", "Result" };
                    formats = new String[] { "dd-MMM-yyyy", "", "", "dd-MMM-yyyy hh:mm:ss a", "", "", "" };
                } else if (option == 2) {
                    filenameBase = PortalSession.getUserLoginID(request) + "_CandidatesRegisterdForExam_" + timestamp;
                    if (Arrays.asList("CA", "WA", "IMF", "BR").contains(roleCode)) {
                        fields = new String[] { "Application Date", "URN", "Applicant Name", "Training Start Date", "Training End Date", "TCC Expiry Date", "Total Training Hrs", "Center Name", "Preferred Date", "Payment mode", "Scheduling mode", "Exam Date" };
                        formats = new String[] { "dd-MMM-yyyy", "", "", "dd-MMM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy", "", "", "dd-MMM-yyyy", "", "", "dd-MMM-yyyy hh:mm:ss a" };
                    } else {
                        fields = new String[] { "Application Date", "URN", "Applicant Name", "Center Name", "Preferred Date", "Payment mode", "Scheduling mode", "Exam Date" };
                        formats = new String[] { "dd-MMM-yyyy", "", "", "", "dd-MMM-yyyy", "", "", "dd-MMM-yyyy hh:mm:ss a" };
                    }
                } else { // Option 3
                    filenameBase = PortalSession.getUserLoginID(request) + "_CandidatesNotScheduledForExam_" + timestamp; // Adjusted name
                    if (Arrays.asList("CA", "WA", "IMF", "BR").contains(roleCode)) {
                        fields = new String[] { "Application Date", "URN", "Applicant Name", "Training Start Date", "Training End Date", "TCC Expiry Date", "Total Training Hrs" };
                        formats = new String[] { "dd-MMM-yyyy", "", "", "dd-MMM-yyyy", "dd-MMM-yyyy", "dd-MMM-yyyy", "" };
                    } else {
                        fields = new String[] { "Application Date", "URN", "Applicant Name", "Center Name" };
                        formats = new String[] { "dd-MMM-yyyy", "", "", "" };
                    }
                }

                String downloadsDir = getDownloadsDir();
                String excelFileName = filenameBase + ".xlsx";
                String csvFileName = filenameBase + ".csv";
                String excelFullFilePath = Paths.get(downloadsDir, excelFileName).toString();
                String csvFullFilePath = Paths.get(downloadsDir, csvFileName).toString();
                String clientExcelPath = Paths.get("Downloads", excelFileName).toString().replace("\\", "/");
                String clientCsvPath = Paths.get("Downloads", csvFileName).toString().replace("\\", "/");

                XLXporter.writeExcel(excelFullFilePath, dataTable, fields, fields, formats); // Use original dataTable for Excel
                XLXporter.dataTableToCsvFile(dataTableForCsv, csvFullFilePath, "\t"); // Tab delimited as per C#

                additionalParams = new HashMap<>();
                additionalParams.put("_RESPONSE_FILE_", clientExcelPath);
                additionalParams.put("_RESPONSE_FILE2_", clientCsvPath);
                success = true; // C# logic
                message = CommonMessages.FILE_PROCESS_SUCCESS;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND_FOR_SELECTED_CRITERIA;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(success, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }

    // @AuthorizeExt
    @GetMapping("/ScheduleReport")
    public String scheduleReportView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ScheduleReport";
    }

    // @AuthorizeAJAX
    @PostMapping(value="/ScheduleReport", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> scheduleReportAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy) {
        boolean initialStatus = true; // C# _Status
        String message = "";
        Map<String, String> additionalParams = null;
        int hint = -1; Date fromDate = null; Date toDate = null;

        try {
            String hintStr = request.getParameter("cboReportType");
            if (hintStr == null || hintStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid cboReportType");
            hint = Integer.parseInt(hintStr.trim());

            String fromDateStr = request.getParameter("txtFromDate");
            if (fromDateStr == null || fromDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid txtFromDate");
            fromDate = new SimpleDateFormat("yyyy-MM-dd").parse(fromDateStr.trim()); // Adjust format

            String toDateStr = request.getParameter("txtToDate");
            if (toDateStr == null || toDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid txtToDate");
            toDate = new SimpleDateFormat("yyyy-MM-dd").parse(toDateStr.trim());
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                // Assuming SchedulingService.getScheduleReport returns List<Map>
                List<Map<String, Object>> dataTable = schedulingService.getScheduleReport(
                    PortalApplication.getConnectionString(), hint, fromDate, toDate);

                if (dataTable != null && !dataTable.isEmpty()) {
                    initialStatus = true; // Final success
                    String filenameBase = PortalSession.getUserLoginID(request) + "_ScheduleReport_" + new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
                    String downloadsDir = getDownloadsDir();
                    String excelFileName = filenameBase + ".xlsx";
                    String excelFullFilePath = Paths.get(downloadsDir, excelFileName).toString();
                    String clientExcelPath = Paths.get("Downloads", excelFileName).toString().replace("\\", "/");
                    
                    String[] displayColumns = null, displayHeaders = null, displayFormat = null;
                    switch (hint) {
                        case 1: 
                            displayColumns = new String[] { "exam_date", "candidate_count" };
                            displayHeaders = new String[] { "Exam Date", "Candidate Count" };
                            break;
                        case 2:
                            displayColumns = new String[] { "exam_date","center_id", "center_name", "candidate_count" };
                            displayHeaders = new String[] { "Exam Date", "Center Id", "Center Name", "Candidate Count" };
                            break;
                        case 3: 
                            displayColumns = new String[] { "exam_date", "center_id", "center_name","exam_batch", "candidate_count" };
                            displayHeaders = new String[] { "Exam Date", "Center Id", "Center Name","Exam Batch", "Candidate Count" };
                            break;
                        default: throw new IllegalArgumentException("Invalid Hint for report columns: " + hint);
                    }
                    displayFormat = new String[displayColumns.length]; Arrays.fill(displayFormat, "");

                    XLXporter.writeExcel(excelFullFilePath, dataTable, displayColumns, displayHeaders, displayFormat);
                    
                    additionalParams = new HashMap<>();
                    additionalParams.put("_RESPONSE_FILE_", clientExcelPath);
                    message = CommonMessages.FILE_PROCESS_SUCCESS;
                } else {
                    initialStatus = true; // C# logic
                    message = CommonMessages.NO_DATA_FOUND;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        ApiResponse<Void> response = new ApiResponse<>(initialStatus, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }
    
    // @AuthorizeExt
    @GetMapping("/CenterWisePendingScheduleCountReport")
    public String centerWisePendingScheduleCountReportView(Model model, HttpServletRequest request){
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "CenterWisePendingScheduleCountReport";
    }

    // @AuthorizeAJAX
    @PostMapping(value="/CenterWisePendingScheduleCountReport", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> centerWisePendingScheduleCountReportAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
        boolean success = false; // C# _Status
        String message = "";
        Map<String, String> additionalParams = null;
        try {
            // Assuming BatchMgmtService.getCenterWisePendingScheduleCount returns List<Map>
            List<Map<String, Object>> dataTable = batchMgmtService.getCenterWisePendingScheduleCount(PortalApplication.getConnectionString());

            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
                String filenameBase = PortalSession.getUserLoginID(request) + "_CenterWisePendingScheduleCountReport_" + new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
                String downloadsDir = getDownloadsDir();
                String excelFileName = filenameBase + ".xlsx";
                String excelFullFilePath = Paths.get(downloadsDir, excelFileName).toString();
                String clientExcelPath = Paths.get("Downloads", excelFileName).toString().replace("\\", "/");
                
                // C# XLXporter.WriteExcel(OutputFileName2, objDataTable); (no specific columns/headers)
                XLXporter.writeExcel(excelFullFilePath, dataTable, null, null, null);

                additionalParams = new HashMap<>();
                additionalParams.put("_RESPONSE_FILE_", clientExcelPath);
                message = CommonMessages.FILE_PROCESS_SUCCESS;
            } else {
                success = true; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(success, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }

    // @AuthorizeExt
    @GetMapping("/PaymentReport")
    public String paymentReportView(Model model, HttpServletRequest request){
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "PaymentReport";
    }

    // @AuthorizeExt
    @PostMapping(value="/PaymentReport", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> paymentReportAction(HttpServletRequest request, @RequestParam(value="dummy", required=false) String dummy){
        boolean initialStatus = true; // C# _Status
        String message = "";
        Map<String, String> additionalParams = null;
        Date fromDate = null; Date toDate = null;

        try {
            String fromDateStr = request.getParameter("txtFromDate");
            if (fromDateStr == null || fromDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid txtFromDate");
            fromDate = new SimpleDateFormat("yyyy-MM-dd").parse(fromDateStr.trim()); // Adjust format

            String toDateStr = request.getParameter("txtToDate");
            if (toDateStr == null || toDateStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid txtToDate");
            toDate = new SimpleDateFormat("yyyy-MM-dd").parse(toDateStr.trim());
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                // Assuming ExamReportsService.generatePaymentReport returns List<Map>
                List<Map<String, Object>> dataTable = examReportsService.generatePaymentReport(
                    PortalApplication.getOaimsConnectionString(), // Note: OAIMS Connection String
                    PortalSession.getTopUserLoginID(request), fromDate, toDate);

                if (dataTable != null && !dataTable.isEmpty()) {
                    initialStatus = true; // Final success
                    String filenameBase = PortalSession.getUserLoginID(request) + "_PaymentReport_" + new SimpleDateFormat("ddMMMyyyyhhmmsstt").format(new Date());
                    String downloadsDir = getDownloadsDir();
                    String excelFileName = filenameBase + ".xlsx";
                    String excelFullFilePath = Paths.get(downloadsDir, excelFileName).toString();
                    String clientExcelPath = Paths.get("Downloads", excelFileName).toString().replace("\\", "/");

                    String[] displayColumns = { "ROWNUM", "PG_TRANS_Id", "TRANSACTION_DATE", "TRANSACTION_TIME", "EED_TXNS_ID", "EED_EXM_IRDA_URN", "APPLICATION_DATE", "AMOUNT" };
                    String[] displayHeaders = { "Sr. No.", "Transaction Id", "Transaction Date", "Transaction Time", "'Batch ID", "URN Details", "Application Date", "Transaction Amount" };
                    String[] fieldsFormat = { "", "", "dd-MMM-yyyy", "", "", "", "dd-MMM-yyyy", "" };
                    XLXporter.writeExcel(excelFullFilePath, dataTable, displayColumns, displayHeaders, fieldsFormat);
                    
                    additionalParams = new HashMap<>();
                    additionalParams.put("_RESPONSE_FILE_", clientExcelPath);
                    additionalParams.put("_RESPONSE_FILE_NAME_", excelFileName); // As per C#
                    message = CommonMessages.FILE_PROCESS_SUCCESS;
                } else {
                    initialStatus = false; // C# _Status = false for no data here
                    message = CommonMessages.NO_DATA_FOUND;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        ApiResponse<Void> response = new ApiResponse<>(initialStatus, message);
        if (additionalParams != null) response.setAdditionalParams(additionalParams);
        return response;
    }
    
    // @AuthorizeExt
    @GetMapping("/PaymentReceipts")
    public String paymentReceiptsView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "PaymentReceipts";
    }

    // @AuthorizeAJAX
    @PostMapping(value="/RequestPaymentReceipts", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> requestPaymentReceiptsAction(HttpServletRequest request, @RequestParam(value="Dummy", required=false) String dummy){
        boolean initialStatus = true; // C# _Status
        String message = "";
        Map<String, String> reportParams = new HashMap<>();

        try {
            String month = request.getParameter("txtMonth");
            if(month == null || month.trim().isEmpty()) throw new IllegalArgumentException("Invalid txtMonth");
            reportParams.put("Month", month.trim());

            String roleCode = PortalSession.getRoleCode(request);
            if(Arrays.asList("CA","WA","IMF","BR").contains(roleCode)){
                reportParams.put("CompanyLoginCode", PortalSession.getUserLoginID(request));
            } else if ("I".equals(roleCode)){ // C# uses RoleCode.In(new string[] { "I"})
                reportParams.put("CompanyLoginCode", PortalSession.getInsurerCode(request));
            }
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if(initialStatus){
            try {
                String paramsJsonString = jacksonObjectMapper.writeValueAsString(reportParams);
                
                // Assuming ReportsInfraService.saveReportRequest returns the message string
                String serviceMessage = reportsInfraService.saveReportRequest(
                    PortalApplication.getConnectionString(), 
                    PortalSession.getInsurerUserID(request), 
                    "PAYMENT_RECEIPT", paramsJsonString);

                if (serviceMessage != null && !serviceMessage.isEmpty()) { // C# logic: non-empty message is success
                    initialStatus = true; // Final status
                    message = serviceMessage; 
                } else { // C# logic: empty message is error
                    initialStatus = true; // C# sets _Status = true here, but message is ERROR_OCCURRED
                    message = CommonMessages.ERROR_OCCURED + " (IIIBL returned empty message for SaveReportRequest)";
                }
            } catch (Exception ex){
                 ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(initialStatus, message);
    }
}