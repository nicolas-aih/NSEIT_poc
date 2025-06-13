package com.example.model;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ExamReport {
    private LocalDateTime applicationDate;
    private String urn;
    private String applicantName;
    private LocalDateTime examDate;
    private String examRollNumber;
    private Integer examMarks;
    private String result;
    private LocalDateTime trainingStartDate;
    private LocalDateTime trainingEndDate;
    private LocalDateTime tccExpiryDate;
    private Integer totalTrainingHours;
    private String centerName;
    private LocalDateTime preferredDate;
    private String paymentMode;
    private String schedulingMode;
} 