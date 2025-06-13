package com.example.service;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@Service
public class NotificationService {
    
    private final JdbcTemplate jdbcTemplate;

    @Value("${portal.connection-string}")
    private String connectionString;

    public NotificationService(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public String saveNotification(
            String title,
            String description,
            LocalDateTime validFrom,
            LocalDateTime validTo,
            String notificationType,
            Integer userId) {
        
        try {
            String sql = "EXEC sp_SaveNotification @Title = ?, @Description = ?, @ValidFrom = ?, " +
                        "@ValidTo = ?, @NotificationType = ?, @UserId = ?";
            
            jdbcTemplate.update(sql, 
                title,
                description,
                validFrom,
                validTo,
                notificationType,
                userId
            );
            
            return ""; // Empty string means success
        } catch (Exception e) {
            return "Error saving notification: " + e.getMessage();
        }
    }
} 