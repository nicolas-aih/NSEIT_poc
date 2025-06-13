package com.example.model;

import java.time.LocalDateTime;
import lombok.Data;

@Data
public class BatchApplicant {
    private String urn;
    private String enrollmentNo;
    private String emailId;
    private LocalDateTime onOrAfterDate;
    private String paymentMode;
    private String batchMode;
    private String schedulingMode;
    private String examBatchNo;
    private String status;
    private String remarks;
    private Boolean isValidRecord;
    private String uploadRemark;
} 