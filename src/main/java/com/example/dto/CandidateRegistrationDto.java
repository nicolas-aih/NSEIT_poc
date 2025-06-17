package com.example.dto;

import java.time.LocalDate;

public class CandidateRegistrationDto {
    private String name;
    private LocalDate dob;
    private String email;
    private String mobile;
    private String address;
    private String pan;
    private String gender;
    private Integer stateId;
    private Integer centerId;
    private String qualification;
    private Integer yearOfPassing;

    public CandidateRegistrationDto() {}

    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public LocalDate getDob() { return dob; }
    public void setDob(LocalDate dob) { this.dob = dob; }
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    public String getMobile() { return mobile; }
    public void setMobile(String mobile) { this.mobile = mobile; }
    public String getAddress() { return address; }
    public void setAddress(String address) { this.address = address; }
    public String getPan() { return pan; }
    public void setPan(String pan) { this.pan = pan; }
    public String getGender() { return gender; }
    public void setGender(String gender) { this.gender = gender; }
    public Integer getStateId() { return stateId; }
    public void setStateId(Integer stateId) { this.stateId = stateId; }
    public Integer getCenterId() { return centerId; }
    public void setCenterId(Integer centerId) { this.centerId = centerId; }
    public String getQualification() { return qualification; }
    public void setQualification(String qualification) { this.qualification = qualification; }
    public Integer getYearOfPassing() { return yearOfPassing; }
    public void setYearOfPassing(Integer yearOfPassing) { this.yearOfPassing = yearOfPassing; }
} 