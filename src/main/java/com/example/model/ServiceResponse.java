package com.example.model;

import lombok.Data;
import java.util.List;
import java.util.Map;

@Data
public class ServiceResponse {
    private boolean success;
    private String message;
    private List<Map<String, Object>> data;
    private Map<String, String> additionalInfo;

    public ServiceResponse(boolean success, String message) {
        this.success = success;
        this.message = message;
    }

    public ServiceResponse(boolean success, String message, List<Map<String, Object>> data) {
        this.success = success;
        this.message = message;
        this.data = data;
    }

    public ServiceResponse(boolean success, String message, Map<String, String> additionalInfo) {
        this.success = success;
        this.message = message;
        this.additionalInfo = additionalInfo;
    }
} 