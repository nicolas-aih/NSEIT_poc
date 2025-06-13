package com.example.model;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ScheduleReport {
    private LocalDateTime examDate;
    private Integer centerId;
    private String centerName;
    private String examBatch;
    private Integer candidateCount;
} 