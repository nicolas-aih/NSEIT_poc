package com.example.dto;

import jakarta.validation.constraints.*;
import lombok.Data;
import java.math.BigDecimal;

@Data
public class UpdateExamCenterDto {
    @Size(min = 3, max = 100, message = "Name must be between 3 and 100 characters")
    private String name;

    @Size(min = 10, max = 500, message = "Address must be between 10 and 500 characters")
    private String address;

    @NotNull(message = "State ID is required")
    private Integer stateId;

    @Size(min = 2, max = 50, message = "State name must be between 2 and 50 characters")
    private String stateName;

    @NotNull(message = "District ID is required")
    private Integer districtId;

    @Size(min = 2, max = 50, message = "District name must be between 2 and 50 characters")
    private String districtName;

    @Pattern(regexp = "^[0-9]{6}$", message = "Pincode must be 6 digits")
    private String pincode;

    @Min(value = 1, message = "Minimum capacity must be at least 1")
    @Max(value = 1000, message = "Maximum capacity cannot exceed 1000")
    private Integer maxCapacity;

    @Size(min = 3, max = 100, message = "Contact person name must be between 3 and 100 characters")
    private String contactPerson;

    @Pattern(regexp = "^[0-9]{10}$", message = "Phone number must be 10 digits")
    private String contactPhone;

    @Email(message = "Invalid email format")
    @Size(max = 100, message = "Email must not exceed 100 characters")
    private String contactEmail;

    @DecimalMin(value = "0.0", inclusive = true, message = "Registration fee must be non-negative")
    @DecimalMax(value = "10000.0", inclusive = true, message = "Registration fee cannot exceed 10000")
    private BigDecimal registrationFee;

    @DecimalMin(value = "0.0", inclusive = true, message = "Exam fee must be non-negative")
    @DecimalMax(value = "10000.0", inclusive = true, message = "Exam fee cannot exceed 10000")
    private BigDecimal examFee;

    @Pattern(regexp = "^(ACTIVE|INACTIVE)$", message = "Status must be either ACTIVE or INACTIVE")
    private String status;
} 