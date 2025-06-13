package com.example.service.impl;

import com.example.service.UtilityService;
import org.springframework.stereotype.Service;
import java.util.Map;

@Service
public class UtilityServiceImpl implements UtilityService {
    @Override
    public Map<String, Object> saveCandidateProfile(Map<String, Object> profileData) {
        return Map.of("success", true, "message", "Stub: saveCandidateProfile");
    }

    @Override
    public Map<String, Object> loadCompanyDetails(Map<String, Object> requestData) {
        return Map.of("success", true, "message", "Stub: loadCompanyDetails");
    }

    @Override
    public Map<String, Object> getUserPassword(Map<String, Object> requestData) {
        return Map.of("success", true, "message", "Stub: getUserPassword");
    }
} 