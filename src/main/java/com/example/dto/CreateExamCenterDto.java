package com.example.dto;

import jakarta.validation.constraints.*;
import lombok.Data;
import java.math.BigDecimal;

@Data
public class CreateExamCenterDto {
    @NotBlank(message = "Center code is required")
    @Pattern(regexp = "^[A-Z0-9]{3,10}$", message = "Center code must be 3-10 characters long and contain only uppercase letters and numbers")
    private String centerCode;

    @NotBlank(message = "Name is required")
    @Size(min = 3, max = 100, message = "Name must be between 3 and 100 characters")
    private String name;

    @NotBlank(message = "Address is required")
    @Size(max = 500, message = "Address must not exceed 500 characters")
    private String address;

    @NotNull(message = "State ID is required")
    private Integer stateId;

    @NotNull(message = "District ID is required")
    private Integer districtId;

    @NotBlank(message = "Pincode is required")
    @Pattern(regexp = "^[0-9]{6}$", message = "Pincode must be 6 digits")
    private String pincode;

    @NotNull(message = "Capacity is required")
    @Min(value = 1, message = "Capacity must be at least 1")
    @Max(value = 1000, message = "Capacity cannot exceed 1000")
    private Integer capacity;

    @NotBlank(message = "Contact person is required")
    @Size(max = 100, message = "Contact person name must not exceed 100 characters")
    private String contactPerson;

    @NotBlank(message = "Contact phone is required")
    @Pattern(regexp = "^[0-9]{10}$", message = "Phone number must be 10 digits")
    private String contactPhone;

    @Email(message = "Invalid email format")
    @Size(max = 100, message = "Email must not exceed 100 characters")
    private String contactEmail;

    @NotNull(message = "Latitude is required")
    @DecimalMin(value = "-90.0", message = "Latitude must be between -90 and 90")
    @DecimalMax(value = "90.0", message = "Latitude must be between -90 and 90")
    private Double latitude;

    @NotNull(message = "Longitude is required")
    @DecimalMin(value = "-180.0", message = "Longitude must be between -180 and 180")
    @DecimalMax(value = "180.0", message = "Longitude must be between -180 and 180")
    private Double longitude;
} 