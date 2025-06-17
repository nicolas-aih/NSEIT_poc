package com.example.dto;

import jakarta.validation.constraints.*;
import lombok.Data;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
public class ExamRegistrationDto {
    @NotNull(message = "Exam ID is required")
    private Long examId;

    @NotNull(message = "Exam center ID is required")
    private Long examCenterId;

    @NotNull(message = "Exam date is required")
    private LocalDateTime examDate;

    @NotBlank(message = "Time slot is required")
    private String timeSlot;

    // Document upload fields
    private String photoUrl;
    private String signatureUrl;
    private String idProofUrl;
    private String qualificationUrl;
    private String otherDocumentsUrl;

    // Payment information
    @NotNull(message = "Fee amount is required")
    @DecimalMin(value = "0.0", message = "Fee amount must be greater than or equal to 0")
    private BigDecimal fee;

    // Additional information
    @Size(max = 500)
    private String specialRequirements;

    @AssertTrue(message = "You must agree to the terms and conditions")
    private Boolean termsAccepted;

    // Optional fields for additional information
    private String previousExamDetails;
    private String category;
    private String subCategory;
    private String reservationCategory;

    private String status;
    private LocalDateTime registrationDate;
    private String registrationNumber;
    private String candidateName;
    private String candidateEmail;
    private String candidatePhone;
    private String candidateAddress;
    private String candidateCity;
    private String candidateState;
    private String candidatePincode;
    private String paymentStatus;
    private String paymentReference;
    private LocalDateTime paymentDate;
    private String createdBy;
    private String updatedBy;
} 