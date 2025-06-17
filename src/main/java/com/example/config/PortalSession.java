package com.example.config;

import lombok.Data;
import java.time.LocalDateTime;

@Data
public class PortalSession {
    private Long userId;
    private String userRole;
    private String userName;
    private String userEmail;
    private String userStatus;
    private String userLastLoginDate;
    private String userPasswordChangedDate;
    private String userResetToken;
    private String userResetTokenExpiry;
    private String userCreatedAt;
    private String userUpdatedAt;
    private String userCreatedBy;
    private String userUpdatedBy;
    private LocalDateTime lastAccessTime;
    private boolean valid;
    
    public void clear() {
        userId = null;
        userRole = null;
        userName = null;
        userEmail = null;
        userStatus = null;
        userLastLoginDate = null;
        userPasswordChangedDate = null;
        userResetToken = null;
        userResetTokenExpiry = null;
        userCreatedAt = null;
        userUpdatedAt = null;
        userCreatedBy = null;
        userUpdatedBy = null;
        lastAccessTime = null;
        valid = false;
    }
    
    public boolean isValid() {
        return valid && lastAccessTime != null && 
               lastAccessTime.plusHours(24).isAfter(LocalDateTime.now());
    }
} 