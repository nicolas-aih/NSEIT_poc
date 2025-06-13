package com.example.service;

import com.example.model.Notification;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.ResultSet;
import java.util.List;

@Service
public class HomeService {
    
    private final JdbcTemplate jdbcTemplate;

    @Value("${portal.connection-string}")
    private String connectionString;

    public HomeService(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public List<Notification> getNotifications(String type, String roleCode) {
        String sql = "SELECT * FROM Notifications WHERE NotificationType = ? AND RoleCode = ? AND IsActive = 1";
        
        return jdbcTemplate.query(sql, 
            new Object[]{type, roleCode},
            (ResultSet rs, int rowNum) -> {
                Notification notification = new Notification();
                notification.setId(rs.getLong("NotificationId"));
                notification.setTitle(rs.getString("Title"));
                notification.setDescription(rs.getString("Description"));
                notification.setValidFrom(rs.getTimestamp("ValidFrom").toLocalDateTime());
                notification.setValidTo(rs.getTimestamp("ValidTo").toLocalDateTime());
                notification.setNotificationType(rs.getString("NotificationType"));
                return notification;
            }
        );
    }

    public String getTicker() {
        String sql = "SELECT TOP 1 TickerText FROM Ticker WHERE IsActive = 1 ORDER BY CreatedDate DESC";
        
        return jdbcTemplate.queryForObject(sql, String.class);
    }
} 