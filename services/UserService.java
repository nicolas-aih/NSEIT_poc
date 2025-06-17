package com.example.services;

import com.example.interfaces.UserDataAccess; // Import the interface
// Import the nested DTOs specifically if needed for type hinting or explicit use,
// or rely on them being accessible via UserDataAccess.LoginResult etc.
import com.example.interfaces.UserDataAccess.LoginResult;
import com.example.interfaces.UserDataAccess.SaveACResult;
import com.example.interfaces.UserDataAccess.SaveInsurerResult;
import com.example.interfaces.UserDataAccess.CompanyPaymentModes;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

// Corresponds to IIIBL.Users.cs
@Service
public class UserService {

    private final UserDataAccess userDataAccess;

    @Autowired
    public UserService(UserDataAccess userDataAccess) {
        this.userDataAccess = userDataAccess;
    }

    public LoginResult login(String connectionString, String loginId, String password /*, boolean isLive*/) {
        try {
            return userDataAccess.login(connectionString, loginId, password /*, isLive*/);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public void logout(String connectionString, int userId) {
        try {
            userDataAccess.logout(connectionString, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String changePassword(String connectionString, String loginId, String password, String newPassword) {
        try {
            return userDataAccess.changePassword(connectionString, loginId, password, newPassword);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String resetPassword(String connectionString, String loginId, String emailId, String password, String transactionPassword) {
        try {
            return userDataAccess.resetPassword(connectionString, loginId, emailId, password, transactionPassword);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> menuPermissions(String connectionString, long userId /*, boolean isLive*/) {
        try {
            return userDataAccess.menuPermissions(connectionString, userId /*, isLive*/);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getUserDetails(String connectionString, int userId, String userType) {
        try {
            return userDataAccess.getUserDetails(connectionString, userId, userType);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String saveUserProfile(String connectionString, String userType, int userId, String address1, String address2,
                                  String address3, int pincode, String telephoneOffice, String telephoneResidence,
                                  String fax, String poName, String emailId, String mobileNo, String stdCode,
                                  String phoneNo, int districtId) {
        try {
            return userDataAccess.saveUserProfile(connectionString, userType, userId, address1, address2, address3,
                                                 pincode, telephoneOffice, telephoneResidence, fax, poName, emailId,
                                                 mobileNo, stdCode, phoneNo, districtId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public int saveDPDetails(String connectionString, int dpUserId, String dpId, int insurerUserId, String name,
                             String houseNo, String street, String town, int districtId, String pincode,
                             String telephoneOffice, String telephoneResidence, String fax, String emailId, int createdBy,
                             String password, String txnPassword, String mobileNo, boolean isActive,
                             boolean changePwdOnNextLogin, int incorrectLoginAttempts, boolean isSystemDefined,
                             int lastModifiedBy, int flag, byte[] dpSignature) {
        try {
            return userDataAccess.saveDPDetails(connectionString, dpUserId, dpId, insurerUserId, name, houseNo, street,
                                              town, districtId, pincode, telephoneOffice, telephoneResidence, fax,
                                              emailId, createdBy, password, txnPassword, mobileNo, isActive,
                                              changePwdOnNextLogin, incorrectLoginAttempts, isSystemDefined,
                                              lastModifiedBy, flag, dpSignature);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public SaveACResult saveAC(String connectionString, int agentCounsellorUserId, int insurerUserId, int dpUserid,
                                  int userId, String name, String loginId, String password, String txnsPassword,
                                  String houseNo, String street, String town, int districtId, int pincode,
                                  String teloffice, String telResidence, String fax, String emailId, String mobileNo,
                                  int createdBy, int modifiedby, boolean isSytemDefined, boolean isActive,
                                  boolean changePwdOnNextLogin) {
        try {
            return userDataAccess.saveAC(connectionString, agentCounsellorUserId, insurerUserId, dpUserid, userId, name,
                                        loginId, password, txnsPassword, houseNo, street, town, districtId, pincode,
                                        teloffice, telResidence, fax, emailId, mobileNo, createdBy, modifiedby,
                                        isSytemDefined, isActive, changePwdOnNextLogin);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String deleteAC(String connectionString, int agentCounsellorUserId, int createdBy) {
        try {
            return userDataAccess.deleteAC(connectionString, agentCounsellorUserId, createdBy);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getRoles(String connectionString, int roleId) {
        try {
            return userDataAccess.getRoles(connectionString, roleId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getRolesForUserCreation(String connectionString) {
        try {
            return userDataAccess.getRolesForUserCreation(connectionString);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String saveRole(String connectionString, int roleId, String roleCode, String roleName, String remark,
                           boolean isActive, boolean isSystemDefined, int userId) {
        try {
            return userDataAccess.saveRole(connectionString, roleId, roleCode, roleName, remark, isActive,
                                          isSystemDefined, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public SaveInsurerResult saveInsurer(String connectionString, int insurerUserId, int userId, String insurerName,
                                            String cdpName, String insurerLoginId, String insurerRegNo, String password,
                                            String txnPassword, int insurerType, String addressLine1, String addressLine2,
                                            String addressLine3, int districtId, int pincode, String telNoOffice,
                                            String telNoResidence, String faxNo, String emailId, int currentUserId,
                                            boolean isActive, byte[] cdpSignature) {
        try {
            return userDataAccess.saveInsurer(connectionString, insurerUserId, userId, insurerName, cdpName,
                                             insurerLoginId, insurerRegNo, password, txnPassword, insurerType,
                                             addressLine1, addressLine2, addressLine3, districtId, pincode,
                                             telNoOffice, telNoResidence, faxNo, emailId, currentUserId, isActive,
                                             cdpSignature);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String saveUser(String connectionString, int userId, String userLoginId, String userName, String password,
                           String txnPassword, String mobileNo, String emailId, boolean isActive, int roleId,
                           int currentUserId) {
        try {
            return userDataAccess.saveUser(connectionString, userId, userLoginId, userName, password, txnPassword,
                                          mobileNo, emailId, isActive, roleId, currentUserId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getUsers(String connectionString, int userId) {
        try {
            return userDataAccess.getUsers(connectionString, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getAllUsers(String connectionString) {
        try {
            return userDataAccess.getAllUsers(connectionString, 0);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getMenuPermissions(String connectionString, long intSearchId, boolean isRole) {
        try {
            return userDataAccess.getAllMenuPermissions(connectionString, intSearchId, isRole);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> saveMenuPermissions(String connectionString, String txtXml, long intSearchId, boolean isRole) {
        try {
            return userDataAccess.saveMenuPermissions(connectionString, txtXml, intSearchId, isRole);
        } catch (Exception ex) {
            throw ex;
        }
    }
    
    public CompanyPaymentModes getCompanyPaymentModes(String connectionString, String companyType, String companyLoginId) {
        try {
            return userDataAccess.getCompanyPaymentModes(connectionString, companyType, companyLoginId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String saveCompanyPaymentModes(String connectionString, String companyType, String companyLoginId, boolean creditMode, boolean onlineMode) {
        try {
            return userDataAccess.saveCompanyPaymentModes(connectionString, companyType, companyLoginId, creditMode, onlineMode);
        } catch (Exception ex) {
            throw ex;
        }
    }
}