package com.example.dto;

public class ResetPasswordDto {
    private String userId;
    private String emailId;

    public ResetPasswordDto() {}

    public String getUserId() { return userId; }
    public void setUserId(String userId) { this.userId = userId; }
    public String getEmailId() { return emailId; }
    public void setEmailId(String emailId) { this.emailId = emailId; }
} 