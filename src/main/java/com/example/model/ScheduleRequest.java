package com.example.model;

import jakarta.persistence.*;
import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ScheduleRequest {
    private String urn;
    private Integer centerId;
    private LocalDateTime preferredDate;
    private String startTime;
    private String remarks;
    private Integer examDuration;
    private String languageId;
    private String slot;
    private Integer currentCenter;
    private LocalDateTime currentDate;
    private String currentSlot;
    private String preferredSlot;
} 