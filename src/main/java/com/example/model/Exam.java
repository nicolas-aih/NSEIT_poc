package com.example.model;

import jakarta.persistence.*;
import lombok.Data;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "exams")
public class Exam {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "exam_code", nullable = false, unique = true)
    private String examCode;

    @Column(nullable = false)
    private String name;

    @Column(nullable = false)
    private String description;

    @Column(name = "start_date", nullable = false)
    private LocalDateTime startDate;

    @Column(name = "end_date", nullable = false)
    private LocalDateTime endDate;

    @Column(name = "registration_start_date", nullable = false)
    private LocalDateTime registrationStartDate;

    @Column(name = "registration_end_date", nullable = false)
    private LocalDateTime registrationEndDate;

    @Column(nullable = false)
    private Integer duration; // in minutes

    @Column(nullable = false)
    private BigDecimal fee;

    @Column(name = "max_candidates")
    private Integer maxCandidates;

    @Column(nullable = false)
    private String status; // DRAFT, PUBLISHED, ONGOING, COMPLETED, CANCELLED

    @Column(nullable = false)
    private String examType; // THEORY, PRACTICAL, BOTH

    @Column(nullable = false)
    private Boolean requiresDocuments;

    @Column(nullable = false)
    private String documentTypes; // Comma-separated list of required document types

    @Column(nullable = false)
    private String eligibilityCriteria;

    @Column(nullable = false)
    private String syllabus;

    @Column(nullable = false)
    private String instructions;

    @Column(name = "created_at")
    private LocalDateTime createdAt;

    @Column(name = "updated_at")
    private LocalDateTime updatedAt;

    @Column(name = "created_by")
    private String createdBy;

    @Column(name = "updated_by")
    private String updatedBy;

    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
        updatedAt = LocalDateTime.now();
        if (status == null) {
            status = "DRAFT";
        }
    }

    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
} 