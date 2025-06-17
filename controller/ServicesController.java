package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse;
import com.example.services.*; // Import all services
import com.example.integration.CssSchedulingService; // For CSSIntegration
import com.example.interfaces.UrnDataAccess; // For nested DTOs if directly used by controller
import com.example.interfaces.UserDataAccess;
import com.example.interfaces.MasterDataDataAccess;
import com.example.interfaces.NotificationsDataAccess;
import com.example.interfaces.ExamCentersDataAccess;
import com.example.interfaces.DPRangeMstDataAccess;
import com.example.interfaces.BatchMgmtDataAccess;
import com.example.util.CommonMessages;
import com.example.util.ErrorLogger;
import com.example.util.PortalSession;
import com.iiibl.AadhaarEncryptorDecryptor; // Assuming this path for the util

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import java.nio.charset.StandardCharsets;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.ZoneId;
import java.util.*;
import java.util.function.BiFunction;
import java.util.function.Function;
import java.util.stream.Collectors;
import java.math.BigDecimal;


// C# Attributes:
// [AuthorizeAJAX]
// These would be handled by Spring Security or custom interceptors.

@Controller
@RequestMapping("/Services")
public class ServicesController {

    private final UrnService urnService;
    private final MasterDataService masterDataService;
    private final NotificationsService notificationsService;
    private final UserService userService;
    private final ExamCentersService examCentersService;
    private final DPRangeMstService dpRangeMstService;
    private final BatchMgmtService batchMgmtService;
    private final TelemarketerService telemarketerService; // For GetDetailsForCOR
    private final CssSchedulingService cssSchedulingService; // For CSSIntegration

    @Autowired
    public ServicesController(UrnService urnService, MasterDataService masterDataService,
                              NotificationsService notificationsService, UserService userService,
                              ExamCentersService examCentersService, DPRangeMstService dpRangeMstService,
                              BatchMgmtService batchMgmtService, TelemarketerService telemarketerService,
                              CssSchedulingService cssSchedulingService) {
        this.urnService = urnService;
        this.masterDataService = masterDataService;
        this.notificationsService = notificationsService;
        this.userService = userService;
        this.examCentersService = examCentersService;
        this.dpRangeMstService = dpRangeMstService;
        this.batchMgmtService = batchMgmtService;
        this.telemarketerService = telemarketerService;
        this.cssSchedulingService = cssSchedulingService;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName();
    }

    private String getCurrentMethodName() {
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }

    private LocalDate toLocalDate(Date date) {
        if (date == null) return null;
        return date.toInstant().atZone(ZoneId.systemDefault()).toLocalDate();
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateDOB", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateDOB(@RequestParam("date") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date date) {
        LocalDate dob = toLocalDate(date);
        LocalDate today = LocalDate.now();
        boolean success = dob != null && (dob.isBefore(today.minusYears(18)) || dob.isEqual(today.minusYears(18)));
        return new ApiResponse<>(success, "");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateYOP", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateYOP(@RequestParam("date") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date date) {
        LocalDate yopDate = toLocalDate(date);
        LocalDate today = LocalDate.now();
        boolean success = yopDate != null && (yopDate.isBefore(today) || yopDate.isEqual(today));
        return new ApiResponse<>(success, "");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateFromTillDate", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateFromTillDate(
            @RequestParam("FromDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date fromDate,
            @RequestParam("TillDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date tillDate) {
        LocalDate from = toLocalDate(fromDate);
        LocalDate till = toLocalDate(tillDate);
        // C# Compare(date1.Date, date2) implies comparing date part of fromDate with full tillDate.
        // For consistency, comparing date parts of both.
        boolean success = (from != null && till != null) && (from.isBefore(till) || from.isEqual(till));
        return new ApiResponse<>(success, "");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateNotificationFromDate", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateNotificationFromDate(@RequestParam("date") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date date) {
        LocalDate notificationDate = toLocalDate(date);
        LocalDate today = LocalDate.now();
        boolean success = notificationDate != null && (notificationDate.isAfter(today) || notificationDate.isEqual(today));
        return new ApiResponse<>(success, "");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateYOP2", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateYOP2(
            @RequestParam("dateDOB") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date dateDOB,
            @RequestParam("dateYOP") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) Date dateYOP) {
        LocalDate dob = toLocalDate(dateDOB);
        LocalDate yop = toLocalDate(dateYOP);
        boolean success = (dob != null && yop != null) && dob.isBefore(yop);
        return new ApiResponse<>(success, "");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidatePAN", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validatePAN(@RequestParam("PAN") String pan, HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            String validationResult = urnService.validatePAN(PortalApplication.getConnectionString(), pan);
            switch (validationResult != null ? validationResult.toUpperCase() : "EXCEPTION_FALLBACK") {
                case "DEBARRED":
                    status = false;
                    message = "The PAN entered is debarred";
                    break;
                case "AVAILABLE":
                    status = true;
                    // message remains empty
                    break;
                case "EXCEPTION": // Fallthrough
                default:
                    status = false;
                    message = "Exception occurred while validating PAN";
                    if (validationResult != null && !validationResult.equalsIgnoreCase("EXCEPTION") && !validationResult.equalsIgnoreCase("AVAILABLE") && !validationResult.equalsIgnoreCase("DEBARRED")) {
                        message += " (Details: " + validationResult + ")"; // Include unknown message
                    }
                    break;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidatePAN2", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validatePAN2(@RequestParam("PAN") String pan,
                                          @RequestParam("ApplicantId") long applicantId, // C# was Int64
                                          HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            message = urnService.validatePAN(PortalApplication.getConnectionString(), pan, applicantId, PortalSession.getInsurerUserID(request));
            status = (message == null || message.isEmpty());
            // If message is not empty, it contains the error/reason.
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }
    
    // --- Other validation methods from UrnService (ValidateInternalRefNo, Aadhaar, Email, Mobile etc.) ---
    // These were defined in previous ServicesController conversion, assuming UrnService provides them.
    // Example for ValidateInternalRefNo:
    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateInternalRefNo", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateInternalRefNo(@RequestParam("InternalRefNo") String internalRefNo,
                                                   @RequestParam("ApplicantId") int applicantId, // C# was Int32
                                                   HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            status = urnService.validateInternalRefNo(PortalApplication.getConnectionString(), internalRefNo, PortalSession.getInsurerUserID(request), applicantId);
            if (!status) {
                message = "The entered number is already in use.";
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateInternalRefNoApp", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateInternalRefNoApp(@RequestParam("InternalRefNo") String internalRefNo,
                                                      @RequestParam("ApplicantDataId") int applicantDataId, // C# was Int32
                                                      HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            status = urnService.validateInternalRefNoApp(PortalApplication.getConnectionString(), internalRefNo, PortalSession.getInsurerUserID(request), applicantDataId);
            if (!status) {
                message = "The entered number is already in use.";
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateAadhaarCorporates", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateAadhaarCorporates(@RequestParam("AadhaarNo") String aadhaarNo,
                                                       @RequestParam("PAN") String pan,
                                                       @RequestParam(value = "URN", required = false, defaultValue = "") String urn,
                                                       HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            byte[] keyBytes = PortalApplication.getAadhaarKey().getBytes(StandardCharsets.UTF_8);
            byte[] ivBytes = PortalApplication.getAadhaarIV().getBytes(StandardCharsets.UTF_8);
            String encryptedAadhaar = AadhaarEncryptorDecryptor.EncryptAadhaar(aadhaarNo, keyBytes, ivBytes);
            
            message = urnService.validateAadhaarCorporates(PortalApplication.getConnectionString(), encryptedAadhaar, pan, urn);
            if ("AVAILABLE".equalsIgnoreCase(message)) {
                message = "";
                status = true;
            } else {
                status = false; // Message from service contains the reason
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/ValidateAadhaarCorporatesApp", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> validateAadhaarCorporatesApp(@RequestParam("AadhaarNo") String aadhaarNo,
                                                          @RequestParam("PAN") String pan,
                                                          @RequestParam("ApplicantDataId") long applicantDataId, // C# was Int64
                                                          HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            byte[] keyBytes = PortalApplication.getAadhaarKey().getBytes(StandardCharsets.UTF_8);
            byte[] ivBytes = PortalApplication.getAadhaarIV().getBytes(StandardCharsets.UTF_8);
            String encryptedAadhaar = AadhaarEncryptorDecryptor.EncryptAadhaar(aadhaarNo, keyBytes, ivBytes);
            
            message = urnService.validateAadhaarCorporatesApp(PortalApplication.getConnectionString(), encryptedAadhaar, pan, applicantDataId);
            if ("AVAILABLE".equalsIgnoreCase(message)) {
                message = "";
                status = true;
            } else {
                status = false;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }
    
    // Helper for similar validation methods
    private ApiResponse<Void> genericValidateCorporateDetail(String detailType, String value, String pan, Long applicantId, String urn, boolean isAppEndpoint, HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            String serviceMessage;
            switch (detailType) {
                case "Email":
                    serviceMessage = isAppEndpoint ? urnService.validateEmailCorporatesApp(PortalApplication.getConnectionString(), value, applicantId, pan)
                                        : urnService.validateEmailCorporates(PortalApplication.getConnectionString(), value, applicantId, pan);
                    break;
                case "Mobile":
                     serviceMessage = isAppEndpoint ? urnService.validateMobileCorporatesApp(PortalApplication.getConnectionString(), value, applicantId, pan)
                                        : urnService.validateMobileCorporates(PortalApplication.getConnectionString(), value, applicantId, pan);
                    break;
                case "WhatsApp":
                     serviceMessage = isAppEndpoint ? urnService.validateWhatsAppCorporatesApp(PortalApplication.getConnectionString(), value, applicantId, pan)
                                        : urnService.validateWhatsAppCorporates(PortalApplication.getConnectionString(), value, applicantId, pan);
                    break;
                case "WhatsAppMod": // URN instead of applicantId
                    serviceMessage = urnService.validateWhatsAppCorporatesForMod(PortalApplication.getConnectionString(), urn, value);
                    break;
                default:
                    throw new IllegalArgumentException("Invalid detailType for generic validation: " + detailType);
            }
            if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                status = true;
            } else {
                status = false;
                message = serviceMessage; // Use message from service
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), "validate" + detailType + (isAppEndpoint ? "App" : (detailType.equals("WhatsAppMod") ? "ForMod" : "")), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }

    @PostMapping(value = "/ValidateEmailCorporates", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateEmailCorporates(@RequestParam("EmailId") String emailId, @RequestParam("Applicantid") Long applicantId, @RequestParam("PAN") String pan, HttpServletRequest request) {
        return genericValidateCorporateDetail("Email", emailId, pan, applicantId, null, false, request);
    }
    @PostMapping(value = "/ValidateEmailCorporatesApp", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateEmailCorporatesApp(@RequestParam("EmailId") String emailId, @RequestParam("ApplicantDataId") Long applicantDataId, @RequestParam("PAN") String pan, HttpServletRequest request) {
        return genericValidateCorporateDetail("Email", emailId, pan, applicantDataId, null, true, request);
    }
    @PostMapping(value = "/ValidateMobileCorporates", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateMobileCorporates(@RequestParam("MobileNo") String mobileNo, @RequestParam("Applicantid") Long applicantId, @RequestParam("PAN") String pan, HttpServletRequest request) {
        return genericValidateCorporateDetail("Mobile", mobileNo, pan, applicantId, null, false, request);
    }
    @PostMapping(value = "/ValidateWhatsAppCorporates", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateWhatsAppCorporates(@RequestParam("MobileNo") String mobileNo, @RequestParam("Applicantid") Long applicantId, @RequestParam("PAN") String pan, HttpServletRequest request) {
        return genericValidateCorporateDetail("WhatsApp", mobileNo, pan, applicantId, null, false, request);
    }
    @PostMapping(value = "/ValidateMobileCorporatesApp", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateMobileCorporatesApp(@RequestParam("MobileNo") String mobileNo, @RequestParam("ApplicantDataId") Long applicantDataId, @RequestParam("PAN") String pan, HttpServletRequest request) {
        return genericValidateCorporateDetail("Mobile", mobileNo, pan, applicantDataId, null, true, request);
    }
    @PostMapping(value = "/ValidateWhatsAppCorporatesApp", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateWhatsAppCorporatesApp(@RequestParam("MobileNo") String mobileNo, @RequestParam("ApplicantDataId") Long applicantDataId, @RequestParam("PAN") String pan, HttpServletRequest request) {
        return genericValidateCorporateDetail("WhatsApp", mobileNo, pan, applicantDataId, null, true, request);
    }
    @PostMapping(value = "/ValidateWhatsAppCorporatesForMod", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> validateWhatsAppCorporatesForMod(@RequestParam("URN") String urn, @RequestParam("MobileNo") String mobileNo, HttpServletRequest request) {
        return genericValidateCorporateDetail("WhatsAppMod", mobileNo, null, null, urn, false, request);
    }

    // --- Methods using MasterDataService, NotificationsService, UserService, ExamCentersService, DPRangeMstService ---
    // These follow a similar pattern: call service, handle List<Map> or DTO, return ApiResponse
    // Example: GetACforDP
    // @AuthorizeAJAX
    @PostMapping(value = "/GetACforDP", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getACforDP(@RequestParam("InsurerID") int insurerID,
                                                             @RequestParam("DPUserID") int dpUserID,
                                                             @RequestParam("ACUserId") int acUserId,
                                                             HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            // Assuming MasterDataService.getACForDPs returns List<Map> (was DataSet.Tables[0])
            dataTable = masterDataService.getACForDPs(PortalApplication.getConnectionString(), insurerID, dpUserID, acUserId);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic: success false if no data
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>(); // Ensure empty list for JSON
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // --- Methods using NotificationsService ---
    // @AuthorizeAJAX
    @PostMapping(value = "/GetAllNotifications", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getAllNotifications(
            @RequestParam(value = "NotificationId", defaultValue = "-1") int notificationId,
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = notificationsService.getAllNotifications(PortalApplication.getConnectionString(), notificationId);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    @PostMapping(value = "/GetTbxSchedule", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getTbxSchedule(HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = notificationsService.getTBXSchedule(PortalApplication.getConnectionString());
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }
    
    // --- Methods using UserService (Roles, Users) ---
    // @AuthorizeAJAX
    @PostMapping(value = "/GetAllRoles", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getAllRoles(
            @RequestParam(value = "RoleId", defaultValue = "-1") int roleId, 
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            // UserService.getRoles returns List<Map>
            dataTable = userService.getRoles(PortalApplication.getConnectionString(), roleId);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetRolesForUserCreation", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getRolesForUserCreation(HttpServletRequest request) {
         List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = userService.getRolesForUserCreation(PortalApplication.getConnectionString());
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetUsers", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getUsers(
            @RequestParam(value = "UserId", defaultValue = "-1") int userId, 
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = userService.getUsers(PortalApplication.getConnectionString(), userId);
             if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    @PostMapping(value = "/GetAllUsers", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getAllUsers(
            @RequestParam(value = "UserId", defaultValue = "-1") int userId, // C# has this param but calls GetAllUsers() without it.
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            // C# BL.GetAllUsers() doesn't take UserId. Assuming this for Java service too.
            dataTable = userService.getAllUsers(PortalApplication.getConnectionString()); 
             if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        // C# MaxJsonLength not directly applicable; configure Jackson/Spring if needed for very large responses.
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetDPForInsurer", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getDPForInsurer(
            @RequestParam("InsurerId") int insurerId,
            @RequestParam("DPId") int dpId,
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            int effectiveDpId = dpId;
            String roleCode = PortalSession.getRoleCode(request);
            String roleName = PortalSession.getRoleName(request);

            if (Arrays.asList("I", "superadmin").contains(roleCode)) {
                if ("Designated Person".equals(roleName)) {
                    effectiveDpId = PortalSession.getDPUserID(request); // Make sure PortalSession has this
                }
            }
            // Assuming masterDataService.getDPForInsurer returns List<Map>
            dataTable = masterDataService.getDPForInsurer(PortalApplication.getConnectionString(), insurerId, effectiveDpId);
            
            if (dataTable != null) {
                // Remove imgDPSignature column
                dataTable = dataTable.stream()
                    .map(row -> {
                        Map<String, Object> newRow = new HashMap<>(row);
                        newRow.remove("imgDPSignature");
                        return newRow;
                    })
                    .collect(Collectors.toList());

                if (!dataTable.isEmpty()) {
                    success = true;
                } else {
                    success = true; // C# logic: success true even if no data
                    message = CommonMessages.NO_DATA_FOUND;
                }
            } else {
                success = true; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetDPForInsurerEx", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getDPForInsurerEx(
            @RequestParam("InsurerId") int insurerId,
            @RequestParam("DPId") int dpId,
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = masterDataService.getDPForInsurer(PortalApplication.getConnectionString(), insurerId, dpId); // Assuming same underlying DAL call
            
            if (dataTable != null && !dataTable.isEmpty()) {
                // Add imgDPSignatureB64 and remove original
                 dataTable = dataTable.stream()
                    .map(row -> {
                        Map<String, Object> newRow = new HashMap<>(row);
                        Object signatureObj = newRow.get("imgDPSignature");
                        if (signatureObj instanceof byte[]) {
                            newRow.put("imgDPSignatureB64", Base64.getEncoder().encodeToString((byte[]) signatureObj));
                        } else {
                            newRow.put("imgDPSignatureB64", null);
                        }
                        newRow.remove("imgDPSignature");
                        return newRow;
                    })
                    .collect(Collectors.toList());
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetInsurer", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getInsurer(HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            int insurerUserIdToFetch = -1;
            String roleCode = PortalSession.getRoleCode(request);
            String roleName = PortalSession.getRoleName(request);

            if (Arrays.asList("I", "superadmin").contains(roleCode)) {
                if ("Corporate Designated Person".equals(roleName) || "Designated Person".equals(roleName)) {
                    insurerUserIdToFetch = PortalSession.getInsurerUserID(request);
                }
            }
            dataTable = masterDataService.getInsurers(PortalApplication.getConnectionString(), insurerUserIdToFetch);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetInsurer2", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getInsurer2(
            @RequestParam(value = "InsurerId", defaultValue = "-1") int insurerId,
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = masterDataService.getInsurers2(PortalApplication.getConnectionString(), insurerId);
             if (dataTable != null && !dataTable.isEmpty()) {
                if (insurerId != -1) { // Process signature only if specific insurerId
                     dataTable = dataTable.stream()
                        .map(row -> {
                            Map<String, Object> newRow = new HashMap<>(row);
                            Object signatureObj = newRow.get("imgCDPSignature");
                            if (signatureObj instanceof byte[]) {
                                newRow.put("imgCDPSignatureB64", Base64.getEncoder().encodeToString((byte[]) signatureObj));
                            } else {
                                newRow.put("imgCDPSignatureB64", null);
                            }
                            newRow.remove("imgCDPSignature");
                            return newRow;
                        })
                        .collect(Collectors.toList());
                }
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetBasicQualificationForCOR", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getBasicQualificationForCOR(
            @RequestParam("CORType") String corType,
            HttpServletRequest request) {
        List<Map<String, Object>> resultDataTable = null;
        String message = "";
        boolean success = true; // C# default
        try {
            // Assumes PortalApplication.BasicQualificationSpecific is List<Map<String, Object>>
            List<Map<String, Object>> sourceTable = PortalApplication.BasicQualificationSpecific;
            if (corType != null && !corType.isEmpty()) {
                resultDataTable = sourceTable.stream()
                    .filter(row -> corType.equals(String.valueOf(row.get("cor_type")))) // Ensure safe get and compare
                    .collect(Collectors.toList());
            } else {
                resultDataTable = new ArrayList<>(sourceTable); // Copy if no filter
            }
             // C# logic doesn't set NO_DATA_FOUND if filtered list is empty, success is true
            if(resultDataTable.isEmpty() && (corType != null && !corType.isEmpty())) {
                // message remains empty, success true, data empty as per C#
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
            resultDataTable = null;
        }
        return new ApiResponse<>(success, message, resultDataTable);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetProQualificationForCOR", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getProQualificationForCOR(HttpServletRequest request) {
        List<Map<String, Object>> resultDataTable = null;
        String message = "";
        boolean success = true;
        String corType = "";

        try {
            corType = request.getParameter("cboCorType");
            if (corType == null || !Arrays.asList("IA", "AV", "PO", "SP", "ISP").contains(corType.toUpperCase())) {
                 ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + ": Invalid CORType " + corType, new IllegalArgumentException("Invalid CORType"), request.getParameterMap());
                 return ApiResponse.error(CommonMessages.INVALID_INPUT, null);
            }
            String roleCode = PortalSession.getRoleCode(request);
            List<Map<String, Object>> sourceTable = PortalApplication.ProQualification; // Assumes List<Map>

            resultDataTable = sourceTable.stream()
                .filter(row -> roleCode.equals(String.valueOf(row.get("role_code"))) && 
                               corType.equals(String.valueOf(row.get("cor_type"))))
                .collect(Collectors.toList());
            
            // C# success=true, message empty even if filtered list is empty
            // if(resultDataTable.isEmpty()){ message = CommonMessages.NO_DATA_FOUND; }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
            resultDataTable = null;
        }
        return new ApiResponse<>(success, message, resultDataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetDetailsForCOR", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Map<String, List<Map<String, Object>>>> getDetailsForCOR(HttpServletRequest request) {
        Map<String, List<Map<String, Object>>> resultData = new HashMap<>();
        String message = "";
        boolean success = true; // C# _Status
        String corType = "";

        try {
            corType = request.getParameter("cboCorType");
            if (corType == null || !Arrays.asList("IA", "AV", "PO", "SP", "ISP").contains(corType.toUpperCase())) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + ": Invalid CORType " + corType, new IllegalArgumentException("Invalid CORType"), request.getParameterMap());
                return new ApiResponse<>(false, CommonMessages.INVALID_INPUT, null);
            }
            String roleCode = PortalSession.getRoleCode(request);

            List<Map<String, Object>> basicQual = PortalApplication.BasicQualificationSpecific.stream()
                .filter(row -> roleCode.equals(row.get("role_code")) && corType.equals(row.get("cor_type")))
                .collect(Collectors.toList());
            resultData.put("basicQualifications", basicQual);

            List<Map<String, Object>> proQual = PortalApplication.ProQualification.stream()
                .filter(row -> roleCode.equals(row.get("role_code")) && corType.equals(row.get("cor_type")))
                .collect(Collectors.toList());
            resultData.put("proQualifications", proQual);

            // Assuming TelemarketerService.getTelemarketerData returns List<Map>
            List<Map<String, Object>> telemarketers = telemarketerService.getTelemarketerData(
                PortalApplication.getConnectionString(), PortalSession.getInsurerUserID(request), -1L, 1); // -1 for TMId, 1 for IsActive
            
            resultData.put("telemarketers", telemarketers != null ? telemarketers : new ArrayList<>());
            
            // C# sets Success=true, Message=NO_DATA_FOUND if objDataSet for telemarketers is null/empty
            // Here, if telemarketers list is null or empty, it's included as such.
            // The overall success remains true unless an exception occurs.
            if (telemarketers == null || telemarketers.isEmpty()) {
                // C# set message for this specific case. If other tables have data, this might be misleading for overall response.
                // message = CommonMessages.NO_DATA_FOUND; // Commenting this to reflect overall success for structure
            }

        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
            resultData = null;
        }
        // HelperUtilities.ToJSON2 (multiple tables) is mapped to ApiResponse with data as Map<String, List<Map>>
        return new ApiResponse<>(success, message, resultData);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/FindNearestExamCenter", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> findNearestExamCenter(
            @RequestParam("Pincode") int pincode, HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = examCentersService.getExamCenters(PortalApplication.getConnectionString(), pincode);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = true; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetCentersForState", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getCentersForState(
            @RequestParam("StateId") int stateId, 
            @RequestParam("CenterId") int centerId, 
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = examCentersService.examCentersForState(PortalApplication.getConnectionString(), stateId, centerId);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetCentersForStateEx", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getCentersForStateEx(
            @RequestParam("StateId") int stateId, 
            @RequestParam("CenterId") int centerId, 
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = examCentersService.examCentersForStateEx(PortalApplication.getConnectionString(), stateId, centerId);
             if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetSimilarCenters", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getSimilarCenters(
            @RequestParam("CenterId") int centerId, HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = examCentersService.similarExamCenters(PortalApplication.getConnectionString(), centerId);
             if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }
    
    @PostMapping(value = "/GetCentersForStatePreL", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getCentersForStatePreL(
            @RequestParam("StateId") int stateId, HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = examCentersService.examCentersForState(PortalApplication.getConnectionString(), stateId, -1); // CenterId hardcoded
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveExamCenter", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> saveExamCenter(HttpServletRequest request) {
        boolean status = true; // C# initial status
        String message = "";
        try {
            String centerIdStr = request.getParameter("hdnExamCenterId");
            int centerId = (centerIdStr == null || centerIdStr.trim().equals("0") || centerIdStr.trim().isEmpty()) ? 0 : Integer.parseInt(centerIdStr);
            String centerName = request.getParameter("txtExamCenterName");
            String centerCode = request.getParameter("txtExamCenterCode");
            String addressLine1 = request.getParameter("txtAddress1");
            String addressLine2 = request.getParameter("txtAddress2");
            String addressLine3 = request.getParameter("txtAddress3");
            int cssCode = 0;
            if ("OAIMS".equals(PortalApplication.getIntegrationMode())) {
                cssCode = -1;
            } else if ("CSS".equals(PortalApplication.getIntegrationMode())) {
                String cssCodeStr = request.getParameter("txtCSSCode");
                cssCode = (cssCodeStr == null || cssCodeStr.trim().isEmpty()) ? 0 : Integer.parseInt(cssCodeStr);
            }
            int districtId = Integer.parseInt(request.getParameter("cboDistricts"));
            int pincode = Integer.parseInt(request.getParameter("txtPincode"));
            String isActiveStr = request.getParameter("chkIsActive");
            boolean isActive = "on".equalsIgnoreCase(isActiveStr) || "true".equalsIgnoreCase(isActiveStr);
            String centerType = request.getParameter("cboCenterType");
            if (centerType == null) centerType = "";

            // Call service
            String serviceMessage = examCentersService.saveCenterDetails(PortalApplication.getConnectionString(), 
                centerId, centerName, centerCode, cssCode, 5, // 5 for EntityTypeID
                addressLine1, addressLine2, addressLine3, districtId, pincode, isActive, centerType, 
                PortalSession.getUserID(request));

            if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                status = true;
                message = CommonMessages.DATA_SAVE_SUCCESS;
            } else {
                status = false;
                message = CommonMessages.DATA_SAVE_FAIL + " : " + serviceMessage;
            }
        } catch (NumberFormatException nfe) {
            status = false;
            message = CommonMessages.INVALID_INPUT + " (Numeric field error)";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            status = false;
            message = CommonMessages.INVALID_INPUT; // C# returned this on initial parsing error
            if (!message.contains("Numeric field error")) { // Avoid duplicate common message
                 message = CommonMessages.ERROR_OCCURED; // For service call errors
            }
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
        }
        return new ApiResponse<>(status, message);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetMenuData", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getMenuData(
            @RequestParam("intSearchId") int searchId, 
            @RequestParam("isRole") int isRoleInt, 
            HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            boolean isRoleBoolean = (isRoleInt == 1);
            // Assuming userService.getMenuPermissions returns List<Map>
            dataTable = userService.getMenuPermissions(PortalApplication.getConnectionString(), searchId, isRoleBoolean);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = true; // C# logic
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveMenuPermission", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> saveMenuPermission( // C# returns objDataTable, but it's null in success path
            @RequestParam("intSearchId") int searchId,
            @RequestParam("isRole") int isRoleInt,
            @RequestParam("MenuId[]") String[] menuIds,
            @RequestParam("oldvalue[]") String[] oldValues,
            @RequestParam("newValue[]") String[] newValues,
            HttpServletRequest request) {
        
        boolean success = false;
        String message = "";
        List<Map<String, Object>> resultDataTable = null; // C# version returns this, but it's often null

        try {
            if (menuIds == null || oldValues == null || newValues == null || 
                menuIds.length != oldValues.length || menuIds.length != newValues.length) {
                return ApiResponse.error(CommonMessages.INVALID_INPUT + " (Mismatched permission arrays)");
            }

            boolean isRoleBoolean = (isRoleInt == 1);
            
            StringBuilder xmlBuilder = new StringBuilder("<dsMenu><TblMenu>"); // Simplified XML
            boolean changesMade = false;
            // C# loop starts from 1, assuming arr[0] might be a header or placeholder from client
            for (int i = (menuIds.length > 0 && "header_placeholder".equals(menuIds[0]) ? 1 : 0); i < menuIds.length; i++) {
                if (!oldValues[i].equals(newValues[i])) {
                    changesMade = true;
                    xmlBuilder.append("<Row>"); // Assuming SP expects rows
                    xmlBuilder.append("<sntMenuId>").append(menuIds[i]).append("</sntMenuId>");
                    xmlBuilder.append("<varActionName>MenuAccess</varActionName>");
                    xmlBuilder.append("<IsRevoked>").append(newValues[i]).append("</IsRevoked>");
                    xmlBuilder.append("</Row>");
                }
            }
            xmlBuilder.append("</TblMenu></dsMenu>");

            if (!changesMade && menuIds.length > 0 && !(menuIds.length == 1 && "header_placeholder".equals(menuIds[0]))) {
                 // If no changes, C# code would proceed and likely return success from DB if SP handles empty XML.
                 // For Java, we might return early or ensure the service handles empty XML string appropriately.
                 // For now, let it proceed to service.
            }
            
            // Assuming userService.saveMenuPermissions returns List<Map> (DataSet in C#)
            resultDataTable = userService.saveMenuPermissions(PortalApplication.getConnectionString(), 
                                (changesMade ? xmlBuilder.toString() : "<dsMenu><TblMenu></TblMenu></dsMenu>"), // Pass empty if no changes
                                searchId, isRoleBoolean);
            
            success = true; // C# sets success true and message
            message = "Roles Saved Successfully";
            // The resultDataTable from C# SP was not consistently used, often null.

        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, resultDataTable); // resultDataTable might be null as in C#
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetDPRangeData", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getDPRangeData(HttpServletRequest request) {
        List<Map<String, Object>> dataTable = null;
        boolean success = false;
        String message = "";
        try {
            dataTable = dpRangeMstService.getDPRangeList(PortalApplication.getConnectionString(), 0);
            if (dataTable != null && !dataTable.isEmpty()) {
                success = true;
            } else {
                success = false; // C# logic: success false if no data
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>();
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveDPRange", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> saveDPRange(@RequestParam("Insurercode") int insurerCode,
                                         @RequestParam("dpCount") int dpCount,
                                         HttpServletRequest request) {
        boolean status = false;
        String message = "";
        try {
            // dpRangeId = 0, createdBy = PortalSession.UserID from C#
            String serviceMessage = dpRangeMstService.saveDPRange(PortalApplication.getConnectionString(), 0, insurerCode, dpCount, PortalSession.getUserID(request));
            if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                status = true;
                message = CommonMessages.DATA_SAVE_SUCCESS;
            } else {
                status = false;
                message = CommonMessages.DATA_SAVE_FAIL + " : " + serviceMessage;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false; // C# sets message, implies status false
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }
    
    // --- Batch Management Region ---
    @PostMapping(value = "/GetBatchDetailsForPayment", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getBatchDetailsForPayment(HttpServletRequest request) {
        boolean success = false;
        String message = "";
        List<Map<String, Object>> urnDetailsTable = null;
        Map<String, String> additionalParams = null;

        try {
            String transactionId = request.getParameter("txtBatchNo");
            if (transactionId == null || transactionId.isEmpty()) {
                return new ApiResponse<>(false, "Required Parameter Not Found", null);
            }

            BatchMgmtDataAccess.BatchDetailsPaymentPGResult result = batchMgmtService.getBatchDetailsForPayment_PG(
                PortalApplication.getConnectionString(), transactionId);
            
            message = result.getMessage(); // Message from service
            if (result.isCanProceed()) {
                if (result.getUrnDetails() != null && !result.getUrnDetails().isEmpty() && 
                    result.getSummaryDetails() != null && !result.getSummaryDetails().isEmpty()) {
                    
                    urnDetailsTable = result.getUrnDetails();
                    Map<String, Object> summary = result.getSummaryDetails(); // Assuming first row if it's a list
                    
                    additionalParams = new HashMap<>();
                    additionalParams.put("transaction_id", transactionId);
                    additionalParams.put("grand_total", String.valueOf(summary.get("grand_total")));
                    additionalParams.put("payment_mode", String.valueOf(summary.get("payment_mode")));
                    additionalParams.put("total_urns", String.valueOf(summary.get("total_urns")));
                    
                    success = true;
                    if (message == null || message.isEmpty()) message = ""; // Clear message on full success
                } else {
                    success = false; // C# set success=false
                    if (message == null || message.isEmpty()) message = CommonMessages.NO_DATA_FOUND;
                }
            } else {
                success = false; // Cannot proceed, message should have reason
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<List<Map<String, Object>>> response = new ApiResponse<>(success, message, urnDetailsTable);
        if (success && additionalParams != null) {
            response.setAdditionalParams(additionalParams);
        }
        return response;
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetBatchDetailsForMgmt", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Map<String, List<Map<String, Object>>>> getBatchDetailsForMgmt(HttpServletRequest request) {
        boolean success = false;
        String message = "";
        Map<String, List<Map<String, Object>>> responseData = null; // For URNs and Problem URNs tables
        Map<String, String> additionalParams = null; // For summary data

        try {
            String transactionId = request.getParameter("txtBatchNo");
            if (transactionId == null || transactionId.isEmpty()) {
                return new ApiResponse<>(false, "Required Parameter Not Found", null);
            }
            
            BatchMgmtDataAccess.BatchDetailsMgmtResult result = batchMgmtService.getBatchDetailsForPayment( // C# calls this with hint=2
                PortalApplication.getConnectionString(), 2, transactionId);
            
            message = result.getMessage();
            if (message == null || message.isEmpty()) {
                 if (result.getUrnDetails() != null && !result.getUrnDetails().isEmpty() && 
                    result.getSummaryDetails() != null && !result.getSummaryDetails().isEmpty()) {
                    
                    responseData = new HashMap<>();
                    responseData.put("urnDetails", result.getUrnDetails()); // Table0
                    responseData.put("problemUrns", result.getProblemUrns() != null ? result.getProblemUrns() : new ArrayList<>()); // Table2

                    Map<String, Object> summary = result.getSummaryDetails(); // Assuming first row
                    additionalParams = new HashMap<>();
                    additionalParams.put("transaction_id", transactionId);
                    additionalParams.put("grand_total", String.valueOf(summary.get("grand_total")));
                    additionalParams.put("payment_mode", String.valueOf(summary.get("payment_mode")));
                    additionalParams.put("total_urns", String.valueOf(summary.get("total_urns")));
                    additionalParams.put("StatusId", String.valueOf(summary.get("status_id")));
                    additionalParams.put("PaymentStatus", String.valueOf(summary.get("payment_status")));
                    additionalParams.put("PaymentDate", String.valueOf(summary.get("payment_date")));
                    success = true;
                 } else {
                    success = true; // C# logic
                    message = CommonMessages.NO_DATA_FOUND;
                 }
            } else {
                success = false; // Message from service indicates failure
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Map<String, List<Map<String, Object>>>> apiResponse = new ApiResponse<>(success, message, responseData);
        if (success && additionalParams != null) {
            apiResponse.setAdditionalParams(additionalParams);
        }
        return apiResponse;
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/GetBatchList", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getBatchList(HttpServletRequest request) {
        boolean initialParseSuccess = true;
        String message = "";
        List<Map<String, Object>> dataTable = null;

        String transactionIdSearch = "";
        Date dtFromDate = null; // Initialize to null, set if DATERANGE
        Date dtToDate = null;
        int hint = 0;
        int statusFilter = -1;

        try {
            String searchBy = request.getParameter("cboSearchBy");
            if ("BATCHNO".equals(searchBy)) {
                transactionIdSearch = request.getParameter("txtBatchNo");
                hint = 1;
                if (transactionIdSearch == null || transactionIdSearch.trim().isEmpty()) {
                    throw new IllegalArgumentException("Batch Number is required for BATCHNO search.");
                }
            } else if ("DATERANGE".equals(searchBy)) {
                hint = 2;
                String fromDateStr = request.getParameter("txtFromDate");
                String toDateStr = request.getParameter("txtToDate");
                String statusStr = request.getParameter("cboStatus");

                if (fromDateStr == null || fromDateStr.isEmpty()) throw new IllegalArgumentException("Invalid txtFromDate");
                dtFromDate = new SimpleDateFormat("yyyy-MM-dd").parse(fromDateStr); // Adjust format
                if (toDateStr == null || toDateStr.isEmpty()) throw new IllegalArgumentException("Invalid txtToDate");
                dtToDate = new SimpleDateFormat("yyyy-MM-dd").parse(toDateStr);
                if (statusStr == null || statusStr.isEmpty()) throw new IllegalArgumentException("Invalid cboStatus");
                statusFilter = Integer.parseInt(statusStr);
            } else {
                throw new IllegalArgumentException("Invalid search criteria.");
            }
        } catch (Exception ex) {
            initialParseSuccess = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialParseSuccess) {
            try {
                // Assuming BatchMgmtService.getTransactionList returns List<Map> and handles its own internal message.
                // The C# code had an 'out Message' from BL, but it was only checked if it was empty.
                // Let's assume the service returns the data list directly, and message is for controller-level status.
                dataTable = batchMgmtService.getTransactionList(
                    PortalApplication.getConnectionString(), hint, transactionIdSearch, 
                    dtFromDate, dtToDate, statusFilter, PortalSession.getUserID(request));
                
                if (dataTable != null && !dataTable.isEmpty()) {
                    initialParseSuccess = true; // Final success
                    // message remains empty
                } else {
                    initialParseSuccess = true; // C# logic: success true, message NO_DATA_FOUND
                    message = CommonMessages.NO_DATA_FOUND;
                    dataTable = new ArrayList<>(); // ensure empty list for JSON
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialParseSuccess = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(initialParseSuccess, message, dataTable);
    }
    
    // Helper for DeleteBatch and DeleteProbURNs
    private ApiResponse<Void> deleteBatchOperation(int hint, HttpServletRequest request, String successMessageTemplate) {
        boolean success = false;
        String message = "";
        try {
            String transactionId = request.getParameter("hdBatchNo");
            if (transactionId == null || transactionId.isEmpty()) {
                return new ApiResponse<>(false, "Batch number is required.");
            }
            String serviceMessage = batchMgmtService.deleteBatches(
                PortalApplication.getConnectionString(), hint, transactionId, PortalSession.getUserID(request));

            if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                success = true;
                message = successMessageTemplate;
            } else {
                success = false; // C# sets success=true but uses the message.
                                // More consistent if non-empty message from service implies an issue/specific status.
                                // For direct translation of C# status: success=true, message=serviceMessage
                message = serviceMessage; 
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), "deleteBatchOperation (hint: " + hint + ")", ex, request.getParameterMap());
            success = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(success, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/DeleteBatch", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> deleteBatch(HttpServletRequest request) {
        return deleteBatchOperation(1, request, "Batch Deleted Successfully");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/DeleteProbURNs", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> deleteProbURNs(HttpServletRequest request) {
        return deleteBatchOperation(2, request, "URNs Removed Successfully From The Batch No");
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/VerifyBatch", produces = MediaType.APPLICATION_JSON_VALUE) @ResponseBody
    public ApiResponse<Void> verifyBatch(HttpServletRequest request) {
        boolean success = false;
        String message = "";
        Map<String, String> additionalParams = null;
        try {
            String transactionId = request.getParameter("hdBatchNo");
            String pgName = request.getParameter("selPG"); // C# comment: ???

            if (transactionId == null || transactionId.isEmpty()) {
                return ApiResponse.error("Batch number is required.");
            }

            BatchMgmtDataAccess.VerifyBatchResult result = batchMgmtService.verifyBatch(
                PortalApplication.getConnectionString(), transactionId);
            
            message = result.getMessage(); // Message from service/DAL
            String paymentMode = result.getPaymentMode();
            boolean canProceed = result.isCanProceed();

            if ("CR".equals(paymentMode)) {
                success = canProceed;
                // message already set
            } else if ("PG".equals(paymentMode)) {
                if (canProceed) {
                    HttpSession session = request.getSession();
                    session.setAttribute("batch_no_pmt", transactionId);
                    session.setAttribute("amt_pmt", result.getTotalAmount()); // Store BigDecimal
                    session.setAttribute("selPG", pgName);

                    additionalParams = new HashMap<>();
                    additionalParams.put("redirect_to", "../Payments/PG2"); // Relative path to another controller action
                    success = true;
                    if(message == null || message.isEmpty()) message = ""; // Clear message if successful
                } else {
                    success = false;
                    // message should contain reason why cannot proceed
                }
            } else { // Unknown payment mode
                success = false;
                if (message == null || message.isEmpty()) message = "Unknown payment mode from batch verification.";
            }
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
    // --- End Batch Management Region ---

    // @AuthorizeAJAX
    @PostMapping(value = "/GetAvailableSeats", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<String> getAvailableSeats(HttpServletRequest request) { // data field will contain HTML string
        boolean success = false;
        String message = "";
        String htmlData = ""; // To hold generated HTML
        boolean initialParseStatus = true;
        
        int stateId = -1;
        int centerId = -1;
        Date fromDate = null;

        try {
            String dummy;
            // Check if keys exist before trying to get parameters
            if (request.getParameterMap().containsKey("cboStates")) {
                dummy = request.getParameter("cboStates");
                stateId = (dummy == null || dummy.trim().isEmpty()) ? -1 : Integer.parseInt(dummy.trim());
            }
            if (request.getParameterMap().containsKey("cboCenter")) {
                dummy = request.getParameter("cboCenter");
                centerId = (dummy == null || dummy.trim().isEmpty()) ? -1 : Integer.parseInt(dummy.trim());
            }
            dummy = request.getParameter("txtFromDate");
            if (dummy == null || dummy.trim().isEmpty()) throw new IllegalArgumentException("FromDate is required.");
            fromDate = new SimpleDateFormat("yyyy-MM-dd").parse(dummy.trim()); // Adjust format

        } catch (Exception ex) {
            initialParseStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialParseStatus) {
            StringBuilder htmlBuilder = new StringBuilder();
            try {
                // Assuming CssSchedulingService.getDashBoardOAIMS returns List<Map>
                List<Map<String, Object>> dataTable = cssSchedulingService.getDashBoardOAIMS(
                    PortalApplication.getOaimsConnectionString(), stateId, centerId, fromDate);

                if (dataTable != null && !dataTable.isEmpty()) {
                    // HTML generation logic from C# translated to Java
                    if (stateId > 0 && centerId == -1) { /* ... HTML for state ... */ 
                        htmlBuilder.append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                        Map<String, Object> firstRow = dataTable.get(0); // Assume consistent columns
                        for (String colName : firstRow.keySet()) {
                            if ("TCM_ID".equalsIgnoreCase(colName)) continue;
                            String headerText = "CENTER_NAME".equalsIgnoreCase(colName) ? "Center Name" : colName.replace("'", "");
                            htmlBuilder.append(String.format("<th>%s</th>", headerText));
                        }
                        htmlBuilder.append("</tr></thead><tbody>");
                        for (Map<String, Object> row : dataTable) {
                            htmlBuilder.append("<tr>");
                            for (String colName : firstRow.keySet()) {
                                if ("TCM_ID".equalsIgnoreCase(colName)) continue;
                                Object val = row.get(colName);
                                if ("CENTER_NAME".equalsIgnoreCase(colName)) {
                                    htmlBuilder.append(String.format("<td>%s</td>", val != null ? val : ""));
                                } else {
                                    int seatCount = 0;
                                    if (val != null) { try { seatCount = Integer.parseInt(String.valueOf(val));} catch (NumberFormatException e) {/* ignore */} }
                                    if (seatCount == 0) htmlBuilder.append(String.format("<td>%d</td>", seatCount));
                                    else {
                                        String func = String.format("javascript:d(%s,'%s');", row.get("TCM_ID"), colName); // Note: "javascript:"
                                        htmlBuilder.append(String.format("<td><a href=\"%s\">%d</a></td>", func, seatCount));
                                    }
                                }
                            }
                            htmlBuilder.append("</tr>");
                        }
                        htmlBuilder.append("</tbody></table>");
                    } else if (stateId == -1 && centerId > 0) { /* ... HTML for center ... */
                        Map<String, Object> firstRow = dataTable.get(0);
                        htmlBuilder.append("<table class='table table-striped table-bordered table-hover'><thead>");
                        htmlBuilder.append(String.format("<tr><th colspan='2' align='center'>%s</th></tr>", firstRow.get("CENTER_NAME")));
                        htmlBuilder.append(String.format("<tr><th colspan='2' align='left'>%s</th></tr>", 
                            new SimpleDateFormat("dd-MMM-yyyy").format((Date)firstRow.get("SCH_TST_DT")))); // Cast to Date
                        htmlBuilder.append("<tr><th>Slot</th><th>Available Seats</th></tr></thead><tbody>");
                        for (Map<String, Object> row : dataTable) {
                            htmlBuilder.append("<tr>");
                            htmlBuilder.append(String.format("<td>%s</td><td>%s</td>", row.get("SCH_SLOT"), row.get("SCH_AVL_SEATS")));
                            htmlBuilder.append("</tr>");
                        }
                        htmlBuilder.append("</tbody></table>");
                    } else { // No specific HTML structure in C# for other combinations
                         message = CommonMessages.NO_DATA_FOUND + " (Unsupported display combination or data for it)";
                    }
                    htmlData = htmlBuilder.toString();
                    if (!htmlData.isEmpty()) {
                        success = true;
                    } else { // HTML remained empty
                        success = false; // C# set Success=false here
                        if (message.isEmpty()) message = CommonMessages.NO_DATA_FOUND;
                    }
                } else {
                    success = false; // C# set Success=false
                    message = CommonMessages.NO_DATA_FOUND;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                success = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        // C# ToJSON4 implies data is the HTML string
        return new ApiResponse<>(success, message, htmlData);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetCompanyPaymentModes", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> getCompanyPaymentModes(HttpServletRequest request) {
        boolean initialStatus = true;
        String message = "";
        Map<String, String> kvPair = null;
        String companyType = "", loginId = "";

        try {
            companyType = request.getParameter("cboCompanyType");
            if (companyType == null || !Arrays.asList("IA", "CA", "WA", "IMF", "BR").contains(companyType.toUpperCase())) {
                throw new IllegalArgumentException("Invalid cboCompanyType value: " + companyType);
            }
            loginId = request.getParameter("txtCompanyLoginId");
            if (loginId == null || loginId.trim().isEmpty()) {
                throw new IllegalArgumentException("Invalid txtCompanyLoginId: " + loginId);
            }
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                // UserService.getCompanyPaymentModes returns UserDataAccess.CompanyPaymentModes (nested DTO)
                UserDataAccess.CompanyPaymentModes result = userService.getCompanyPaymentModes(
                    PortalApplication.getConnectionString(), companyType, loginId.trim());
                
                message = result.getMessage(); // Message from service/DAL
                if (message == null || message.trim().isEmpty()) {
                    initialStatus = true; // Final success
                    kvPair = new HashMap<>();
                    kvPair.put("CNAME", result.getCompanyName());
                    kvPair.put("CMODE", result.isCreditMode() ? "Y" : "N");
                    kvPair.put("OMODE", result.isOnlineMode() ? "Y" : "N");
                } else {
                    initialStatus = false; // Error message from service/DAL
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                initialStatus = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        ApiResponse<Void> response = new ApiResponse<>(initialStatus, message);
        if (kvPair != null) {
            response.setAdditionalParams(kvPair);
        }
        return response;
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveCompanyPaymentModes", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> saveCompanyPaymentModes(HttpServletRequest request) {
        boolean initialStatus = true;
        String message = "";
        String companyType = "", loginId = "";
        boolean creditMode = false, onlineMode = false;

        try {
            companyType = request.getParameter("hddCompanyType");
            if (companyType == null || !Arrays.asList("IA", "CA", "WA", "IMF", "BR").contains(companyType.toUpperCase())) {
                throw new IllegalArgumentException("Invalid hddCompanyType value: " + companyType);
            }
            loginId = request.getParameter("hddCompanyLoginId");
            if (loginId == null || loginId.trim().isEmpty()) {
                throw new IllegalArgumentException("Invalid hddCompanyLoginId: " + loginId);
            }
            String chkCredit = request.getParameter("chkCreditBalance");
            creditMode = "on".equalsIgnoreCase(chkCredit) || "true".equalsIgnoreCase(chkCredit);
            String chkOnline = request.getParameter("chkOnlinePayment");
            onlineMode = "on".equalsIgnoreCase(chkOnline) || "true".equalsIgnoreCase(chkOnline);
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + ": " + ex.getMessage();
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                String serviceMessage = userService.saveCompanyPaymentModes(
                    PortalApplication.getConnectionString(), companyType, loginId.trim(), creditMode, onlineMode);
                
                if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                    initialStatus = true; // Final success
                    message = CommonMessages.DATA_SAVE_SUCCESS;
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
        return new ApiResponse<>(initialStatus, message);
    }
}