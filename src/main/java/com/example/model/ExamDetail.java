package com.example.model;

import jakarta.persistence.*;
import lombok.Data;

import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "exam_details")
public class ExamDetail {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "candidate_id")
    private Candidate candidate;

    private LocalDateTime examDate;
    private String examTime;
    private String examineeId;
    private Integer marks;
    private String result;
    private String status;

    // Training Details
    private LocalDateTime trainingStartDate;
    private LocalDateTime trainingEndDate;
    private Integer totalTrainingHours;
    private String trainingStatus;
    private String trainingCenter;
    private String trainer;

    // TCC Details
    private LocalDateTime tccExpiryDate;
    private String tccNumber;
    private String tccStatus;

    // Payment Details
    private String paymentMode;
    private String paymentStatus;
    private Double paymentAmount;
    private String transactionId;
    private LocalDateTime paymentDate;

    // Scheduling Details
    private String schedulingMode;
    private LocalDateTime preferredDate;
    private String preferredSlot;
    private String batchCode;

    private LocalDateTime createdAt;
    private String createdBy;
    private LocalDateTime updatedAt;
    private String updatedBy;

    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
        updatedAt = LocalDateTime.now();
    }

    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
} 