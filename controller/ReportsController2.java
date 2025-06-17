package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse;
import com.example.services.ReportsInfraService; // For IIIBL.ReportsInfra
import com.example.util.CommonMessages;
import com.example.util.ErrorLogger;
import com.example.util.PortalSession;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.InputStreamResource;
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
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

// C# Attributes:
// [AuthorizeExt]
// [AuthorizeAJAX]

@Controller
@RequestMapping("/Reports") // Base path, assuming all report actions are under /Reports
public class ReportsController2 {

    private final ReportsInfraService reportsInfraService;
    private final ServletContext servletContext;

    @Autowired
    public ReportsController(ReportsInfraService reportsInfraService, ServletContext servletContext) {
        this.reportsInfraService = reportsInfraService;
        this.servletContext = servletContext;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName();
    }

    private String getCurrentMethodName() {
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }

    private String getReportsDumpDirectory() {
        String path = PortalApplication.getReportsDumpPath();
        if (path == null || path.isEmpty()) {
            // Fallback or critical error logging
            path = System.getProperty("java.io.tmpdir") + File.separator + "iiireg_reports_dump_fallback";
            ErrorLogger.logError(getCurrentClassName(), "getReportsDumpDirectory",
                    new IOException("PortalApplication.ReportsDump path not configured, using temp: " + path), null);
            new File(path).mkdirs(); // Attempt to create if not exists
        }
        return path;
    }

    // @AuthorizeExt
    @GetMapping("/ReportsDashboard")
    public String reportsDashboardView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ReportsDashboard"; // View name: ReportsDashboard.html or similar
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/LoadReportRequests", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> loadReportRequests(HttpServletRequest request) {
        // C# _Status variable was always true if this block was reached.
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";

        try {
            // Assuming reportsInfraService.getReportRequests returns List<Map<String, Object>>
            // (representing the first DataTable from the DataSet)
            dataTable = reportsInfraService.getReportRequests(
                    PortalApplication.getConnectionString(),
                    PortalSession.getInsurerUserID(request));

            if (dataTable != null) { // Service call didn't throw exception
                if (!dataTable.isEmpty()) {
                    success = true;
                    // message remains empty for success with data
                } else {
                    success = true; // C# logic: success is true even for NO_DATA_FOUND
                    message = CommonMessages.NO_DATA_FOUND;
                    dataTable = new ArrayList<>(); // Ensure client gets empty list, not null
                }
            } else { // Service returned null, treat as NO_DATA_FOUND or error
                success = true; // C# logic: success is true even for NO_DATA_FOUND
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
            dataTable = null; // Ensure data is null on error for ApiResponse
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeExt
    @GetMapping("/DownloadReport")
    public ResponseEntity<InputStreamResource> downloadReport(@RequestParam("FileName") String fileName) {
        String reportsDumpDir = getReportsDumpDirectory();

        try {
            Path filePath = Paths.get(reportsDumpDir, fileName);
            File file = filePath.toFile();

            if (file.exists() && file.isFile()) {
                InputStreamResource resource = new InputStreamResource(new FileInputStream(file));

                String contentType = null;
                try {
                    contentType = Files.probeContentType(filePath);
                } catch (IOException e) {
                    ErrorLogger.logError(getCurrentClassName(), "DownloadReport.MimeTypeProbe", e, null);
                }
                if (contentType == null) {
                    contentType = servletContext.getMimeType(file.getAbsolutePath());
                }
                if (contentType == null) {
                    contentType = "application/octet-stream"; // Fallback
                }
                
                // Original C# code hardcoded the download name to "PaymentReceipts.zip".
                // This implies the FileName parameter might be an internal ID or actual name,
                // but the client always sees "PaymentReceipts.zip".
                String attachmentFilename = "PaymentReceipts.zip"; 

                return ResponseEntity.ok()
                        .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + attachmentFilename + "\"")
                        .contentType(MediaType.parseMediaType(contentType))
                        .contentLength(file.length())
                        .body(resource);
            } else {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), 
                                     new FileNotFoundException("File not found: " + filePath.toString()), null);
                return ResponseEntity.notFound().build();
            }
        } catch (FileNotFoundException ex) { // Catch specific file not found from FileInputStream
             ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, null);
            return ResponseEntity.notFound().build();
        }
        catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, null);
            return ResponseEntity.status(500).build(); // Internal Server Error
        }
    }

    // C# Comment:
    // //Future function would be
    // // 1.   SaveReportsConfig :- For Auto mailers that would be needed in the system
    // Java equivalent placeholder:
    /*
    // @AuthorizeAJAX
    // @PostMapping("/SaveReportsConfig")
    // @ResponseBody
    // public ApiResponse<?> saveReportsConfig(HttpServletRequest request) {
    //     // TODO: Implement logic for saving report configurations
    //     return ApiResponse.success("Configuration saved (not yet implemented).");
    // }
    */
}