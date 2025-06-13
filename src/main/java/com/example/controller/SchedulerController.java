package com.example.controller;

import com.example.model.ScheduleRequest;
import com.example.model.ScheduleResponse;
import com.example.service.SchedulerService;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;

@RestController
@RequestMapping("/api/scheduler")
public class SchedulerController {

    private final SchedulerService schedulerService;

    public SchedulerController(SchedulerService schedulerService) {
        this.schedulerService = schedulerService;
    }

    // Individual candidate scheduling endpoints
    @PostMapping("/candidate/details")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> getCandidateDetails(@RequestParam String urn) {
        return ResponseEntity.ok(schedulerService.getCandidateDetails(urn));
    }

    @PostMapping("/candidate/details/reschedule")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> getCandidateDetailsForRescheduling(@RequestParam String urn) {
        return ResponseEntity.ok(schedulerService.getCandidateDetailsForRescheduling(urn));
    }

    @PostMapping("/seat/schedule")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> scheduleSeat(@RequestBody ScheduleRequest request) {
        return ResponseEntity.ok(schedulerService.scheduleSeat(request));
    }

    @PostMapping("/seat/reschedule")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> rescheduleSeat(@RequestBody ScheduleRequest request) {
        return ResponseEntity.ok(schedulerService.rescheduleSeat(request));
    }

    @PostMapping("/seat/cancel")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> cancelSeat(
            @RequestParam String urn,
            @RequestParam String remarks) {
        return ResponseEntity.ok(schedulerService.cancelSeat(urn, remarks));
    }

    // Center and batch management endpoints
    @GetMapping("/center/{centerId}/dates")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> getDatesForCenter(@PathVariable Integer centerId) {
        return ResponseEntity.ok(schedulerService.getDatesForCenter(centerId));
    }

    @GetMapping("/center/{centerId}/batches")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> getBatchesForCenterDate(
            @PathVariable Integer centerId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime preferredDate) {
        return ResponseEntity.ok(schedulerService.getBatchesForCenterDate(centerId, preferredDate));
    }

    @GetMapping("/center/{centerId}/scheduled-batches")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> getScheduledBatchesForCenterDate(
            @PathVariable Integer centerId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime preferredDate) {
        return ResponseEntity.ok(schedulerService.getScheduledBatchesForCenterDate(centerId, preferredDate));
    }

    @GetMapping("/center/{centerId}/batch-count")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<ScheduleResponse> getScheduledBatchCountForCenterDate(
            @PathVariable Integer centerId,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime preferredDate,
            @RequestParam String slot) {
        return ResponseEntity.ok(schedulerService.getScheduledBatchCountForCenterDate(centerId, preferredDate, slot));
    }

    @PostMapping("/batch/reschedule")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<ScheduleResponse> rescheduleBatch(@RequestBody ScheduleRequest request) {
        return ResponseEntity.ok(schedulerService.rescheduleBatch(request));
    }

    @PostMapping("/batch/cancel")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<ScheduleResponse> cancelBatch(@RequestBody ScheduleRequest request) {
        return ResponseEntity.ok(schedulerService.cancelBatch(request));
    }

    // Reconciliation endpoint
    @PostMapping("/reconcile")
    @PreAuthorize("hasRole('ADMIN')")
    public ResponseEntity<ScheduleResponse> reconcileBookings(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime reconDate) {
        return ResponseEntity.ok(schedulerService.reconcileBookings(reconDate));
    }
} 