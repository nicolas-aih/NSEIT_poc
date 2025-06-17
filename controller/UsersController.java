package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse;
import com.example.interfaces.MasterDataDataAccess; // For GetInsurers etc. if MasterData is separate
import com.example.interfaces.UserDataAccess; // For nested DTOs like LoginResult
import com.example.services.MasterDataService; // If MasterData is separate
import com.example.services.UserService;
import com.example.util.*; // CommonMessages, ErrorLogger, PortalSession, LoginEncryptor, etc.

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;
import java.util.*;
import java.util.stream.Collectors;

// C# Attributes:
// [AuthorizeExt]
// [AuthorizeAJAX]
// These would be handled by Spring Security or custom interceptors in Java.

@Controller
@RequestMapping("/Users")
public class UsersController {

    private final UserService userService;
    private final MasterDataService masterDataService; // Inject if IIIBL.MasterData is separate

    @Autowired
    public UsersController(UserService userService, MasterDataService masterDataService) {
        this.userService = userService;
        this.masterDataService = masterDataService;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName();
    }

    private String getCurrentMethodName() {
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }

    @GetMapping("/Logout")
    public String logout(HttpServletRequest request, HttpServletResponse response) {
        // C# Commented out IIIBL.Users.Logout call:
        /*
        try {
            userService.logout(PortalApplication.getConnectionString(), PortalSession.getUserID(request));
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
        }
        */

        // Equivalent to FormsAuthentication.SignOut()
        // Remove custom authentication cookie if you set one during login
        Cookie authCookie = new Cookie("IIIRPAuth", null); // Use the same name as set during login
        authCookie.setPath(request.getContextPath() + "/"); // Important to set the correct path
        authCookie.setMaxAge(0); // Expires immediately
        authCookie.setHttpOnly(true); // Good practice
        response.addCookie(authCookie);

        // Equivalent to Session.Abandon()
        PortalSession.abandonSession(request);

        return "redirect:/Home/Index"; // Spring MVC redirect
    }

    @PostMapping(value = "/Login", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> login(@RequestParam("UserId") String userId,
                                @RequestParam("Password") String password,
                                HttpServletRequest request,
                                HttpServletResponse response) {
        StringBuilder messageBuilder = new StringBuilder();
        if (userId == null || userId.trim().isEmpty()) {
            messageBuilder.append("Please enter user id; ");
        }
        if (password == null || password.trim().isEmpty()) {
            messageBuilder.append("Please enter password; ");
        }

        if (messageBuilder.length() > 0) {
            return ApiResponse.error(messageBuilder.toString().trim());
        }

        boolean status = false; // C# Status variable
        String finalMessage = "";    // C# Message variable

        try {
            String encryptedPassword = LoginEncryptor.encrypt(password);
            // The service method 'login' returns UserDataAccess.LoginResult (nested DTO)
            UserDataAccess.LoginResult loginServiceResult = userService.login(
                    PortalApplication.getConnectionString(), userId, encryptedPassword);

            finalMessage = loginServiceResult.getResultMessage(); // Message from DB/Service layer

            switch (finalMessage != null ? finalMessage.toUpperCase() : "EXCEPTION_FALLBACK") {
                case "SUCCESS1":
                case "SUCCESS":
                    List<Map<String, Object>> dataSetTables = loginServiceResult.getDataSet();
                    if (dataSetTables != null && !dataSetTables.isEmpty()) { // Assuming first table contains user data
                        Map<String, Object> dr = dataSetTables.get(0); // Get first row

                        String clientMachineIP1 = PortalSession.getClientMachineIP(request);
                        PortalSession.clearSession(request); // Clears current session attributes
                        PortalSession.setClientMachineIP(request, clientMachineIP1); // Restore IP

                        PortalSession.setUserID(request, DataConverter.getInt(dr, "intUserID"));
                        PortalSession.setUserName(request, DataConverter.getString(dr, "varUserName"));
                        PortalSession.setRoleID(request, DataConverter.getShort(dr, "sntRoleID"));
                        PortalSession.setRoleName(request, DataConverter.getString(dr, "varRoleName"));
                        PortalSession.setInsurerUserID(request, DataConverter.getInt(dr, "intTblMstInsurerUserID"));
                        PortalSession.setDPUserID(request, DataConverter.getInt(dr, "intTblMstDPUserID"));
                        PortalSession.setAgentCounselorUserID(request, DataConverter.getInt(dr, "intTblMstAgntCounselorUserID"));
                        PortalSession.setLastLoggedInDateTime(request, DataConverter.getDate(dr, "dtLastLoggedIn"));
                        PortalSession.setInsurerName(request, DataConverter.getString(dr, "varInsurerName"));
                        PortalSession.setDPName(request, DataConverter.getString(dr, "varDPName"));
                        PortalSession.setAgentCounselorName(request, DataConverter.getString(dr, "varCounselorName"));
                        PortalSession.setUserLoginID(request, userId);
                        PortalSession.setInsurerType(request, DataConverter.getString(dr, "InsurerType"));
                        PortalSession.setUserEmailID(request, DataConverter.getString(dr, "varEmailID"));
                        PortalSession.setInsurerCode(request, DataConverter.getString(dr, "varInsurerID"));
                        PortalSession.setInsurerTypeNew(request, DataConverter.getString(dr, "InsurerTypeNew"));
                        PortalSession.setCAid(request, DataConverter.getChar(dr, "CAid"));
                        PortalSession.setRoleCode(request, DataConverter.getString(dr, "role_code"));
                        // PortalSession.setInsurerTypeNew(request, DataConverter.getString(dr, "InsurerTypeNew")); // Duplicate in C#
                        PortalSession.setTopUserLoginID(request, DataConverter.getString(dr, "TopLevelUserLoginId"));

                        PortalSession.setKey(request, AadhaarEncryptorDecryptor.GenerateKey(AadhaarEncryptorDecryptor.EncryptionAlgorithm.TripleDES));
                        PortalSession.setIV(request, AadhaarEncryptorDecryptor.GenerateIV(AadhaarEncryptorDecryptor.EncryptionAlgorithm.TripleDES));

                        status = true;
                        finalMessage = "Success"; // Overwrite with simple success for client
                    } else {
                        status = false;
                        finalMessage = "User data not found after login.";
                    }
                    break;
                case "INVALID_ID": finalMessage = "User id or Password is invalid."; break;
                case "INVALID_PWD": finalMessage = "User id or Password is invalid."; break;
                case "INACTIVE": finalMessage = "User is not active."; break;
                case "EXCEPTION": finalMessage = "Could not login."; break;
                case "LOGGEDIN": finalMessage = "User is already logged in."; break;
                case "ROLE_NOT_MAPPED": finalMessage = "Role not assigned to user."; break;
                case "LOGIN_ATTEMPT_EXCEEDED": finalMessage = "User has been suspended."; break;
                case "SUSPENDED": finalMessage = "User has been suspended."; break;
                case "CHANGE_PWD": finalMessage = "Change password before login."; break;
                case "NO_DATA": finalMessage = "Temporarily Suspended: Agency Data not Submitted"; break;
                default: finalMessage = "Unknown error during login (" + finalMessage + ")."; break;
            }

            if (status) {
                // Set Auth Cookie (Example of manual cookie, Spring Security handles this differently)
                String cookieString = PortalSession.getUserID(request) + "|" + PortalSession.getUserName(request);
                Cookie authCookie = new Cookie("IIIRPAuth", cookieString); // Example cookie name
                authCookie.setPath(request.getContextPath() + "/");
                authCookie.setHttpOnly(true);
                // authCookie.setSecure(request.isSecure()); // For HTTPS only
                // authCookie.setMaxAge(...); // Set expiry
                response.addCookie(authCookie);

                // Fetch Menu
                try {
                    List<Map<String, Object>> menuDataSet = userService.menuPermissions(
                            PortalApplication.getConnectionString(), (long)PortalSession.getUserID(request));
                    
                    if (menuDataSet != null && !menuDataSet.isEmpty()) {
                        // Filter for MenuAccess = 1 (assuming "MenuAccess" is a key and value is 1 or "1" or true)
                        List<Map<String, Object>> accessibleMenuItems = menuDataSet.stream()
                            .filter(item -> "1".equals(String.valueOf(item.get("MenuAccess"))) || Boolean.TRUE.equals(item.get("MenuAccess")))
                            .collect(Collectors.toList());

                        PortalSession.setMenuAccess(request, accessibleMenuItems);

                        // Get distinct ParentMenuName
                        // C#: dataTable.DefaultView.ToTable(true, new[] { "ParentMenuName" });
                        List<Map<String, Object>> parentMenu = accessibleMenuItems.stream()
                            .map(item -> {
                                Map<String, Object> parentMap = new HashMap<>();
                                parentMap.put("ParentMenuName", item.get("ParentMenuName"));
                                return parentMap;
                            })
                            .distinct() // Handles distinct based on map content
                            .collect(Collectors.toList());
                        PortalSession.setParentMenu(request, parentMenu);
                        PortalSession.setChildMenu(request, accessibleMenuItems); // ChildMenu in C# was the filtered table
                    } else {
                        PortalSession.setParentMenu(request, new ArrayList<>());
                        PortalSession.setChildMenu(request, new ArrayList<>());
                    }
                } catch (Exception exMenu) {
                    ErrorLogger.logError(getCurrentClassName(), "Login.FetchMenu", exMenu, request.getParameterMap());
                    PortalSession.setParentMenu(request, new ArrayList<>());
                    PortalSession.setChildMenu(request, new ArrayList<>());
                    // Optionally update status/message if menu fetch is critical
                }
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            finalMessage = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, finalMessage);
    }

    @PostMapping(value = "/ChangePassword", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> changePassword(@RequestParam("UserId") String userId,
                                         @RequestParam("Password") String password,
                                         @RequestParam("NewPassword") String newPassword,
                                         @RequestParam("ConfirmPassword") String confirmPassword,
                                         HttpServletRequest request) {
        boolean status = false;
        String message = "";
        StringBuilder validationMessages = new StringBuilder();

        if (userId == null || userId.trim().isEmpty()) validationMessages.append("Please enter user id; ");
        if (password == null || password.trim().isEmpty()) validationMessages.append("Please enter old password; ");
        if (newPassword == null || newPassword.trim().isEmpty()) validationMessages.append("Please enter new password; ");
        if (newPassword != null && !newPassword.trim().equals(confirmPassword != null ? confirmPassword.trim() : "")) {
            validationMessages.append("Password and confirm password do not match; ");
        }

        if (validationMessages.length() == 0) {
            try {
                String encryptedPassword = LoginEncryptor.encrypt(password);
                String encryptedNewPassword = LoginEncryptor.encrypt(newPassword);
                String serviceMessage = userService.changePassword(PortalApplication.getConnectionString(), userId, encryptedPassword, encryptedNewPassword);

                if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                    status = true;
                    message = "Password changed successfully";
                } else {
                    status = false;
                    message = serviceMessage; // Use message from service directly
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                status = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        } else {
            status = false;
            message = validationMessages.toString().trim();
        }
        return new ApiResponse<>(status, message);
    }

    @PostMapping(value = "/ResetPassword", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> resetPassword(@RequestParam("UserId") String userId,
                                        @RequestParam("EmailId") String emailId,
                                        HttpServletRequest request) {
        boolean status = false;
        String message = "";
        StringBuilder validationMessages = new StringBuilder();

        if (userId == null || userId.trim().isEmpty()) validationMessages.append("Please enter user id; ");
        if (emailId == null || emailId.trim().isEmpty()) validationMessages.append("Please enter email id associated with this user; ");

        if (validationMessages.length() == 0) {
            try {
                String[] passwordParts = UUID.randomUUID().toString().split("-");
                String newPassword = passwordParts[0];
                String newTxnPassword = passwordParts[3]; // As per C# logic

                String encryptedPassword = LoginEncryptor.encrypt(newPassword);
                String encryptedTxnPassword = LoginEncryptor.encrypt(newTxnPassword);
                
                String serviceMessage = userService.resetPassword(PortalApplication.getConnectionString(), userId, emailId, encryptedPassword, encryptedTxnPassword);

                if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                    String subject = "Password Resolved";
                    String emailBody = "Dear User,<br><br>Your password has been reset as below:<br/><br/>"
                                     + "Login ID : " + userId + "<br/><br/>"
                                     + "Login Password : " + newPassword + "<br/><br/>"
                                     + "Kindly change password on next login.<br/><br/><br/>"
                                     + "with regards<br/><br/>IIIExams.org";
                    try {
                        HelperUtilities.sendMail(subject, emailId, "", "", emailBody, true);
                        status = true;
                        message = "Password has been reset successfully and e-mail sent to the registered e-mail id";
                    } catch (Exception mailEx) {
                        ErrorLogger.logError(getCurrentClassName(), "ResetPassword.SendMail", mailEx, request.getParameterMap());
                        status = true; // Password reset was still successful
                        message = "Password has been reset successfully, but error occurred while sending e-mail. Kindly contact support team.";
                    }
                } else {
                    status = false;
                    message = serviceMessage; // Use message from service
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                status = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        } else {
            status = false;
            message = validationMessages.toString().trim();
        }
        return new ApiResponse<>(status, message);
    }
    
    // @AuthorizeExt
    @GetMapping("/MyProfile")
    public String myProfileView(Model model, HttpServletRequest request) {
        String userType = "";
        String viewName = ""; // Will be "MyProfile" or "MyProfile2" or an error/default view
        String roleName = PortalSession.getRoleName(request);

        if (roleName != null) {
            switch (roleName) {
                case "Corporate Agent User": viewName = "MyProfile"; userType = "CA"; break;
                case "Web Aggregator": viewName = "MyProfile"; userType = "WA"; break;
                case "Insurance Marketing Firm": viewName = "MyProfile"; userType = "IMF"; break;
                case "Insurance Broker": viewName = "MyProfile"; userType = "BR"; break;
                case "Corporate Designated Person": viewName = "MyProfile2"; userType = "CDP"; break;
                case "Designated Person": viewName = "MyProfile2"; userType = "DP"; break;
                case "Agent Counselor": viewName = "MyProfile2"; userType = "AC"; break;
                default: userType = ""; viewName = "ErrorView"; break; // Or a default profile view
            }
        } else {
             viewName = "ErrorView"; // Or redirect to login
        }
        model.addAttribute("UserType", userType);
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return viewName.isEmpty() ? "DefaultProfileView" : viewName;
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GetProfile", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<List<Map<String, Object>>> getProfile(@RequestParam(value = "Dummy", required = false) String dummy,
                                                              HttpServletRequest request) {
        String userType = "";
        String roleName = PortalSession.getRoleName(request);
         if (roleName != null) {
            switch (roleName) {
                case "Corporate Agent User": userType = "CA"; break;
                case "Web Aggregator": userType = "WA"; break;
                case "Insurance Marketing Firm": userType = "IMF"; break;
                case "Insurance Broker": userType = "BR"; break;
                case "Corporate Designated Person": userType = "CDP"; break;
                case "Designated Person": userType = "DP"; break;
                case "Agent Counselor": userType = "AC"; break;
                default: userType = ""; break;
            }
        }
        if (userType.isEmpty()) {
            return ApiResponse.error("User role not recognized for profile.", null);
        }

        boolean status = false;
        String message = "";
        List<Map<String, Object>> dataTable = null;
        try {
            dataTable = userService.getUserDetails(PortalApplication.getConnectionString(), PortalSession.getUserID(request), userType);
            if (dataTable != null && !dataTable.isEmpty()) {
                status = true;
            } else {
                status = false; // C# logic: status false if no data
                message = CommonMessages.NO_DATA_FOUND;
                dataTable = new ArrayList<>(); // Ensure data is empty list, not null
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message, dataTable);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveProfile", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveProfile(HttpServletRequest request) {
        // Parameters from C#: Address1, POName, EMailId, MobileNo, STDCode, PhoneNo
        String address1 = request.getParameter("Address1");
        String poName = request.getParameter("POName");
        String emailId = request.getParameter("EMailId");
        String mobileNo = request.getParameter("MobileNo");
        String stdCode = request.getParameter("STDCode");
        String phoneNo = request.getParameter("PhoneNo");

        String userType = "";
        String roleName = PortalSession.getRoleName(request);
        if (roleName != null) {
             switch (roleName) {
                case "Corporate Agent User": userType = "CA"; break;
                case "Web Aggregator": userType = "WA"; break;
                case "Insurance Marketing Firm": userType = "IMF"; break;
                case "Insurance Broker": userType = "BR"; break;
                // These roles use this endpoint in C#
                case "Corporate Designated Person": userType = "CDP"; break;
                case "Designated Person": userType = "DP"; break;
                case "Agent Counselor": userType = "AC"; break;
                default: userType = ""; break;
            }
        }
        if (userType.isEmpty()) {
            return ApiResponse.error("User role not recognized for saving profile.");
        }

        boolean status = false;
        String message = "";
        try {
            // C# call: (..., Address1, "", "", -1, "", "", "", POName, EMailId, MobileNo, STDCode, PhoneNo, -1)
            String serviceMessage = userService.saveUserProfile(PortalApplication.getConnectionString(), userType,
                    PortalSession.getUserID(request), address1, "", "", -1, 
                    "", "", "", poName, emailId, mobileNo, stdCode, phoneNo, -1);

            if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                status = true;
                message = CommonMessages.DATA_SAVE_SUCCESS;
            } else {
                status = false; // Assuming non-empty message from service is an error/failure detail
                message = CommonMessages.DATA_SAVE_FAIL + ". " + serviceMessage;
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/UpdateProfile2", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> updateProfile2(HttpServletRequest request) {
        String userType = "";
        String roleName = PortalSession.getRoleName(request);
        if (roleName != null) {
            switch (roleName) { // User types from C# logic for this endpoint
                case "Corporate Agent User": userType = "CA"; break;
                case "Web Aggregator": userType = "WA"; break;
                case "Insurance Marketing Firm": userType = "IMF"; break;
                case "Insurance Broker": userType = "BR"; break;
                case "Corporate Designated Person": userType = "CDP"; break;
                case "Designated Person": userType = "DP"; break;
                case "Agent Counselor": userType = "AC"; break;
                default: userType = ""; break;
            }
        }
         if (userType.isEmpty()) {
            return ApiResponse.error("User role not recognized for updating profile (UpdateProfile2).");
        }

        boolean status = false;
        String message = "";
        try {
            String address1 = request.getParameter("Address1");
            String address2 = request.getParameter("Address2");
            String address3 = request.getParameter("Address3");
            int pincode = Integer.parseInt(request.getParameter("Pincode"));
            String telephoneOffice = request.getParameter("TelephoneOffice");
            String telephoneResidence = request.getParameter("TelephoneResidence");
            String fax = request.getParameter("Fax");
            String emailId = request.getParameter("EMailId");
            int districtId = Integer.parseInt(request.getParameter("DistrictId"));
            String mobile = request.getParameter("Mobile");

            // C# call: (..., Address1, Address2, Address3, Pincode, TelephoneOffice, TelephoneResidence, Fax, "", EMailId, Mobile, "", "", DistrictId)
            String serviceMessage = userService.saveUserProfile(PortalApplication.getConnectionString(), userType,
                    PortalSession.getUserID(request), address1, address2, address3, pincode,
                    telephoneOffice, telephoneResidence, fax, "", emailId, mobile, "", "", districtId);

            if (serviceMessage == null || serviceMessage.trim().isEmpty()) {
                status = true;
                message = CommonMessages.DATA_SAVE_SUCCESS;
            } else {
                status = false;
                message = CommonMessages.DATA_SAVE_FAIL + ". " + serviceMessage;
            }
        } catch (NumberFormatException nfe) {
             ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
            status = false;
            message = CommonMessages.INVALID_INPUT + " (Numeric field error).";
        }
        catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(status, message);
    }
    
    // @AuthorizeExt
    @GetMapping("/ViewDP")
    public String viewDPView(Model model, HttpServletRequest request) {
        List<Map<String, Object>> cdpData = new ArrayList<>(); // Default to empty
        try {
            String roleCode = PortalSession.getRoleCode(request);
            String roleName = PortalSession.getRoleName(request);

            if ("I".equals(roleCode) || "superadmin".equals(roleCode)) {
                int insurerUserIdForQuery = -1; // Default to all
                if ("Corporate Designated Person".equals(roleName) || "Designated Person".equals(roleName)) {
                    insurerUserIdForQuery = PortalSession.getInsurerUserID(request);
                }
                // Assuming masterDataService.getInsurers returns List<Map> directly (was DataSet.Tables[0])
                cdpData = masterDataService.getInsurers(PortalApplication.getConnectionString(), insurerUserIdForQuery);
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            // cdpData remains empty
        }
        model.addAttribute("CDP", cdpData == null ? new ArrayList<>() : cdpData);
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ViewDP"; // View name
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveDP", consumes = MediaType.MULTIPART_FORM_DATA_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveDP(HttpServletRequest request,
                                 @RequestParam(value = "txtFileSign", required = false) MultipartFile signFile) {
        boolean status = false;
        String message = "";
        boolean saveSuccessful = false;
        boolean mailSuccessful = false;
        int dpUserIdFromForm = 0; // To check if it's an add operation for mail logic

        try {
            int insurerUserId = Integer.parseInt(request.getParameter("hdnInsurerUserId"));
            dpUserIdFromForm = Integer.parseInt(request.getParameter("hdnDPUserId"));
            String name = request.getParameter("txtName");
            String address = request.getParameter("txtAddress");
            String street = request.getParameter("txtStreet");
            String town = request.getParameter("txtTown");
            String pincode = request.getParameter("txtPincode");
            String dpId = request.getParameter("txtDPId");
            String telephoneO = request.getParameter("txtTelephoneO");
            String telephoneR = request.getParameter("txtTelephoneR");
            String mobile = request.getParameter("txtMobileNo");
            String fax = request.getParameter("txtFax");
            String emailId = request.getParameter("txtEmailID");
            int districtId = Integer.parseInt(request.getParameter("cboDistricts"));
            String chkActiveParam = request.getParameter("chkActive");
            boolean isActive = "on".equalsIgnoreCase(chkActiveParam) || "true".equalsIgnoreCase(chkActiveParam);

            byte[] signBytes = null;
            if (signFile != null && !signFile.isEmpty()) {
                signBytes = signFile.getBytes();
            }

            boolean isAddition = (dpUserIdFromForm == 0);
            int createdBy;
            boolean changePasswordOnNextLogin;
            String plainPassword = ""; // For email
            String encPassword, encTxnPassword;
            int lastModifiedBy;

            if (isAddition) {
                createdBy = PortalSession.getUserID(request);
                changePasswordOnNextLogin = true;
                plainPassword = UUID.randomUUID().toString().split("-")[0];
                String plainTxnPassword = UUID.randomUUID().toString().split("-")[0]; // As per C#
                encPassword = LoginEncryptor.encrypt(plainPassword);
                encTxnPassword = LoginEncryptor.encrypt(plainTxnPassword);
                lastModifiedBy = -1;
            } else {
                createdBy = -1;
                changePasswordOnNextLogin = false;
                // C# code generates new passwords even on update in this specific method.
                plainPassword = UUID.randomUUID().toString().split("-")[0];
                String plainTxnPassword = UUID.randomUUID().toString().split("-")[0];
                encPassword = LoginEncryptor.encrypt(plainPassword);
                encTxnPassword = LoginEncryptor.encrypt(plainTxnPassword);
                lastModifiedBy = PortalSession.getUserID(request);
            }
            
            // Call service
            // int SaveDPDetails(..., int incorrectLoginAttempts, boolean isSystemDefined, int lastModifiedBy, int flag, byte[] dpSignature)
            // C# call had false for isSystemDefined, 0 for flag
            int iSuccess = userService.saveDPDetails(PortalApplication.getConnectionString(), dpUserIdFromForm, dpId, insurerUserId,
                    name, address, street, town, districtId, pincode, telephoneO, telephoneR, fax, emailId,
                    createdBy, encPassword, encTxnPassword, mobile, isActive, changePasswordOnNextLogin,
                    0, false, lastModifiedBy, 0, signBytes);

            if (iSuccess == -100 || iSuccess == -300) { // -300 from C# comments
                status = false;
                message = CommonMessages.DATA_SAVE_FAIL + " : EmailID already Exists.";
            } else if (iSuccess == 100) { // New DP Added & Saved
                saveSuccessful = true;
                String subject = "System Mail - Login Information";
                String body = String.format("<B>Dear %s,</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> %s<br><b>Password:</b> %s<br><br>Thanks and regards", name, dpId, plainPassword);
                try {
                    HelperUtilities.sendMail(subject, emailId, "", "", body, true);
                    mailSuccessful = true;
                    status = true;
                    message = CommonMessages.DATA_SAVE_SUCCESS + ". Password has been sent to designated person on his Email id";
                } catch (Exception mailEx) {
                    ErrorLogger.logError(getCurrentClassName(), "SaveDP.SendMail", mailEx, request.getParameterMap());
                    status = true; // Save was successful
                    message = CommonMessages.DATA_SAVE_SUCCESS + ". However, an error occurred while sending mail to the designated person.";
                }
            } else if (iSuccess == 200) { // DP Updated
                saveSuccessful = true;
                status = true;
                message = CommonMessages.DATA_SAVE_SUCCESS;
            } else if (iSuccess == -200) { // Other DB error from procedure
                status = false;
                message = CommonMessages.ERROR_OCCURED + " (Code: " + iSuccess + ")";
            } else {
                status = false;
                message = CommonMessages.DATA_SAVE_FAIL + " (Unknown code: " + iSuccess + ")";
            }

        } catch (IOException ioEx) { // For signFile.getBytes()
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (FileRead)", ioEx, request.getParameterMap());
            status = false;
            message = "Error processing signature file. " + CommonMessages.ERROR_OCCURED;
        } catch (NumberFormatException nfe) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
            status = false;
            message = CommonMessages.INVALID_INPUT + " (Numeric field error)";
        }
        catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            if (dpUserIdFromForm == 0 && saveSuccessful && !mailSuccessful) { // New add, save ok, mail fail
                status = true;
                message = CommonMessages.DATA_SAVE_SUCCESS + ". However error occurred while sending mail to designated person";
            } else {
                status = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/GenerateNewDPId", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<Void> generateNewDPId(@RequestParam("InsurerUserId") int insurerUserId, HttpServletRequest request) {
        int dpId = -1;
        boolean status = false;
        String message = "";
        Map<String, String> additionalParams = null;
        try {
            // Assuming MasterDataService has getNewDPId
            dpId = masterDataService.getNewDPId(PortalApplication.getConnectionString(), insurerUserId);
            if (dpId == -1) {
                status = false;
                message = "Unable to register new Designated Person as DP ID range is exhausted.\nKindly contact technical support team";
            } else {
                status = true;
                additionalParams = new HashMap<>();
                additionalParams.put("DPUserID", String.valueOf(dpId));
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            status = false;
            message = CommonMessages.ERROR_OCCURED;
        }
        ApiResponse<Void> response = new ApiResponse<>(status, message);
        if (additionalParams != null) {
            response.setAdditionalParams(additionalParams);
        }
        return response;
    }
    
    // @AuthorizeExt
    @GetMapping("/NewAC")
    public String newACView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "NewAC";
    }

    // @AuthorizeExt
    @GetMapping("/ViewAC")
    public String viewACView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "ViewAC";
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/SaveAC", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveAC(HttpServletRequest request) {
        boolean initialStatus = true; // For parsing phase
        String message = "";

        int agentCounsellorUserId = 0;
        String nameForMail = "";
        String loginIdForMail = "";
        String emailIdForMail = "";
        String passwordForMail = ""; // Plain password for email
        boolean isNewAddition = false;

        try {
            String dummy = request.getParameter("hdnAgentCounsellorUserId");
            agentCounsellorUserId = (dummy == null || dummy.trim().equals("0") || dummy.trim().isEmpty()) ? 0 : Integer.parseInt(dummy);
            isNewAddition = (agentCounsellorUserId == 0);

            int dpUserid = Integer.parseInt(request.getParameter("hdnDPUserId"));
            int insurerUserId = Integer.parseInt(request.getParameter("hdnInsurerUserId")); // Used in service call
            int userIdAc = Integer.parseInt(request.getParameter("hdnACUserId")); // 'UserId' param for service
            nameForMail = request.getParameter("txtName");

            if (isNewAddition) {
                loginIdForMail = request.getParameter("txtLoginId");
            } else {
                loginIdForMail = request.getParameter("hdnLoginId");
            }

            String houseNo = request.getParameter("txtAddress");
            String street = request.getParameter("txtStreet");
            String town = request.getParameter("txtTown");
            int districtId = Integer.parseInt(request.getParameter("cboDistricts"));
            int pincode = Integer.parseInt(request.getParameter("txtPincode"));
            String teloffice = request.getParameter("txtTelephoneO");
            String telResidence = request.getParameter("txtTelephoneR");
            String fax = request.getParameter("txtFax");
            emailIdForMail = request.getParameter("txtEmailID");
            String mobileNo = request.getParameter("txtMobileNo");
            
            dummy = request.getParameter("chkActive");
            boolean isActive = "on".equalsIgnoreCase(dummy) || "true".equalsIgnoreCase(dummy);
            dummy = request.getParameter("chkChangePwd");
            boolean changePwdOnNextLogin = "on".equalsIgnoreCase(dummy) || "true".equalsIgnoreCase(dummy);

            // If parsing successful, proceed to service call
            boolean saveSuccessful = false; // For mail logic in catch block
            boolean mailSuccessful = false;

            passwordForMail = UUID.randomUUID().toString().split("-")[0];
            String txnsPassword = UUID.randomUUID().toString().split("-")[0];
            String encPassword = LoginEncryptor.encrypt(passwordForMail);
            String encTxnsPassword = LoginEncryptor.encrypt(txnsPassword);

            int createdBy = isNewAddition ? PortalSession.getUserID(request) : -1;
            int modifiedBy = isNewAddition ? -1 : PortalSession.getUserID(request);
            
            // Service call returns UserDataAccess.SaveACResult (nested DTO)
            UserDataAccess.SaveACResult serviceResult = userService.saveAC(PortalApplication.getConnectionString(),
                    agentCounsellorUserId, insurerUserId, dpUserid, userIdAc, nameForMail, loginIdForMail, 
                    encPassword, encTxnsPassword, houseNo, street, town, districtId, pincode, 
                    teloffice, telResidence, fax, emailIdForMail, mobileNo, 
                    createdBy, modifiedBy, false, isActive, changePwdOnNextLogin); // isSystemDefined = false

            int retVal = serviceResult.getReturnValue();
            message = serviceResult.getMessage(); // Message from service/DAL

            if (retVal == 0) {
                initialStatus = false; // Final status is failure
                message = CommonMessages.DATA_SAVE_FAIL + " : " + message;
            } else if (retVal == 1) { // Success
                saveSuccessful = true;
                initialStatus = true; // Final status is success
                if (isNewAddition) {
                    String subject = "System Mail - Login Information";
                    String body = String.format("<B>Dear %s,</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> %s<br><b>Password:</b> %s<br><br>Thanks and regards", nameForMail, loginIdForMail, passwordForMail);
                    try {
                        HelperUtilities.sendMail(subject, emailIdForMail, "", "", body, true);
                        mailSuccessful = true;
                        message = CommonMessages.DATA_SAVE_SUCCESS + " : Mail sent to " + nameForMail + " at (" + emailIdForMail + ")";
                    } catch (Exception mailEx) {
                        ErrorLogger.logError(getCurrentClassName(), "SaveAC.SendMail", mailEx, request.getParameterMap());
                        message = CommonMessages.DATA_SAVE_SUCCESS + ". However, an error occurred while sending mail to " + nameForMail + ".";
                    }
                } else { // Modification success
                    message = CommonMessages.DATA_SAVE_SUCCESS;
                }
            } else {
                 initialStatus = false;
                 message = CommonMessages.DATA_SAVE_FAIL + " (Unknown return: " + retVal + ", Msg: " + message +")";
            }

        } catch (NumberFormatException nfe) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + " (Numeric field error)";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            initialStatus = false; // Default to failure on general exception
            // The C# mail check: if (InsurerUserId == 0 && SaveSuccessful && !MailSuccessful)
            // This condition 'InsurerUserId == 0' seems out of place for SaveAC.
            // For SaveAC, the relevant flag is 'isNewAddition'.
            // The 'saveSuccessful' and 'mailSuccessful' flags are local to the try block after parsing.
            // If an exception occurs *during* the service call or mail sending, those flags might not reflect the true state prior to *this* catch block.
            // For robustness, the logic should be: If service call threw exception, it's ERROR_OCCURRED.
            // If mail sending threw exception *after* successful save, the message is already set inside the 'if (retVal == 1)' block.
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(initialStatus, message);
    }
    
    // @AuthorizeAJAX
    @PostMapping(value = "/DeleteAC", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> deleteAC(HttpServletRequest request) {
        boolean status = true;
        String message = "";
        int agentCounsellorUserId = 0;

        try {
            String acUserIdStr = request.getParameter("AgentCounsellorUserId");
            if (acUserIdStr == null || !acUserIdStr.matches("\\d+")) { // Check if it's a valid integer string
                status = false;
                message = CommonMessages.INVALID_INPUT + " (AgentCounsellorUserId is required and must be a number)";
            } else {
                agentCounsellorUserId = Integer.parseInt(acUserIdStr);
            }
        } catch (NumberFormatException e) { // Should be caught by regex, but as a fallback
            status = false;
            message = CommonMessages.INVALID_INPUT + " (Invalid AgentCounsellorUserId format)";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", e, request.getParameterMap());
        }

        if (status) {
            try {
                String serviceMessage = userService.deleteAC(PortalApplication.getConnectionString(), agentCounsellorUserId, PortalSession.getUserID(request));
                if (serviceMessage == null || serviceMessage.isEmpty()) {
                    status = true;
                    message = CommonMessages.DATA_DELETION_SUCCESS;
                } else {
                    status = false;
                    message = CommonMessages.DATA_DELETION_FAIL + " : " + serviceMessage;
                }
            } catch (Exception ex) {
                ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
                status = false;
                message = CommonMessages.ERROR_OCCURED;
            }
        }
        return new ApiResponse<>(status, message);
    }

    // @AuthorizeExt
    @GetMapping("/Roles")
    public String rolesView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "Roles";
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveRole", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveRole(HttpServletRequest request) {
        boolean initialStatus = true;
        String message = "";
        int roleId = 0;
        String roleCode = "", roleName = "", remark = "";
        boolean isActive = false;

        try {
            String dummy = request.getParameter("hdnRoleId");
            roleId = (dummy == null || dummy.trim().equals("0") || dummy.trim().isEmpty()) ? 0 : Integer.parseInt(dummy);
            roleCode = request.getParameter("txtRoleCode");
            roleName = request.getParameter("txtRoleName");
            remark = request.getParameter("txtRoleDescription");
            dummy = request.getParameter("chkIsActive");
            isActive = "on".equalsIgnoreCase(dummy) || "true".equalsIgnoreCase(dummy);

            if (roleCode == null || roleCode.trim().isEmpty() || roleName == null || roleName.trim().isEmpty()) {
                initialStatus = false;
                message = CommonMessages.INVALID_INPUT + " (Role Code and Name are required)";
            }
        } catch (NumberFormatException nfe) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + " (Invalid RoleId format)";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing-RoleId)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT;
             ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", ex, request.getParameterMap());
        }

        if (initialStatus) {
            try {
                String serviceMessage = userService.saveRole(PortalApplication.getConnectionString(), roleId, roleCode, roleName, remark, isActive, false, PortalSession.getUserID(request)); // isSystemDefined = false
                if (serviceMessage == null || serviceMessage.isEmpty()) {
                    initialStatus = true; // Final status is success
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
    
    // @AuthorizeAJAX
    @PostMapping(value = "/SaveInsurer", consumes = MediaType.MULTIPART_FORM_DATA_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveInsurer(HttpServletRequest request,
                                      @RequestParam(value = "txtFileSign", required = false) MultipartFile signFile) {
        boolean initialStatus = true; // For parsing
        String message = "";
        
        int insurerUserId = 0; // PK of tblMstInsurerUser
        int userId = 0;        // PK of tblMstUser for the CDP
        String emailIdForMail = "";
        String cdpNameForMail = "";
        String insurerLoginIdForMail = "";
        String passwordForMail = ""; // Plain password for email
        boolean isNewAddition = false; // To control mail sending

        try {
            String dummy = request.getParameter("hdnInttblmstinsureruserid");
            insurerUserId = (dummy == null || dummy.trim().equals("0") || dummy.trim().isEmpty()) ? 0 : Integer.parseInt(dummy);
            isNewAddition = (insurerUserId == 0);

            dummy = request.getParameter("hdnIntUserID");
            userId = (dummy == null || dummy.trim().equals("0") || dummy.trim().isEmpty()) ? 0 : Integer.parseInt(dummy);

            insurerLoginIdForMail = request.getParameter("txtInsurerCode");
            String insurerRegNo = request.getParameter("txtInsurerRegistrationNumber");
            String insurerName = request.getParameter("txtName");
            cdpNameForMail = request.getParameter("txtCDPName");
            int insurerType = Integer.parseInt(request.getParameter("cboInsurerType"));
            String addressLine1 = request.getParameter("txtAddress1");
            String addressLine2 = request.getParameter("txtAddress2");
            String addressLine3 = request.getParameter("txtAddress3");
            int districtId = Integer.parseInt(request.getParameter("cboDistricts"));
            int pincode = Integer.parseInt(request.getParameter("txtPincode"));
            String telephoneO = request.getParameter("txtTelephoneO");
            String telephoneR = request.getParameter("txtTelephoneR");
            String fax = request.getParameter("txtFax");
            emailIdForMail = request.getParameter("txtEmailID");

            byte[] signBytes = null;
            if (signFile != null && !signFile.isEmpty()) {
                signBytes = signFile.getBytes();
            }

            passwordForMail = UUID.randomUUID().toString().split("-")[0];
            String txnPassword = UUID.randomUUID().toString().split("-")[0];
            String encPassword = LoginEncryptor.encrypt(passwordForMail);
            String encTxnPassword = LoginEncryptor.encrypt(txnPassword);

            // Call service
            // UserDataAccess.SaveInsurerResult has (int returnValue, String message)
            UserDataAccess.SaveInsurerResult serviceResult = userService.saveInsurer(PortalApplication.getConnectionString(), 
                insurerUserId, userId, insurerName, cdpNameForMail, insurerLoginIdForMail, insurerRegNo, 
                encPassword, encTxnPassword, insurerType, addressLine1, addressLine2, addressLine3, 
                districtId, pincode, telephoneO, telephoneR, fax, emailIdForMail, 
                PortalSession.getUserID(request), true, signBytes); // isActive = true

            int successCode = serviceResult.getReturnValue(); // returnValue from DTO
            message = serviceResult.getMessage(); // message from DTO

            boolean saveSuccessful = false; // For mail logic catch block
            boolean mailSuccessful = false;

            if (successCode == 0) { // C# implies 0 is failure
                initialStatus = false; // Final status
                message = CommonMessages.DATA_SAVE_FAIL + " : " + message;
            } else if (successCode == 1) { // New Addition
                saveSuccessful = true;
                initialStatus = true;
                String subject = "System Mail - Login Information";
                String body = String.format("<B>Dear %s,</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> %s<br><b>Password:</b> %s<br><br>Thanks and regards", cdpNameForMail, insurerLoginIdForMail, passwordForMail);
                try {
                    HelperUtilities.sendMail(subject, emailIdForMail, "", "", body, true);
                    mailSuccessful = true;
                    message = CommonMessages.DATA_SAVE_SUCCESS + ". Password has been sent to corporate designated person on his Email id";
                } catch (Exception mailEx) {
                    ErrorLogger.logError(getCurrentClassName(), "SaveInsurer.SendMail", mailEx, request.getParameterMap());
                    message = CommonMessages.DATA_SAVE_SUCCESS + ". However, an error occurred while sending mail to corporate designated person.";
                }
            } else if (successCode == 2) { // Modification
                initialStatus = true;
                message = CommonMessages.DATA_SAVE_SUCCESS;
            } else { // Other codes from DB (-200, -400 etc. in C# comments) map to this case
                initialStatus = false;
                 if (message == null || message.isEmpty()) message = "Unknown error or specific error from DB.";
                 message = CommonMessages.DATA_SAVE_FAIL + " : " + message + " (Code: " + successCode + ")";
            }

        } catch (NumberFormatException nfe) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + " (Numeric field error)";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
        } catch (IOException ioEx) {
            initialStatus = false;
            message = "Error processing signature file.";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (FileProcessing)", ioEx, request.getParameterMap());
        }
        catch (Exception ex) { // General catch for service call issues or other unhandled
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            initialStatus = false;
            // The mail check 'if (InsurerUserId == 0 && SaveSuccessful && !MailSuccessful)' from C#
            // maps to 'if (isNewAddition && saveSuccessful && !mailSuccessful)'
            // This specific re-evaluation of status and message is complex to replicate exactly
            // if exception occurs *after* initial save but *before* or *during* mail.
            // The current structure sets message appropriately inside the mail block.
            // If this catch is reached, it's likely a more general failure.
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(initialStatus, message);
    }
    
    // @AuthorizeExt
    @GetMapping("/Users")
    public String usersView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "Users"; // View name
    }

    // @AuthorizeAJAX
    @PostMapping(value = "/SaveUser", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public ApiResponse<?> saveUser(HttpServletRequest request) {
        boolean initialStatus = true; // For parsing
        String message = "";
        
        int userId = 0;
        String userLoginIdForMail = "";
        String userNameForMail = "";
        String emailIdForMail = "";
        String passwordForMail = ""; // Plain password for email
        boolean isAddition = false;

        try {
            String dummy = request.getParameter("hdnUserId");
            userId = (dummy == null || dummy.trim().equals("0") || dummy.trim().isEmpty()) ? 0 : Integer.parseInt(dummy);
            isAddition = (userId == 0);

            userLoginIdForMail = request.getParameter("txtLoginId");
            userNameForMail = request.getParameter("txtUserName");
            String mobileNo = request.getParameter("txtMobileNo");
            emailIdForMail = request.getParameter("txtEmailID");
            int roleId = Integer.parseInt(request.getParameter("cboRoles"));
            dummy = request.getParameter("chkIsActive");
            boolean isActive = "on".equalsIgnoreCase(dummy) || "true".equalsIgnoreCase(dummy);

            // If parsing is successful:
            boolean saveSuccessful = false; // For mail logic in catch block
            boolean mailSuccessful = false;

            passwordForMail = UUID.randomUUID().toString().split("-")[0];
            String txnPassword = UUID.randomUUID().toString().split("-")[0];
            String encPassword = LoginEncryptor.encrypt(passwordForMail);
            String encTxnPassword = LoginEncryptor.encrypt(txnPassword);

            String serviceMessage = userService.saveUser(PortalApplication.getConnectionString(), userId, userLoginIdForMail, 
                userNameForMail, encPassword, encTxnPassword, mobileNo, emailIdForMail, isActive, roleId, 
                PortalSession.getUserID(request));

            if (serviceMessage == null || serviceMessage.isEmpty()) {
                saveSuccessful = true;
                initialStatus = true; // Final status success
                message = CommonMessages.DATA_SAVE_SUCCESS;

                if (isAddition) { // Send mail only for new additions if save was successful
                    String subject = "System Mail - Login Information";
                    String body = String.format("<B>Dear %s,</B><br><br>This is System Generated Mail. Please do not reply.<br><br>User Login Information:<br><b>User Id:</b> %s<br><b>Password:</b> %s<br><br>Thanks and regards", userNameForMail, userLoginIdForMail, passwordForMail);
                    try {
                        HelperUtilities.sendMail(subject, emailIdForMail, "", "", body, true);
                        mailSuccessful = true;
                        message = CommonMessages.DATA_SAVE_SUCCESS + ". Password has been sent to the user on his Email id";
                    } catch (Exception mailEx) {
                        ErrorLogger.logError(getCurrentClassName(), "SaveUser.SendMail", mailEx, request.getParameterMap());
                        message = CommonMessages.DATA_SAVE_SUCCESS + ". However, an error occurred while sending mail to the User.";
                    }
                }
            } else { // Service returned an error message
                initialStatus = false;
                message = CommonMessages.DATA_SAVE_FAIL + " : " + serviceMessage;
            }

        } catch (NumberFormatException nfe) {
            initialStatus = false;
            message = CommonMessages.INVALID_INPUT + " (Numeric field error)";
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName() + " (InputParsing)", nfe, request.getParameterMap());
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            initialStatus = false;
            // C# mail check: if (UserId == 0 && SaveSuccessful && !MailSuccessful)
            // As before, this fine-grained check within a general catch is complex.
            // The current structure should set the message correctly if mail fails *after* successful save.
            message = CommonMessages.ERROR_OCCURED;
        }
        return new ApiResponse<>(initialStatus, message);
    }
    
    @GetMapping("/RolePermission") // Assuming AuthorizeExt would apply if it were a secure page
    public String rolePermissionView(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "RolePermission"; // View name
    }
}