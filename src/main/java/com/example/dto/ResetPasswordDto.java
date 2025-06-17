package com.example.dto;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import lombok.Data;

@Data
public class ResetPasswordDto {
    @NotBlank(message = "Email is required")
    @Email(message = "Invalid email format")
    private String email;
    
    @NotBlank(message = "Token is required")
    private String token;
    
    @NotBlank(message = "New password is required")
    private String newPassword;
    
    @NotBlank(message = "Confirm password is required")
    private String confirmPassword;
} 