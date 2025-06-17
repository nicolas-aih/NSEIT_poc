package com.example.controllers;

import com.example.config.PortalApplication; // For ConnectionString
import com.example.dto.ApiResponse;
import com.example.interfaces.UtilityDataAccess; // For nested DTOs
import com.example.services.UtilityService;
import com.example.util.CommonMessages;
import com.example.util.ErrorLogger;
import com.example.util.LoginEncryptor; // For Decrypt
import com.example.util.PortalSession;   // For UserID

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.MediaType;


import javax.servlet.http.HttpServletRequest;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

// C# attributes:
// [AuthorizeExt]
// [AuthorizeAJAX]
// These would be handled by Spring Security or custom interceptors in Java.

@Controller
@RequestMapping("/Utility") // Assuming base path for this controller
public class UtilityController {

    private final UtilityService utilityService;

    @Autowired
    public UtilityController(UtilityService utilityService) {
        this.utilityService = utilityService;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName(); // Or getName() for fully qualified
    }

    private String getCurrentMethodName() {
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }

    // @AuthorizeExt
    @GetMapping("/UpdateCandidateProfile")
    public String updateCandidateProfileView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "UpdateCandidateProfile"; // View name
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveCandidateProfile", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveCandidateProfile(HttpServletRequest request) {
        boolean success = false;
        String message = ""; // Holds validation messages or final status message
        Exception dbException = null; // Per original C#

        try {
            String updateAction = request.getParameter("cboUpdateAction");
            if (updateAction == null || updateAction.isEmpty()) {
                message = "Invalid Update Action;";
            }

            String urn = request.getParameter("txtURN");
            // Original C# logic: if (UpdateAction == String.Empty) - likely a typo, should be URN
            if (urn == null || urn.isEmpty()) { // Corrected logic based on common sense
                if (!message.isEmpty()) message += " ";
                message += "Invalid URN;";
            }

            String updateValue = request.getParameter("txtValue");
            String language = request.getParameter("cboLanguage");

            if (updateAction != null && !"Update_Lang".equals(updateAction) && (updateValue == null || updateValue.isEmpty())) {
                if (!message.isEmpty()) message += " ";
                message += "Invalid Update Value;";
            } else if (updateAction != null && "Update_Lang".equals(updateAction) && (language == null || language.isEmpty())) {
                if (!message.isEmpty()) message += " ";
                message += "Invalid Language Value;";
            }

            if (updateAction != null && "Update_Lang".equals(updateAction)) {
                updateValue = language; // Use language as the value to update
            }

            if (message.isEmpty()) { // If initial validations pass
                String serviceMessage = utilityService.saveCandidateProfile(
                        PortalApplication.getConnectionString(),
                        updateValue,
                        urn,
                        PortalSession.getUserID(request),
                        updateAction);

                if (dbException != null) { // This will likely never be true as dbException is not set
                    ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), dbException, request.getParameterMap());
                }

                if (serviceMessage == null || serviceMessage.isEmpty()) {
                    success = true;
                    message = CommonMessages.DATA_SAVE_SUCCESS;
                } else {
                    success = false; // Assuming non-empty message from service indicates failure
                    message = CommonMessages.DATA_SAVE_FAIL + ". " + serviceMessage;
                }
                return new ApiResponse<>(success, message, null);
            } else {
                // Validation messages collected, return error
                return ApiResponse.error(message.trim());
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            return ApiResponse.error(CommonMessages.ERROR_OCCURED);
        }
        // 'finally { objUtility = null; }' is handled by Java GC.
    }

    // @AuthorizeExt
    @GetMapping("/CompanyLookup")
    public String companyLookupView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "CompanyLookup"; // View name
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/LoadCompanyDetails", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> loadCompanyDetails(HttpServletRequest request) {
        boolean success = false;
        String message = "";
        List<Map<String, Object>> dataTable = null;
        Exception dbException = null; // Per original C#

        try {
            String companyName = request.getParameter("txtCompanyName");
            if (companyName == null || companyName.isEmpty()) {
                message = "Invalid Input;";
            }

            if (message.isEmpty()) {
                UtilityDataAccess.CompanyDetails result = utilityService.getCompanyDetails(
                        PortalApplication.getConnectionString(), companyName);
                
                // The DTO contains the message from the data access layer.
                // If result.getMessage() is not empty, it might indicate an error from DAL.
                // However, C# code set Message only on exception, otherwise used the out param from service.
                // Let's assume an empty result.getMessage() means success at DAL level.
                
                message = result.getMessage(); // Use message from DTO if DAL set it
                
                if (message == null || message.isEmpty()) { // If DAL didn't report an error message
                    dataTable = result.getData();
                    if (dataTable != null && !dataTable.isEmpty()) {
                        success = true;
                        // Message remains empty for success with data
                    } else {
                        success = true; // C# logic: success true even for NO_DATA_FOUND
                        message = CommonMessages.NO_DATA_FOUND;
                        dataTable = new ArrayList<>(); // Ensure data is an empty list not null
                    }
                } else {
                    success = false; // DAL reported an issue
                    // message already contains the issue
                }


                if (dbException != null) { // This will likely never be true
                    ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), dbException, request.getParameterMap());
                }
                return new ApiResponse<>(success, message, dataTable);
            } else {
                return ApiResponse.error(message, null);
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            return ApiResponse.error(CommonMessages.ERROR_OCCURED, null);
        }
    }

    // @AuthorizeExt
    @GetMapping("/GetUserPassword") // This serves the view
    public String getUserPasswordView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "GetUserPassword"; // View name
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetUserPassword", produces = MediaType.APPLICATION_JSON_VALUE) // This handles the POST action
    @ResponseBody
    public ApiResponse<Void> getUserPasswordAction(@RequestParam(value = "Dummy", required = false) String dummy,
                                                    HttpServletRequest request) {
        boolean success = false;
        String message = "";
        Map<String, String> additionalParams = null;
        Exception dbException = null; // Per original C#

        try {
            String userLoginId = request.getParameter("txtUserLoginId");
            if (userLoginId == null || userLoginId.isEmpty()) {
                message = "Invalid Login ID;";
            }

            if (message.isEmpty()) {
                UtilityDataAccess.UserPassword result = utilityService.getUserPassword(
                        PortalApplication.getConnectionString(), userLoginId);

                message = result.getMessage(); // Message from service/DAL
                String password = result.getPassword();

                if (dbException != null) { // This will likely never be true
                    ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), dbException, request.getParameterMap());
                }

                if (message == null || message.isEmpty()) { // Success if service message is empty
                    success = true;
                    String decryptedPassword = LoginEncryptor.decrypt(password);
                    additionalParams = new HashMap<>();
                    additionalParams.put("Password", decryptedPassword);
                } else {
                    success = false;
                    // message already contains the error reason from service/DAL
                }
                return ApiResponse.successWithParams(message, null, additionalParams); // Success true/false based on outcome
            } else {
                return ApiResponse.error(message.trim());
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            return ApiResponse.error(CommonMessages.ERROR_OCCURED);
        }
    }
}