package com.example.service.impl;

import com.example.service.ValidationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.Period;
import java.util.Map;

@Service
@Transactional
public class ValidationServiceImpl implements ValidationService {

    private final JdbcTemplate jdbcTemplate;

    @Autowired
    public ValidationServiceImpl(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    @Override
    public boolean validateDateOfBirth(LocalDate dob) {
        if (dob == null) {
            return false;
        }
        
        LocalDate now = LocalDate.now();
        Period age = Period.between(dob, now);
        return age.getYears() >= 18;
    }

    @Override
    public boolean validateYearOfPassing(LocalDate yop) {
        if (yop == null) {
            return false;
        }
        return !yop.isAfter(LocalDate.now());
    }

    @Override
    public String validatePAN(String pan, Long applicantId) {
        if (pan == null || pan.trim().isEmpty()) {
            return "PAN number is required";
        }

        if (!pan.matches("[A-Z]{5}[0-9]{4}[A-Z]{1}")) {
            return "Invalid PAN format";
        }

        // Check if PAN already exists for another applicant
        String sql = "SELECT COUNT(*) FROM applicants WHERE pan = ? AND applicant_id != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, pan, applicantId);
        
        return count > 0 ? "PAN already exists" : "";
    }

    @Override
    public String validateEmailForCorporates(String email, Long applicantId, String pan) {
        if (email == null || email.trim().isEmpty()) {
            return "Email is required";
        }

        if (!email.matches("^[A-Za-z0-9+_.-]+@(.+)$")) {
            return "Invalid email format";
        }

        // Check if email already exists for another corporate
        String sql = "SELECT COUNT(*) FROM corporate_applicants WHERE email = ? AND applicant_id != ? AND pan != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, email, applicantId, pan);
        
        return count > 0 ? "Email already exists" : "";
    }

    @Override
    public String validateMobileForCorporates(String mobile, Long applicantId, String pan) {
        if (mobile == null || mobile.trim().isEmpty()) {
            return "Mobile number is required";
        }

        if (!mobile.matches("^[0-9]{10}$")) {
            return "Invalid mobile number format";
        }

        // Check if mobile already exists for another corporate
        String sql = "SELECT COUNT(*) FROM corporate_applicants WHERE mobile = ? AND applicant_id != ? AND pan != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, mobile, applicantId, pan);
        
        return count > 0 ? "Mobile number already exists" : "";
    }

    @Override
    public String validateMobileForCorporatesApp(String mobile, Long applicantDataId, String pan) {
        if (mobile == null || mobile.trim().isEmpty()) {
            return "Mobile number is required";
        }

        if (!mobile.matches("^[0-9]{10}$")) {
            return "Invalid mobile number format";
        }

        // Check if mobile already exists for another corporate application
        String sql = "SELECT COUNT(*) FROM corporate_applications WHERE mobile = ? AND applicant_data_id != ? AND pan != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, mobile, applicantDataId, pan);
        
        return count > 0 ? "Mobile number already exists" : "";
    }

    // Stub/mock implementations for new methods
    @Override
    public Map<String, Object> validateFromTillDate(LocalDate fromDate, LocalDate tillDate) {
        return Map.of("success", true, "message", "Stub: validateFromTillDate");
    }

    @Override
    public Map<String, Object> validateNotificationFromDate(LocalDate date) {
        return Map.of("success", true, "message", "Stub: validateNotificationFromDate");
    }

    @Override
    public Map<String, Object> validateYOP2(LocalDate dateDOB, LocalDate dateYOP) {
        return Map.of("success", true, "message", "Stub: validateYOP2");
    }

    @Override
    public Map<String, Object> validatePAN2(String pan, Long applicantId) {
        return Map.of("success", true, "message", "Stub: validatePAN2");
    }

    @Override
    public Map<String, Object> validateInternalRefNo(String internalRefNo, Long applicantId) {
        return Map.of("success", true, "message", "Stub: validateInternalRefNo");
    }

    @Override
    public Map<String, Object> validateInternalRefNoApp(String internalRefNo, Long applicantDataId) {
        return Map.of("success", true, "message", "Stub: validateInternalRefNoApp");
    }

    @Override
    public Map<String, Object> validateAadhaarCorporates(String aadhaarNo, String pan, String urn) {
        return Map.of("success", true, "message", "Stub: validateAadhaarCorporates");
    }

    @Override
    public Map<String, Object> validateAadhaarCorporatesApp(String aadhaarNo, String pan, Long applicantDataId) {
        return Map.of("success", true, "message", "Stub: validateAadhaarCorporatesApp");
    }

    @Override
    public Map<String, Object> validateEmailCorporates(String emailId, Long applicantId, String pan) {
        return Map.of("success", true, "message", "Stub: validateEmailCorporates");
    }

    @Override
    public Map<String, Object> validateEmailCorporatesApp(String emailId, Long applicantDataId, String pan) {
        return Map.of("success", true, "message", "Stub: validateEmailCorporatesApp");
    }

    @Override
    public Map<String, Object> validateMobileCorporates(String mobileNo, Long applicantId, String pan) {
        return Map.of("success", true, "message", "Stub: validateMobileCorporates");
    }

    @Override
    public Map<String, Object> validateWhatsAppCorporates(String mobileNo, Long applicantId, String pan) {
        return Map.of("success", true, "message", "Stub: validateWhatsAppCorporates");
    }

    @Override
    public Map<String, Object> validateMobileCorporatesApp(String mobileNo, Long applicantDataId, String pan) {
        return Map.of("success", true, "message", "Stub: validateMobileCorporatesApp");
    }

    @Override
    public Map<String, Object> validateWhatsAppCorporatesApp(String mobileNo, Long applicantDataId, String pan) {
        return Map.of("success", true, "message", "Stub: validateWhatsAppCorporatesApp");
    }

    @Override
    public Map<String, Object> validateWhatsAppCorporatesForMod(String urn, String mobileNo) {
        return Map.of("success", true, "message", "Stub: validateWhatsAppCorporatesForMod");
    }

    @Override
    public Map<String, Object> getACforDP(Integer insurerId, Integer dpUserId, Integer acUserId) {
        return Map.of("success", true, "message", "Stub: getACforDP");
    }

    @Override
    public Map<String, Object> getAllNotifications(Integer notificationId) {
        return Map.of("success", true, "message", "Stub: getAllNotifications");
    }

    @Override
    public Map<String, Object> getTbxSchedule() {
        return Map.of("success", true, "message", "Stub: getTbxSchedule");
    }

    @Override
    public Map<String, Object> getAllRoles(Integer roleId) {
        return Map.of("success", true, "message", "Stub: getAllRoles");
    }

    @Override
    public Map<String, Object> getRolesForUserCreation() {
        return Map.of("success", true, "message", "Stub: getRolesForUserCreation");
    }

    @Override
    public Map<String, Object> getUsers(Integer userId) {
        return Map.of("success", true, "message", "Stub: getUsers");
    }

    @Override
    public Map<String, Object> getAllUsers(Integer userId) {
        return Map.of("success", true, "message", "Stub: getAllUsers");
    }

    @Override
    public Map<String, Object> getDPForInsurer(Integer insurerId, Integer dpId) {
        return Map.of("success", true, "message", "Stub: getDPForInsurer");
    }

    @Override
    public Map<String, Object> getDPForInsurerEx(Integer insurerId, Integer dpId) {
        return Map.of("success", true, "message", "Stub: getDPForInsurerEx");
    }

    @Override
    public Map<String, Object> getInsurer() {
        return Map.of("success", true, "message", "Stub: getInsurer");
    }

    @Override
    public Map<String, Object> getInsurer2(Integer insurerId) {
        return Map.of("success", true, "message", "Stub: getInsurer2");
    }

    @Override
    public Map<String, Object> getBasicQualificationForCOR(String corType) {
        return Map.of("success", true, "message", "Stub: getBasicQualificationForCOR");
    }

    @Override
    public Map<String, Object> getProQualificationForCOR(String corType) {
        return Map.of("success", true, "message", "Stub: getProQualificationForCOR");
    }

    @Override
    public Map<String, Object> getDetailsForCOR(String corType) {
        return Map.of("success", true, "message", "Stub: getDetailsForCOR");
    }

    @Override
    public Map<String, Object> findNearestExamCenter(Integer pincode) {
        return Map.of("success", true, "message", "Stub: findNearestExamCenter");
    }

    @Override
    public Map<String, Object> getCentersForState(Integer stateId, Integer centerId) {
        return Map.of("success", true, "message", "Stub: getCentersForState");
    }

    @Override
    public Map<String, Object> getCentersForStateEx(Integer stateId, Integer centerId) {
        return Map.of("success", true, "message", "Stub: getCentersForStateEx");
    }

    @Override
    public Map<String, Object> getSimilarCenters(Integer centerId) {
        return Map.of("success", true, "message", "Stub: getSimilarCenters");
    }

    @Override
    public Map<String, Object> getCentersForStatePreLogin(Integer stateId) {
        return Map.of("success", true, "message", "Stub: getCentersForStatePreLogin");
    }

    @Override
    public Map<String, Object> saveExamCenter(Map<String, Object> examCenterData) {
        return Map.of("success", true, "message", "Stub: saveExamCenter");
    }

    @Override
    public Map<String, Object> getAvailableSeats(Integer stateId, Integer centerId, String examDate) {
        return Map.of("success", true, "message", "Stub: getAvailableSeats");
    }

    @Override
    public Map<String, Object> getMenuData(Integer searchId, Integer isRole) {
        return Map.of("success", true, "message", "Stub: getMenuData");
    }

    @Override
    public Map<String, Object> saveMenuPermission(Integer searchId, Integer isRole, java.util.List<String> menuId, java.util.List<String> oldValue, java.util.List<String> newValue) {
        return Map.of("success", true, "message", "Stub: saveMenuPermission");
    }

    @Override
    public Map<String, Object> getDPRangeData() {
        return Map.of("success", true, "message", "Stub: getDPRangeData");
    }

    @Override
    public Map<String, Object> saveDPRange(Integer insurerCode, Integer dpCount) {
        return Map.of("success", true, "message", "Stub: saveDPRange");
    }

    @Override
    public Map<String, Object> getBatchDetailsForPayment(String batchNo) {
        return Map.of("success", true, "message", "Stub: getBatchDetailsForPayment");
    }

    @Override
    public Map<String, Object> getBatchDetailsForMgmt(String batchNo) {
        return Map.of("success", true, "message", "Stub: getBatchDetailsForMgmt");
    }

    @Override
    public Map<String, Object> getBatchList(Map<String, String> searchParams) {
        return Map.of("success", true, "message", "Stub: getBatchList");
    }

    @Override
    public Map<String, Object> deleteBatch(String batchNo) {
        return Map.of("success", true, "message", "Stub: deleteBatch");
    }

    @Override
    public Map<String, Object> deleteProbURNs(String batchNo) {
        return Map.of("success", true, "message", "Stub: deleteProbURNs");
    }

    @Override
    public Map<String, Object> verifyBatch(String batchNo, String paymentGateway) {
        return Map.of("success", true, "message", "Stub: verifyBatch");
    }

    @Override
    public Map<String, Object> getCompanyPaymentModes(String companyType, String companyLoginId) {
        return Map.of("success", true, "message", "Stub: getCompanyPaymentModes");
    }

    @Override
    public Map<String, Object> saveCompanyPaymentModes(Map<String, Object> paymentModesData) {
        return Map.of("success", true, "message", "Stub: saveCompanyPaymentModes");
    }
} 