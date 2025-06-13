package com.example.model;

import java.time.LocalDateTime;
import lombok.Data;

@Data
public class Batch {
    private String batchNo;
    private String paymentMode;
    private String batchMode;
    private String schedulingMode;
    private LocalDateTime createdDate;
    private String status;
    private String paymentStatus;
    private LocalDateTime paymentDate;
    private Double totalAmount;
    private Integer totalCandidates;
    private String remarks;
    private String examBatchNo;
    private Boolean isActive;
} 