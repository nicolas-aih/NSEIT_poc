package com.example.model;

import jakarta.persistence.*;
import lombok.Data;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "exam_registrations")
public class ExamRegistration {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "exam_id", nullable = false)
    private Exam exam;

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @ManyToOne
    @JoinColumn(name = "exam_center_id", nullable = false)
    private ExamCenter examCenter;

    @Column(name = "registration_number", nullable = false, unique = true)
    private String registrationNumber;

    @Column(name = "registration_date", nullable = false)
    private LocalDateTime registrationDate;

    @Column(nullable = false)
    private String status; // PENDING, APPROVED, REJECTED, CANCELLED

    @Column(nullable = false)
    private BigDecimal fee;

    @Column(name = "payment_status", nullable = false)
    private String paymentStatus; // PENDING, PAID, REFUNDED

    @Column(name = "payment_transaction_id")
    private String paymentTransactionId;

    @Column(name = "payment_date")
    private LocalDateTime paymentDate;

    @Column(name = "document_status", nullable = false)
    private String documentStatus; // PENDING, VERIFIED, REJECTED

    @Column(name = "exam_date", nullable = false)
    private LocalDateTime examDate;

    @Column(name = "exam_time_slot", nullable = false)
    private String examTimeSlot;

    @Column(name = "created_at")
    private LocalDateTime createdAt;

    @Column(name = "updated_at")
    private LocalDateTime updatedAt;

    @Column(name = "created_by")
    private String createdBy;

    @Column(name = "updated_by")
    private String updatedBy;

    @Column
    private String documentRemarks;

    @Column
    private String rollNumber;

    @Column
    private String seatNumber;

    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
        updatedAt = LocalDateTime.now();
        if (status == null) {
            status = "PENDING";
        }
        if (paymentStatus == null) {
            paymentStatus = "PENDING";
        }
        if (documentStatus == null) {
            documentStatus = "PENDING";
        }
    }

    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
} 