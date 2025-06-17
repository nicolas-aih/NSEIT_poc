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

    @Column(nullable = false, unique = true)
    private String urn;

    @Column(name = "applicant_name", nullable = false)
    private String applicantName;

    @Column(nullable = false)
    private String email;

    @Column(name = "phone_number", nullable = false)
    private String phoneNumber;

    @Column(name = "date_of_birth")
    private LocalDateTime dateOfBirth;

    @Column(name = "address")
    private String address;

    @Column(name = "state_id")
    private Integer stateId;

    @Column(name = "district_id")
    private Integer districtId;

    @Column(name = "pincode")
    private String pincode;

    @Column(name = "status", nullable = false)
    private String status;

    @Column(name = "remarks")
    private String remarks;

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
            status = "PENDING";
        }
    }

    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
} 