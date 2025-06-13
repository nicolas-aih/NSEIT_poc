package com.example.model;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class ExamCenter {
    private Integer centerId;
    private String centerName;
    private String centerCode;
    private String addressLine1;
    private String addressLine2;
    private String city;
    private Integer stateId;
    private String state;
    private String pinCode;
    private String contactPerson;
    private String contactNumber;
    private String email;
    private Integer capacity;
    private Boolean isActive;
    private String createdBy;
    private LocalDateTime createdDate;
    private String modifiedBy;
    private LocalDateTime modifiedDate;
    private Double latitude;
    private Double longitude;
} 