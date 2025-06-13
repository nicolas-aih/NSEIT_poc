package com.example.dto;

public class TelemarketerDto {
    private Long tmId;
    private String traiRegNo;
    private String name;
    private String address;
    private String cpName;
    private String cpEmailId;
    private String cpContactNo;
    private String dpName;
    private String dpEmailId;
    private String dpContactNo;
    private String isActive;

    public TelemarketerDto() {}

    public Long getTmId() { return tmId; }
    public void setTmId(Long tmId) { this.tmId = tmId; }
    public String getTraiRegNo() { return traiRegNo; }
    public void setTraiRegNo(String traiRegNo) { this.traiRegNo = traiRegNo; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getAddress() { return address; }
    public void setAddress(String address) { this.address = address; }
    public String getCpName() { return cpName; }
    public void setCpName(String cpName) { this.cpName = cpName; }
    public String getCpEmailId() { return cpEmailId; }
    public void setCpEmailId(String cpEmailId) { this.cpEmailId = cpEmailId; }
    public String getCpContactNo() { return cpContactNo; }
    public void setCpContactNo(String cpContactNo) { this.cpContactNo = cpContactNo; }
    public String getDpName() { return dpName; }
    public void setDpName(String dpName) { this.dpName = dpName; }
    public String getDpEmailId() { return dpEmailId; }
    public void setDpEmailId(String dpEmailId) { this.dpEmailId = dpEmailId; }
    public String getDpContactNo() { return dpContactNo; }
    public void setDpContactNo(String dpContactNo) { this.dpContactNo = dpContactNo; }
    public String getIsActive() { return isActive; }
    public void setIsActive(String isActive) { this.isActive = isActive; }
} 