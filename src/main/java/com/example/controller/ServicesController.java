package com.example.controller;

import com.example.model.ExamCenter;
import com.example.service.ExamCenterService;
import com.example.service.ValidationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/services")
public class ServicesController {

    @Autowired
    private ValidationService validationService;

    @Autowired
    private ExamCenterService examCenterService;

    @PostMapping("/validate/dob")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateDateOfBirth(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate date) {
        boolean isValid = validationService.validateDateOfBirth(date);
        return ResponseEntity.ok(Map.of(
            "success", isValid,
            "message", isValid ? "" : "Age must be at least 18 years"
        ));
    }

    @PostMapping("/validate/yop")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateYearOfPassing(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate date) {
        boolean isValid = validationService.validateYearOfPassing(date);
        return ResponseEntity.ok(Map.of(
            "success", isValid,
            "message", isValid ? "" : "Year of passing cannot be in the future"
        ));
    }

    @PostMapping("/validate/pan")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validatePAN(
            @RequestParam String pan,
            @RequestParam Long applicantId) {
        Map<String, Object> result = validationService.validatePAN2(pan, applicantId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/email")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateEmailForCorporates(
            @RequestParam String email,
            @RequestParam Long applicantId,
            @RequestParam String pan) {
        String message = validationService.validateEmailForCorporates(email, applicantId, pan);
        return ResponseEntity.ok(Map.of(
            "success", message.isEmpty(),
            "message", message
        ));
    }

    @PostMapping("/validate/corporate/mobile")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateMobileCorporates(
            @RequestParam String mobileNo,
            @RequestParam Long applicantId,
            @RequestParam String pan) {
        Map<String, Object> result = validationService.validateMobileCorporates(mobileNo, applicantId, pan);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/whatsapp")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateWhatsAppCorporates(
            @RequestParam String mobileNo,
            @RequestParam Long applicantId,
            @RequestParam String pan) {
        Map<String, Object> result = validationService.validateWhatsAppCorporates(mobileNo, applicantId, pan);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/mobile-app")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateMobileCorporatesApp(
            @RequestParam String mobileNo,
            @RequestParam Long applicantDataId,
            @RequestParam String pan) {
        Map<String, Object> result = validationService.validateMobileCorporatesApp(mobileNo, applicantDataId, pan);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/whatsapp-app")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateWhatsAppCorporatesApp(
            @RequestParam String mobileNo,
            @RequestParam Long applicantDataId,
            @RequestParam String pan) {
        Map<String, Object> result = validationService.validateWhatsAppCorporatesApp(mobileNo, applicantDataId, pan);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/from-till-date")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateFromTillDate(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate tillDate) {
        Map<String, Object> result = validationService.validateFromTillDate(fromDate, tillDate);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/notification-from-date")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateNotificationFromDate(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate date) {
        Map<String, Object> result = validationService.validateNotificationFromDate(date);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/yop2")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateYOP2(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dateDOB,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dateYOP) {
        Map<String, Object> result = validationService.validateYOP2(dateDOB, dateYOP);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/pan2")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validatePAN2(
            @RequestParam String pan,
            @RequestParam Long applicantId) {
        Map<String, Object> result = validationService.validatePAN2(pan, applicantId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/internal-ref-no")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateInternalRefNo(
            @RequestParam String internalRefNo,
            @RequestParam Long applicantId) {
        Map<String, Object> result = validationService.validateInternalRefNo(internalRefNo, applicantId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/internal-ref-no-app")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateInternalRefNoApp(
            @RequestParam String internalRefNo,
            @RequestParam Long applicantDataId) {
        Map<String, Object> result = validationService.validateInternalRefNoApp(internalRefNo, applicantDataId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/aadhaar")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateAadhaarCorporates(
            @RequestParam String aadhaarNo,
            @RequestParam String pan,
            @RequestParam(required = false) String urn) {
        Map<String, Object> result = validationService.validateAadhaarCorporates(aadhaarNo, pan, urn);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/aadhaar-app")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateAadhaarCorporatesApp(
            @RequestParam String aadhaarNo,
            @RequestParam String pan,
            @RequestParam Long applicantDataId) {
        Map<String, Object> result = validationService.validateAadhaarCorporatesApp(aadhaarNo, pan, applicantDataId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/email")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateEmailCorporates(
            @RequestParam String emailId,
            @RequestParam Long applicantId,
            @RequestParam String pan) {
        Map<String, Object> result = validationService.validateEmailCorporates(emailId, applicantId, pan);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/email-app")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateEmailCorporatesApp(
            @RequestParam String emailId,
            @RequestParam Long applicantDataId,
            @RequestParam String pan) {
        Map<String, Object> result = validationService.validateEmailCorporatesApp(emailId, applicantDataId, pan);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/validate/corporate/whatsapp-mod")
    public ResponseEntity<Map<String, Object>> validateWhatsAppCorporatesForMod(
            @RequestParam String urn,
            @RequestParam String mobileNo) {
        String message = validationService.validateWhatsAppCorporatesForMod(urn, mobileNo);
        boolean status = message == null || message.trim().isEmpty();
        return ResponseEntity.ok(Map.of(
            "success", status,
            "message", status ? "" : message
        ));
    }

    @PostMapping("/ac-for-dp")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getACforDP(
            @RequestParam Integer insurerId,
            @RequestParam Integer dpUserId,
            @RequestParam Integer acUserId) {
        Map<String, Object> result = validationService.getACforDP(insurerId, dpUserId, acUserId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/notifications")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getAllNotifications(
            @RequestParam(required = false, defaultValue = "-1") Integer notificationId) {
        Map<String, Object> result = validationService.getAllNotifications(notificationId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/tbx-schedule")
    public ResponseEntity<Map<String, Object>> getTbxSchedule() {
        Map<String, Object> result = validationService.getTbxSchedule();
        return ResponseEntity.ok(result);
    }

    @PostMapping("/roles")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getAllRoles(
            @RequestParam(required = false, defaultValue = "-1") Integer roleId) {
        Map<String, Object> result = validationService.getAllRoles(roleId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/roles/user-creation")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getRolesForUserCreation() {
        Map<String, Object> result = validationService.getRolesForUserCreation();
        return ResponseEntity.ok(result);
    }

    @PostMapping("/users")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getUsers(
            @RequestParam(required = false, defaultValue = "-1") Integer userId) {
        Map<String, Object> result = validationService.getUsers(userId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/users/all")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getAllUsers(
            @RequestParam(required = false, defaultValue = "-1") Integer userId) {
        Map<String, Object> result = validationService.getAllUsers(userId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/dp-for-insurer")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getDPForInsurer(
            @RequestParam Integer insurerId,
            @RequestParam Integer dpId) {
        Map<String, Object> result = validationService.getDPForInsurer(insurerId, dpId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/dp-for-insurer-ex")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getDPForInsurerEx(
            @RequestParam Integer insurerId,
            @RequestParam Integer dpId) {
        Map<String, Object> result = validationService.getDPForInsurerEx(insurerId, dpId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/insurer")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getInsurer() {
        Map<String, Object> result = validationService.getInsurer();
        return ResponseEntity.ok(result);
    }

    @PostMapping("/insurer2")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getInsurer2(
            @RequestParam(required = false, defaultValue = "-1") Integer insurerId) {
        Map<String, Object> result = validationService.getInsurer2(insurerId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/cor/basic-qualification")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getBasicQualificationForCOR(
            @RequestParam String corType) {
        Map<String, Object> result = validationService.getBasicQualificationForCOR(corType);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/cor/pro-qualification")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getProQualificationForCOR(
            @RequestParam String corType) {
        Map<String, Object> result = validationService.getProQualificationForCOR(corType);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/cor/details")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getDetailsForCOR(
            @RequestParam String corType) {
        Map<String, Object> result = validationService.getDetailsForCOR(corType);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/centers/state")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamCenter>> getCentersForState(
            @RequestParam Integer stateId,
            @RequestParam Integer centerId) {
        List<ExamCenter> centers = examCenterService.getCentersForState(stateId, centerId);
        return ResponseEntity.ok(centers);
    }

    @GetMapping("/centers/state/{stateId}/district/{districtId}")
    public ResponseEntity<List<ExamCenter>> getCentersForStateAndDistrict(
            @PathVariable Integer stateId,
            @PathVariable Integer districtId) {
        List<ExamCenter> centers = examCenterService.getCentersForState(stateId, districtId);
        return ResponseEntity.ok(centers);
    }

    @PostMapping("/centers/state/ex")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getCentersForStateEx(
            @RequestParam Integer stateId,
            @RequestParam Integer centerId) {
        Map<String, Object> result = validationService.getCentersForStateEx(stateId, centerId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/centers/state/prelogin")
    public ResponseEntity<Map<String, Object>> getCentersForStatePreLogin(
            @RequestParam Integer stateId) {
        Map<String, Object> result = validationService.getCentersForStatePreLogin(stateId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/centers/save")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> saveExamCenter(
            @RequestBody Map<String, Object> examCenterData) {
        Map<String, Object> result = validationService.saveExamCenter(examCenterData);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/menu/data")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getMenuData(
            @RequestParam Integer searchId,
            @RequestParam Integer isRole) {
        Map<String, Object> result = validationService.getMenuData(searchId, isRole);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/menu/permission/save")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> saveMenuPermission(
            @RequestParam Integer searchId,
            @RequestParam Integer isRole,
            @RequestParam List<String> menuId,
            @RequestParam List<String> oldValue,
            @RequestParam List<String> newValue) {
        Map<String, Object> result = validationService.saveMenuPermission(searchId, isRole, menuId, oldValue, newValue);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/dp-range/data")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getDPRangeData() {
        Map<String, Object> result = validationService.getDPRangeData();
        return ResponseEntity.ok(result);
    }

    @PostMapping("/dp-range/save")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> saveDPRange(
            @RequestParam Integer insurerCode,
            @RequestParam Integer dpCount) {
        Map<String, Object> result = validationService.saveDPRange(insurerCode, dpCount);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/batch/payment-details")
    public ResponseEntity<Map<String, Object>> getBatchDetailsForPayment(
            @RequestParam String batchNo) {
        Map<String, Object> result = validationService.getBatchDetailsForPayment(batchNo);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/batch/management-details")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getBatchDetailsForMgmt(
            @RequestParam String batchNo) {
        Map<String, Object> result = validationService.getBatchDetailsForMgmt(batchNo);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/batch/list")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getBatchList(
            @RequestParam Map<String, String> searchParams) {
        Map<String, Object> result = validationService.getBatchList(searchParams);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/batch/delete")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> deleteBatch(
            @RequestParam String batchNo) {
        Map<String, Object> result = validationService.deleteBatch(batchNo);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/batch/delete-prob-urns")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> deleteProbURNs(
            @RequestParam String batchNo) {
        Map<String, Object> result = validationService.deleteProbURNs(batchNo);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/batch/verify")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> verifyBatch(
            @RequestParam String batchNo,
            @RequestParam String paymentGateway) {
        Map<String, Object> result = validationService.verifyBatch(batchNo, paymentGateway);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/company/payment-modes")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> getCompanyPaymentModes(
            @RequestParam String companyType,
            @RequestParam String companyLoginId) {
        Map<String, Object> result = validationService.getCompanyPaymentModes(companyType, companyLoginId);
        return ResponseEntity.ok(result);
    }

    @PostMapping("/company/payment-modes/save")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> saveCompanyPaymentModes(
            @RequestBody Map<String, Object> paymentModesData) {
        Map<String, Object> result = validationService.saveCompanyPaymentModes(paymentModesData);
        return ResponseEntity.ok(result);
    }
} 