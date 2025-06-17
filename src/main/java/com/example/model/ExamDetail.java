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

    @Column(nullable = false, unique = true)
    private String urn;

    @Column(name = "applicant_name", nullable = false)
    private String applicantName;

    @Column(name = "exam_date")
    private LocalDateTime examDate;

    @Column(name = "exam_roll_number")
    private String examRollNumber;

    @Column(name = "exam_marks")
    private Integer examMarks;

    @Column(name = "result")
    private String result;

    @Column(name = "exam_center_id")
    private Long examCenterId;

    @Column(name = "status")
    private String status;

    @Column(name = "created_at")
    private LocalDateTime createdAt;

    @Column(name = "updated_at")
    private LocalDateTime updatedAt;

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