package com.example.service;

import com.example.dto.LoginRequestDto;
import com.example.dto.ChangePasswordDto;
import com.example.dto.ResetPasswordDto;
import java.util.Map;

public interface UserService {
    
    // Authentication methods
    Map<String, Object> login(LoginRequestDto loginRequest);
    void logout();
    
    // Password management
    void changePassword(ChangePasswordDto changePasswordDto);
    void resetPassword(ResetPasswordDto resetPasswordDto);
    
    // Profile management
    Map<String, Object> myProfile();
    Map<String, Object> getProfile();
    void saveProfile(Map<String, Object> profileData);
    void updateProfile2(Map<String, Object> profileData);
    
    // User management
    Map<String, Object> getUserById(Long userId);
    Map<String, Object> getUserByEmail(String email);
    void updateUserStatus(Long userId, String status);
    void updateUserRole(Long userId, String role);
    
    // Session management
    boolean validateSession(String sessionId);
    void invalidateSession(String sessionId);
    
    // Security
    boolean hasPermission(String permission);
    boolean hasRole(String role);
}