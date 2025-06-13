package com.example.service;

import com.example.model.Batch;
import com.example.model.BatchApplicant;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@Service
public class BatchService {
    
    private final JdbcTemplate jdbcTemplate;

    @Value("${portal.connection-string}")
    private String connectionString;

    public BatchService(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public List<BatchApplicant> getTrainedApplicants(Integer examBodyId, Integer examCenterId, 
            LocalDateTime fromDate, LocalDateTime toDate, Integer userId) {
        String sql = "SELECT * FROM TrainedApplicants WHERE ExamBodyId = ? AND ExamCenterId = ? " +
                    "AND CreatedDate BETWEEN ? AND ? AND IsActive = 1";
        
        return jdbcTemplate.query(sql, 
            (rs, rowNum) -> {
                BatchApplicant applicant = new BatchApplicant();
                applicant.setUrn(rs.getString("URN"));
                applicant.setEmailId(rs.getString("EmailId"));
                applicant.setStatus(rs.getString("Status"));
                applicant.setRemarks(rs.getString("Remarks"));
                return applicant;
            },
            examBodyId, examCenterId, fromDate, toDate
        );
    }

    @Transactional
    public Map<String, Object> updateApplicantDetails(List<BatchApplicant> applicants, String paymentMode, 
            String batchMode, String schedulingMode, String remarks) {
        // Validate batch mode
        if ("Credit".equals(paymentMode)) {
            batchMode = "BULK";
        }
        batchMode = "1".equals(batchMode) ? "BULK" : "SINGLE";
        schedulingMode = "1".equals(schedulingMode) ? "AUTO" : "SELF";

        // Create new batch
        String batchNo = generateBatchNumber();
        String sql = "EXEC sp_CreateBatch @BatchNo = ?, @PaymentMode = ?, @BatchMode = ?, " +
                    "@SchedulingMode = ?, @Remarks = ?";
        
        jdbcTemplate.update(sql, batchNo, paymentMode, batchMode, schedulingMode, remarks);

        // Add applicants to batch
        for (BatchApplicant applicant : applicants) {
            sql = "EXEC sp_AddApplicantToBatch @BatchNo = ?, @URN = ?, @EmailId = ?, " +
                  "@OnOrAfterDate = ?, @PaymentMode = ?, @BatchMode = ?, @SchedulingMode = ?";
            
            jdbcTemplate.update(sql,
                batchNo,
                applicant.getUrn(),
                applicant.getEmailId(),
                applicant.getOnOrAfterDate(),
                paymentMode,
                batchMode,
                schedulingMode
            );
        }

        return Map.of(
            "success", true,
            "batchNo", batchNo,
            "message", "Batch created successfully"
        );
    }

    public List<Map<String, Object>> getExamCenters(Integer examBodyId) {
        String sql = "SELECT * FROM ExamCenters WHERE ExamBodyId = ? AND IsActive = 1";
        return jdbcTemplate.queryForList(sql, examBodyId);
    }

    private String generateBatchNumber() {
        return "B" + System.currentTimeMillis();
    }

    public List<Batch> getBatchList(String searchType, String batchNo, 
            LocalDateTime fromDate, LocalDateTime toDate, Integer status, Integer userId) {
        String sql;
        Object[] params;

        if ("BATCHNO".equals(searchType)) {
            sql = "SELECT * FROM Batches WHERE BatchNo = ? AND UserId = ?";
            params = new Object[]{batchNo, userId};
        } else {
            sql = "SELECT * FROM Batches WHERE CreatedDate BETWEEN ? AND ? " +
                  "AND (? = -1 OR Status = ?) AND UserId = ?";
            params = new Object[]{fromDate, toDate, status, status, userId};
        }

        return jdbcTemplate.query(sql, 
            (rs, rowNum) -> {
                Batch batch = new Batch();
                batch.setBatchNo(rs.getString("BatchNo"));
                batch.setPaymentMode(rs.getString("PaymentMode"));
                batch.setBatchMode(rs.getString("BatchMode"));
                batch.setSchedulingMode(rs.getString("SchedulingMode"));
                batch.setCreatedDate(rs.getTimestamp("CreatedDate").toLocalDateTime());
                batch.setStatus(rs.getString("Status"));
                batch.setPaymentStatus(rs.getString("PaymentStatus"));
                batch.setTotalAmount(rs.getDouble("TotalAmount"));
                batch.setTotalCandidates(rs.getInt("TotalCandidates"));
                return batch;
            },
            params
        );
    }

    @Transactional
    public String deleteBatch(String batchNo, Integer userId) {
        String sql = "EXEC sp_DeleteBatch @BatchNo = ?, @UserId = ?";
        jdbcTemplate.update(sql, batchNo, userId);
        return "Batch deleted successfully";
    }
} 