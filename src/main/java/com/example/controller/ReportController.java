package com.example.controller;

import com.example.model.ExamReport;
import com.example.model.ScheduleReport;
import com.example.service.ReportService;
import org.springframework.core.io.Resource;
import org.springframework.core.io.UrlResource;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/reports")
public class ReportController {

    private final ReportService reportService;

    public ReportController(ReportService reportService) {
        this.reportService = reportService;
    }

    @GetMapping("/dashboard")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> reportsDashboard() {
        return ResponseEntity.ok(Map.of(
            "isLoggedOn", true,
            "className", "col-sm-9"
        ));
    }

    @GetMapping("/corporate-examination")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamReport>> getCorporateExaminationReport(
            @RequestParam String examMonth,
            @RequestParam Integer examYear,
            @RequestParam String roleName) {
        
        List<ExamReport> reports = reportService.getCorporateExaminationReport(
            examMonth, examYear, roleName);
        return ResponseEntity.ok(reports);
    }

    @GetMapping("/examination")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamReport>> getExaminationReport(
            @RequestParam Integer option,
            @RequestParam Integer userId) {
        
        List<ExamReport> reports = reportService.getExaminationReport(option, userId);
        return ResponseEntity.ok(reports);
    }

    @GetMapping("/schedule")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ScheduleReport>> getScheduleReport(
            @RequestParam Integer hint,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime toDate) {
        
        List<ScheduleReport> reports = reportService.getScheduleReport(hint, fromDate, toDate);
        return ResponseEntity.ok(reports);
    }

    @GetMapping("/center-wise-pending")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<Map<String, Object>>> getCenterWisePendingScheduleCount() {
        List<Map<String, Object>> reports = reportService.getCenterWisePendingScheduleCount();
        return ResponseEntity.ok(reports);
    }

    @GetMapping("/payment")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<Map<String, Object>>> getPaymentReport(
            @RequestParam String userLoginId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime fromDate,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime toDate) {
        
        List<Map<String, Object>> reports = reportService.getPaymentReport(userLoginId, fromDate, toDate);
        return ResponseEntity.ok(reports);
    }

    @GetMapping("/sponsorship")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<Map<String, Object>>> getSponsorshipReport(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime applicationDateFrom,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime applicationDateTo,
            @RequestParam String roleName,
            @RequestParam Integer insurerUserId,
            @RequestParam Integer dpUserId,
            @RequestParam Integer acUserId,
            @RequestParam(required = false) String urn,
            @RequestParam(required = false) String insurerRefNo,
            @RequestParam(required = false) String examBatch,
            @RequestParam(required = false) Integer examBodyId,
            @RequestParam(required = false) Integer examCenterId,
            @RequestParam(defaultValue = "false") boolean statusAll,
            @RequestParam(defaultValue = "false") boolean statusSponsored,
            @RequestParam(defaultValue = "false") boolean statusTrained,
            @RequestParam(defaultValue = "false") boolean statusEc,
            @RequestParam(defaultValue = "false") boolean statusEa,
            @RequestParam(defaultValue = "false") boolean statusE,
            @RequestParam(defaultValue = "false") boolean photo,
            @RequestParam(defaultValue = "false") boolean sign,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime examDateFrom,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime examDateTo) {
        
        List<Map<String, Object>> reports = reportService.getSponsorshipReport(
            applicationDateFrom, applicationDateTo, roleName, insurerUserId, dpUserId,
            acUserId, urn, insurerRefNo, examBatch, examBodyId, examCenterId,
            statusAll, statusSponsored, statusTrained, statusEc, statusEa, statusE,
            photo, sign, examDateFrom, examDateTo);
        return ResponseEntity.ok(reports);
    }

    @GetMapping("/download/{fileName}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Resource> downloadReport(@PathVariable String fileName) {
        try {
            Path filePath = Paths.get(reportService.getReportsDumpPath()).resolve(fileName);
            Resource resource = new UrlResource(filePath.toUri());

            if (resource.exists()) {
                return ResponseEntity.ok()
                    .contentType(MediaType.APPLICATION_OCTET_STREAM)
                    .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + fileName + "\"")
                    .body(resource);
            } else {
                return ResponseEntity.notFound().build();
            }
        } catch (Exception e) {
            return ResponseEntity.internalServerError().build();
        }
    }
} 