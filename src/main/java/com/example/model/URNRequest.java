package com.example.model;

import jakarta.persistence.*;
import lombok.Data;

import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "urn_requests")
public class URNRequest {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "candidate_id")
    private Candidate candidate;

    private String requestType; // MODIFICATION, DUPLICATE, etc.
    private String status; // PENDING, APPROVED, REJECTED
    private String remarks;
    private String modifiedFields;
    private String oldValues;
    private String newValues;

    private LocalDateTime requestedAt;
    private String requestedBy;
    private LocalDateTime processedAt;
    private String processedBy;

    @PrePersist
    protected void onCreate() {
        requestedAt = LocalDateTime.now();
    }

    @PreUpdate
    protected void onUpdate() {
        if (status.equals("APPROVED") || status.equals("REJECTED")) {
            processedAt = LocalDateTime.now();
        }
    }
} 