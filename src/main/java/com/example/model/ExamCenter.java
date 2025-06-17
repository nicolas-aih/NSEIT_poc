package com.example.model;

import jakarta.persistence.*;
import lombok.Data;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "exam_centers")
public class ExamCenter {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(name = "center_code", nullable = false, unique = true)
    private String centerCode;
    
    @Column(nullable = false)
    private String name;
    
    @Column(nullable = false)
    private String address;
    
    @Column(name = "state_id", nullable = false)
    private Integer stateId;
    
    @Column(name = "state_name", nullable = false)
    private String stateName;
    
    @Column(name = "district_id", nullable = false)
    private Integer districtId;
    
    @Column(name = "district_name", nullable = false)
    private String districtName;
    
    @Column(nullable = false)
    private String pincode;
    
    @Column(nullable = false)
    private Integer capacity;
    
    @Column(name = "contact_person", nullable = false)
    private String contactPerson;
    
    @Column(name = "contact_phone", nullable = false)
    private String contactPhone;
    
    @Column(name = "contact_email", nullable = false)
    private String contactEmail;
    
    @Column(name = "max_capacity", nullable = false)
    private Integer maxCapacity;
    
    @Column(name = "min_capacity", nullable = false)
    private Integer minCapacity;
    
    @Column(name = "registration_fee", nullable = false)
    private BigDecimal registrationFee;
    
    @Column(name = "exam_fee", nullable = false)
    private BigDecimal examFee;
    
    @Column(nullable = false)
    private String status;
    
    @Column(name = "latitude")
    private Double latitude;
    
    @Column(name = "longitude")
    private Double longitude;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @Column(name = "updated_at")
    private LocalDateTime updatedAt;
    
    @Column(name = "created_by")
    private String createdBy;
    
    @Column(name = "updated_by")
    private String updatedBy;
    
    @Transient
    private Double distance;
    
    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
        updatedAt = LocalDateTime.now();
        if (status == null) {
            status = "ACTIVE";
        }
    }
    
    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
} 