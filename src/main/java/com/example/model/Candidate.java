package com.example.model;

import jakarta.persistence.*;
import lombok.Data;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "candidates")
public class Candidate {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(unique = true)
    private String urn;

    private String salutation;
    private String name;
    private String fatherName;
    private String category;

    // Address Information
    private String currentHouseNumber;
    private String currentStreet;
    private String currentTown;
    private String currentDistrict;
    private String currentState;
    private String currentPincode;

    private String permanentHouseNumber;
    private String permanentStreet;
    private String permanentTown;
    private String permanentDistrict;
    private String permanentState;
    private String permanentPincode;

    private String area;

    // Educational Information
    private String basicQualification;
    private String boardName;
    private String rollNumber;
    private LocalDate yearOfPassing;
    private String professionalQualification;
    private String otherQualification;

    // Personal Information
    private LocalDate dateOfBirth;
    private String gender;
    private String primaryOccupation;
    private String employeeCode;

    // Contact Information
    private String landlineNumber;
    private String mobileNumber;
    private String email;
    private String contactPersonEmail;
    private String whatsappNumber;
    private Boolean allowWhatsappMessages;

    // Identification
    private String panNumber;
    private String voterId;
    private String drivingLicense;
    private String passportNumber;
    private String govtIdCard;
    private String nationality;
    private String telemarketerId;

    // Exam Related
    @ManyToOne
    @JoinColumn(name = "branch_id")
    private Branch branch;

    private String examMode;
    private String examBody;

    @ManyToOne
    @JoinColumn(name = "exam_center_id")
    private ExamCenter examCenter;

    @Column(name = "exam_language_id")
    private Integer examLanguage;

    // Document Storage
    @Lob
    private byte[] photo;
    
    @Lob
    private byte[] signature;

    // Metadata
    private String corType;
    private String insuranceCategory;
    private LocalDateTime sponsorshipDate;
    private String uploadRemark;
    private String status;
    private LocalDateTime createdAt;
    private LocalDateTime updatedAt;
    private String createdBy;
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