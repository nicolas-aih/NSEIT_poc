package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse;
import com.example.services.TelemarketerService;
import com.example.util.CommonMessages;
import com.example.util.ErrorLogger;
import com.example.util.PortalSession;
import com.utils.XLXporter; // Your Excel utility

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.io.File;
import java.nio.file.Paths;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

// C# Attributes:
// [AuthorizeExt]
// [AuthorizeAJAX]
// These would be handled by Spring Security or custom interceptors.

@Controller
@RequestMapping("/Telemarketer")
public class TelemarketerController {

    private final TelemarketerService telemarketerService;

    @Autowired
    public TelemarketerController(TelemarketerService telemarketerService) {
        this.telemarketerService = telemarketerService;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName();
    }

    private String getCurrentMethodName() {
        // A simplified way to get method name for logging, if needed.
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }
    
    private String getDownloadsDir() { // Helper to get and ensure downloads directory
        String path = PortalApplication.getDownloadsPath();
        if (path == null || path.isEmpty()) {
            path = System.getProperty("java.io.tmpdir") + File.separator + "iiireg_downloads_fallback";
            ErrorLogger.logError(getCurrentClassName(), "getDownloadsDir", new IOException("Downloads path not configured, using temp: " + path), null);
            new File(path).mkdirs();
        }
        return path;
    }

    // @AuthorizeExt
    @GetMapping("/RegisterTelemarketer")
    public String registerTelemarketerView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "RegisterTelemarketer"; // View name
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/RegisterTelemarketer", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> registerTelemarketerAction(HttpServletRequest request,
                                                     @RequestParam(value = "Dummy", required = false) String dummy) {
        boolean initialStatus = true; // For parsing phase
        String message = "";
        Map<String, String> additionalParams = null;

        String traiRegNo = "", name = "", address = "", cpName = "", cpEmailId = "", cpContactNo = "";
        String dpName = "", dpEmailId = "", dpContactNo = "", isActiveStr = "";

        try {
            traiRegNo = request.getParameter("txtTraiRegNo");
            name = request.getParameter("txtName");
            address = request.getParameter("txtAddress");
            cpName = request.getParameter("txtCPName");
            cpEmailId = request.getParameter("txtCPEmailId");
            cpContactNo = request.getParameter("txtCPContactNo");
            dpName = request.getParameter("txtDPName");
            dpEmailId = request.getParameter("txtDPEmailId");
            dpContactNo = request.getParameter("txtDPContactNo");
            isActiveStr = request.getParameter("cboIsActive");

            // Validations
            if (traiRegNo == null || traiRegNo.trim().isEmpty()) throw new IllegalArgumentException("Invalid TraiRegNo");
            if (name == null || name.trim().isEmpty()) throw new IllegalArgumentException("Invalid Name");
            if (address == null || address.trim().isEmpty()) throw new IllegalArgumentException("Invalid Address");
            if (cpName == null || cpName.trim().isEmpty()) throw new IllegalArgumentException("Invalid CpName");
            if (cpEmailId == null || cpEmailId.trim().isEmpty()) throw new IllegalArgumentException("Invalid CpEmailId");
            if (cpContactNo != null && !cpContactNo.trim().isEmpty() && cpContactNo.trim().length() != 10) throw new IllegalArgumentException("Invalid CpContactNo: Must be 10 digits if provided.");
            if (dpName == null || dpName.trim().isEmpty()) throw new IllegalArgumentException("Invalid DpName");
            if (dpEmailId == null || dpEmailId.trim().isEmpty()) throw new IllegalArgumentException("Invalid DpEmailId");
            if (dpContactNo != null && !dpContactNo.trim().isEmpty() && dpContactNo.trim().length() != 10) throw new IllegalArgumentException("Invalid DpContactNo: Must be 10 digits if provided.");
            if (isActiveStr == null || isActiveStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid IsActive selection");

        } catch (IllegalArgumentException ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputValidation)", ex, request.getParameterMap());
        } catch (Exception ex) { // Catch any other parsing issues
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT;
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) { // Proceed only if initial parsing and validation were okay
            try {
                String serviceMessage = telemarketerService.registerTelemarketer(
                        PortalApplication.getConnectionString(),
                        PortalSession.getInsurerUserID(request),
                        PortalSession.getUserID(request),
                        traiRegNo.trim(), name.trim(), address.trim(), cpName.trim(), cpEmailId.trim(), 
                        cpContactNo != null ? cpContactNo.trim() : "", // Pass empty if null
                        dpName.trim(), dpEmailId.trim(), 
                        dpContactNo != null ? dpContactNo.trim() : "", // Pass empty if null
                        isActiveStr.trim());

                if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                    initialStatus = true; // Final success
                    message = CommonMessages.DATA_SAVE_SUCCESS;
                    additionalParams = new HashMap<>();
                    additionalParams.put("Success message", CommonMessages.DATA_SAVE_SUCCESS);
                } else {
                    initialStatus = false;
                    message = CommonMessages.DATA_SAVE_FAIL + " : " + serviceMessage;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        ApiResponse<Void> response = new ApiResponse<>(initialStatus, message);
        if (additionalParams != null) {
            response.setAdditionalParams(additionalParams);
        }
        return response;
    }

    // @AuthorizeExt
    @GetMapping("/ManageTelemarketer")
    public String manageTelemarketerView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ManageTelemarketer";
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/EditTelemarketer", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> editTelemarketerAction(HttpServletRequest request,
                                                 @RequestParam(value = "Dummy", required = false) String dummy) {
        boolean initialStatus = true;
        String message = "";
        Map<String, String> additionalParams = null;

        long tmId = 0;
        String traiRegNo = "", name = "", address = "", cpName = "", cpEmailId = "", cpContactNo = "";
        String dpName = "", dpEmailId = "", dpContactNo = "", isActiveStr = "";

        try {
            String tmIdStr = request.getParameter("txt_tmid");
            if (tmIdStr == null || tmIdStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid Tmid");
            tmId = Long.parseLong(tmIdStr.trim());

            traiRegNo = request.getParameter("txtTraiRegNo");
            name = request.getParameter("txtName");
            address = request.getParameter("txtAddress");
            cpName = request.getParameter("txtCPName");
            cpEmailId = request.getParameter("txtCPEmailId");
            cpContactNo = request.getParameter("txtCPContactNo");
            dpName = request.getParameter("txtDPName");
            dpEmailId = request.getParameter("txtDPEmailId");
            dpContactNo = request.getParameter("txtDPContactNo");
            isActiveStr = request.getParameter("cboIsActive");

            // Validations (same as RegisterTelemarketer)
            if (traiRegNo == null || traiRegNo.trim().isEmpty()) throw new IllegalArgumentException("Invalid TraiRegNo");
            if (name == null || name.trim().isEmpty()) throw new IllegalArgumentException("Invalid Name");
            // ... (add all other validations from RegisterTelemarketer)
            if (address == null || address.trim().isEmpty()) throw new IllegalArgumentException("Invalid Address");
            if (cpName == null || cpName.trim().isEmpty()) throw new IllegalArgumentException("Invalid CpName");
            if (cpEmailId == null || cpEmailId.trim().isEmpty()) throw new IllegalArgumentException("Invalid CpEmailId");
            if (cpContactNo != null && !cpContactNo.trim().isEmpty() && cpContactNo.trim().length() != 10) throw new IllegalArgumentException("Invalid CpContactNo");
            if (dpName == null || dpName.trim().isEmpty()) throw new IllegalArgumentException("Invalid DpName");
            if (dpEmailId == null || dpEmailId.trim().isEmpty()) throw new IllegalArgumentException("Invalid DpEmailId");
            if (dpContactNo != null && !dpContactNo.trim().isEmpty() && dpContactNo.trim().length() != 10) throw new IllegalArgumentException("Invalid DpContactNo");
            if (isActiveStr == null || isActiveStr.trim().isEmpty()) throw new IllegalArgumentException("Invalid IsActive");

        } catch (NumberFormatException nfe) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": Invalid Tmid format.";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputValidation-TmId)", nfe, request.getParameterMap());
        } catch (IllegalArgumentException ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputValidation)", ex, request.getParameterMap());
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT;
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                String serviceMessage = telemarketerService.updateTelemarketer(
                        PortalApplication.getConnectionString(),
                        PortalSession.getInsurerUserID(request),
                        PortalSession.getUserID(request), tmId,
                        traiRegNo.trim(), name.trim(), address.trim(), cpName.trim(), cpEmailId.trim(), 
                        cpContactNo != null ? cpContactNo.trim() : "",
                        dpName.trim(), dpEmailId.trim(), 
                        dpContactNo != null ? dpContactNo.trim() : "",
                        isActiveStr.trim());

                if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                    initialStatus = true;
                    message = CommonMessages.DATA_SAVE_SUCCESS;
                    additionalParams = new HashMap<>();
                    additionalParams.put("Success message", CommonMessages.DATA_SAVE_SUCCESS);
                } else {
                    initialStatus = false;
                    message = CommonMessages.DATA_SAVE_FAIL + " : " + serviceMessage;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        ApiResponse<Void> response = new ApiResponse<>(initialStatus, message);
        if (additionalParams != null) {
            response.setAdditionalParams(additionalParams);
        }
        return response;
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/LoadTelemarketerData", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> loadTelemarketerData(HttpServletRequest request) {
        boolean initialStatus = true;
        String message = "";
        List<Map<String, Object>> dataTable = null;
        long tmId = -1L; // Default to fetch all if not applicable
        int isActiveFilter = 0; // As per C# call to GetTelemarketerData

        try {
            String tmIdStr = request.getParameter("tm_id");
            // C# logic converted tm_id to Int32 but then called service with Int64. Assuming tm_id can be long.
            tmId = (tmIdStr == null || tmIdStr.trim().isEmpty()) ? -1L : Long.parseLong(tmIdStr.trim());
            
            String isActiveStr = request.getParameter("is_active");
            // C# reads is_active but then passes 0 hardcoded to the service for this specific load.
            // If isActiveStr was meant to be used for filtering, the service call would change.
            // For now, sticking to C# behavior of passing 0.
            // isActiveFilter = (isActiveStr == null || isActiveStr.trim().isEmpty()) ? 0 : Integer.parseInt(isActiveStr.trim());

        } catch (NumberFormatException nfe) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": Invalid tm_id or is_active format.";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT;
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                // C# logic: objTelemarketer.GetTelemarketerData(..., PortalSession.InsurerUserID, TMId, 0);
                // The last '0' was for isActive filter.
                dataTable = telemarketerService.getTelemarketerData(
                        PortalApplication.getConnectionString(),
                        PortalSession.getInsurerUserID(request),
                        tmId, 0); // Hardcoded 0 for isActiveFilter

                if (dataTable != null && !dataTable.isEmpty()) {
                    initialStatus = true; // Data found
                } else {
                    initialStatus = true; // No data found, but operation successful
                    message = CommonMessages.NO_DATA_FOUND;
                    dataTable = new ArrayList<>(); // Ensure data is empty list
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(initialStatus, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/DeleteTelemarketer", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> deleteTelemarketer(@RequestParam("TmId") long tmId, // Spring can bind directly if param name matches
                                                HttpServletRequest request) {
        boolean initialStatus = true;
        String message = "";
        Map<String, String> additionalParams = null;

        if (tmId == 0) { // Or some other invalid indicator if 0 is a valid ID
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": Invalid Tmid";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputValidation)", 
                                 new IllegalArgumentException("Invalid Tmid: 0"), request.getParameterMap());
        }

        if (initialStatus) {
            try {
                String serviceMessage = telemarketerService.deleteTelemarketer( // Note: C# BL had DeleteTelemarkiter
                        PortalApplication.getConnectionString(),
                        PortalSession.getInsurerUserID(request),
                        PortalSession.getUserID(request), tmId);

                if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                    initialStatus = true;
                    message = CommonMessages.DATA_DELETION_SUCCESS;
                    additionalParams = new HashMap<>();
                    additionalParams.put("Success message", CommonMessages.DATA_DELETION_SUCCESS);
                } else {
                    initialStatus = false;
                    message = CommonMessages.DATA_DELETION_FAIL + " : " + serviceMessage;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        ApiResponse<Void> response = new ApiResponse<>(initialStatus, message);
        if (additionalParams != null) {
            response.setAdditionalParams(additionalParams);
        }
        return response;
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/DownloadData", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> downloadData(HttpServletRequest request) {
        String message = "";
        boolean success = false;
        Map<String, String> additionalParams = null;

        try {
            // Get data
            List<Map<String, Object>> dtTelemarketer = telemarketerService.getTelemarketerData(
                    PortalApplication.getConnectionString(),
                    PortalSession.getInsurerUserID(request), -1, 0); // Fetch all active/inactive for the insurer

            if (dtTelemarketer == null || dtTelemarketer.isEmpty()) {
                 return new ApiResponse<>(true, CommonMessages.NO_DATA_FOUND + " to download.");
            }

            // File naming and path
            String downloadsDir = getDownloadsDir();
            String dateSuffix = new SimpleDateFormat("ddMMMyy").format(new Date());
            // PortalSession.InsurerUserID() might return 0 if not logged in or not an insurer, handle this.
            int insurerUserId = PortalSession.getInsurerUserID(request);
            String filenameBase = (insurerUserId != 0 ? insurerUserId : "ALL") + "_TelemarketerData_" + dateSuffix;
            String excelFileName = filenameBase + ".xlsx";
            String fullOutputFilePath = Paths.get(downloadsDir, excelFileName).toString();
            String clientDownloadPath = Paths.get("Downloads", excelFileName).toString().replace("\\", "/");


            String[] displayColumns = new String[] { "tm_name", "tm_trai_reg_no", "tm_address", "tm_contact_person_name", "tm_cp_email_id", "tm_cp_contact_no", "tm_designated_person_name", "tm_dp_email_id", "tm_dp_contact_no", "is_active" };
            String[] displayHeaders = new String[] { "Telemarketer's Name", "TRAI Reg. No.", "Address", "Contact Person's Name", "Contact Person's Email Id", "Contact Person's Mobile No.", "Designated Person's Name", "Designated Person's Email Id", "Designated Person's Mobile No.", "Is Active ?" };
            String[] displayFormat = new String[displayColumns.length]; 
            Arrays.fill(displayFormat, "");

            XLXporter.writeExcel(fullOutputFilePath, dtTelemarketer, displayColumns, displayHeaders, displayFormat);

            additionalParams = new HashMap<>();
            additionalParams.put("_RESPONSE_FILE_", clientDownloadPath);
            success = true;
            message = CommonMessages.FILE_PROCESS_SUCCESS;

        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(success, message);
        if (additionalParams != null) {
            response.setAdditionalParams(additionalParams);
        }
        return response;
    }
}