package com.example.model;

import lombok.Data;
import java.util.List;
import java.util.Map;

@Data
public class ScheduleResponse {
    private boolean success;
    private String message;
    private String status;
    private List<Map<String, Object>> data;
    private String cssReferenceNumber;
    private String clientSideIdentifier;
    
    public ScheduleResponse(boolean success, String message) {
        this.success = success;
        this.message = message;
    }
    
    public ScheduleResponse(boolean success, String message, List<Map<String, Object>> data) {
        this.success = success;
        this.message = message;
        this.data = data;
    }
} 