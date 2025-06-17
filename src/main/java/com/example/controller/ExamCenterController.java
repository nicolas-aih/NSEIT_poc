package com.example.controller;

import com.example.config.PortalSession;
import com.example.model.ApiResponse;
import com.example.model.ExamCenter;
import com.example.service.ExamCenterService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;
import java.util.Map;

@Controller
@RequestMapping("/api/exam-centers")
public class ExamCenterController {

    @Autowired
    private ExamCenterService examCenterService;

    @PostMapping("/list")
    @ResponseBody
    public ResponseEntity<ApiResponse> getExamCentersList() {
        try {
            List<ExamCenter> centers = examCenterService.getExamCentersForDownload();
            return ResponseEntity.ok(new ApiResponse(true, "Exam centers retrieved successfully", centers));
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "Error retrieving exam centers"));
        }
    }

    @PostMapping("/nearest")
    @ResponseBody
    public ResponseEntity<ApiResponse> findNearestCenters(@RequestParam Integer pincode) {
        try {
            List<ExamCenter> centers = examCenterService.findNearest3ExamCenters(pincode);
            return ResponseEntity.ok(new ApiResponse(true, "Nearest centers found", centers));
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "Error finding nearest centers"));
        }
    }

    @GetMapping("/by-district/{districtId}")
    @ResponseBody
    public ResponseEntity<ApiResponse> getCentersForDistrict(@PathVariable Integer districtId) {
        try {
            List<ExamCenter> centers = examCenterService.getCentersForDistrict(districtId);
            return ResponseEntity.ok(new ApiResponse(true, "Centers retrieved successfully", centers));
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "Error retrieving centers"));
        }
    }

    @GetMapping("/seats")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> getSeatsAvailability() {
        try {
            Object seatsData = examCenterService.getSeatsAvailability();
            return ResponseEntity.ok(new ApiResponse(true, "Seats availability retrieved", seatsData));
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "Error retrieving seats availability"));
        }
    }

    @GetMapping
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamCenter>> getAllExamCenters() {
        return ResponseEntity.ok(examCenterService.getAllExamCenters());
    }

    @GetMapping("/{id}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ExamCenter> getExamCenterById(@PathVariable Long id) {
        return ResponseEntity.ok(examCenterService.getExamCenterById(id));
    }

    @GetMapping("/state/{stateId}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamCenter>> getExamCentersByState(@PathVariable Long stateId) {
        return ResponseEntity.ok(examCenterService.getExamCentersByState(stateId));
    }

    @GetMapping("/district/{districtId}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamCenter>> getExamCentersByDistrict(@PathVariable Long districtId) {
        return ResponseEntity.ok(examCenterService.getExamCentersByDistrict(districtId));
    }

    @GetMapping("/nearest")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamCenter>> findNearestExamCenters(
            @RequestParam Integer pincode,
            @RequestParam Long examId) {
        return ResponseEntity.ok(examCenterService.findNearestExamCenters(pincode, examId));
    }

    @GetMapping("/similar/{centerId}")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<List<ExamCenter>> getSimilarCenters(@PathVariable Long centerId) {
        return ResponseEntity.ok(examCenterService.getSimilarCenters(centerId));
    }

    @PostMapping
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<ExamCenter> createExamCenter(@RequestBody ExamCenter examCenter) {
        return ResponseEntity.ok(examCenterService.createExamCenter(examCenter));
    }

    @PutMapping("/{id}")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<ExamCenter> updateExamCenter(
            @PathVariable Long id,
            @RequestBody ExamCenter examCenter) {
        return ResponseEntity.ok(examCenterService.updateExamCenter(id, examCenter));
    }

    @DeleteMapping("/{id}")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Void> deleteExamCenter(@PathVariable Long id) {
        examCenterService.deleteExamCenter(id);
        return ResponseEntity.noContent().build();
    }

    @GetMapping("/export")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Resource> exportExamCenters() {
        byte[] data = examCenterService.exportExamCenters();
        ByteArrayResource resource = new ByteArrayResource(data);
        
        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment;filename=exam_centers.xlsx")
                .contentType(MediaType.parseMediaType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                .contentLength(data.length)
                .body(resource);
    }

    @PostMapping("/bulk-update")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<String, Object>> bulkUpdateExamCenters(
            @RequestParam MultipartFile file) {
        return ResponseEntity.ok(examCenterService.bulkUpdateExamCenters(file));
    }

    @GetMapping("/pending-schedules")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<Map<Long, Integer>> getCenterWisePendingScheduleCount() {
        return ResponseEntity.ok(examCenterService.getCenterWisePendingScheduleCount());
    }
} 