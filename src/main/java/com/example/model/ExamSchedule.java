package com.example.model;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ExamSchedule {
    private String urn;
    private Integer centerId;
    private String centerName;
    private LocalDateTime examDate;
    private String examBatch;
    private String fromTime;
    private Integer examDuration;
    private String status;
    private String remarks;
    private String schedulingMode;
    private String paymentMode;
    private String languageId;
    private String cssReferenceNumber;
} 