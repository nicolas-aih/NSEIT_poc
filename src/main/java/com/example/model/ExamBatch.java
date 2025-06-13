package com.example.model;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ExamBatch {
    private String batchId;
    private Integer centerId;
    private String centerName;
    private LocalDateTime examDate;
    private String slot;
    private Integer capacity;
    private Integer bookedSeats;
    private Integer availableSeats;
    private String status;
    private String remarks;
} 