package com.example.service;

import com.example.model.ExamSchedule;
import com.example.model.ExamBatch;
import com.example.model.ScheduleRequest;
import com.example.model.ScheduleResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

public interface SchedulerService {
    // Candidate scheduling operations
    ScheduleResponse getCandidateDetails(String urn);
    ScheduleResponse getCandidateDetailsForRescheduling(String urn);
    ScheduleResponse scheduleSeat(ScheduleRequest request);
    ScheduleResponse rescheduleSeat(ScheduleRequest request);
    ScheduleResponse cancelSeat(String urn, String remarks);
    
    // Batch operations
    ScheduleResponse getDatesForCenter(Integer centerId);
    ScheduleResponse getBatchesForCenterDate(Integer centerId, LocalDateTime preferredDate);
    ScheduleResponse getScheduledBatchesForCenterDate(Integer centerId, LocalDateTime preferredDate);
    ScheduleResponse getScheduledBatchCountForCenterDate(Integer centerId, LocalDateTime preferredDate, String slot);
    ScheduleResponse rescheduleBatch(ScheduleRequest request);
    ScheduleResponse cancelBatch(ScheduleRequest request);
    
    // Reconciliation
    ScheduleResponse reconcileBookings(LocalDateTime reconDate);
    
    // Helper methods
    List<Map<String, Object>> getAvailableSlots(Integer centerId, LocalDateTime date);
    boolean validateSchedulingRequest(ScheduleRequest request);
} 