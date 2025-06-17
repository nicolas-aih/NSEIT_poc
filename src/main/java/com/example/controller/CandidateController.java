package com.example.controller;

import com.example.model.Candidate;
import com.example.model.ExamDetail;
import com.example.model.URNRequest;
import com.example.service.CandidateService;
import com.example.dto.CandidateRegistrationDto;
import org.springframework.core.io.Resource;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/candidates")
public class CandidateController {

    private final CandidateService candidateService;

    public CandidateController(CandidateService candidateService) {
        this.candidateService = candidateService;
    }

    // Scorecard Management
    @GetMapping("/scorecard")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Resource> downloadScorecard(
            @RequestParam String urn,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dob) {
        Resource scorecard = candidateService.generateScorecard(urn, dob);
        return ResponseEntity.ok()
                .contentType(MediaType.APPLICATION_PDF)
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"scorecard.pdf\"")
                .body(scorecard);
    }

    // URN Management
    @GetMapping("/urn/{urn}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Candidate> getCandidateByURN(@PathVariable String urn) {
        return ResponseEntity.ok(candidateService.getCandidateByURN(urn));
    }

    @PostMapping("/urn/modify")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> modifyURN(
            @RequestBody URNRequest request) {
        return ResponseEntity.ok(candidateService.modifyURN(request));
    }

    @GetMapping("/urn/pending")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<List<URNRequest>> getPendingURNRequests(
            @RequestParam(defaultValue = "PENDING") String status) {
        return ResponseEntity.ok(candidateService.getPendingURNRequests(status));
    }

    @PostMapping("/urn/approve/{id}")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<String, Object>> approveURNRequest(
            @PathVariable Long id,
            @RequestParam String remarks) {
        return ResponseEntity.ok(candidateService.approveURNRequest(id, remarks));
    }

    @PostMapping("/urn/reject/{id}")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<String, Object>> rejectURNRequest(
            @PathVariable Long id,
            @RequestParam String remarks) {
        return ResponseEntity.ok(candidateService.rejectURNRequest(id, remarks));
    }

    // Exam Management
    @GetMapping("/exam/{urn}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ExamDetail> getExamDetails(@PathVariable String urn) {
        return ResponseEntity.ok(candidateService.getExamDetails(urn));
    }

    @PostMapping("/exam")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<String, Object>> saveExamDetails(
            @RequestBody ExamDetail examDetail) {
        return ResponseEntity.ok(candidateService.saveExamDetails(examDetail));
    }

    @PostMapping("/exam/results/upload")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<String, Object>> uploadExamResults(
            @RequestParam MultipartFile file,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime toDate) {
        return ResponseEntity.ok(candidateService.uploadExamResults(file, fromDate, toDate));
    }

    // PAN Lookup
    @GetMapping("/pan/{panNumber}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<Candidate>> lookupByPAN(@PathVariable String panNumber) {
        return ResponseEntity.ok(candidateService.lookupByPAN(panNumber));
    }

    // Bulk Operations
    @PostMapping("/training/bulk")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<String, Object>> bulkUploadTrainingDetails(
            @RequestParam MultipartFile file) {
        return ResponseEntity.ok(candidateService.bulkUploadTrainingDetails(file));
    }

    @PostMapping("/register")
    public ResponseEntity<Map<String, Object>> registerCandidate(@RequestBody CandidateRegistrationDto dto) {
        return ResponseEntity.ok(candidateService.registerCandidate(dto));
    }
} 