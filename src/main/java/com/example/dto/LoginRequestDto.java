package com.example.dto;

public class LoginRequestDto {
    private String userId;
    private String password;

    public LoginRequestDto() {}

    public String getUserId() { return userId; }
    public void setUserId(String userId) { this.userId = userId; }
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
} 