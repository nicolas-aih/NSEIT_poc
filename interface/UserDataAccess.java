package com.example.interfaces;

import java.util.List;
import java.util.Map;

// Corresponds to IIIDL.Users.cs
public interface UserDataAccess {

    // --- Nested DTOs for return types ---
    class LoginResult { // Renamed from LoginResultDto for brevity as nested
        private List<Map<String, Object>> dataSet;
        private String resultMessage;

        public LoginResult(List<Map<String, Object>> dataSet, String resultMessage) {
            this.dataSet = dataSet;
            this.resultMessage = resultMessage;
        }
        public List<Map<String, Object>> getDataSet() { return dataSet; }
        public String getResultMessage() { return resultMessage; }
        public void setDataSet(List<Map<String, Object>> dataSet) { this.dataSet = dataSet;}
        public void setResultMessage(String resultMessage) { this.resultMessage = resultMessage;}
    }

    class SaveACResult { // Renamed from SaveACResultDto
        private int returnValue;
        private String message;

        public SaveACResult(int returnValue, String message) {
            this.returnValue = returnValue;
            this.message = message;
        }
        public int getReturnValue() { return returnValue; }
        public String getMessage() { return message; }
        public void setReturnValue(int returnValue) { this.returnValue = returnValue;}
        public void setMessage(String message) { this.message = message;}
    }

    class SaveInsurerResult { // Renamed from SaveInsurerResultDto
        private int returnValue;
        private String message;

        public SaveInsurerResult(int returnValue, String message) {
            this.returnValue = returnValue;
            this.message = message;
        }
        public int getReturnValue() { return returnValue; }
        public String getMessage() { return message; }
        public void setReturnValue(int returnValue) { this.returnValue = returnValue;}
        public void setMessage(String message) { this.message = message;}
    }

    class CompanyPaymentModes { // Renamed from CompanyPaymentModesDto
        private String companyName;
        private boolean creditMode;
        private boolean onlineMode;
        private String message;

        public CompanyPaymentModes(String companyName, boolean creditMode, boolean onlineMode, String message) {
            this.companyName = companyName;
            this.creditMode = creditMode;
            this.onlineMode = onlineMode;
            this.message = message;
        }
        public String getCompanyName() { return companyName; }
        public boolean isCreditMode() { return creditMode; }
        public boolean isOnlineMode() { return onlineMode; }
        public String getMessage() { return message; }
        public void setCompanyName(String companyName) { this.companyName = companyName;}
        public void setCreditMode(boolean creditMode) { this.creditMode = creditMode;}
        public void setOnlineMode(boolean onlineMode) { this.onlineMode = onlineMode;}
        public void setMessage(String message) { this.message = message;}
    }

    // --- Interface Methods ---
    LoginResult login(String connectionString, String loginId, String password /*, boolean isLive*/);

    void logout(String connectionString, int userId);

    String changePassword(String connectionString, String loginId, String password, String newPassword);

    String resetPassword(String connectionString, String loginId, String emailId, String password, String transactionPassword);

    List<Map<String, Object>> menuPermissions(String connectionString, long userId /*, boolean isLive*/);

    List<Map<String, Object>> getUserDetails(String connectionString, int userId, String userType);

    String saveUserProfile(String connectionString, String userType, int userId, String address1, String address2,
                           String address3, int pincode, String telephoneOffice, String telephoneResidence,
                           String fax, String poName, String emailId, String mobileNo, String stdCode,
                           String phoneNo, int districtId);

    int saveDPDetails(String connectionString, int dpUserId, String dpId, int insurerUserId, String name,
                      String houseNo, String street, String town, int districtId, String pincode,
                      String telephoneOffice, String telephoneResidence, String fax, String emailId, int createdBy,
                      String password, String txnPassword, String mobileNo, boolean isActive,
                      boolean changePwdOnNextLogin, int incorrectLoginAttempts, boolean isSystemDefined,
                      int lastModifiedBy, int flag, byte[] dpSignature);

    SaveACResult saveAC(String connectionString, int agentCounsellorUserId, int insurerUserId, int dpUserid,
                           int userId, String name, String loginId, String password, String txnsPassword,
                           String houseNo, String street, String town, int districtId, int pincode,
                           String teloffice, String telResidence, String fax, String emailId, String mobileNo,
                           int createdBy, int modifiedby, boolean isSytemDefined, boolean isActive,
                           boolean changePwdOnNextLogin);

    String deleteAC(String connectionString, int agentCounsellorUserId, int createdBy);

    List<Map<String, Object>> getRoles(String connectionString, int roleId);

    List<Map<String, Object>> getRolesForUserCreation(String connectionString);

    String saveRole(String connectionString, int roleId, String roleCode, String roleName, String remark,
                    boolean isActive, boolean isSystemDefined, int userId);

    SaveInsurerResult saveInsurer(String connectionString, int insurerUserId, int userId, String insurerName,
                                     String cdpName, String insurerLoginId, String insurerRegNo, String password,
                                     String txnPassword, int insurerType, String addressLine1, String addressLine2,
                                     String addressLine3, int districtId, int pincode, String telNoOffice,
                                     String telNoResidence, String faxNo, String emailId, int currentUserId,
                                     boolean isActive, byte[] cdpSignature);

    String saveUser(String connectionString, int userId, String userLoginId, String userName, String password,
                    String txnPassword, String mobileNo, String emailId, boolean isActive, int roleId,
                    int currentUserId);

    List<Map<String, Object>> getUsers(String connectionString, int userId);

    List<Map<String, Object>> getAllUsers(String connectionString, int intUserId);

    List<Map<String, Object>> getAllMenuPermissions(String connectionString, long intSearchId, boolean isRole);

    List<Map<String, Object>> saveMenuPermissions(String connectionString, String txtMenuXml, long intSearchId, boolean isRole);
    
    CompanyPaymentModes getCompanyPaymentModes(String connectionString, String companyType, String companyLoginId);

    String saveCompanyPaymentModes(String connectionString, String companyType, String companyLoginId, boolean creditMode, boolean onlineMode);
}