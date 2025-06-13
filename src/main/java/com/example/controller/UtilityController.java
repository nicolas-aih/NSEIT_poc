package com.example.controller;

import com.example.service.UtilityService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;

@RestController
@RequestMapping("/api/utility")
public class UtilityController {

    @Autowired
    private UtilityService utilityService;

    @GetMapping("/update-candidate-profile")
    public ResponseEntity<?> updateCandidateProfile() {
        return ResponseEntity.ok("Update Candidate Profile View Stub");
    }

    @PostMapping("/save-candidate-profile")
    public ResponseEntity<?> saveCandidateProfile(@RequestBody Map<String, Object> profileData) {
        return ResponseEntity.ok(utilityService.saveCandidateProfile(profileData));
    }

    @GetMapping("/company-lookup")
    public ResponseEntity<?> companyLookup() {
        return ResponseEntity.ok("Company Lookup View Stub");
    }

    @PostMapping("/load-company-details")
    public ResponseEntity<?> loadCompanyDetails(@RequestBody Map<String, Object> requestData) {
        return ResponseEntity.ok(utilityService.loadCompanyDetails(requestData));
    }

    @GetMapping("/get-user-password")
    public ResponseEntity<?> getUserPasswordView() {
        return ResponseEntity.ok("Get User Password View Stub");
    }

    @PostMapping("/get-user-password")
    public ResponseEntity<?> getUserPassword(@RequestBody Map<String, Object> requestData) {
        return ResponseEntity.ok(utilityService.getUserPassword(requestData));
    }
} 