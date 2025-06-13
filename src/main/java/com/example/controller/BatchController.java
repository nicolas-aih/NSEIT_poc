package com.example.controller;

import com.example.model.Batch;
import com.example.model.BatchApplicant;
import com.example.service.BatchService;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/batches")
public class BatchController {

    private final BatchService batchService;

    public BatchController(BatchService batchService) {
        this.batchService = batchService;
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @GetMapping("/exam-registration")
    public ResponseEntity<Map<String, Object>> examRegistration() {
        return ResponseEntity.ok(Map.of(
            "isLoggedOn", true,
            "className", "col-sm-9"
        ));
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @GetMapping("/manage")
    public ResponseEntity<Map<String, Object>> manageBatches() {
        return ResponseEntity.ok(Map.of(
            "isLoggedOn", true,
            "className", "col-sm-9"
        ));
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @GetMapping("/trained-applicants")
    public ResponseEntity<List<BatchApplicant>> getTrainedApplicants(
            @RequestParam Integer examBodyId,
            @RequestParam Integer examCenterId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime toDate,
            @RequestParam Integer userId) {
        
        List<BatchApplicant> applicants = batchService.getTrainedApplicants(
            examBodyId, examCenterId, fromDate, toDate, userId);
        return ResponseEntity.ok(applicants);
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @PostMapping("/applicants")
    public ResponseEntity<Map<String, Object>> updateApplicantDetails(
            @RequestBody List<BatchApplicant> applicants,
            @RequestParam String paymentMode,
            @RequestParam String batchMode,
            @RequestParam String schedulingMode,
            @RequestParam(required = false) String remarks) {
        
        Map<String, Object> result = batchService.updateApplicantDetails(
            applicants, paymentMode, batchMode, schedulingMode, remarks);
        return ResponseEntity.ok(result);
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @GetMapping("/exam-centers/{examBodyId}")
    public ResponseEntity<List<Map<String, Object>>> getExamCenters(
            @PathVariable Integer examBodyId) {
        List<Map<String, Object>> centers = batchService.getExamCenters(examBodyId);
        return ResponseEntity.ok(centers);
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @GetMapping("/list")
    public ResponseEntity<List<Batch>> getBatchList(
            @RequestParam String searchType,
            @RequestParam(required = false) String batchNo,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime fromDate,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime toDate,
            @RequestParam(defaultValue = "-1") Integer status,
            @RequestParam Integer userId) {
        
        List<Batch> batches = batchService.getBatchList(searchType, batchNo, fromDate, toDate, status, userId);
        return ResponseEntity.ok(batches);
    }

    @PreAuthorize("hasRole('EXAMINER')")
    @DeleteMapping("/{batchNo}")
    public ResponseEntity<Map<String, Object>> deleteBatch(
            @PathVariable String batchNo,
            @RequestParam Integer userId) {
        
        String message = batchService.deleteBatch(batchNo, userId);
        return ResponseEntity.ok(Map.of(
            "success", true,
            "message", message
        ));
    }
} 