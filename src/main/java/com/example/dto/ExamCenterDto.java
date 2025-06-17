package com.example.dto;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ExamCenterDto {
    private Long id;
    private String centerCode;
    private String name;
    private String address;
    private Integer stateId;
    private String stateName;
    private Integer districtId;
    private String districtName;
    private String pincode;
    private Integer capacity;
    private String contactPerson;
    private String contactPhone;
    private String contactEmail;
    private String status;
    private Double latitude;
    private Double longitude;
    private LocalDateTime createdAt;
    private LocalDateTime updatedAt;
    private String createdBy;
    private String updatedBy;
} 