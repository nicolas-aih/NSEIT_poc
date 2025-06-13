package com.example.service.impl;

import com.example.model.ScheduleRequest;
import com.example.model.ScheduleResponse;
import com.example.service.SchedulerService;
import com.example.integration.CSSIntegrationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class SchedulerServiceImpl implements SchedulerService {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Autowired
    private CSSIntegrationService cssIntegrationService;

    @Value("${app.css.client-id}")
    private String cssClientId;

    @Value("${app.exam.duration}")
    private Integer examDuration;

    @Override
    public ScheduleResponse getCandidateDetails(String urn) {
        try {
            String sql = "EXEC sp_get_candidate_details ?";
            List<Map<String, Object>> results = jdbcTemplate.queryForList(sql, urn);
            
            if (results != null && !results.isEmpty()) {
                return new ScheduleResponse(true, "", results);
            }
            return new ScheduleResponse(false, "No candidate found with URN: " + urn);
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error retrieving candidate details: " + e.getMessage());
        }
    }

    @Override
    public ScheduleResponse getCandidateDetailsForRescheduling(String urn) {
        try {
            String sql = "EXEC sp_get_candidate_details_for_rescheduling ?";
            List<Map<String, Object>> results = jdbcTemplate.queryForList(sql, urn);
            
            if (results != null && !results.isEmpty()) {
                return new ScheduleResponse(true, "", results);
            }
            return new ScheduleResponse(false, "No candidate found for rescheduling with URN: " + urn);
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error retrieving candidate details: " + e.getMessage());
        }
    }

    @Override
    @Transactional
    public ScheduleResponse scheduleSeat(ScheduleRequest request) {
        try {
            if (!validateSchedulingRequest(request)) {
                return new ScheduleResponse(false, "Invalid scheduling request");
            }

            // Get candidate details
            ScheduleResponse candidateResponse = getCandidateDetails(request.getUrn());
            if (!candidateResponse.isSuccess()) {
                return candidateResponse;
            }

            // Prepare data for CSS integration
            Map<String, Object> schedulingData = new HashMap<>();
            schedulingData.put("center_id", request.getCenterId());
            schedulingData.put("preferred_test_date", request.getPreferredDate());
            schedulingData.put("exam_duration", examDuration);
            schedulingData.put("from_time", request.getStartTime());
            schedulingData.putAll(candidateResponse.getData().get(0));

            // Call CSS integration service
            Map<String, String> cssResponse = cssIntegrationService.bookSeat(
                cssClientId, 
                schedulingData,
                request.getRemarks()
            );

            if ("SUCCESS".equals(cssResponse.get("status"))) {
                // Update local database
                String sql = "EXEC sp_update_booking_status ?, ?, ?, ?, ?, ?, ?";
                jdbcTemplate.update(sql,
                    request.getUrn(),
                    cssResponse.get("clientSideIdentifier"),
                    cssResponse.get("cssReferenceNumber"),
                    request.getPreferredDate(),
                    request.getStartTime(),
                    request.getCenterId(),
                    request.getLanguageId()
                );

                return new ScheduleResponse(true, "Seat booked successfully");
            }
            return new ScheduleResponse(false, cssResponse.get("message"));
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error scheduling seat: " + e.getMessage());
        }
    }

    @Override
    @Transactional
    public ScheduleResponse rescheduleSeat(ScheduleRequest request) {
        try {
            if (!validateSchedulingRequest(request)) {
                return new ScheduleResponse(false, "Invalid rescheduling request");
            }

            // Get candidate details
            ScheduleResponse candidateResponse = getCandidateDetailsForRescheduling(request.getUrn());
            if (!candidateResponse.isSuccess()) {
                return candidateResponse;
            }

            // Prepare data for CSS integration
            Map<String, Object> reschedulingData = new HashMap<>();
            reschedulingData.put("center_id", request.getCenterId());
            reschedulingData.put("preferred_test_date", request.getPreferredDate());
            reschedulingData.put("exam_duration", examDuration);
            reschedulingData.put("from_time", request.getStartTime());
            reschedulingData.putAll(candidateResponse.getData().get(0));

            // Call CSS integration service
            Map<String, String> cssResponse = cssIntegrationService.rescheduleSeat(
                cssClientId,
                reschedulingData,
                request.getRemarks()
            );

            if ("SUCCESS".equals(cssResponse.get("status"))) {
                // Update local database
                String sql = "EXEC sp_update_rescheduling_status ?, ?, ?, ?, ?, ?";
                jdbcTemplate.update(sql,
                    request.getUrn(),
                    request.getPreferredDate(),
                    request.getStartTime(),
                    request.getCenterId(),
                    cssResponse.get("cssReferenceNumber"),
                    request.getRemarks()
                );

                return new ScheduleResponse(true, "Seat rescheduled successfully");
            }
            return new ScheduleResponse(false, cssResponse.get("message"));
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error rescheduling seat: " + e.getMessage());
        }
    }

    @Override
    @Transactional
    public ScheduleResponse cancelSeat(String urn, String remarks) {
        try {
            // Get candidate details
            ScheduleResponse candidateResponse = getCandidateDetailsForRescheduling(urn);
            if (!candidateResponse.isSuccess()) {
                return candidateResponse;
            }

            // Call CSS integration service
            Map<String, String> cssResponse = cssIntegrationService.cancelSeat(
                cssClientId,
                candidateResponse.getData().get(0),
                remarks
            );

            if ("SUCCESS".equals(cssResponse.get("status"))) {
                // Update local database
                String sql = "EXEC sp_update_cancellation_status ?, ?";
                jdbcTemplate.update(sql, urn, remarks);

                return new ScheduleResponse(true, "Seat cancelled successfully");
            }
            return new ScheduleResponse(false, cssResponse.get("message"));
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error cancelling seat: " + e.getMessage());
        }
    }

    @Override
    public ScheduleResponse getDatesForCenter(Integer centerId) {
        try {
            List<Map<String, Object>> availableDates = cssIntegrationService.getAvailableDates(
                cssClientId,
                centerId,
                examDuration
            );
            
            return new ScheduleResponse(true, "", availableDates);
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error retrieving dates: " + e.getMessage());
        }
    }

    @Override
    public ScheduleResponse getBatchesForCenterDate(Integer centerId, LocalDateTime preferredDate) {
        try {
            List<Map<String, Object>> batches = cssIntegrationService.getAvailableBatches(
                cssClientId,
                centerId,
                preferredDate,
                examDuration
            );
            
            return new ScheduleResponse(true, "", batches);
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error retrieving batches: " + e.getMessage());
        }
    }

    @Override
    public ScheduleResponse getScheduledBatchesForCenterDate(Integer centerId, LocalDateTime preferredDate) {
        try {
            String sql = "EXEC sp_get_scheduled_batches ?, ?";
            List<Map<String, Object>> results = jdbcTemplate.queryForList(sql, centerId, preferredDate);
            
            return new ScheduleResponse(true, "", results);
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error retrieving scheduled batches: " + e.getMessage());
        }
    }

    @Override
    public ScheduleResponse getScheduledBatchCountForCenterDate(Integer centerId, LocalDateTime preferredDate, String slot) {
        try {
            String sql = "EXEC sp_get_scheduled_batch_count ?, ?, ?";
            List<Map<String, Object>> results = jdbcTemplate.queryForList(sql, centerId, preferredDate, slot);
            
            return new ScheduleResponse(true, "", results);
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error retrieving batch count: " + e.getMessage());
        }
    }

    @Override
    @Transactional
    public ScheduleResponse rescheduleBatch(ScheduleRequest request) {
        try {
            // Get candidates in batch
            String sql = "EXEC sp_get_candidates_in_batch ?, ?, ?";
            List<Map<String, Object>> candidates = jdbcTemplate.queryForList(sql, 
                request.getCurrentCenter(),
                request.getCurrentDate(),
                request.getCurrentSlot()
            );

            if (candidates.isEmpty()) {
                return new ScheduleResponse(false, "No candidates found in the batch");
            }

            // Prepare batch data
            for (Map<String, Object> candidate : candidates) {
                candidate.put("center_id", request.getCenterId());
                candidate.put("preferred_test_date", request.getPreferredDate());
                candidate.put("exam_duration", examDuration);
                candidate.put("from_time", request.getPreferredSlot());
            }

            // Call CSS integration service
            Map<String, String> cssResponse = cssIntegrationService.rescheduleBatch(
                cssClientId,
                candidates,
                request.getRemarks()
            );

            if ("SUCCESS".equals(cssResponse.get("status"))) {
                // Update local database for each candidate
                String updateSql = "EXEC sp_update_batch_rescheduling_status ?, ?, ?, ?, ?, ?";
                for (Map<String, Object> candidate : candidates) {
                    jdbcTemplate.update(updateSql,
                        candidate.get("urn"),
                        request.getPreferredDate(),
                        request.getPreferredSlot(),
                        request.getCenterId(),
                        cssResponse.get("cssReferenceNumber"),
                        request.getRemarks()
                    );
                }

                return new ScheduleResponse(true, "Batch rescheduled successfully");
            }
            return new ScheduleResponse(false, cssResponse.get("message"));
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error rescheduling batch: " + e.getMessage());
        }
    }

    @Override
    @Transactional
    public ScheduleResponse cancelBatch(ScheduleRequest request) {
        try {
            // Get candidates in batch
            String sql = "EXEC sp_get_candidates_in_batch ?, ?, ?";
            List<Map<String, Object>> candidates = jdbcTemplate.queryForList(sql, 
                request.getCurrentCenter(),
                request.getCurrentDate(),
                request.getCurrentSlot()
            );

            if (candidates.isEmpty()) {
                return new ScheduleResponse(false, "No candidates found in the batch");
            }

            // Call CSS integration service
            Map<String, String> cssResponse = cssIntegrationService.cancelBatch(
                cssClientId,
                candidates,
                request.getRemarks()
            );

            if ("SUCCESS".equals(cssResponse.get("status"))) {
                // Update local database for each candidate
                String updateSql = "EXEC sp_update_batch_cancellation_status ?, ?";
                for (Map<String, Object> candidate : candidates) {
                    jdbcTemplate.update(updateSql,
                        candidate.get("urn"),
                        request.getRemarks()
                    );
                }

                return new ScheduleResponse(true, "Batch cancelled successfully");
            }
            return new ScheduleResponse(false, cssResponse.get("message"));
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error cancelling batch: " + e.getMessage());
        }
    }

    @Override
    @Transactional
    public ScheduleResponse reconcileBookings(LocalDateTime reconDate) {
        try {
            // Get data from CSS
            List<Map<String, Object>> cssBookings = cssIntegrationService.getBookingsForReconciliation(
                cssClientId,
                reconDate
            );

            if (cssBookings.isEmpty()) {
                return new ScheduleResponse(false, "No bookings found for reconciliation");
            }

            // Update local database
            String sql = "EXEC sp_reconcile_bookings ?";
            jdbcTemplate.update(sql, cssBookings);

            return new ScheduleResponse(true, "Bookings reconciled successfully");
        } catch (Exception e) {
            return new ScheduleResponse(false, "Error reconciling bookings: " + e.getMessage());
        }
    }

    @Override
    public List<Map<String, Object>> getAvailableSlots(Integer centerId, LocalDateTime date) {
        String sql = "EXEC sp_get_available_slots ?, ?";
        return jdbcTemplate.queryForList(sql, centerId, date);
    }

    @Override
    public boolean validateSchedulingRequest(ScheduleRequest request) {
        return request != null 
            && request.getCenterId() != null
            && request.getPreferredDate() != null
            && request.getStartTime() != null;
    }
} 