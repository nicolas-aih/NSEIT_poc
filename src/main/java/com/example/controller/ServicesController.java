package com.example.controller;

import com.example.model.ExamCenter;
import com.example.service.ExamCenterService;
import com.example.service.ValidationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

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
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDateTime date) {
        boolean isValid = validationService.validateDateOfBirth(date);
        return ResponseEntity.ok(Map.of(
            "success", isValid,
            "message", isValid ? "" : "Age must be at least 18 years"
        ));
    }

    @PostMapping("/validate/yop")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateYearOfPassing(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDateTime date) {
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
        String message = validationService.validatePAN(pan, applicantId);
        return ResponseEntity.ok(Map.of(
            "success", message.isEmpty(),
            "message", message
        ));
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
    public ResponseEntity<Map<String, Object>> validateMobileForCorporates(
            @RequestParam String mobile,
            @RequestParam Long applicantId,
            @RequestParam String pan) {
        String message = validationService.validateMobileForCorporates(mobile, applicantId, pan);
        return ResponseEntity.ok(Map.of(
            "success", message.isEmpty(),
            "message", message
        ));
    }

    @PostMapping("/validate/corporate/mobile-app")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> validateMobileForCorporatesApp(
            @RequestParam String mobile,
            @RequestParam Long applicantDataId,
            @RequestParam String pan) {
        String message = validationService.validateMobileForCorporatesApp(mobile, applicantDataId, pan);
        return ResponseEntity.ok(Map.of(
            "success", message.isEmpty(),
            "message", message
        ));
    }

    @PostMapping("/centers/state")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<List<ExamCenter>> getCentersForState(
            @RequestParam Integer stateId,
            @RequestParam(required = false, defaultValue = "-1") Integer centerId) {
        return ResponseEntity.ok(examCenterService.getCentersForState(stateId, centerId));
    }

    @PostMapping("/centers/state/extended")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<List<ExamCenter>> getCentersForStateExtended(
            @RequestParam Integer stateId,
            @RequestParam(required = false, defaultValue = "-1") Integer centerId) {
        return ResponseEntity.ok(examCenterService.getCentersForStateExtended(stateId, centerId));
    }

    @PostMapping("/centers/similar")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<List<ExamCenter>> getSimilarCenters(
            @RequestParam Integer centerId) {
        return ResponseEntity.ok(examCenterService.getSimilarCenters(centerId));
    }

    @PostMapping("/centers/nearest")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<ExamCenter> findNearestExamCenter(
            @RequestParam String pinCode) {
        return ResponseEntity.ok(examCenterService.findNearestExamCenter(pinCode));
    }

    @PostMapping("/centers")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<ExamCenter> saveExamCenter(
            @RequestBody ExamCenter examCenter) {
        return ResponseEntity.ok(examCenterService.saveExamCenter(examCenter));
    }

    @PostMapping("/centers/available-seats")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<List<ExamCenter>> getAvailableSeats(
            @RequestParam(required = false) Integer stateId,
            @RequestParam(required = false) Integer centerId,
            @RequestParam String examDate) {
        return ResponseEntity.ok(examCenterService.getAvailableSeats(stateId, centerId, examDate));
    }
} 